using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using DG.Tweening;
using TMPro;
using System.Linq;

public class DashboardManager : MonoBehaviour
{
    private static string TAG = ">>>DASHBORD ";

    public static DashboardManager instance;
    [SerializeField] GameObject tournamentBtn, customPlayBrn;
    public Camera MainCamera;

    [Header("ALL Panle ")]
    [SerializeField] List<GameObject> closablePanels;


    [Header("Tutorial IntroTxt")]
    public Transform centerScroll;
    public GameObject introGirl;
    public TextMeshProUGUI IntroTxt;
    public GameObject transParentImg;
    public ModeButtonModle[] tu_modeButtons;
    public Button flipBootSelectBtn;
    public GameObject[] modeoutlines;

    [Space(5)]
    public GameObject centerPos;
    public GameObject rightpos;
    public GameObject leftPos;

    public TextMeshProUGUI PlayerName, PlayerGold, PlayerGems, PlayerLevel, levelProgress, levelPercent;
    public RawImage PlayerImg;
    public RawImage playerFrame;
    [SerializeField] Slider levelSlider;

    [Header(" Coin Content")]
    [SerializeField] Transform coinPrent;
    [SerializeField] Transform gemsPrent;
    public Transform spawnLocation;
    [SerializeField] Transform endPosition;
    [SerializeField] Transform endGemsPosition;
    [SerializeField] TextMeshProUGUI userTextGold;
    [SerializeField] TextMeshProUGUI userTextGems;

    [Header("RateUs & MoreGame")]
    [SerializeField] GameObject rateUsPanel;
    [SerializeField] GameObject moreGamePanel;
    [SerializeField] Image rateUsBg, moreGameBg;
    [SerializeField] internal Transform rateUsPopUp, moreGamePopUp;
    [SerializeField] List<Sprite> rateSprites;
    [SerializeField] List<Image> rateImgs;

    public int CurrentLevel = 0;

    [Header("For Counter")]
    [SerializeField] GameObject buddyHubBtnCounter;
    public GameObject inboxBtnCounter;
    public TextMeshProUGUI buddyHubBtnCounterText, inboxBtnCounterText;
    public GameObject settingBtnCounter, weeklyWinnerCounter;
    public Image settingImg, buddyHubImg, inboxImg;
    public Sprite haveNoti, normal;

    [Header("Maintenence Tooltip")]
    public GameObject maintenenceToolTip;
    public TextMeshProUGUI maintenenceTimerTxt;

    [Header("Free Video Reward")]
    [SerializeField] GameObject videoObject, coinObject, freeTxtObject;
    [SerializeField] TextMeshProUGUI watchgoldText;
    [SerializeField] Button watchVideoBtn;

    public bool isWWAnounced = false;

    public int inboxCounter = 0, buddyHubCounter = 0, settingsCounter = 0;

    [SerializeField] List<DashboardAnimControl> allDashAnim = new();
    [Header("What's New")]
    [SerializeField] Transform whatsNewScreen;
    [SerializeField] Image[] updateImg;
    public Image[] pagination;
    public Sprite active_pagination, deactive_pagination;

    private void Awake()
    {
        if (instance == null) instance = this;

        Logger.Print(TAG + "My Golds As Per Prefrence Manager >> " + PrefrenceManager.GOLD);
        AdmobManager.instance.InitAds();
    }

    private void Start()
    {
        if (!AppData.SignUpData.Equals(""))
        {
            Logger.Print("SP Called as SignUp Event Called...2");
            Loading_screen.instance.ShowLoadingScreen(false);
            OnRecevied_SIGNUP(AppData.SignUpData);
        }
        Logger.Print($"Scene chamge Time in sec = {DateTime.Now.TimeOfDay}");
    }

    private void Update()
    {
        if (AppData.videoRewardTimer > 0) AppData.videoRewardTimer -= Time.deltaTime;
        HandleWatchVideoToGetReward();
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_SP += OnRecevied_SIGNUP;
        SocketManagergame.OnListner_LWU += OnReceived_Lwu;
        SocketManagergame.OnListner_OFC += OnRecevied_OFC;
        SocketManagergame.OnListner_MBV += HandleMBV;
        SocketManagergame.OnListner_UG += OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE += OnRecevied_USERGEMS;
        AppData.AllPanelClose += HandleCloseAllPanels;
        SocketManagergame.OnListner_LINFO += HandleLinfo;
        EventHandler.OnlineFriendsCounter();
    }

    void OnDisable()
    {
        SocketManagergame.OnListner_SP -= OnRecevied_SIGNUP;
        SocketManagergame.OnListner_LWU -= OnReceived_Lwu;
        SocketManagergame.OnListner_OFC -= OnRecevied_OFC;
        SocketManagergame.OnListner_MBV -= HandleMBV;
        SocketManagergame.OnListner_UG -= OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE -= OnRecevied_USERGEMS;
        SocketManagergame.OnListner_LINFO -= HandleLinfo;
        AppData.AllPanelClose -= HandleCloseAllPanels;
        isShowWO_OnFirstTime = false;
        AppData.isFirstTimeUSerEnter = false;
        PrefrenceManager.userFirstTimeOpen = 1;
    }

    public void AllDashAnimRestart()
    {
        foreach (var anims in allDashAnim)
        {
            anims.RestartAnimation();
        }
    }

    List<LevelWiseUnlock> levelWiseUnlocks = new List<LevelWiseUnlock>();

    private void OnReceived_Lwu(JSONNode data)
    {
        levelWiseUnlocks = JsonConvert.DeserializeObject<List<LevelWiseUnlock>>(data["level_wise_unlock"].ToString());

        HandleLockLevelWise();
    }

    public void HandleLockLevelWise()
    {

        foreach (var levelUnlock in levelWiseUnlocks)
        {
            bool isLevelUnlocked = AppData.currantLvl >= levelUnlock.level;

            foreach (var unlockKey in levelUnlock.unlock)
            {
                UpdateButtonUI(unlockKey, levelUnlock.level, isLevelUnlocked);
            }
        }
    }

    private void UpdateButtonUI(string unlockKey, int unlockLevel, bool isLevelUnlocked)
    {
        // Determine which button to update based on the unlock key
        int buttonIndex = -1;
        int ruleIndex = -1;
        bool isRuleButton = false;

        switch (unlockKey)
        {
            case "Custom":
                buttonIndex = 0;
                break;
            case "invert":
                buttonIndex = 1;
                flipBootSelectBtn.interactable = isLevelUnlocked;
                break;
            case "Live_Table":
                buttonIndex = 2;
                break;
            case "Tournament":
                buttonIndex = 3;
                break;
            case "2XSKIP":
                ruleIndex = 4;
                isRuleButton = true;
                break;
            case "CHALLENGE":
                ruleIndex = 7;
                isRuleButton = true;
                break;
            case "STACK":
                ruleIndex = 8;
                isRuleButton = true;
                break;
            case "SHIELD":
                ruleIndex = 1;
                isRuleButton = true;
                break;
            case "0-7":
                ruleIndex = 5;
                isRuleButton = true;
                break;
            case "HIGH-5":
                ruleIndex = 6;
                isRuleButton = true;
                break;
            case "CYCLONE":
                ruleIndex = 3;
                isRuleButton = true;
                break;
            case "WildUp":
                ruleIndex = 0;
                isRuleButton = true;
                break;
            case "TARGET+4":
                ruleIndex = 2;
                isRuleButton = true;
                break;
        }

        Logger.Print($"buttonIndex : {buttonIndex} | ruleIndex: {ruleIndex} | isLevelUnlocked: {isLevelUnlocked}");

        if (buttonIndex >= 0 && buttonIndex < tu_modeButtons.Length)
        {
            // Set the lock image and level text
            tu_modeButtons[buttonIndex].click.interactable = isLevelUnlocked;
            tu_modeButtons[buttonIndex].lockImg.gameObject.SetActive(!isLevelUnlocked);
            tu_modeButtons[buttonIndex].lockLevelTxt.text = isLevelUnlocked ? "" : $"Unlock to level {unlockLevel}";
        }

        if (isRuleButton)
        {
            // Update the rule button's interactability (like flipBootSelectBtn or others)
            //flipBootSelectBtn.interactable = isLevelUnlocked;
            PrivateTable.instance.rulesButtons[ruleIndex].selectBtn.interactable = isLevelUnlocked;
            PrivateTable.instance.rulesButtons[ruleIndex].lockObj.gameObject.SetActive(!isLevelUnlocked);
            PrivateTable.instance.rulesButtons[ruleIndex].lockLevelTxt.text = isLevelUnlocked ? "" : $"Unlock to level {unlockLevel}";
        }
    }
    /*
        private void OnReceived_Lwu(JSONNode data)
        {
            List<LevelWiseUnlock> levelWiseUnlocks = JsonConvert.DeserializeObject<List<LevelWiseUnlock>>(data["level_wise_unlock"].ToString());
            Logger.Print($"LEVEL WISE UNLOCK: {levelWiseUnlocks[0].level}");

            Dictionary<string, int> keyToButtonIndex = new Dictionary<string, int>
            {
                { AppData.CustomKey, 0 },
                { AppData.InvertKey, 1 },
                { AppData.LiveTableKey, 2 },
                { AppData.TournamentKey, 3 }
            };

            foreach (var levelUnlock in levelWiseUnlocks)
            {
                bool isLevelUnlocked = AppData.currantLvl == levelUnlock.level;

                foreach (var unlockKey in levelUnlock.unlock)
                {
                    if (keyToButtonIndex.TryGetValue(unlockKey, out int buttonIndex))
                    {
                        UpdateButtonUI(buttonIndex, levelUnlock.level, isLevelUnlocked);
                    }
                }
            }

            for (int i = 0; i < levelWiseUnlocks.Count; i++)
            {
                switch (levelWiseUnlocks[i].level)
                {
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                }
            }
        }

        private void UpdateButtonUI(int buttonIndex, int unlockLevel, bool isLevelUnlocked)
        {
            var button = tu_modeButtons[buttonIndex];

            button.lockImg.gameObject.SetActive(!isLevelUnlocked);

            button.lockLevelTxt.text = $"Unlock to \n level {unlockLevel}";

            button.click.interactable = isLevelUnlocked;

            if (buttonIndex == 1)
            {
                flipBootSelectBtn.interactable = isLevelUnlocked;
            }
        }
        */
    /*
        void SetData(JSONNode data)
        {
            List<LevelWiseUnlock> LWU = JsonConvert.DeserializeObject<List<LevelWiseUnlock>>(data["level_wise_unlock"].ToString());
            Logger.Print($"LEVEL WISE  UNLOCK L:::: {LWU[0].level}");
            for (int i = 0; i < LWU.Count; i++)
            {
                for (int j = 0; j < LWU[i].unlock.Count; j++)
                {
                    switch (LWU[i].unlock[j])
                    {
                        case AppData.CustomKey:

                            tu_modeButtons[0].lockImg.gameObject.SetActive(AppData.currantLvl != LWU[i].level);
                            tu_modeButtons[0].lockLevelTxt.text = $"Unlock to \n level {LWU[i].level}";
                            tu_modeButtons[0].click.interactable = (AppData.currantLvl == LWU[i].level);
                            break;

                        case AppData.InvertKey:

                            tu_modeButtons[1].lockImg.gameObject.SetActive(AppData.currantLvl != LWU[i].level);
                            tu_modeButtons[1].lockLevelTxt.text = $"Unlock to \n level {LWU[i].level}";
                            tu_modeButtons[1].click.interactable = (AppData.currantLvl == LWU[i].level);
                            flipBootSelectBtn.interactable = AppData.currantLvl == LWU[i].level;
                            break;

                        case AppData.LiveTableKey:

                            tu_modeButtons[2].lockImg.gameObject.SetActive(AppData.currantLvl != LWU[i].level);
                            tu_modeButtons[2].lockLevelTxt.text = $"Unlock to \n level {LWU[i].level}";
                            tu_modeButtons[2].click.interactable = (AppData.currantLvl == LWU[i].level);
                            break;

                        case AppData.TournamentKey:

                            tu_modeButtons[3].lockImg.gameObject.SetActive(AppData.currantLvl != LWU[i].level);
                            tu_modeButtons[3].lockLevelTxt.text = $"Unlock to \n level {LWU[i].level}";
                            tu_modeButtons[3].click.interactable = (AppData.currantLvl == LWU[i].level);
                            break;
                    }
                }
            }
        }
    */
    public void DashOfferButtonClick(int number)
    {
        //StorePanel.Instance.SetOffersTapData();
        switch (number)
        {
            case 1:
                OfferDialog.instance.SetWelcomeOfferDialogData();
                break;
            case 2:
                StorePanel.Instance.SetLimitedTimerOfferDialogData();
                break;
            case 3:
                StorePanel.Instance.SetLimitedStockOfferDialogData();
                break;
        }
    }

    private void HandleWatchVideoToGetReward()
    {
        if (AppData.videoRewardTimer > 0)
        {
            watchgoldText.text = AppData.GetTimeInFormateMin((long)AppData.videoRewardTimer * 1000);
            videoObject.SetActive(false);
            coinObject.SetActive(false);
            freeTxtObject.SetActive(false);
            watchVideoBtn.interactable = false;
        }
        else
        {
            watchgoldText.text = AppData.numDifferentiation(AppData.VIDEOREWARDCOINS);
            videoObject.SetActive(true);
            coinObject.SetActive(true);
            freeTxtObject.SetActive(true);
            watchVideoBtn.interactable = true;
        }
    }

    private void HandleCloseAllPanels()
    {
        for (int i = 0; i < closablePanels.Count; i++) closablePanels[i].gameObject.SetActive(false);
    }

    private void OnRecevied_USERGOLD(JSONNode data)
    {
        PrefrenceManager.GOLD = data["gold"];
        PlayerGold.text = AppData.numDifferentiation(data["gold"].AsLong);

        StorePanel.Instance.myGoldText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));

        if (data["tp"].Value.Contains("Video Bonus") || data["tp"].Equals("Video Reward") ||
            data["tp"].Value.Contains("Hourly Bonus") || data["tp"].Value.Contains("Magic Box") || data["tp"].Value.Contains("Daily Bonus") ||
            data["tp"].Value.Contains("Scratch Card Reward"))
        {
            //AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.congratulations, "THE " + AppData.numDifferentiation(data["goldAdded"].AsLong) + " " + data["tp"].Value + " "
            //+ "HAS BEEN SUCCESSFULLY CREDITED TO YOUR ACCOUNT. YOU CAN TRACK IT IN YOUR GOLD HISTORY.");
            ChipsAddAnimation(data["goldAdded"].AsLong, data["gold"].AsLong);
        }

        Logger.Print($"{data["tp"]}");
        if (data["tp"].Value.Contains("Daily Video Bonus") || data["tp"].Value.Contains("Level up Video Bonus") || data["tp"].Value.Contains("Hourly Video Bonus"))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.congratulations, "THE " + AppData.numDifferentiation(data["goldAdded"].AsLong) + " " + data["tp"].Value + " "
            + "HAS BEEN SUCCESSFULLY CREDITED TO YOUR ACCOUNT. YOU CAN TRACK IT IN YOUR GOLD HISTORY.");
        }

    }

    private void OnRecevied_USERGEMS(JSONNode data)
    {
        PrefrenceManager.GEMS = data["gems"];
        PlayerGems.text = AppData.numDifferentiation(data["gems"].AsLong);
        StorePanel.Instance.myGemsText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));
        //ChipsAddAnimation(data["gemsAdded"].AsLong, data["gems"].AsLong);
    }

    public void ChipsAddAnimation(long chips, long totalChips)
    {
        Action action = () =>
        {
            userTextGold.text = AppData.numDifferentiation(totalChips);
        };

        CollectCoinAnimation.instance.SetConfiguration(coinPrent, spawnLocation, endPosition, userTextGold, chips, action, 0, 20);
    }

    internal void ResetXpPoint(Oldxp newXp)
    {
        levelProgress.text = newXp.cp + "/" + newXp.nlvp;
        levelPercent.text = newXp.per + "%";
        levelSlider.value = newXp.per;
    }

    private void HandleLinfo(JSONNode data)
    {
        levelSlider.value = data["level_info"]["per"].AsInt;
        levelPercent.text = data["level_info"]["per"] + "%";
        levelProgress.text = data["level_info"]["cp"] + "/" + data["level_info"]["nlvp"];
    }

    public static bool IsRJMini = false;
    public int totalMatch;
    [SerializeField] GameObject whtsDashBtn;
    [SerializeField] ScrollSnap scrollSnap;
    public Image leftBtn, rightBtn;
    public Sprite de_spr1, de_spr2;
    public Sprite se_spr1, se_spr2;

    public void RightLeftBunClick(int i)
    {
        switch (i)
        {
            case 0: // left
                scrollSnap.SnapToPrev();
                break;
            case 1: // right
                scrollSnap.SnapToNext();
                break;
        }
        //Logger.Print($"scrollSnap.CurrentIndex { scrollSnap.CurrentIndex } || Max { scrollSnap.CalculateMaxIndex()} ");
        //leftBtn.sprite = scrollSnap.CurrentIndex == 0 ? de_spr1 : se_spr1;
        //rightBtn.sprite = scrollSnap.CurrentIndex == scrollSnap.CalculateMaxIndex() ? de_spr2 : se_spr2;
    }

    IEnumerator ShowWhatsNewImages(int t = 1)
    {
        Logger.Print($"ShowWhatsNewImages called");
        foreach (var wht in updateImg)
            wht.gameObject.SetActive(false);
        foreach (var pag in pagination)
            pag.gameObject.SetActive(false);

        for (int i = 0; i < AppData.UpdateNEwVesrion.Count; i++)
        {
            updateImg[i].gameObject.SetActive(true);
            pagination[i].gameObject.SetActive(true);
            StartCoroutine(AppData.SpriteSetFromURL(AppData.UpdateNEwVesrion[i], updateImg[i], "ShowWhatsNewImages()"));
        }

        yield return new WaitUntil(() => !AppData.IsShowDailyBonus);

        yield return new WaitForSeconds(t);
        //if (!whatsNewScreen.gameObject.activeInHierarchy) whatsNewScreen.gameObject.SetActive(true);
        if (!whatsNewScreen.gameObject.activeInHierarchy) whatsNewScreen.gameObject.SetActive(AppData.UpdateNEwVesrion.Count > 0);
    }

    public void ShowAgainWhatsNewScreenClick()
    {
        StartCoroutine(ShowWhatsNewImages(0));
    }

    public void OnRecevied_SIGNUP(JSONNode data)
    {
        if (AppData.isTutorialPlay)
        {
            Logger.Print($"SIGNUP NOT GO AHADE BCZ TUTORIAL IS ACTIVE");
            return;
        }
            AppData.canShowChallenge = true;
        AllCommonGameDialog.instance.HideConnectionPopup();

        Logger.Print("SP Called as SignUp Event Called...");
        PlayerName.text = data["pn"];
        PlayerLevel.text = "Current Level " + data["level"]["clvl"].AsInt;
        AppData.currantLvl = data["level"]["clvl"].AsInt;

        PlayerGold.text = AppData.numDifferentiation(data["gold"].AsLong);
        PlayerGems.text = AppData.numDifferentiation(data["gems"].AsLong);
        levelSlider.value = data["level"]["per"].AsInt;
        levelPercent.text = data["level"]["per"] + "%";
        levelProgress.text = data["level"]["cp"] + "/" + data["level"]["nlvp"];
        Logger.Print(TAG + "IS Weekly Winner Declared >>> " + data["flags"]["_wwv"].AsInt);
        settingImg.sprite = (data["flags"]["_wwv"].AsInt == 1) ? haveNoti : normal;
        settingBtnCounter.SetActive(data["flags"]["_wwv"].AsInt == 1);
        weeklyWinnerCounter.SetActive(data["flags"]["_wwv"].AsInt == 1);
        isWWAnounced = data["flags"]["_wwv"].AsInt == 1;

        ProfilePanel.instance.ProfilePanelClick(9, false);
        ProfilePanel.instance.ProfilePanelClick(10, false);

        PrefrenceManager.GOLD = data["gold"].Value;
        PrefrenceManager.GEMS = data["gems"].Value;
        PrefrenceManager._ID = data["_id"];
        PrefrenceManager.UNIQUE_ID = data["unique_id"];
        PrefrenceManager.Ch = data["flags"]["_ch"];
        PrefrenceManager.PN = data["pn"];
        PrefrenceManager.FCM_TOKEN = data["fcm_token"];
        PrefrenceManager.PURCHASEADS = data["flags"]["_isads"];

        AppData.IsShowDailyBonus = data["isdailybonus"].AsBool;
        AppData.isRewardAvailable = true;
        AppData.isShowHourlyBonus = (data["HourlyBonus"].AsInt == 1);
        AppData.NormalEntryGold = data["NormalEntryGold"];

        AppData.PlayedMiniGame = data["isfreemini"];


        tournamentBtn.gameObject.SetActive(AppData.configData["ISCUSTOMEMODE"].AsBool);
        customPlayBrn.gameObject.SetActive(AppData.configData["ISTournament"].AsBool);

        //whtsDashBtn.gameObject.SetActive(data["is_newupdate"] == 1l);
        whtsDashBtn.gameObject.SetActive(AppData.UpdateNEwVesrion.Count > 0 && data["is_newupdate"] == 1);

        if (data["Rjmini"] == 1)
        {
            IsRJMini = true;
            AppData.canShowChallenge = false;
            Loading_screen.instance.LoaderPanel.SetActive(true);
            EventHandler.RejoinMiniGame(data["Rjmini"]);
        }

        AppData.IsLogSp = (data["flags"]["_islog"].AsInt == 1); // For Log manege
        AppData.IsLogfileUpload = (data["flags"]["_islogupload"].AsInt == 1);// For Log Upload
        Logger.Print($"_islogupload VVV= {data["flags"]["_islogupload"]} || IsLogSp = {data["flags"]["_islog"].AsInt} ");

        if (AppData.IsLogSp)
        {
            LogServer.instance.DeleteLogFile();
        }

        AppData.chatSuggetions.Clear();

        AppData.chatSuggetions = JsonConvert.DeserializeObject<List<string>>(data["PML"].ToString());

        StartCoroutine(AppData.ProfilePicSet(data["pp"], PlayerImg));
        //StartCoroutine(AppData.ProfilePicSet(data["frameImg"], playerFrame));

        EventHandler.SendHDD();
        Logger.NormalLog($"VVV::: tuid = {data["tuid"]} || tbid = {data["tbid"]} || lgs = {data["lgs"]}");
        Logger.NormalLog($"isnewVersion :: {data["isnewVersion"].AsInt}");

        if (!data["tuid"].Equals(""))
        {
            Loading_screen.instance.ShowLoadingScreen(true);
            EventHandler.RejoinTournament(data["tbid"], data["si"], data["tuid"], data["tsi"]);
            AppData.canShowChallenge = false;
        }
        else if (!data["tbid"].Equals(""))
        {
            Loading_screen.instance.ShowLoadingScreen(true);
            EventHandler.RejoinTable(data["tbid"], data["si"]);
            AppData.canShowChallenge = false;
        }
        else
        {
            GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
            if (!AppData.isTutorialPlay)
                CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
            CommanAnimations.instance.FullScreenPanelAnimation(TournamentPanel.instance.tournamentPanel, false);
            CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel, false);
            Loading_screen.instance.ShowLoadingScreen(false);

            Logger.Print($"{TAG} || purchaseStoreNode = {PrefrenceManager._purchaseNodeData}");

            if (PrefrenceManager._purchaseNodeData != "") // if purchase is failed
            {
                var jsonNode = JSON.Parse(PrefrenceManager._purchaseNodeData);
                Logger.Print($"{TAG} || jsonNode = {jsonNode}");

                if (jsonNode["itemName"] == "gems")
                    EventHandler.HandlePaymentGems(jsonNode);
                else
                    EventHandler.HandlePaymentGold(jsonNode);
                PrefrenceManager._purchaseNodeData = "";
            }

            if (AppData.isShowHourlyBonus && !AppData.IsShowDailyBonus)
            {
                HourlyBonus.instance.ShowHourlyBonus(AppData.hourlyBonusGold, AppData.videoHourlyBonusGold);
            }

            if (data["ulgs"].AsInt != 0)
            {
                if (data["ulgs"].AsInt == 1)
                    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.congratulations, MessageClass.victorieMsg);
                else
                    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.dontGiveUp, MessageClass.loseMsg);
                EventHandler.LgsReqSend();
            }
            if (data["lgs"].AsInt != 0)
            {
                if (data["lgs"].AsInt == 1)
                    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.tournamentWin, "You won your last tournament! Your rewards have been credited to your account. Check them out in your gold history.");
                else
                    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.tournamentLoss, "You lost your last tournament. Better luck next time! The rest of the tournament continues between the winning players.");
                EventHandler.LgsReqSend();
            }

            AppData.isTutorialPlay = (data["is_first"].AsInt == 1);
            AppData.isTutorialFirstTimeReward = (data["is_first"].AsInt == 1);

            AppData.isFirstTimeUSerEnter = (data["is_first"].AsInt == 1);

            if (data["is_first"].AsInt == 1)
            {
                TutorialManager.Instance.HandleNewUserTutorialClick(4);
            }
            else
            {
                if (AppData.isTutorialPlay)
                    if (TutorialManager.Instance.playingTutorialScreen.activeInHierarchy) TutorialManager.Instance.playingTutorialScreen.SetActive(false);

                Logger.NormalLog($"|| || SignUpGot = is_first = {data["is_first"].AsInt} || AppData.FromShowIntro = {AppData.FromShowIntro} ");
                if (!AppData.FromShowIntro && data["is_first"].AsInt != 1 && !isShowWO_OnFirstTime)
                {
                    Logger.Print(TAG + " Is From Splash Screen >> ");
                    AdmobManager.instance.ShowSplashAds();
                }
                Logger.Print(TAG + " Is AppData.IsShowDailyBonus >> " + AppData.IsShowDailyBonus);
                Logger.Print($"{TAG} | isShowWO_OnFirstTime >>  {isShowWO_OnFirstTime} | IsVideoConnectShow = {AppData.IsVideoConnectShow} || {AppData.IsCanShowAd}");
                if (!isShowWO_OnFirstTime)
                {
                    isShowWO_OnFirstTime = true;

                    if (data["is_newupdate"] == 1)
                    {
                        StartCoroutine(ShowWhatsNewImages());
                    }

                    Invoke(nameof(WaitForeSodGet), 0.5f);
                }
            }
            GameManager.instance.ResetPlayerProfile();

            Logger.Print($"{TAG} isnewVersion {data["isnewVersion"]}");

            if (data["isnewVersion"].AsInt == 0)
            {
                //lock remove kari devana
                for (int i = 0; i < tu_modeButtons.Length; i++)
                {
                    var modeBtn = tu_modeButtons[i];
                    modeBtn.click.interactable = true;
                    modeBtn.lockImg.gameObject.SetActive(false);
                }

                PrivateTable.instance.RuleLockRemove();
            }
        }

        // CalculateTotalMatchCount 
        List<int> matchCounts = new List<int>
        {
            data["tournament"]["gp"].AsInt,
            data["CLASSIC"]["gp"].AsInt,
            data["PARTNER"]["gp"].AsInt,
            data["EMOJISOLO"]["gp"].AsInt,
            data["EMOJIPARTNER"]["gp"].AsInt
        };
        AppData.totalPlayedMatch = data["CLASSIC"]["gp"].AsInt;

        totalMatch = CalculateTotalMatchCount(matchCounts);
        Logger.Print($"{TAG} || totalMatch {totalMatch}");

        Logger.Print($"{TAG} || AppData.isVideoClose = {AppData.isVideoClose}");
        if (AppData.isVideoClose)
        {
            AdmobManager.instance.GetRewardOfShowedRewardVideo();
        }
    }

    public static bool isShowWO_OnFirstTime = false;

    private void WaitForeSodGet()
    {
        Logger.Print($" Is AppData.IsShowDailyBonus >> = {PrefrenceManager.GOLD} ");
        if (long.Parse(PrefrenceManager.GOLD) <= 500 && !AppData.IsShowDailyBonus)
        {
            ShowWelcomeOffer();
        }
        if (!AppData.IsShowDailyBonus)
            OfferPopupController.offerBannerSlotCall?.Invoke();

        Logger.Print($" Is Offer Empty ? >> = {(CentralPurchase.FirstTimeOffer == null && CentralPurchase.TimerOffeNew == null && CentralPurchase.StockOfferNew == null)} ");

        if (CentralPurchase.FirstTimeOffer == null && CentralPurchase.TimerOffeNew == null && CentralPurchase.StockOfferNew == null)
        {
            if (long.Parse(PrefrenceManager.GOLD) <= 500)
                WinLossScreen.instance.ShowWatchVideoPopup(true);
        }

    }

    public void DashOfferButtonStatus(bool status)
    {
        Logger.Print($"DashOfferButtonStatus Total match : {totalMatch} | status = {status}");

        if (totalMatch >= 5)
        {
            if (status) // welocme offer hoi
            {
                if (CentralPurchase.FirstTimeOffer == null) return;

                dashOfferBtn[0].gameObject.SetActive(true);
                dashOfferBtn[1].gameObject.SetActive(false);
                dashOfferBtn[2].gameObject.SetActive(false);

                if (dashOfferBtn[0].gameObject.activeInHierarchy)
                {
                    dashOfferBtn[0].dashIpCoinTxt.text = (CentralPurchase.FirstTimeOffer != null) ? CentralPurchase.FirstTimeOffer.gold.ToString() : "";
                    OfferDialog.instance.wO_PriceTxt.text = dashOfferBtn[0].dashIpPriceTxt.text =
                        CentralPurchase.FirstTimeProduct == null ? ("$ " + CentralPurchase.FirstTimeOffer.price) : CentralPurchase.FirstTimeProduct.price;
                }
            }

            else
            {
                dashOfferBtn[0].gameObject.SetActive(false);

                if (totalMatch >= 10)
                {
                    dashOfferBtn[2].gameObject.SetActive(CentralPurchase.StockOfferNew != null);

                    if (CentralPurchase.StockOfferNew != null)
                    {
                        dashOfferBtn[2].gameObject.SetActive(CentralPurchase.StockOfferNew.usestock < CentralPurchase.StockOfferNew.stock);
                        dashOfferBtn[2].dashIpPriceTxt.text = CentralPurchase.StockOfferProduct == null ?
                            ("$ " + CentralPurchase.StockOfferNew.price) : CentralPurchase.StockOfferProduct.price;
                    }

                    dashOfferBtn[1].gameObject.SetActive(CentralPurchase.TimerOffeNew != null);
                    if (CentralPurchase.TimerOfferProduct != null)
                        dashOfferBtn[1].dashIpPriceTxt.text = CentralPurchase.TimerOfferProduct == null ?
                            ("$ " + CentralPurchase.TimerOffeNew.price) : CentralPurchase.TimerOfferProduct.price;
                }
            }
        }
    }

    private int CalculateTotalMatchCount(List<int> matchCounts)
    {
        // Using LINQ to sum the match counts
        return matchCounts.Sum();
    }

    public DashOfferButtonControl[] dashOfferBtn;//0 : 1st time, 2 Stock, 1 Timer

    public void MatchCountShowOffer()
    {
        Logger.Print($"{TAG} || totalMatch {totalMatch} | AppData.promoflag | {AppData.promoflag}");
        if (totalMatch >= 5)
        {
            switch (AppData.promoflag)
            {
                case "welcomeoffer":
                    OfferDialog.instance.SetWelcomeOfferDialogData();
                    break;
                case "timeroffer":
                    if (totalMatch >= 10)
                        StorePanel.Instance.SetLimitedTimerOfferDialogData();
                    break;
                case "stockoffer":
                    if (totalMatch >= 10)
                        StorePanel.Instance.SetLimitedStockOfferDialogData();
                    break;
            }
        }
        else
        {
            if (long.Parse(PrefrenceManager.GOLD) <= 500)
                WinLossScreen.instance.ShowWatchVideoPopup(true);
        }
        AppData.promoflag = "";

        DashOfferButtonStatus((CentralPurchase.FirstTimeOffer != null));
    }

    public void TryShowSlotAndTimerOffer(int clamp)
    {
        // Generate a random number between 0 and 1
        Logger.Print($"{TAG} || chance ");

        StorePanel.Instance.ShowRandomOfferDialog(clamp);
        Logger.Print("Offer dialog shown!");

    }

    public void ShowWelcomeOffer()
    {
        Logger.Print("Welcome offer dialog shown!");
        OfferDialog.instance.SetWelcomeOfferDialogData();
    }

    public void OnRecevied_OFC(JSONNode data)
    {
        buddyHubBtnCounter.SetActive(data["count"].AsInt > 0);
        buddyHubBtnCounterText.text = data["count"].ToString();
        buddyHubImg.sprite = (data["count"].AsInt > 0) ? haveNoti : normal;
        StartCoroutine(BuddiesCounter());
    }

    public void CenterModeClick(int i)
    {
        if (i != 4) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        Logger.Print(TAG + " CenterModeClick " + i);
        int introIndex = -1;

        switch (i)
        {
            case 0://Uno Classic
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.OpenBootValuePanel(-1, AppData.CLASSIC);
                PrivateTable.instance.CustomModeSelect(3);
                break;

            case 1://Uno Team
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.OpenBootValuePanel(-1, AppData.PARTNER);
                break;

            case 2://Uno Emoji
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.OpenBootValuePanel(0, AppData.EMOJI);
                break;

            case 3://Classic Tournament
                introIndex = 3;
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.ListofTournament();
                break;

            case 4://Live Table
                introIndex = 2;
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.ListOfPlayingTable();
                break;

            case 5://Private Table                           
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.OpenBootValuePanel(1, AppData.PRIVATE);
                break;

            case 7:// Customize Table                           
                introIndex = 0;
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.CustomizeBootPanel(AppData.CLASSIC);
                PrivateTable.instance.CustomModeSelect(1);
                PrivateTable.instance.ResetCustomSlot();
                break;

            case 8:// Invert Table                           
                introIndex = 1;
                Loading_screen.instance.ShowLoadingScreen(true);
                PrivateTable.instance.OpenBootValuePanel(-1, AppData.FLIP);
                break;
        }

        UpdateIntroSet(i, introIndex);
    }

    public void UpdateIntroSet(int i, int introIndex)
    {
        if (transParentImg.activeInHierarchy)
        {
            string key = i switch
            {
                7 => "Custom",
                8 => "invert",
                4 => "Live_Table",
                3 => "Tournament",
                _ => ""
            };
            modeoutlines[introIndex].gameObject.SetActive(false);
            GameManager.instance.UpdateTutorial(key, tu_modeButtons[introIndex].gameObject.GetComponent<Canvas>(), tu_modeButtons[introIndex].gameObject.GetComponent<GraphicRaycaster>());
        }
    }

    public void TopRightClick(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        Loading_screen.instance.ShowLoadingScreen(true);
        Logger.Print(TAG + " TopRightClick " + i);
        switch (i)
        {
            case 0://Leaderboard
                EventHandler.LeaderBoard(0);
                break;

            case 1://Buddies
                EventHandler.BuddiesHub(0);
                break;

            case 2://Message
                EventHandler.NotificationData();
                break;

            case 3://Setting
                setting_script.instance.SetSettingPanelData();
                break;

            case 4://profile
                EventHandler.MyProfile();
                break;
        }
    }

    public void BottomClick(int i)
    {
        Logger.Print(TAG + " BottomClick " + i);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (i)
        {
            case 0://Daily-Mission
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.SendDailyMission();
                break;

            case 1://MINI game
                CommanAnimations.instance.FullScreenPanelAnimation(Dashboard_MiniGame.instance.MiniGamePanel, true);
                Dashboard_MiniGame.instance.SetMiniGameData();
                AppData.canShowChallenge = false;
                break;

            case 2://Lucky Spin
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.SendLuckySpin();
                break;

            case 3://Rate Us
                for (int j = 0; j < rateImgs.Count; j++)
                {
                    rateImgs[j].sprite = rateSprites[0];
                }
                CommanAnimations.instance.PopUpAnimation(rateUsPanel, rateUsBg, rateUsPopUp, Vector3.one, true);
                break;

            case 4://Feedback
                setting_script.instance.SettingBtnClick(6);
                break;

            case 5://MoreGame
                string plateform = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) ? "android" : "ios";
                //string plateform = "android";
                EventHandler.SendMoreGame(plateform);
                break;

            case 6:
                AppData.canShowChallenge = false;
                CommanAnimations.instance.PopUpAnimation(moreGamePanel, moreGameBg, moreGamePopUp, Vector3.one, true);
                break;
        }
    }

    public void TopCenterClick(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (i)
        {
            case 0://Watch Video
                AppData.isShownAdsFrom = 4;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 1:
            case 2://GoldStore & Gems Store
                Loading_screen.instance.ShowLoadingScreen(true);
                StorePanel.Instance.OnClick_StorePanel(25);
                EventHandler.GoldStore(i - 1);
                break;

            case 3:
                AppData.isShownAdsFrom = 6;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 4:
                AppData.isShownAdsFrom = 7;
                AdmobManager.instance.ShowRewardedAd();
                break;

        }
    }

    int stars = 0;
    public void RateUsPanelClick(int click)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (click)
        {
            case 0://Close
                CommanAnimations.instance.PopUpAnimation(rateUsPanel, rateUsBg, rateUsPopUp, Vector3.zero, false, false);
                for (int i = 0; i < rateImgs.Count; i++)
                {
                    rateImgs[i].sprite = rateSprites[0];
                }
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                stars = click;
                for (int i = 0; i < rateImgs.Count; i++)
                {
                    rateImgs[i].sprite = rateSprites[stars > i ? i + 1 : 0];
                }
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                break;
            case 6://Feedback
                for (int i = 0; i < rateImgs.Count; i++)
                {
                    rateImgs[i].sprite = rateSprites[0];
                }
                setting_script.instance.SettingBtnClick(6);
                break;
            case 7://Submit
                if (stars == 0) return;

                if (stars < 3)
                {
                    RateUsPanelClick(6);
                }
                else if (stars >= 3)
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

#if UNITY_ANDROID
                    Application.OpenURL("market://details?id=partygame.free.onlinecardgame.uno");
#elif UNITY_IPHONE
                    Application.OpenURL("itms-apps://itunes.apple.com/app/id6487778444");
#endif
                }
                for (int i = 0; i < rateImgs.Count; i++)
                {
                    rateImgs[i].sprite = rateSprites[0];
                }
                break;
        }
    }

    public void MoreGamesPanelClick(int click)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (click)
        {
            case 0://Close
                AppData.canShowChallenge = true;
                CommanAnimations.instance.PopUpAnimation(moreGamePanel, moreGameBg, moreGamePopUp, Vector3.zero, false, false);
                break;
            case 1://GameClick
                break;
        }
    }

    IEnumerator BuddiesCounter()
    {
        yield return new WaitForSeconds(5f);
        if (!AllCommonGameDialog.instance.cLostPanel.activeInHierarchy) EventHandler.OnlineFriendsCounter();
    }

    private void HandleMBV(JSONNode data)
    {
        AppData.TableSloteInGame.Clear();
        AppData.TableSloteInGame = JsonConvert.DeserializeObject<List<TableBootValue>>(data.ToString());
    }
}