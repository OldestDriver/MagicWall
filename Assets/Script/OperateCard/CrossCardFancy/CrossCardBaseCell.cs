﻿using UnityEngine;


namespace MagicWall
{
    public abstract class CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext> : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the index of the data.
        /// </summary>
        /// <value>The index of the data.</value>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:FancyScrollView.FancyScrollViewCell`2"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public virtual bool IsVisible => gameObject.activeSelf;

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected CrossCardScrollViewContext Context { get; private set; }

        /// <summary>
        /// Setup the context.
        /// </summary>
        /// <param name="context">Context.</param>
        public virtual void SetupContext(CrossCardScrollViewContext context) => Context = context;

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">If set to <c>true</c> visible.</param>
        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="itemData">Item data.</param>
        public abstract void UpdateContent(CrossCardCellData itemData);

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public abstract void UpdatePosition(float position);

        public abstract void UpdateComponentStatus();

        public abstract void ClearComponentStatus();

        public abstract string GetCurrentDescription();


        public abstract void InitData();


        public abstract void GetInfo();

    }
}