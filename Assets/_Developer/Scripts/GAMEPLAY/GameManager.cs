using DG.Tweening;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using System.Data;
//using GoogleMobileAds.Api;

//// Don't forget to add this.
//using UnityEngine.XR;
//using Unity.Collections.LowLevel.Unsafe;
//using GoogleMobileAds.Api;
//using System.Reflection;

public class GameManager : MonoBehaviour
{
    private string TAG = ">> PLAYING >>>> ";

    List<GTIResponse> gtiRespoce = new List<GTIResponse>();
    public List<int> rules = new List<int>();

    [Header("Panel")]
    public GameObject playingScreen;
    public GameObject allPlayingScreen;

    [Space(5)]
    //bottom player
    public RawImage BottomPlayerImg, LeftPlayerImg, TopPlayerImg, RightPlayerImg;
    public Image bottomPlayerFrame, leftPlayerFrame, topPlayerFrame, rightPlayerFrame;
    public Texture2D DefaultUser, DefaultGift;
    public Sprite defaultFrame;
    public RawImage BottomPlayerGift, LeftPlayerGift, TopPlayerGift, RightPlayerGift;
    public SelfTurnTimerControl BottomPlayerTurnRing, LeftPlayerTurnRing, TopPlayerTurnRing, RightPlayerTurnRing;
    public GameObject LeftPlayerAnimCard, TopPlayerAnimCard, RightPlayerAnimCard;
    public GameObject LeftPlayerAnimCardCopy, TopPlayerAnimCardCopy, RightPlayerAnimCardCopy;
    public TextMeshProUGUI BottomPlayerName, LeftPlayerName, TopPlayerName, RightPlayerName;
    public GameObject BottomPlayerCardPile, LeftPlayerCardPile, TopPlayerCardPile, RightPlayerCardPile;
    public TextMeshProUGUI BottomPlayerCardCounter, LeftPlayerCardCounter, TopPlayerCardCounter, RightPlayerCardCounter, CloseDeckCounter;
    public GameObject BottomDealCard, RightDealCard, TopDealCard, LeftDealCard, demoCard, RotationDemo, CenterFrame, CardCounter;
    public GameObject BottomUnoTxt, LeftUnoTxt, TopUnoTxt, RightUnoTxt;

    [SerializeField] Image flipCloseDeckImg;
    [SerializeField] Image playinBG;
    [SerializeField] Sprite[] playingBgSprites;

    [SerializeField] Transform backDeckTransform;

    [Header("Playing Hint tutorial")]
    [SerializeField] Transform playingHandTutorial, playingClickHand;
    [SerializeField] bool isPlayingHint;

    [Header("SymbolCard")]
    public Image symbolStaticCard;
    Vector3 discardPosStore;
    [SerializeField] List<GameObject> discardList = new List<GameObject>();


    [SerializeField] GameObject hintUnoToolTip;
    [SerializeField] GameObject hintActionToolTip;
    [SerializeField] TextMeshProUGUI hintActionToolText;

    [Space(10)]
    public GameObject BottomThrowCard, LeftThrowCard, TopThrowCard, RightThrowCard, BottomThrowAnimCard;
    public GameObject bottomFlipTag, leftFlipTag, topFlipTag, rightFlipTag;
    public Rotation CenterGlowCard;
    public GameObject partnerCardsPosition;
    public GameObject MyCardGrid, partnerCardGrid;

    public GameObject SpriteImg;
    public Image cardSprite;
    [SerializeField] Button messageBtn, bottomPlayerGiftBtn, leftPlayerGiftBtn, topPlayerGiftBtn, rightPlayerGiftBtn;

    public List<Sprite> Red, Green, Blue, Yellow, Kali;
    public List<Sprite> wildAllColor, wildPlusAllColor, wildUpColor, wildShieldColor, wildSpesificColor;

    [Header("Emoji Mode Sprites")]
    public List<Sprite> e_Red, e_Green, e_Blue, e_Yellow;
    public TextMeshProUGUI BetValue, PotValue, gemsValue, goldValue;

    public GameObject GameStartTimer;
    public TextMeshProUGUI GameStartTimerTxt;

    //public Image deckImg;
    public Image cardBunch;
    public Image close_DeckFull, closeDeckHalf, closeDeckSingle;

    public List<AnimationClip> emojiClips = new List<AnimationClip>();

    [HideInInspector] public PlayersCards[] UpdatedCardList;
    [Serializable]
    public class PlayersCards
    {
        public List<string> cardsString = new List<string>();
    }

    [Header("---------- WildUPCARD --------")]
    [SerializeField] TextMeshProUGUI wildUpCounter;
    [SerializeField] Animator wildUpshiny;
    [SerializeField] ParticleSystem wildUpAnimParticle;
    [SerializeField] ParticleSystem sparks1, sparks2;

    public GameObject wildDummyShowObject;
    public Image wildDummyShowImg;

    [Header("------------------------------- ALL PLAYER CARD LIST -----------------------")]
    public List<string> LeftPlayerCard, TopPlayerCard, RightPlayerCard, BottomPlayerCard;
    public List<string> selectedCard;

    [SerializeField] Texture2D inviteUser;

    [Header("ChooseColor")]
    public GameObject ChooseColorPanel, ChooseColorText;
    public Image colorBlackBg;
    public Image glowFadeAnim;
    public Color[] flipColors;
    public Image[] SelectColorImg;
    [SerializeField] Vector3[] extraPositions;
    [SerializeField] Vector3[] resetExtraPositions;
    public Sprite[] BlueColor, RedColor, YellowColor, GreenColor, ColorTxt, ResetColor;
    public Sprite[] setFlipColor;
    public Button[] allColorButtons;

    //KeepPlay & Challenge
    public GameObject KeepPlayPanel;
    [SerializeField] Image keepPlayBg, keepCardImg;
    [SerializeField] Transform keepPlayPopUp;
    [SerializeField] GameObject hummerObjKeepPlay;
    [SerializeField] TextMeshProUGUI keepBtnText, playBtnText, noteText;
    [SerializeField] GameObject passBtn, drawBg;

    //uno button
    public GameObject UnoBtn;

    [Header("ChipsCollectAnim")]
    [SerializeField] GameObject BottomCollectAnimFrm;
    public GameObject LeftCollectAnimFrm, TopCollectAnimFrm, RightCollectAnimFrm, CenterCollectAnimFrm;

    [Header("BackMenu")]
    [SerializeField] GameObject BackMenuPannel;
    [SerializeField] Image backMenuBg;
    [SerializeField] Transform backMenuPopUp;
    public GameObject BackMenuSound, BackMenuVibrate;
    public Sprite[] OnOffSprite;

    [Header("TableInfo")]
    [SerializeField] GameObject TableInfoPannel;
    [SerializeField] Image tableInfoBg;
    [SerializeField] Transform tableInfoPopUp;
    public TextMeshProUGUI[] TableInfoValue;

    public static GameManager instance;

    //local Variable
    float cardAnimDuration = 0.6f;

    string lastPickCard = "";
    bool isKeep = false;

    public bool isUno = false;
    public bool isFlipStatus = false;
    public bool MyTurn = false;
    int cIndex = -1;

    public string TableId = "";

    string OpenCard = "";
    int LeftCardDeal = 0, RightCardDeal = 0, TopCardDeal = 0, BottomCardDeal = 0, TotalDealCard = 7;

    int LeftSeatIndex = -1, TopSeatIndex = -1, RightSeatIndex = -1, BottomSeatIndex = -1;

    public long bv = 100, tpv = 0;
    int gems = 0, Player = 4;
    public string mode = "", tbid = "";
    public bool isTournament = false,
        BacktoLobby = true;
    public int ip = 0;

    [Header("Action Cards")]
    //[SerializeField] Transform[] specificFour;
    [SerializeField] Transform[] shildPlayerImg;
    [SerializeField] Transform cycloneObject;
    [SerializeField] Transform cycloneImg, cycloneParticle;
    [SerializeField] Transform cycloneFireWork;
    [SerializeField] RectTransform[] highFiveHands;

    [Header("UNO Text")]
    [SerializeField] GameObject unoTextParticle;
    [SerializeField] Animator[] unoAnimator;
    [SerializeField] Transform[] catchUNOBtn;
    [SerializeField] Image[] catchUNOImg;
    [SerializeField] Sprite catchUNOSprite, willySprite;

    [Header("New Reverse Card Animation")]
    [SerializeField] ReverseRotation reverseRotation;
    [SerializeField] ParticleSystem reversePar;

    [Header("Reverse Card Animation")]
    [SerializeField] GameObject arrowsObj, arrow1Obj, arrow2Obj;
    [SerializeField] Sprite[] arrowSprite;
    [SerializeField] Sprite[] arrow1SliceSprite;
    [SerializeField] Sprite[] arrow2SliceSprite;
    [SerializeField] GameObject arrowSpawnParticle;
    [SerializeField] Light lightObj;
    [SerializeField] List<ParticleSystem> particleSystems;
    [SerializeField] List<Material> particleMaterials;
    [SerializeField] List<Gradient> redEffect, yellowEffect, greenEffect, blueEffect;
    [SerializeField] List<Texture2D> materialTextures;

    [Header("Skip Image Animation")]
    [SerializeField] GameObject skipImgObj, skipImgObj2, skipImgObj3, skipImgObj4;
    [SerializeField] Image blockTxtImg;
    [SerializeField] Sprite blockSpr, megaBlockSpr, reverseSpr, skip2blockSpr;
    [SerializeField] List<Sprite> skipImgSprites;
    [SerializeField] GameObject skipParticle, skipParticle2;

    [Header("+2 +4 Card Animation")]
    [SerializeField] List<TextMeshProUGUI> cardAddedImgs = new();
    [SerializeField] List<GameObject> cardAddTextParticles;
    [SerializeField] Sprite plus2Sprite, plus4Sprite, plus6Sprite, plus1Sprite, plus5Sprite;
    [SerializeField] Sprite pRed, pgreen, pblue, pyellow;
    [SerializeField] Sprite[] plus2Icons;
    [SerializeField] Sprite[] plus4Icons;
    [SerializeField] Image plusAnimImg, plusAnimGlow;
    //[SerializeField] Image[] specificSelectShow;

    [Header("UTS Animation")]
    public List<GameObject> playerFrms;

    [Header("Challange Animation")]
    [SerializeField] GameObject hummarObject, hammer1, hammer2;
    [SerializeField] GameObject challengeBottom, challengeLeft, challengeTop, challengeRight;
    [SerializeField] GameObject challengeParticle;
    [SerializeField] Sprite challengeWinSprite, challengeLoseSprite;
    int timeOutindex = -1;
    int throwIndex = -1;
    int throw2Index = -1;

    [Header("Wining Animation")]
    [SerializeField] List<GameObject> winningTags;
    [SerializeField] List<TextMeshProUGUI> winPoints;
    public List<Sprite> winningTagSprites;
    [SerializeField] List<GameObject> crackerParticles;
    [SerializeField] GameObject lossTextObject, winningTextObject, kingObject;
    [SerializeField] List<GameObject> HighlightObjects;
    float winAnimDuration = 1f;

    [Header("Emoji Mode")]
    [SerializeField] Animator bottomPlayerEmoji;
    [SerializeField] Animator leftPlayerEmoji, topPlayerEmoji, rightPlayerEmoji;

    [SerializeField] GameObject leftPlayerCardGrid, rightPlayerCardGrid;


    [SerializeField] Button bottomPlayerImgButton, leftPlayerImgButton, rightPlayerImgButton, topPlayerImgButton;
    bool isInTesting = true;

    [Header("Turn Miss ToolTip")]
    [SerializeField] GameObject turnMissToolTip;
    [SerializeField] TextMeshProUGUI turnMissToolTipMsgText;

    [Header("Gold & Coin Cut Animation")]
    [SerializeField] GameObject animationTxtObjCoin;
    [SerializeField] GameObject animationTxtObjGems;
    [SerializeField] TextMeshProUGUI animationTextCoin, animationTextGems;

    [SerializeField] Sprite[] modeNames, modeTypes;
    [SerializeField] Image modeNameImg, modeTypeImg;

    [Header("For Rejoin & Reconnect")]
    [SerializeField] List<Image> centerThrowCards;

    [Header("Tournament")]
    [SerializeField] Button switchTableBtn;
    [SerializeField] Image betValImg;
    [SerializeField] RectTransform betValTransform;
    [SerializeField] Sprite betInTour, normalBet;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] GameObject roundTextObject;

    [SerializeField] List<GameObject> closablePanels;

    [Header("Maintenence Tooltip")]
    public GameObject maintenenceToolTip;
    public TextMeshProUGUI maintenenceTimerTxt;

    [Header("Message Tooltip")]
    [SerializeField] List<GameObject> msgTooltips;
    [SerializeField] List<TextMeshProUGUI> msgToolTipTxts;

    [Header("Leaf Animation")]
    [SerializeField] GameObject leafAnimationParent;
    [SerializeField] TextMeshProUGUI leafTxt;

    //For GamePlayMultiClick Management...
    bool isDeckClickable = false;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (TableInfoPannel.activeInHierarchy) OnClick_BackMenu(6, false);
            else if (!BackMenuPannel.activeInHierarchy && !KeepPlayPanel.activeInHierarchy && !GameWinner.instance.WinnerPannel.activeInHierarchy && !AppData.isTutorialPlay) LeaveTable();
            else OnClick_BackMenu(0, false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //ArrowAnimation(true);
        }
    }

    private void Start()
    {

        if (AppData.closePlaying)
        {
            Logger.Print(TAG + "Close Playing Is  : " + AppData.closePlaying);
            AppData.closePlaying = false;
            SceneManager.LoadScene(EventHandler.DASHBOARD);
            return;
        }
        SocketManagergame.ISDEADLOCKACCURE = false;
        //AppData.canShowChallenge = false;
        discardPosStore = symbolStaticCard.transform.localPosition;
        allPlayingScreen.gameObject.SetActive(false);

        for (int k = 0; k < resetExtraPositions.Length; k++)
        {
            resetExtraPositions[k] = SelectColorImg[k].transform.localPosition;
        }
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_JT += OnRecevied_JT;
        SocketManagergame.OnListner_GST += OnRecevied_GST;
        SocketManagergame.OnListner_CBV += OnRecevied_CBV;
        SocketManagergame.OnListner_SDC += OnRecevied_SDC;
        SocketManagergame.OnListner_SMC += OnRecevied_SMC;
        SocketManagergame.OnListner_EG += OnRecevied_EG;
        SocketManagergame.OnListner_UTS += OnRecevied_UTS;
        SocketManagergame.OnListner_SELECTCARD += SelectWildCardCall;
        SocketManagergame.OnListner_TC += OnRecevied_TC;
        SocketManagergame.OnListner_PFCD += OnRecevied_PFCD;
        SocketManagergame.OnListner_PENALTY += OnRecevied_PENALTY;
        SocketManagergame.OnListner_KEEP += OnRecevied_KEEP;
        SocketManagergame.OnListner_GTI += HandleGTI;
        SocketManagergame.OnListner_RGTI += HandleRGTI;
        SocketManagergame.OnListner_CHALLENGE += Challenge;
        SocketManagergame.OnListner_CHALLENGERES += OnRecevied_CHALLENGERESULT;
        SocketManagergame.OnListener_SGTU += OnRecevied_SGTU;
        SocketManagergame.OnListner_COT += OnRecevied_COT;
        SocketManagergame.OnListner_GGL += OnRecevied_GGL;
        SocketManagergame.OnListner_UG += OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE += OnRecevied_USERGEMS;
        SocketManagergame.OnListner_SAYUNO += HandleSAYUNO;
        SocketManagergame.OnListner_SWAPCARD += HandleZeroSeven;
        SocketManagergame.OnListner_HighFive += HandleHighFive;
        SocketManagergame.OnListner_ULE += HandleUpdateLeaf;
        SocketManagergame.OnListner_OTCDACK += OnRecevied_OTCDACKANIMATION;

        AppData.AllPanelClose += HandleCloseAllPanels;

    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_JT -= OnRecevied_JT;
        SocketManagergame.OnListner_GST -= OnRecevied_GST;
        SocketManagergame.OnListner_CBV -= OnRecevied_CBV;
        SocketManagergame.OnListner_SDC -= OnRecevied_SDC;
        SocketManagergame.OnListner_SMC -= OnRecevied_SMC;
        SocketManagergame.OnListner_EG -= OnRecevied_EG;
        SocketManagergame.OnListner_UTS -= OnRecevied_UTS;
        SocketManagergame.OnListner_SELECTCARD -= SelectWildCardCall;
        SocketManagergame.OnListner_TC -= OnRecevied_TC;
        SocketManagergame.OnListner_PFCD -= OnRecevied_PFCD;
        SocketManagergame.OnListner_PENALTY -= OnRecevied_PENALTY;
        SocketManagergame.OnListner_KEEP -= OnRecevied_KEEP;
        SocketManagergame.OnListner_GTI -= HandleGTI;
        SocketManagergame.OnListner_RGTI -= HandleRGTI;
        SocketManagergame.OnListner_CHALLENGE -= Challenge;
        SocketManagergame.OnListner_CHALLENGERES -= OnRecevied_CHALLENGERESULT;
        SocketManagergame.OnListener_SGTU -= OnRecevied_SGTU;
        SocketManagergame.OnListner_COT -= OnRecevied_COT;
        SocketManagergame.OnListner_GGL -= OnRecevied_GGL;
        SocketManagergame.OnListner_UG -= OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE -= OnRecevied_USERGEMS;
        SocketManagergame.OnListner_SAYUNO -= HandleSAYUNO;
        SocketManagergame.OnListner_SWAPCARD -= HandleZeroSeven;
        SocketManagergame.OnListner_HighFive -= HandleHighFive;
        SocketManagergame.OnListner_ULE -= HandleUpdateLeaf;
        SocketManagergame.OnListner_OTCDACK -= OnRecevied_OTCDACKANIMATION;

        AppData.AllPanelClose -= HandleCloseAllPanels;
    }

    [SerializeField] float cardDealAnimTime = 0.3f;
    [SerializeField] GameObject animPosCard;

    [SerializeField]
    List<string> closeDeck = new List<string>();
    int clCounter;

    public void GetCloseDeck(JSONNode data)
    {
        var players = new List<List<string>>();

        for (int i = 0; i < 4; i++)
        {
            players.Add(JsonConvert.DeserializeObject<List<string>>(data["Allplayercard"][i]["cards"].ToString()));
        }

        Logger.Print($" user1 {players[0].Count} | user2 {players[1].Count} | user3 {players[2].Count} | user4 {players[3].Count}");
        closeDeck = new List<string>();
        clCounter = 0;
        int currentPlayerIndex = data["d"].AsInt;

        // Add cards based on the current player
        for (int i = 0; i < players[0].Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                closeDeck.Add(players[(currentPlayerIndex + j) % 4][i]);
            }
        }

        closeDeck.Add(data["c"]);
        closeDeck.Add(data["closedeck"][0]);
    }

    private void OnRecevied_SDC(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_SDC called  && Bottom Player Card >> " + BottomPlayerCard.Count + "n = " + CardDeckController.instance.playerData[0].myCardPos.GetChild(0).transform.gameObject);
        try
        {
            if (mode.Equals(AppData.FLIP))
                GetCloseDeck(data);

            BacktoLobby = false;
            TotalDealCard = (AppData.isTutorialPlay) ? 7 : BottomPlayerCard.Count;

            cardDealAnimTime = .2f;
            animPosCard.transform.localScale = new Vector2(1.3f, 1.3f);
            animPosCard.gameObject.SetActive(false);

            if (data["d"].AsInt == BottomSeatIndex) CardDealAnimation(BottomDealCard, animPosCard.transform);
            else if (data["d"].AsInt == LeftSeatIndex) CardDealAnimation(LeftDealCard, LeftPlayerImg.transform);
            else if (data["d"].AsInt == TopSeatIndex) CardDealAnimation(TopDealCard, TopPlayerImg.transform);
            else if (data["d"].AsInt == RightSeatIndex) CardDealAnimation(RightDealCard, RightPlayerImg.transform);

            if (!AppData.isTutorialPlay)
                StartCoroutine(WaitForAnimColpelete(data));
            else
                StartCoroutine(WaitTutorialAnimationOff());

        }
        catch (Exception e)
        {
            Logger.Print(TAG + " Exception " + e.ToString());
        }

        OpenCard = data["c"];
    }

    bool animWaitCom = false;

    private IEnumerator WaitForAnimColpelete(JSONNode data)
    {
        Logger.Print($"WaitFoeAnimColpelete ::: CAll 1");

        yield return new WaitUntil(() => animWaitCom);

        yield return new WaitForSeconds(1);

        Logger.Print($"WaitForAnimColpelete ::: CAll 2");

        closeDeckCounter = data["counter"].AsInt;
        animWaitCom = false;

        for (int i = 0; i < data["Allplayercard"].Count; i++)
        {
            List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["Allplayercard"][i]["cards"].ToString());
            if (data["Allplayercard"][i]["si"] == BottomSeatIndex)
            {
                BottomPlayerCard = PlayerCard;
            }
            else if (data["Allplayercard"][i]["si"] == LeftSeatIndex)
            {
                LeftPlayerCard = PlayerCard;
                RedrawCard(LeftSeatIndex);
            }
            else if (data["Allplayercard"][i]["si"] == TopSeatIndex)
            {
                TopPlayerCard = PlayerCard;
                RedrawCard(TopSeatIndex);
            }
            else if (data["Allplayercard"][i]["si"] == RightSeatIndex)
            {
                RightPlayerCard = PlayerCard;
                RedrawCard(RightSeatIndex);
            }
        }
    }

    private void CardDealAnimation(GameObject cardObject, Transform Destination)
    {
        Logger.Print(TAG + " CardDealAnimation called For >>> " + cardObject.name);
        if (gameIsInBackground)
        {
            Logger.Print(TAG + " Game Is In Background >> ");

            Sprite spr = IsFlipModeSprite(OpenCard, isFlipStatus);

            BottomThrowCard.transform.GetComponent<Image>().sprite = spr;

            RedrawCard(BottomSeatIndex);
            MyCardGrid.GetComponent<GridLayoutGroup>().spacing = new Vector2(-65, -110);
            if (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))
            {
                RedrawCard(TopSeatIndex);
                partnerCardGrid.GetComponent<GridLayoutGroup>().spacing = new Vector2(-32.5f, -55);
            }

            closeDeckCounter = 104;
            CloseDeckAnim();

            cardObject.gameObject.SetActive(false);
            cardObject.transform.position = demoCard.transform.position;
            cardObject.transform.rotation = RotationDemo.transform.rotation;
            cardObject.transform.localScale = demoCard.transform.localScale;

            BottomPlayerCardCounter.text = "7";
            LeftPlayerCardCounter.text = "7";
            TopPlayerCardCounter.text = "7";
            RightPlayerCardCounter.text = "7";
            return;
        }

        closeDeckCounter--;
        CloseDeckAnim();


        if (mode.Equals(AppData.FLIP))
        {
            flipCloseDeckImg.gameObject.SetActive(true);
            Image cSpr = cardObject.GetComponent<Image>();
            cSpr.sprite = IsFlipModeSprite(closeDeck[clCounter], !isFlipStatus);
        }

        cardObject.transform.DOMove(Destination.position, cardDealAnimTime).SetDelay(cardDealAnimTime / 2).OnStart(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.cardDeal);
            Logger.Print(TAG + " Do Move Start For  >> " + cardObject.name);
            cardObject.gameObject.SetActive(true);

            if (mode.Equals(AppData.FLIP))
            {
                clCounter++;
                flipCloseDeckImg.sprite = IsFlipModeSprite(closeDeck[clCounter], !isFlipStatus);
            }

            if (cardObject == BottomDealCard)
            {
                Logger.Print(TAG + " Do Rotate Start For  >> " + cardObject.name + " Count B  " + LeftCardDeal + " Totle = " + TotalDealCard);
                BottomCardDeal++;
                BottomPlayerCardCounter.text = BottomCardDeal.ToString();
                if (LeftCardDeal != TotalDealCard)
                    CardDealAnimation(LeftDealCard, LeftPlayerImg.transform);
                else
                    OpenCardAnimation();

                cardObject.transform.DOScale(Vector3.one * 1.3f, cardDealAnimTime / 1.67f).SetDelay(cardDealAnimTime / 2.5f).OnComplete(() =>
                {
                    Logger.Print(TAG + " Do Scale Completed For  >> " + cardObject.name);
                    animPosCard.SetActive(true);
                    animPosCard.gameObject.GetComponent<Image>().sprite = mode.Equals(AppData.FLIP) ? IsFlipModeSprite(closeDeck[clCounter], !isFlipStatus) : cardSprite.sprite;

                    cardObject.gameObject.SetActive(false);
                    cardObject.transform.position = demoCard.transform.position;

                    cardObject.transform.rotation = RotationDemo.transform.rotation;
                    cardObject.transform.localScale = demoCard.transform.localScale;


                });//0.45f
            }
            else if (cardObject == LeftDealCard)
            {

                Logger.Print(TAG + " Do Rotate Start For  >> " + cardObject.name + " Count Left  " + TopCardDeal + " Totle = " + TotalDealCard);
                LeftCardDeal++;
                LeftPlayerCardCounter.text = LeftCardDeal.ToString();
                if (TopCardDeal != TotalDealCard)
                    CardDealAnimation(TopDealCard,/* (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) ? partnerCardsPosition.transform :*/ TopPlayerImg.transform);
                else
                    OpenCardAnimation();

                cardObject.transform.DOScale(new Vector3(0f, 0f, 0f), cardDealAnimTime / 2.5f).SetDelay(cardDealAnimTime * 0.6f).OnComplete(() =>
                {
                    Logger.Print(TAG + " Do Scale Completed For  >> " + cardObject.name);
                    cardObject.gameObject.SetActive(false);
                    cardObject.transform.position = demoCard.transform.position;
                    cardObject.transform.rotation = RotationDemo.transform.rotation;
                    cardObject.transform.localScale = demoCard.transform.localScale;


                });
            }
            else if (cardObject == TopDealCard)
            {
                Logger.Print(TAG + " Do Rotate Start For  >> " + cardObject.name + " Count T  " + RightCardDeal + " Totle = " + TotalDealCard);
                TopCardDeal++;
                TopPlayerCardCounter.text = TopCardDeal.ToString();
                if (RightCardDeal != TotalDealCard)
                    CardDealAnimation(RightDealCard, RightPlayerImg.transform);
                else
                    OpenCardAnimation();

                cardObject?.transform.DOScale(Vector3.zero, cardDealAnimTime / 2.5f).SetDelay(cardDealAnimTime * 0.6f).OnComplete(() =>
                {
                    Logger.Print(TAG + " Do Scale Completed For  >> " + cardObject.name);
                    cardObject.gameObject.SetActive(false);
                    cardObject.transform.position = demoCard.transform.position;
                    cardObject.transform.rotation = RotationDemo.transform.rotation;
                    cardObject.transform.localScale = demoCard.transform.localScale;


                });
            }
            else if (cardObject == RightDealCard)
            {
                Logger.Print(TAG + " Do Rotate Start For  >> " + " Count R  " + BottomCardDeal + " Totle = " + TotalDealCard);
                RightCardDeal++;
                RightPlayerCardCounter.text = RightCardDeal.ToString();
                Logger.Print(TAG + "Bottom Animation Called >> " + animPosCard.name);
                if (BottomCardDeal != TotalDealCard)
                    CardDealAnimation(BottomDealCard, animPosCard.transform);
                else
                    OpenCardAnimation();

                cardObject.transform.DOScale(new Vector3(0f, 0f, 0f), cardDealAnimTime / 2.5f).SetDelay(cardDealAnimTime * 0.6f).OnComplete(() =>
                {
                    Logger.Print(TAG + " Do Scale Completed For  >> " + cardObject.name);
                    cardObject.gameObject.SetActive(false);
                    cardObject.transform.position = demoCard.transform.position;
                    cardObject.transform.rotation = RotationDemo.transform.rotation;
                    cardObject.transform.localScale = demoCard.transform.localScale;

                });
            }
        });
    }

    private void OpenCardAnimation()
    {
        if (gameIsInBackground)
        {
            closeDeckCounter--;
            CloseDeckAnim();

            OnCardCloseDackAnimation();
            if (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) RedrawCard(TopSeatIndex);

            animPosCard.transform.localScale = Vector3.zero;
            TopThrowCard.transform.GetComponent<Image>().sprite = IsFlipModeSprite(OpenCard, isFlipStatus);
            //TopThrowCard.transform.GetComponent<Image>().sprite = GetSprite(OpenCard);
            TopThrowCard.SetActive(true);
            topFlipTag.SetActive(mode.Equals(AppData.FLIP));
            CenterGlowCard.gameObject.SetActive(true);
            CenterGlowCard.enabled = true;
            CenterGlowCard.SetRingColor(getCardColor(OpenCard));

            BottomDealCard.SetActive(false);
            LeftDealCard.SetActive(false);
            TopDealCard.SetActive(false);
            RightDealCard.SetActive(false);
            BottomDealCard.transform.position = demoCard.transform.position;
            BottomDealCard.transform.rotation = RotationDemo.transform.rotation;
            BottomDealCard.transform.localScale = demoCard.transform.localScale;
            return;
        }
        BottomDealCard.transform.DOMove(TopThrowCard.transform.position, cardDealAnimTime).SetDelay(cardDealAnimTime).OnStart(() =>
        {
            BottomDealCard.gameObject.SetActive(true);
            BottomDealCard.transform.eulerAngles = Vector3.zero;
            BottomDealCard.transform.DOScale(Vector3.one, cardDealAnimTime);
            AudioManager.instance.AudioPlay(AudioManager.instance.OpenCard);

            OnCardCloseDackAnimation();
            animWaitCom = true;
            Invoke(nameof(DelayOpenDack), 1.5f);
        });
    }

    private void DelayOpenDack()
    {
        //BottomDealCard.transform.DORotate(new Vector3(35, 0, 10), cardDealAnimTime).OnStart(() =>
        BottomDealCard.transform.DORotate(new Vector3(35, 0, 0), cardDealAnimTime).OnStart(() =>
        {
            closeDeckCounter--;
            CloseDeckCounter.transform.parent.gameObject.SetActive(closeDeckCounter != 0);
            CloseDeckCounter.gameObject.SetActive(closeDeckCounter != 0);
            CloseDeckAnim();

            BottomDealCard.transform.GetComponent<Image>().sprite = IsFlipModeSprite(OpenCard, isFlipStatus);
            TopThrowCard.transform.GetComponent<Image>().sprite = IsFlipModeSprite(OpenCard, isFlipStatus);

        }).OnComplete(() =>
        {
            TopThrowCard.SetActive(true);
            topFlipTag.SetActive(mode.Equals(AppData.FLIP));

            CenterGlowCard.gameObject.SetActive(true);
            CenterGlowCard.enabled = true;
            CenterGlowCard.SetRingColor(getCardColor(OpenCard));
            cardDealAnimTime = .4f;

            BottomDealCard.SetActive(false);
            LeftDealCard.SetActive(false);
            TopDealCard.SetActive(false);
            RightDealCard.SetActive(false);

            BottomDealCard.transform.position = demoCard.transform.position;
            BottomDealCard.transform.rotation = RotationDemo.transform.rotation;
            BottomDealCard.transform.localScale = demoCard.transform.localScale;
        });
    }

    public Sprite GetFlipSide(string card, bool isFlip)
    {
        string cardName = AppData.GetFilpName(card, isFlip);

        Logger.Print($"GetFlipSide : {cardName} | isFlip : {isFlip}");

        if (isFlip)
        {
            switch (getCardColor(cardName))
            {
                case "o":
                    return CardDeckController.instance.cardSpritesFlipDark[0].cardSpr[getCardValue(cardName)];

                case "t":
                    return CardDeckController.instance.cardSpritesFlipDark[1].cardSpr[getCardValue(cardName)];

                case "j":
                    return CardDeckController.instance.cardSpritesFlipDark[2].cardSpr[getCardValue(cardName)];

                case "p":
                    return CardDeckController.instance.cardSpritesFlipDark[3].cardSpr[getCardValue(cardName)];

                case "k":
                    return CardDeckController.instance.cardSpritesFlipDark[4].cardSpr[getCardValue(cardName)];
            }
            return null;
        }
        else
        {
            switch (getCardColor(cardName))
            {
                case "r":
                    return CardDeckController.instance.cardSpritesFlipLight[0].cardSpr[getCardValue(cardName)];

                case "g":
                    return CardDeckController.instance.cardSpritesFlipLight[1].cardSpr[getCardValue(cardName)];

                case "b":
                    return CardDeckController.instance.cardSpritesFlipLight[2].cardSpr[getCardValue(cardName)];

                case "y":
                    return CardDeckController.instance.cardSpritesFlipLight[3].cardSpr[getCardValue(cardName)];

                case "k":
                    return CardDeckController.instance.cardSpritesFlipLight[4].cardSpr[getCardValue(cardName)];
            }
            return null;
        }

    }

    internal Sprite GetSprite(string card)
    {
        if (mode.Equals(AppData.EMOJISOLO) || mode.Equals(AppData.EMOJIPARTNER))
        {
            switch (getCardColor(card))
            {
                case "r":
                    return e_Red[getCardValue(card)];

                case "g":
                    return e_Green[getCardValue(card)];

                case "b":
                    return e_Blue[getCardValue(card)];

                case "y":
                    return e_Yellow[getCardValue(card)];

                case "k":
                    return Kali[getCardValue(card)];
            }
            return null;
        }
        else
        {
            int cardValue = getCardValue(card);
            bool isHighFive = rules.Contains(7) && cardValue == 5;
            bool isSwapSeven = rules.Contains(6) && cardValue == 7;
            bool isSwapZero = rules.Contains(6) && cardValue == 0;
            string cardColor = getCardColor(card);

            int colorIndex = cardColor switch
            {
                "r" => 0,
                "g" => 1,
                "b" => 2,
                "y" => 3,
                _ => -1 // Default, in case we need to handle invalid card color
            };

            if (colorIndex >= 0 && isHighFive)
            {
                return CardDeckController.instance.highFiveSprites[colorIndex];
            }

            if (colorIndex >= 0 && isSwapSeven)
            {
                return CardDeckController.instance.sevenSwapSpr[colorIndex];
            }

            if (colorIndex >= 0 && isSwapZero)
            {
                return CardDeckController.instance.zeroSwapSpr[colorIndex];
            }
            //Logger.Print($"GetSprite :: colorIndex = {colorIndex} | cardValue : {cardValue}");
            if (colorIndex >= 0)
            {
                return CardDeckController.instance.cardSpritesNormal[colorIndex].cardSpr[cardValue];
            }

            if (cardColor == "k")
            {
                return Kali[cardValue];
            }

            return null; // Return null if no valid match is found
        }
    }

    bool tcodFlage = false;

    int CheckTableSi(int seat)
    {
        int si = -1;
        for (int j = 0; j < gtiRespoce.Count; j++)
        {
            if (gtiRespoce[j].getSi() == seat)
            {
                si = j;
            }
        }
        return si;
    }

    string GetColorOnTutorial(string cs)
    {
        string color = cs switch
        {
            "r" => "ROSE",
            "g" => "TEAL",
            "b" => "STEAL BLUE",
            "y" => "LEMON",
            _ => ""
        };

        return color;
    }

    private void OnRecevied_TC(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_TC called");
        Loading_screen.instance.ShowLoadingScreen(false);

        fillTweenAnim?.Kill();
        ImageAlphaClear();

        if (manageTimer != null)
            StopCoroutine(manageTimer);

        symbolStaticCard.transform.localPosition = discardPosStore;
        AudioManager.instance.tikAudioSource.Stop();// Stop sound
        tcodFlage = true;

        BacktoLobby = (data["cards"].Count > 0);

        Logger.Print(TAG + $" BacktoLobby:: {data["cards"].Count > 0}");

        Vector2 allpos = new Vector2(97, 5);
        BottomThrowCard.transform.localPosition = LeftThrowCard.transform.localPosition = TopThrowCard.transform.localPosition = RightThrowCard.transform.localPosition = allpos;

        lastPickCard = "";
        isUno = MyTurn = isKeep = isInUNO = false;

        if (isKeepedCard)
            isKeepedCard = false;

        cIndex = -1;
        int skipSi = data["skipSi"].AsInt;
        bool isForgotUNO = data["penalty"];


        if (isForgotUNO)
            isInUNO = true;

        unoForgotterSI = isForgotUNO ? data["si"].AsInt : -1;

        AudioManager.instance.AudioPlay(AudioManager.instance.cardThrow);

        isFlipStatus = data["isFlip"].AsBool; // Flip Card Status Update || <<===  UPDATE

        bool isThrowFlipCard = data["flipcard"].AsBool;

        string card = IsFlipModeCardStr(data["c"]);
        string ltc;

        Logger.Print($" TC ==> card: {card} AND isWild = {getCardColor(card).Equals("k")}");
        drawBtn.gameObject.SetActive(false);

        if (mode.Equals(AppData.FLIP))
            ltc = AppData.GetFilpName(data["ltc"], isFlipStatus);
        else
            ltc = data["ltc"];

        isClickSpecificBtn = isChooseSpecific = false;
        speceficHelpInfo.DOScale(0, .3f);

        if (ChooseColorPanel.activeInHierarchy) ChooseColorText.SetActive(false);

        if (getCardColor(card).Equals("k"))
        {
            if (!ChooseColorPanel.activeInHierarchy && !isSpecific4Card)
            {
                colorBlackBg.DOFade(1, 0);
                CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.one, true, false);
            }
            ProfilePanel.instance.ProfilePanelClick(10);

            int color;

            if (mode.Equals(AppData.FLIP))
            {
                color = isFlipStatus ?
                         (data["cs"] == "o" ? 0 : data["cs"] == "t" ? 1 : data["cs"] == "j" ? 2 : 3) :
                        (data["cs"] == "r" ? 0 : data["cs"] == "g" ? 1 : data["cs"] == "b" ? 2 : 3);
            }
            else
                color = (data["cs"] == "r" ? 0 : data["cs"] == "g" ? 1 : data["cs"] == "b" ? 2 : 3);

            if (!isSpecific4Card)
                SelectColorAnimation(color, getCardValue(card) == 5);

            if (getCardValue(card) == 5) // Shield Card throw
            {
                int si = -1;

                if (data["si"].AsInt == BottomSeatIndex) si = 0;
                if (data["si"].AsInt == LeftSeatIndex) si = 1;
                if (data["si"].AsInt == TopSeatIndex) si = 2;
                if (data["si"].AsInt == RightSeatIndex) si = 3;

                if (si != -1)
                {

                    shildPlayerImg[si].DOScale(Vector3.one, 0.3f).SetDelay(0.3f).OnComplete(() =>
                    {
                        AudioManager.instance.audioSource.Stop();
                        AudioManager.instance.AudioPlay(AudioManager.instance.shield);
                        shildPlayerImg[si].DOScale(Vector3.zero, 0.3f).SetDelay(1);
                    });
                }
            }

        }

        else if (ChooseColorPanel.activeInHierarchy)
            CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.zero, false, false);

        if (getCardColor(card) != getCardColor(ltc) && !(getCardColor(card).Equals("k")))
            AudioManager.instance.AudioPlay(AudioManager.instance.colorChangeClip);

        Logger.NormalLog($"VV unoRemoveSI:: {data["unoRemoveSI"]} | si = {data["si"]} || b= {BottomSeatIndex} | l= {LeftSeatIndex} || t= {TopSeatIndex} || r= {RightSeatIndex}");

        if (data["unoRemoveSI"] != -1)
        {
            for (int i = 0; i < gtiRespoce.Count; i++) /// Remove without BG last catd
            {
                if (data["unoRemoveSI"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0);
                        SayUnoAnimation(BottomUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1);
                        SayUnoAnimation(LeftUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2);
                        SayUnoAnimation(TopUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3);
                        SayUnoAnimation(RightUnoTxt, false);
                    }
                    break;
                }
            }
        }

        List<string> dis = JsonConvert.DeserializeObject<List<string>>(data["discardCards"].ToString()); // DISCARD CARD CHECK LIST

        List<int> SkipAll = JsonConvert.DeserializeObject<List<int>>(data["SkipAll"].ToString()); // DISCARD CARD CHECK LIST

        bool isFlipCard = (data["UsersCards"].Count > 0); // GET IS FLIP CARD THROW

        string closeDeck = data["closedeck"];

        flipCardStore = data["opendeck"][0];

        if (isFlipCard)
        {
            UpdatedCardList[0].cardsString = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][BottomSeatIndex].ToString());
            UpdatedCardList[1].cardsString = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][LeftSeatIndex].ToString());
            UpdatedCardList[2].cardsString = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][TopSeatIndex].ToString());
            UpdatedCardList[3].cardsString = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][RightSeatIndex].ToString());
        }

        bool isShildHas = ((getCardColor(card).Equals("k") && getCardValue(card) == 5) && data["wildupcounter"].AsInt > 0);
        string isPreviousHas = data["wilduplastcard"];

        //////// EXplain
        bool isWild = getCardColor(data["c"]).Equals("k");
        bool isWildltc = getCardColor(ltc).Equals("k");
        bool isHintShow = false;

        // Hint Toot tip show handle
        isHintShow = HinToolTipSetData(data, isWild, isHintShow);

        Logger.Print($"isHintShow: {isHintShow} || wildUpStep: {PrefrenceManager.wildUpStep} | zeroCounter: {PrefrenceManager.zeroCounter} | zeroCounter : {PrefrenceManager.zeroCounter}");

        Logger.Print($"isShildHas :;;;;;;;;;;;;;;;;; {isShildHas}");

        if (data["wildupcounter"].AsInt > 0 || data["totalpenaltyCard"].AsInt > 0)
        {
            bool isup = data["wildupcounter"].AsInt > 0 ? true : false;
            StartCoroutine(WildPlusAnimation(isup ? data["wildupcounter"].AsInt : data["totalpenaltyCard"].AsInt, isup, data["si"].AsInt));
        }

        wildDummyShowObject.SetActive(data["wildupcounter"].AsInt > 0 && (getCardColor(card).Equals("k") && getCardValue(card) == 5));
        if ((getCardColor(ltc).Equals("k") && getCardValue(ltc) == 5) && data["wildupcounter"].AsInt > 0)
        {
            isShildHas = false;
            wildDummyShowObject.SetActive(false);
        }

        if (data["si"].AsInt == BottomSeatIndex)
        {
            isChallenge = false;

            if (data["te"] == 1) ShowTurnMissToolTip();
            if (KeepPlayPanel.activeInHierarchy) CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false);
            if (!isForgotUNO) mySideSayUno = false;

            BottomPlayerTurnRing.gameObject.SetActive(false);
            timeOutindex = data["te"];
            isKeepPopupOn = false;
            isUnoPressed = false;

            if (isFlipCard) throwIndex = BottomPlayerCard.IndexOf(data["opendeck"][0]); // FOR GWT THRO CARD INDEX
            else throwIndex = BottomPlayerCard.IndexOf(data["c"]); // FOR GWT THRO CARD INDEX

            Logger.Print($" ThrowIndex {throwIndex} | Timeout : {timeOutindex} = skipSi = {skipSi} || DISCARD COUNT: {data["discardCards"].Count}");

            if ((data["discardCards"].Count > 0))
            {
                DiscardSameSuitCards(BottomSeatIndex, dis, BottomPlayerCard); // DISCARD CARD CHECK
                if (timeOutindex == 1)
                {
                    var discard = CardDeckController.instance.playerData[0].myCards
          .FirstOrDefault(x => x.cardValue == data["c"]);
                    if (discard != null && discard.gameObject != null)
                        discard.gameObject.SetActive(false);

                }
            }

            if (!isFlipCard) BottomPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());

            BottomPlayerCardCounter.text = BottomPlayerCard.Count + "";
            isResGet = true;

            if (timeOutindex != 1 && !isPlayClickAnimTime && CardDeckController.instance.selectCard != null)
            {
                if (CardDeckController.instance.selectCard.isClicktoThrow) // Click to throw
                    MyNewCardThrowAnimation(CardDeckController.instance.selectCard, BottomThrowCard.transform, 35, 0,
                        data["isReverse"].AsBool, skipSi, data["cs"], (data["discardCards"].Count > 0), SkipAll, bottomFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);
                else // Swipe to throw
                {
                    GameObject animCopy = (MyCardGrid.transform.childCount == 1) ? MyCardGrid.transform.GetChild(0).gameObject : BottomThrowAnimCard;
                    CardThrowAnimation(BottomThrowAnimCard, BottomThrowCard, data["c"], animCopy, 35, 0,
                        data["isReverse"].AsBool, data["cs"], skipSi, (data["discardCards"].Count > 0), BottomSeatIndex, SkipAll, bottomFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);
                }
            }
            else // time out and play click
            {
                GameObject animCopy = (MyCardGrid.transform.childCount == 1) ? MyCardGrid.transform.GetChild(0).gameObject : BottomThrowAnimCard;
                CardThrowAnimation(BottomThrowAnimCard, BottomThrowCard, data["c"], animCopy, 35, 0, data["isReverse"].AsBool, data["cs"], skipSi,
                    (data["discardCards"].Count > 0), BottomSeatIndex, SkipAll, bottomFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);

                if (timeOutindex == 1) // time out and action card thow
                {
                    ShowSwapButtons(false);
                    ShowPlus4SpeceficButtons(false);
                    if (ChooseColorPanel.activeInHierarchy)
                        CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.zero, false, false);
                }
            }
        }
        else if (data["si"].AsInt == LeftSeatIndex)
        {
            LeftPlayerTurnRing.gameObject.SetActive(false);

            if ((data["discardCards"].Count > 0))
                DiscardSameSuitCards(LeftSeatIndex, dis); // DISCARD CARD CHECK

            if (!isFlipCard) LeftPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());

            LeftPlayerCardCounter.text = LeftPlayerCard.Count + "";
            CardThrowAnimation(LeftPlayerAnimCard, LeftThrowCard, data["c"], LeftPlayerAnimCardCopy, 10, 45,
                data["isReverse"].AsBool, data["cs"], skipSi, (data["discardCards"].Count > 0), -1, SkipAll, leftFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);
            if (isForgotUNO) ShowCatchUNO(false, 1);
            if (!isFlipCard)
                RedrawCard(LeftSeatIndex);
        }
        else if (data["si"].AsInt == TopSeatIndex)
        {
            TopPlayerTurnRing.gameObject.SetActive(false);
            throw2Index = TopPlayerCard.IndexOf(data["c"]);

            if ((data["discardCards"].Count > 0))
                DiscardSameSuitCards(TopSeatIndex, dis); // DISCARD CARD CHECK

            if (!isFlipCard) TopPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());

            TopPlayerCardCounter.text = TopPlayerCard.Count + "";
            CardThrowAnimation(TopPlayerAnimCard, TopThrowCard, data["c"], TopPlayerAnimCardCopy, 10, 10,
                data["isReverse"].AsBool, data["cs"], skipSi, (data["discardCards"].Count > 0), -1, SkipAll, topFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);

            if (isForgotUNO)
                ShowCatchUNO(false, 2);

            if (!isFlipCard)
                RedrawCard(TopSeatIndex);
        }
        else if (data["si"].AsInt == RightSeatIndex)
        {
            RightPlayerTurnRing.gameObject.SetActive(false);

            if ((data["discardCards"].Count > 0))
                DiscardSameSuitCards(RightSeatIndex, dis); // DISCARD CARD CHECK

            if (!isFlipCard) RightPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());

            RightPlayerCardCounter.text = RightPlayerCard.Count + "";
            CardThrowAnimation(RightPlayerAnimCard, RightThrowCard, data["c"], RightPlayerAnimCardCopy, 10, -45, data["isReverse"].AsBool, data["cs"], skipSi,
                (data["discardCards"].Count > 0), -1, SkipAll, rightFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);
            if (isForgotUNO) ShowCatchUNO(false, 3);

            if (!isFlipCard)
                RedrawCard(RightSeatIndex);
        }
    }

    bool issaveHint = false;

    private bool HinToolTipSetData(JSONNode data, bool isWild, bool isHintShow)
    {
        if (isWild && getCardValue(data["c"]) == 5) // Shield
        {
            isHintShow = PrefrenceManager.shieldStepCount < 5;
            if (PrefrenceManager.shieldStepCount < 5)
                PrefrenceManager.shieldStepCount += 1;

            if (isHintShow)
            {
                string p1name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                string p2name = gtiRespoce[CheckTableSi(data["nextTunerSI"].AsInt)].getPn();
                Logger.Print($"p1 name: {p1name } | p2 name: {p2name }");
                Logger.Print($"p1 name: {CheckTableSi(data["si"].AsInt) } | p2 name: {GetPlayerSwapSi(data["si"].AsInt) }");

                hintActionToolText.text = $"<color=yellow>{p1name}</color> {TutorialActionSteps.shield_Hint} <color=yellow>{p2name}</color> takes the penalty cards instead.";
            }
        }
        else if ((isWild && getCardValue(data["c"]) == 4) || data["wildupcounter"].AsInt >= 2) // wild up
        {
            isHintShow = PrefrenceManager.wildUpStep < 5;
            if (PrefrenceManager.wildUpStep < 5 && data["wildupcounter"].AsInt == 1)
                PrefrenceManager.wildUpStep += 1;

            if (isHintShow)
            {
                string p1name = gtiRespoce[CheckTableSi(data["nextTunerSI"].AsInt)].getPn();
                string cs = data["cs"];
                hintActionToolText.text = $"<color=yellow>{p1name}</color> must play a {GetColorOnTutorial(cs)} | {TutorialActionSteps.wild_upHint}";
                Logger.Print($"isHintShow C :: color :: || cs : {cs}");
            }
        }
        else if ((getCardValue(data["c"]) == 0 && rules.Contains(6)) && !isWild) // Zero seven
        {
            isHintShow = PrefrenceManager.zeroCounter < 5;
            if (PrefrenceManager.zeroCounter < 5)
                PrefrenceManager.zeroCounter += 1;

            if (isHintShow)
            {
                string t2 = data[""].AsBool ? "Clockwise Direction." : "Anti Clockwise";
                hintActionToolText.text = $"{TutorialActionSteps.zeroHint} {t2}";
            }
        }
        else if (isWild && (getCardValue(data["c"]) == 6) && data["si"].AsInt == BottomSeatIndex) // spcific
        {
            isHintShow = PrefrenceManager.spcificPlayerStepCount < 5;
        }

        if (mode.Equals(AppData.CLASSIC) && rules.Count == 0)
            if (!hintActionToolTip.gameObject.activeInHierarchy)
            {
                if (getCardValue(data["c"]) == 10 && PrefrenceManager.skipShow == 0) // Skip
                {
                    issaveHint = true;
                    isHintShow = PrefrenceManager.skipShow == 0;
                    PrefrenceManager.skipShow = 1;
                    hintActionToolText.text = $"{TutorialActionSteps.skip}";
                }

                else if (getCardValue(data["c"]) == 11 && PrefrenceManager.reverseShow == 0) // reverse
                {
                    issaveHint = true;
                    isHintShow = PrefrenceManager.reverseShow == 0;
                    PrefrenceManager.reverseShow = 1;
                    string rev = data["isReverse"].AsBool ? "anticounterclockwise" : "counterclockwise";
                    hintActionToolText.text = $"{TutorialActionSteps.reverse} {rev} direction.";
                }

                else if (getCardValue(data["c"]) == 19 && PrefrenceManager.discardAllShow == 0) // discardall
                {
                    if (data["si"] == BottomSeatIndex)
                    {
                        issaveHint = true;
                        isHintShow = PrefrenceManager.discardAllShow == 0;
                        PrefrenceManager.discardAllShow = 1;

                        string cs = data["cs"];
                        hintActionToolText.text = $"Discard all {GetColorOnTutorial(cs)} {TutorialActionSteps.discardAll}";
                    }
                }
                Invoke(nameof(FlaseFlagInvoke), 1);
            }

        if (issaveHint)
        {
            hintActionToolTip.gameObject.SetActive(true);
        }
        else
            hintActionToolTip.gameObject.SetActive(isHintShow);
        Logger.Print($"isHintShow C :: {isHintShow}  | issaveHint : {issaveHint}");
        if (isWild && getCardValue(data["c"]) != 4 || data["wildupcounter"].AsInt == 0)
        {
            CancelInvoke(nameof(ShowHintToolTip));
            Invoke(nameof(ShowHintToolTip), 5);

        }

        return isHintShow;
    }

    private void FlaseFlagInvoke()
    {
        issaveHint = false;
    }

    private void ShowHintToolTip()
    {
        hintActionToolTip.gameObject.SetActive(false);
        issaveHint = false;
        Logger.Print($"isHintShow C :: false");
    }

    private IEnumerator WildPlusAnimation(int wildupcounter, bool isup, int si)
    {
        Logger.Print($"WildPlusAnimation ===> > Called {wildupcounter}");
        wildUpCounter.gameObject.SetActive(wildupcounter > 0);
        wildUpshiny.gameObject.SetActive(wildupcounter > 0);

        if (wildupcounter > 0)
        {
            if (isup)
            {
                bool isHintShow = PrefrenceManager.wildUpStep2 < 5;
                if (PrefrenceManager.wildUpStep2 < 5 && wildupcounter == 1)
                    PrefrenceManager.wildUpStep2 += 1;

                if (isHintShow)
                {
                    string p1name = gtiRespoce[CheckTableSi(si)].getPn();
                    hintActionToolText.text = $"<color=yellow>{p1name}</color> {TutorialActionSteps.wild_upHint2}";
                }
                hintActionToolTip.gameObject.SetActive(isHintShow);
            }

            // Show & scale
            wildUpCounter.transform.DOScale(Vector3.one * 1.1f, cardAnimDuration / 4).OnComplete(() => wildUpCounter.transform.DOScale(Vector3.one, cardAnimDuration / 4));
            wildUpCounter.text = isup ? $" {wildupcounter} UP" : $" +{wildupcounter}";

            if (wildupcounter >= 2 && !isup) // Arrow animation
                ArrowAnimation(true);
        }

        yield return new WaitForSeconds(0);

        wildUpAnimParticle.gameObject.SetActive(wildupcounter > 0 && isup);
        if (isup && (wildupcounter > 0 && wildupcounter <= 1))
        {
            var main = wildUpAnimParticle.main;
            var spk = sparks1.main;
            var spk2 = sparks2.main;

            main.startColor = new ParticleSystem.MinMaxGradient(new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, 0.7f));
            spk.startColor = new ParticleSystem.MinMaxGradient(new Color(spk.startColor.color.r, spk.startColor.color.g, spk.startColor.color.b, 0.7f));
            spk2.startColor = new ParticleSystem.MinMaxGradient(new Color(spk2.startColor.color.r, spk2.startColor.color.g, spk2.startColor.color.b, 0.7f));

            DOTween.To(() => main.startColor.color.a, x => main.startColor = new ParticleSystem.MinMaxGradient(new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, x)), 0.7f, 0f);
            DOTween.To(() => spk.startColor.color.a, x => spk.startColor = new ParticleSystem.MinMaxGradient(new Color(spk.startColor.color.r, spk.startColor.color.g, spk.startColor.color.b, x)), 0.7f, 0f);
            DOTween.To(() => spk2.startColor.color.a, x => spk2.startColor = new ParticleSystem.MinMaxGradient(new Color(spk2.startColor.color.r, spk2.startColor.color.g, spk2.startColor.color.b, x)), 0.7f, 0f);


            Logger.Print($"wildUpAnimParticle ===> > Called {wildUpAnimParticle.gameObject.activeInHierarchy}");

            if (wildupcounter > 0 && wildupcounter <= 1)
            {
                yield return new WaitForSeconds(1f);
                AudioManager.instance.AudioPlay(AudioManager.instance.wildUp);
            }
        }
    }

    [SerializeField] Image[] wildUpArrows;

    void ArrowAnimation(bool status)
    {
        for (int i = 0; i < wildUpArrows.Length; i++)
        {
            var arrow = wildUpArrows[i];

            if (status)
            {
                arrow.gameObject.SetActive(true);
                arrow.DOFade(1, 0);
                arrow.transform.localPosition = new Vector2(UnityEngine.Random.Range(-200, 200), -400);
                arrow.transform.DOLocalMoveY(300, 1f).SetDelay(i * 0.15f);
                arrow.DOFade(0, 1f).SetEase(Ease.InSine).SetDelay(i * 0.15f);
            }
            else
            {
                arrow.gameObject.SetActive(false);
                Color imageColor = new Color(255, 255, 255, 255);
                arrow.color = imageColor;
            }

        }
    }

    internal void MyPlayerCardThrowAnimation(CardController selectCard, Transform Destination, bool wild, bool isSymbolcard = false)// Event before 
    {
        cardAnimDuration = 0.6f;
        selectCard.glowImg.gameObject.SetActive(false);
        Logger.Print(TAG + "  MyPlayerCard Destination Is True Now >1> " + Destination.name);

        if (isPlayingHint)
        {
            bool colorMatch = PrefrenceManager.colorChangeStatus == 0 && selectedCard.Any(x => getCardColor(x) == getCardColor(selectCard.cardValue));
            bool numberMatch = PrefrenceManager.matchNumberStatus == 0 && selectedCard.Any(x => getCardValue(x) == getCardValue(selectCard.cardValue));
            if (colorMatch)
            {
                PrefrenceManager.colorChangeStatus = 1;
            }
            else if (numberMatch)
            {
                PrefrenceManager.matchNumberStatus = 1;
            }
            PrefrenceManager.clickSwipeStatus = 1;
            isPlayingHint = false;
            playingHandTutorial.gameObject.SetActive(false);
            playingClickHand.gameObject.SetActive(false);
            ShowHintToolTip();
        }

        var vec = new Vector2(97, 5);
        if (BottomThrowCard.transform.localPosition.x != vec.x || BottomThrowCard.transform.localPosition.y != vec.y)
        {
            Vector2 allpos = new Vector2(97, 5);
            BottomThrowCard.transform.localPosition = LeftThrowCard.transform.localPosition = TopThrowCard.transform.localPosition = RightThrowCard.transform.localPosition = allpos;
        }

        isResGet = false;
        Sequence animationSequence = DOTween.Sequence();
        selectCard.myRect.DOSizeDelta(new Vector2(195, 260), cardAnimDuration / 2);

        animationSequence.Append(selectCard.transform.DOMove(Destination.transform.position, cardAnimDuration / 2));
        animationSequence.Join(selectCard.transform.DOScale(new Vector3(0.85f, 0.85f, 1f), cardAnimDuration / 4));
        animationSequence.Append(selectCard.transform.DOScale(Vector3.one, cardAnimDuration / 5));
        animationSequence.Join(selectCard.transform.DORotate(new Vector3(35, 0, 0), cardAnimDuration / 2));
        animationSequence.OnComplete(() =>
        {
            Logger.Print(TAG + "  MyPlayerCard Destination Is True Now >> " + Destination.name + " value " + getCardValue(selectCard.cardValue));
            Logger.Print(TAG + $"  MyPlayerCard Destination Is True Now >> {(getCardValue(selectCard.cardValue) != 14 && (rules.Contains(6) && getCardValue(selectCard.cardValue) != 7))}");
            if (CardDeckController.instance.trappedCard != null)
                DestroyImmediate(CardDeckController.instance.trappedCard.gameObject);


            bool isHintShow = false;
            string valuecard = "";
            bool isWildPlusHint = (wild && getCardValue(selectCard.cardValue) == 1) && PrefrenceManager.fourPlusOwnShow == 0;
            bool isWilcolorHint = (wild && getCardValue(selectCard.cardValue) == 0) && PrefrenceManager.colorChooseShow == 0;

            valuecard = selectCard.cardValue;

            if (selectCard.cardValue.Contains("k-6")) //isSpecific4Card
            {
                isSpecific4Card = true;

                // TODO: Show +4 Button Show
                ShowPlus4SpeceficButtons(true);

                // is velid show hint!!!!!                
                hintActionToolText.text = $"{TutorialActionSteps.sp_PlThrow}";

                isHintShow = PrefrenceManager.spcificPlayerStepCount < 5;
                if (PrefrenceManager.spcificPlayerStepCount < 5)
                    PrefrenceManager.spcificPlayerStepCount += 1;

            }
            else if (isWildPlusHint)
            {
                isHintShow = PrefrenceManager.fourPlusOwnShow == 0;
                PrefrenceManager.fourPlusOwnShow = 1;
                hintActionToolText.text = $"{TutorialActionSteps.wildplus4Own}";
            }
            else if (isWilcolorHint)
            {
                isHintShow = PrefrenceManager.colorChooseShow == 0;
                PrefrenceManager.colorChooseShow = 1;
                hintActionToolText.text = $"{TutorialActionSteps.wildcolor}";
            }

            bool isFlipcardflag = getCardValue(selectCard.cardValue) == 14;
            bool isSevFlag = rules.Contains(6) && (getCardValue(selectCard.cardValue) == 7);
            bool isZeroFlag = rules.Contains(6) && (getCardValue(selectCard.cardValue) == 0);

            if (!wild && !isSymbolcard)
            {
                if ((rules.Contains(6) && (getCardValue(selectCard.cardValue) == 7))) //  0-7 mode hoi
                {
                    Destination.gameObject.GetComponent<Image>().sprite = IsFlipModeSprite(selectCard.cardValue, false);
                    Destination.gameObject.SetActive(true);
                    Destination.transform.SetSiblingIndex(4);
                }

                if (selectCard != null)
                    CardDeckController.instance.RemoveSelfPlayerCard(selectCard.gameObject);


                if (isSevFlag)
                {
                    // is velid show hint!!!!!                
                    hintActionToolText.text = $"{TutorialActionSteps.sevenHint}";

                    isHintShow = PrefrenceManager.sevenSelectCounter < 5;
                    if (PrefrenceManager.sevenSelectCounter < 5)
                        PrefrenceManager.sevenSelectCounter += 1;
                }
                Logger.NormalLog($" ===>>> AfterResRedraw is Called | {(isFlipcardflag || isZeroFlag || isSevFlag)} | Bcz Flipcardflag is = {isFlipcardflag} | And ZeroSevFlag is | {isZeroFlag} | & | {isSevFlag}.");

                if (isFlipcardflag || isZeroFlag || isSevFlag) // is this Flip card not || 0-7 mode no hoi
                { }
                else
                    StartCoroutine(AfterResRedraw());
            }

            //if (getCardValue(valuecard) != 0 || getCardValue(valuecard) != 10 || getCardValue(valuecard) != 11)
            if (isSpecific4Card || isSevFlag || isWildPlusHint || isWilcolorHint)
            {
                CancelInvoke(nameof(ShowHintToolTip));
                hintActionToolTip.gameObject.SetActive(isHintShow);
                Logger.Print($"isHintShow C :: {isHintShow}");
            }

            CardDeckController.instance.TrappCardDestroy();
            CardDeckController.instance.SelectCardChildDestroy();
        });
    }

    string flipCardStore = "";
    bool isResGet = false;

    IEnumerator AfterResRedraw()
    {
        Logger.Print($"AfterResRedraw Called >>>>>>>>>");
        yield return new WaitUntil(() => isResGet);
        Logger.Print($"Afer Get Res Redraw");
        RedrawCard(BottomSeatIndex);
    }

    void MyNewCardThrowAnimation(CardController selectCard, Transform destination, int xRotation, int zRotation, bool isReverse, int skipSI, string cs,
        bool isDiscardCard, List<int> skipAll = null, GameObject flipTag = null, bool isThrowFlipCard = false, string closeDeck = "", bool isShildHas = false, string isPreviousCStr = "") // Event res Click 
    {
        cardAnimDuration = 0.6f;
        var selectRect = selectCard.GetComponent<RectTransform>();
        var destiRect = destination.GetComponent<RectTransform>();
        CardController cValue = selectCard;

        MyTurn = false;
        selectCard.glowImg.gameObject.SetActive(false);
        selectRect.DORotate(new Vector3(xRotation, 0, zRotation), cardAnimDuration / 2);
        selectRect.DOSizeDelta(destiRect.sizeDelta, 0.2f).SetEase(Ease.Linear);
        selectRect.DOScale(new Vector3(.85f, 0.85f, .85f), cardAnimDuration / 2).SetEase(Ease.Linear);

        Vector3 vector = (isDiscardCard) ? symbolStaticCard.transform.position : destination.transform.position;
        //Vector3 vector = (isDiscardCard) ? symbolStaticCard.transform.localPosition : destination.transform.position;
        Logger.Print($" vector : {vector} Click | cardAnimDuration : {cardAnimDuration}");

        string cardValue = IsFlipModeCardStr(cValue.cardValue);
        Sprite spr = IsFlipModeSprite(cValue.cardValue, isFlipStatus);

        Logger.Print(TAG + $" selectCard >> {selectCard.gameObject.activeInHierarchy} cardAnimDuration: {cardAnimDuration}");

        //selectCard.transform.DOLocalMove(vector, cardAnimDuration / 2).OnComplete(() =>
        selectCard.transform.DOMove(vector, cardAnimDuration / 2).OnComplete(() =>
        {
            Logger.Print($"{TAG} || Card Value ????????????? = || selectCard {cValue.cardValue} seatIndex = {BottomSeatIndex} cs {cs} || cc {getCardValue(cValue.cardValue).Equals("13")} | isActive {symbolStaticCard.gameObject.activeInHierarchy}");
            if (skipSI == LeftSeatIndex)
                SkipTurnAnimation(skipImgObj, LeftPlayerImg.transform);
            else if (skipSI == RightSeatIndex)
                SkipTurnAnimation(skipImgObj2, RightPlayerImg.transform);

            if (getCardValue(cardValue) == 11)
                ReverseCardAnimation(cs, isReverse ? 417f : -303f);

            else if (getCardValue(cardValue) == 17) // +1 plus card throw
            {
                plusAnimImg.sprite = getCardColor(cardValue) == "r" ? pRed :
                                     getCardColor(cardValue) == "b" ? pblue :
                                     getCardColor(cardValue) == "g" ? pgreen :
                                     getCardColor(cardValue) == "y" ? pyellow : null; // Handle unexpected color

                plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
                plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
            }

            else if (getCardValue(cardValue) == 12) // 2+ plus card throw
            {
                plusAnimImg.sprite = getCardColor(cardValue) == "r" ? plus2Icons[0] :
                                     getCardColor(cardValue) == "g" ? plus2Icons[1] :
                                     getCardColor(cardValue) == "b" ? plus2Icons[2] :
                                     getCardColor(cardValue) == "y" ? plus2Icons[3] : null; // Handle unexpected color
                plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
                plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
            }

            else if (rules.Contains(7) && getCardValue(cardValue) == 5 && !getCardColor(cardValue).Equals("k")) // Highfive
            {
                highFiveHands[0].gameObject.SetActive(true);
            }

            else if (getCardColor(cardValue).Equals("k") && getCardValue(cardValue) == 1) //+4 Card throw
            {
                plusAnimImg.sprite = cs == "r" ? plus4Icons[0] :
                                     cs == "g" ? plus4Icons[1] :
                                     cs == "b" ? plus4Icons[2] :
                                     cs == "y" ? plus4Icons[3] : null; // Handle unexpected color
                plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
                plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
            }

            else if (isThrowFlipCard)
            {
                StartCoroutine(UserCardFlip(flipCardStore, cardValue, closeDeck));
                spr = IsFlipModeSprite(flipCardStore, !isFlipStatus);
            }

            CenterGlowCard.reverse = isReverse;
            CenterGlowCard.SetRingColor(cs);
            Logger.Print($"{TAG} || Card isActive {symbolStaticCard.gameObject.activeInHierarchy}");
            CardDeckController.instance.RemoveSelfPlayerCard(CardDeckController.instance.selectCard.gameObject);
            CardDeckController.instance.TrappCardDestroy();
            CardDeckController.instance.AddSelfPlayerCard();
            CardDeckController.instance.SelectCardChildDestroy();

            if (skipAll != null)
                if (skipAll.Count > 0)
                {
                    StartCoroutine(MegaBlockAnimation(skipAll));
                }

            Sprite s = null;
            if (getCardColor(cardValue).Equals("k"))
            {
                s = WildGetSprite(cs, cardValue, s);
            }
            else
                s = spr; // Default sprite

            destination.gameObject.GetComponent<Image>().sprite = s;

            flipTag.SetActive(mode.Equals(AppData.FLIP));

            if (isShildHas) // wild up + shield
            {
                Sequence seq = DOTween.Sequence();
                // shild move down
                seq.Append(destination.transform.DOMoveY(-1.5f, 0.5f));
                seq.Join(destination.transform.DOScale(1.1f, 0.5f / 2));
                seq.Join(destination.transform.DOScale(1f, 0.5f / 2).SetDelay(0.5f / 2f));

                // next object
                seq.Append(destination.transform.parent.GetChild(3).transform.DOMove(wildDummyShowObject.transform.position, 0.5f));
                destination.transform.parent.GetChild(3).transform.GetComponent<Image>().sprite = IsFlipModeSprite(isPreviousCStr, false);

                // 
                seq.Append(destination.transform.DOLocalMoveY(5, 0.5f));
            }

            if (isDiscardCard)
            {
                symbolStaticCard.sprite = GetSprite(cValue.cardValue);
                symbolStaticCard.gameObject.SetActive(isDiscardCard);
                StartCoroutine(DiscardAllActionAnimation(destination.gameObject, BottomSeatIndex));
            }
            else
            {
                destination.transform.SetSiblingIndex(4);
                destination.gameObject.SetActive(true);
            }
        });
        Logger.Print($"{TAG} || Card Value = || selectCard {selectCard.cardValue} isActive {symbolStaticCard.gameObject.activeInHierarchy} ");
    }

    public void CardThrowAnimation(GameObject AnimCard, GameObject Destination, string c, GameObject AnimCopy, int xRotation, int zRotation, bool isReverse,
        string cs, int skipSI, bool isDiscardCard, int isMySi = -1, List<int> skipAll = null, GameObject flipTag = null, bool isThrowFlipCard = false, string closeDeck = "",
        bool isShildHas = false, string isPreviousCStr = "")
    {
        Logger.Print(TAG + $" Throwable Card Throw Animation Called For : >>  {AnimCard.name} >> To Destination : >> {Destination.name} >> Anim Copy : {AnimCopy.name} >> c: {c} | isShildHas {isShildHas}");

        Sprite spr = IsFlipModeSprite(c, isFlipStatus);

        if (gameIsInBackground)
        {
            if (AnimCard == BottomThrowAnimCard) RedrawCard(BottomSeatIndex);
            if (AnimCard == TopPlayerAnimCard && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) RedrawCard(TopSeatIndex);

            Destination.gameObject.GetComponent<Image>().sprite = spr;
            Destination.transform.SetSiblingIndex(4);
            Destination.SetActive(true);

            AnimCard.SetActive(false);
            //AnimCard.transform.rotation = AnimCopy.transform.rotation;
            AnimCard.transform.localScale = AnimCopy.transform.localScale;
            AnimCard.transform.position = AnimCopy.transform.position;

            CenterGlowCard.reverse = isReverse;
            CenterGlowCard.SetRingColor(cs);

            return;
        }

        Logger.Print(TAG + $" Throwable Card Animcopy Position >>>>> {AnimCopy.gameObject.transform.position} isPlayClickAnimTime : {isPlayClickAnimTime} anim {(AnimCard == BottomThrowAnimCard)} isDiscardCard ? {isDiscardCard}");
        //AnimCard.transform.rotation = AnimCopy.transform.rotation;
        AnimCard.transform.rotation = Quaternion.identity;
        AnimCard.transform.localScale = AnimCopy.transform.localScale;
        float delay = 0.6f;

        string cardValue = IsFlipModeCardStr(c);

        if (AnimCard == BottomThrowAnimCard)
        {
            if (timeOutindex != 0) // Time Out Throw  ===================<<<<<<<<<<<<
            {
                if (CardDeckController.instance.selectCard != null)
                {
                    Logger.NormalLog($"selectSprite : {CardDeckController.instance.selectCard.cardImage.sprite.name} == {spr}");

                    if (CardDeckController.instance.selectCard.cardImage.sprite == spr)
                        AnimCard.transform.position = CardDeckController.instance.selectCard.transform.position;
                    CardDeckController.instance.selectCard.gameObject.SetActive(false);
                }
                else
                {
                    cardAnimDuration = 0.6f;
                    Logger.NormalLog($"selectSprite ELSE :AnimCard name = {AnimCard.activeInHierarchy} ||throwIndex = {throwIndex} == {cardValue} | Child: {MyCardGrid.transform.childCount}");
                    //if (!c.Contains("k"))
                    if (MyCardGrid.transform.childCount < throwIndex)
                        if (MyCardGrid.transform.childCount > 0 && MyCardGrid.transform.GetChild(throwIndex).gameObject != null)
                            AnimCard.transform.position = MyCardGrid.transform.GetChild(throwIndex).transform.position;
                }
            }
            else       // Swipe Throw =================<<<<<<<<<<<<<<<
            {
                Logger.NormalLog($"isPlayClickAnimTime ELSE : {isPlayClickAnimTime} == {getCardColor(cardValue)} || cardAnimDuration::  {cardAnimDuration} | throwIndex: {throwIndex}");

                if (isPlayClickAnimTime)
                {
                    cardAnimDuration = 0.6f;
                    AnimCard.transform.position = MyCardGrid.transform.GetChild(throwIndex).transform.position;
                }
                else
                {
                    cardAnimDuration = 0f;
                }
            }
        }
        else
        {
            Logger.Print(TAG + " Throwable Card Opponent Throw >> " + AnimCard.transform.position + " CHECK : cs" + cs);
            cardAnimDuration = 0.6f;
            AnimCard.transform.position = AnimCopy.transform.position;
        }

        Vector3 vector = (isDiscardCard) ? symbolStaticCard.transform.position : Destination.transform.position;

        AnimCard.transform.DOMove(vector, cardAnimDuration / 2).SetDelay(delay / 3).OnStart(() =>
        {
            Logger.Print(TAG + $" u >> {Destination.name} cardAnimDuration: {cardAnimDuration} c {c}");

            AnimCard.SetActive(true);

            AnimCard.gameObject.GetComponent<Image>().sprite = isThrowFlipCard ? IsFlipModeSprite(flipCardStore, !isFlipStatus) : spr;
            //AnimCard.gameObject.GetComponent<Image>().sprite = spr;
            AnimCard.transform.DOScale(new Vector3(0.85f, 0.85f, 1f), cardAnimDuration / 4);

            if (!AppData.isTutorialPlay)
            {
                Logger.Print($"mythrow Check 1 = {AnimCard == BottomThrowAnimCard } || {isPlayClickAnimTime} ||  {getCardColor(cardValue).Equals("k")} || {timeOutindex == 1} ");
                Logger.Print($"mythrow Check 2 = {(AnimCard == BottomThrowAnimCard && isPlayClickAnimTime || getCardColor(cardValue).Equals("k") || timeOutindex == 1)} discardList = {discardList.Count}");
                if (AnimCard == BottomThrowAnimCard)
                {
                    if (AnimCard == BottomThrowAnimCard && isPlayClickAnimTime || getCardColor(cardValue).Equals("k") || timeOutindex == 1) // Play button click to work / Time Out
                    {
                        bool isZeroFlag = rules.Contains(6) && (getCardValue(cardValue) == 0);
                        bool isSevFlag = rules.Contains(6) && (getCardValue(cardValue) == 7);

                        Logger.Print($"mythrow Check 3 = {(discardList.Count != 0 || (isZeroFlag && !getCardColor(cardValue).Equals("k")) || isSevFlag)} || {(isZeroFlag && !getCardColor(cardValue).Equals("k")) } | {isSevFlag}");

                        if (discardList.Count != 0 || (isZeroFlag && !getCardColor(cardValue).Equals("k")) || isSevFlag)
                        { }
                        else
                            RedrawCard(BottomSeatIndex);
                    }
                }
            }

            if (AnimCard == TopPlayerAnimCard && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)))
                RedrawCard(TopSeatIndex);

            AnimCard.transform.DOScale(Vector3.one, cardAnimDuration / 5).SetDelay(cardAnimDuration / 4).OnComplete(() =>
            {
                Sprite s = CardPreformAction(AnimCard, cardValue, isReverse, cs, skipSI, isDiscardCard, skipAll, spr, cardValue, isThrowFlipCard, closeDeck);

                Destination.gameObject.GetComponent<Image>().sprite = s;

                flipTag.SetActive(mode.Equals(AppData.FLIP));

                if (isDiscardCard)
                {
                    symbolStaticCard.sprite = spr;
                    symbolStaticCard.gameObject.SetActive(isDiscardCard);
                    StartCoroutine(DiscardAllActionAnimation(Destination, isMySi));
                }
                else
                {
                    Destination.transform.SetSiblingIndex(4);
                    Destination.SetActive(true);

                    if (isShildHas) // wild up + shield
                    {
                        Sequence seq = DOTween.Sequence();
                        // shild move down
                        seq.Append(Destination.transform.DOMoveY(-1.5f, 0.5f));
                        seq.Join(Destination.transform.DOScale(1.1f, 0.5f / 2));
                        seq.Join(Destination.transform.DOScale(1f, 0.5f / 2).SetDelay(0.5f / 2f));

                        // next object
                        seq.Append(Destination.transform.parent.GetChild(3).transform.DOMove(wildDummyShowObject.transform.position, 0.5f));
                        Destination.transform.parent.GetChild(3).transform.GetComponent<Image>().sprite = IsFlipModeSprite(isPreviousCStr, false);

                        // 
                        seq.Append(Destination.transform.DOLocalMoveY(5, 0.5f));
                    }
                }

                Logger.Print(TAG + $" Throwable Destination Is True Now >> {Destination.name} {cardValue} AnimCard.name  | isShildHas: {isShildHas}");
                AnimCard.SetActive(false);
                CenterGlowCard.reverse = isReverse;
                CenterGlowCard.SetRingColor(cs);
            });
        });
    }

    bool isActionCyclonCard = false;// Temp

    private Sprite CardPreformAction(GameObject AnimCard, string c, bool isReverse, string cs, int skipSI, bool isDiscardCard, List<int> skipAll, Sprite spr, string cardValue, bool isThrowFlipCard, string closeDeck)
    {
        Logger.Print($"CardPreformAction : cs: {cs} | getCardColor : {getCardColor(c)} | getCardValue: {getCardValue(c)}");

        isActionCyclonCard = false;

        if (getCardValue(cardValue) == 11) //For Reverse Animation...
            ReverseCardAnimation(getCardColor(cardValue), isReverse ? 417f : -303f);

        else if (getCardValue(c) == 17) // +1 plus card throw
        {
            plusAnimImg.sprite = getCardColor(cardValue) == "r" ? pRed :
                                 getCardColor(cardValue) == "b" ? pblue :
                                 getCardColor(cardValue) == "g" ? pgreen :
                                 getCardColor(cardValue) == "y" ? pyellow : null; // Handle unexpected color

            plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
            plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
        }

        else if (getCardValue(c) == 12) // 2+ plus card throw
        {
            plusAnimImg.sprite = getCardColor(cardValue) == "r" ? plus2Icons[0] :
                                 getCardColor(cardValue) == "g" ? plus2Icons[1] :
                                 getCardColor(cardValue) == "b" ? plus2Icons[2] :
                                 getCardColor(cardValue) == "y" ? plus2Icons[3] : null; // Handle unexpected color

            plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
            plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
        }

        else if (getCardColor(c).Equals("k") && getCardValue(c) == 1) //+4 Card throw
        {
            plusAnimImg.sprite = cs == "r" ? plus4Icons[0] :
                                 cs == "g" ? plus4Icons[1] :
                                 cs == "b" ? plus4Icons[2] :
                                 cs == "y" ? plus4Icons[3] : null; // Handle unexpected color
            plusAnimImg.transform.DOScale(Vector2.one * 4, 0.5f).OnComplete(() => plusAnimImg.transform.DOScale(Vector2.zero, 0.5f));
            plusAnimGlow.transform.DOScale(Vector2.one * 3, 0.5f).OnComplete(() => plusAnimGlow.transform.DOScale(Vector2.zero, 0.5f));
        }

        else if (rules.Contains(7) && getCardValue(c) == 5 && !getCardColor(c).Equals("k"))
        {
            highFiveHands[0].gameObject.SetActive(true);
        }

        else if (getCardValue(cardValue) != 19 && isDiscardCard)
        {
            if (AnimCard == BottomThrowAnimCard)
                RedrawCard(BottomSeatIndex);
        }

        else if (isThrowFlipCard)
        {
            StartCoroutine(UserCardFlip(flipCardStore, cardValue, closeDeck));
            spr = IsFlipModeSprite(flipCardStore, !isFlipStatus);
        }

        else if (getCardValue(cardValue) == 18) // Cyclon 
        {
            isActionCyclonCard = true;
        }

        //For Skip Animation...
        {
            if (skipSI == BottomSeatIndex)
                SkipTurnAnimation(skipImgObj, BottomPlayerImg.transform);
            else if (skipSI == LeftSeatIndex)
                SkipTurnAnimation(skipImgObj, LeftPlayerImg.transform);
            else if (skipSI == TopSeatIndex)
                SkipTurnAnimation(skipImgObj2, TopPlayerImg.transform);
            else if (skipSI == RightSeatIndex)
                SkipTurnAnimation(skipImgObj2, RightPlayerImg.transform);
        }

        if (skipAll != null)
            if (skipAll.Count > 0)
            {
                StartCoroutine(MegaBlockAnimation(skipAll));
            }

        Sprite s = null;
        if (getCardColor(cardValue).Equals("k"))
        {
            s = WildGetSprite(cs, cardValue, s);
        }
        else
            s = spr; // Default sprite

        return s;
    }

    private Sprite WildGetSprite(string cs, string cardValue, Sprite s)
    {
        int index = -1;
        if (mode.Equals(AppData.FLIP))
        {
            if (isFlipStatus)
                if (getCardValue(cardValue) != 3)
                    switch (cs)
                    {
                        case "o": index = 4; break; // Orange
                        case "t": index = 5; break; // Teal
                        case "j": index = 6; break; // Jambli
                        case "p": index = 7; break; // Purple
                    }
                else
                    switch (cs)
                    {
                        case "o": index = 8; break; // Orange
                        case "t": index = 9; break; // Teal
                        case "j": index = 10; break; // Jambli
                        case "p": index = 11; break; // Purple
                    }
            else
            {
                if (getCardValue(cardValue) == 2)
                    switch (cs)
                    {
                        case "r": index = 4; break; // +2 Red
                        case "g": index = 5; break; // +2 Green
                        case "b": index = 6; break; // +2 Blue
                        case "y": index = 7; break; // +2 Yellow
                    }
                else
                    switch (cs)
                    {
                        case "r": index = 0; break; // Red
                        case "g": index = 1; break; // Green
                        case "b": index = 2; break; // Blue
                        case "y": index = 3; break; // Yellow
                    }
            }
        }
        else
            switch (cs)
            {
                case "r": index = 0; break; // Red
                case "g": index = 1; break; // Green
                case "b": index = 2; break; // Blue
                case "y": index = 3; break; // Yellow
            }

        Logger.Print($"{TAG} || WildGetSprite:: {index} || cs: {cs} | cardValue :{cardValue} ");
        isSpecific4Card = false;

        Logger.Print($"{TAG} | WildGetSprite isSpecific4Card = {isSpecific4Card}");
        if (index != -1)
        {
            switch (getCardValue(cardValue))
            {
                case 0:
                    s = wildAllColor[index];
                    break;
                case 1:
                    s = wildPlusAllColor[index];
                    break;
                case 2:
                case 3:
                    s = IsFlipModeSprite(cardValue, isFlipStatus);
                    break;
                case 4:
                    s = wildUpColor[index];
                    break;
                case 5:
                    s = wildShieldColor[index];
                    break;
                case 6:
                    s = wildSpesificColor[index];
                    break;
            }
        }
        return s;
    }

    #region //========================= ACTION CARD FUNCTIONALITY =========================//

    int GetPlayerSwapSi(int seatIndex)
    {
        if (seatIndex == BottomSeatIndex) return 0;
        if (seatIndex == LeftSeatIndex) return 1;
        if (seatIndex == TopSeatIndex) return 2;
        if (seatIndex == RightSeatIndex) return 3;
        return -1; // Invalid seat index
    }

    private void HandleZeroSeven(JSONNode data)
    {
        // Get seat indices
        int seat1 = GetPlayerSwapSi(data["mysi"].AsInt);
        int seat2 = GetPlayerSwapSi(data["swapersi"].AsInt);

        Logger.Print($"HandleZeroSeven:: seat1: {seat1} | seat2: {seat2}");
        Logger.Print($"HandleZeroSeven:: Total Player Si Check: b = {BottomSeatIndex} |  l = {LeftSeatIndex} | t = {TopSeatIndex} | r = {RightSeatIndex} |");

        void AssignPlayerCards(int seatIndex, string cardData)
        {
            var cards = JsonConvert.DeserializeObject<List<string>>(cardData);
            switch (seatIndex)
            {
                case var _ when seatIndex == BottomSeatIndex:
                    BottomPlayerCard = cards;
                    break;

                case var _ when seatIndex == LeftSeatIndex:
                    LeftPlayerCard = cards;
                    break;

                case var _ when seatIndex == TopSeatIndex:
                    TopPlayerCard = cards;
                    break;

                case var _ when seatIndex == RightSeatIndex:
                    RightPlayerCard = cards;
                    break;
            }
        }

        if (data["isseven"].AsBool)
        {
            // Assign main players card
            AssignPlayerCards(data["mysi"].AsInt, data["cards"].ToString());
            AssignPlayerCards(data["swapersi"].AsInt, data["swapercards"].ToString());

            //sevenHint2
            bool isHintShow = PrefrenceManager.sevenCounter < 5;
            if (PrefrenceManager.sevenCounter < 5)
                PrefrenceManager.sevenCounter += 1;

            if (isHintShow)
            {
                string p1name = gtiRespoce[CheckTableSi(data["mysi"].AsInt)].getPn();
                string p2name = gtiRespoce[CheckTableSi(data["swapersi"].AsInt)].getPn();
                hintActionToolText.text = $"<color=yellow>{p1name}</color> chooses <color=yellow>{p2name}</color> to swap cards with him.";
            }

            hintActionToolTip.gameObject.SetActive(isHintShow);
            Logger.Print($"isHintShow C :: {isHintShow}");
            CancelInvoke(nameof(ShowHintToolTip));
            Invoke(nameof(ShowHintToolTip), 5);
        }
        else
        {
            // Assign cards from UsersCards array
            BottomPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][BottomSeatIndex].ToString());
            LeftPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][LeftSeatIndex].ToString());
            TopPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][TopSeatIndex].ToString());
            RightPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["UsersCards"][RightSeatIndex].ToString());

            HideUnoAnimation(BottomUnoTxt);
            HideUnoAnimation(LeftUnoTxt);
            HideUnoAnimation(TopUnoTxt);
            HideUnoAnimation(RightUnoTxt);
        }

        string firstCard = data["cards"][0];
        bool isSevenCard = data["isseven"].AsBool;

        Logger.Print($" ====>>>>>> isSevenCard : {isSevenCard}");
        StartCoroutine(ZeroSevenSwapCard(data["isReverse"].AsBool, !isSevenCard, seat1, seat2, firstCard, data["mysi"].AsInt, data["swapersi"].AsInt));
    }

    private IEnumerator ZeroSevenSwapCard(bool isReverse, bool isZeroCard, int swapIndex1, int swapIndex2, string cardValue, int swapRedrawIndex1, int swapRedrawIndex2)
    {
        Logger.Print($"ZeroSevenSwapCard ==> reverse : {isReverse} | isZeroCard: {isZeroCard}");

        int index = isZeroCard ? (isReverse ? 2 : 0) : -1;

        yield return new WaitForSeconds(0.5f);

        yield return CollectCardsToCenter(isZeroCard, swapIndex1, swapIndex2);

        Logger.Print($"ZeroSevenSwapCard ==> index : {index}");

        // Move cards to new positions
        yield return MoveCardsToNewPositions(isZeroCard, isReverse, index, swapIndex1, swapIndex2, cardValue);

        // Redraw the cards after movement
        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();

        if (swapRedrawIndex1 == BottomSeatIndex || swapRedrawIndex2 == BottomSeatIndex || isZeroCard)
        {
            Logger.Print($"My player card Reset Position ==> count : { MyCardGrid.transform.childCount}");
            if (BottomPlayerCard.Count > 0)
                for (int j = 0; j < CardDeckController.instance.playerData[0].myCards.Count; j++) // Self card reset 
                {
                    CardController myCard = CardDeckController.instance.playerData[0].myCards[j];
                    myCard.transform.position = CardDeckController.instance.playerData[0].myCardPos.position;
                    myCard.cardImage.sprite = cardSprite.sprite;
                    myCard.myRect.DOSizeDelta(new Vector2(148, 196), 0.3f);
                }

            for (int i = MyCardGrid.transform.childCount - 1; i >= 0; i--)
            {
                if (i != 0) DestroyImmediate(MyCardGrid.transform.GetChild(i).gameObject);
            }
        }

        yield return new WaitForSeconds(0.5f);
        leftGrid.enabled = topGrid.enabled = rightGrid.enabled = true;

        if (isZeroCard)
            for (int i = 0; i < 4; i++)
            {
                RedrawCard(i);
            }
        else
        {
            Logger.NormalLog($"Redraw Calling si===>> Globle si swapRedrawIndexFirst :: {swapRedrawIndex1} || swapRedrawIndexsec: {swapRedrawIndex2} ");
            RedrawCard(swapRedrawIndex1);
            RedrawCard(swapRedrawIndex2);
        }
    }

    /// <summary>
    /// Collect all player cards to the center for animation.
    /// </summary>
    private IEnumerator CollectCardsToCenter(bool isZeroCard, int swapIndex1, int swapIndex2)
    {
        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();
        if (isZeroCard)
        {
            leftGrid.enabled = topGrid.enabled = rightGrid.enabled = false;

            for (int i = 0; i < CardDeckController.instance.playerData.Count; i++)
            {
                var player = CardDeckController.instance.playerData[i];
                for (int j = 0; j < player.myCards.Count; j++)
                {
                    CardController myCard = player.myCards[j];
                    myCard.transform.DOMove(player.myCards[player.myCards.Count / 2].transform.position, 0.4f);
                    myCard.transform.eulerAngles = Vector3.zero;
                }
            }
        }
        else
        {
            // TODO: Implement the card collection for the specific player 
            // Implement card collection and move animations for 7 specific player swap

            var player = CardDeckController.instance.playerData[swapIndex1];
            var player2 = CardDeckController.instance.playerData[swapIndex2];

            List<PlayerController> tempPlayerList = new List<PlayerController> { player, player2 };

            for (int i = 0; i < tempPlayerList.Count; i++)
            {
                var players = tempPlayerList[i];
                Logger.Print($"players.myCards.Count = {players.myCards.Count}");
                for (int j = 0; j < players.myCards.Count; j++)
                {
                    try
                    {
                        CardController myCard = players.myCards[j];
                        if (myCard != null)
                        {
                            myCard?.transform.DOMove(players.myCards[players.myCards.Count / 2].transform.position, 0.3f);
                            myCard.transform.eulerAngles = Vector3.zero;
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.Print($"Ex = {ex}");
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.32f);
    }

    /// <summary>
    /// Move the cards to their new positions based on the direction and card type.
    /// </summary>
    private IEnumerator MoveCardsToNewPositions(bool isZeroCard, bool isReverse, int index, int swapIndex1, int swapIndex2, string cardValue)
    {
        float t = 0.6f;
        List<Image> cardSpr = new List<Image>();

        if (isZeroCard)
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.zeroClip);

            for (int i = 0; i < CardDeckController.instance.playerData.Count; i++)
            {
                var player = CardDeckController.instance.playerData[i];

                if (!isReverse && index == 3) index = -1;
                index = index + 1;

                // Move cards to the new player's position
                for (int j = 0; j < player.myCards.Count; j++)
                {
                    CardController myCard = player.myCards[j];

                    Logger.Print($"MoveCardsToNewPositions ==> i : {i} | index : {index}");

                    if (index == 0)
                        cardSpr.Add(myCard.cardImage);

                    myCard.transform.DOMove(CardDeckController.instance.playerData[index].myCardPos.position, t);
                    myCard.transform.DORotate(new Vector3(0, -90, 0), t / 2).OnComplete(() =>
                    {
                        if (LeftPlayerCard.Count > 0 && RightPlayerCard.Count > 0)
                            cardSpr.All(x => x.sprite = IsFlipModeSprite(isReverse ? LeftPlayerCard[0] : RightPlayerCard[0], false));
                        myCard.transform.DORotate(Vector3.zero, t / 2);
                    });

                    myCard.myRect.DOSizeDelta(new Vector2(74, 98), t);
                }

                if (isReverse && index == 3) index = -1;
                Logger.Print($"MoveCardsToNewPositions ==> index : {index}");
            }
        }
        else
        {
            var player = CardDeckController.instance.playerData[swapIndex1];
            var player2 = CardDeckController.instance.playerData[swapIndex2];

            AudioManager.instance.AudioPlay(AudioManager.instance.sevenClip);

            List<PlayerController> tempPlayerList = new List<PlayerController> { player, player2 };
            index = 2;

            for (int i = 0; i < tempPlayerList.Count; i++)
            {
                var players = tempPlayerList[i];
                index = index - 1;

                for (int j = 0; j < players.myCards.Count; j++)
                {
                    CardController myCard = players.myCards[j];

                    if (i == 1 && (swapIndex1 == BottomSeatIndex || swapIndex2 == BottomSeatIndex))
                        cardSpr.Add(myCard.cardImage);
                    if (myCard != null)
                    {

                        myCard.transform.DOMove(tempPlayerList[index].myCardPos.position, t);
                        myCard.transform.DORotate(new Vector3(0, -90, 0), t / 2).OnComplete(() =>
                        {
                            cardSpr.All(x => x.sprite = IsFlipModeSprite(cardValue, false));
                            myCard.transform.DORotate(Vector3.zero, t / 2);
                        });

                        myCard.myRect.DOSizeDelta(new Vector2(74, 98), t);
                    }
                }
            }
        }

        // Wait for the card movement to finish before continuing
        yield return new WaitForSeconds(0.7f);
        cardSpr.Clear();
    }

    private void HandleHighFive(JSONNode data)
    {
        Logger.Print($"HandleHighFive ==> si : {data["si"]}");
        int si = data["si"];

        int targetSeatIndex = si switch
        {
            0 => BottomSeatIndex,
            1 => LeftSeatIndex,
            2 => TopSeatIndex,
            3 => RightSeatIndex,
            _ => -1
        };

        AudioManager.instance.AudioPlay(AudioManager.instance.highfive);
        // Animation
        highFiveHands[targetSeatIndex].transform.SetSiblingIndex(3);
        highFiveHands[targetSeatIndex].gameObject.SetActive(true);
        highFiveHands[targetSeatIndex].sizeDelta = new Vector2(160, 160);
        highFiveHands[targetSeatIndex].DOMove(CenterGlowCard.transform.position, 0.6f);
        highFiveHands[targetSeatIndex].DOSizeDelta(new Vector2(350, 350), 0.6f).OnComplete(() =>
        {
            highFiveHands[targetSeatIndex].DOSizeDelta(new Vector2(300, 300), 0.6f).OnComplete(() =>
            {
                //highFiveHands[i].sizeDelta = new Vector2(160, 160);
                //highFiveHands[i].gameObject.SetActive(false);
                //highFiveHands[i].anchoredPosition = (i == 0) ? new Vector2(260, 160) : new Vector2(-40, -38);
            });
        });
    }

    bool isfiveSend;
    public void HighFiveHandClick(int i)
    {
        if (isfiveSend) return;
        isfiveSend = true;
        // TODO : Send Event
        EventHandler.SendHigh(i);
    }

    private void ShowHighFiveHand(bool status)
    {
        for (int i = 0; i < highFiveHands.Length; i++)
        {
            highFiveHands[i].gameObject.SetActive(status);
            if (!status)
            {
                highFiveHands[i].sizeDelta = new Vector2(160, 160);
                highFiveHands[i].gameObject.SetActive(false);

                highFiveHands[i].anchoredPosition = i switch
                {
                    0 => new Vector2(-350, -145),
                    1 => new Vector2(-705, 145),
                    2 => new Vector2(0, 375),
                    3 => new Vector2(730, 130),
                    _ => highFiveHands[i].anchoredPosition  // Default case if necessary, although this should never hit
                };
            }
        }
    }

    private void ShowPlus4SpeceficButtons(bool status)
    {
        Logger.Print($"ShowPlus4SpeceficButtons:: {status}");
        for (int i = 0; i < CardDeckController.instance.playerData.Count; i++)
        {
            if (i == 0) continue;
            var plus4 = CardDeckController.instance.playerData[i].specefic4Btn;
            var plus4child = CardDeckController.instance.playerData[i].specefic4Btn.GetChild(0).transform;
            if (status)
            {
                plus4.DOScale(1, cardAnimDuration / 2).OnComplete(() =>
                {
                    plus4child.DOScale(0.9f, cardAnimDuration / 2).SetLoops(-1, LoopType.Yoyo);
                });
            }
            else
                plus4.DOScale(0, cardAnimDuration / 2).OnComplete(() => DOTween.Kill(plus4child));
        }
    }

    private void ShowSwapButtons(bool status)
    {
        for (int i = 0; i < CardDeckController.instance.playerData.Count; i++)
        {
            if (i == 0) continue;
            var seven = CardDeckController.instance.playerData[i].swapBtn;
            var sevenchild = CardDeckController.instance.playerData[i].swapBtn.GetChild(0).transform;
            if (status)
            {
                seven.DOScale(1, cardAnimDuration / 2).OnComplete(() =>
                {
                    sevenchild.DOScale(0.9f, cardAnimDuration / 2).SetLoops(-1, LoopType.Yoyo);
                });
            }
            else
                seven.DOScale(0, cardAnimDuration / 3).OnComplete(() => DOTween.Kill(sevenchild));
        }
    }

    private IEnumerator MegaBlockAnimation(List<int> skipAll)
    {
        for (int k = 0; k < skipAll.Count; k++)
        {
            if (skipAll[k] == BottomSeatIndex)
                SkipTurnAnimation(skipImgObj, BottomPlayerImg.transform, true, (skipAll.Count == 2));
            if (skipAll[k] == LeftSeatIndex)
                SkipTurnAnimation(skipImgObj2, LeftPlayerImg.transform, true, (skipAll.Count == 2));
            if (skipAll[k] == RightSeatIndex)
                SkipTurnAnimation(skipImgObj3, RightPlayerImg.transform, true, (skipAll.Count == 2));
            if (skipAll[k] == TopSeatIndex)
                SkipTurnAnimation(skipImgObj4, TopPlayerImg.transform, true, (skipAll.Count == 2));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator UserCardFlip(string lastCard, string currantCard, string closeDeck)
    {
        yield return new WaitForSeconds(0.5f);
        float t = 0.1f;
        float duration = 0.3f;
        Logger.Print($"lastCard : {lastCard} || currantCard: {currantCard} || closeDeck: {closeDeck} ");

        // Define an array of cards
        var throwCards = new[] { BottomThrowCard, LeftThrowCard, TopThrowCard, RightThrowCard };

        AudioManager.instance.AudioPlay(isFlipStatus ? AudioManager.instance.darkFlip : AudioManager.instance.lightFlip);

        // Rotate each throw card
        foreach (var card in throwCards)
        {
            card.GetComponent<Image>().sprite = IsFlipModeSprite(lastCard, !isFlipStatus);

            card.transform.DORotate(new Vector3(0, -90, 0), duration)
                .OnComplete(() => card.GetComponent<Image>().sprite = IsFlipModeSprite(currantCard, isFlipStatus));

            card.transform.DORotate(new Vector3(35, 0, 0), duration).SetDelay(duration);
        }

        // CloseDeckCounter Text withbg
        CloseDeckCounter.transform.parent.transform.DORotate(new Vector3(0, -90, 0), duration);
        CloseDeckCounter.transform.parent.transform.DORotate(new Vector3(35, 0, 0), duration).SetDelay(duration);

        // Rotate close deck image
        closeDeckImg.transform.DORotate(new Vector3(0, -90, 0), duration);
        closeDeckImg.transform.DORotate(new Vector3(35, 0, 0), duration).SetDelay(duration);

        flipCloseDeckImg.transform.DORotate(new Vector3(0, -90, 0), duration)
            .OnComplete(() =>
            {
                flipCloseDeckImg.GetComponent<Image>().sprite = IsFlipModeSprite(closeDeck, !isFlipStatus);
                playinBG.sprite = playingBgSprites[isFlipStatus ? 1 : 0];
            });
        flipCloseDeckImg.transform.DORotate(new Vector3(35, 0, 0), duration).SetDelay(duration);

        Coroutine playerCardsAnimation = StartCoroutine(AnimatePlayerCards(BottomSeatIndex, t, UpdatedCardList[0].cardsString));
        Coroutine leftCardsAnimation = StartCoroutine(AnimatePlayerCards(LeftSeatIndex, t, UpdatedCardList[1].cardsString));
        Coroutine TopCardsAnimation = StartCoroutine(AnimatePlayerCards(TopSeatIndex, t, UpdatedCardList[2].cardsString));
        Coroutine RightCardsAnimation = StartCoroutine(AnimatePlayerCards(RightSeatIndex, t, UpdatedCardList[3].cardsString));

        // Wait for both animations to complete
        yield return playerCardsAnimation;
        yield return leftCardsAnimation;
        yield return TopCardsAnimation;
        yield return RightCardsAnimation;

        yield return new WaitForSeconds(0.3f);
        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();

        leftGrid.enabled = topGrid.enabled = rightGrid.enabled = true;

        for (int p = 0; p < 4; p++)
        {
            RedrawCard(p);
        }
    }

    private IEnumerator AnimatePlayerCards(int seatIndex, float t, List<string> updateCardList)
    {
        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();

        leftGrid.enabled = topGrid.enabled = rightGrid.enabled = false;

        if (seatIndex == BottomSeatIndex)
        {
            PlayerController player = CardDeckController.instance.playerData[0];

            foreach (var item in player.myCards)
            {
                item.myCanvasGroup.blocksRaycasts = true;
                item.cardImage.raycastTarget = true;
                item.cardImage.enabled = true;
                item.disableImg.gameObject.SetActive(false);
                item.SetCardGlowAnimation(false);
            }

            for (int i = 0; i < player.myCards.Count; i++)
            {
                CardController myCard = player.myCards[i];

                Sequence s = DOTween.Sequence();
                s.Append(myCard.transform.DOLocalRotate(new Vector3(0, -90, myCard.transform.localEulerAngles.z), t)
                    .OnComplete(() => myCard.cardImage.sprite = IsFlipModeSprite(BottomPlayerCard[i], isFlipStatus)));
                s.Append(myCard.transform.DOLocalRotate(new Vector3(0, 0, myCard.transform.localEulerAngles.z), t));

                yield return new WaitForSeconds(t);
            }

            for (int i = 0; i < player.myCards.Count; i++)
            {
                CardController myCard = player.myCards[i];
                myCard.transform.DOMove(player.myCards[(int)(player.myCards.Count / 2)].transform.position, t + 0.3f);
                myCard.transform.eulerAngles = Vector3.zero;
            }

            yield return new WaitForSeconds(t + 0.3f);

            BottomPlayerCard = updateCardList; // Update Card List
            //CardPositionSet.instance.UpdateCardPosition(true);

            yield return new WaitForSeconds(t + 0.3f);
        }
        else if (seatIndex == LeftSeatIndex)
        {
            for (int l = 0; l < leftPlayerCardGrid.transform.childCount; l++)
            {
                if (leftPlayerCardGrid.transform.GetChild(l).gameObject == null) continue;

                var left = leftPlayerCardGrid.transform.GetChild(l);
                var leftImg = leftPlayerCardGrid.transform.GetChild(l).GetComponent<Image>();

                Sequence s = DOTween.Sequence();
                s.Append(left.DOLocalRotate(new Vector3(0, -90, left.localEulerAngles.z), t).OnComplete(() => leftImg.sprite = IsFlipModeSprite(LeftPlayerCard[l], !isFlipStatus)));
                s.Append(left.DOLocalRotate(new Vector3(0, 0, left.localEulerAngles.z), t));

                yield return new WaitForSeconds(t);
            }

            for (int i = 0; i < leftPlayerCardGrid.transform.childCount; i++)
            {
                if (leftPlayerCardGrid.transform.GetChild(i).gameObject == null) continue;

                var top = leftPlayerCardGrid.transform.GetChild(i);
                top.transform.DOMove(leftPlayerCardGrid.transform.GetChild((int)leftPlayerCardGrid.transform.childCount / 2).transform.position, t + 0.3f);
            }

            yield return new WaitForSeconds(t + 0.3f);

            LeftPlayerCard = updateCardList; // Update Card List
        }
        else if (seatIndex == TopSeatIndex)
        {
            for (int l = 0; l < partnerCardGrid.transform.childCount; l++)
            {
                if (partnerCardGrid.transform.GetChild(l).gameObject == null) continue;

                var top = partnerCardGrid.transform.GetChild(l);
                var topImg = partnerCardGrid.transform.GetChild(l).GetComponent<Image>();

                Sequence s = DOTween.Sequence();
                s.Append(top.DOLocalRotate(new Vector3(0, -90, top.localEulerAngles.z), t).OnComplete(() => topImg.sprite = IsFlipModeSprite(TopPlayerCard[l], !isFlipStatus))); // If partner mod it's change!!!!
                s.Append(top.DOLocalRotate(new Vector3(0, 0, top.localEulerAngles.z), t));

                yield return new WaitForSeconds(t);
            }

            for (int i = 0; i < partnerCardGrid.transform.childCount; i++)
            {
                if (partnerCardGrid.transform.GetChild(i).gameObject == null) continue;

                var top = partnerCardGrid.transform.GetChild(i);
                top.transform.DOMove(partnerCardGrid.transform.GetChild((int)partnerCardGrid.transform.childCount / 2).transform.position, t + 0.3f);
            }

            yield return new WaitForSeconds(t + 0.3f);

            TopPlayerCard = updateCardList; // Update Card List
        }
        else if (seatIndex == RightSeatIndex)
        {
            Logger.Print($" RightSeatIndex : {rightPlayerCardGrid.transform.childCount} == {RightPlayerCard.Count}");
            for (int r = 0; r < rightPlayerCardGrid.transform.childCount; r++)
            {
                if (rightPlayerCardGrid.transform.GetChild(r).gameObject == null) continue;

                var right = rightPlayerCardGrid.transform.GetChild(r);
                var rightImg = rightPlayerCardGrid.transform.GetChild(r).GetComponent<Image>();

                Sequence s = DOTween.Sequence();
                s.Append(right.DOLocalRotate(new Vector3(0, -90, right.localEulerAngles.z), t).OnComplete(() => rightImg.sprite = IsFlipModeSprite(RightPlayerCard[r], !isFlipStatus)));
                s.Append(right.DOLocalRotate(new Vector3(0, 0, right.localEulerAngles.z), t));

                yield return new WaitForSeconds(t);
            }

            for (int i = 0; i < rightPlayerCardGrid.transform.childCount; i++)
            {
                if (rightPlayerCardGrid.transform.GetChild(i).gameObject == null) continue;

                var right = rightPlayerCardGrid.transform.GetChild(i);
                right.transform.DOMove(rightPlayerCardGrid.transform.GetChild((int)rightPlayerCardGrid.transform.childCount / 2).transform.position, t + 0.3f);
            }

            yield return new WaitForSeconds(t + 0.3f);

            RightPlayerCard = updateCardList; // Update Card List
        }
    }

    private void DiscardSameSuitCards(int si, List<string> discardStrList, List<string> playerCardList = null)
    {
        Logger.Print(TAG + " DiscardSameSuitCards called");

        if (si == BottomSeatIndex)
        {
            Logger.Print(TAG + $" playerCardList = {playerCardList.Count} == {discardStrList.Count} || p = {CardDeckController.instance.playerData[0].myCards.Count}");
            for (int i = 0; i < playerCardList.Count; i++)
            {
                for (int j = 0; j < discardStrList.Count; j++)
                {
                    if (playerCardList[i] == discardStrList[j])
                    {
                        discardList.Add(CardDeckController.instance.playerData[0].myCards[i].gameObject);
                        break;
                    }
                }
            }
        }
        else
        {
            Transform spawnpos = null;
            if (si == LeftSeatIndex) spawnpos = LeftPlayerImg.transform;
            else if (si == TopSeatIndex) spawnpos = TopPlayerImg.transform;
            else if (si == RightSeatIndex) spawnpos = RightPlayerImg.transform;

            if (spawnpos == null) return;

            for (int j = 0; j < discardStrList.Count; j++)
            {
                GameObject d = Instantiate(SpriteImg, spawnpos.position, Quaternion.identity);
                d.transform.SetParent(playingScreen.transform);
                d.transform.localScale = Vector3.zero;
                d.GetComponent<Image>().sprite = GetSprite(discardStrList[j]);
                discardList.Add(d);
            }
        }
    }

    private IEnumerator DiscardAllActionAnimation(GameObject centerCard, int si)
    {
        // Animation
        yield return new WaitForSeconds(cardAnimDuration);
        Logger.Print(TAG + $" DiscardAllActionAnimation Coroutine called isActive {symbolStaticCard.gameObject.activeInHierarchy}");

        Sequence animationSequence = DOTween.Sequence();

        float t = 0.4f;

        for (int c = 0; c < discardList.Count; c++)
        {
            animationSequence = DOTween.Sequence();

            discardList[c].transform.SetParent(playingScreen.transform);

            animationSequence.Append(discardList[c].transform.DOMove(centerCard.transform.position, t));
            animationSequence.Join(discardList[c].GetComponent<RectTransform>().DOSizeDelta(new Vector2(178, 238), t));
            animationSequence.Join(discardList[c].transform.DOScale(new Vector3(0.85f, 0.85f, 1f), t));
            animationSequence.Append(discardList[c].transform.DOScale(Vector3.one, t));
            animationSequence.Join(discardList[c].transform.DORotate(centerCard.transform.eulerAngles, t));

            yield return new WaitForSeconds(0.2f);
        }

        yield return animationSequence.Play().WaitForCompletion();

        Logger.Print(TAG + $" DiscardAllActionAnimation END?  {symbolStaticCard.gameObject.activeInHierarchy}");

        Sequence animationSequenceNew = DOTween.Sequence();
        animationSequenceNew.Append(symbolStaticCard.transform.DOMove(centerCard.transform.position, t));
        animationSequenceNew.Join(symbolStaticCard.transform.DORotate(centerCard.transform.eulerAngles, t));
        animationSequenceNew.Join(symbolStaticCard.transform.DOScale(Vector3.one * 1.5f, t / 2));
        animationSequenceNew.Append(symbolStaticCard.transform.DOScale(Vector3.one, t / 2).SetDelay(0.1f).SetEase(Ease.InFlash));
        animationSequenceNew.OnComplete(() =>
        {
            centerCard.transform.SetSiblingIndex(4);
            centerCard.SetActive(true);
            symbolStaticCard.gameObject.SetActive(false);
            symbolStaticCard.transform.localPosition = discardPosStore;
            Logger.Print(TAG + $" DiscardAllActionAnimation CARD MOVE END?  {symbolStaticCard.gameObject.activeInHierarchy}");
        });

        yield return new WaitForSeconds(0.2f);

        foreach (var item in discardList)
            Destroy(item.gameObject);

        discardList.Clear();

        if (si == BottomSeatIndex)
            RedrawCard(BottomSeatIndex);

        //symbolStaticCard.transform.position = discardPosStore;
    }

    List<Transform> stackPanelty = new List<Transform>();
    private IEnumerator StackCardAnimation(List<string> TakeCard, int si, int pi, bool isCyclonePenelty)
    {
        bool checkAnimComplete = false;
        if (isCyclonePenelty)
        {
            float ti = 0.6f;
            cycloneObject.gameObject.SetActive(true);
            cycloneFireWork.gameObject.SetActive(false);

            cycloneObject.transform.localPosition = Vector3.zero;
            cycloneParticle.localScale = Vector3.zero;
            cycloneImg.localScale = Vector3.zero;

            Sequence cyclone = DOTween.Sequence();
            cyclone.Append(cycloneParticle.DOScale(new Vector3(150, 100, 100), ti).SetEase(Ease.OutSine));
            cyclone.Join(cycloneImg.DOScale(Vector3.one, ti).SetEase(Ease.OutSine).SetDelay(0.2f).OnStart(() =>
            {
                cycloneImg.DOLocalRotate(Vector3.forward * 350, ti).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            }).OnComplete(() => AudioManager.instance.AudioPlay(AudioManager.instance.cycloneClip)));

            cyclone.Append(cycloneObject.DOJump(CardDeckController.instance.playerData[pi].profilePic.transform.position, 1, 1, ti).SetDelay(0.3f));
            cyclone.Join(cycloneImg.DOScale(Vector3.zero, ti).SetDelay(0.2f));
            cyclone.Join(cycloneParticle.DOScale(Vector3.zero, ti).SetDelay(.3f));
            cyclone.OnComplete(() =>
            {
                cycloneFireWork.gameObject.SetActive(true);
                DOTween.Kill(cycloneImg);
                cyclone.Kill();
                checkAnimComplete = true;
            });

            yield return new WaitForSeconds(0.3f);
        }
        else
            checkAnimComplete = true;

        yield return new WaitUntil(() => checkAnimComplete);

        PlayerController targetPos = CardDeckController.instance.playerData[pi];
        List<Tween> activeTweens = new List<Tween>();

        var spawnVector = targetPos.stackCardParent.transform.position;

        float gapValueX = (si == BottomSeatIndex || si == TopSeatIndex) ? 0.4f : 0f;
        float gapValueY = (si == BottomSeatIndex || si == TopSeatIndex) ? 0f : 0.4f;

        Logger.Print($"{TAG} | StackCardAnimation ==> gapValueX: {gapValueX} / gapValueY: {gapValueY}");

        for (int i = 0; i < TakeCard.Count; i++)
        {
            GameObject stackP = Instantiate(SpriteImg, targetPos.transform);
            stackP.GetComponent<Image>().sprite = IsFlipModeSprite(TakeCard[i], isFlipStatus);
            stackP.transform.position = backDeckTransform.transform.position;
            stackP.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 100);
            stackP.transform.eulerAngles = Vector3.zero;
            stackPanelty.Add(stackP.transform);
            AudioManager.instance.AudioPlay(AudioManager.instance.stackFlip);

            Tween moveTween = stackP.transform.DOMove(new Vector2(spawnVector.x + (gapValueX * i), spawnVector.y - (gapValueY * i)), 0.3f).OnComplete(() =>
            {
                stackP.transform.SetParent(targetPos.stackCardParent.transform);
            });
            activeTweens.Add(moveTween);

            yield return new WaitForSeconds(0.65f);
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var stack in stackPanelty)
        {
            stack.DOMove(targetPos.myCardPos.position, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);

        foreach (var stack in stackPanelty)
            Destroy(stack.gameObject);
        stackPanelty.Clear();

        RedrawCard(si);
        yield break;
    }

    public void PlayPlus4BtnClick(int index)
    {
        if (isDontTouch) return;
        isKeepPopupOn = false;
        mySideSayUno = false;
        AudioManager.instance.tikAudioSource.Stop();
        isPlayClickAnimTime = true;
        string cardclr = "";

        switch (index)
        {
            case 0: // PlayPlus4BtnClick
                Logger.Print($" PlayPlus4BtnClick....isCardplusHas: {isCardplusHas}");

                if (isCardplusHas)
                    cardclr = BottomPlayerCard.Find(x => x.Contains("k-1"));
                else
                {
                    cardclr = BottomPlayerCard.Find(x => x.Contains("k-6"));
                    isSpecific4Card = true;
                    ShowPlus4SpeceficButtons(true);
                }

                Logger.Print($" cardclr.... {cardclr}");

                break;

            case 1: // Play Shield

                Logger.Print($" PlayShield ....>");

                cardclr = BottomPlayerCard.Find(x => x.Contains("k-5"));
                Logger.Print($" cardclr.... {cardclr}");
                break;
        }

        cIndex = BottomPlayerCard.IndexOf(cardclr);
        for (int i = 0; i < allColorButtons.Length; i++) allColorButtons[i].interactable = true;
        EventHandler.SelectCardReq(BottomPlayerCard[cIndex]);
        ChooseColorText.SetActive(false);

        isKeepPlayOn = false;
        BacktoLobby = true;
    }

    #endregion

    #region // Tutrial Mathods //
    public void TutorialUTSHandle(int index, SelfTurnTimerControl ring)
    {
        if (index == 0)
            AudioManager.instance.AudioPlay(AudioManager.instance.userTurn);
        timerAnim?.Kill();
        fillTweenAnim?.Kill();
        UTSAnimation(playerFrms[index].transform);
        StartUserTurnTimer(ring, 30, true);
    }

    public void TutorialSkipAnim(string c, Transform ThrowCard, Transform playerImg)
    {
        SkipTurnAnimation(skipImgObj, playerImg.transform);
    }

    public void TutorialReverseAnimation(string c, bool isReverse)
    {
        if (getCardValue(c) == 11)
        {
            ReverseCardAnimation(getCardColor(c), isReverse ? 417f : -303f);
            CenterGlowCard.reverse = isReverse;
        }
    }

    public void TutorialWild4PlusCard(string cs)
    {
        Logger.Print($"{TAG} | TutorialWild4PlusCard cs = {cs}");
        if (!ChooseColorPanel.activeInHierarchy)
        {
            colorBlackBg.DOFade(1, 0);
            CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.one, true, false);
            ProfilePanel.instance.ProfilePanelClick(10);
        }
        SelectColorAnimation(cs == "r" ? 0 : cs == "g" ? 1 : cs == "b" ? 2 : 3);
    }

    public void TutorialKeepPlayPopupon()
    {
        isKeepPlayOn = true;
        if (BackMenuPannel.activeInHierarchy) OnClick_BackMenu(0, false);
        AudioManager.instance.AudioPlay(AudioManager.instance.aleartPopUp);

        keepCardImg.gameObject.SetActive(false);

        string color = "YELLOW";
        noteText.text = "DOES " + "Ananya" + " STILL HAVE A " + color + " Card?";

        keepBtnText.text = "DRAW CARD";
        playBtnText.text = "CHALLENGE";
        hummerObjKeepPlay.SetActive(true);
        isDeckClickable = false;
        isDontTouch = false;

        CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.one, true, false);
    }

    public IEnumerator TutorialChallangeTrue()
    {
        ChallengeAnimation(LeftPlayerImg.transform, true);
        ChallengeTextAnimation(challengeBottom, true);

        yield return new WaitForSeconds(2);

        ExtraCardAddAnimation(LeftDealCard, LeftPlayerImg.gameObject, 4, 1, 90);
        AddedCardAnimation(4, LeftPlayerImg.transform.position, 1);

        for (int i = 0; i < 4; i++)
            TutorialManager.Instance.leftPlayerCard.Add("r-4-1");
        StartCoroutine(WaitTutorialAnimationOff(1));

        yield return new WaitForSeconds(2);

        TutorialManager.Instance.HandleTutorial(21);
    }

    public void TutorialDrawCard(int count, int si)
    {
        Logger.Print(TAG + "TutorialDrawCard >> " + count + " c = " + si);
        if (si == 3)
        {
            ExtraCardAddAnimation(RightDealCard, RightPlayerImg.gameObject, count, si, 90);
            AddedCardAnimation(count, RightPlayerImg.transform.position, 3);
        }
        if (si == 2)
        {
            ExtraCardAddAnimation(TopDealCard, TopPlayerImg.gameObject, count, si, 0);
            AddedCardAnimation(count, TopPlayerImg.transform.position, 2);
        }

        for (int i = 0; i < count; i++)
        {
            if (si == 3)
                TutorialManager.Instance.rightPlayerCard.Add("b-4-1");
            if (si == 2)
                TutorialManager.Instance.topPlayerCard.Add("y-4-1");
        }
        StartCoroutine(WaitTutorialAnimationOff(1));
    }

    #endregion

    private void ShowTurnMissToolTip()
    {
        turnMissToolTipMsgText.text = MessageClass.TURNMISSTOOL;
        CommanAnimations.instance.FullScreenPanelAnimation(turnMissToolTip.GetComponent<RectTransform>(), true);
        Invoke(nameof(HideTurnMissToolTip), 5f);
    }

    public void HideTurnMissToolTip()
    {
        CommanAnimations.instance.FullScreenPanelAnimation(turnMissToolTip.GetComponent<RectTransform>(), false);
    }

    private void SayUnoAnimation(GameObject unoTextObject, bool isSound = true)
    {
        if (gameIsInBackground)
        {
            unoTextObject.SetActive(false);
            return;
        }
        unoTextParticle.transform.position = unoTextObject.transform.position;

        if (hintUnoToolTip.gameObject.activeInHierarchy)
            hintUnoToolTip.gameObject.SetActive(false);
        hintUnoToolTip.gameObject.transform.localScale = Vector3.one;

        unoTextObject.transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.3f).OnStart(() =>
        {
            if (isSound) AudioManager.instance.AudioPlay(AudioManager.instance.lastCard);
            unoTextObject.SetActive(true);
            unoTextObject.transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetDelay(0.3f).OnStart(() =>
            {
                unoTextParticle.SetActive(true);
            });
        });
    }

    private void HideUnoAnimation(GameObject unoTextObject)
    {
        Logger.Print($"  HideUnoAnimation ========= Gameobject name {unoTextObject.name}");

        if (unoTextObject.activeInHierarchy)
        {
            unoTextObject.transform.localScale = Vector3.zero;
            unoTextObject.SetActive(false);
            unoTextParticle.SetActive(false);
            Logger.NormalLog($"  HideUnoAnimation ========= Gameobject Scale complete ; {unoTextObject.name}");
        }
    }

    private void OpenDeckClick()
    {
        Logger.Print(TAG + " Card Bunch Click & My Turn >> " + MyTurn + " & Deck Clickable >> " + isDeckClickable + " isDontTouch = " + isDontTouch + " isKeepPopupOn = " + isKeepPopupOn + " | popup " + KeepPlayPanel.activeInHierarchy);
        Logger.Print($"challengeDackClickAble : {challengeDackClickAble} || drawBtn: {drawBtn.gameObject.activeInHierarchy}");

        if (KeepPlayPanel.activeInHierarchy || drawBtn.gameObject.activeInHierarchy)
            return;

        if (!MyTurn || !isDeckClickable || isDontTouch || isKeepPopupOn || !challengeDackClickAble)
            return;

        if (PrefrenceManager.cardPicStatus == 0)
        {
            PrefrenceManager.cardPicStatus = 1;
        }
        if (hintActionToolTip.gameObject.activeInHierarchy) hintActionToolTip.gameObject.SetActive(false);
        if (playingClickHand.gameObject.activeInHierarchy) playingClickHand.gameObject.SetActive(false);
        if (playingHandTutorial.gameObject.activeInHierarchy) playingHandTutorial.gameObject.SetActive(false);

        issaveHint = false;
        CancelInvoke(nameof(ShowHintToolTip));

        MyTurn = isDeckClickable = false;
        challengeDackClickAble = true;
        EventHandler.SendPickFromCloseDeck();
    }

    [Header("==============================")]
    [SerializeField] bool isKeepPopupOn;
    [SerializeField] GameObject deckParent;

    JSONNode ChallengeData = "";
    bool isKeepPlayOn = false;
    bool isChallenge = false;
    [SerializeField] GameObject plus4PlayBtn, shieldPlayBtn;
    [SerializeField] GameObject drawBtn;
    [SerializeField] TextMeshProUGUI drawBtnText;

    [SerializeField] bool challengeDackClickAble = true;
    bool isCardSpecificHas, isCardplusHas;
    bool isPlayClickAnimTime;

    [Header("For Panelty")]
    [SerializeField] List<GameObject> addedPaneltyCards = new List<GameObject>();
    [SerializeField] List<GameObject> addedPartnerPaneltyCards = new List<GameObject>();
    Vector2 distance;
    float duration = 0.12f;
    Tween panaltyTween;
    List<GameObject> dummyCard = new List<GameObject>();

    [SerializeField] bool isPaneltyAnimOn = false;
    public Button leaveButton;

    private void OnRecevied_PFCD(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_PFCD called");
        isUno = false;
        isInUNO = false;
        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
        selectedCard = JsonConvert.DeserializeObject<List<string>>(data["SelectedCard"].ToString());
        StopDeckGlow();

        Invoke(nameof(ImageAlphaClear), 0.1f);
        AudioManager.instance.tikAudioSource.Stop();

        if (data["unoRemoveSI"] != -1)
        {
            //HideCatchUno(data["unoRemoveSI"]);
            for (int i = 0; i < gtiRespoce.Count; i++) /// Remove without BG last catd
            {
                if (data["unoRemoveSI"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0);
                        isUnoPressed = false;
                        SayUnoAnimation(BottomUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1);
                        SayUnoAnimation(LeftUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2);
                        SayUnoAnimation(TopUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3);
                        SayUnoAnimation(RightUnoTxt, false);
                    }
                    break;
                }
            }
        }

        closeDeckCounter = data["closedeck"].Count;
        CloseDeckAnim();

        if (flipCloseDeckImg.gameObject.activeInHierarchy)
            flipCloseDeckImg.sprite = IsFlipModeSprite(data["closedeck"][0], !isFlipStatus);

        lastPickCard = data["c"];

        if (data["si"].AsInt == BottomSeatIndex)
        {
            mySideSayUno = false;

            if (data["te"] == 1)
                ShowTurnMissToolTip();

            Logger.Print($"PFCD :: cs = {data["cs"]}");

            BottomPlayerCard = PlayerCard;

            Logger.Print(TAG + " Geted Card Value >> " + BottomPlayerCard.IndexOf(lastPickCard).ToString());

            if (BottomPlayerCard.Count > 1)
                HideUnoAnimation(BottomUnoTxt);

            if (MyCardGrid.transform.childCount <= BottomPlayerCard.IndexOf(lastPickCard))
            {
                Sprite spr = IsFlipModeSprite(BottomPlayerCard[BottomPlayerCard.IndexOf(lastPickCard)], isFlipStatus);

                GameObject img = Instantiate(SpriteImg, MyCardGrid.transform);
                img.transform.GetComponent<CardController>().cardValue = lastPickCard;
                img.transform.GetComponent<Image>().sprite = spr;
            }

            Logger.NormalLog($"PICK NAME : {MyCardGrid.transform.GetChild(BottomPlayerCard.IndexOf(lastPickCard)).gameObject.name}");
            string ltcPic = IsFlipModeCardStr(data["ltc"]);

            CardPickAnimation(BottomDealCard, MyCardGrid.transform.GetChild(BottomPlayerCard.IndexOf(lastPickCard)).transform, 0, false, false, 0, false, -1, lastPickCard, ltcPic, data["cs"]);

            BottomPlayerCardCounter.text = PlayerCard.Count + "";
            isKeepPopupOn = data["wait"].AsBool;
            MyTurn = data["wait"].AsBool;
            KeepPlayChallenge(data["wait"].AsBool && data["te"] != 1);

            isInUNO = data["sayUno"].AsBool;
        }
        else if (data["si"].AsInt == LeftSeatIndex)
        {
            LeftPlayerCard = PlayerCard;
            CardPickAnimation(LeftDealCard, LeftPlayerImg.transform, 90, false, false, 0, false, -1, lastPickCard);
            RedrawCard(LeftSeatIndex);//
            if (LeftPlayerCard.Count > 1) HideUnoAnimation(LeftUnoTxt);
            LeftPlayerCardCounter.text = PlayerCard.Count + "";
        }
        else if (data["si"].AsInt == TopSeatIndex)
        {
            TopPlayerCard = PlayerCard;
            CardPickAnimation(TopDealCard, TopPlayerImg.transform, 0, false, false, 0, false, -1, lastPickCard);
            RedrawCard(TopSeatIndex);//
            if (TopPlayerCard.Count > 1) HideUnoAnimation(TopUnoTxt);
            TopPlayerCardCounter.text = PlayerCard.Count + "";
        }
        else if (data["si"].AsInt == RightSeatIndex)
        {
            RightPlayerCard = PlayerCard;
            CardPickAnimation(RightDealCard, RightPlayerImg.transform, 90, false, false, 0, false, -1, lastPickCard);
            RedrawCard(RightSeatIndex);//
            if (RightPlayerCard.Count > 1) HideUnoAnimation(RightUnoTxt);
            RightPlayerCardCounter.text = PlayerCard.Count + "";
        }
    }

    private void CloseDeckAnim()
    {
        CloseDeckCounter.text = closeDeckCounter.ToString();

        Logger.Print($"CloseDeckAnim : {closeDeckCounter}");
        if (closeDeckCounter == 1)
        {
            cardBunch.sprite = closeDeckSingle.sprite;
        }
        else if (closeDeckCounter <= 54)
        {
            cardBunch.sprite = closeDeckHalf.sprite;
        }
        else
        {
            cardBunch.sprite = close_DeckFull.sprite;
        }
    }

    private void OnRecevied_OTCDACKANIMATION(JSONNode data)
    {
        Logger.Print($"cardBunch : {data}");
        Logger.Print($"OTC transform.SetSiblingIndex(4) : {deckParent.transform.GetChild(4).gameObject.name}");

        StartCoroutine(CardDeckController.instance.OtcDackAnimation(cardBunch.gameObject.transform, cardBunch, data["opendeck"][0], deckParent.transform.GetChild(4).gameObject.transform));
    }

    private void CardPickAnimation(GameObject cardObject, Transform Destination, float rotationZ, bool isBottomPanelty = false, bool isTopPanelty = false,
        float extraDelay = 0, bool isLast = false, int si = -1, string lastPickedCard = "", string lastTC = "", string cs = "")
    {
        if (gameIsInBackground)
        {
            if (cardObject == BottomDealCard) RedrawCard(BottomSeatIndex);
            if (cardObject == TopDealCard && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) RedrawCard(TopSeatIndex);

            cardObject.gameObject.SetActive(false);
            cardObject.transform.position = demoCard.transform.position;
            cardObject.transform.rotation = RotationDemo.transform.rotation;
            cardObject.transform.localScale = demoCard.transform.localScale;
            return;
        }

        if (cardObject == BottomDealCard)
        {
            if (CardDeckController.instance.selectCard != null && CardDeckController.instance.selectCard.gameObject.activeInHierarchy)
                CardDeckController.instance.selectCard.gameObject.SetActive(false);
        };
        if (cardObject == BottomDealCard && isBottomPanelty) Destination = addedPaneltyCards[(int)(extraDelay / (cardDealAnimTime * 1.2f))].transform;

        Logger.RecevideLog(TAG + $" lastPickedCard ::::: {lastPickedCard}");

        Sprite cardPick = mode.Equals(AppData.FLIP) ?
            IsFlipModeSprite(lastPickedCard, (cardObject == BottomDealCard) ? isFlipStatus : !isFlipStatus) :
            cardSprite.sprite;

        cardObject.transform.DOMove(Destination.position, cardDealAnimTime).SetDelay(extraDelay).OnStart(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.cardPick);
            cardObject.gameObject.SetActive(true);

            cardObject.gameObject.GetComponent<Image>().sprite = cardPick;

            cardObject.transform.DORotate(new Vector3(0, 0, rotationZ), cardDealAnimTime);//0.3

            cardObject.transform.DOScale(cardObject == BottomDealCard ? new Vector3(1.3f, 1.3f, 1) :
                cardObject == TopDealCard && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) ? new Vector3(0.65f, 0.65f, 1f) :
                Vector3.zero, cardDealAnimTime / 2f).SetDelay(cardDealAnimTime / 2f).OnComplete(() =>
                {
                    Logger.Print(TAG + " Card Pick Animation Over For : ABC " + cardObject.name);
                    if (cardObject == BottomDealCard)
                    {
                        RedrawCard(BottomSeatIndex);
                        if (isKeepPopupOn)
                        {
                            Logger.Print(" Card Pick LTC : " + lastTC);
                            //UserCardClicable();
                        }
                    }
                    if (cardObject == TopDealCard && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)))
                        RedrawCard(TopSeatIndex);

                    if (isBottomPanelty)
                    {
                        Logger.Print(TAG + "Remove Card From List >> ");
                        addedPaneltyCards[0].SetActive(true);
                        addedPaneltyCards.Remove(addedPaneltyCards[0]);
                    }
                    if (isTopPanelty)
                    {
                        addedPartnerPaneltyCards[0].SetActive(true);
                        addedPartnerPaneltyCards.Remove(addedPartnerPaneltyCards[0]);
                    }

                    if (isLast)
                    {
                        Logger.Print(TAG + " Is Last Card From Panelty >> ");
                        if (si == BottomSeatIndex) addedPaneltyCards.Clear();
                        if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) addedPartnerPaneltyCards.Clear();
                        if (isBottomPanelty) RedrawCard(BottomSeatIndex);
                        if (isTopPanelty) RedrawCard(TopSeatIndex);
                    }

                    cardObject.gameObject.SetActive(false);
                    cardObject.transform.position = demoCard.transform.position;
                    cardObject.transform.rotation = RotationDemo.transform.rotation;
                    cardObject.transform.localScale = demoCard.transform.localScale;
                    Logger.Print(TAG + " Card Pick Animation Over For DFG: " + cardObject.name);
                });
        });
    }

    private void KeepPlayChallenge(bool isKeep)
    {
        if (!isKeep)
            return;
        this.isKeep = isKeep;
        isKeepPlayOn = isKeep;
        isKeepPopupOn = true;
        BacktoLobby = false;
        if (BackMenuPannel.activeInHierarchy) OnClick_BackMenu(0, false);
        if (TableInfoPannel.activeInHierarchy) OnClick_BackMenu(6, false);

        AudioManager.instance.AudioPlay(AudioManager.instance.aleartPopUp);
        Logger.Print("Index Of Glowing Card : " + BottomPlayerCard.IndexOf(lastPickCard));
        CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.one, true);

        keepCardImg.sprite = IsFlipModeSprite(lastPickCard, isFlipStatus);
        keepCardImg.gameObject.SetActive(true);
        noteText.text = "";
        keepBtnText.text = "KEEP CARD";
        playBtnText.text = "PLAY CARD";

        plus4PlayBtn.gameObject.SetActive(false);
        shieldPlayBtn.gameObject.SetActive(false);

        hummerObjKeepPlay.SetActive(false);
    }

    public void DrawKeepPenalty()
    {
        drawBtn.SetActive(false);
        MyTurn = false;
        CardDeckController.instance.AddSelfPlayerCard();
        EventHandler.SendKEEPPENALTY();
    }

    private void Challenge(JSONNode data)
    {
        isKeep = false;

        ChallengeData = "";
        isChallenge = true;
        ProfilePanel.instance.ProfilePanelClick(10);
        Logger.Print(TAG + "Bottom Seat Index > " + BottomSeatIndex + " >>>> Challenge SI >> " + data["si"]);

        int tt = data["tt"];
        if (data["si"].AsInt == BottomSeatIndex)
        {
            isKeepPlayOn = true;
            if (BackMenuPannel.activeInHierarchy) OnClick_BackMenu(0, false);

            ChallengeData = data;
            AudioManager.instance.AudioPlay(AudioManager.instance.aleartPopUp);

            keepCardImg.gameObject.SetActive(false);

            string color = data["color"] == "r" ? "RED" : data["color"] == "y" ? "YELLOW" : data["color"] == "g" ? "GREEN" : "BLUE";
            noteText.text = "DOES " + data["pn"] + " STILL HAVE A " + color + " Card?";

            keepBtnText.text = "DRAW CARD";
            playBtnText.text = "CHALLENGE";
            hummerObjKeepPlay.SetActive(true);
            isDeckClickable = false;

            bool isCardShieldHas = BottomPlayerCard.Any(x => x.Contains("k-5"));
            isCardplusHas = BottomPlayerCard.Any(x => x.Contains("k-1"));
            isCardSpecificHas = BottomPlayerCard.Any(x => x.Contains("k-6"));

            Logger.Print($"plus4PlayBtn is : {rules.Contains(9)} == {isCardplusHas} == {isCardSpecificHas}");
            Logger.Print($"shieldPlayBtn is : {rules.Contains(9)} | {rules.Contains(8)} == {isCardShieldHas}");

            //plus4PlayBtn.GetComponent<Button>().onClick.AddListener(() => PlayPlus4BtnClick(2));

            plus4PlayBtn.gameObject.SetActive(rules.Contains(9) && (isCardplusHas || isCardSpecificHas));
            shieldPlayBtn.gameObject.SetActive((rules.Contains(9) || rules.Contains(8)) && isCardShieldHas);

            CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.one, true, false);
        }
        else if (data["si"].AsInt == LeftSeatIndex)
        {
            UTSAnimation(playerFrms[1].transform);
            StartUserTurnTimer(LeftPlayerTurnRing, tt);
        }
        else if (data["si"].AsInt == TopSeatIndex)
        {
            UTSAnimation(playerFrms[2].transform);
            StartUserTurnTimer(TopPlayerTurnRing, tt);
        }
        else if (data["si"].AsInt == RightSeatIndex)
        {
            UTSAnimation(playerFrms[3].transform);
            StartUserTurnTimer(RightPlayerTurnRing, tt);
        }

        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
        selectedCard = JsonConvert.DeserializeObject<List<string>>(data["SelectedCard"].ToString());
        string CS = data["cs"];
        fillTweenAnim?.Kill();

        float delay;
        if (data["si"].AsInt == BottomSeatIndex)
            delay = 0.5f;
        else
            delay = 0;
        StartCoroutine(DelayUserTurnStart(data, PlayerCard, CS, true, delay));
    }

    private void OnRecevied_CHALLENGERESULT(JSONNode data)
    {
        //Challenge Lenarnu Animation
        if (data["senderSi"].AsInt == BottomSeatIndex) ChallengeAnimation(BottomPlayerImg.transform, data["flag"].AsBool);
        else if (data["senderSi"].AsInt == TopSeatIndex) ChallengeAnimation(TopPlayerImg.transform, data["flag"].AsBool);
        else if (data["senderSi"].AsInt == LeftSeatIndex) ChallengeAnimation(LeftPlayerImg.transform, data["flag"].AsBool);
        else if (data["senderSi"].AsInt == RightSeatIndex) ChallengeAnimation(RightPlayerImg.transform, data["flag"].AsBool);

        mySideSayUno = false;
        isInUNO = data["sayUno"].AsBool;

        if (data["unoRemoveSI"] != -1)
        {
            for (int i = 0; i < gtiRespoce.Count; i++) /// Remove without BG last catd
            {
                if (data["unoRemoveSI"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0);
                        SayUnoAnimation(BottomUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1);
                        SayUnoAnimation(LeftUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2);
                        SayUnoAnimation(TopUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3);
                        SayUnoAnimation(RightUnoTxt, false);
                    }
                    break;
                }
            }
        }

        //Challenge Karnar nu Animation
        if (data["si"].AsInt == BottomSeatIndex)
        {
            CardDeckController.instance.ResetMyCardPos();
            clicableCard = 0;
            challengeDackClickAble = data["flag"].AsBool;
            Logger.NormalLog($"challengeDackClickAble == {challengeDackClickAble}");
            isPaneltyAnimOn = false;
            if (KeepPlayPanel.activeInHierarchy)
            {
                isPaneltyAnimOn = true;
                KeepPlayPanel.gameObject.SetActive(false);
                CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false);
            }
            Logger.NormalLog($"clicableCard  result KeepPlayPanel.activeInHierarchy = + {KeepPlayPanel.activeInHierarchy}");
            if (challengeDackClickAble)
            {
                string CS = data["cs"];
                UserCardClicable();
            }

            ChallengeTextAnimation(challengeBottom, data["flag"].AsBool);
        }
        else if (data["si"].AsInt == TopSeatIndex) ChallengeTextAnimation(challengeTop, data["flag"].AsBool);
        else if (data["si"].AsInt == LeftSeatIndex) ChallengeTextAnimation(challengeLeft, data["flag"].AsBool);
        else if (data["si"].AsInt == RightSeatIndex) ChallengeTextAnimation(challengeRight, data["flag"].AsBool);
    }

    public void KeepClick()
    {
        if (isDontTouch) return;
        mySideSayUno = false;
        ImageAlphaClear();
        AudioManager.instance.tikAudioSource.Stop();
        if (!isKeepPlayOn)
        {
            isKeepPopupOn = false;
            Logger.Print("Is Not My Turn...");
            CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
            return;
        }
        Logger.Print(TAG + " KeepClick >> " + isKeepPlayOn + " Keep Bool Is : >> " + isKeep + "<<< Challenge Data Count : " + ChallengeData.Count);

        if (isKeep)
            EventHandler.KeepCard();

        else if (ChallengeData.Count != 0)
        {
            EventHandler.SendChallenge(ChallengeData["color"], false, ChallengeData["isover"], ChallengeData["si"], ChallengeData["senderSi"], ChallengeData["isColorcard"]);
            ChallengeData = "";
        }

        if (KeepPlayPanel.activeInHierarchy) CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
        isKeepPlayOn = false;
        BacktoLobby = true;
        if (isKeep)
            RedrawCard(BottomSeatIndex);
    }

    public void PlayClick()
    {
        // Tutorial
        if (AppData.isTutorialPlay)
        {
            TutorialManager.Instance.playingTutorialScreen.SetActive(false);
            AudioManager.instance.tikAudioSource.Stop();
            CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
            StartCoroutine(TutorialChallangeTrue());
            MyTurn = false;
            return;
        }

        if (isDontTouch) return;
        isKeepPopupOn = false;
        mySideSayUno = false;
        if (!isKeepPlayOn)
        {
            Logger.Print(" IS Not My Turn....>");
            CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
            return;
        }

        AudioManager.instance.tikAudioSource.Stop();
        Logger.Print(TAG + " PlayClick >> " + isKeepPlayOn + " Keep Bool Is : >> " + isKeep + "<<< Challenge Data Count : " + ChallengeData.Count);

        CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);

        string cardclr = IsFlipModeCardStr(lastPickCard);

        if (isKeep)
        {
            isPlayClickAnimTime = true;
            if (getCardColor(cardclr).Equals("k"))
            {
                cIndex = BottomPlayerCard.IndexOf(lastPickCard);
                for (int i = 0; i < allColorButtons.Length; i++) allColorButtons[i].interactable = true;
                EventHandler.SelectCardReq(BottomPlayerCard[cIndex]);
                ChooseColorText.SetActive(false);

                if (rules.Contains(3) && getCardValue(cardclr) == 6)
                {
                    isSpecific4Card = true;
                    ShowPlus4SpeceficButtons(true);
                }
                return;
            }

            if (rules.Contains(6) && getCardValue(cardclr) == 7)
            {
                swapCard = cardclr;
                ShowSwapButtons(true);
                return;
            }
            EventHandler.ThrowCard(lastPickCard, getCardColor(cardclr), isUno);
        }
        else if (ChallengeData.Count != 0)
        {
            isPlayClickAnimTime = false;
            EventHandler.SendChallenge(ChallengeData["color"], true, ChallengeData["isover"], ChallengeData["si"], ChallengeData["senderSi"], ChallengeData["isColorcard"]);
            ChallengeData = "";
            isChallenge = false;
        }

        isKeepPlayOn = false;
        BacktoLobby = true;
    }

    private void HandleUnoPenalty(int seatIndex)
    {
        if (seatIndex == BottomSeatIndex)
        {
            ShowCatchUNO(false, 0, true);
        }
        else if (seatIndex == LeftSeatIndex)
        {
            ShowCatchUNO(false, 1, true);
        }
        else if (seatIndex == TopSeatIndex)
        {
            ShowCatchUNO(false, 2, true);
        }
        else if (seatIndex == RightSeatIndex)
        {
            ShowCatchUNO(false, 3, true);
        }
    }

    private void OnRecevied_PENALTY(JSONNode data)
    {
        StartCoroutine(DelayThrowPenalty(data));
    }

    private IEnumerator DelayThrowPenalty(JSONNode data)
    {
        yield return new WaitForSeconds(0.5f);
        Logger.Print(TAG + " OnRecevied_PENALTY called");

        if (data["type"].Equals("penalty") && data["unoRemoveSI"] != -1)
        {
            HandleUnoPenalty(data["unoRemoveSI"].AsInt);
        }

        else if ((data["type"].Equals("drawfour") && data.HasKey("unoRemoveSI") && data["unoRemoveSI"] != -1) ||
            data["type"].Equals("drawsix") && data.HasKey("unoRemoveSI") && data["unoRemoveSI"] != -1)
        {
            if (data["unoRemoveSI"] != -1)
            {
                for (int i = 0; i < gtiRespoce.Count; i++) /// Remove without BG last catd
                {
                    if (data["unoRemoveSI"].Equals(gtiRespoce[i].si))
                    {
                        if (gtiRespoce[i].si == BottomSeatIndex)
                        {
                            HideCatchUno(0);
                            SayUnoAnimation(BottomUnoTxt, false);
                        }
                        else if (gtiRespoce[i].si == LeftSeatIndex)
                        {
                            HideCatchUno(1);
                            SayUnoAnimation(LeftUnoTxt, false);
                        }
                        else if (gtiRespoce[i].si == TopSeatIndex)
                        {
                            HideCatchUno(2);
                            SayUnoAnimation(TopUnoTxt, false);
                        }
                        else if (gtiRespoce[i].si == RightSeatIndex)
                        {
                            HideCatchUno(3);
                            SayUnoAnimation(RightUnoTxt, false);
                        }
                        break;
                    }
                }
            }
        }

        if (data.HasKey("CatcherSI"))
        {
            unoAnimator[data["CatcherSI"]].enabled = true;
            unoAnimator[data["CatcherSI"]].Play("UnoAnim");
            int msgIndex = -1;
            if (data["CatcherSI"] == BottomSeatIndex)
            {
                msgIndex = 0;
            }
            else if (data["CatcherSI"] == LeftSeatIndex)
            {
                msgIndex = 1;
            }
            else if (data["CatcherSI"] == TopSeatIndex)
            {
                msgIndex = 2;
            }
            else if (data["CatcherSI"] == RightSeatIndex)
            {
                msgIndex = 3;
            }

            if (msgIndex != -1)
            {
                msgToolTipTxts[msgIndex].text = "CATCH";
                MessageToolTipAnimation(msgTooltips[msgIndex].transform);
            }
        }

        List<string> TakeCard = JsonConvert.DeserializeObject<List<string>>(data["c"].ToString());
        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
        closeDeckCounter = data["closedeck"].Count;
        CloseDeckAnim();

        Logger.Print($"DelayThrowPenalty :: Tack count: {TakeCard.Count}");
        //Logger.Print($"DelayThrowPenalty :: Tack count: {getCardValue(data["ltc"])}");

        string lts = IsFlipModeCardStr(data["ltc"]);

        bool isSpecific4Ltc = lts.Contains("k-6");
        bool iswild = getCardColor(lts).Equals("k");
        bool isHintShow = false;

        hintActionToolText.text = "";

        if (isSpecific4Ltc)
        {
            bool isWild = getCardColor(data["ltc"]).Equals("k");

            if (isWild && getCardValue(data["ltc"]) == 6) // specefic +4
            {
                isHintShow = PrefrenceManager.spcificOpStepCount < 5;
                if (PrefrenceManager.spcificOpStepCount < 5)
                    PrefrenceManager.spcificOpStepCount += 1;

                if (isHintShow)
                {
                    string p1name = gtiRespoce[CheckTableSi(data["te"].AsInt)].getPn();
                    string p2name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                    hintActionToolText.text = $"<color=yellow>{p1name}</color>{TutorialActionSteps.sp_OpThrow} <color=yellow>{p2name}</color>.";
                }
            }
        }
        else if (isActionCyclonCard) // cyclone Explain
        {
            isHintShow = PrefrenceManager.cycloneStepCount < 5;
            if (PrefrenceManager.cycloneStepCount < 5)
                PrefrenceManager.cycloneStepCount += 1;
            if (isHintShow)
            {
                string p2name = gtiRespoce[CheckTableSi(data["si"])].getPn();
                string cs = data["cs"];

                hintActionToolText.text = $"<color=yellow>{p2name}</color> {TutorialActionSteps.cyclone_Hint} {GetColorOnTutorial(cs)} card..";
            }
        }
        else if (data["IsHIGHFIVE"].AsBool) //HighFive
        {
            isHintShow = PrefrenceManager.highFiveStep < 5;
            if (PrefrenceManager.highFiveStep < 5)
                PrefrenceManager.highFiveStep += 1;

            if (isHintShow)
            {
                string p1name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                hintActionToolText.text = $"<color=yellow>{p1name}</color> {TutorialActionSteps.high_fiveHint}";
            }
        }

        if (issaveHint) issaveHint = false;
        if (mode.Equals(AppData.CLASSIC) && rules.Count == 0)
        {
            if (getCardValue(data["ltc"]) == 12 && PrefrenceManager.twoPlusShow == 0)
            {
                isHintShow = PrefrenceManager.twoPlusShow == 0;
                PrefrenceManager.twoPlusShow = 1;
                string p1name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                hintActionToolText.text = $"{TakeCard.Count} {TutorialActionSteps.penalty} <color=yellow>{p1name}</color>, and their turn is skipped.";
            }

            else if (getCardValue(data["ltc"]) == 17 && PrefrenceManager.plusOneShow == 0)
            {
                isHintShow = PrefrenceManager.plusOneShow == 0;
                PrefrenceManager.plusOneShow = 1;
                string p1name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                hintActionToolText.text = $"{TakeCard.Count} {TutorialActionSteps.penalty} <color=yellow>{p1name}</color>, and their turn is skipped.";
            }

            else if ((iswild && getCardValue(data["ltc"]) == 1) && PrefrenceManager.fourPlusPenalty == 0)
            {
                isHintShow = PrefrenceManager.fourPlusPenalty == 0;
                PrefrenceManager.fourPlusPenalty = 1;
                string p1name = gtiRespoce[CheckTableSi(data["si"].AsInt)].getPn();
                hintActionToolText.text = $"{TakeCard.Count} {TutorialActionSteps.penalty} <color=yellow>{p1name}</color>, and their turn is skipped.";
            }

        }

        hintActionToolTip.gameObject.SetActive(isHintShow);
        Logger.Print($"isHintShow C :: {isHintShow}");

        Logger.Print($"spcificOpStepCount: {PrefrenceManager.spcificOpStepCount} | cycloneStepCount: {PrefrenceManager.cycloneStepCount} | highFiveStep : {PrefrenceManager.highFiveStep}");
        Logger.Print($"twoPlusShow: {PrefrenceManager.twoPlusShow} | plusOneShow: {PrefrenceManager.plusOneShow} | fourPlusPenalty : {PrefrenceManager.fourPlusPenalty}");

        wildUpCounter.gameObject.SetActive(false);
        wildUpshiny.gameObject.SetActive(false);

        Vector2 allpos = new Vector2(97, 5);
        BottomThrowCard.transform.localPosition = LeftThrowCard.transform.localPosition = TopThrowCard.transform.localPosition = RightThrowCard.transform.localPosition = allpos;

        if (data["wildupcounter"].AsBool)
        {
            wildUpCounter.text = "";
            wildUpCounter.gameObject.SetActive(false);
            wildDummyShowObject.SetActive(false);
            //wildDummyShowObject.transform.localScale = Vector3.zero;

            var main = wildUpAnimParticle.main;
            var spk = sparks1.main;
            var spk2 = sparks2.main;

            main.startColor = new ParticleSystem.MinMaxGradient(new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, 0.7f));
            spk.startColor = new ParticleSystem.MinMaxGradient(new Color(spk.startColor.color.r, spk.startColor.color.g, spk.startColor.color.b, 0.7f));
            spk2.startColor = new ParticleSystem.MinMaxGradient(new Color(spk2.startColor.color.r, spk2.startColor.color.g, spk2.startColor.color.b, 0.7f));

            DOTween.To(() => main.startColor.color.a, x => main.startColor = new ParticleSystem.MinMaxGradient(new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, x)), 0, 0.2f)
               .OnComplete(() => wildUpAnimParticle.gameObject.SetActive(false));
            DOTween.To(() => spk.startColor.color.a, x => spk.startColor = new ParticleSystem.MinMaxGradient(new Color(spk.startColor.color.r, spk.startColor.color.g, spk.startColor.color.b, x)), 0, 0.2f);
            DOTween.To(() => spk2.startColor.color.a, x => spk2.startColor = new ParticleSystem.MinMaxGradient(new Color(spk2.startColor.color.r, spk2.startColor.color.g, spk2.startColor.color.b, x)), 0, 0.2f);
        }

        isfiveSend = false;
        ShowHighFiveHand(false);

        flipCloseDeckImg.sprite = IsFlipModeSprite(data["closedeck"][0], !isFlipStatus);

        if (data["type"].Equals("penalty")) AudioManager.instance.AudioPlay(AudioManager.instance.unoPanelty);

        if (data["si"] == BottomSeatIndex)
        {
            try
            {
                if (data["te"] == 1) ShowTurnMissToolTip();
                CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
                ImageAlphaClear();
                addedPaneltyCards.Clear();

                if (data["stackcard"] == 1)
                {
                    BottomPlayerCard = PlayerCard;
                    BottomPlayerCardCounter.text = PlayerCard.Count + "";
                    if (BottomPlayerCard.Count > 1) HideUnoAnimation(BottomUnoTxt);
                    StartCoroutine(StackCardAnimation(TakeCard, BottomSeatIndex, 0, isActionCyclonCard));
                }
                else
                {
                    // TODO : Remove All Position Set Cards List  // 
                    CardDeckController.instance.RemoveAllCard();

                    for (int i = 0; i < TakeCard.Count; i++)
                    {
                        GameObject img = Instantiate(SpriteImg, MyCardGrid.transform);
                        img.name = (BottomPlayerCard.Count + i + 1) + "";
                        img.transform.GetComponent<CardController>().cardValue = TakeCard[i];

                        img.SetActive(false);
                    }
                    Logger.Print($" penalty Step: 1");

                    for (int i = 0; i < MyCardGrid.transform.childCount; i++)
                    {
                        CardController myCard = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();

                        Sprite spr = IsFlipModeSprite(PlayerCard[i], isFlipStatus);

                        myCard.cardImage.sprite = spr;
                        myCard.cardValue = PlayerCard[i];

                        myCard.gameObject.name = $"{i}";
                        myCard.gameObject.SetActive(true);
                        CardDeckController.instance.AddCardOnList(0, myCard);
                    }
                    Logger.Print($" penalty Step: 2");

                    foreach (var cardValue in TakeCard)
                    {
                        int index = CardDeckController.instance.playerData[0].myCards.FindIndex(cardController => cardController.cardValue == cardValue);
                        if (index != -1)
                        {
                            GameObject cardObj = CardDeckController.instance.playerData[0].myCards[index].gameObject;
                            cardObj.SetActive(false);
                            cardObj.GetComponent<Image>().sprite = IsFlipModeSprite(cardValue, isFlipStatus);
                            addedPaneltyCards.Add(cardObj);
                        }
                    }
                    Logger.Print($" penalty Step: 3");
                    StartCoroutine(MoveToDeck(data["si"], TakeCard));

                    AddedCardAnimation(TakeCard.Count, BottomPlayerImg.transform.position, 0, isSpecific4Ltc);
                    BottomPlayerCard = PlayerCard;
                    BottomPlayerCardCounter.text = PlayerCard.Count + "";
                    if (BottomPlayerCard.Count > 1) HideUnoAnimation(BottomUnoTxt);
                }
            }
            catch (Exception ex)
            {
                JSONNode objects = new JSONObject
                {
                    ["addedPanelty_Count"] = addedPaneltyCards.Count,
                    ["PlayerCardCount"] = PlayerCard.Count,
                    ["TakeCard"] = TakeCard.Count,
                    ["childCount"] = MyCardGrid.transform.childCount,
                    ["Background"] = SocketManagergame.Instance.Backgroundhandle,
                };
                Loading_screen.instance.SendExe("GameManager", "DelayThrowPenalty", $"{objects}", ex);
                Logger.Print($"ex penalty : {ex}");
            }

        }

        else if (data["si"] == LeftSeatIndex)
        {
            isPaneltyAnimOn = false;

            if (data["stackcard"] == 1)
            {
                LeftPlayerCard = PlayerCard;
                LeftPlayerCardCounter.text = PlayerCard.Count + "";
                if (LeftPlayerCard.Count > 1) HideUnoAnimation(LeftUnoTxt);
                StartCoroutine(StackCardAnimation(TakeCard, LeftSeatIndex, 1, isActionCyclonCard));
            }
            else
            {
                ExtraCardAddAnimation(LeftDealCard, LeftPlayerImg.gameObject, TakeCard.Count, data["si"], 90, false, false, TakeCard);

                AddedCardAnimation(TakeCard.Count, LeftPlayerImg.transform.position, 1, isSpecific4Ltc);
                LeftPlayerCard = PlayerCard;
                LeftPlayerCardCounter.text = PlayerCard.Count + "";
                if (LeftPlayerCard.Count > 1) HideUnoAnimation(LeftUnoTxt);
                RedrawCard(LeftSeatIndex);
            }
        }
        else if (data["si"] == TopSeatIndex)
        {
            isPaneltyAnimOn = false;
            if (data["stackcard"] == 1)
            {
                TopPlayerCard = PlayerCard;
                TopPlayerCardCounter.text = PlayerCard.Count + "";
                if (TopPlayerCard.Count > 1) HideUnoAnimation(TopUnoTxt);
                StartCoroutine(StackCardAnimation(TakeCard, TopSeatIndex, 2, isActionCyclonCard));
            }
            else
            {
                if (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))
                {
                    for (int i = 0; i < TakeCard.Count; i++)
                    {
                        GameObject img = Instantiate(SpriteImg, partnerCardGrid.transform);
                        img.name = (TopPlayerCard.Count + i + 1) + "";
                        img.transform.GetComponent<CardController>().cardValue = TakeCard[i];
                        img.SetActive(false);
                        addedPartnerPaneltyCards.Add(img);
                    }
                }
                AddedCardAnimation(TakeCard.Count, TopPlayerImg.transform.position, 2, isSpecific4Ltc);
                ExtraCardAddAnimation(TopDealCard, TopPlayerImg.gameObject, TakeCard.Count, data["si"], 0, false, (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) ? true : false, TakeCard);
                TopPlayerCard = PlayerCard;
                TopPlayerCardCounter.text = PlayerCard.Count + "";
                if (TopPlayerCard.Count > 1) HideUnoAnimation(TopUnoTxt);
                RedrawCard(TopSeatIndex);//
            }

        }
        else if (data["si"] == RightSeatIndex)
        {
            isPaneltyAnimOn = false;
            if (data["stackcard"] == 1)
            {
                RightPlayerCard = PlayerCard;
                RightPlayerCardCounter.text = PlayerCard.Count + "";
                if (RightPlayerCard.Count > 1) HideUnoAnimation(RightUnoTxt);
                StartCoroutine(StackCardAnimation(TakeCard, RightSeatIndex, 3, isActionCyclonCard));
            }
            else
            {
                ExtraCardAddAnimation(RightDealCard, RightPlayerImg.gameObject, TakeCard.Count, data["si"], 90, false, false, TakeCard);

                AddedCardAnimation(TakeCard.Count, RightPlayerImg.transform.position, 3, isSpecific4Ltc);
                RightPlayerCard = PlayerCard;
                RightPlayerCardCounter.text = PlayerCard.Count + "";
                if (RightPlayerCard.Count > 1) HideUnoAnimation(RightUnoTxt);
                RedrawCard(RightSeatIndex);//
            }
        }
    }

    IEnumerator MoveToDeck(int si, List<string> TakeCard)
    {
        panaltyTween?.Kill();
        if (gameIsInBackground)
        {
            HandleBackgroundGameLogic(si);
            yield break;
        }

        CardDeckController.instance.AddSelfPlayerCard();
        for (int i = 0; i < addedPaneltyCards.Count; i++)
        {
            if (gameIsInBackground)
            {
                HandleBackgroundGameLogic(si);
                yield break;
            }

            try
            {
                GameObject card = Instantiate(SpriteImg, demoCard.transform.parent.position, RotationDemo.transform.rotation);
                card.transform.SetParent(demoCard.transform.parent.transform, false);
                card.transform.localScale = Vector2.one;

                dummyCard.Add(card);

                Sequence sequence = DOTween.Sequence();
                card.transform.DORotate(Vector3.zero, duration);


                sequence.Append(card.transform.DOScale(new Vector3(1.3f, 1.3f), duration).SetEase(Ease.Linear));

                distance = addedPaneltyCards[i].transform.position;

                Logger.NormalLog($"Distance ::: {distance} i = {i}");
                AudioManager.instance.AudioPlay(AudioManager.instance.cardPick);
                panaltyTween = card.transform.DOMove(distance, duration + 0.15f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Logger.NormalLog($"Distance ::: name {addedPaneltyCards[i].name}");
                    addedPaneltyCards[i].SetActive(true);
                    Destroy(card);
                    challengeDackClickAble = true; // For open dack click .////// =====
                });
                card.GetComponent<Image>().sprite = IsFlipModeSprite(TakeCard[i], isFlipStatus);
            }
            catch (Exception ex)
            {
                JSONNode objects = new JSONObject
                {
                    ["addedPanelty_Count"] = addedPaneltyCards.Count,
                    ["dummyCard_Count"] = dummyCard.Count,
                };
                Loading_screen.instance.SendExe("GameManager", "MoveToDeck", $"{objects}", ex);
            }


            yield return new WaitForSeconds(0.18f + 0.15f);
        }
        yield return new WaitForSeconds(2);
        isPaneltyAnimOn = false;
        addedPaneltyCards.Clear();
    }

    void HandleBackgroundGameLogic(int si)
    {
        panaltyTween?.Kill();

        for (int i = 0; i < addedPaneltyCards.Count; i++)
        {
            addedPaneltyCards[i].SetActive(true);
        }
        for (int i = 0; i < dummyCard.Count; i++)
        {
            Destroy(dummyCard[i].gameObject);
        }
        dummyCard.Clear();
        addedPaneltyCards.Clear();
        if (si == BottomSeatIndex)
        {
            challengeDackClickAble = true; // For open dack click .////// =====
            RedrawCard(BottomSeatIndex);
        }
        else if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)))
        {
            RedrawCard(TopSeatIndex);
        }
    }

    private void ExtraCardAddAnimation(GameObject cardObject, GameObject Destination, int cnt, int si, int ZRotation, bool isBottomPanelty = false, bool isTopPanelty = false, List<string> TackanCards = null)
    {
        Logger.Print(TAG + " ExtraCardAddAnimation called " + cnt);

        if (gameIsInBackground)
        {
            if (si == BottomSeatIndex)
                RedrawCard(BottomSeatIndex);

            if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) RedrawCard(TopSeatIndex);

            if (si == BottomSeatIndex) addedPaneltyCards.Clear();
            if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) addedPartnerPaneltyCards.Clear();

            return;
        }

        for (int i = 0; i < cnt; i++)
        {
            if (gameIsInBackground)
            {
                Logger.Print("Is Game is in Background " + gameIsInBackground);
                if (si == BottomSeatIndex)
                    RedrawCard(BottomSeatIndex);
                if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) RedrawCard(TopSeatIndex);
                if (si == BottomSeatIndex) addedPaneltyCards.Clear();
                if (si == TopSeatIndex && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))) addedPartnerPaneltyCards.Clear();
                return;
            }
            Logger.Print("Is Bottom Panelty : " + isBottomPanelty + " >> Is Top Panelty : " + isTopPanelty);
            Logger.Print("Bottom Panelty Cards : " + addedPaneltyCards.Count + " >> Partner Panelty Cards : " + addedPartnerPaneltyCards.Count);

            Logger.Print("Card Pick Animation Called For : " + cardObject.name + "<<< To >>>" + Destination.name);
            //Logger.Print($"Card PcardDealAnimTime {(cardDealAnimTime * 1.2f) * i} || TackanCards[i] ; {TackanCards[i]}");
            var lastCard = "";
            if (TackanCards != null)
                lastCard = TackanCards[i];

            CardPickAnimation(cardObject, Destination.transform, ZRotation, isBottomPanelty, isTopPanelty, (cardDealAnimTime * 1.2f) * i, i == cnt - 1, si, lastCard);
        }
        isKeepPopupOn = false;
    }

    bool isKeepedCard;
    private void OnRecevied_KEEP(JSONNode data)
    {
        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
        MyTurn = isDeckClickable = false;
        isKeepedCard = true;

        if (data["si"].AsInt == BottomSeatIndex)
        {
            if (BottomPlayerCard.Count > 1) HideUnoAnimation(BottomUnoTxt);

            BottomPlayerCard = PlayerCard;
            BottomPlayerCardCounter.text = BottomPlayerCard.Count.ToString();
            RedrawCard(BottomSeatIndex);
        }
        if (data["si"].AsInt == LeftSeatIndex)
        {
            if (LeftPlayerCard.Count > 1) HideUnoAnimation(LeftUnoTxt);

            LeftPlayerCard = PlayerCard;
            LeftPlayerCardCounter.text = LeftPlayerCard.Count.ToString();
            RedrawCard(LeftSeatIndex);
        }
        if (data["si"].AsInt == TopSeatIndex)
        {
            if (TopPlayerCard.Count > 1) HideUnoAnimation(TopUnoTxt);

            TopPlayerCard = PlayerCard;
            TopPlayerCardCounter.text = TopPlayerCard.Count.ToString();
            RedrawCard(TopSeatIndex);
        }
        if (data["si"].AsInt == RightSeatIndex)
        {
            if (RightPlayerCard.Count > 1) HideUnoAnimation(RightUnoTxt);

            RightPlayerCard = PlayerCard;
            RightPlayerCardCounter.text = RightPlayerCard.Count.ToString();
            RedrawCard(RightSeatIndex);
        }
    }

    public int getCardValue(string card)
    {
        char[] sepratore = { '-' };
        return int.Parse(card.Split(sepratore, 3)[1]);
    }

    public static string getCardColor(string card)
    {
        char[] sepratore = { '-' };
        return card.Split(sepratore, 3)[0];
    }

    private void CreateDynamicCard(bool onlyPartner = false)
    {
        if (!onlyPartner)
        {
            for (int i = 0; i < BottomPlayerCard.Count; i++)
            {
                GameObject img = Instantiate(SpriteImg, MyCardGrid.transform);
                img.transform.GetComponent<Image>().sprite = cardSprite.sprite;
                img.transform.GetComponent<Image>().raycastTarget = true;
                img.transform.GetComponent<CardController>().cardValue = BottomPlayerCard[i];
                img.name = i + "";
                img.SetActive(false);
                CardDeckController.instance.AddCardOnList(0, img.GetComponent<CardController>());
            }
            CardDeckController.instance.AddSelfPlayerCard();
        }

        if (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER))
        {
            for (int i = 0; i < TopPlayerCard.Count; i++)
            {
                GameObject img = Instantiate(SpriteImg, partnerCardGrid.transform);
                img.transform.GetComponent<Image>().sprite = cardSprite.sprite;
                img.name = i + "";
                img.SetActive(false);
            }
        }
        Logger.Print(TAG + " Generate Counter " + MyCardGrid.transform.childCount);
    }

    public void LeaveTable()
    {
        Logger.Print(TAG + "Back Menu Click >>>");
        if (!BacktoLobby)
            return;

        switchTableBtn.interactable = (!isTournament && ip != 1);
        BackMenuSound.transform.GetComponent<Image>().sprite = OnOffSprite[PrefrenceManager.Sound];
        BackMenuVibrate.transform.GetComponent<Image>().sprite = OnOffSprite[PrefrenceManager.Vibrate];
        CommanAnimations.instance.PopUpAnimation(BackMenuPannel, backMenuBg, backMenuPopUp, Vector3.one, true);
    }

    public void BackMenuBtnClick(int i)
    {
        OnClick_BackMenu(i);
    }

    public void OnClick_BackMenu(int i, bool soundPlay = true)
    {
        Logger.Print(TAG + " BackMenuBtnClick called " + i);
        switch (i)
        {
            case 0://close
                CommanAnimations.instance.PopUpAnimation(BackMenuPannel, backMenuBg, backMenuPopUp, Vector3.zero, false, false);
                break;

            case 1://sound
                Logger.Print(TAG + " Before Sound " + PrefrenceManager.Sound);
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                PrefrenceManager.Sound = PrefrenceManager.Sound == 1 ? 0 : 1;
                BackMenuSound.transform.GetComponent<Image>().sprite = OnOffSprite[PrefrenceManager.Sound];
                if (PrefrenceManager.Sound == 0) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Logger.Print(TAG + " After Sound " + PrefrenceManager.Sound);
                break;

            case 2://vibrate
                Logger.Print(TAG + " Before Vibrate " + PrefrenceManager.Vibrate);
                PrefrenceManager.Vibrate = PrefrenceManager.Vibrate == 1 ? 0 : 1;
                BackMenuVibrate.transform.GetComponent<Image>().sprite = OnOffSprite[PrefrenceManager.Vibrate];
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Logger.Print(TAG + " After Sound " + PrefrenceManager.Vibrate);
                break;

            case 3://table-info
                CommanAnimations.instance.PopUpAnimation(BackMenuPannel, backMenuBg, backMenuPopUp, Vector3.zero, false, false);
                CommanAnimations.instance.PopUpAnimation(TableInfoPannel, tableInfoBg, tableInfoPopUp, Vector3.one, true);
                Invoke("AutoCloseTableInfo", CommanAnimations.instance.animationTime + 3f);
                break;

            case 4://switch-table
                CommanAnimations.instance.PopUpAnimation(BackMenuPannel, backMenuBg, backMenuPopUp, Vector3.zero, false, false);
                if (AllCommonGameDialog.instance.isHaveGoldGems(bv, gems, true))
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    Loading_screen.instance.ShowLoadingScreen(true);
                    isTimerCountinue = false;

                    JSONArray rulesArray = new JSONArray();
                    if (rules != null && rules.Count > 0)
                    {
                        foreach (int rule in rules)
                        {
                            rulesArray.Add(rule);
                        }
                    }

                    int playingV;
                    if (mode.Equals(AppData.FLIP))
                        playingV = 0;
                    else
                        playingV = 1;

                    EventHandler.SwitchTable(mode, bv, Player, gems, ip, tbid, rulesArray, playingV);
                }
                break;

            case 5://backmenu
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                BackMenuPannel.SetActive(false);
                EventHandler.ExitGame();
                break;

            case 6://closeTableInfo
                CommanAnimations.instance.PopUpAnimation(TableInfoPannel, tableInfoBg, tableInfoPopUp, Vector3.zero, false, false);
                break;
        }
    }

    public void AutoCloseTableInfo()
    {
        if (TableInfoPannel.activeInHierarchy)
        {
            CommanAnimations.instance.PopUpAnimation(TableInfoPannel, tableInfoBg, tableInfoPopUp, Vector3.zero, false, false);
        }
        else return;
    }

    private void OnRecevied_JT(JSONNode data)
    {
        string Uid = data["ui"]["ui"]["uid"];

        if (Uid.Equals(PrefrenceManager._ID))
            return;

        string name = data["ui"]["ui"]["pn"];
        int Si = data["ui"]["si"].AsInt;
        string PP = data["ui"]["ui"]["pp"];
        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["ui"]["cards"].ToString());
        string Gift = data["ui"]["giftImg"];
        bool IsCom = data["ui"]["comp"] == 1;
        string frameImg = data["ui"]["ui"]["frameImg"];
        string cardImg = data["ui"]["ui"]["deckImg"]["backcard"];
        string deckImg = data["ui"]["ui"]["deckImg"]["bunch"];// FUll 

        string bunch1 = data["ui"]["ui"]["deckImg"]["bunch1"];// Half
        string bunch2 = data["ui"]["ui"]["deckImg"]["bunch2"];// Single

        if (frameImg == "")
            frameImg = "";
        GTIResponse SingleGti = new GTIResponse(name, Uid, Si, PP, PlayerCard, Gift, IsCom, frameImg, deckImg, cardImg, bunch1, bunch2);

        if (gtiRespoce.Count == 4)
        {
            for (int i = 0; i < gtiRespoce.Count; i++)
            {
                if (gtiRespoce[i].getSi() == Si)
                {
                    Logger.Print(TAG + "Removed From JT ");
                    gtiRespoce.RemoveAt(i);
                    break;
                }
            }
        }

        gtiRespoce.Add(SingleGti);
        TableRotate(Si);

        Logger.Print(TAG + " OnRecevied_JT called " + gtiRespoce.Count);
    }

    bool isAfterEG = false;

    public void BackgroundEGDataHandle()
    {
        gtiRespoce.Clear();
        DashBoardOn();
    }

    public void ShowIntroduction(GameObject obj, bool isAddReyCast, string txt, int i)//Tournament, 
    {
        if (txt.Equals(""))
            return;
        LevelUpPanel.instance.LevelScreenfalse();
        setting_script.instance.FeedbackPopupShow(false);

        Canvas can = obj.AddComponent<Canvas>();
        can.overrideSorting = true;
        can.sortingOrder = 2;
        DashboardManager.instance.modeoutlines[i].gameObject.SetActive(true);

        if (isAddReyCast)
            obj.AddComponent<GraphicRaycaster>();

        if (i == 3)
        {
            DashboardManager.instance.centerScroll.transform.position = new Vector2(-300, 0);
        }

        StartCoroutine(TutorialManager.Instance.AnimateText(DashboardManager.instance.IntroTxt, txt));
    }

    public void UpdateTutorial(string name, Canvas can, GraphicRaycaster raycaster = null)
    {
        if (raycaster != null)
            Destroy(raycaster);

        Destroy(can);
        EventHandler.ManageUserProfile(name);

        DashboardManager.instance.transParentImg.SetActive(false);
    }

    private void OnRecevied_EG(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_EG called " + " Socket :: " + SocketManagergame.Instance);
        Logger.Print(TAG + " AppData.handleLeaveTable " + AppData.handleLeaveTable);
        AppData.promoflag = data["promoflag"];
        AppData.promoType = data["promotype"];

        if (!AppData.handleLeaveTable)
        {
            gtiRespoce.Clear();
            AppData.GTIDATA = new JSONObject();
            AppData.winLossCoins = data["wlg"].AsLong;
            return;
        }

        if (data["isfrom"].Value.Equals(EventHandler.ST) && data["UID"].Value.Equals(PrefrenceManager._ID))
        {
            gtiRespoce.Clear();
            ResetTable(true);
        }
        else
        {
            if (data["auto"] == 1 && data["si"] == BottomSeatIndex)
            {
                if (data["nogold"] == 1) AllCommonGameDialog.instance.ShowNotEnoughDialog(false, long.Parse(PrefrenceManager.GOLD), long.Parse(PrefrenceManager.GEMS)); // Added
                else if (data["nogems"] == 1) AllCommonGameDialog.instance.ShowNotEnoughDialog(true, long.Parse(PrefrenceManager.GOLD), long.Parse(PrefrenceManager.GEMS)); // Added
                else AllCommonGameDialog.instance.SetJustOkDialogData("Time Out!", "You didn't played 2 times in a row and you were removed from the table.");
            }
            HandleLeaveTable(data["si"].AsInt, data["comp"].AsInt, data["UID"].Value, data["singalround"].AsInt, data["wlg"].AsLong);
        }
    }

    public void ShowIntroOnDashboard()
    {
        switch (AppData.promoType)
        {
            case "Custom":
                DashboardManager.instance.transParentImg.gameObject.SetActive(true);
                DashboardManager.instance.introGirl.gameObject.SetActive(true);
                ShowIntroduction(DashboardManager.instance.tu_modeButtons[0].gameObject, true, MessageClass.introCustom, 0);
                break;

            case "invert":
                DashboardManager.instance.transParentImg.gameObject.SetActive(true);
                DashboardManager.instance.introGirl.gameObject.SetActive(true);
                ShowIntroduction(DashboardManager.instance.tu_modeButtons[1].gameObject, true, MessageClass.introInvert, 1);
                break;

            case "Live_Table":

                DashboardManager.instance.transParentImg.gameObject.SetActive(true);
                DashboardManager.instance.introGirl.gameObject.SetActive(true);
                ShowIntroduction(DashboardManager.instance.tu_modeButtons[2].gameObject, true, MessageClass.introliveTable, 2);
                break;

            case "Tournament":

                DashboardManager.instance.transParentImg.gameObject.SetActive(true);
                DashboardManager.instance.introGirl.gameObject.SetActive(true);
                ShowIntroduction(DashboardManager.instance.tu_modeButtons[3].gameObject, true, MessageClass.introTour, 3);
                break;
        }
    }

    private void HandleLeaveTable(int si, int comp, string uid, int singleRound, long wlg)
    {
        Logger.Print(TAG + " HandleLeaveTable called " + si + " gtiResponce Size " + gtiRespoce.Count + " >> UID > " + uid);
        isAfterEG = true;
        isTimerCountinue = false;
        MessagePanel.Instance.OnClick_CloseMessagePanel(false);
        Loading_screen.instance.trapImage.SetActive(false);
        RemovePlayer(si, uid, comp);

        //bottomPlayer
        if (si == BottomSeatIndex)
        {
            HandleBottomPlayer(singleRound, wlg);

        }
        else if (si == LeftSeatIndex)
        {
            Logger.Print(TAG + "Removed Left Player Seat Index >> " + LeftSeatIndex);
            HandlePlayerRemoval(LeftSeatIndex, LeftPlayerName, LeftPlayerImg, leftPlayerFrame, leftPlayerGiftBtn,
                leftPlayerEmoji, LeftPlayerGift, LeftPlayerCardPile, LeftPlayerCardCounter, leftPlayerImgButton);
        }
        //TopPlayer
        else if (si == TopSeatIndex)
        {
            HandlePlayerRemoval(TopSeatIndex, TopPlayerName, TopPlayerImg, topPlayerFrame, topPlayerGiftBtn,
                topPlayerEmoji, TopPlayerGift, TopPlayerCardPile, TopPlayerCardCounter, topPlayerImgButton);
        }
        //RightPlayer
        else if (si == RightSeatIndex)
        {
            HandlePlayerRemoval(RightSeatIndex, RightPlayerName, RightPlayerImg, rightPlayerFrame, rightPlayerGiftBtn,
                rightPlayerEmoji, RightPlayerGift, RightPlayerCardPile, RightPlayerCardCounter, rightPlayerImgButton);
        }

        Logger.Print(TAG + " HandleLeaveTable Over " + si + " gtiResponce Size " + gtiRespoce.Count);
        if (gtiRespoce.Count == 0)
            CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel.GetComponent<RectTransform>(), false);

    }

    public void ResetPlayerProfile()
    {
        HandlePlayerRemoval(LeftSeatIndex, LeftPlayerName, LeftPlayerImg, leftPlayerFrame, leftPlayerGiftBtn,
  leftPlayerEmoji, LeftPlayerGift, LeftPlayerCardPile, LeftPlayerCardCounter, leftPlayerImgButton);

        HandlePlayerRemoval(TopSeatIndex, TopPlayerName, TopPlayerImg, topPlayerFrame, topPlayerGiftBtn,
            topPlayerEmoji, TopPlayerGift, TopPlayerCardPile, TopPlayerCardCounter, topPlayerImgButton);

        HandlePlayerRemoval(RightSeatIndex, RightPlayerName, RightPlayerImg, rightPlayerFrame, rightPlayerGiftBtn,
            rightPlayerEmoji, RightPlayerGift, RightPlayerCardPile, RightPlayerCardCounter, rightPlayerImgButton);
        gtiRespoce.Clear();
        Logger.NormalLog($"Reset ?????????");
        //BackgroundEGDataHandle();
    }

    private void RemovePlayer(int si, string uid, int comp)
    {
        for (int i = 0; i < gtiRespoce.Count; i++)
        {
            if (si == gtiRespoce[i].si && uid.Equals(gtiRespoce[i].uid))
            {
                gtiRespoce.RemoveAt(i);
                break;
            }
        }

        if (comp == 1)
            return;
    }

    private void HandleBottomPlayer(int singleRound, long wlg)
    {
        Logger.Print(TAG + "Removed Bottom Player >> || " + singleRound + " gameIsInBackground = " + gameIsInBackground + " Backgroundhandle = " + SocketManagergame.Instance.Backgroundhandle);
        AppData.winLossCoins = wlg;
        Logger.Print($"{TAG} = AppData.winLossCoins = {AppData.winLossCoins}");
        if (singleRound == 1)
        {
            gtiRespoce.Clear();
            resetGame?.Invoke(); // Reset CardDack Script on EG
            AppData.GTIDATA = new JSONObject();
            if (SocketManagergame.Instance.Backgroundhandle)
            {
                Invoke(nameof(DelayReset), 0.3f);
            }
            return;
        }

        if (wlg == 0)
            AppData.isInstantLeave = true;
        gtiRespoce.Clear();
        AppData.canShowChallenge = true;

        if (SocketManagergame.Instance.Backgroundhandle)
        {
            Invoke(nameof(DelayReset), 0.3f);
        }
        Invoke(nameof(DealyShowAd), 0.5f);
    }

    private void DelayReset()
    {
        ResetTable(true);
    }

    private void DealyShowAd()
    {
        DashBoardOn();
        AppData.GTIDATA = new JSONObject();
        Loading_screen.instance.trapImage.SetActive(false);
        Logger.Print(TAG + " Showing Ad Bcz Is From Gameplay >> ");
        AdmobManager.instance.ShowInterstitialAd();
    }

    internal void DashBoardOn()
    {
        Logger.Print($"DashBoardOn == {playingScreen.activeInHierarchy}");
        Loading_screen.instance.trapImage.SetActive(false);
        allPlayingScreen.gameObject.SetActive(false);
        playingScreen.gameObject.SetActive(false);
        CommanAnimations.instance.FullScreenPanelAnimation(playingScreen, false, 0);
        CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel, false, 0);
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private void HandlePlayerRemoval(int seatIndex, TextMeshProUGUI playerName, RawImage playerImg, Image playerFrame, Button playerGiftBtn, Animator playerEmoji,
        RawImage playerGift, GameObject playerCardPile, TextMeshProUGUI playerCardCounter, Button playerImgButton)
    {
        Logger.Print(TAG + "Removed Player Seat Index >> " + seatIndex);
        playerName.text = "INVITE";
        playerImg.texture = inviteUser;
        playerFrame.sprite = defaultFrame;
        playerGiftBtn.gameObject.SetActive(false);
        playerEmoji.gameObject.SetActive(false);
        playerGift.enabled = false;
        playerGift.texture = DefaultGift;
        playerCardPile.SetActive(false);
        playerCardCounter.text = "0";

        playerImgButton.onClick.RemoveAllListeners();
        playerImgButton.onClick.AddListener(() =>
        {
            OnClick_InvitePlayers(seatIndex);
        });
    }

    private void OnRecevied_GST(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_GST called");

        //AppData.totalPlayedMatch = (AppData.totalPlayedMatch + 1) % 5;
        isTimerCountinue = true;
        CloseDeckAnim();

        if (GameWinner.instance.WinnerPannel.activeInHierarchy) StartCoroutine(GameWinner.instance.GameStartTimerStart(5));
        StartCoroutine(GameStartTimerStart(5));
    }

    bool isTimerCountinue = true;

    public IEnumerator GameStartTimerStart(int p)
    {
        GameStartTimer.SetActive(true);
        GameStartTimerTxt.text = "Game Start Within " + p + " sec...";

        for (int i = p; i > 0; i--)
        {
            if (isTimerCountinue)
            {
                GameStartTimerTxt.text = "Game Start Within " + i + " sec...";
                if (!isTournament) BacktoLobby = i > 2;
                if (i <= 2)
                {
                    MessagePanel.Instance.OnClick_CloseMessagePanel();
                    if (BackMenuPannel.activeSelf)
                        CommanAnimations.instance.PopUpAnimation(BackMenuPannel, backMenuBg, backMenuPopUp, Vector3.zero, false, false);
                    if (TableInfoPannel.activeSelf)
                        CommanAnimations.instance.PopUpAnimation(TableInfoPannel, tableInfoBg, tableInfoPopUp, Vector3.zero, false, false);
                }
                yield return new WaitForSeconds(1f);
            }
            else
            {
                GameStartTimer.SetActive(false);
                yield break;
            }
        }
        Logger.Print(TAG + "GSt Timer Over >>>> ::::");
        GameStartTimer.SetActive(false);

        if (AppData.isTutorialPlay)
        {
            TutorialManager.Instance.HandleTutorial(3);
        }

        if (GameWinner.instance.WinnerPannel.activeInHierarchy)
        {
            CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel.GetComponent<RectTransform>(), false);
        }
    }

    private void OnRecevied_CBV(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_CBV called");
        BacktoLobby = false;
        if (AppData.gameStartCount <= 2)
            AppData.gameStartCount++;
        AppData.totalPlayedMatch++;

        CollectBootValueAnimation(data["pv"]);
        BetValue.text = AppData.numDifferentiation(data["bv"]);

        DashboardManager.instance.totalMatch += 1;

        PrefrenceManager.ManageGameplay += 1;
        switch (PrefrenceManager.ManageGameplay)
        {
            case 5:
            case 10:
            case 15:
            case 25:
            case 40:
            case 50:
                //                CustomFirebaseEvents.SendEvent("TotalGamePlay" + PrefrenceManager.ManageGameplay);
                break;
        }
    }

    bool gameIsInBackground = false;
    private void OnApplicationPause(bool pause)
    {
        gameIsInBackground = pause;
        if (!AppData.isTutorialPlay)
            winAnim?.Kill();
        Logger.Print(TAG + " OnApplicationPause called " + pause);
    }

    private void CollectBootValueAnimation(long pv)
    {
        Logger.Print(TAG + " CollectBootValueAnimation called");

        if (gameIsInBackground)
        {
            PotValue.text = AppData.numDifferentiation(pv);
            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(0.2f);
        BottomCollectAnimFrm.SetActive(true);
        LeftCollectAnimFrm.SetActive(true);
        TopCollectAnimFrm.SetActive(true);
        RightCollectAnimFrm.SetActive(true);

        AudioManager.instance.AudioPlay(AudioManager.instance.collectBV);
        for (int i = 0; i < BottomCollectAnimFrm.transform.childCount; i++)
        {
            BottomCollectAnimFrm.transform.GetChild(i).gameObject.SetActive(true);

            sequence.SetDelay(0.04f);
            sequence.Insert(0f, BottomCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOScale(Vector3.one, 0.15f));
            sequence.Insert(0f, BottomCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOMove(CenterCollectAnimFrm.transform.position, 0.5f)).OnComplete(() =>
            {
                Logger.Print(TAG + " Animation Complete");
                sequence = DOTween.Sequence();

                for (int i = 0; i < BottomCollectAnimFrm.transform.childCount; i++)
                {
                    sequence.SetDelay(0.04f);
                    sequence.Insert(0f, BottomCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOMove(PotValue.transform.position, 0.5f));
                    sequence.Insert(0.3f, BottomCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOScale(Vector3.zero, 0.2f)).OnComplete(() =>
                    {
                        Logger.Print(TAG + " Final Animation Complete");
                        PotValue.text = AppData.numDifferentiation(pv);

                        BottomCollectAnimFrm.SetActive(false);

                        for (int i = 0; i < BottomCollectAnimFrm.transform.childCount; i++)
                        {
                            BottomCollectAnimFrm.transform.GetChild(i).gameObject.SetActive(false);
                            BottomCollectAnimFrm.transform.GetChild(i).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = BottomCollectAnimFrm.transform.GetComponent<RectTransform>().anchoredPosition;
                        }
                    });
                }
            });
        }

        //left collect
        sequence = DOTween.Sequence();
        for (int i = 0; i < LeftCollectAnimFrm.transform.childCount; i++)
        {
            LeftCollectAnimFrm.transform.GetChild(i).transform.localScale = Vector3.zero;
            LeftCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(true);
            sequence.SetDelay(0.04f);
            sequence.Insert(0f, LeftCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOScale(Vector3.one, 0.15f));
            sequence.Insert(0f, LeftCollectAnimFrm.transform.GetChild(i).transform.DOMove(CenterCollectAnimFrm.transform.position, 0.5f)).OnComplete(() =>
            {
                LeftCollectAnimFrm.SetActive(false);

                sequence = DOTween.Sequence();
                for (int i = 0; i < LeftCollectAnimFrm.transform.childCount; i++)
                {
                    LeftCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(false);
                    LeftCollectAnimFrm.transform.GetChild(i).transform.GetComponent<RectTransform>().anchoredPosition = LeftCollectAnimFrm.transform.GetComponent<RectTransform>().anchoredPosition;
                }
            });
        }

        //Top collect
        sequence = DOTween.Sequence();
        for (int i = 0; i < TopCollectAnimFrm.transform.childCount; i++)
        {
            TopCollectAnimFrm.transform.GetChild(i).transform.localScale = Vector3.zero;
            TopCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(true);
            sequence.SetDelay(0.04f);
            sequence.Insert(0f, TopCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOScale(Vector3.one, 0.15f));
            sequence.Insert(0f, TopCollectAnimFrm.transform.GetChild(i).transform.DOMove(CenterCollectAnimFrm.transform.position, 0.5f)).OnComplete(() =>
            {
                TopCollectAnimFrm.SetActive(false);

                sequence = DOTween.Sequence();
                for (int i = 0; i < TopCollectAnimFrm.transform.childCount; i++)
                {
                    TopCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(false);
                    TopCollectAnimFrm.transform.GetChild(i).transform.GetComponent<RectTransform>().anchoredPosition = TopCollectAnimFrm.transform.GetComponent<RectTransform>().anchoredPosition;
                }
            });
        }

        //right collect
        sequence = DOTween.Sequence();
        for (int i = 0; i < RightCollectAnimFrm.transform.childCount; i++)
        {
            RightCollectAnimFrm.transform.GetChild(i).transform.localScale = Vector3.zero;
            RightCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(true);
            sequence.SetDelay(0.04f);
            sequence.Insert(0f, RightCollectAnimFrm.transform.GetChild(i).gameObject.transform.DOScale(Vector3.one, 0.15f));
            sequence.Insert(0f, RightCollectAnimFrm.transform.GetChild(i).transform.DOMove(CenterCollectAnimFrm.transform.position, 0.5f)).OnComplete(() =>
            {
                RightCollectAnimFrm.SetActive(false);

                sequence = DOTween.Sequence();
                for (int i = 0; i < RightCollectAnimFrm.transform.childCount; i++)
                {
                    RightCollectAnimFrm.transform.GetChild(i).transform.gameObject.SetActive(false);
                    RightCollectAnimFrm.transform.GetChild(i).transform.GetComponent<RectTransform>().anchoredPosition = RightCollectAnimFrm.transform.GetComponent<RectTransform>().anchoredPosition;
                }
            });
        }
    }

    bool isInUNO = false;
    private void OnRecevied_UTS(JSONNode data)
    {
        if (BottomDealCard.activeInHierarchy)
            BottomDealCard.SetActive(false);

        if (BottomThrowAnimCard.activeInHierarchy)
            BottomThrowAnimCard.gameObject.SetActive(false);

        Logger.NormalLog($"VVV::: Leave Btn {leaveButton.interactable} || isTournament = {isTournament} | isTutorialPlay = {AppData.isTutorialPlay}");

        //hintActionToolTip.gameObject.SetActive(false);

        if (isTournament && !leaveButton.interactable && !AppData.isTutorialPlay)
            leaveButton.interactable = true;

        if (KeepPlayPanel.activeInHierarchy && ChallengeData == "")
            CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);

        StopDeckGlow();

        Logger.Print(TAG + " OnRecevied_UTS called");

        speceficHelpInfo.localScale = Vector3.zero;
        isClickSpecificBtn = isChooseSpecific = false;

        closeDeckCounter = data["closedeck"].Count;

        BacktoLobby = true;
        if (isChallenge) isChallenge = false;

        if (!UnoBtn.activeInHierarchy) UnoBtn.SetActive(true);
        if (!messageBtn.gameObject.activeInHierarchy) messageBtn.gameObject.SetActive(true);

        lastPickCard = "";
        isUno = MyTurn = isKeep = isDeckClickable = false;

        List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
        selectedCard = JsonConvert.DeserializeObject<List<string>>(data["SelectedCard"].ToString());

        var ss = PlayerCard.Any(x => x.Equals(selectedCard.Any(y => y == x)));

        string CS = data["cs"];
        int plus = BottomSeatIndex;
        isFlipStatus = data["isFlip"].AsBool;

        Logger.Print($"AllPlayercards CardsCount = {data["AllPlayercards"].Count} || plus = {plus} |isFlipStatus: {isFlipStatus}");

        CenterGlowCard.reverse = data["isReverse"].AsBool;

        CenterGlowCard.SetRingColor(CS);
        fillTweenAnim?.Kill();

        float delay;
        if (data["nt"].AsInt == BottomSeatIndex)
            delay = 0.5f;
        else
            delay = 0;

        StartCoroutine(DelayUserTurnStart(data, PlayerCard, CS, false, delay));
    }

    private IEnumerator DelayUserTurnStart(JSONNode data, List<string> PlayerCard, string CS, bool isChallenge = false, float timeDelay = 0)
    {
        yield return new WaitForSeconds(timeDelay);

        int tt = data["tt"];

        if (data["nt"].AsInt == BottomSeatIndex)
        {
            if (isPlayClickAnimTime) isPlayClickAnimTime = false;
            isKeepedCard = false;
            isKeepPopupOn = false;
            MyTurn = true;
            isDontTouch = false;
            isDeckClickable = true;
            Logger.NormalLog($"isDontTouch  DelayUserTurnStart : {isDontTouch}");
            AudioManager.instance.AudioPlay(AudioManager.instance.userTurn);
            AudioManager.instance.PlayVibration();
            UTSAnimation(playerFrms[0].transform);

            bool isDrawPenltyButton = data["isDrawPenltyButton"].AsInt > 0;
            drawBtn.gameObject.SetActive(isDrawPenltyButton);
            if (isDrawPenltyButton) drawBtnText.text = $"Draw +{data["isDrawPenltyButton"].AsInt} card";

            Logger.NormalLog($"isPfcdClickAble  condition : {data["isDrawPenltyButton"].AsInt == 0} | {data["totalpenaltyCard"].AsInt == 0} | {data["wildupcounter"].AsInt == 0}");
            bool isPfcdClickAble = data["isDrawPenltyButton"].AsInt == 0 && data["totalpenaltyCard"].AsInt == 0 && data["wildupcounter"].AsInt == 0;
            cardBunch.GetComponent<Button>().interactable = isPfcdClickAble;

            StartUserTurnTimer(BottomPlayerTurnRing, tt, true);
            BottomPlayerCard = PlayerCard;

            hintUnoToolTip.transform.gameObject.SetActive(data["ishintUno"].AsBool);
            if (data["ishintUno"].AsBool)
            {
                hintUnoToolTip.transform.localScale = Vector3.one;
                hintUnoToolTip.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1).SetLoops(-1, LoopType.Yoyo);
            }
            else
                DOTween.Kill(hintUnoToolTip);

            string ltc = IsFlipModeCardStr(data["ltc"]);

            RedrawCard(BottomSeatIndex);
            if (!isChallenge)
                UserCardClicable(ltc);

            BottomPlayerCardCounter.text = PlayerCard.Count + "";
            isInUNO = data["sayUno"].AsBool;
            Logger.Print(TAG + "Say Uno Bool >>> " + data["sayUno"].AsBool);
        }
        else if (data["nt"].AsInt == LeftSeatIndex)
        {
            UTSAnimation(playerFrms[1].transform);

            StartUserTurnTimer(LeftPlayerTurnRing, tt);
            LeftPlayerCard = PlayerCard;
            LeftPlayerCardCounter.text = PlayerCard.Count + "";
            RedrawCard(LeftSeatIndex);//Testing
        }
        else if (data["nt"].AsInt == RightSeatIndex)
        {
            UTSAnimation(playerFrms[3].transform);

            StartUserTurnTimer(RightPlayerTurnRing, tt);
            RightPlayerCard = PlayerCard;
            RightPlayerCardCounter.text = PlayerCard.Count + "";
            RedrawCard(RightSeatIndex);//Testing
        }
        else if (data["nt"].AsInt == TopSeatIndex)
        {
            UTSAnimation(playerFrms[2].transform);
            StartUserTurnTimer(TopPlayerTurnRing, tt);
            TopPlayerCard = PlayerCard;
            TopPlayerCardCounter.text = PlayerCard.Count + "";
            Logger.Print(TAG + "Top Player Cards ; " + TopPlayerCard.Count);
            RedrawCard(TopSeatIndex);
        }
    }

    public void UTSAnimation(Transform nextPlayerFrm)
    {
        for (int i = 0; i < playerFrms.Count; i++)
        {
            if (playerFrms[i].transform.localScale != Vector3.one)
                playerFrms[i].transform.DOScale(Vector3.one, 0.1f);
        }

        if (nextPlayerFrm)
        {
            if (nextPlayerFrm.localScale == Vector3.one)
                nextPlayerFrm.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).SetDelay(0.1f);
        }
    }

    int clicableCard = 0;
    private void UserCardClicable(string ltc = "")
    {
        Logger.Print(TAG + $" User Card Clickable Called >> ClikckAbleCardCount : {selectedCard.Count}");
        clicableCard = 0;
        clicableCard = selectedCard.Count;
        Logger.NormalLog($"clicableCard ==  {clicableCard}");

        // Original logic for normal card checking
        PlayerController player = CardDeckController.instance.playerData[0];
        for (int i = 0; i < BottomPlayerCard.Count; i++)
        {
            player.myCards[i].enabled = false;

            string cardId = player.myCards[i].cardValue;
            if (selectedCard.Contains(cardId))
            {
                if (player.myCards[i].gameObject.activeInHierarchy)
                    player.myCards[i].SetCardGlowAnimation(true);
                player.myCards[i].disableImg.gameObject.SetActive(false);

            }
            else
            {
                if (player.myCards[i].gameObject.activeInHierarchy)
                    player.myCards[i].SetCardGlowAnimation(false);
                player.myCards[i].disableImg.gameObject.SetActive(true);
            }
        }

        //
        if (mode.Equals(AppData.CLASSIC) && rules.Count == 0)
            if (!hintActionToolTip.gameObject.activeInHierarchy && !ltc.Equals(""))
                StartCoroutine(ReSelectCardForTutorial(ltc, player));

        Logger.Print(TAG + " Challenege Data is >> " + ChallengeData.ToString() + " Condition >> " + (ChallengeData == "") + " clicableCard = " + clicableCard);

        if (clicableCard == 0 && (ChallengeData == ""))
            DeckGlowAnimation();

        Invoke(nameof(DelayClick), .6f);
    }

    private IEnumerator ReSelectCardForTutorial(string ltc, PlayerController player)
    {
        yield return new WaitForSeconds(0.3f);
        bool colorMatch = PrefrenceManager.colorChangeStatus == 0 && selectedCard.Any(x => getCardColor(x) == getCardColor(ltc));
        bool numberMatch = PrefrenceManager.matchNumberStatus == 0 && selectedCard.Any(x => getCardValue(x) == getCardValue(ltc));

        Logger.Print($" IS Macth Color: {colorMatch} | IS Match Number: {numberMatch} | ltc: {ltc}");

        if (colorMatch || numberMatch)
        {
            CancelInvoke(nameof(ShowHintToolTip));
            for (int i = 0; i < BottomPlayerCard.Count; i++)
            {
                player.myCards[i].enabled = false;
                if (player.myCards[i].gameObject.activeInHierarchy)
                    player.myCards[i].SetCardGlowAnimation(false);

                DOTween.Kill(player.myCards[i].glowImg);
                player.myCards[i].disableImg.gameObject.SetActive(true);
            }
        }

        GameObject handTutorial;
        if (PrefrenceManager.clickSwipeStatus == 0)
        {
            handTutorial = playingClickHand.gameObject;
            playingClickHand.gameObject.SetActive((colorMatch || numberMatch)); // Animation Hand
        }
        else
        {
            handTutorial = playingHandTutorial.gameObject;
            playingHandTutorial.gameObject.SetActive((colorMatch || numberMatch)); // Animation Hand
        }
        isPlayingHint = false;

        if (colorMatch)
        {
            isPlayingHint = true;
            List<string> colorMatchList = selectedCard
    .Where(x => getCardColor(x) == getCardColor(ltc)) // Match cards based on color
    .Select(x => x.ToString()) // Convert each matching card to string or appropriate representation
    .ToList();

            Logger.Print(TAG + "colorMatchList: " + string.Join(", ", colorMatchList));
            int index = -1;
            for (int i = 0; i < player.myCards.Count; i++)
            {
                if (colorMatchList.Contains(player.myCards[i].cardValue))
                {
                    index = i;
                    if (player.myCards[i].gameObject.activeInHierarchy)
                        player.myCards[i].SetCardGlowAnimation(true);
                    player.myCards[i].disableImg.gameObject.SetActive(false);
                }
            }

            if (index != -1)
                handTutorial.transform.position = player.myCards[index].transform.position;

            hintActionToolText.text = $"{TutorialActionSteps.matchColor}";
            hintActionToolTip.gameObject.SetActive(true);
        }
        else if (numberMatch)
        {
            isPlayingHint = true;
            Logger.Print($" HAS CARD L {selectedCard.Any(c => getCardValue(c) == 19)}");
            if (selectedCard.Any(c => getCardValue(c) == 19) && getCardValue(ltc) != 19)
            {
                for (int i = 0; i < BottomPlayerCard.Count; i++)
                {
                    player.myCards[i].enabled = false;

                    string cardId = player.myCards[i].cardValue;
                    if (selectedCard.Contains(cardId))
                    {
                        if (player.myCards[i].gameObject.activeInHierarchy)
                            player.myCards[i].SetCardGlowAnimation(true);
                        player.myCards[i].disableImg.gameObject.SetActive(false);

                    }
                    else
                    {
                        if (player.myCards[i].gameObject.activeInHierarchy)
                            player.myCards[i].SetCardGlowAnimation(false);
                        player.myCards[i].disableImg.gameObject.SetActive(true);
                    }
                }

                handTutorial.gameObject.SetActive(false);
                yield break;
            }

            List<string> numberMatchList = selectedCard
   .Where(x => getCardValue(x) == getCardValue(ltc)) // Match cards based on number
   .Select(x => x.ToString()) // Convert each matching card to string or appropriate representation
   .ToList();

            Logger.Print(TAG + "numberMatchList: " + string.Join(", ", numberMatchList));
            int index = -1;
            for (int i = 0; i < player.myCards.Count; i++)
            {
                if (numberMatchList.Contains(player.myCards[i].cardValue))
                {
                    index = i;
                    if (player.myCards[i].gameObject.activeInHierarchy)
                        player.myCards[i].SetCardGlowAnimation(true);
                    player.myCards[i].disableImg.gameObject.SetActive(false);
                }
            }

            if (index != -1)
                handTutorial.transform.position = player.myCards[index].transform.position;
            hintActionToolText.text = $"{TutorialActionSteps.matchNumber}";
            hintActionToolTip.gameObject.SetActive(true);
            Logger.Print($"isHintShow C :: {true}");
        }
    }

    private void DelayClick() // FOR ALL CARDS CLICKABLE TRUE
    {
        for (int i = 0; i < BottomPlayerCard.Count; i++)
        {
            if (i < CardDeckController.instance.playerData[0].myCards.Count) // < ---  TrapcardDestroy na lidhe update nai thai hoy
            {
                if (CardDeckController.instance.playerData[0].myCards[i].gameObject != null) // FOR CHECK IF OBJECT IS DESTTROYED = NOT RUN || already added
                    CardDeckController.instance.playerData[0].myCards[i].enabled = true;
            }
        }
        Logger.Print($" DelayClick  == {isDontTouch}");
    }

    private void OnRecevied_SMC(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_SMC called");

        if (AppData.isTutorialPlay)
        {
            BottomPlayerCard.AddRange(TutorialManager.Instance.myCards);
            TotalDealCard = BottomPlayerCard.Count;
            Logger.Print($"SMC :: {BottomPlayerCard.Count}");
        }
        else
        {
            BottomPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["cards"].ToString());
            TopPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["partnerCard"].ToString());
            TotalDealCard = BottomPlayerCard.Count;
        }
        CreateDynamicCard();
    }

    private void OnCardCloseDackAnimation()
    {
        int end = BottomPlayerCard.Count > MyCardGrid.transform.childCount ? BottomPlayerCard.Count : MyCardGrid.transform.childCount;
        Logger.Print(TAG + "Redraw Going To Do BottomPlayerCard = " + BottomPlayerCard.Count + " | child = " + MyCardGrid.transform.childCount + " | End Val = " + end);
        CardDeckController.instance.playerData[0].myCards.Clear();
        for (int i = 0; i < end; i++)
        {
            if (i < BottomPlayerCard.Count)//3
            {
                if (i < MyCardGrid.transform.childCount)//4
                {
                    CardController myCard = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();
                    myCard.enabled = true;
                    myCard.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                    myCard.cardValue = BottomPlayerCard[i];
                }
                else
                {
                    Debug.Log($" Viren Redraw :: = {i}");
                    GameObject img = Instantiate(SpriteImg, MyCardGrid.transform);
                    img.transform.GetComponent<CardController>().enabled = true;

                    img.transform.GetComponent<CardController>().flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                    img.transform.GetComponent<CardController>().cardValue = BottomPlayerCard[i];
                    img.name = $"{i}";
                }
                CardDeckController.instance.AddCardOnList(0, MyCardGrid.transform.GetChild(i).GetComponent<CardController>());
            }
            else
            {
                Logger.Print($" Redraw Going To Do Card Object Destroyed OnCardCloseDackAnimation>> " + MyCardGrid.transform.GetChild(i).name + " i = " + i);
                if (!addedPaneltyCards.Contains(MyCardGrid.transform.GetChild(i).gameObject))
                {
                    CardDeckController.instance.RemoveSelfPlayerCard(MyCardGrid.transform.GetChild(i).gameObject);
                    Destroy(MyCardGrid.transform.GetChild(i).gameObject);
                }
            }
        }
        CardDeckController.instance.AddSelfPlayerCard();
        animPosCard.transform.DOScale(Vector3.zero, 0.2f);
        CardDeckController.instance.CardAnim();
    }

    internal void WinRedrawCard(int si, List<string> cards)
    {
        if (si == TopSeatIndex && (!mode.Equals(AppData.PARTNER) || !mode.Equals(AppData.EMOJIPARTNER)))
        {
            TopPlayerCard = cards;
        }
        else if (si == LeftSeatIndex)
        {
            LeftPlayerCard = cards;
        }
        else if (si == RightSeatIndex)
        {
            RightPlayerCard = cards;
        }

        if (si == TopSeatIndex)
        {
            int end = TopPlayerCard.Count > partnerCardGrid.transform.childCount ? TopPlayerCard.Count : partnerCardGrid.transform.childCount;
            Logger.Print(TAG + " PartnerPlayerCard " + TopPlayerCard.Count + " child " + partnerCardGrid.transform.childCount);

            for (int i = 0; i < end; i++)
            {
                if (i < TopPlayerCard.Count)
                {
                    Sprite spr = IsFlipModeSprite(TopPlayerCard[i], isFlipStatus);

                    if (i < partnerCardGrid.transform.childCount)
                    {
                        partnerCardGrid.transform.GetChild(i).gameObject.SetActive(true);
                        partnerCardGrid.transform.GetChild(i).GetComponent<Image>().sprite = spr;
                        partnerCardGrid.transform.GetChild(i).gameObject.name = i + "";
                    }
                    else
                    {
                        GameObject img = Instantiate(SpriteImg, partnerCardGrid.transform);
                        img.transform.GetComponent<Image>().sprite = spr;
                        img.name = i + "";
                    }
                }
                else
                {
                    Logger.Print("Top Card Destoryed >>");
                    Destroy(partnerCardGrid.transform.GetChild(i).gameObject);
                }
            }
        }

        else if (si == LeftSeatIndex)
        {
            int end = LeftPlayerCard.Count > leftPlayerCardGrid.transform.childCount ? LeftPlayerCard.Count : leftPlayerCardGrid.transform.childCount;
            Logger.Print(TAG + " LeftPlayerCard " + LeftPlayerCard.Count + " child " + leftPlayerCardGrid.transform.childCount + "Redraw");
            for (int i = 0; i < end; i++)
            {
                if (i < LeftPlayerCard.Count)
                {
                    Sprite spr = IsFlipModeSprite(LeftPlayerCard[i], isFlipStatus);

                    if (i < leftPlayerCardGrid.transform.childCount)
                    {
                        leftPlayerCardGrid.transform.GetChild(i).gameObject.SetActive(true);
                        leftPlayerCardGrid.transform.GetChild(i).GetComponent<Image>().sprite = spr;
                        leftPlayerCardGrid.transform.GetChild(i).gameObject.name = i + "";
                    }
                    else
                    {
                        GameObject img = Instantiate(SpriteImg, leftPlayerCardGrid.transform);
                        Logger.Print($" left i = {i}");
                        Logger.Print($" LeftPlayerCard i = {LeftPlayerCard[i]}");
                        img.transform.GetComponent<Image>().sprite = spr;
                        img.name = i + "";
                    }
                }
                else
                {
                    Logger.Print("Left Card Destoryed >>");
                    Destroy(leftPlayerCardGrid.transform.GetChild(i).gameObject);
                }
            }
        }

        else if (si == RightSeatIndex)
        {
            int end = RightPlayerCard.Count > rightPlayerCardGrid.transform.childCount ? RightPlayerCard.Count : rightPlayerCardGrid.transform.childCount;
            Logger.Print(TAG + " RightPlayerCard " + RightPlayerCard.Count + " child " + rightPlayerCardGrid.transform.childCount + " Redraw");
            for (int i = 0; i < end; i++)
            {
                if (i < RightPlayerCard.Count)
                {
                    Sprite spr = IsFlipModeSprite(RightPlayerCard[i], isFlipStatus);

                    if (i < rightPlayerCardGrid.transform.childCount)
                    {
                        rightPlayerCardGrid.transform.GetChild(i).gameObject.SetActive(true);
                        rightPlayerCardGrid.transform.GetChild(i).GetComponent<Image>().sprite = spr;
                        rightPlayerCardGrid.transform.GetChild(i).gameObject.name = i + "";
                    }
                    else
                    {
                        GameObject img = Instantiate(SpriteImg, rightPlayerCardGrid.transform);
                        img.transform.GetComponent<Image>().sprite = spr;
                        img.name = i + "";
                    }
                }
                else
                {
                    Logger.Print("Right Card Destoryed >>");
                    Destroy(rightPlayerCardGrid.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    private void RedrawCard(int si, bool rejoinSelfWildC = false)
    {
        Logger.Print(TAG + " Redraw Called " + si + " Is Child Count >> " + MyCardGrid.transform.childCount);
        Logger.Print(TAG + " Redraw Called Is Child Count BB>> " + BottomPlayerCard.Count);
        Vector2 botFlipTagsize = new Vector2(30, 10);
        Vector2 botFlipTagPos = new Vector2(-6, -4.5f);

        if (!mode.Equals(AppData.FLIP))
            isFlipStatus = false;

        PlayerController leftPl = CardDeckController.instance.playerData[1];
        PlayerController topPl = CardDeckController.instance.playerData[2];
        PlayerController rightPl = CardDeckController.instance.playerData[3];

        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();

        bool isCrashCheck = false;
        bool ischreshCheck2 = false;
        try
        {
            if (si == BottomSeatIndex)
            {
                CardDeckController.instance.playerData[0].myCards.Clear();

                Logger.Print($"b: {BottomPlayerCard.Count} | m : {MyCardGrid.transform.childCount}");
                int end = Mathf.Max(BottomPlayerCard.Count, MyCardGrid.transform.childCount);
                Logger.Print(TAG + "Redraw Going To Do BottomPlayerCard = " + BottomPlayerCard.Count + " | child = " + MyCardGrid.transform.childCount + " | End Val = " + end);
                for (int i = 0; i < end; i++)
                {
                    if (i < BottomPlayerCard.Count)//3
                    {
                        Sprite spr = IsFlipModeSprite(BottomPlayerCard[i], isFlipStatus);
                        string cardS = IsFlipModeCardStr(BottomPlayerCard[i]); // USE FOR LOG

                        if (i < MyCardGrid.transform.childCount)//4
                        {
                            CardController myCard = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();
                            Logger.Print($" Viren Redraw :: = {i} | Active ? {myCard.gameObject.activeInHierarchy}");
                            myCard.gameObject.SetActive(addedPaneltyCards.Contains(MyCardGrid.transform.GetChild(i).gameObject) ? false : true); // ??

                            myCard.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            myCard.myRect.sizeDelta = new Vector2(148, 196);
                            myCard.myRect.localScale = Vector3.one * 1.3f;
                            myCard.cardImage.sprite = spr;
                            myCard.enabled = true;
                            myCard.cardValue = $"{BottomPlayerCard[i]}";
                            myCard.disableImg.gameObject.SetActive(false);

                            Logger.Print($" Viren Redraw getCardColor :: = {getCardColor(cardS).Equals("k")} || {getCardColor(cardS)}");

                            myCard.posY = 0;
                            if (!myCard.gameObject.activeInHierarchy) myCard.gameObject.SetActive(true);
                            myCard.gameObject.name = $"{i}";
                        }
                        else
                        {
                            Logger.Print($" Viren Redraw Spawn :: = {i}");
                            CardController img = Instantiate(SpriteImg.GetComponent<CardController>(), MyCardGrid.transform);

                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.myRect.sizeDelta = new Vector2(148, 196);
                            img.myRect.localScale = Vector3.one * 1.3f;
                            img.cardImage.sprite = spr;
                            img.enabled = true;
                            img.cardValue = $"{BottomPlayerCard[i]}";
                            img.disableImg.gameObject.SetActive(false);
                            Logger.Print($" Viren Redraw getCardColor :: = {getCardColor(cardS).Equals("k")}");

                            img.posY = 0;
                            img.name = $"{i}";
                        }
                        CardController c = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();
                        if (rejoinSelfWildC)
                        {
                            if (MyCardGrid.transform.GetChild(i).GetComponent<CardController>().cardValue == isSelectCardStr)
                            {
                                isSelectCardStr = "";
                                MyCardGrid.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                        CardDeckController.instance.AddCardOnList(0, c);
                    }
                    else
                    {
                        // If BottomPlayerCard count is less than i, handle out of bounds logic
                        if (i < MyCardGrid.transform.childCount)
                        {
                            CardController c = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();
                            Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> " + c.name + " i = " + i);
                            isCrashCheck = true;
                            if (!addedPaneltyCards.Contains(c.gameObject))
                            {
                                if (c.gameObject != null)
                                {
                                    Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> " + c.gameObject.name);
                                    CardDeckController.instance.RemoveSelfPlayerCard(c.gameObject);
                                    Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> Next? ");
                                    DestroyImmediate(c.gameObject);
                                }
                                ischreshCheck2 = true;
                            }
                        }
                        //CardController c = MyCardGrid.transform.GetChild(i).GetComponent<CardController>();
                        //Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> " + c.name + " i = " + i);
                        //if (!addedPaneltyCards.Contains(MyCardGrid.transform.GetChild(i).gameObject))
                        //{
                        //    if (c.gameObject != null)
                        //    {
                        //        Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> " + c.gameObject.name);
                        //        CardDeckController.instance.RemoveSelfPlayerCard(c.gameObject);
                        //        Logger.Print(TAG + " Redraw Going To Do Card Object Destroyed >> Next? ");
                        //        DestroyImmediate(c.gameObject);
                        //    }
                        //}
                    }
                }
                CardDeckController.instance.AddSelfPlayerCard();
                Logger.Print(TAG + "Redraw Going To Do After Redraw My Grid Card >> " + MyCardGrid.transform.childCount + " >> Bottom Player Card >> " + BottomPlayerCard.Count);
            }

            else if (si == TopSeatIndex)
            {
                if (!topGrid.enabled) topGrid.enabled = true;

                int end = Mathf.Max(TopPlayerCard.Count, partnerCardGrid.transform.childCount);
                Logger.Print(TAG + " d " + TopPlayerCard.Count + " child " + partnerCardGrid.transform.childCount);
                CardDeckController.instance.playerData[2].myCards.Clear();
                for (int i = 0; i < end; i++)
                {
                    if (i < TopPlayerCard.Count)
                    {
                        Sprite spr = IsFlipModeSprite(TopPlayerCard[i], (mode.Equals(AppData.FLIP) ? !isFlipStatus : isFlipStatus));

                        if (i < partnerCardGrid.transform.childCount)
                        {
                            CardController img = partnerCardGrid.transform.GetChild(i).GetComponent<CardController>();

                            img.gameObject.SetActive(addedPartnerPaneltyCards.Contains(partnerCardGrid.transform.GetChild(i).gameObject) ? false : true);

                            if (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER) || (mode.Equals(AppData.FLIP)))
                                img.cardImage.sprite = spr;

                            else
                                img.cardImage.sprite = cardSprite.sprite;

                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            partnerCardGrid.transform.GetChild(i).gameObject.name = i + "";

                            /*img.cardImage.sprite = spr;*/

                        }
                        else
                        {
                            CardController img = Instantiate(SpriteImg.GetComponent<CardController>(), partnerCardGrid.transform);

                            //img.transform.GetComponent<Image>().sprite = GetSprite(TopPlayerCard[i]);

                            if ((mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) || mode.Equals(AppData.FLIP))
                                img.cardImage.sprite = spr;
                            else
                                img.cardImage.sprite = cardSprite.sprite;
                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            img.name = i + "";

                            /*img.cardImage.sprite = spr;*///
                        }
                        CardController c = partnerCardGrid.transform.GetChild(i).GetComponent<CardController>();
                        topPl.myCards.Add(c);
                    }
                    else
                    {
                        Logger.Print("Top Card Destoryed >>");
                        Destroy(partnerCardGrid.transform.GetChild(i).gameObject);
                    }
                }
            }

            else if (si == LeftSeatIndex)
            {
                if (!leftGrid.enabled) leftGrid.enabled = true;

                CardDeckController.instance.playerData[1].myCards.Clear();
                int end = Mathf.Max(LeftPlayerCard.Count, leftPlayerCardGrid.transform.childCount);
                Logger.Print(TAG + " LeftPlayerCard " + LeftPlayerCard.Count + " child " + leftPlayerCardGrid.transform.childCount + "Redraw " + " || end = " + end);
                for (int i = 0; i < end; i++)
                {
                    if (i < LeftPlayerCard.Count)
                    {
                        Sprite spr = IsFlipModeSprite(LeftPlayerCard[i], (mode.Equals(AppData.FLIP) ? !isFlipStatus : isFlipStatus));
                        if (i < leftPlayerCardGrid.transform.childCount)
                        {
                            CardController img = leftPlayerCardGrid.transform.GetChild(i).GetComponent<CardController>();

                            img.gameObject.SetActive(addedPartnerPaneltyCards.Contains(leftPlayerCardGrid.transform.GetChild(i).gameObject) ? false : true);
                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            img.cardImage.sprite = mode.Equals(AppData.FLIP) ? spr : cardSprite.sprite;
                            img.gameObject.name = i + "";

                            /*img.cardImage.sprite = spr;*///
                        }
                        else
                        {
                            CardController img = Instantiate(SpriteImg.GetComponent<CardController>(), leftPlayerCardGrid.transform);

                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            img.cardImage.sprite = mode.Equals(AppData.FLIP) ? spr : cardSprite.sprite;
                            img.name = i + "";

                            /*img.cardImage.sprite = spr;*///
                        }

                        CardController c = leftPlayerCardGrid.transform.GetChild(i).GetComponent<CardController>();
                        leftPl.myCards.Add(c);
                    }
                    else
                    {
                        Logger.Print("Left Card Destoryed >>");
                        Destroy(leftPlayerCardGrid.transform.GetChild(i).gameObject);
                    }
                }
            }

            else if (si == RightSeatIndex)
            {
                if (!rightGrid.enabled) rightGrid.enabled = true;

                CardDeckController.instance.playerData[3].myCards.Clear();
                int end = Mathf.Max(RightPlayerCard.Count, rightPlayerCardGrid.transform.childCount);
                Logger.Print(TAG + " RightPlayerCard " + RightPlayerCard.Count + " child " + rightPlayerCardGrid.transform.childCount + " Redraw");
                for (int i = 0; i < end; i++)
                {
                    if (i < RightPlayerCard.Count)
                    {
                        Sprite spr = IsFlipModeSprite(RightPlayerCard[i], (mode.Equals(AppData.FLIP) ? !isFlipStatus : isFlipStatus));
                        if (i < rightPlayerCardGrid.transform.childCount)
                        {
                            CardController img = rightPlayerCardGrid.transform.GetChild(i).GetComponent<CardController>();

                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            img.gameObject.SetActive(addedPartnerPaneltyCards.Contains(rightPlayerCardGrid.transform.GetChild(i).gameObject) ? false : true);
                            img.cardImage.sprite = mode.Equals(AppData.FLIP) ? spr : cardSprite.sprite;
                            img.gameObject.name = i + "";

                            /*img.cardImage.sprite = spr;*/
                        }
                        else
                        {
                            CardController img = Instantiate(SpriteImg.GetComponent<CardController>(), rightPlayerCardGrid.transform);

                            img.flipModeTag.gameObject.SetActive(mode.Equals(AppData.FLIP));
                            img.flipModeTag.sizeDelta = botFlipTagsize;
                            img.flipModeTag.anchoredPosition = botFlipTagPos;
                            img.cardImage.sprite = mode.Equals(AppData.FLIP) ? spr : cardSprite.sprite;
                            img.cardValue = RightPlayerCard[i];
                            img.name = i + "";

                            /*img.cardImage.sprite = spr;*///
                        }

                        CardController c = rightPlayerCardGrid.transform.GetChild(i).GetComponent<CardController>();
                        rightPl.myCards.Add(c);
                    }
                    else
                    {
                        Logger.Print("Right Card Destoryed >>");
                        Destroy(rightPlayerCardGrid.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            JSONNode objects = new JSONObject
            {
                ["si"] = si,
                ["bottomSeat"] = BottomSeatIndex,
                ["leftSeat"] = LeftSeatIndex,
                ["topSeat"] = TopSeatIndex,
                ["rightSeat"] = RightSeatIndex,

                ["bottomPlayerCard"] = BottomPlayerCard.Count,
                ["leftPlayerCard"] = LeftPlayerCard.Count,
                ["topPlayerCard"] = TopPlayerCard.Count,
                ["rightPlayerCard"] = RightPlayerCard.Count,

                ["partnerCardGrid"] = partnerCardGrid.transform.childCount,
                ["bottomCardGrid"] = MyCardGrid.transform.childCount,

                ["Else bottom 1Cross: "] = isCrashCheck,
                ["Else bottom 2Cross: "] = ischreshCheck2,


                ["rejoinSelfWildC"] = rejoinSelfWildC,
            };
            Loading_screen.instance.SendExe("GameManager", "RedrawCard", $"{objects}", ex);
            Logger.Print($"Ex on redraw : {ex}");
            //Debug.Break();
        }
        Logger.NormalLog($"======================================>>>>>>>>>>>>>>END ");
    }

    public bool ControlTouch()
    {
        //Logger.Print($"{TAG} |1|  {(!KeepPlayPanel.activeInHierarchy)} |2| {MyTurn} |3| {!isDontTouch} |4| {!isKeepPopupOn} |5| {!isChallenge} |6| {!isPaneltyAnimOn}");
        return ((!KeepPlayPanel.activeInHierarchy) && (MyTurn && !isDontTouch && !isKeepPopupOn && !isChallenge && !isPaneltyAnimOn));
        //return (/*(!KeepPlayPanel.activeInHierarchy) && */(MyTurn && !isDontTouch && /*!isKeepPopupOn &&*/ !isChallenge && !isPaneltyAnimOn)); 
    }

    public void SelectWildCardCall(JSONNode j)
    {
        drawBtn.gameObject.SetActive(false);
        ProfilePanel.instance.ProfilePanelClick(10);
        colorBlackBg.DOFade(1, 0);
        if (KeepPlayPanel.activeInHierarchy) CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false);

        for (int i = 0; i < SelectColorImg.Length; i++)
            SelectColorImg[i].sprite = isFlipStatus ? setFlipColor[i] : ResetColor[i];

        CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.one, true);

        var vec = new Vector2(97, 5);
        if (BottomThrowCard.transform.localPosition.x != vec.x || BottomThrowCard.transform.localPosition.y != vec.y)
        {
            Vector2 allpos = new Vector2(97, 5);
            BottomThrowCard.transform.localPosition = LeftThrowCard.transform.localPosition = TopThrowCard.transform.localPosition = RightThrowCard.transform.localPosition = allpos;
            if (wildDummyShowObject.gameObject.activeInHierarchy) wildDummyShowObject.gameObject.SetActive(false);
        }

        if (j != null)
            if (j["si"].AsInt == BottomSeatIndex)
            {
                BottomThrowCard.transform.SetSiblingIndex(4);
                BottomThrowCard.SetActive(true);
            }
        Logger.Print($"SELECTWILDCARDCALL RESET DACK POSITION:  {CardDeckController.instance.playerData[0].myCards.Count}");
    }

    [SerializeField] bool isDontTouch;
    bool isSayUnoResGet;

    public void CardSelectEventSend(string card, CardController c)
    {
        MyTurn = false;
        string str = IsFlipModeCardStr(card);

        if (isPlayingHint)
        {
            bool colorMatch = PrefrenceManager.colorChangeStatus == 0 && selectedCard.Any(x => getCardColor(x) == getCardColor(c.cardValue));
            bool numberMatch = PrefrenceManager.matchNumberStatus == 0 && selectedCard.Any(x => getCardValue(x) == getCardValue(c.cardValue));
            if (colorMatch)
            {
                PrefrenceManager.colorChangeStatus = 1;
            }
            else if (numberMatch)
            {
                PrefrenceManager.matchNumberStatus = 1;
            }
            PrefrenceManager.clickSwipeStatus = 1;
            isPlayingHint = false;
            playingHandTutorial.gameObject.SetActive(false);
            playingClickHand.gameObject.SetActive(false);
            ShowHintToolTip();
        }


        if (getCardColor(str).Equals("k"))
        {
            EventHandler.SelectCardReq(card);
            for (int i = 0; i < allColorButtons.Length; i++) allColorButtons[i].interactable = true;
            ChooseColorText.SetActive(false);
            cIndex = int.Parse(c.name);

            MyPlayerCardThrowAnimation(c, BottomThrowCard.transform, true); // Wild Throw Card animation         
        }
        else if (rules.Contains(6) && getCardValue(card) == 7)
        {
            swapCard = card;
            ShowSwapButtons(true);
        }
        else
        {
            EventHandler.ThrowCard(card, getCardColor(str), isUno);
        }
    }

    internal void CardClickListner(CardController card)
    {
        Logger.Print(TAG + "VVC Card Clicked & My Turn = " + MyTurn + " &&isDontTouch = " + isDontTouch + " || isSayUnoResGet = " + isSayUnoResGet + " || isKeepPopupOn = " + isKeepPopupOn);

        string cardS = IsFlipModeCardStr(card.cardValue);

        if (KeepPlayPanel.activeInHierarchy) return;
        if (MyTurn && !isDontTouch &&/* !isKeepPopupOn &&*/ !isChallenge)
        {
            cIndex = int.Parse(card.name);
            throwIndex = cIndex;
            MyTurn = false;
            isDontTouch = true;

            Logger.Print($"Card name : {card.name} || cIndex = {cIndex}");

            var cardSuit = getCardColor(cardS);

            var suitGroups = BottomPlayerCard.GroupBy(c => getCardColor(c));

            var sameSuitGroup = suitGroups.FirstOrDefault(group => group.Key == cardSuit);

            int sameSuitCount = sameSuitGroup != null ? sameSuitGroup.Count() : 0;

            Logger.Print($"Count of same suit cards: {sameSuitCount}");

            bool isSymbolcard = false;

            if (getCardValue(cardS) == 19 && sameSuitCount > 1)
            {
                isSymbolcard = true;
                symbolStaticCard.sprite = GetSprite(cardS);
            }

            MyPlayerCardThrowAnimation(card, isSymbolcard ? symbolStaticCard.transform : BottomThrowCard.transform, getCardColor(cardS).Equals("k"), isSymbolcard);

            Logger.Print("Bottom PLayer Latest Card >> " + BottomPlayerCard.Count);
            Sprite spr = IsFlipModeSprite(BottomPlayerCard[cIndex], isFlipStatus);

            if (KeepPlayPanel.activeInHierarchy)
            {
                KeepPlayPanel.gameObject.SetActive(false);
                CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
            }
            if (getCardColor(cardS).Equals("k"))
            {
                EventHandler.SelectCardReq(BottomPlayerCard[cIndex]);

                BottomThrowCard.GetComponent<Image>().sprite = spr;

                for (int i = 0; i < allColorButtons.Length; i++) allColorButtons[i].interactable = true;
                ChooseColorText.SetActive(false);
            }
            else if (rules.Contains(6) && getCardValue(cardS) == 7)
            {
                swapCard = cardS;
                ShowSwapButtons(true);
            }
            else
            {
                if (isSayUnoResGet)
                    StartCoroutine(DelayThrowCard(isSayUnoResGet));
                else
                    StartCoroutine(DelayThrowCard(true));
            }
        }
    }

    private IEnumerator DelayThrowCard(bool getRes)
    {
        yield return new WaitUntil(() => getRes);

        string cardS = IsFlipModeCardStr(BottomPlayerCard[cIndex]);

        getRes = false;
        EventHandler.ThrowCard(BottomPlayerCard[cIndex], getCardColor(cardS), isUno);
    }

    public string IsFlipModeCardStr(string plCard)
    {
        string cardS = (mode.Equals(AppData.FLIP)) ? AppData.GetFilpName(plCard, isFlipStatus) : plCard;
        Logger.NormalLog($"IsFlipModeCardStr : {cardS}");

        return cardS;
    }

    public Sprite IsFlipModeSprite(string plCard, bool isFlipStatus)
    {
        Sprite spr = mode.Equals(AppData.FLIP) ?
              GetFlipSide(plCard, isFlipStatus) :
              GetSprite(plCard);
        return spr;
    }

    public void SelectColor(int i)
    {
        Logger.Print($"SSelectColor :  {i} | isTutorialPlay : {AppData.isTutorialPlay}");
        if (AppData.isTutorialPlay)
        {
            Logger.Print($"stepCount :  {TutorialManager.stepCount} ");
            SelectColorAnimation(i);
            TutorialManager.Instance.playingTutorialScreen.SetActive(false);
            if (i == 0 && TutorialManager.stepCount <= 28)
                Invoke(nameof(TutorialLateCall), 1);
            else
            {
                BottomThrowCard.GetComponent<Image>().sprite = wildAllColor[i];
                TutorialManager.Instance.HandleTutorial(31);
            }
            return;
        }

        if (isDontTouch) return;
        if (cIndex == -1) return;


        for (int k = 0; k < SelectColorImg.Length; k++)
        {
            if (k == i)
            {
                //  o,t,j,p,k
                string color;

                if ((mode.Equals(AppData.FLIP)))
                {
                    color = isFlipStatus ?
                        (i == 0 ? "o" : (i == 1 ? "t" : (i == 2 ? "j" : "p"))) :
                        (i == 0 ? "r" : (i == 1 ? "g" : (i == 2 ? "b" : "y")));
                }
                else
                    color = (i == 0 ? "r" : (i == 1 ? "g" : (i == 2 ? "b" : "y")));

                if (isSpecific4Card)
                {
                    speceficColorStore = color;
                    isChooseSpecific = true;

                    StartCoroutine(ThrowCardEventSend(color));
                    SelectColorAnimation(i);
                    for (int j = 0; j < SelectColorImg.Length; j++)
                    {
                        if (j == i)
                            continue;

                        Image selectImga = SelectColorImg[j].GetComponent<Image>();
                        if (!selectImga.color.a.Equals(0))
                        {
                            selectImga.DOFade(0, 0.5f);
                        }
                    }

                    if (!isClickSpecificBtn)
                    {
                        // show tooltip
                        speceficHelpInfo.DOScale(1, 1);
                        return;
                    }
                }
                else
                    EventHandler.ThrowCard(BottomPlayerCard[cIndex], color, isUno);

                continue;
            }
            Image selectImg = SelectColorImg[k].GetComponent<Image>();

            if (!selectImg.color.a.Equals(0))
            {
                selectImg.DOFade(0, 0.5f);
            }
        }
    }
    [SerializeField] Transform speceficHelpInfo;
    string speceficColorStore;
    bool isClickSpecificBtn, isChooseSpecific, isSpecific4Card = false;
    int targetSpecificSi = -1;

    private IEnumerator ThrowCardEventSend(string color) // Work only k-6-1 Card
    {
        Logger.NormalLog($"ThrowCardEventSend || targetSpecificSi :: {targetSpecificSi} || isClickSpecificBtn: {isClickSpecificBtn} = isChooseSpecific: {isChooseSpecific}");

        yield return new WaitUntil(() => (isClickSpecificBtn && isChooseSpecific));
        EventHandler.ThrowCard(BottomPlayerCard[cIndex], color, isUno, targetSpecificSi);
    }

    private void SelectColorAnimation(int i, bool isShield = false)
    {
        Logger.Print($"{TAG} | SelectColorAnimation i = {i}");
        for (int j = 0; j < allColorButtons.Length; j++)
            allColorButtons[j].interactable = false;

        if (!isShield) AudioManager.instance.AudioPlay(AudioManager.instance.colorChange);

        if (BackMenuPannel.activeInHierarchy)
            OnClick_BackMenu(0, false);

        if (TableInfoPannel.activeInHierarchy)
            OnClick_BackMenu(6, false);

        BacktoLobby = false;
        int p = i;//4

        Logger.Print($"{TAG} | SelectColorAnimation isSpecific4Card = {isSpecific4Card}");
        if (!isSpecific4Card)
            for (int k = 0; k < SelectColorImg.Length; k++)
            {
                if (k == i)
                    continue;

                Image selectImg = SelectColorImg[k].GetComponent<Image>();
                if (!selectImg.color.a.Equals(0))
                {
                    selectImg.DOFade(0, 0.5f);
                }
            }

        colorBlackBg.DOFade(0, 0.3f);
        glowFadeAnim.gameObject.SetActive(true);
        glowFadeAnim.transform.localScale = new Vector2(0.2f, 0.2f);
        var storePos = SelectColorImg[i].GetComponent<RectTransform>().anchoredPosition;
        SelectColorImg[i].transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(() =>
        {
            //TODO: glow ANimation
            glowFadeAnim.transform.DOScale(Vector2.one, 0.3f);
            glowFadeAnim.DOFade(0.5f, 1f).SetEase(Ease.InOutQuad);

            Logger.Print($"color is ? {i} | isFlipStatus : {isFlipStatus}");
            Color32 color;
            if (mode.Equals(AppData.FLIP))
            {
                color = (!isFlipStatus) ?
                         (i == 0 ? Color.red :
                          i == 1 ? Color.green :
                          i == 2 ? new Color(0, 200, 255, 255) :
                          Color.yellow) :

                         (i == 0 ? flipColors[0] : // Orange
                          i == 1 ? flipColors[1] : // Teal
                          i == 2 ? flipColors[2] : // Jambli (Purple)
                          flipColors[3]); // Purple
            }
            else
                color = (i == 0 ? Color.red :
                         i == 1 ? Color.green :
                           i == 2 ? new Color(0, 200, 255, 255) :
                          Color.yellow);

            StartCoroutine(ChangeColor(0, glowFadeAnim, color));

            SelectColorImg[i].transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.zero, false, false);

                SelectColorImg[i].GetComponent<RectTransform>().anchoredPosition = storePos;
                SelectColorImg[i].transform.DOScale(Vector3.one, 0).SetDelay(1).OnComplete(() =>
                {
                    for (int k = 0; k < SelectColorImg.Length; k++)
                    {
                        Image selectImg = SelectColorImg[k].GetComponent<Image>();
                        if (!selectImg.color.a.Equals(1))
                            selectImg.DOFade(1, 0);
                    }
                });
                BacktoLobby = true;
            });
        });
        //cs == "r" ? 0 : cs == "g" ? 1 : cs == "b" ? 2 : 3
        if (AppData.isTutorialPlay)
            CenterGlowCard.SetRingColor(i == 0 ? "r" : i == 1 ? "r" : i == 2 ? "b" : "y");

        //PerformColorChange(i, p);
    }

    float glowDuration = 1f;

    IEnumerator ChangeColor(float timeElapsed, Image image, Color32 targetColor)
    {
        targetColor.a = (byte)125f / (byte)255f;

        while (timeElapsed < glowDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / glowDuration; // Calculate the interpolation factor
            image.color = Color.Lerp(Color.white, targetColor, t); // Interpolate color
            yield return null; // Wait for the next frame
        }

        image.color = targetColor; // Ensure the final color is set
        glowFadeAnim.gameObject.SetActive(false);
    }

    void TutorialLateCall()
    {
        TutorialManager.Instance.HandleTutorial(27);
    }

    public IEnumerator WaitTutorialAnimationOff(int sec = 2)
    {
        yield return new WaitForSeconds(sec);
        LeftPlayerCard = (TutorialManager.Instance.leftPlayerCard);
        RedrawCard(LeftSeatIndex);

        TopPlayerCard = (TutorialManager.Instance.topPlayerCard);
        RedrawCard(TopSeatIndex);

        RightPlayerCard = (TutorialManager.Instance.rightPlayerCard);
        RedrawCard(RightSeatIndex);
    }

    public void TutorialSeatIndexSet()
    {
        BottomSeatIndex = 0;
        LeftSeatIndex = 1;
        TopSeatIndex = 2;
        RightSeatIndex = 3;
        bottomPlayerImgButton.interactable = leftPlayerImgButton.interactable =
            rightPlayerImgButton.interactable = topPlayerImgButton.interactable = false;
        flipCloseDeckImg.gameObject.SetActive(false);
        closeDeckCounter = 104;
        CloseDeckCounter.text = closeDeckCounter.ToString();
    }

    public void TutorialTopPlayerLastCardForget()
    {
        TutorialManager.Instance.TopUserForgetLastCard();
        ShowCatchUNO(false, 2);
    }

    public void Report() => EventHandler.Report(TableId);

    float tim = 0.3f;
    int closeDeckCounter = 132;

    private void HandleRGTI(JSONNode data)
    {
        tim = 0;
        Logger.Print(TAG + " HandleRGTI called");
        AppData.IsRejoin = true;
        HandleGTI(data);
    }

    string isSelectCardStr = "";
    private void HandleGTI(JSONNode data)
    {
        Logger.Print($"GTI ::  isChooseSpecific: {isChooseSpecific} | isClickSpecificBtn: {isClickSpecificBtn} ");
        bool tempSpeceficColor = isChooseSpecific;
        bool tempSpeceficClick = isClickSpecificBtn;
        string tempC = speceficColorStore;
        ResetTable(true);
        Logger.Print("My UID : " + PrefrenceManager._ID);
        Logger.Print("Old Table Joined >> " + AppData.GTIDATA.ToString() + " tim: " + tim);

        if (!AppData.handleLeaveTable) AppData.handleLeaveTable = true;
        gtiRespoce.Clear();
        rules.Clear();
        PrivateTable.instance.rulasAddList.Clear();

        // Active Status Panels =======================================
        allPlayingScreen.gameObject.SetActive(true);
        CommanAnimations.instance.FullScreenPanelAnimation(playingScreen, true, 0);
        PrivateTable.instance.PrivetTablePanleActiveHandle(false, 0);
        PrivateTable.instance.CustomTablePanleActiveHandle(false, 0);
        CommanAnimations.instance.FullScreenPanelAnimation(CreateTable.instance.createTablePanel.gameObject, false, 0); //off live tavle
        CommanAnimations.instance.FullScreenPanelAnimation(BuddiesScreen.instance.BuddiesPannel, false, 0);
        CommanAnimations.instance.FullScreenPanelAnimation(LiveTablePanel.Instance.liveTablePanel.gameObject, false, 0);
        CommanAnimations.instance.FullScreenPanelAnimation(DailyMission.instance.dailyMissionPanel.gameObject, false, 0);
        CommanAnimations.instance.FullScreenPanelAnimation(Notification.Instance.NotiPannel.gameObject, false, 0);

        if (!bottomPlayerImgButton.interactable)
            bottomPlayerImgButton.interactable = leftPlayerImgButton.interactable =
                rightPlayerImgButton.interactable = topPlayerImgButton.interactable = true;

        if (!TutorialManager.Instance.topGoldGemsObj.activeInHierarchy)
        {
            leaveButton.gameObject.SetActive(true);
            leaveButton.gameObject.SetActive(true);
            TutorialManager.Instance.topGoldGemsObj.SetActive(true);
        }
        if (!TutorialManager.Instance.topRightGrid.activeInHierarchy || TutorialManager.Instance.topRightGrid.transform.localScale == Vector3.zero)
        {
            TutorialManager.Instance.topRightGrid.transform.localScale = Vector3.one;
            TutorialManager.Instance.topRightGrid.SetActive(true);
        }

        Loading_screen.instance.ShowLoadingScreen(false);
        Loading_screen.instance.trapImage.SetActive(false);

        if (TournamentPanel.instance.tournamentPanel.gameObject.activeInHierarchy) TournamentPanel.instance.tournamentPanel.gameObject.SetActive(false);
        TournamentPanel.instance.t_Timer_Panel.SetActive(false);
        TournamentPanel.instance.t_MappingPanel.gameObject.SetActive(false);
        Logger.Print(TAG + " Handle GTI called " + data.ToString());
        AllCommonGameDialog.instance.ChallengeDialog.SetActive(false);
        OnClick_BackMenu(0, false);

        leftPlayerImgButton.onClick.RemoveAllListeners();
        LeftPlayerImg.texture = inviteUser;
        leftPlayerImgButton.onClick.AddListener(() =>
        {
            OnClick_InvitePlayers(1);
        });

        topPlayerImgButton.onClick.RemoveAllListeners();
        TopPlayerImg.texture = inviteUser;
        topPlayerImgButton.onClick.AddListener(() =>
        {
            OnClick_InvitePlayers(2);
        });

        rightPlayerImgButton.onClick.RemoveAllListeners();
        RightPlayerImg.texture = inviteUser;
        Logger.Print($"{TAG} || Right :  {inviteUser.name}");
        rightPlayerImgButton.onClick.AddListener(() =>
        {
            OnClick_InvitePlayers(3);
        });

        mode = data["data"]["mode"];
        bv = data["data"]["bv"].AsLong;
        tpv = data["data"]["tpv"].AsLong;
        gems = data["data"]["gems"].AsInt;
        isTournament = data["data"]["tou"].AsBool;
        ip = data["data"]["_ip"];
        tbid = data["data"]["_id"];
        rules = JsonConvert.DeserializeObject<List<int>>(data["data"]["rules"].ToString());

        ruleShowBtn.gameObject.SetActive(rules.Count > 0);
        ruleshowToolTip.gameObject.SetActive(rules.Count > 0);
        if (rules.Count > 0)
        {
            SetRuleIconOnPlaying(rules);
        }

        if (!AppData.IsRejoin)
        {
            FirebaseData.EventSendWithFirebase("Gameplay");
            FirebaseData.EventSendWithFirebase(mode);
        }

        if (ip == 1 && data["data"]["ap"] == 1) Invoke("OnClick_InvitePlayerList", 0.5f);

        GameStartTimerTxt.text = (ip == 1) ? "Kindly extend invitations to both your friends and online players." : "Please Wait For Players...";
        GameStartTimer.SetActive(true);

        //For Tournament
        betValTransform.sizeDelta = new Vector2(isTournament ? 400f : 300f, betValTransform.rect.height);
        betValImg.sprite = isTournament ? betInTour : normalBet;
        roundTextObject.SetActive(isTournament);
        roundText.text = data["data"]["status"];
        BetValue.text = AppData.numDifferentiation(bv);

        if (AppData.IsRejoin) PotValue.text = AppData.numDifferentiation(data["data"]["pv"].AsLong);
        if (isTournament) PotValue.text = AppData.numDifferentiation(tpv);

        BacktoLobby = !isTournament;
        modeNameImg.sprite = (mode == AppData.CLASSIC) ? modeNames[0] : (mode.Equals(AppData.PARTNER)) ? modeNames[1] : modeNames[2];
        if (isTournament) modeNameImg.sprite = modeNames[3];
        if (ip == 1) modeNameImg.sprite = modeNames[4];
        modeTypeImg.sprite = (mode == AppData.CLASSIC || mode == AppData.EMOJISOLO) ? modeTypes[0] : (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)) ? modeTypes[1] : modeTypes[0];

        modeNameImg.sprite = (mode == AppData.FLIP) ? modeNames[5] : modeNameImg.sprite; // Flip center

        closeDeckCounter = data["data"]["deck"].Count == 0 ? 132 : data["data"]["deck"].Count;

        CloseDeckCounter.gameObject.SetActive(data["data"]["deck"].Count != 0);
        CloseDeckCounter.transform.parent.gameObject.SetActive(data["data"]["deck"].Count != 0);
        CloseDeckAnim();

        Logger.Print(TAG + " Mode of The Game " + mode);

        TableInfoValue[0].text = mode;
        TableInfoValue[1].text = AppData.numDifferentiation(bv);
        TableInfoValue[2].text = Player + "";
        TableInfoValue[3].text = isTournament ? "YES" : "NO";

        if (GameWinner.instance != null && GameWinner.instance.WinnerPannel.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel.GetComponent<RectTransform>(), false);

        isFlipStatus = data["data"]["isFlip"].AsBool;
        reversePar.gameObject.SetActive(true);

        flipCloseDeckImg.gameObject.SetActive(mode.Equals(AppData.FLIP));
        if (mode.Equals(AppData.FLIP))
            playinBG.sprite = playingBgSprites[isFlipStatus ? 1 : 0];


        //OpenCard = data["data"]["ltc"].Value;
        TableId = data["data"]["_id"];
        string ltc = IsFlipModeCardStr(data["data"]["ltc"].Value);
        OpenCard = ltc;

        isDontTouch = true;
        bool isSelectCardHas = false;
        isSelectCardStr = "";
        selectedCard = JsonConvert.DeserializeObject<List<string>>(data["data"]["SelectedCard"].ToString());

        for (int i = 0; i < data["data"]["pi"].Count; i++)
        {
            if (data["data"]["pi"][i].Count == 0 || !data["data"]["pi"][i].HasKey("ui"))
            {
                Logger.Print(TAG + " Data Not Found Of >> " + i);
                continue;
            }

            string name = data["data"]["pi"][i]["ui"]["pn"];
            string Uid = data["data"]["pi"][i]["ui"]["uid"];
            int Si = data["data"]["pi"][i]["ui"]["si"].AsInt;
            string PP = data["data"]["pi"][i]["ui"]["pp"];
            List<string> PlayerCard = JsonConvert.DeserializeObject<List<string>>(data["data"]["pi"][i]["cards"].ToString());
            string Gift = data["data"]["pi"][i]["giftImg"];
            bool IsCom = data["data"]["pi"][i]["comp"] == 1;
            string frameImg = data["data"]["pi"][i]["ui"]["frameImg"];
            string cardImg = data["data"]["pi"][i]["ui"]["deckImg"]["backcard"];
            string deckImg = data["data"]["pi"][i]["ui"]["deckImg"]["bunch"];

            string bunch1 = data["data"]["pi"][i]["ui"]["deckImg"]["bunch1"];
            string bunch2 = data["data"]["pi"][i]["ui"]["deckImg"]["bunch2"];

            Logger.Print(TAG + "Is My UID : " + (Uid == PrefrenceManager._ID));

            if (Uid == PrefrenceManager._ID)
            {
                BottomSeatIndex = Si;
            }

            if (data["data"]["pi"][i]["selectCard"] != "")
            {

                for (int j = 0; j < data["data"]["pi"][i]["cards"].Count; j++)
                {
                    Logger.Print($"Rejoin::::: Show ???? j {data["data"]["pi"][i]["cards"][j]} || s = {data["data"]["pi"][i]["selectCard"]}");
                    if (data["data"]["pi"][i]["cards"][j] == data["data"]["pi"][i]["selectCard"])
                    {
                        isSelectCardStr = data["data"]["pi"][i]["selectCard"];
                        cIndex = j;
                        if (BottomSeatIndex == Si)
                        {
                            isSelectCardHas = true;
                        }
                    }
                }
                Logger.Print($"Rejoin::::: Show ????|| cardIndex = {cIndex}");

                isKeepPopupOn = true;
                ProfilePanel.instance.ProfilePanelClick(10);
                colorBlackBg.DOFade(1, 0);

                for (int p = 0; p < SelectColorImg.Length; p++)
                    SelectColorImg[p].sprite = isFlipStatus ? setFlipColor[p] : ResetColor[p];

                CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.one, true);

                if (data["data"]["ti"].AsInt == BottomSeatIndex)
                {
                    if (isSelectCardStr.Contains("k-6"))
                    {
                        isSpecific4Card = true;
                        Logger.RecevideLog($" ===========>>>> isClickSpecificBtn: {tempSpeceficClick} | isChooseSpecific: {tempSpeceficColor} | tempc: {tempC} <<=============");
                        isClickSpecificBtn = tempSpeceficClick;
                        isChooseSpecific = tempSpeceficColor;
                        speceficColorStore = tempC;

                        if (speceficColorStore != null)
                            StartCoroutine(ThrowCardEventSend(tempC));

                        if (!isClickSpecificBtn)
                            ShowPlus4SpeceficButtons(true);
                        if (isChooseSpecific)
                            CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.zero, false);
                    }

                }
            }

            GTIResponse SingleGti = new GTIResponse(name, Uid, Si, PP, PlayerCard, Gift, IsCom, frameImg, deckImg, cardImg, bunch1, bunch2);

            gtiRespoce.Add(SingleGti);

            if (i == 3) GameStartTimer.SetActive(false);
        }

        StartCoroutine(DataSetupCorutine(data, isSelectCardHas));
    }

    private IEnumerator DataSetupCorutine(JSONNode data, bool isSelectCardHas)
    {
        yield return new WaitForSeconds(0.5f);

        TableRotate(-1);

        Logger.Print(TAG + " Table Status " + data["data"]["t_status"].Value + " OpenCard " + OpenCard + " Size = " + gtiRespoce.Count);

        if (data["data"]["touId"] != null && isTournament)
        {
            leaveButton.interactable = false;
            Logger.NormalLog($"GST Leave Btn {leaveButton.interactable} || isTournament = {isTournament}");
        }
        if (data["data"]["t_status"].Value != "RoundStarted")
            addedPaneltyCards.Clear();

        switch (data["data"]["t_status"].Value)
        {
            case "GameStartTimer":
                Logger.Print(TAG + " GameStartTimer RoundStarted");
                BacktoLobby = data["data"]["round_timer"].AsInt >= 2;
                isTimerCountinue = true;
                StartCoroutine(GameStartTimerStart(data["data"]["round_timer"].AsInt));
                break;

            case "CollectingBootValue":
                Logger.Print(TAG + " CollectingBootValue RoundStarted");
                BacktoLobby = false;
                break;

            case "StartDealingCard":
                BacktoLobby = false;
                Logger.Print(TAG + " StartDealingCard RoundStarted");

                if (BottomPlayerCard.Count > 0)
                {
                    RedrawCard(BottomSeatIndex);
                }

                if (TopPlayerCard.Count > 0 && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)))
                {
                    RedrawCard(TopSeatIndex);
                }

                Sprite spr = IsFlipModeSprite(data["data"]["opendeck"][0], isFlipStatus);

                TopThrowCard.SetActive(true);
                TopThrowCard.GetComponent<Image>().sprite = spr;
                TopThrowCard.transform.SetSiblingIndex(4);

                CenterGlowCard.gameObject.SetActive(true);
                CenterGlowCard.reverse = data["data"]["isReverse"].AsBool;
                CenterGlowCard.enabled = true;
                CenterGlowCard.SetRingColor(getCardColor(data["data"]["cs"]));
                break;

            case "HIGHFIVE":
            case "RoundStarted":
                BacktoLobby = true;
                UnoBtn.SetActive(true);
                Logger.Print(TAG + $" Rejoin RoundStarted {BottomPlayerCard.Count} BottomSI  {BottomSeatIndex} OpenDeck{data["data"]["opendeck"].Count} isDontTouch {isDontTouch} | | isFlipStatus: {data["isFlip"].AsBool}");

                highFiveHands[0].gameObject.SetActive(data["data"]["t_status"].Value.Equals("HIGHFIVE"));

                if (BottomPlayerCard.Count > 0)
                {
                    Logger.Print(TAG + $" Redraw Sended From Here... >> Bottom Player Cards >> {BottomPlayerCard.Count} My Grid Cards >> {MyCardGrid.transform.childCount} | isFlipStatus {isFlipStatus}");
                    RedrawCard(BottomSeatIndex, isSelectCardHas);
                }
                StartCoroutine(WaitForLoadSorite(data));

                unoForgotterSI = data["data"]["sayUnoPenaltySI"].AsInt;

                Logger.NormalLog($"VV unoRemoveSI::: {data["data"]["ti"].AsInt} | b= {BottomSeatIndex} | l= {LeftSeatIndex} | t= {TopSeatIndex} | r= {RightSeatIndex} | {unoForgotterSI}");

                if (data["data"]["opendeck"].Count > 0)
                {
                    int cardIndex = 0;
                    for (int i = 0; i < data["data"]["opendeck"].Count; i++)
                    {
                        Sprite cspr = IsFlipModeSprite(data["data"]["opendeck"][i], isFlipStatus);

                        centerThrowCards[cardIndex].sprite = cspr;
                        centerThrowCards[cardIndex].gameObject.SetActive(true);
                        centerThrowCards[cardIndex].gameObject.transform.SetSiblingIndex(4);
                        cardIndex = (cardIndex + 1) % 4;
                    }

                    CenterGlowCard.gameObject.SetActive(true);
                    CenterGlowCard.enabled = true;
                    CenterGlowCard.reverse = data["data"]["isReverse"].AsBool;
                    CenterGlowCard.SetRingColor(getCardColor(data["data"]["cs"]));
                }

                if (data["data"]["wildupcounter"].AsInt > 0 || data["data"]["totalpenaltyCard"].AsInt > 0)
                {
                    bool isup = data["data"]["wildupcounter"].AsInt > 0 ? true : false;
                    StartCoroutine(WildPlusAnimation(isup ? data["data"]["wildupcounter"].AsInt : data["data"]["totalpenaltyCard"].AsInt, isup, data["data"]["ti"].AsInt));

                    if (data["data"]["wildupcounter"].AsInt > 0)
                    {
                        Logger.Print($"wildupcounter :::::::::: Enter");
                        deckParent.transform.GetChild(3).transform.DOMove(wildDummyShowObject.transform.position, 0f);
                        wildDummyShowObject.SetActive(true);
                    }
                }

                if (data["data"]["left_time"].AsInt >= 2)
                {
                    CardDeckController.instance.ResetMyCardPos();
                    if (data["data"]["sayUnoPenaltySI"].AsInt == BottomSeatIndex && data["data"]["sayUnoPenaltySI"].AsInt != -1)
                        isInUNO = data["data"]["pi"][BottomSeatIndex]["sayUno"].AsBool;

                    if (data["data"]["ti"].AsInt == BottomSeatIndex)
                    {
                        MyTurn = true;

                        StartUserTurnTimer(BottomPlayerTurnRing, data["data"]["left_time"].AsInt, true);
                        isInUNO = data["data"]["pi"][BottomSeatIndex]["sayUno"].AsBool;
                        leaveButton.interactable = true;
                        isChallenge = false;

                        bool totalpenaltyCard = data["data"]["isDrawPenltyButton"].AsInt > 0;
                        drawBtn.gameObject.SetActive(totalpenaltyCard);

                        Logger.NormalLog($"isPfcdClickAble  Rejoin : { data["data"]["isDrawPenltyButton"].AsInt == 0} | { data["data"]["totalpenaltyCard"].AsInt == 0} | { data["data"]["wildupcounter"].AsInt == 0}");
                        bool isPfcdClickAble = data["data"]["isDrawPenltyButton"].AsInt == 0 && data["data"]["totalpenaltyCard"].AsInt == 0 && data["data"]["wildupcounter"].AsInt == 0;
                        cardBunch.GetComponent<Button>().interactable = isPfcdClickAble;

                        if (data["data"]["pi"][BottomSeatIndex]["s"].Equals("pfcd"))
                        {
                            lastPickCard = data["data"]["pi"][BottomSeatIndex]["pfcdcard"];
                            KeepPlayChallenge(true);
                        }
                        else if (data["data"]["pi"][BottomSeatIndex]["s"].Equals("challenge"))
                        {
                            Challenge(data["data"]["pi"][BottomSeatIndex]["challengeJson"]);
                        }
                        else
                        {
                            if (!ChooseColorPanel.gameObject.activeInHierarchy)
                                UserCardClicable();
                        }
                        isDeckClickable = !data["data"]["pi"][BottomSeatIndex]["s"].Equals("pfcd");

                    }
                    else if (data["data"]["ti"].AsInt == LeftSeatIndex)
                    {
                        StartUserTurnTimer(LeftPlayerTurnRing, data["data"]["left_time"].AsInt);
                        isDontTouch = true;
                    }
                    else if (data["data"]["ti"].AsInt == TopSeatIndex)
                    {
                        StartUserTurnTimer(TopPlayerTurnRing, data["data"]["left_time"].AsInt);
                        isDontTouch = true;
                    }
                    else if (data["data"]["ti"].AsInt == RightSeatIndex)
                    {
                        StartUserTurnTimer(RightPlayerTurnRing, data["data"]["left_time"].AsInt);
                        isDontTouch = true;
                    }

                    if (unoForgotterSI != -1)
                    {
                        if (unoForgotterSI == LeftSeatIndex)
                            ShowCatchUNO(false, 1);
                        else if (unoForgotterSI == TopSeatIndex)
                            ShowCatchUNO(false, 2);
                        else if (unoForgotterSI == RightSeatIndex)
                            ShowCatchUNO(false, 3);
                    }

                    Logger.Print($"<color=white><b> VV sayUnoTag::: {data["data"]["pi"][BottomSeatIndex]["sayUnoTag"]} || " +
            $"b= {data["data"]["pi"][LeftSeatIndex]["sayUnoTag"]} ||" +
            $"l= {data["data"]["pi"][TopSeatIndex]["sayUnoTag"]} || " +
            $"t= {data["data"]["pi"][RightSeatIndex]["sayUnoTag"]}  isDontTouch ={isDontTouch}</b></color>");
                    if (data["data"]["pi"][BottomSeatIndex]["sayUnoTag"])
                    {
                        isUnoPressed = true;
                        SayUnoAnimation(BottomUnoTxt, false);
                    }
                    if (data["data"]["pi"][LeftSeatIndex]["sayUnoTag"])
                    {
                        SayUnoAnimation(LeftUnoTxt, false);
                    }
                    if (data["data"]["pi"][TopSeatIndex]["sayUnoTag"])
                    {
                        SayUnoAnimation(TopUnoTxt, false);
                    }
                    if (data["data"]["pi"][RightSeatIndex]["sayUnoTag"])
                    {
                        SayUnoAnimation(RightUnoTxt, false);
                    }
                }
                break;

            case "FWinnerDeclared":

                if (data["data"]["_ip"] != 1)
                {
                    DashBoardOn();

                    bool isWinner = false;

                    for (int i = 0; i < data["data"]["pi"].Count; i++)
                    {
                        if (data["data"]["pi"][i]["ui"]["uid"] == PrefrenceManager._ID && data["data"]["pi"][i]["lastgame"] == true)
                        {
                            isWinner = true;
                            break;
                        }
                    }
                    if (isWinner)
                    {
                        AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.congratulations, "You won your last game! Fantastic job! Keep up the great work and aim for even more victories in the next game!");
                    }
                    else
                    {
                        AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.dontGiveUp, "You didn’t win this time, but don't worry! Keep playing, and your next victory is just around the corner. Best of luck for your next game!");
                    }
                    EventHandler.LgsReqSend();
                }

                //Rejoin When Win
                break;

        }

        Loading_screen.instance.ShowLoadingScreen(false);

        AppData.IsRejoin = false;
    }

    private IEnumerator WaitForLoadSorite(JSONNode data)
    {
        yield return new WaitForSeconds(0.2f);

        if (TopPlayerCard.Count > 0 && (mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)))
        {
            RedrawCard(TopSeatIndex);
        }

        for (int i = 0; i < data["data"]["pi"].Count; i++)
        {
            if (data["data"]["pi"][i]["si"] == LeftSeatIndex)
            {
                LeftPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["data"]["pi"][i]["cards"].ToString());
                Logger.Print($" Rejoin LeftPlayerCard :: {LeftPlayerCard.Count}");

                RedrawCard(LeftSeatIndex);
            }
            else if (data["data"]["pi"][i]["si"] == TopSeatIndex && (!mode.Equals(AppData.PARTNER) || !mode.Equals(AppData.EMOJIPARTNER)))
            {
                TopPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["data"]["pi"][i]["cards"].ToString());
                Logger.Print($" Rejoin TopPlayerCard :: {TopPlayerCard.Count}");
                RedrawCard(TopSeatIndex);
            }
            else if (data["data"]["pi"][i]["si"] == RightSeatIndex)
            {
                RightPlayerCard = JsonConvert.DeserializeObject<List<string>>(data["data"]["pi"][i]["cards"].ToString());
                Logger.Print($" Rejoin RightPlayerCard:: {RightPlayerCard.Count}");

                RedrawCard(RightSeatIndex);
            }
        }
    }

    private void TableRotate(int si = -1)
    {
        Logger.Print(TAG + " TableRotate called " + gtiRespoce.Count + " BottomSI " + BottomSeatIndex);

        if (BottomSeatIndex == 0)
        {
            LeftSeatIndex = 1;
            TopSeatIndex = 2;
            RightSeatIndex = 3;
        }
        else if (BottomSeatIndex == 1)
        {
            LeftSeatIndex = 2;
            TopSeatIndex = 3;
            RightSeatIndex = 0;
        }
        else if (BottomSeatIndex == 2)
        {
            LeftSeatIndex = 3;
            TopSeatIndex = 0;
            RightSeatIndex = 1;
        }
        else if (BottomSeatIndex == 3)
        {
            LeftSeatIndex = 0;
            TopSeatIndex = 1;
            RightSeatIndex = 2;
        }

        Logger.Print(TAG + " Player Si " + BottomSeatIndex + " Left " + LeftSeatIndex + " Top " + TopSeatIndex + " Right " + RightSeatIndex);

        for (int i = 0; i < gtiRespoce.Count; i++)
        {
            int tempIndex = i;
            if (gtiRespoce[i].getSi() == si || si == -1)
            {
                if (gtiRespoce[i].getSi() == BottomSeatIndex)
                {
                    Logger.Print($"{TAG} >> {BottomSeatIndex} | {(AppData.BU_PROFILE_URL + gtiRespoce[i].getPp())} | {gtiRespoce[i].GetDeckImage()} | {gtiRespoce[i].Bunch2()} | {gtiRespoce[i].Bunch1()}");

                    Logger.Print("Deck & CloseDeckAnim Card URL Is : " + gtiRespoce[i].GetCardImage());
                    if (!gtiRespoce[i].getGiftImg().Equals(""))
                    {
                        StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getGiftImg(), BottomPlayerGift));
                        EmojiStickerAnimationPlay(BottomPlayerGift);
                    }
                    else GiftImgReset(BottomPlayerGift);

                    BottomDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
                    LeftDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
                    TopDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
                    RightDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;

                    BottomPlayerName.text = gtiRespoce[i].getPn();

                    if (!gtiRespoce[i].getPp().Equals("")) StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getPp(), BottomPlayerImg));
                    else BottomPlayerImg.texture = DefaultUser;

                    bottomPlayerGiftBtn.gameObject.SetActive(true);

                    BottomPlayerCardPile.SetActive(true);
                    if (!isAfterEG)
                    {
                        BottomPlayerCard = gtiRespoce[i].GetPlayerCard();
                        BottomPlayerCardCounter.text = gtiRespoce[i].GetPlayerCard().Count + "";
                    }

                    goldValue.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));
                    gemsValue.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));

                    bottomPlayerImgButton.onClick.RemoveAllListeners();
                }

                else if (gtiRespoce[i].getSi() == LeftSeatIndex)
                {
                    LeftPlayerName.text = gtiRespoce[i].getPn();

                    if (!gtiRespoce[i].getPp().Equals("")) StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getPp(), LeftPlayerImg));
                    else LeftPlayerImg.texture = DefaultUser;

                    if (!gtiRespoce[i].getGiftImg().Equals(""))
                    {
                        Logger.Print(TAG + "Left Player Olf Gift >> " + gtiRespoce[i].getGiftImg());
                        StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getGiftImg(), LeftPlayerGift));
                        EmojiStickerAnimationPlay(LeftPlayerGift);
                    }
                    else GiftImgReset(LeftPlayerGift);

                    leftPlayerGiftBtn.gameObject.SetActive(!gtiRespoce[i].getIscom());

                    LeftPlayerCardPile.SetActive(true);
                    if (!isAfterEG)
                    {
                        LeftPlayerCard = gtiRespoce[i].GetPlayerCard();
                        LeftPlayerCardCounter.text = gtiRespoce[i].GetPlayerCard().Count + "";
                    }

                    leftPlayerImgButton.onClick.RemoveAllListeners();
                    if (!gtiRespoce[i].getIscom())
                    {
                        //leftPlayerImgButton.onClick.AddListener(() =>
                        //{
                        //    ProfileTouchControl(tempIndex);
                        //});
                    }
                }

                else if (gtiRespoce[i].getSi() == TopSeatIndex)
                {

                    TopPlayerName.text = gtiRespoce[i].getPn();

                    if (!gtiRespoce[i].getPp().Equals("")) StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getPp(), TopPlayerImg));
                    else TopPlayerImg.texture = DefaultUser;

                    if (!gtiRespoce[i].getGiftImg().Equals(""))
                    {
                        StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getGiftImg(), TopPlayerGift));
                        EmojiStickerAnimationPlay(TopPlayerGift);
                    }
                    else GiftImgReset(TopPlayerGift);

                    topPlayerGiftBtn.gameObject.SetActive(!gtiRespoce[i].getIscom());
                    TopPlayerCardPile.SetActive(true);

                    Logger.Print("Top Player Cards Setted...." + gtiRespoce[i].GetPlayerCard().Count);
                    if (!isAfterEG)
                    {
                        TopPlayerCard = gtiRespoce[i].GetPlayerCard();
                        TopPlayerCardCounter.text = gtiRespoce[i].GetPlayerCard().Count + "";
                    }

                    topPlayerImgButton.onClick.RemoveAllListeners();

                    if (!gtiRespoce[i].getIscom())
                    {
                        //    topPlayerImgButton.onClick.AddListener(() =>
                        //{
                        //    //  ProfileTouchControl(tempIndex);
                        //});
                    }
                }

                else if (gtiRespoce[i].getSi() == RightSeatIndex)
                {
                    RightPlayerName.text = gtiRespoce[i].getPn();

                    if (!gtiRespoce[i].getPp().Equals("")) StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getPp(), RightPlayerImg));
                    else RightPlayerImg.texture = DefaultUser;

                    rightPlayerGiftBtn.gameObject.SetActive(!gtiRespoce[i].getIscom());
                    if (!gtiRespoce[i].getGiftImg().Equals(""))
                    {
                        StartCoroutine(AppData.ProfilePicSet(gtiRespoce[i].getGiftImg(), RightPlayerGift));
                        EmojiStickerAnimationPlay(RightPlayerGift);
                    }
                    else
                    {
                        GiftImgReset(RightPlayerGift);
                    }

                    RightPlayerCardPile.SetActive(true);
                    if (!isAfterEG)
                    {
                        RightPlayerCard = gtiRespoce[i].GetPlayerCard();
                        RightPlayerCardCounter.text = gtiRespoce[i].GetPlayerCard().Count + "";
                    }

                    rightPlayerImgButton.onClick.RemoveAllListeners();
                    if (!gtiRespoce[i].getIscom())
                    {
                        //rightPlayerImgButton.onClick.AddListener(() =>
                        //{
                        //  //  ProfileTouchControl(tempIndex);
                        //});
                    }
                }
            }

            messageBtn?.gameObject.SetActive(i == gtiRespoce.Count - 1);
        }
    }

    public void ProfileClick(int l)
    {
        switch (l)
        {
            case 0://bottom
                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    if (BottomSeatIndex == gtiRespoce[i].getSi())
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.OpponentUserProfile(gtiRespoce[i].getUid());
                        break;
                    }
                }
                break;
            case 1://left
                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    if (LeftSeatIndex == gtiRespoce[i].getSi())
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.OpponentUserProfile(gtiRespoce[i].getUid());
                        break;
                    }
                }
                break;
            case 2://top
                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    if (TopSeatIndex == gtiRespoce[i].getSi())
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.OpponentUserProfile(gtiRespoce[i].getUid());
                        break;
                    }
                }
                break;
            case 3://right
                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    if (RightSeatIndex == gtiRespoce[i].getSi())
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.OpponentUserProfile(gtiRespoce[i].getUid());
                        break;
                    }
                }
                break;
        }
    }

    private void ProfileTouchControl(int tempIndex)
    {
        Logger.Print($"ChooseColorPanel ==?? {ChooseColorPanel.activeInHierarchy} || {KeepPlayPanel.activeInHierarchy}");
        if (!ChooseColorPanel.activeInHierarchy && !KeepPlayPanel.activeInHierarchy)
        {
            Loading_screen.instance.ShowLoadingScreen(true);
            EventHandler.OpponentUserProfile(gtiRespoce[tempIndex].getUid());
        }
    }

    IEnumerator ChangeFrame(Image playerFrame)
    {
        yield return new WaitForSeconds(0.2f);
        playerFrame.sprite = defaultFrame;
    }

    void GiftImgReset(RawImage img)
    {
        img.enabled = false;
    }

    string RecieverIdGift = "";

    public void GiftClick(int i)
    {
        int si = -1;

        switch (i)
        {
            case 0://bottom gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == BottomSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        RecieverIdGift = gtiRespoce[j].getUid();
                        break;
                    }
                }
                break;

            case 1://left gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == LeftSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        RecieverIdGift = gtiRespoce[j].getUid();
                        break;
                    }
                }
                break;

            case 2://top gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == TopSeatIndex)
                    {

                        si = gtiRespoce[j].getSi();
                        RecieverIdGift = gtiRespoce[j].getUid();
                        break;
                    }
                }
                break;

            case 3://right gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == RightSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        RecieverIdGift = gtiRespoce[j].getUid();
                        break;
                    }
                }
                break;
        }

        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        MessagePanel.Instance.OpenMessagePanel(2, si);
    }

    Tween timerAnim;
    Tween fillTweenAnim;
    Coroutine manageTimer;
    float remainingTime = 0;
    bool timecountdown = false;
    [SerializeField] Text timerTxt;

    IEnumerator ManageTurnTimer(Image endBorder, bool isMe, int count = 6)
    {
        int timer = 5;
        timerAnim = endBorder.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        if (isMe)
            AudioManager.instance.tikAudioSource.Play();
        for (int i = 0; i < count; i++)
        {

            yield return new WaitForSeconds(1);
            Logger.NormalLog($"VVV timer = {timer}");
            if (timer <= 2)
            {
                if (CardDeckController.instance.selectCard != null)
                    CardDeckController.instance.TrappedCardDestroy(CardDeckController.instance.selectCard);

                if (!isDontTouch)
                    isDontTouch = true;

                if (KeepPlayPanel.activeInHierarchy && !AppData.isTutorialPlay)
                    CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);

                if (timer == 1)
                    timerAnim?.Kill();
            }
            else
            {
                isDontTouch = false;
            }
            timer--;
        }
    }

    IEnumerator RepetCheck(float reStoreLeftTime)
    {
        if (AppData.isTutorialPlay) yield break;

        Logger.Print($" Timer repeat check ::::  {reStoreLeftTime}");
        if (reStoreLeftTime <= 3)
        {
            if (!isDontTouch)
                isDontTouch = true;
            if (isPlayingHint)
            {
                //playingHandTutorial.gameObject.SetActive(false);
                isPlayingHint = false;
            }
            if (playingHandTutorial.gameObject.activeInHierarchy) playingHandTutorial.gameObject.SetActive(false);
            if (playingClickHand.gameObject.activeInHierarchy) playingClickHand.gameObject.SetActive(false);

            if (KeepPlayPanel.activeInHierarchy) CommanAnimations.instance.PopUpAnimation(KeepPlayPanel, keepPlayBg, keepPlayPopUp, Vector3.zero, false, false);
        }
        yield return new WaitForSeconds(1);
        reStoreLeftTime--;
        if (reStoreLeftTime >= 0)
            StartCoroutine(RepetCheck(reStoreLeftTime));
    }

    private void StartUserTurnTimer(SelfTurnTimerControl TurnTimerRing, float leftTime, bool isMe = false)
    {
        bool DontTouch = false;
        isDontTouch = DontTouch;
        Logger.NormalLog($"isDontTouch : {isDontTouch} || manageTimer = {(manageTimer != null)}");

        timerAnim?.Kill();
        DOTween.Kill(TurnTimerRing.EndTurnFadeImg());

        Invoke(nameof(ImageAlphaClear), 0.1f);
        if (manageTimer != null)
        {
            StopCoroutine(manageTimer);
        }
        Logger.NormalLog($"StartUserTurnTimer ===========>>> leftTime = {leftTime}");

        float reStoreLeftTime = leftTime;

        leftTime = leftTime - 5;
        float totalTime = leftTime;

        BottomPlayerTurnRing.gameObject.SetActive(false);
        LeftPlayerTurnRing.gameObject.SetActive(false);
        TopPlayerTurnRing.gameObject.SetActive(false);
        RightPlayerTurnRing.gameObject.SetActive(false);

        //CancelInvoke(nameof(StartCountDown));
        remainingTime = leftTime;
        TurnTimerRing.TurnTimerImg().DOKill();

        if (reStoreLeftTime <= 5)
        {
            StartCoroutine(RepetCheck(reStoreLeftTime));

            timerAnim = TurnTimerRing.EndTurnFadeImg().DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
            TurnTimerRing.gameObject.SetActive(false);
        }
        else
        {
            TurnTimerRing.TurnTimerImg().fillAmount = leftTime / totalTime;
            Logger.Print("Remaining Fill Amount : >> " + TurnTimerRing.TurnTimerImg().fillAmount + " << Time : " + leftTime);
            TurnTimerRing.gameObject.SetActive(true);

            fillTweenAnim = TurnTimerRing.TurnTimerImg().DOFillAmount(0, leftTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                Logger.NormalLog($" IMG name : {TurnTimerRing.name}");
                if (reStoreLeftTime > 5)
                    manageTimer = StartCoroutine(ManageTurnTimer(TurnTimerRing.EndTurnFadeImg(), isMe));

                Logger.Print(TAG + " OnComplete Called");
                TurnTimerRing.gameObject.SetActive(false);
                TurnTimerRing.TurnTimerImg().fillAmount = 1;

            }).OnUpdate(() =>
            {
                float timerCurrentValue = TurnTimerRing.TurnTimerImg().fillAmount;
                if (timerCurrentValue >= .5f)
                {
                    TurnTimerRing.TurnTimerImg().color = new Color32(0, 255, 0, 120);
                }
                else if (timerCurrentValue < 0.5f && timerCurrentValue > 0.3f)
                {
                    TurnTimerRing.TurnTimerImg().color = new Color32(255, 255, 0, 120);
                }
                else
                {
                    TurnTimerRing.TurnTimerImg().color = new Color32(255, 0, 0, 120);
                }
                if (timerCurrentValue <= 0.09f && !isDontTouch) { }
                else
                {
                    if (isMe)
                    {
                        isDontTouch = false;
                    }
                }
            }).OnStart(() =>
            {
                Logger.Print(TAG + " OnStart Called");
            });
        }
    }

    Coroutine temp;

    IEnumerator Timer(float tt, Text txt)
    {
        txt.text = $"{tt}";
        tt -= 1;
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer(tt, txt));
    }

    private void ReverseCardAnimation(string c, float rotationZ)
    {
        reverseRotation.gameObject.SetActive(true);

        switch (c)
        {
            case "o":
                reverseRotation.ColorChangeOnReverse(flipColors[0], rotationZ); break;

            case "t":
                reverseRotation.ColorChangeOnReverse(flipColors[1], rotationZ); break;

            case "j":
                reverseRotation.ColorChangeOnReverse(flipColors[2], rotationZ); break;

            case "p":
                reverseRotation.ColorChangeOnReverse(flipColors[3], rotationZ); break;

            case "r":
                reverseRotation.ColorChangeOnReverse(Color.red, rotationZ); break;

            case "y":
                reverseRotation.ColorChangeOnReverse(Color.yellow, rotationZ); break;

            case "g":
                reverseRotation.ColorChangeOnReverse(Color.green, rotationZ); break;

            case "b":
                reverseRotation.ColorChangeOnReverse(Color.blue, rotationZ); break;

        }

        blockTxtImg.transform.parent.gameObject.SetActive(true);
        blockTxtImg.transform.parent.transform.localScale = Vector2.zero;

        blockTxtImg.sprite = reverseSpr;
        blockTxtImg.transform.parent.transform.DOScale(Vector2.one, 1).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            blockTxtImg.transform.parent.transform.DOScale(Vector2.zero, 1 / 2);
        });

        AudioManager.instance.AudioPlay(AudioManager.instance.reverseCard);
    }

    private void SkipTurnAnimation(GameObject ImgObj, Transform destinationPoint, bool isMegaBlock = false, bool skip2 = false)
    {
        ImgObj.SetActive(false);
        blockTxtImg.transform.parent.gameObject.SetActive(true);
        blockTxtImg.transform.parent.transform.localScale = Vector2.zero;

        ImgObj.transform.position = destinationPoint.position;

        AudioManager.instance.AudioPlay(AudioManager.instance.skipTurn);
        ImgObj.SetActive(true);
        ImgObj.transform.localScale = new Vector3(0.6f, 0.6f, 1);

        blockTxtImg.sprite = !isMegaBlock ? blockSpr : megaBlockSpr;
        blockTxtImg.sprite = skip2 ? skip2blockSpr : blockTxtImg.sprite;
        blockTxtImg.transform.parent.transform.DOScale(Vector2.one, 1).SetEase(Ease.OutBounce);
        ImgObj.transform.DOScale(Vector3.one * 2, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            ImgObj.transform.DOScale(Vector3.one * 1.6f, .5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                blockTxtImg.transform.parent.transform.DOScale(Vector2.zero, 1).SetEase(Ease.OutBounce).SetDelay(0.5f);
                ImgObj.transform.DOScale(Vector2.zero, 0.3f).SetDelay(2).OnComplete(() =>
                {
                    blockTxtImg.transform.parent.gameObject.SetActive(false);
                    ImgObj.SetActive(false);
                    ImgObj.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                });
            });
        });
    }

    [Header("=============== Anim Time =======================")]
    [SerializeField] float cardPENALTYAnimTime = 0.4f;
    private void AddedCardAnimation(int count, Vector3 startPoint, int si, bool isSpecific4Ltc = false)
    {
        if (isSpecific4Ltc)
        {
            CardDeckController.instance.playerData[si].specefic4Btn.transform.localScale = Vector3.zero;
            Sequence seq = DOTween.Sequence();
            seq.Append(CardDeckController.instance.playerData[si].specefic4Btn.transform.DOScale(Vector3.one, cardPENALTYAnimTime).SetEase(Ease.OutBounce));
            seq.Append(CardDeckController.instance.playerData[si].specefic4Btn.transform.DOScale(Vector3.zero, cardPENALTYAnimTime).SetDelay(0.15f).SetEase(Ease.OutBounce));
        }

        float delay = isSpecific4Ltc ? 1.3f : 0.1f;

        cardAddedImgs[si].transform.position = startPoint;
        cardAddTextParticles[si].transform.position = startPoint - new Vector3(0, 0.3f, 0);
        cardAddedImgs[si].transform.localScale = Vector3.zero;
        cardAddedImgs[si].text = $"+{count}";
        cardAddedImgs[si].DOFade(1, 0f);

        cardAddedImgs[si].gameObject.SetActive(true);
        cardAddedImgs[si].transform.DOScale(Vector3.one, cardPENALTYAnimTime).SetDelay(delay).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            cardAddedImgs[si].transform.DOMoveY(startPoint.y + 1.2f, cardPENALTYAnimTime).SetEase(Ease.InCubic).OnStart(() =>
            {
                cardAddTextParticles[si].SetActive(true);
                cardAddedImgs[si].DOFade(0, 0.2f).SetDelay(0.1f).OnComplete(() =>
                {
                    cardAddedImgs[si].gameObject.SetActive(false);
                    cardAddTextParticles[si].SetActive(false);
                });

            });
        });
    }

    float challengeAnimDuration = 2f;

    private void ChallengeAnimation(Transform destinationPoint, bool challengeResult)
    {
        Logger.Print(" Challenge Animation Start For >> " + destinationPoint.name);
        hummarObject.transform.position = destinationPoint.transform.position + new Vector3(0.5f, 0.4f, 0);
        hammer1.transform.position = destinationPoint.transform.position + new Vector3(0.5f, 0.4f, 0);
        hammer2.transform.position = destinationPoint.transform.position + new Vector3(0.5f, 0.4f, 0);
        challengeParticle.transform.position = destinationPoint.position;
        hummarObject.transform.rotation = Quaternion.Euler(0, 0, -20);
        hummarObject.transform.localScale = Vector3.zero;
        hammer1.transform.rotation = Quaternion.Euler(0, 0, 30);
        hammer2.transform.rotation = Quaternion.Euler(0, 0, 30);
        hummarObject.SetActive(true);

        hummarObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), challengeAnimDuration / 6);
        hummarObject.transform.DORotate(new Vector3(0, 0, -60), challengeAnimDuration / 4.5f).SetDelay(challengeAnimDuration / 4.5f).OnComplete(() =>
        {
            hummarObject.transform.DOScale(new Vector3(1.7f, 1.7f, 1f), challengeAnimDuration / 4.5f);
            hummarObject.transform.DORotate(new Vector3(0, 0, 50), challengeAnimDuration / 4.5f);

            if (challengeResult)
            {
                hummarObject.transform.DORotate(new Vector3(0, 0, -30f), challengeAnimDuration / 3f).SetDelay(challengeAnimDuration / 6 + challengeAnimDuration / 10).OnStart(() =>
                {
                    challengeParticle.SetActive(true);
                    AudioManager.instance.AudioPlay(AudioManager.instance.hummar);
                    hummarObject.transform.DOScale(Vector3.zero, challengeAnimDuration / 6).SetDelay(challengeAnimDuration / 4.5f).OnComplete(() =>
                    {
                        Logger.Print(" Challenge Animation Ended For >> " + destinationPoint.name);
                        hummarObject.SetActive(false);
                        hammer1.SetActive(false);
                        hammer2.SetActive(false);
                        challengeParticle.SetActive(false);
                    });
                });
            }
            else
            {
                hammer1.transform.DORotate(new Vector3(0, 0, 40), challengeAnimDuration / 3).SetDelay(challengeAnimDuration / 12 + challengeAnimDuration / 10).OnStart(() =>
                {
                    challengeParticle.SetActive(true);
                    hammer1.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
                    hammer2.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
                    hammer1.SetActive(true);
                    hammer2.SetActive(true);
                    hummarObject.SetActive(false);
                    AudioManager.instance.AudioPlay(AudioManager.instance.hummarBreak);
                    hammer2.transform.DORotate(new Vector3(0, 0, -30), challengeAnimDuration / 4.5f).OnComplete(() =>
                    {
                        hammer1.transform.DOScale(Vector3.zero, challengeAnimDuration / 4);
                        hammer2.transform.DOScale(Vector3.zero, challengeAnimDuration / 4).OnComplete(() =>
                        {
                            Logger.Print(" Challenge Animation Ended For >> " + destinationPoint.name);
                            hummarObject.SetActive(false);
                            hammer1.SetActive(false);
                            hammer2.SetActive(false);
                            challengeParticle.SetActive(false);
                        });
                    });
                });
            }
        });
    }

    private void ChallengeTextAnimation(GameObject challengeObj, bool isChallengeWin)
    {
        challengeObj.GetComponent<Image>().sprite = isChallengeWin ? challengeWinSprite : challengeLoseSprite;
        challengeObj.SetActive(true);
        challengeObj.transform.DOScale(new Vector3(1, 1, 1), 0.4f).SetDelay(1.5f).OnComplete(() =>
        {
            AudioManager.instance.AudioPlay(isChallengeWin ? AudioManager.instance.chgTrue : AudioManager.instance.chgFalse);
            challengeObj.transform.DOShakeScale(0.4f, 0.1f, 1, 2, true /*ShakeRandomnessMode.Full*/).OnComplete(() =>
            {
                challengeObj.transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.5f).OnComplete(() =>
                {
                    challengeObj.transform.DOScale(new Vector3(1, 1, 1), 0.5f).OnComplete(() =>
                    {
                        challengeObj.transform.DOScale(Vector3.zero, 0.4f).OnComplete(() =>
                        {
                            isKeepPopupOn = false;

                            challengeObj.SetActive(false);
                        });
                    });
                });
            });
        });
    }

    Tween winAnim;

    public void WinnerAnimation(List<Winner> PlayerWinner)
    {
        fillTweenAnim?.Kill();
        Logger.Print(TAG + "Winner Animation Called... " + gameIsInBackground);
        OnClick_BackMenu(0, false);

        hintActionToolTip.gameObject.SetActive(false);
        Logger.Print($"isHintShow C :: false");
        ShowPlus4SpeceficButtons(false);
        if (ChooseColorPanel.activeInHierarchy)
            CommanAnimations.instance.PopUpAnimation(ChooseColorPanel, ChooseColorPanel.GetComponent<Image>(), ChooseColorPanel.transform, Vector3.zero, false, false);

        MessagePanel.Instance.OnClick_CloseMessagePanel(false);
        messageBtn.gameObject.SetActive(false);
        bool winStatus = false;
        for (int i = 0; i < PlayerWinner.Count; i++)
        {
            if (PlayerWinner[i].si == BottomSeatIndex)
            {
                if (PlayerWinner[i].w == 1)
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.winnerSound);
                    winStatus = true;
                }
                else
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.lossSound);
                    winStatus = false;
                }
            }
        }

        BacktoLobby = false;

        Logger.Print($"WinnerAnimation C :: gameIsInBackground : {gameIsInBackground}");
        if (gameIsInBackground)
        {
            if (isTournament)
            {
                for (int i = 0; i < PlayerWinner.Count; i++)
                {
                    if (PlayerWinner[i].w != 1 && PlayerWinner[i].uid.Equals(PrefrenceManager._ID)) GameWinner.instance.OpenWinnerScreen(false);
                }
            }
            else GameWinner.instance.OpenWinnerScreen(false);
        }
        Logger.Print($"WinnerAnimation C :: gameIsInBackground ENTER 1: {gameIsInBackground}");

        for (int i = 0; i < HighlightObjects.Count; i++)
        {
            Canvas canvas = HighlightObjects[i].AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 5;
        }
        Logger.Print($"WinnerAnimation C :: gameIsInBackground ENTER 2: {gameIsInBackground}");

        UnoBtn.SetActive(false);

        if (winStatus)
        {
            winningTextObject.transform.position = Vector3.zero;
            winningTextObject.transform.localScale = Vector3.zero;
            winningTextObject.SetActive(true);
            winAnim = winningTextObject.transform.DOScale(new Vector3(5, 5, 1), 0.5f);
            Logger.Print($"WinnerAnimation C :: gameIsInBackground ENTER 3: {gameIsInBackground}");
            winAnim = winningTextObject.transform.DOLocalMoveY(125, 0.5f).SetDelay(0.5f).OnComplete(() =>
             {
                 for (int i = 0; i < PlayerWinner.Count; i++)
                 {
                     Logger.Print($"Points ======>> :: ^^  BottomSeatIndex {BottomSeatIndex}  == {PlayerWinner[i].si}");
                     Logger.Print($"Points ======>> :: ^^  BottomSeatIndex {LeftSeatIndex}  == {PlayerWinner[i].si}");
                     Logger.Print($"Points ======>> :: ^^  BottomSeatIndex {TopSeatIndex}  == {PlayerWinner[i].si}");
                     Logger.Print($"Points ======>> :: ^^  BottomSeatIndex {RightSeatIndex}  == {PlayerWinner[i].si}");
                     if (PlayerWinner[i].si == BottomSeatIndex)
                         WinnerTagAnimation(i, winningTags[i].transform, BottomPlayerImg.transform,
                             winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER) ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1], 0, PlayerWinner[i].points);

                     else if (PlayerWinner[i].si == LeftSeatIndex)
                         WinnerTagAnimation(i, winningTags[i].transform, LeftPlayerImg.transform,
                             winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER) ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1], 1, PlayerWinner[i].points);

                     else if (PlayerWinner[i].si == TopSeatIndex)
                         WinnerTagAnimation(i, winningTags[i].transform, TopPlayerImg.transform,
                             winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER) ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1], 2, PlayerWinner[i].points);

                     else if (PlayerWinner[i].si == RightSeatIndex)
                         WinnerTagAnimation(i, winningTags[i].transform, RightPlayerImg.transform,
                             winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER) ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1], 3, PlayerWinner[i].points);
                 }
                 winningTextObject.transform.DOLocalMoveY(0, 0.5f).SetDelay((4 * (winAnimDuration / 2))).OnComplete(() =>
                 {
                     Logger.Print($"Points ======>> :: ^^  isTournament {isTournament}");
                     winningTextObject.transform.DOScale(new Vector3(7.5f, 7.5f, 1), 0.5f).OnComplete(() =>
                     {
                         for (int i = 0; i < crackerParticles.Count; i++)
                         {
                             crackerParticles[i].SetActive(true);
                         }

                         winningTextObject.transform.DOScale(new Vector3(7.3f, 7.3f, 1), 1f).SetLoops(5, LoopType.Yoyo).OnUpdate(() =>
                         {
                             if (!AppData.isTutorialPlay)
                             {
                                 if (gameIsInBackground)
                                     return;
                             }

                         }).OnComplete(() =>
                         {
                             if (isTournament)
                             {
                                 for (int i = 0; i < PlayerWinner.Count; i++)
                                 {
                                     Logger.Print($"Points ======>> :: ^^  isTournament {PlayerWinner[i].w} == {PlayerWinner[i].uid.Equals(PrefrenceManager._ID)}");

                                     if (PlayerWinner[i].w != 1 && PlayerWinner[i].uid.Equals(PrefrenceManager._ID))
                                         GameWinner.instance.OpenWinnerScreen();
                                 }
                             }
                             else GameWinner.instance.OpenWinnerScreen(true, gameIsInBackground);
                         });
                     });
                 });
             });
            Logger.Print($"WinnerAnimation C ::  PlayerWinner.Count ENTER 5: { PlayerWinner.Count}");
        }
        else
        {
            Logger.Print($"WinnerAnimation C ::  PlayerWinner.Count ENTER 6: { PlayerWinner.Count}");
            for (int i = 0; i < PlayerWinner.Count; i++)
            {
                Logger.Print($"Points ======>> :si: {PlayerWinner[i].si} || {BottomSeatIndex}");
                if (PlayerWinner[i].si == BottomSeatIndex) WinnerTagAnimation(i, winningTags[i].transform,
                    BottomPlayerImg.transform,
                    winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)
                    ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1],
                    0,
                    PlayerWinner[i].points);

                else if (PlayerWinner[i].si == LeftSeatIndex) WinnerTagAnimation(i, winningTags[i].transform,
                    LeftPlayerImg.transform,
                    winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)
                    ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1],
                    1,
                    PlayerWinner[i].points);

                else if (PlayerWinner[i].si == TopSeatIndex) WinnerTagAnimation(i, winningTags[i].transform,
                    TopPlayerImg.transform,
                    winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)
                    ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1],
                    2,
                    PlayerWinner[i].points);

                else if (PlayerWinner[i].si == RightSeatIndex) WinnerTagAnimation(i, winningTags[i].transform,
                    RightPlayerImg.transform,
                    winningTagSprites[mode.Equals(AppData.PARTNER) || mode.Equals(AppData.EMOJIPARTNER)
                    ? (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1],
                    3,
                    PlayerWinner[i].points);
            }

            lossTextObject.SetActive(true);
            lossTextObject.transform.localScale = Vector3.zero;
            lossTextObject.transform.DOScale(new Vector3(7.5f, 7.5f, 1), 0.5f).OnComplete(() =>
            {
                lossTextObject.transform.DOScale(new Vector3(7.3f, 7.3f, 1), 1f).SetLoops(5, LoopType.Yoyo).OnComplete(() =>
                {
                    if (isTournament)
                    {
                        for (int i = 0; i < PlayerWinner.Count; i++)
                        {
                            if (PlayerWinner[i].w != 1 && PlayerWinner[i].uid.Equals(PrefrenceManager._ID)) GameWinner.instance.OpenWinnerScreen();
                        }
                    }
                    else GameWinner.instance.OpenWinnerScreen();
                });
            });
        }
    }

    private void WinnerTagAnimation(int i, Transform movingObject, Transform destinationPoint, Sprite tagSprite = null, int indexofPoint = 0, int points = 0)
    {
        Logger.Print($"Points ======>> :: {points} || indexofPoint {indexofPoint} || winPoints ::: {winPoints[indexofPoint].name}");

        winPoints[indexofPoint].transform.localScale = Vector3.zero;
        winPoints[indexofPoint].text = "  points :<br>" + points.ToString();
        winPoints[indexofPoint].gameObject.SetActive(true);
        winPoints[indexofPoint].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);

        if (winningTags[0].transform == movingObject)
        {
            kingObject.transform.position = destinationPoint.transform.position + new Vector3(-0.8f, 0.8f, 0);
            kingObject.transform.localScale = Vector3.zero;
            kingObject.transform.rotation = Quaternion.Euler(0, 0, -15);
            kingObject.SetActive(true);
            kingObject.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), winAnimDuration / 2);
        }
    }

    GameObject startGiftPos = null;
    [Header("Gift & Gif Prefab")]
    public RectTransform giftPrefab;
    public RectTransform gifPrefab;
    public Transform giftPrent;

    private void OnRecevied_SGTU(JSONNode data)
    {
        Logger.Print(">> GIF Checking >> " + TAG + data.ToString());

        Loading_screen.instance.ShowLoadingScreen(false);

        if (data["ssi"].AsInt == BottomSeatIndex)
        {
            startGiftPos = BottomPlayerGift.gameObject;
            Logger.Print("VVV P1");
        }
        else if (data["ssi"].AsInt == LeftSeatIndex)
        {
            startGiftPos = LeftPlayerGift.gameObject;
            Logger.Print("VVV P2");
        }
        else if (data["ssi"].AsInt == TopSeatIndex)
        {
            startGiftPos = TopPlayerGift.gameObject;
            Logger.Print("VVV P3");
        }
        else if (data["ssi"].AsInt == RightSeatIndex)
        {
            startGiftPos = RightPlayerGift.gameObject;
            Logger.Print("VVV P4");
        }

        #region For GITFT...Emoji 1
        if (data["imgType"] == "png")
        {
            Logger.Print(TAG + " >> GIF Checking Reciever SI " + data["rsi"].AsInt + " sender " + data["ssi"].AsInt);
            Logger.Print(TAG + " >> GIF Checking PlayerSI " + BottomSeatIndex + " left " + LeftSeatIndex + " Top " + TopSeatIndex + " Right " + RightSeatIndex);
            Logger.Print(TAG + " >> GIF Checking Gift Start Position " + startGiftPos.name);

            if (!data["rid"].Equals("&&"))
            {
                RectTransform giftObj = Instantiate(giftPrefab, startGiftPos.transform.localPosition, Quaternion.identity);
                giftObj.SetParent(startGiftPos.transform, false);
                string imageUrl = AppData.BU_PROFILE_URL + "/" + data["img"];
                StartCoroutine(AppData.ProfilePicSet(imageUrl, giftObj.GetComponent<RawImage>()));
                if (data["rsi"].AsInt == BottomSeatIndex)
                {
                    StartCoroutine(AppData.ProfilePicSet(imageUrl, BottomPlayerGift));
                    GiftAnimation(giftObj.gameObject, BottomPlayerGift);
                    Logger.Print("VVVR P1");
                }
                else if (data["rsi"].AsInt == LeftSeatIndex)
                {
                    StartCoroutine(AppData.ProfilePicSet(imageUrl, LeftPlayerGift));
                    GiftAnimation(giftObj.gameObject, LeftPlayerGift);
                    Logger.Print("VVVR P2");
                }
                else if (data["rsi"].AsInt == TopSeatIndex)
                {
                    StartCoroutine(AppData.ProfilePicSet(imageUrl, TopPlayerGift));
                    GiftAnimation(giftObj.gameObject, TopPlayerGift);
                    Logger.Print("VVVR P3");
                }
                else if (data["rsi"].AsInt == RightSeatIndex)
                {
                    StartCoroutine(AppData.ProfilePicSet(imageUrl, RightPlayerGift));
                    GiftAnimation(giftObj.gameObject, RightPlayerGift);
                    Logger.Print("VVVR P4");
                }
            }
            else
            {
                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    if (data["rid"].Equals("&&"))
                    {
                        RectTransform giftObj = Instantiate(giftPrefab, startGiftPos.transform.localPosition, Quaternion.identity);
                        giftObj.SetParent(startGiftPos.transform, false);
                        string imageUrl = AppData.BU_PROFILE_URL + "/" + data["img"];
                        StartCoroutine(AppData.ProfilePicSet(imageUrl, giftObj.GetComponent<RawImage>()));

                        if (gtiRespoce[i].si == BottomSeatIndex)
                        {
                            StartCoroutine(AppData.ProfilePicSet(imageUrl, BottomPlayerGift));
                            GiftAnimation(giftObj.gameObject, BottomPlayerGift);
                            Logger.Print("VVVR P1");
                        }
                        else if (gtiRespoce[i].si == LeftSeatIndex)
                        {
                            StartCoroutine(AppData.ProfilePicSet(imageUrl, LeftPlayerGift));
                            GiftAnimation(giftObj.gameObject, LeftPlayerGift);
                            Logger.Print("VVVR P2");
                        }
                        else if (gtiRespoce[i].si == TopSeatIndex)
                        {
                            StartCoroutine(AppData.ProfilePicSet(imageUrl, TopPlayerGift));
                            GiftAnimation(giftObj.gameObject, TopPlayerGift);
                            Logger.Print("VVVR P3");
                        }
                        else if (gtiRespoce[i].si == RightSeatIndex)
                        {
                            StartCoroutine(AppData.ProfilePicSet(imageUrl, RightPlayerGift));
                            GiftAnimation(giftObj.gameObject, RightPlayerGift);
                            Logger.Print("VVVR P4");
                        }
                    }
                }
            }
        }
        #endregion
        #region For GIF...Emoji 2
        else
        {
            if (!data["rid"].Equals("&&")) //Single player play
            {
                RectTransform gifObj = Instantiate(gifPrefab, startGiftPos.transform.localPosition, Quaternion.identity);
                gifObj.SetParent(startGiftPos.transform, false);
                Image cloneImg = gifObj.GetComponent<Image>();
                cloneImg.sprite = MessagePanel.Instance.GetEmojiSprite(data["emojinumber"].AsInt);

                if (data["rsi"].AsInt == BottomSeatIndex)
                {
                    CancelInvoke(nameof(BottomLateAnimFlase));

                    GifAnumation(cloneImg, bottomPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == BottomSeatIndex) ? 0 : 1);
                    Invoke(nameof(BottomLateAnimFlase), 6);
                }
                else if (data["rsi"].AsInt == LeftSeatIndex)
                {
                    CancelInvoke(nameof(LeftLateAnimFlase));

                    GifAnumation(cloneImg, leftPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == LeftSeatIndex) ? 0 : 1);
                    Invoke(nameof(LeftLateAnimFlase), 6);
                }
                else if (data["rsi"].AsInt == TopSeatIndex)
                {
                    CancelInvoke(nameof(TopLateAnimFlase));

                    GifAnumation(cloneImg, topPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == TopSeatIndex) ? 0 : 1);
                    Invoke(nameof(TopLateAnimFlase), 6);
                }
                else if (data["rsi"].AsInt == RightSeatIndex)
                {
                    CancelInvoke(nameof(RightLateAnimFlase));

                    GifAnumation(cloneImg, rightPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == RightSeatIndex) ? 0 : 1);
                    Invoke(nameof(RightLateAnimFlase), 6);
                }
            }
            else
            {
                CancelInvoke(nameof(BottomLateAnimFlase));
                CancelInvoke(nameof(LeftLateAnimFlase));
                CancelInvoke(nameof(TopLateAnimFlase));
                CancelInvoke(nameof(RightLateAnimFlase));

                for (int i = 0; i < gtiRespoce.Count; i++)
                {
                    RectTransform gifObj = Instantiate(gifPrefab, startGiftPos.transform.localPosition, Quaternion.identity);
                    gifObj.SetParent(startGiftPos.transform, false);
                    Image cloneImg = gifObj.GetComponent<Image>();
                    cloneImg.sprite = MessagePanel.Instance.GetEmojiSprite(data["emojinumber"].AsInt);

                    Tween gifTween = null;

                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        cloneImg.enabled = false;
                        gifTween = GifAnumation(cloneImg, bottomPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == BottomSeatIndex) ? 0 : 1);
                        Invoke(nameof(BottomLateAnimFlase), 6);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        gifTween = GifAnumation(cloneImg, leftPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == LeftSeatIndex) ? 0 : 1);
                        Invoke(nameof(LeftLateAnimFlase), 6);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        gifTween = GifAnumation(cloneImg, topPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == TopSeatIndex) ? 0 : 1);
                        Invoke(nameof(TopLateAnimFlase), 6);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        gifTween = GifAnumation(cloneImg, rightPlayerEmoji, data["emojinumber"].AsInt, (data["ssi"] == RightSeatIndex) ? 0 : 1);
                        Invoke(nameof(RightLateAnimFlase), 6);
                    }
                }
            }
        }
        #endregion

    }

    void GiftAnimation(GameObject giftObj, RawImage endPosObj)
    {
        Logger.Print($"<color=white><b> Viren :: startAnim Name : {giftObj.name} || End Anim name : {endPosObj.name}  active {endPosObj.gameObject.activeInHierarchy}</b></color>");
        endPosObj.enabled = false;
        giftObj.transform.DOJump(endPosObj.transform.position, 1, 1, 1f).OnComplete(() =>
        {
            EmojiStickerAnimationPlay(endPosObj);
            Destroy(giftObj.gameObject);
        });
    }

    Tween GifAnumation(Image img, Animator animator, int num, float animDuration)
    {
        Logger.Print($"<color=white><b> Viren :: startAnim Name : {img.name} || End Anim name : {animator.name}  active {animator.gameObject.activeInHierarchy}</b></color>");
        Tween moveTween = img.transform.DOMove(animator.transform.position, animDuration).OnComplete(() =>
        {
            AudioManager.instance.PlayGIFSound(AudioManager.instance.gIFSounds[num]);
            SGTU_GIFAnimation(animator, null, "", num);
            Destroy(img.gameObject);
        });

        return moveTween;
    }

    private void EmojiStickerAnimationPlay(RawImage emojiObject)
    {
        emojiObject.enabled = true;
        emojiObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        emojiObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        emojiObject.transform.DOLocalRotate(new Vector3(0, 0, -5), 0.5f).SetDelay(0.3f).OnComplete(() =>
        {
            emojiObject.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.5f).SetDelay(0.1f).SetLoops(-1, LoopType.Yoyo);
        });
    }

    Tween SGTU_GIFAnimation(Animator GIFObject, UniGifImage GIFScript, string url, int emojiNumber)
    {
        int index = emojiNumber;
        DOTween.Kill(GIFObject.gameObject);

        Tween scaleTween = GIFObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            GIFObject.gameObject.SetActive(false);
            GIFObject.transform.localScale = Vector3.zero;
            GIFObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            GIFObject.gameObject.SetActive(true);

            if (index >= 0 && index < emojiClips.Count)
            {
                AnimationClip clip = emojiClips[index];
                if (clip != null)
                {
                    Logger.Print($"EMOJI NUMBER :: {index} || {clip.name}");
                    GIFObject.Play(clip.name);
                }
            }

            // Start GIF animation
            GIFObject.transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.5f).SetEase(Ease.OutCubic);

        });

        // Return the initial scale tween
        return scaleTween;
    }

    void RightLateAnimFlase()
    {
        rightPlayerEmoji.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            rightPlayerEmoji.gameObject.SetActive(false);
        });
    }

    void BottomLateAnimFlase()
    {
        bottomPlayerEmoji.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            bottomPlayerEmoji.gameObject.SetActive(false);
        });
    }

    void LeftLateAnimFlase()
    {
        leftPlayerEmoji.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            leftPlayerEmoji.gameObject.SetActive(false);
        });
    }

    void TopLateAnimFlase()
    {
        topPlayerEmoji.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            topPlayerEmoji.gameObject.SetActive(false);
        });
    }

    private void EmojiAnimation(GameObject emojiObject, GameObject giftPos = null)
    {
        DOTween.Kill(emojiObject);
        if (giftPos != null)
        {
            Vector3 tempPos = emojiObject.transform.position;

            emojiObject.transform.position = giftPos.transform.position;

            emojiObject.SetActive(false);
            emojiObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            emojiObject.transform.localScale = Vector3.one;
            emojiObject.SetActive(true);

            emojiObject.transform.DOMove(tempPos, 1f).OnComplete(() =>
            {
                emojiObject.transform.DOLocalRotate(new Vector3(0, 0, -5), 0.5f).SetDelay(0.3f).OnComplete(() =>
                {
                    emojiObject.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.5f).SetDelay(0.1f).SetLoops(-1, LoopType.Yoyo);
                    giftPos = null;
                });
            });

        }
        else
        {
            emojiObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                emojiObject.SetActive(false);
                Logger.Print(TAG + "SGTU EMOJI Animation Called For >> " + emojiObject.activeSelf);
                emojiObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                emojiObject.transform.localScale = Vector3.zero;
                emojiObject.SetActive(true);

                emojiObject.transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.5f).SetEase(Ease.OutCubic);
                emojiObject.transform.DOLocalRotate(new Vector3(0, 0, -5), 0.5f).SetDelay(0.3f).OnComplete(() =>
                {
                    emojiObject.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.5f).SetDelay(0.1f).SetLoops(-1, LoopType.Yoyo);
                });
            });
        }
    }

    private void OnRecevied_COT(JSONNode data)
    {
        MessagePanel.Instance.CreateChat(data["pn"], data["body"]);

        Logger.NormalLog($"COT MsgToolTipTxtsSet : 1 Check = {data["si"].AsInt} == {BottomSeatIndex}");

        if (data["si"].AsInt == BottomSeatIndex)
        {
            MsgGet(data, 0);
            MessageToolTipAnimation(msgTooltips[0].transform);
        }
        else if (data["si"].AsInt == LeftSeatIndex)
        {
            MsgGet(data, 1);
            MessageToolTipAnimation(msgTooltips[1].transform);
        }
        else if (data["si"].AsInt == TopSeatIndex)
        {
            MsgGet(data, 2);
            MessageToolTipAnimation(msgTooltips[2].transform);
        }
        else if (data["si"].AsInt == RightSeatIndex)
        {
            MsgGet(data, 3);
            MessageToolTipAnimation(msgTooltips[3].transform);
        }
    }

    private void MsgGet(JSONNode data, int si)
    {
        msgToolTipTxts[si].text = data["body"];
        msgToolTipTxts[si].text = MessagePanel.Instance.InsertLineBreaks(msgToolTipTxts[si].text, 4);
        Logger.Print($"MsgToolTipTxtsSet : 2 {msgToolTipTxts[si].text}");
    }

    private void OnRecevied_GGL(JSONNode data)
    {
        Logger.Print(TAG + " >> My Card Grid Cards >> " + MyCardGrid.transform.childCount + " >> BottomPlayer Cards >>> " + BottomPlayerCard.Count);
        Loading_screen.instance.ShowLoadingScreen(false);
        MessagePanel.Instance.SetGiftData(data);
        MessagePanel.Instance.SetSuggetionData();
        MessagePanel.Instance.SetGIFData(data);
    }

    public void OnClick_SendGif(string img, string imgType, long price, int SI, int emojiNumber = 0)
    {
        string UID = "";
        for (int i = 0; i < gtiRespoce.Count; i++)
        {
            if (gtiRespoce[i].getSi() == SI)
            {
                UID = gtiRespoce[i].getUid();
                break;
            }
        }

        if (imgType.Equals("png")) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.SendGiftToUser(TableId,
            img,
            imgType,
            SI == -1 ? "All" : string.Empty,
            price,
            SI == -1 ? "&&" : UID,
            mode,
            false,
            SI,
            emojiNumber);
        Loading_screen.instance.ShowLoadingScreen(true);
        MessagePanel.Instance.OnClick_CloseMessagePanel(false);
    }

    private void OnRecevied_USERGOLD(JSONNode data)
    {
        PrefrenceManager.GOLD = data["gold"];
        goldValue.text = AppData.numDifferentiation(data["gold"].AsLong);
        CoinAndGemsUpdateAnimation(animationTxtObjCoin, animationTextCoin, data["goldAdded"].AsLong);
        WinLossScreen.instance.ShowWatchVideoPopup(false);// if active 
    }

    private void OnRecevied_USERGEMS(JSONNode data)
    {
        PrefrenceManager.GEMS = data["gems"];
        gemsValue.text = AppData.numDifferentiation(data["gems"].AsLong);
        Dashboard_MiniGame.instance.SSAHGems.text = AppData.numDifferentiation(data["gems"].AsLong);
        CoinAndGemsUpdateAnimation(animationTxtObjGems, animationTextGems, data["gemsAdded"].AsLong);
    }

    private void CoinAndGemsUpdateAnimation(GameObject animObject, TextMeshProUGUI textObject, long val)
    {
        if (val == 0) return;

        string text = AppData.numDifferentiation(val);
        Logger.Print("Boot Amount Cuted >> " + text);
        animObject.transform.localPosition = new Vector3(animObject.transform.position.x, 0, animObject.transform.position.z);
        textObject.text = val > 0 ? "<color=green>" + text + "</color>" : "<color=red>" + text + "</color>";
        textObject.DOFade(1, 0f);
        animObject.SetActive(true);
        animObject.transform.DOLocalMoveY(-75, 1f);
        textObject.DOFade(0, 0.3f).SetDelay(0.7f).OnComplete(() =>
        {
            animObject.SetActive(false);
        });
    }

    public void OnClick_Invite(string uid)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.InviteToPlaying(uid, TableId, bv, inviteClickedSI, gems, ip, mode);
    }

    int inviteClickedSI;
    public void OnClick_InvitePlayers(int SI = -1)
    {
        inviteClickedSI = SI;
        EventHandler.InvitePlayer(bv);
    }

    private void OnClick_InvitePlayerList()
    {
        if (!InviteToPlayScreen.instance.inviteToPlayPanel.activeInHierarchy)
            EventHandler.InvitePlayer(bv);
    }

    [SerializeField] GameObject deckGlow;
    [SerializeField] Image closeDeckImg;
    public RectTransform handIMg;
    //float deckPinValue = .4f;
    private void DeckGlowAnimation()
    {
        Logger.Print($"{TAG} | DeckGlowAnimation ");
        deckGlow.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        handIMg.transform.localScale = new Vector3(0.6f, 0.6f, .6f);
        deckGlow.SetActive(true);
        handIMg.gameObject.SetActive(true);
        deckGlow.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
        handIMg.DOScale(Vector3.one, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        handIMg.DORotate(new Vector3(0, 0, -20), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);

        if (mode.Equals(AppData.CLASSIC))
            if (PrefrenceManager.cardPicStatus == 0)
            {
                hintActionToolText.text = $"{TutorialActionSteps.cardPic}";
                hintActionToolTip.gameObject.SetActive(true);
                CancelInvoke(nameof(ShowHintToolTip));
            }
    }

    private void StopDeckGlow()
    {
        Logger.Print($"{TAG} | StopDeckGlow ");
        DOTween.Kill(deckGlow);
        DOTween.Kill(handIMg);
        handIMg.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        deckGlow.SetActive(false);
        handIMg.gameObject.SetActive(false);

        //hintActionToolTip.gameObject.SetActive(false);
    }

    private void HandleCloseAllPanels()
    {
        for (int i = 0; i < closablePanels.Count; i++) closablePanels[i].gameObject.SetActive(false);
    }

    int unoForgotterSI = -1;
    string UNOTAG = " >>>>>>>>> UNOTAG >> ::: ";
    bool isInShowingUNO = false;
    private void ShowCatchUNO(bool isWilly = false, int i = -1, bool ispen = false, bool isVelidCatch = true)
    {
        if (isVelidCatch)
        {
            catchUNOBtn[i].DOScale(1, 0.5f).OnStart(() =>
            {
                isInShowingUNO = true;
                Logger.Print("VV  Type :: ShowCatchUNO " + TAG + UNOTAG + "Show Uno Called + " + i + " Again  NAme Print :::: " + unoAnimator[i].name);
                unoAnimator[i].enabled = false;
                catchUNOImg[i].sprite = isWilly ? willySprite : catchUNOSprite;
                catchUNOBtn[i].transform.localScale = Vector3.zero;
                catchUNOBtn[i].gameObject.SetActive(true);
                //  catchUNOBtn[i].transform.position = destination.position + new Vector3(0.5f, -0.5f, 0);
            }).OnComplete(() =>
            {
                Logger.Print(TAG + UNOTAG + "Show Uno Completed Scale" + catchUNOBtn[i].localScale);
                catchUNOBtn[i].DOScale(1.3f, 1f).SetLoops(-1, LoopType.Yoyo).OnUpdate(() =>
                {
                    //Logger.Print(TAG + UNOTAG + "Show Uno In Update Scale" + catchUNOBtn.localScale);
                });
                isInShowingUNO = false;

                if (isWilly)
                {
                    catchUNOBtn[i].DOScale(0, 0.5f).SetDelay(2f).OnComplete(() =>
                    {
                        DOTween.Kill(catchUNOBtn);
                        catchUNOBtn[i].gameObject.SetActive(false);
                    });
                }
                if (ispen)
                {
                    unoAnimator[i].enabled = true;
                    unoAnimator[i].Play("UnoAnim");
                    a = catchUNOBtn[i].DOScale(0, 0.5f).SetDelay(1f).OnComplete(() =>
                    {
                        a.Kill();
                        unoAnimator[i].enabled = false;
                        unoAnimator[i].gameObject.SetActive(false);
                    });
                }

            });
        }
        else
        {
            catchUNOBtn[i].DOScale(0, 0.5f).SetDelay(2f).OnComplete(() =>
            {
                DOTween.Kill(catchUNOBtn);
                catchUNOBtn[i].gameObject.SetActive(false);
            });
        }

    }

    Tween a;
    public bool mySideSayUno;
    public bool isUnoPressed;

    public void PressUno()
    {
        Logger.Print($"<color=black><b> Uno Press>> MyTurn = {MyTurn} || isDontTouch = {isDontTouch} > IsIn UNO :: {isInUNO} mySideSayUno = {mySideSayUno} || Count ::  {BottomPlayerCard.Count} | isKeepedCard = {isKeepedCard} </b></color>");

        if (AppData.isTutorialPlay)
        {
            if (isUnoPressed && MyTurn)
            {
                ShowToolTip("You already called LAST CARD!");
            }
            else
            {
                if (!isUnoPressed)
                    TutorialManager.Instance.HandleTutorial(26);
                isUnoPressed = true;
                SayUnoAnimation(BottomUnoTxt);
            }
            return;
        }

        if (isDontTouch) return;
        if (isInUNO && !mySideSayUno && !isKeepedCard)
        {
            mySideSayUno = true;
            EventHandler.SendSayUNO(BottomSeatIndex);
        }
        else
        {
            if (BottomPlayerCard.Count == 2 && !isInUNO && MyTurn)
            {
                if (!isUnoPressed)
                {
                    ShowToolTip("You must Take a card from the deck. You can't hit LAST CARD right now!");
                }
            }
            else if (BottomPlayerCard.Count == 2 && isUnoPressed && MyTurn)
            {
                ShowToolTip("You already called LAST CARD!");
            }
            else
            {
                ShowToolTip("Click before you're down to just one card.");
                //ShowToolTip("you should only press the \"last card\" button before playing your second - to - last card");
            }
        }
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
    }

    void ShowToolTip(string message)
    {
        unoToolTipPopup.SetActive(true);
        unoToolTipPopupText.text = message;
        unoToolTipPopup.transform.DOScale(Vector2.one, .2f);
        Invoke(nameof(DisableToolTip), 2);
    }

    public GameObject unoToolTipPopup;
    public TextMeshProUGUI unoToolTipPopupText;

    private void DisableToolTip()
    {
        unoToolTipPopup.SetActive(false);
        unoToolTipPopup.transform.DOScale(Vector2.zero, .2f);
    }

    private void HandleSAYUNO(JSONNode data)
    {
        isSayUnoResGet = true;

        if (data["unoRemoveSI"] != -1)
        {
            for (int i = 0; i < gtiRespoce.Count; i++)
            {
                if (data["unoRemoveSI"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0);
                        isUnoPressed = true;
                        SayUnoAnimation(BottomUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1);
                        SayUnoAnimation(LeftUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2);
                        SayUnoAnimation(TopUnoTxt, false);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3);
                        SayUnoAnimation(RightUnoTxt, false);
                    }
                    break;
                }
            }
            for (int i = 0; i < gtiRespoce.Count; i++)
            {
                if (data["si"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0, false, true, false);
                        isUnoPressed = true;
                        SayUnoAnimation(BottomUnoTxt);
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1, false, true, false);
                        SayUnoAnimation(LeftUnoTxt);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2, false, true, false);
                        SayUnoAnimation(TopUnoTxt);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3, false, true, false);
                        SayUnoAnimation(RightUnoTxt);
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < gtiRespoce.Count; i++) /// Remove without BG last catd
            {
                if (data["si"].Equals(gtiRespoce[i].si))
                {
                    if (gtiRespoce[i].si == BottomSeatIndex)
                    {
                        HideCatchUno(0, false, false, false);
                        SayUnoAnimation(BottomUnoTxt);
                        isUnoPressed = true;
                    }
                    else if (gtiRespoce[i].si == LeftSeatIndex)
                    {
                        HideCatchUno(1, false, false, false);
                        SayUnoAnimation(LeftUnoTxt);
                    }
                    else if (gtiRespoce[i].si == TopSeatIndex)
                    {
                        HideCatchUno(2, false, false, false);
                        SayUnoAnimation(TopUnoTxt);
                    }
                    else if (gtiRespoce[i].si == RightSeatIndex)
                    {
                        HideCatchUno(3, false, false, false);
                        SayUnoAnimation(RightUnoTxt);
                    }
                    break;
                }
            }
        }
    }

    private void HideCatchUno(int si = -1, bool isFromPanelty = false, bool isFromSayUno = false, bool isVelidCatch = true)
    {
        unoForgotterSI = -1;
        unoAnimator[si].enabled = isFromPanelty;

        Logger.Print($"Hide Uno Called With Bool Of Panelty >> {isFromPanelty} | {catchUNOBtn[si].name}");
        catchUNOBtn[si].DOScale(0, 0.3f).OnComplete(() =>
        {
            Logger.Print($"Hide Uno Completed  = {isFromPanelty} || {si} | isInShowingUNO = {isInShowingUNO} || tcodFlage {tcodFlage}");

            if (!isInShowingUNO || tcodFlage)
            {
                tcodFlage = false;
                Logger.Print($" Viren anim complete = {isFromPanelty} || {si}");
                DOTween.Kill(catchUNOBtn);
                catchUNOBtn[si].gameObject.SetActive(false);

                if (AppData.isTutorialPlay) return;

                if (!isFromPanelty && si != -1)
                {
                    if (si == BottomSeatIndex)
                    {
                        Logger.Print($"Viren b = {BottomSeatIndex}");
                        if (isFromSayUno) SayUnoAnimation(BottomUnoTxt, false);
                        else ShowCatchUNO(true, si, false, isVelidCatch);
                    }

                    else if (si == LeftSeatIndex)
                    {
                        Logger.Print($"Viren l = {LeftSeatIndex}");
                        if (isFromSayUno) SayUnoAnimation(LeftUnoTxt, false);
                        else ShowCatchUNO(true, si, false, isVelidCatch);
                    }

                    else if (si == TopSeatIndex)
                    {
                        Logger.Print($"Viren t = {TopSeatIndex}");
                        if (isFromSayUno) SayUnoAnimation(TopUnoTxt, false);
                        else ShowCatchUNO(true, si, false, isVelidCatch);
                    }

                    else if (si == RightSeatIndex)
                    {
                        Logger.Print($"Viren r = {RightSeatIndex}");
                        if (isFromSayUno) SayUnoAnimation(RightUnoTxt, false);
                        else ShowCatchUNO(true, si, false, isVelidCatch);
                    }
                }
            }
        });
    }

    public void OnClickSpecificPlusFour(int i)
    {
        if (isClickSpecificBtn) return;

        int si = -1;

        switch (i)
        {
            case 0://bottom gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == BottomSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 1://left gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == LeftSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 2://top gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == TopSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 3://right gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == RightSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;
        }


        isClickSpecificBtn = true;
        targetSpecificSi = si;
        ShowPlus4SpeceficButtons(false);

        Logger.NormalLog($"OnClickSpecificPlusFour targetSpecificSi :: {targetSpecificSi} || isClickSpecificBtn::: {isClickSpecificBtn}");

        //EventHandler.SendPanelty(si, BottomSeatIndex);
    }

    string swapCard;
    public void OnSwapClick(int i)
    {
        int si = -1;

        switch (i)
        {
            case 0://bottom gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == BottomSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 1://left gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == LeftSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 2://top gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == TopSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 3://right gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == RightSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;
        }
        Logger.Print($" Swap With Si : {si}");
        ShowSwapButtons(false);
        EventHandler.ThrowCard(swapCard, getCardColor(swapCard), isUno, -1, si);
    }

    public void OnClick_CatchUno(int i)
    {
        int si = -1;

        switch (i)
        {
            case 0://bottom gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == BottomSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 1://left gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == LeftSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 2://top gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == TopSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;

            case 3://right gift click
                for (int j = 0; j < gtiRespoce.Count; j++)
                {
                    if (gtiRespoce[j].getSi() == RightSeatIndex)
                    {
                        si = gtiRespoce[j].getSi();
                        break;
                    }
                }
                break;
        }

        if (AppData.isTutorialPlay)
        {
            // Catch animation
            unoAnimator[2].enabled = true;
            unoAnimator[2].Play("UnoAnim");
            msgToolTipTxts[0].text = "CATCH";
            MessageToolTipAnimation(msgTooltips[0].transform);

            TutorialDrawCard(2, 2);
            Invoke(nameof(ControlHideUno), 1);
            if (!isFTUEclick)
            {
                isFTUEclick = true;
                TutorialManager.Instance.HandleTutorial(TutorialManager.stepCount);
                TutorialManager.Instance.playingTutorialScreen.SetActive(false);
            }
            return;
        }

        Logger.Print($"unoForgotterSI :: {si} || TopSeatIndex::: {TopSeatIndex} || si == {si}");
        if (si == -1 || (mode.Equals(AppData.PARTNER) && si == TopSeatIndex)) return;

        Logger.Print(TAG + " Catch Uno Clicked &  >> Forgoton Si is >> " + unoForgotterSI);
        EventHandler.SendPanelty(si, BottomSeatIndex);
    }


    bool isFTUEclick = false;

    void ControlHideUno()
    {
        Logger.Print($"Hide Uno Called With | {catchUNOBtn[2].name}");
        HideCatchUno(2);
    }

    private void MessageToolTipAnimation(Transform toolTipObject)
    {
        DOTween.Kill(toolTipObject);
        toolTipObject.DOScale(0, 0.2f).OnComplete(() =>
        {
            toolTipObject.gameObject.SetActive(false);
            toolTipObject.localScale = Vector3.zero;
            toolTipObject.gameObject.SetActive(true);
            toolTipObject.DOScale(1, 0.5f);
            toolTipObject.DOScale(0, 0.5f).SetDelay(5f).OnComplete(() =>
            {
                toolTipObject.gameObject.SetActive(false);
            });
        });
    }

    private void HandleUpdateLeaf(JSONNode data)
    {
        if (data["leafAdded"].AsInt > 0 && (data["tp"].Equals("Winner For Game") || data["tp"].Equals("Win From Tournament")))
        {
            leafAnimationParent.transform.position = BottomPlayerImg.transform.position;
            leafTxt.text = "+" + AppData.numDifferentiation(data["leafAdded"].AsLong);
            leafAnimationParent.SetActive(true);
            leafAnimationParent.transform.DOLocalMoveY(-170f, 1f).OnComplete(() =>
            {
                leafAnimationParent.SetActive(false);
            });
        }
    }

    [SerializeField] GameObject ruleShowBtn;
    [SerializeField] RectTransform ruleshowToolTip;
    [SerializeField] Image[] rulePlayingIcon;

    public void RuleShowOnPlaying(bool status)
    {
        //ruleshowToolTip.gameObject.SetActive()
        if (status)
        {
            ruleshowToolTip.DOAnchorPosX(0, duration + 0.5f);
        }
        else
        {
            ruleshowToolTip.DOAnchorPosX(-300f, duration + 0.5f);
        }
    }

    void SetRuleIconOnPlaying(List<int> ruleNum)
    {
        Logger.Print($"SetRuleIconOnPlaying {ruleNum.Count}");
        // Deactivate all icons first
        for (int i = 0; i < rulePlayingIcon.Length; i++)
            rulePlayingIcon[i].gameObject.SetActive(false);

        if (ruleNum.Count == 0) return;

        // Activate icons based on the ruleNum count
        for (int i = 0; i < ruleNum.Count && i < rulePlayingIcon.Length; i++)
        {
            Logger.Print($"SetRuleIconOnPlaying ruleNum  {i} : {ruleNum[i]}");
            rulePlayingIcon[i].gameObject.SetActive(true); // Activate the icon
            rulePlayingIcon[i].sprite = PrivateTable.instance.rulesSprite[ruleNum[i] - 1]; // Set the correct sprite
        }
    }

    public static Action resetGame;

    public void ResetTable(bool fromSwitch)
    {
        Logger.Print(TAG + "Table Is Now Reseted...");

        StopAllCoroutines();
        DOTween.KillAll();
        CancelInvoke();
        if (fromSwitch)
        {
            Logger.Print($"{TAG} + Table Is Now Reseted... {fromSwitch}");
            LiveTablePanel.Instance.currentTap = -1;

            LeftPlayerName.text = TopPlayerName.text = RightPlayerName.text = "INVITE";
            LeftPlayerImg.texture = RightPlayerImg.texture = TopPlayerImg.texture = inviteUser;
            leftPlayerFrame.sprite = topPlayerFrame.sprite = rightPlayerFrame.sprite = defaultFrame;

            BottomPlayerCardPile.SetActive(false);
            LeftPlayerCardPile.SetActive(false);
            TopPlayerCardPile.SetActive(false);
            RightPlayerCardPile.SetActive(false);

            leftPlayerImgButton.onClick.RemoveAllListeners();
            leftPlayerImgButton.onClick.AddListener(() =>
            {
                OnClick_InvitePlayers(1);
            });

            topPlayerImgButton.onClick.RemoveAllListeners();
            topPlayerImgButton.onClick.AddListener(() =>
            {
                OnClick_InvitePlayers(2);
            });

            rightPlayerImgButton.onClick.RemoveAllListeners();
            rightPlayerImgButton.onClick.AddListener(() =>
            {
                OnClick_InvitePlayers(3);
            });

            modeNameImg.sprite = modeNames[0];
            modeTypeImg.sprite = modeTypes[0];
        }

        if (hintActionToolTip.gameObject.activeInHierarchy)
            hintActionToolTip.gameObject.SetActive(false);
        Logger.Print($"isHintShow C :: false");
        //hintActionToolTip.gameObject.transform.localScale = Vector3.one;

        speceficHelpInfo.localScale = Vector3.zero;
        speceficColorStore = "";
        isClickSpecificBtn = isChooseSpecific = false;
        swapCard = "";

        isFlipStatus = false;
        isfiveSend = false;
        playinBG.sprite = playingBgSprites[0];
        reversePar.gameObject.SetActive(false);

        cardBunch.sprite = close_DeckFull.sprite;
        cardBunch.enabled = true;
        ruleshowToolTip.anchoredPosition = new Vector2(-300f, -20);

        symbolStaticCard.gameObject.SetActive(false);
        symbolStaticCard.transform.localPosition = discardPosStore;
        symbolStaticCard.transform.localScale = Vector3.one;

        CloseDeckCounter.transform.parent.transform.eulerAngles = new Vector3(35, 0, 0);
        closeDeckImg.transform.eulerAngles = new Vector3(35, 0, 0);
        flipCloseDeckImg.transform.eulerAngles = new Vector3(35, 0, 0);

        playingHandTutorial.gameObject.SetActive(false);
        playingClickHand.gameObject.SetActive(false);

        //Wild up highlate card
        wildDummyShowObject.SetActive(false);
        wildDummyShowObject.transform.localScale = Vector3.one;

        foreach (var item in shildPlayerImg)
            item.localScale = Vector3.zero;

        foreach (var item in discardList)
            Destroy(item.gameObject);

        foreach (var stack in stackPanelty)
            Destroy(stack.gameObject);

        stackPanelty.Clear();
        discardList.Clear();

        ShowHighFiveHand(false);
        ShowPlus4SpeceficButtons(false);
        ShowSwapButtons(false);
        // ===>>> Action Cyclone 
        cycloneObject.gameObject.SetActive(false);
        cycloneFireWork.gameObject.SetActive(false);

        cycloneObject.transform.localPosition = Vector3.zero;
        cycloneParticle.localScale = Vector3.zero;
        cycloneImg.localScale = Vector3.zero;
        cycloneImg.localEulerAngles = Vector3.zero;

        glowFadeAnim.gameObject.SetActive(false);

        wildUpCounter.transform.localScale = Vector3.one;
        wildUpCounter.text = "";

        wildUpCounter.gameObject.SetActive(false);
        var main = wildUpAnimParticle.main;
        var spk = sparks1.main;
        var spk2 = sparks2.main;

        main.startColor = new ParticleSystem.MinMaxGradient(new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, 0));
        spk.startColor = new ParticleSystem.MinMaxGradient(new Color(spk.startColor.color.r, spk.startColor.color.g, spk.startColor.color.b, 0));
        spk2.startColor = new ParticleSystem.MinMaxGradient(new Color(spk2.startColor.color.r, spk2.startColor.color.g, spk2.startColor.color.b, 0));

        wildUpAnimParticle.gameObject.SetActive(false);
        drawBtn.gameObject.SetActive(false);

        if (CardDeckController.instance.selectCard != null)
            Destroy(CardDeckController.instance.selectCard.gameObject);
        CardDeckController.instance.TrappCardDestroy();

        for (int i = 0; i < dummyCard.Count; i++)
        {
            Destroy(dummyCard[i].gameObject);
        }
        dummyCard.Clear();

        blockTxtImg.transform.parent.transform.localScale = Vector2.zero;
        blockTxtImg.transform.parent.gameObject.SetActive(false);

        plusAnimImg.transform.localScale = Vector3.zero;
        plusAnimGlow.transform.localScale = Vector3.zero;

        reverseRotation.ResetThisScript();

        GridLayoutGroup leftGrid = leftPlayerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup topGrid = partnerCardGrid.GetComponent<GridLayoutGroup>();
        GridLayoutGroup rightGrid = rightPlayerCardGrid.GetComponent<GridLayoutGroup>();

        leftGrid.enabled = topGrid.enabled = rightGrid.enabled = true;

        Logger.Print($"|| RESET || partnerCardGrid . c {partnerCardGrid.transform.childCount}");

        for (int i = partnerCardGrid.transform.childCount - 1; i >= 0; i--)
        {
            if (i != 0) DestroyImmediate(partnerCardGrid.transform.GetChild(i).gameObject);
            else
                partnerCardGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = MyCardGrid.transform.childCount - 1; i >= 0; i--)
        {
            if (i != 0) DestroyImmediate(MyCardGrid.transform.GetChild(i).gameObject);
            else
            {
                MyCardGrid.transform.GetChild(0).GetComponent<CardController>().disableImg.gameObject.SetActive(false);
                MyCardGrid.transform.GetChild(0).GetComponent<CardController>().myRect.sizeDelta = new Vector2(148, 196);
                MyCardGrid.transform.GetChild(0).GetComponent<Image>().sprite = cardSprite.sprite;
                MyCardGrid.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(529f, -111.3667f);
                MyCardGrid.transform.GetChild(i).gameObject.SetActive(false);

                Canvas canvas = MyCardGrid.transform.GetChild(i).GetComponent<Canvas>();
                if (canvas != null)
                {
                    Destroy(canvas);
                }
            }
        }

        List<GameObject> left = new();
        List<GameObject> right = new();
        for (int i = 0; i < leftPlayerCardGrid.transform.childCount; i++)
            left.Add(leftPlayerCardGrid.transform.GetChild(i).gameObject);

        for (int i = 0; i < rightPlayerCardGrid.transform.childCount; i++)
            right.Add(rightPlayerCardGrid.transform.GetChild(i).gameObject);

        Logger.Print($"|| RESET || leftPlayerCardGrid . c {left.Count} || ");
        Logger.Print($"|| RESET || rightPlayerCardGrid . c {right.Count}");
        for (int i = 0; i < left.Count; i++)
        {
            DestroyImmediate(left[i].gameObject);
        }

        for (int i = 0; i < right.Count; i++)
        {
            DestroyImmediate(right[i].gameObject);
        }
        left.Clear();
        right.Clear();

        animPosCard.SetActive(false);

        if (tcodFlage) tcodFlage = false;
        CenterGlowCard.reverse = false;
        CenterGlowCard.ringImg.sprite = CenterGlowCard.defaultImg;

        BottomThrowCard.SetActive(false);
        LeftThrowCard.SetActive(false);
        TopThrowCard.SetActive(false);
        RightThrowCard.SetActive(false);

        Vector2 allpos = new Vector2(97, 5);
        BottomThrowCard.transform.localPosition = LeftThrowCard.transform.localPosition = TopThrowCard.transform.localPosition = RightThrowCard.transform.localPosition = allpos;

        BottomThrowCard.transform.eulerAngles = new Vector3(35, 0, 0);
        LeftThrowCard.transform.eulerAngles = new Vector3(35, 0, 0);
        TopThrowCard.transform.eulerAngles = new Vector3(35, 0, 0);
        RightThrowCard.transform.eulerAngles = new Vector3(35, 0, 0);

        bottomFlipTag.SetActive(false);
        leftFlipTag.SetActive(false);
        topFlipTag.SetActive(false);
        rightFlipTag.SetActive(false);

        BottomPlayerTurnRing.gameObject.SetActive(false);
        LeftPlayerTurnRing.gameObject.SetActive(false);
        TopPlayerTurnRing.gameObject.SetActive(false);
        RightPlayerTurnRing.gameObject.SetActive(false);

        BottomDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
        LeftDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
        TopDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;
        RightDealCard.transform.GetComponent<Image>().sprite = cardSprite.sprite;

        LeftPlayerGift.texture = DefaultGift;
        TopPlayerGift.texture = DefaultGift;
        RightPlayerGift.texture = DefaultGift;

        cardBunch.GetComponent<Button>().onClick.RemoveAllListeners();
        cardBunch.GetComponent<Button>().onClick.AddListener(OpenDeckClick);

        for (int i = 0; i < msgTooltips.Count; i++) msgTooltips[i].SetActive(false);

        bottomPlayerEmoji.gameObject.SetActive(false);
        leftPlayerEmoji.gameObject.SetActive(false);
        topPlayerEmoji.gameObject.SetActive(false);
        rightPlayerEmoji.gameObject.SetActive(false);


        BottomPlayerGift.enabled = false;
        LeftPlayerGift.enabled = false;
        TopPlayerGift.enabled = false;
        RightPlayerGift.enabled = false;

        bottomPlayerGiftBtn.gameObject.SetActive(false);
        leftPlayerGiftBtn.gameObject.SetActive(false);
        topPlayerGiftBtn.gameObject.SetActive(false);
        rightPlayerGiftBtn.gameObject.SetActive(false);
        messageBtn.gameObject.SetActive(false);

        BottomPlayerCardCounter.text = "0";
        LeftPlayerCardCounter.text = "0";
        TopPlayerCardCounter.text = "0";
        RightPlayerCardCounter.text = "0";

        LeftPlayerAnimCard.SetActive(false);
        TopPlayerAnimCard.SetActive(false);
        RightPlayerAnimCard.SetActive(false);

        LeftPlayerAnimCardCopy.SetActive(false);
        TopPlayerAnimCardCopy.SetActive(false);
        RightPlayerAnimCardCopy.SetActive(false);

        deckGlow.SetActive(false); handIMg.gameObject.SetActive(false);
        RightDealCard.transform.position = TopDealCard.transform.position = BottomDealCard.transform.position = LeftDealCard.transform.position = demoCard.transform.position;
        RightDealCard.transform.rotation = TopDealCard.transform.rotation = BottomDealCard.transform.rotation = LeftDealCard.transform.rotation = RotationDemo.transform.rotation;
        RightDealCard.transform.localScale = TopDealCard.transform.localScale = BottomDealCard.transform.localScale = LeftDealCard.transform.localScale = demoCard.transform.localScale;

        TopDealCard.SetActive(false);
        BottomDealCard.SetActive(false);
        RightDealCard.SetActive(false);
        LeftDealCard.SetActive(false);

        LeftPlayerAnimCard.transform.position = LeftPlayerAnimCardCopy.transform.position;
        LeftPlayerAnimCard.transform.rotation = LeftPlayerAnimCardCopy.transform.rotation;

        TopPlayerAnimCard.transform.position = TopPlayerAnimCardCopy.transform.position;
        TopPlayerAnimCard.transform.rotation = TopPlayerAnimCardCopy.transform.rotation;

        RightPlayerAnimCard.transform.position = RightPlayerAnimCardCopy.transform.position;
        RightPlayerAnimCard.transform.rotation = RightPlayerAnimCardCopy.transform.rotation;

        BottomUnoTxt.SetActive(false);
        LeftUnoTxt.SetActive(false);
        TopUnoTxt.SetActive(false);
        RightUnoTxt.SetActive(false);

        BottomCollectAnimFrm.SetActive(false);
        LeftCollectAnimFrm.SetActive(false);
        TopCollectAnimFrm.SetActive(false);
        RightCollectAnimFrm.SetActive(false);

        BottomPlayerCard.Clear();
        LeftPlayerCard.Clear();
        TopPlayerCard.Clear();
        RightPlayerCard.Clear();

        UnoBtn.SetActive(false);
        unoTextParticle.SetActive(false);

        foreach (var item in catchUNOBtn)
            item.gameObject.SetActive(false);

        unoForgotterSI = -1;
        isInUNO = false;

        animationTxtObjCoin.SetActive(false);
        animationTxtObjGems.SetActive(false);

        turnMissToolTip.SetActive(false);
        GameStartTimer.SetActive(false);
        closeDeckCounter = 132;
        CloseDeckCounter.text = "0";
        BetValue.text = "0";
        PotValue.text = "0";
        roundText.text = "0";
        LeftCardDeal = RightCardDeal = TopCardDeal = BottomCardDeal = 0;
        TotalDealCard = 7;
        lastPickCard = "";

        isKeep = isUno = MyTurn = false;
        isKeepedCard = mySideSayUno = isKeepPopupOn = isUnoPressed = animWaitCom = isPaneltyAnimOn = isAfterEG = false;

        cIndex = -1;
        challengeDackClickAble = true;
        ChooseColorPanel.SetActive(false);
        KeepPlayPanel.SetActive(false);
        OnClick_BackMenu(0, false);
        OnClick_BackMenu(6, false);

        leafAnimationParent.SetActive(false);

        if (manageTimer != null)
        {
            timerAnim?.Kill();
            Invoke(nameof(ImageAlphaClear), 0.1f);
            StopCoroutine(manageTimer);
        }

        DailyMission.instance.DmListDestroy();

        UTSAnimation(null);

        //Winning Animation...
        for (int i = 0; i < winningTags.Count; i++) winningTags[i].SetActive(false);
        for (int i = 0; i < winPoints.Count; i++) winPoints[i].gameObject.SetActive(false);
        for (int i = 0; i < crackerParticles.Count; i++)
        {
            //Logger.Print(TAG + "Crackers Off Now");
            crackerParticles[i].SetActive(false);
        }
        for (int i = 0; i < HighlightObjects.Count; i++)
        {
            if (HighlightObjects[i].GetComponent<Canvas>())
            {
                Destroy(HighlightObjects[i].GetComponent<Canvas>());
            }
        }
        for (int k = 0; k < SelectColorImg.Length; k++)
        {
            SelectColorImg[k].gameObject.transform.localPosition = resetExtraPositions[k];
            SelectColorImg[k].gameObject.transform.localScale = Vector3.one;

            Image selectImg = SelectColorImg[k].GetComponent<Image>();
            if (!selectImg.color.a.Equals(1))
                selectImg.DOFade(1, 0);
        }

        for (int id = 0; id < ResetColor.Length; id++)
        {
            SelectColorImg[id].sprite = ResetColor[id];
        }

        winningTextObject.SetActive(false);
        lossTextObject.SetActive(false);
        kingObject.SetActive(false);

        //Challenge Animation...
        hummarObject.SetActive(false);
        hammer1.SetActive(false);
        hammer2.SetActive(false);
        challengeBottom.SetActive(false);
        challengeLeft.SetActive(false);
        challengeTop.SetActive(false);
        challengeRight.SetActive(false);
        challengeParticle.SetActive(false);

        //Skip Animation...
        skipImgObj.SetActive(false);
        skipImgObj2.SetActive(false);
        skipImgObj3.SetActive(false);
        skipImgObj4.SetActive(false);
        skipParticle.SetActive(false);
        skipParticle2.SetActive(false);

        //Reverse Animation...
        arrowsObj.SetActive(false);
        arrow1Obj.SetActive(false);
        arrow2Obj.SetActive(false);
        arrowSpawnParticle.SetActive(false);

        ImageAlphaClear();
        AudioManager.instance.tikAudioSource.Stop();

        //+2, +4 Animation...
        for (int i = 0; i < cardAddedImgs.Count; i++) cardAddedImgs[i].gameObject.SetActive(false);
        for (int i = 0; i < cardAddTextParticles.Count; i++) cardAddTextParticles[i].gameObject.SetActive(false);

        ArrowAnimation(false);

        resetGame?.Invoke();

        DisableToolTip();
        GC.Collect(); // For Clear GC Data...............................................................!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        DashboardManager.instance.AllDashAnimRestart(); // Restart Animation
        Logger.Print(TAG + " Reset Table Over >>> ");
    }

    private void ImageAlphaClear()
    {
        Color imageColor = new Color(255, 255, 255, 0); // Create a transparent color
        timerAnim?.Kill();

        if (!Mathf.Approximately(BottomPlayerTurnRing.EndTurnFadeImg().color.a, 0))
            BottomPlayerTurnRing.EndTurnFadeImg().color = imageColor;


        if (!Mathf.Approximately(LeftPlayerTurnRing.EndTurnFadeImg().color.a, 0))
            LeftPlayerTurnRing.EndTurnFadeImg().color = imageColor;

        if (!Mathf.Approximately(TopPlayerTurnRing.EndTurnFadeImg().color.a, 0))
            TopPlayerTurnRing.EndTurnFadeImg().color = imageColor;

        if (!Mathf.Approximately(RightPlayerTurnRing.EndTurnFadeImg().color.a, 0))
            RightPlayerTurnRing.EndTurnFadeImg().color = imageColor;
    }

}

[System.Serializable]
public class GTIResponse
{
    public string name, uid, pp, GiftImg, frameImage, deckImage, cardImage, bunch1, bunch2;
    public int si;
    public bool isCom;

    public List<string> PlayerCard = new List<string>();

    public List<List<string>> PlayerSpread = new List<List<string>>();

    public GTIResponse(string name, string uid, int si, string pp, List<string> PlayerCard, string GiftImg, bool iscom, string frameImg,
        string deckImg, string cardImg, string bunch1, string bunch2)
    {
        this.name = name;
        this.uid = uid;
        this.si = si;
        this.pp = pp;
        this.PlayerCard = PlayerCard;
        this.GiftImg = GiftImg;
        this.isCom = iscom;
        frameImage = frameImg;
        deckImage = deckImg;
        cardImage = cardImg;
        this.bunch1 = bunch1;
        this.bunch2 = bunch2;
    }

    public List<string> GetPlayerCard()
    {
        return PlayerCard;
    }

    public string getGiftImg()
    {
        return GiftImg;
    }

    public string getPn()
    {
        return name;
    }

    public string getUid()
    {
        return uid;
    }

    public bool getIscom()
    {
        return isCom;
    }

    public int getSi()
    {
        return si;
    }

    public string getPp()
    {
        return pp;
    }

    public string GetProfileFrame()
    {
        return frameImage;
    }

    public string GetDeckImage() { return deckImage; }
    public string GetCardImage() { return cardImage; }

    public string Bunch1()
    {
        return bunch1;
    }
    public string Bunch2()
    {
        return bunch2;
    }
}
