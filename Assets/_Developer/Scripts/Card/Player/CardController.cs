using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    public string cardValue;

    public Image cardImage;

    public Image glowImg;

    public Image disableImg;

    public RectTransform myRect;

    private Canvas gameCanvas;

    public bool isThrowAble;

    public Vector2 startPos;

    public bool isGlowScale;

    public bool isMove;

    public CanvasGroup myCanvasGroup;

    public LayerMask swipeLayerMask;

    public RectTransform dummyRectObj;

    public RectTransform flipModeTag;

    public float posY;

    [SerializeField] float endDrageValue = 60f;

    Tween tween;

    public bool isClicktoThrow = false;

    [SerializeField] bool isMoveAblePos = true;

    private void Start()
    {
        gameCanvas = CardDeckController.instance.gameCanvas;
        if (isMoveAblePos)
        {
            Logger.Print($" MY CARD Start Psoition is {transform.localPosition.y}");
            isMoveAblePos = false;
        }
    }

    void TutorialClickDown()
    {
        if (AppData.isTutorialPlay && GameManager.instance.MyTurn)
        {
            GameManager.instance.MyTurn = false;
            myCanvasGroup.blocksRaycasts = false;
            isGlowScale = false;
            isMove = false;
            TutorialManager.Instance.MyCardThrowAnimation(this, GameManager.instance.BottomThrowCard.transform);
            return;
        }

    }

    #region ======================>> Card Drag System <<=======================

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CardPositionSet.instance.isAnimationStart) return;
        Logger.Print($"OnPointerDown | isTutorialPlay | Name : {this.gameObject.name} | MyTurn = {GameManager.instance.MyTurn} |AppData.isTutorialPlay {AppData.isTutorialPlay}");

        TutorialClickDown();

        if (!GameManager.instance.ControlTouch()) return;

        if (!isMove)
        {
            //myCanvasGroup.blocksRaycasts = false;
            startPos = dummyRectObj.transform.localPosition;
            isGlowScale = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AppData.isTutorialPlay) return;
        if (CardPositionSet.instance.isAnimationStart) return;
        if (!GameManager.instance.ControlTouch()) return;

        if (isThrowAble)
        {
            myCanvasGroup.blocksRaycasts = false;
            isGlowScale = false;
            Logger.Print($"OnPointerClick | IsValidMove | Name : {this.gameObject.name} | {cardValue}");
            CardDeckController.instance.TrappedCardSpawn(this, true);

            isClicktoThrow = true;
            GameManager.instance.CardSelectEventSend(cardValue, this); // SEND EVENT 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameManager.instance.ControlTouch()) return;
        myCanvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CardPositionSet.instance.isAnimationStart || AppData.isTutorialPlay) return;

        if (GameManager.instance.ControlTouch() && !isMove && isThrowAble)
        {
            Debug.Log($"******** OnBeginDrag Value = {cardValue}");
            myCanvasGroup.blocksRaycasts = false;
            isGlowScale = false;
            startPos = transform.localPosition;

            CardDeckController.instance.TrappedCardSpawn(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CardPositionSet.instance.isAnimationStart && AppData.isTutorialPlay) return;
        if (!GameManager.instance.ControlTouch()) return;
        if (!isThrowAble || !isMove) return;

        // Calculate new position based on drag
        Vector2 delta = eventData.delta / gameCanvas.scaleFactor;
        Vector2 newPosition = myRect.anchoredPosition + delta;

        RectTransform canvasRect = gameCanvas.GetComponent<RectTransform>();
        RectTransform elementRect = myRect;

        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 elementSize = elementRect.sizeDelta;

        float halfElementWidth = elementSize.x * 0.5f;
        float halfElementHeight = elementSize.y * 0.5f;

        float minX = -canvasSize.x * 0.5f + halfElementWidth;
        float maxX = canvasSize.x * 0.5f - halfElementWidth;
        float minY = -canvasSize.y * 0.5f + halfElementHeight;
        float maxY = canvasSize.y * 0.5f - halfElementHeight;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        elementRect.anchoredPosition = newPosition;

        if (eventData.pointerCurrentRaycast.gameObject == null) return;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (AppData.isTutorialPlay) return;
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            CardDeckController.instance.TrappedCardDestroy(this);
            return;
        }

        if (!GameManager.instance.ControlTouch()) return;

        if (isMove)
        {
            if (transform.position.y >= -2f && isThrowAble) // {-2.92f >= -2f}
            {
                //TODO: Throw When you get OpenDack Card Pos
                isMove = false;
                GameManager.instance.CardClickListner(this);
            }
            else
            {
                Logger.Print($"OnEndDrag | IsValidMove | Name : {this.gameObject.name}");
                CardDeckController.instance.TrappedCardDestroy(this);
            }
        }
    }

    #endregion


    #region =======================>> Card Glow Method <<===============================
    public void SetCardGlowAnimation(bool status)
    {
        Logger.NormalLog($"==>>> SetCardGlowAnimation >> {status}");
        if (GameManager.instance.ControlTouch() && status) // work when it's my turn.
            StartCoroutine(GlowCorutine(status));
        else
            StartCoroutine(GlowCorutine(status));
    }

    private IEnumerator GlowCorutine(bool status)
    {
        yield return new WaitForSeconds(status ? 0.2f : 0);

        isThrowAble = status;
        glowImg.gameObject.SetActive(status);

        tween?.Kill();

        if (status)
        {
            //posY = transform.localPosition.y + 70f;
            //this.myRect.transform.DOLocalMoveY(posY, 0.3f);
            isGlowScale = true;
            tween = glowImg.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            cardImage.enabled = true;          
            posY = 0;
            //disableImg.gameObject.SetActive(false);
            if (!Mathf.Approximately(glowImg.color.a, 0)) // Check if alpha is not approximately 0
                tween = glowImg.DOFade(0, 0);
            isGlowScale = false;
        }
    }

    #endregion
}

