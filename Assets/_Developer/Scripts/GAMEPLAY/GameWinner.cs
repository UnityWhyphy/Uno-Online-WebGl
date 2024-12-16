using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameWinner : MonoBehaviour
{
    public static GameWinner instance;
    private static string TAG = ">>WINNER ";

    public GameObject WinnerPannel;
    [SerializeField] Image lostGoldBG;
    [SerializeField] Transform lostGoldPopUp;
    [SerializeField] TextMeshProUGUI lostGoldTxt;
    [SerializeField] TextMeshProUGUI lostGoldTimerTxt;

    [Header("WinnerData")]
    [SerializeField] Image[] playerRings;
    public RawImage[] PlayerImg;
    public TextMeshProUGUI[] PlayerName, PlayerLevel, PlayerWinChips;
    [SerializeField] List<Image> winnerTags;
    [SerializeField] Image chestSprite;
    [SerializeField] Transform chesrot;
    public Sprite[] ChestImgArray;

    [SerializeField] Sprite defaultRing;
    [SerializeField] Texture defaultProfile;
    [SerializeField] GameObject[] youTags;
    [SerializeField] HorizontalLayoutGroup playerDetailsLayout;
    [SerializeField] GameObject vsTextObject;

    [SerializeField] TextMeshProUGUI gameStartTimerTxt;

    [SerializeField] TextMeshProUGUI playBtnText;
    [SerializeField] List<GameObject> coins;
    [SerializeField] Button ExitBtn;
    [SerializeField] Button exitBtnTutorial;
    [SerializeField] GameObject selectRuleAgain;

    public void Awake()
    {
        Logger.Print(TAG + " Awake called");
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_WIN += OnRecievedWin;
        SocketManagergame.Onlistner_ATC += HandleChestData;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_WIN -= OnRecievedWin;
        SocketManagergame.Onlistner_ATC -= HandleChestData;
    }

    [HideInInspector] public long bv = 100;
    int gems = 0, player = 4;
    string mode = "CLASSIC";
    bool isSingleRound = false;
    [HideInInspector] public bool isTournament = false;

    private void OnRecievedWin(JSONNode data)
    {

        if (AppData.isTutorialPlay)
        {
            TutorialWin(TutorialManager.Instance.tutorialWinnerMain);
            return;
        }

        Logger.Print(TAG + " OnRecievedWin called " + data["data"].Count);

        vsTextObject.SetActive(data["mode"] == AppData.PARTNER || data["mode"] == AppData.EMOJIPARTNER);
        playerDetailsLayout.spacing = data["mode"] == AppData.PARTNER || data["mode"] == AppData.EMOJIPARTNER ? -40 : -300;
        isTournament = data["istournament"].AsBool;

        ProfilePanel.instance.ProfilePanelClick(10); // Opponent Panel Close

        List<Winner> PlayerWinner = JsonConvert.DeserializeObject<List<Winner>>(data["data"].ToString());

        SetDataForIsTournament(isTournament);

        AudioManager.instance.tikAudioSource.Stop();
        bool isiWin = false;

        for (int i = 0; i < PlayerWinner.Count; i++)
        {
            GameManager.instance.WinRedrawCard(PlayerWinner[i].si, PlayerWinner[i].cards/*, PlayerWinner[i].newxp*/); // For endGame Open All player card.....!!

            PlayerImg[i].texture = defaultProfile;
            playerRings[i].sprite = defaultRing;
            PlayerName[i].text = PlayerWinner[i].pn;
            winnerTags[i].sprite = GameManager.instance.winningTagSprites[data["mode"] == AppData.PARTNER || data["mode"] == AppData.EMOJIPARTNER ?
                (PlayerWinner[i].w == 0 ? 1 : 0) : (isTournament) ? i : PlayerWinner[i].rank - 1];
            PlayerLevel[i].text = (PlayerWinner[i].w == 1) ? $"Win" : $"Loss";
            PlayerWinChips[i].text = PlayerWinner[i].rw < 0 ? "-" + AppData.numDifferentiation(Mathf.Abs(PlayerWinner[i].rw)) : AppData.numDifferentiation(PlayerWinner[i].rw);
            youTags[i].SetActive(PrefrenceManager._ID == PlayerWinner[i].uid);
            StartCoroutine(AppData.ProfilePicSet(PlayerWinner[i].pp, PlayerImg[i]));

            if (PrefrenceManager._ID == PlayerWinner[i].uid)
            {
                myIndex = i;

            }
            //StartCoroutine(AppData.SpriteSetFromURL(PlayerWinner[i].frameImg, playerRings[i], "OnRecievedWin"));

            if (PlayerWinner[i].w == 1 && PlayerWinner[i].uid.Equals(PrefrenceManager._ID))
                FirebaseData.EventSendWithFirebase("GameWin");
        }

        if (!isiWin)
            FirebaseData.EventSendWithFirebase("GameLoss");

        ExitBtn.interactable = true;
        bv = data["bv"];
        mode = data["mode"];
        gems = data["gems"];
        gameStartTimerTxt.text = "Waiting For New Round...";
        List<string> SingleRoundPlayerIds = JsonConvert.DeserializeObject<List<string>>(data["isSingleround"].ToString());
        isSingleRound = SingleRoundPlayerIds.Contains(PrefrenceManager._ID);
        gameStartTimerTxt.gameObject.SetActive(!isSingleRound && !isTournament);

        if (SingleRoundPlayerIds.Contains(PrefrenceManager._ID))
            AppData.handleLeaveTable = false;
    }

    void TutorialWin(TutorialWinnerMain tutorialWinnerMain)
    {
        vsTextObject.SetActive(false);
        playerDetailsLayout.spacing = -300;

        ProfilePanel.instance.ProfilePanelClick(10); // Opponent Panel Close

        List<WinData> PlayerWinner = tutorialWinnerMain.data;

        AudioManager.instance.tikAudioSource.Stop();
        SetDataForIsTournament(false);

        for (int i = 0; i < PlayerWinner.Count; i++)
        {
            Logger.Print($"TutorialWin SI = {PlayerWinner[i].si} || Count = {PlayerWinner[i].cards.Count}");
            GameManager.instance.WinRedrawCard(PlayerWinner[i].si, PlayerWinner[i].cards); // For endGame Open All player card.....!!

            PlayerImg[i].texture = defaultProfile;
            playerRings[i].sprite = defaultRing;
            PlayerName[i].text = PlayerWinner[i].pn;
            winnerTags[i].sprite = GameManager.instance.winningTagSprites[i];

            PlayerLevel[i].text = (PlayerWinner[i].w == 1) ? $"Win" : $"Loss";
            PlayerWinChips[i].text = PlayerWinner[i].rw < 0 ? "-" + AppData.numDifferentiation(Mathf.Abs(PlayerWinner[i].rw)) : AppData.numDifferentiation(PlayerWinner[i].rw);
            //PlayerWinChips[i].gameObject.SetActive(false);
            youTags[i].SetActive(PrefrenceManager._ID == PlayerWinner[i].uid);
            PlayerImg[i].texture = TutorialManager.Instance.userDetails[i].profilePic;
            playerRings[i].sprite = TutorialManager.Instance.userDetails[i].profileFrm;
        }

        ExitBtn.interactable = true;
        gameStartTimerTxt.text = "";
    }

    public void OpenWinnerScreen(bool showAd = true, bool gameIsInBackground = false)
    {
        //Back Menu Check...
        Logger.Print($"OpenWinnerScreen :: {showAd} || gameIsInBackground {gameIsInBackground}");
        Logger.Print($"OpenWinnerScreen AppData.isReviewAd  = {AppData.isReviewAd}");
        //if (AppData.isReviewAd >= 0)
        //    if (DashboardManager.instance.totalMatch >= 5 && (AppData.winLossCoins >= 0) && !isTournament)
        //    {
        //        StartCoroutine(AppReview.Instance.ReviewPopup());
        //        AppData.isReviewAd--;
        //    }

        if (GameManager.instance.ip == 0)
        {
            GameManager.instance.ResetTable(true);//true
            if (!AppData.isTutorialPlay)
                Invoke(nameof(PopupOnDelayLostGold), 0.5f);

            if (ChestJson != null)
            {
                if (ChestJson["data"].HasKey("isfull"))
                {
                    isChestFull = true;
                }
                else
                {
                    chestAddName = ChestJson["data"]["addchest"]["chestname"].Value;
                    StartCoroutine(ChestAddAnimation(myIndex));
                }
            }

            if (AppData.isTutorialPlay)
            {
                isSingleRound = true;
                AppData.isTutorialPlay = false;
                Logger.Print($" isTutorialPlay Flag update on Winner screen :::: ?? {AppData.isTutorialPlay}");
            }
        }
        else
        {
            GameManager.instance.ResetTable(false);//true
        }

        selectRuleAgain.SetActive((GameManager.instance.rules.Count > 0));

        int gpCount = AppData.totalPlayedMatch;

        Logger.Print($"gpCount : {gpCount} || AppData.totalPlayedMatch:  { AppData.totalPlayedMatch}");

        if (gpCount % 5 == 0 && gpCount <= 20 && gpCount < 25)
        {
            StartCoroutine(AppReview.Instance.ReviewPopup());
        }

        CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel.GetComponent<RectTransform>(), true);
        if (showAd) AdmobManager.instance.ShowInterstitialAd();
    }

    private void PopupOnDelayLostGold()
    {
        Logger.Print($"PopupOnDelayLostGold : PrefrenceManager.GOLD = {PrefrenceManager.GOLD} == {AppData.USERGOLDRECOVER} || {(AppData.winLossCoins <= 0)} | bv = {bv}");
        if (long.Parse(PrefrenceManager.GOLD) <= AppData.USERGOLDRECOVER && (AppData.winLossCoins <= 0))
        {
            Logger.Print($"PopupOnDelayLostGold : winLossCoins = {AppData.winLossCoins}");
            Logger.Print($"PopupOnDelayLostGold : {Mathf.Abs(AppData.winLossCoins / 2)} | BV = {bv / 2}");

            lostGoldTxt.text = (isTournament) ? $"{bv / 2}" : $"{Mathf.Abs(AppData.winLossCoins / 2)}";
            LostGoldPopupStatus(true);
        }
    }

    public void LostGoldPopupStatus(bool isActive)
    {
        if (isActive)
        {
            timer = 10;
            CancelInvoke(nameof(CountDown));
            InvokeRepeating(nameof(CountDown), 1, 1);
            CommanAnimations.instance.PopUpAnimation(lostGoldBG.gameObject, lostGoldBG, lostGoldPopUp, Vector3.one, true);
        }
        else
        {
            CommanAnimations.instance.PopUpAnimation(lostGoldBG.gameObject, lostGoldBG, lostGoldPopUp, Vector3.zero, false);
        }
    }

    int timer = 0;

    void CountDown()
    {
        if (timer > 0)
        {
            timer--;
            lostGoldTimerTxt.text = $"{timer}";
        }
        else
        {
            lostGoldTimerTxt.text = $"0";
            LostGoldPopupStatus(false);
            CancelInvoke(nameof(CountDown));
        }
    }

    private void SetDataForIsTournament(bool isTour)
    {
        gameStartTimerTxt.gameObject.SetActive(!isTour);
        for (int i = 0; i < coins.Count; i++) coins[i].SetActive(!isTour);
        playBtnText.fontSize = (isTour) ? 38 : 53;
        playBtnText.text = (isTour) ? "START NEW TOURNAMENT" : "NEW GAME";
        if (AppData.isTutorialPlay)
            playBtnText.text = "NEW GAME";
    }

    public void NewGameExitClick(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (i)
        {
            case 0://exit Game
                if (isSingleRound || isTournament)
                {
                    Logger.Print(TAG + "is SingleRound >> " + isSingleRound + " Or Is Tournament >> " + isTournament + " | istourShowFeed = " + AppData.istourShowFeed);
                    EventHandler.LgsReqSend();
                    GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
                    Loading_screen.instance.ShowLoadingScreen(false);
                    CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
                    CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel, false);
                    if (AppData.promoflag.Equals("")) WinLossScreen.instance.ShowWatchVideoPopup(true);

                    if (AppData.gameStartCount == 2)
                    {
                        setting_script.instance.FeedbackPopupShow(true); // Winner exit
                    }

                    if (isTournament)
                    {
                        if (AppData.istourShowFeed)
                        {
                            AppData.istourShowFeed = false;
                            setting_script.instance.FeedbackPopupShow(true); // Tour Winner exit
                        }

                        // Intro tutorial
                        GameManager.instance.ShowIntroOnDashboard();
                    }

                }
                else
                {
                    Logger.Print(TAG + "Not SingleRound && Not Tournament");
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.ExitGame();
                }
                break;

            case 1://new Game
                if (isTournament)
                {
                    Logger.Print(TAG + "Playing Tournament");
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.LgsReqSend();
                    GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
                    GameManager.instance.playingScreen.gameObject.SetActive(false);
                    CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
                    WinnerPannel.gameObject.SetActive(false);
                    CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel, false);
                    EventHandler.ListofTournament();
                }

                else if (isSingleRound)
                {
                    Logger.Print(TAG + "Playing Single Round");
                    if (AllCommonGameDialog.instance.isHaveGoldGems(bv, gems))
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.LgsReqSend();
                        GameManager.instance.BacktoLobby = true;

                        JSONArray rulesArray = new JSONArray();
                        if (GameManager.instance.rules != null && GameManager.instance.rules.Count > 0)
                        {
                            foreach (int rule in GameManager.instance.rules)
                            {
                                rulesArray.Add(rule);
                            }
                        }

                        EventHandler.PlayGame(mode, bv, player, gems, 0, rulesArray);
                    }
                    else
                    {
                        GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
                        CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
                    }
                }
                else
                {
                    Logger.Print(TAG + "Panel Close");
                    GameManager.instance.BacktoLobby = ExitBtn.interactable;
                    CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel.GetComponent<RectTransform>(), false);
                }
                break;

            case 2: // Select rule 
                if (GameManager.instance.rules.Count > 0)
                {
                    Logger.Print(TAG + "Panel Select 2");
                    WinLossScreen.instance.WinLossScreenFalse();
                    CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel.GetComponent<RectTransform>(), false);
                    CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);

                    GameManager.instance.BacktoLobby = ExitBtn.interactable;
                    GameManager.instance.allPlayingScreen.gameObject.SetActive(false);

                    PrivateTable.instance.CustomizeBootPanel(mode);
                    //PrivateTable.instance.CustomModeSelect(0);
                    PrivateTable.instance.AutoSelectRules(GameManager.instance.rules);
                    PrivateTable.instance.AutoSelectBetSlot();
                }
                break;
        }
    }

    public IEnumerator GameStartTimerStart(int p)
    {
        gameStartTimerTxt.text = "New Round Will Be Start After " + p + " sec...";
        for (int i = p; i >= 0; i--)
        {
            ExitBtn.interactable = i > 2;
            gameStartTimerTxt.text = "New Round Will Be Start After " + i + " sec...";
            yield return new WaitForSeconds(1f);
        }

        CommanAnimations.instance.FullScreenPanelAnimation(WinnerPannel.GetComponent<RectTransform>(), false);
        StopCoroutine(GameStartTimerStart(p));
    }


    bool isChestFull = false;
    string chestAddName = "";
    int myIndex = -1;
    JSONNode ChestJson;
    private void HandleChestData(JSONNode data)
    {
        Logger.Print(TAG + " HandleChestData called");

        if (GameManager.instance == null || !GameManager.instance.playingScreen.activeSelf)
            return;

        ChestJson = data;
    }

    Sprite GetChestImgSprite(string name)
    {
        switch (name)
        {
            case "silver":
                return ChestImgArray[1];

            case "gold":
                return ChestImgArray[2];

            default:
                return ChestImgArray[0];
        }
    }

    IEnumerator ChestAddAnimation(int winIndex)
    {
        yield return new WaitForSeconds(2f);

        chestSprite.gameObject.SetActive(true);

        chestSprite.sprite = GetChestImgSprite(chestAddName);
        Sequence seq = DOTween.Sequence();

        RectTransform rec = chestSprite.GetComponent<RectTransform>();

        chesrot.localScale = chestSprite.transform.localScale = Vector3.zero;
        chestSprite.transform.DOMove(new Vector2(playerRings[winIndex].transform.position.x, -3), 0);
        chesrot.transform.DOMove(new Vector2(playerRings[winIndex].transform.position.x, -3), 0);

        seq.Append(chestSprite.transform.DOScale(Vector3.one, 1).OnStart(() =>
        {
            chesrot.transform.DOScale(Vector3.one, 1);
            chesrot.transform.DOLocalRotate(new Vector3(0, 0, 200), 1, RotateMode.FastBeyond360).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear);
            AudioManager.instance.AudioPlay(AudioManager.instance.OfferComeClip);
        }));


        seq.Join(chestSprite.transform.DOShakeRotation(.5f).SetDelay(0.5f));

        seq.Append(chestSprite.transform.DOMove(playerRings[winIndex].transform.position, 1).SetDelay(0.3f).OnStart(() =>
        {
            chestSprite.transform.DOShakeRotation(1f, 40, 5, 90, true, ShakeRandomnessMode.Harmonic);
        }).OnComplete(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ChestAdd);
            chesrot.gameObject.SetActive(false);
        }));
        seq.Join(chesrot.transform.DOMove(playerRings[winIndex].transform.position, 1));

        seq.Append(chestSprite.transform.DOScale(Vector3.zero, 0.5f));
        seq.Join(chesrot.transform.DOScale(Vector3.zero, 0.5f));

        ChestJson = null;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //chestAddName = "bronze";
            //StartCoroutine(ChestAddAnimation(0));
            //chesrot.transform.DOLocalRotate(new Vector3(0, 0, 200), 1,RotateMode.FastBeyond360).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }
}
