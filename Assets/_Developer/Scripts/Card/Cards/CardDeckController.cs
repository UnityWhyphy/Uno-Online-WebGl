using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CardSpritesC
{
    public string cardColor;
    public Sprite[] cardSpr;
}

public class CardDeckController : Singleton<CardDeckController>
{
    public CardSpritesC[] cardSpritesNormal;
    public CardSpritesC[] cardSpritesFlipLight;// r,g,b,y,k
    public CardSpritesC[] cardSpritesFlipDark;// o,t,j,p,k

    public Sprite[] highFiveSprites;
    public Sprite[] sevenSwapSpr;
    public Sprite[] zeroSwapSpr;

    [Header(" === Card Deck Spawn === ")]
    public List<PlayerController> playerData = new List<PlayerController>();
    public List<Transform> selfPlayerStaticCard = new List<Transform>();

    [Space(10)]
    public CardController cardPrefab;  // Assign Card prefab 
    public Transform cardSpawnPoint; // Assign Card deck point 
    public int totalCard;

    [Space(10)]
    [Header(" === Card Drag And Drop === ")]
    public Canvas gameCanvas;
    [Header(" === Trapped Cards ===")]
    public CardController trappedCard;
    public CardController selectCard;
    public Transform selectCardParent;

    [Space(10)]
    [SerializeField] GameObject openDeckAnimParent;
    [SerializeField] GameObject openDeckAnim1, openDeckAnim2, openDeckAnim3;
    [SerializeField] Image dummyOpenDeckCard;
    [SerializeField] Transform clCounter, clBg,flipTempCard;

    private Sprite full, half, single;

    private void Awake()
    {
        base.SingletonAwake();
    }

    private void Start()
    {
        full = openDeckAnim1.GetComponent<Image>().sprite;
        half = openDeckAnim2.GetComponent<Image>().sprite;
        single = openDeckAnim3.GetComponent<Image>().sprite;
    }

    private void OnEnable()
    {
        GameManager.resetGame += ResetThisScript;
    }

    private void OnDisable()
    {
        GameManager.resetGame -= ResetThisScript;
    }

    public void SelectCardChildDestroy()
    {
        for (int i = 0; i < selectCardParent.childCount; i++)
        {
            Destroy(selectCardParent.transform.GetChild(i).gameObject);
        }
    }

    public void OtcCardSpriteSet(string strUrl1, string strUrl2, string strUrl3)
    {
        if (strUrl1 == null || strUrl2 == null || strUrl3 == null)
        {
            //set as default
            openDeckAnim1.GetComponent<Image>().sprite = full;
            openDeckAnim2.GetComponent<Image>().sprite = half;
            openDeckAnim3.GetComponent<Image>().sprite = single;
            return;
        }

        StartCoroutine(AppData.SpriteSetFromURL(strUrl1, openDeckAnim1.GetComponent<Image>(), "OtcCardSpriteSet"));
        StartCoroutine(AppData.SpriteSetFromURL(strUrl2, openDeckAnim2.GetComponent<Image>(), "OtcCardSpriteSet"));
        StartCoroutine(AppData.SpriteSetFromURL(strUrl3, openDeckAnim3.GetComponent<Image>(), "OtcCardSpriteSet"));
    }

    float duration = 0.2f;
    Vector3 tempPos;
    public IEnumerator OtcDackAnimation(Transform destination, Image backBanch, string card, Transform moveAbleImg)
    {
        // TODO : Open
        Logger.Print($"OTC backBanch : {openDeckAnimParent.activeInHierarchy}");

        tempPos = openDeckAnimParent.transform.position;
        openDeckAnimParent.gameObject.SetActive(true);
        var pos = backBanch.transform.position;
        //var poscounter = clCounter.transform.position;
        var posBg = clBg.transform.position;

        backBanch.transform.DOMove(moveAbleImg.transform.position, duration + 0.1f).SetEase(Ease.OutSine);
        clCounter.transform.DOMove(moveAbleImg.transform.position, duration + 0.1f).SetEase(Ease.OutSine);
        clBg.transform.DOMove(moveAbleImg.transform.position, duration + 0.1f).SetEase(Ease.OutSine);
        flipTempCard.transform.DOMove(moveAbleImg.transform.position, duration + 0.1f).SetEase(Ease.OutSine);
        //dummyOpenDeckCard.sprite = GameManager.instance.GetSprite(card);
        //dummyOpenDeckCard.gameObject.SetActive(true);
        //dummyOpenDeckCard.transform.DOMove(new Vector3(4, 0.5f, 0), 0.6f);

        yield return new WaitForSeconds(1);

        // TODO: DAck animation on center
        yield return new WaitForSeconds(duration);
        openDeckAnim1.gameObject.SetActive(true);
        AudioManager.instance.AudioPlay(AudioManager.instance.otcDeck);

        yield return new WaitForSeconds(duration);
        openDeckAnim2.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);
        openDeckAnim3.gameObject.SetActive(true);

        // TODO: DAck Move animation
        yield return new WaitForSeconds(duration);

        backBanch.transform.DOMove(pos, duration + 0.1f).SetEase(Ease.OutSine);
        clCounter.transform.DOMove(pos, duration + 0.1f).SetEase(Ease.OutSine);
        clBg.transform.DOMove(posBg, duration + 0.1f).SetEase(Ease.OutSine);
        flipTempCard.transform.DOMove(pos, duration + 0.1f).SetEase(Ease.OutSine);
        //openDeckAnimParent.transform.DOMove(destination.position, duration + 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
        openDeckAnimParent.transform.DOMove(pos, duration + 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            openDeckAnimParent.gameObject.SetActive(false);
            backBanch.sprite = GameManager.instance.close_DeckFull.sprite;
            backBanch.enabled = true;
            openDeckAnim1.gameObject.SetActive(false);
            openDeckAnim2.gameObject.SetActive(false);
            openDeckAnim3.gameObject.SetActive(false);

            dummyOpenDeckCard.transform.DOMove(destination.position, duration - 0.1f).OnComplete(() =>
            {
                openDeckAnimParent.transform.position = tempPos;
                dummyOpenDeckCard.gameObject.SetActive(false);
                moveAbleImg.gameObject.SetActive(true);
            });
        });
    }

    public void AddCardOnList(int i, CardController card)
    {
        playerData[i].myCards.Add(card);
    }

    public void RemoveAllCard()
    {
        playerData[0].myCards.Clear();
    }

    public void AddSelfPlayerCard()
    {
        CardPositionSet.instance.ResetCardData();
        for (int i = 0; i < GetMyPlayer().myCards.Count; i++)
        {
            CardPositionSet.instance.Add(GetMyPlayer().myCards[i].gameObject);
            GetMyPlayer().myCards[i].gameObject.tag = StaticVariable.myPlayerTag;
        }
        Logger.NormalLog($"AddSelfPlayerCard RemoveL List??? {CardPositionSet.instance.cards.Count} P = {GetMyPlayer().myCards.Count}");
        ResetMyCardPos();
    }

    internal void RemoveSelfPlayerCard(GameObject cardObj)
    {
        var player = GetMyPlayer();
        var cardController = cardObj.GetComponent<CardController>();
        Logger.NormalLog($"RemoveSelfPlayerCard Remove List??? {CardPositionSet.instance.cards.Count} P = {player.myCards.Count} null? {cardController == null}");

        if (player.myCards.Contains(cardController))
        {
            CardPositionSet.instance.RemoveCard(cardObj);
            player.myCards.Remove(cardController);
        }
    }

    internal void ResetMyCardPos()
    {
        Logger.NormalLog($"ResetMyCardPos :: {GetMyPlayer().myCards.Count}");
        foreach (var item in GetMyPlayer().myCards)
        {
            if (!item.myCanvasGroup.blocksRaycasts) item.myCanvasGroup.blocksRaycasts = true;
            if (!item.cardImage.raycastTarget) item.cardImage.raycastTarget = true;
            if (!item.cardImage.enabled) item.cardImage.enabled = true;
            if (item.disableImg.gameObject.activeInHierarchy) item.disableImg.gameObject.SetActive(false);
            if (item.gameObject.activeInHierarchy) item.SetCardGlowAnimation(false);
        }
        CardPositionSet.instance.UpdateCardPosition(true);
    }

    internal void CardAnim()
    {
        StartCoroutine(CardDistributeAnimation(0));
    }

    Tween cardDistributeTween;

    public IEnumerator CardDistributeAnimation(int playerIndex)
    {
        Logger.NormalLog($"CardDistributeAnimation===================>>>>>>>>>>>>>>");
        // Move Cards To All Player 
        for (int i = 0; i < playerData[playerIndex].myCards.Count; i++) playerData[0].myCards[i].gameObject.SetActive(false);

        for (int i = 0; i < playerData[playerIndex].myCards.Count; i++)
        {
            cardDistributeTween = playerData[playerIndex].myCards[i].transform?.DOScale(Vector3.zero, 0.2f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return StartCoroutine(SelfPlayerCard());
        cardDistributeTween?.Kill();
    }

    public IEnumerator SelfPlayerCard()
    {
        yield return new WaitForSeconds(0.3f);

        Logger.Print($"&&&&&=======>>SelfPlayerCard");
        CardPositionSet.instance.UpdateCardPosition(false);

        for (int i = 0; i < playerData[0].myCards.Count; i++)
        {
            playerData[0].myCards[i]?.gameObject.SetActive(true);
            playerData[0].myCards[i]?.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f).SetEase(Ease.OutElastic);

            if (i < GameManager.instance.BottomPlayerCard.Count)
            {
                Sprite spr = GameManager.instance.IsFlipModeSprite(GameManager.instance.BottomPlayerCard[i], !GameManager.instance.isFlipStatus);
                playerData[0].myCards[i].cardImage.sprite = spr;
            }

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);        // Move All Card To Middle
        Logger.Print($"&&&&&=======>>SelfPlayerCard 2222 ");

        for (int i = 0; i < playerData[0].myCards.Count; i++)
        {
            playerData[0].myCards[i].transform.DOMove(playerData[0].myCards[(int)(playerData[0].myCards.Count) / 2].transform.position, 0.1f).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(0.2f);
        Logger.Print($"&&&&&=======>>SelfPlayerCard 33333  Mode: {GameManager.instance.mode}");

        for (int i = 0; i < playerData[0].myCards.Count; i++)
        {
            if (i < GameManager.instance.BottomPlayerCard.Count)
            {
                Sprite spr = GameManager.instance.IsFlipModeSprite(GameManager.instance.BottomPlayerCard[i], GameManager.instance.isFlipStatus);
                playerData[0].myCards[i].cardImage.sprite = spr;
                playerData[0].myCards[i].gameObject.name = $"{i}";
            }
        }
        CardPositionSet.instance.UpdateCardPosition(true);

        if (AppData.isTutorialPlay)
            TutorialManager.Instance.HandleTutorial(4);
    }

    private PlayerController GetMyPlayer()
    {
        return playerData.Find(player => player.isMyPlayer == true);
    }

    public void TrappedCardSpawn(CardController selectCard, bool isClick = false)
    {
        this.selectCard = selectCard;
        selectCard.isMove = true;
        selectCard.transform.rotation = Quaternion.identity;
        if (!isClick)
        {
            trappedCard = Instantiate(cardPrefab, GetMyPlayer().myCardPos);
            trappedCard.GetComponent<Image>().raycastTarget = false;
            trappedCard.transform.SetSiblingIndex(this.selectCard.transform.GetSiblingIndex());
            trappedCard.cardImage.enabled = false;
        }
        selectCard.transform.SetParent(selectCardParent);
    }

    public void TrappedCardDestroy(CardController selectCard)
    {
        if (selectCard == null) return;
        selectCard.dummyRectObj.anchoredPosition = Vector2.zero;
        selectCard.myRect.sizeDelta = new Vector2(148f, 196f);
        selectCard.myRect.Rotate(Vector2.zero);
        selectCard.transform.SetParent(GetMyPlayer().myCardPos);
        if (trappedCard != null)
        {
            Logger.NormalLog($"trappedCard = {trappedCard.name} ");
            selectCard.transform.SetSiblingIndex(trappedCard.transform.GetSiblingIndex());
            trappedCard.transform.SetParent(selectCardParent);
            DestroyImmediate(trappedCard.gameObject);
            trappedCard = null;
        }
        else trappedCard = null;
        //Logger.NormalLog($"  this.selectCard : TrappedCardDestroy{this.selectCard.name}");
        CardPositionSet.instance.CustomUpdateCardPosition();
        selectCard.isMove = false;
        this.selectCard = null;
    }

    public void TrappCardDestroy()
    {
        if (selectCard != null)
        {
            DestroyImmediate(selectCard);
        }
        selectCard = trappedCard = null;
    }

    private void ResetThisScript()
    {
        if (GameManager.instance.MyCardGrid.transform.childCount != 0)
            GameManager.instance.MyCardGrid.transform.GetChild(0).GetComponent<CardController>().glowImg.DOFade(0, 0);
        playerData[0].myCards.Clear();
        CardPositionSet.instance?.ResetCardData();
        dummyOpenDeckCard.transform.position = Vector3.zero;
        dummyOpenDeckCard.gameObject.SetActive(false);
        openDeckAnimParent.gameObject.SetActive(false);

        openDeckAnim1.gameObject.SetActive(false);
        openDeckAnim2.gameObject.SetActive(false);
        openDeckAnim3.gameObject.SetActive(false);
    }

}
public static class StaticVariable
{
    public const string myPlayerTag = "MyPlayerCard";
}

