﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(ScrollRect), typeof(CanvasGroup))]
public class ScrollSnap : UIBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] public int startingIndex = 0;
    [SerializeField] public bool wrapAround = false;
    [SerializeField] public float lerpTimeMilliSeconds = 200f;
    [SerializeField] public float triggerPercent = 5f;
    [Range(0f, 10f)] public float triggerAcceleration = 1f;

    public class OnLerpCompleteEvent : UnityEvent { }
    public OnLerpCompleteEvent onLerpComplete;
    public class OnReleaseEvent : UnityEvent<int> { }
    public OnReleaseEvent onRelease;

    int actualIndex;
    int cellIndex;
    [SerializeField] ScrollRect scrollRect;
    CanvasGroup canvasGroup;
    internal RectTransform content;
    Vector2 cellSize;
    bool indexChangeTriggered = false;
    bool isLerping = false;
    float lerpStartedAt;
    Vector2 releasedPosition;
    Vector2 targetPosition;

    public void Awake()
    {
        //base.Awake();
        actualIndex = startingIndex;
        cellIndex = startingIndex;
        this.onLerpComplete = new OnLerpCompleteEvent();
        this.onRelease = new OnReleaseEvent();
        this.scrollRect = GetComponent<ScrollRect>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.content = scrollRect.content;
        this.cellSize = content.GetComponent<GridLayoutGroup>().cellSize;
        content.anchoredPosition = new Vector2(-cellSize.x * cellIndex, content.anchoredPosition.y);
        int count = LayoutElementCount();
        SetContentSize(count);

        if (startingIndex < count)
        {
            MoveToIndex(startingIndex);
        }

    }

    void LateUpdate()
    {
        if (isLerping)
        {
            LerpToElement();
            if (ShouldStopLerping())
            {
                isLerping = false;
                canvasGroup.blocksRaycasts = true;
                onLerpComplete.Invoke();
                onLerpComplete.RemoveListener(WrapElementAround);

            }
        }
    }

    #region Push And Pop
    public void PushLayoutElement(LayoutElement element)
    {
        Logger.Print($" Snep >>  element = {element.name}");
        //Logger.Print($" Snep >>  element = content || {content == null}");
        element.transform.SetParent(content.transform, false);
        //Logger.Print($" Snep >>  child count = {content.transform.childCount}");
        SetContentSize(LayoutElementCount());
    }

    public void PopLayoutElement()
    {
        LayoutElement[] elements = content.GetComponentsInChildren<LayoutElement>();
        Destroy(elements[elements.Length - 1].gameObject);
        SetContentSize(LayoutElementCount() - 1);
        if (cellIndex == CalculateMaxIndex())
        {
            cellIndex -= 1;
        }
    }
    #endregion

    public void UnshiftLayoutElement(LayoutElement element)
    {
        cellIndex += 1;
        element.transform.SetParent(content.transform, false);
        element.transform.SetAsFirstSibling();
        SetContentSize(LayoutElementCount());
        content.anchoredPosition = new Vector2(content.anchoredPosition.x - cellSize.x, content.anchoredPosition.y);
    }

    public void ShiftLayoutElement()
    {
        Destroy(GetComponentInChildren<LayoutElement>().gameObject);
        SetContentSize(LayoutElementCount() - 1);
        cellIndex -= 1;
        content.anchoredPosition = new Vector2(content.anchoredPosition.x + cellSize.x, content.anchoredPosition.y);
    }

    public int LayoutElementCount()
    {
        return content.GetComponentsInChildren<LayoutElement>(false)
            .Count(e => e.transform.parent == content);
    }

    public int CurrentIndex
    {
        get
        {
            int count = LayoutElementCount();
            int mod = actualIndex % count;
            return mod >= 0 ? mod : count + mod;
        }
    }

    #region Drag Handle
    public void OnDrag(PointerEventData data)
    {
        float dx = data.delta.x;
        float dt = Time.deltaTime * 1000f;
        float acceleration = Mathf.Abs(dx / dt);
        if (acceleration > triggerAcceleration && !float.IsPositiveInfinity(acceleration))
        {
            indexChangeTriggered = true;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (IndexShouldChangeFromDrag(data))
        {
            int direction = (data.pressPosition.x - data.position.x) > 0f ? 1 : -1;
            SnapToIndex(cellIndex + direction * CalculateScrollingAmount(data));
        }
        else
        {
            StartLerping();
        }
    }
    #endregion

    public int CalculateScrollingAmount(PointerEventData data)
    {
        var offset = scrollRect.content.anchoredPosition.x + cellIndex * cellSize.x;
        var normalizedOffset = Mathf.Abs(offset / cellSize.x);
        var skipping = (int)Mathf.Floor(normalizedOffset);
        if (skipping == 0)
            return 1;
        if ((normalizedOffset - skipping) * 100f > triggerPercent)
        {
            return skipping + 1;
        }
        else
        {
            return skipping;
        }
    }

    public void SnapToNext()
    {
        SnapToIndex(cellIndex + 1);
    }

    public void SnapToPrev()
    {
        SnapToIndex(cellIndex - 1);
    }

    public void SnapToIndex(int newCellIndex)
    {
        int maxIndex = CalculateMaxIndex();
        if (wrapAround && maxIndex > 0)
        {
            actualIndex += newCellIndex - cellIndex;
            cellIndex = newCellIndex;
            onLerpComplete.AddListener(WrapElementAround);
        }
        else
        {
            newCellIndex = Mathf.Clamp(newCellIndex, 0, maxIndex);
            actualIndex += newCellIndex - cellIndex;
            cellIndex = newCellIndex;
            Debug.Log($"newCellIndex = {newCellIndex} | cellIndex = {cellIndex} | CalculateMaxIndex()= {CalculateMaxIndex()} | CurrentIndex = {CurrentIndex}");

            Logger.Print($"scrollSnap.CurrentIndex { CurrentIndex } || Max { CalculateMaxIndex()} ");
            foreach (var item in DashboardManager.instance.pagination)
            {
                item.sprite = DashboardManager.instance.deactive_pagination;
            }
            DashboardManager.instance.pagination[CurrentIndex].sprite = DashboardManager.instance.active_pagination;
            DashboardManager.instance.leftBtn.sprite = CurrentIndex == 0 ? DashboardManager.instance.de_spr1 : DashboardManager.instance.se_spr1;
            DashboardManager.instance.rightBtn.sprite = CurrentIndex == CalculateMaxIndex() ? DashboardManager.instance.de_spr2 : DashboardManager.instance.se_spr2;
        }
        onRelease.Invoke(cellIndex);
        StartLerping();
    }

    public void MoveToIndex(int newCellIndex)
    {
        int maxIndex = CalculateMaxIndex();
        Debug.Log($"MoveToIndex | maxIndex = {maxIndex} | newCellIndex = {newCellIndex} | actualIndex = {actualIndex}");
        if (newCellIndex >= 0 && newCellIndex <= maxIndex)
        {
            actualIndex += newCellIndex - cellIndex;
            cellIndex = newCellIndex;
        }
        onRelease.Invoke(cellIndex);
        content.anchoredPosition = CalculateTargetPoisition(cellIndex);
    }

    void StartLerping()
    {
        releasedPosition = content.anchoredPosition;
        targetPosition = CalculateTargetPoisition(cellIndex);
        lerpStartedAt = Time.time;
        canvasGroup.blocksRaycasts = false;
        isLerping = true;
    }

    public void HandleNextandPrivious(int click)
    {
        switch (click)
        {
            case 1: // Next
                //posGet -= 1000;
                //ScoreBoardContent.transform.DOMoveX(posGet, 0.3f);
                break;

            case 2:// previous
                //posGet += 1000;
                //ScoreBoardContent.transform.DOMoveX(posGet, 0.3f);

                break;
        }
    }


    public int CalculateMaxIndex()
    {
        int cellPerFrame = Mathf.FloorToInt(scrollRect.GetComponent<RectTransform>().rect.size.x / cellSize.x);
        return LayoutElementCount() - cellPerFrame;
    }

    bool IndexShouldChangeFromDrag(PointerEventData data)
    {
        // acceleration was above threshold
        if (indexChangeTriggered)
        {
            indexChangeTriggered = false;
            return true;
        }
        // dragged beyond trigger threshold
        var offset = scrollRect.content.anchoredPosition.x + cellIndex * cellSize.x;
        var normalizedOffset = Mathf.Abs(offset / cellSize.x);
        return normalizedOffset * 100f > triggerPercent;
    }

    void LerpToElement()
    {
        float t = (Time.time - lerpStartedAt) * 1000f / lerpTimeMilliSeconds;
        float newX = Mathf.Lerp(releasedPosition.x, targetPosition.x, t);
        content.anchoredPosition = new Vector2(newX, content.anchoredPosition.y);
    }

    void WrapElementAround()
    {
        if (cellIndex <= 0)
        {
            var elements = content.GetComponentsInChildren<LayoutElement>();
            elements[elements.Length - 1].transform.SetAsFirstSibling();
            cellIndex += 1;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x - cellSize.x, content.anchoredPosition.y);
        }
        else if (cellIndex >= CalculateMaxIndex())
        {
            var element = content.GetComponentInChildren<LayoutElement>();
            element.transform.SetAsLastSibling();
            cellIndex -= 1;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x + cellSize.x, content.anchoredPosition.y);
        }
    }

    void SetContentSize(int elementCount)
    {
        content.sizeDelta = new Vector2(cellSize.x * elementCount, content.rect.height);
    }

    Vector2 CalculateTargetPoisition(int index)
    {
        return new Vector2(-cellSize.x * index, content.anchoredPosition.y);
    }

    bool ShouldStopLerping()
    {
        return Mathf.Abs(content.anchoredPosition.x - targetPosition.x) < 0.001f;
    }
}
