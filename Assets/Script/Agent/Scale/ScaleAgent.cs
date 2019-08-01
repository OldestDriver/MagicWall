﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ScaleAgent : MonoBehaviour
{
    Texture _imageTexture;

    [SerializeField] RawImage image;
    [SerializeField] RectTransform tool_box;

    public float maxScale = 2.0f;//最大倍数
    public int plusCount = 5;//放大次数


    float MAX_WIDTH = 660;
    float MAX_HEIGHT = 950;

    Action OnCloseClicked;
    Action _onReturnClicked;
    Action OnUpdate;
    Action _onOpen; //打开时


    private RectTransform imgRtf;
    public float currentScale;//当前缩放倍数
    private float perScale;//每次放大倍数
    private Vector2 originalSize;



    // Start is called before the first frame update
    void Start()
    {
        _onOpen.Invoke();
    }

    void FixedUpdate() {

        // 当缩放窗口打开时，保证卡片不被关闭
        //OnUpdate.Invoke();

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print(imgRtf.localPosition);

    }

    public void SetImage(Texture texture)
    {

        imgRtf = image.GetComponent<RectTransform>();
        currentScale = 1;
        perScale = (maxScale - 1) / plusCount;

        // 需要防止变形
        _imageTexture = texture;

        SizeToScale();

        image.texture = texture;

    }

    public void SetOnCloseClicked(Action action) {
        OnCloseClicked = action;
    }

    public void SetOnReturnClicked(Action action)
    {
        _onReturnClicked = action;
    }

    public void SetOnUpdated(Action action)
    {
        OnUpdate = action;
    }

    public void SetOnOpen(Action action)
    {
        _onOpen = action;
    }


    public void DoReturn() {
        _onReturnClicked?.Invoke();
    }

    public void DoClose()
    {
        Debug.Log("关闭缩放窗口");

        OnCloseClicked?.Invoke();
    }

    // 点击放大按钮
    public void DoPlus()
    {
        //Debug.Log("放大图片操作");
        if (currentScale < maxScale)
        {
            currentScale += perScale;
            if (currentScale > maxScale)
            {
                currentScale = maxScale;
            }
            ResetImage();
        }
        
    }

    // 点击减少按钮
    public void DoMinus()
    {
        //Debug.Log("缩小图片操作");
        if (currentScale > 1.0f)
        {
            currentScale -= perScale;
            if (currentScale < 1)
            {
                currentScale = 1;
            }
            ResetImage();
            imgRtf.anchoredPosition = Vector2.zero;
        }
    }

    public void ResetImage()
    {
        imgRtf.sizeDelta = new Vector2(originalSize.x * currentScale, originalSize.y * currentScale);
        
    }

    private void SizeToScale()
    {
        // 将图片大小定在指定大小
        var parent = image.transform.parent.GetComponent<RectTransform>();


        float w = _imageTexture.width, h = _imageTexture.height;

        string str = "";

        if (_imageTexture.width > MAX_WIDTH)
        {
            float radio = _imageTexture.width / _imageTexture.height;
            w = MAX_WIDTH;
            h = w / radio;

            str = "WIDTH > MAX WIDTH";
        }
        else
        {
            str = "WIDTH < MAX WIDTH";
        }

        //设置图片原始大小
        originalSize = new Vector2(w, h);

        // 将mask调整至新的大小
        //parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        //parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        //parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        //parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        //imgRtf.sizeDelta = originalSize;
        //parent.sizeDelta = originalSize;
        //parent.parent.GetComponent<RectTransform>().sizeDelta = originalSize;

        imgRtf.sizeDelta = originalSize;
        GetComponent<RectTransform>().sizeDelta = originalSize;


    }


    


 


}
