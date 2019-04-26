﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] RectTransform viewport;
    [SerializeField] ScrollDirection directionOfRecognize = ScrollDirection.Vertical;
    [SerializeField] MovementType movementType = MovementType.Elastic;
    [SerializeField] float elasticity = 0.1f;
    [SerializeField] float scrollSensitivity = 1f;
    [SerializeField] bool inertia = true;

    [SerializeField, Tooltip("Only used when inertia is enabled")]
    float decelerationRate = 0.03f;

    [SerializeField, Tooltip("Only used when inertia is enabled")]
    Snap snap = new Snap {Enable = true, VelocityThreshold = 0.5f, Duration = 0.3f};

    [SerializeField] int dataCount;

    readonly AutoScrollState autoScrollState = new AutoScrollState();

    Action<float> onUpdatePosition;
    Action<int> onSelectedIndexChanged;

    Vector2 pointerStartLocalPosition;
    float dragStartScrollPosition;
    float prevScrollPosition;
    float currentScrollPosition;

    bool dragging;
    float velocity;

    enum ScrollDirection
    {
        Vertical,
        Horizontal,
    }

    enum MovementType
    {
        Unrestricted = ScrollRect.MovementType.Unrestricted,
        Elastic = ScrollRect.MovementType.Elastic,
        Clamped = ScrollRect.MovementType.Clamped
    }

    [Serializable]
    struct Snap
    {
        public bool Enable;
        public float VelocityThreshold;
        public float Duration;
    }

    class AutoScrollState
    {
        public bool Enable;
        public bool Elastic;
        public float Duration;
        public float StartTime;
        public float EndScrollPosition;

        public void Reset()
        {
            Enable = false;
            Elastic = false;
            Duration = 0f;
            StartTime = 0f;
            EndScrollPosition = 0f;
        }
    }

    public void OnUpdatePosition(Action<float> callback) => onUpdatePosition = callback;
        
    public void OnSelectedIndexChanged(Action<int> callback) => onSelectedIndexChanged = callback;
        
    public void SetDataCount(int dataCount) => this.dataCount = dataCount;

    public void ScrollTo(int index, float duration)
    {
        autoScrollState.Reset();
        autoScrollState.Enable = true;
        autoScrollState.Duration = duration;
        autoScrollState.StartTime = Time.unscaledTime;
        autoScrollState.EndScrollPosition = CalculateDestinationIndex(index);

        velocity = 0f;
        dragStartScrollPosition = currentScrollPosition;

        UpdateSelectedIndex(Mathf.RoundToInt(GetCircularPosition(autoScrollState.EndScrollPosition, dataCount)));
    }

    public void JumpTo(int index)
    {
        autoScrollState.Reset();

        velocity = 0f;
        dragging = false;

        index = CalculateDestinationIndex(index);

        UpdateSelectedIndex(index);
        UpdatePosition(index);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        pointerStartLocalPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewport,
            eventData.position,
            eventData.pressEventCamera,
            out pointerStartLocalPosition);

        dragStartScrollPosition = currentScrollPosition;
        dragging = true;
        autoScrollState.Reset();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!dragging)
        {
            return;
        }

        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewport,
            eventData.position,
            eventData.pressEventCamera,
            out localCursor))
        {
            return;
        }

        var pointerDelta = localCursor - pointerStartLocalPosition;
        var position = (directionOfRecognize == ScrollDirection.Horizontal ? -pointerDelta.x : pointerDelta.y)
                        / ViewportSize
                        * scrollSensitivity
                        + dragStartScrollPosition;

        var offset = CalculateOffset(position);
        position += offset;

        if (movementType == MovementType.Elastic)
        {
            if (offset != 0f)
            {
                position -= RubberDelta(offset, scrollSensitivity);
            }
        }

        UpdatePosition(position);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        dragging = false;
    }

    float ViewportSize => directionOfRecognize == ScrollDirection.Horizontal
            ? viewport.rect.size.x
            : viewport.rect.size.y;

    float CalculateOffset(float position)
    {
        if (movementType == MovementType.Unrestricted)
        {
            return 0f;
        }

        if (position < 0f)
        {
            return -position;
        }

        if (position > dataCount - 1)
        {
            return dataCount - 1 - position;
        }

        return 0f;
    }

    void UpdatePosition(float position)
    {
        currentScrollPosition = position;
        onUpdatePosition?.Invoke(currentScrollPosition);
    }

    void UpdateSelectedIndex(int index) => onSelectedIndexChanged?.Invoke(index);

    float RubberDelta(float overStretching, float viewSize) =>
        (1 - 1 / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1)) * viewSize * Mathf.Sign(overStretching);

    void Update()
    {
        var deltaTime = Time.unscaledDeltaTime;
        var offset = CalculateOffset(currentScrollPosition);

        if (autoScrollState.Enable)
        {
            var position = 0f;

            if (autoScrollState.Elastic)
            {
                position = Mathf.SmoothDamp(currentScrollPosition, currentScrollPosition + offset, ref velocity,
                    elasticity, Mathf.Infinity, deltaTime);

                if (Mathf.Abs(velocity) < 0.01f)
                {
                    position = Mathf.Clamp(Mathf.RoundToInt(position), 0, dataCount - 1);
                    velocity = 0f;
                    autoScrollState.Reset();
                }
            }
            else
            {
                var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollState.StartTime) /
                                            Mathf.Max(autoScrollState.Duration, float.Epsilon));
                position = Mathf.Lerp(dragStartScrollPosition, autoScrollState.EndScrollPosition,
                    EaseInOutCubic(0, 1, alpha));

                if (Mathf.Approximately(alpha, 1f))
                {
                    autoScrollState.Reset();
                }
            }

            UpdatePosition(position);
        }
        else if (!dragging && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
        {
            var position = currentScrollPosition;

            if (movementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
            {
                autoScrollState.Reset();
                autoScrollState.Enable = true;
                autoScrollState.Elastic = true;

                UpdateSelectedIndex(Mathf.Clamp(Mathf.RoundToInt(position), 0, dataCount - 1));
            }
            else if (inertia)
            {
                velocity *= Mathf.Pow(decelerationRate, deltaTime);

                if (Mathf.Abs(velocity) < 0.001f)
                {
                    velocity = 0f;
                }

                position += velocity * deltaTime;

                if (snap.Enable && Mathf.Abs(velocity) < snap.VelocityThreshold)
                {
                    ScrollTo(Mathf.RoundToInt(currentScrollPosition), snap.Duration);
                }
            }
            else
            {
                velocity = 0f;
            }

            if (!Mathf.Approximately(velocity, 0f))
            {
                if (movementType == MovementType.Clamped)
                {
                    offset = CalculateOffset(position);
                    position += offset;

                    if (Mathf.Approximately(position, 0f) || Mathf.Approximately(position, dataCount - 1f))
                    {
                        velocity = 0f;
                        UpdateSelectedIndex(Mathf.RoundToInt(position));
                    }
                }

                UpdatePosition(position);
            }
        }

        if (!autoScrollState.Enable && dragging && inertia)
        {
            var newVelocity = (currentScrollPosition - prevScrollPosition) / deltaTime;
            velocity = Mathf.Lerp(velocity, newVelocity, deltaTime * 10f);
        }

        prevScrollPosition = currentScrollPosition;
    }

    int CalculateDestinationIndex(int index) => movementType == MovementType.Unrestricted
        ? CalculateClosestIndex(index)
        : Mathf.Clamp(index, 0, dataCount - 1);

    int CalculateClosestIndex(int index)
    {
        var diff = GetCircularPosition(index, dataCount)
                    - GetCircularPosition(currentScrollPosition, dataCount);

        if (Mathf.Abs(diff) > dataCount * 0.5f)
        {
            diff = Mathf.Sign(-diff) * (dataCount - Mathf.Abs(diff));
        }

        return Mathf.RoundToInt(diff + currentScrollPosition);
    }

    float GetCircularPosition(float position, int length) =>
        position < 0 ? length - 1 + (position + 1) % length : position % length;

    float EaseInOutCubic(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;

        if (value < 1f)
        {
            return end * 0.5f * value * value * value + start;
        }

        value -= 2f;
        return end * 0.5f * (value * value * value + 2f) + start;
    }
}

