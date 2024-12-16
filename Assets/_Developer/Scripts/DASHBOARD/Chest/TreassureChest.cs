using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class TreassureChest : MonoBehaviour
{
    public static string TAG = ">>>TreassureChest ";

    public static TreassureChest instance;

    [Header("ChestToolTip")]
    [SerializeField] GameObject ChestCont;
    [SerializeField] Sprite[] ChestImgTooltip, BtnSprite;

    [SerializeField] List<ChestPrefab> AllChestList = new List<ChestPrefab>();
    [SerializeField] Sprite emptySpr;
    //[Header("Chest Info")]
    //[SerializeField] GameObject ChestInfo;

    [Header("Chest Popup")]
    [SerializeField] GameObject ChestPopup;
    [SerializeField] Image ChestPopupImg, ChestImg, ChestGlow;
    [SerializeField] Transform ChestPopupTrans;
    [SerializeField] Text ChestName, ChestGetGold, ChestGetTile, ChestOpenNowGems, ParallelTxt, UnlockTimer;
    [SerializeField] GameObject ParallelBtnObj, AnotherBeingUnlockObj, VideoIconChestDialog;
    [SerializeField] Sprite[] BtnSpritePopup;
    [SerializeField] GameObject gemOpenBtn, gemOrObj;

    [Header("ChestReward")]
    [SerializeField] GameObject ChestReward;
    [SerializeField] GameObject[] RewardBox, TileObject;
    [SerializeField] GameObject ChestGlowReward,/* ActiveBoosterBtn,SurpriseActiveBoosterBtn,*/  OpenAgainToolTip, ButtonLin, AnimBox, CopyAnimBox, TapToOpen;
    [SerializeField] Image ChestImgReard;
    [SerializeField] Text GemsValueOpenAgain;
    [SerializeField] Sprite[] ChestImgReward, SurpriseRewardImg/*, TicketImg, TicketImgSmall*/;

    [Header("OpenAgain? ")]
    [SerializeField] GameObject chestAgainOpenPopup;
    [SerializeField] Image chestAgainPopupBg;
    [SerializeField] Transform chestgainOpenPopupTrans;
    [SerializeField] Button yes, no;

    public static int ChestCount = 4, LabSlot = 4;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.Onlistner_TCL += OnRecievedTCL;
        SocketManagergame.Onlistner_OTC += HandleOpenChest;
        SocketManagergame.Onlistner_SCR += SurpriseRewardServer;
        SocketManagergame.Onlistner_ATC += OnRecievedTCL;

        //our handler
        SocketManagergame.ChestUnlockVideo += SendChestUnlock;
        SocketManagergame.ChestTimerRemove += SendChestTimeRemove;
        SocketManagergame.HandleSurPriseReward += HandleSurPriseReward;
    }

    private void OnDisable()
    {
        SocketManagergame.Onlistner_TCL -= OnRecievedTCL;
        SocketManagergame.Onlistner_OTC -= HandleOpenChest;
        SocketManagergame.Onlistner_SCR -= SurpriseRewardServer;
        SocketManagergame.Onlistner_ATC -= OnRecievedTCL;

        //our handler
        SocketManagergame.ChestUnlockVideo -= SendChestUnlock;
        SocketManagergame.ChestTimerRemove -= SendChestTimeRemove;
        SocketManagergame.HandleSurPriseReward -= HandleSurPriseReward;
    }

    public List<ChestModel> ChestList = new List<ChestModel>();

    private void OnRecievedTCL(JSONNode data)
    {
        Logger.Print(TAG + " OnRecievedTCL called");

        if (data["data"].HasKey("isfull") && data["data"]["isfull"].AsInt == 1)
            return;

        Loading_screen.instance.LoaderPanel.SetActive(false);
        gemOpenBtn.gameObject.SetActive(true);
        gemOrObj.gameObject.SetActive(true);

        int videoSeatIndex = -1;
        string key = "";
        Logger.Print(TAG + $" OnRecievedTCL ||ChestPopupData called data[en].Value :: {data["en"].Value} | {data["data"].Count}");

        //if(data["data"].Count == 0 )

        switch (data["en"].Value)
        {
            case EventHandler.TreasureChestStopTimer:
                key = data["data"]["data"].ToString();
                break;

            case EventHandler.AddTreasureChest:
                key = data["data"]["chest"].ToString();
                break;

            default:
                key = data["data"].ToString();
                break;
        }

        ChestList = JsonConvert.DeserializeObject<List<ChestModel>>(key);

        switch (data["en"].Value)
        {
            case EventHandler.TreasureChestStopTimer:
                AudioManager.instance.AudioPlay(AudioManager.instance.MissionTimer);
                if (ChestPopup.activeInHierarchy)
                {
                    ChestOpenNowGems.text = GetGems(currentOpenChest) + "";
                    videoSeatIndex = data["data"]["index"].AsInt;
                    ParallelTxt.text = ChestList[currentOpenChest].lefttime > 5400 ? "Skip 1.5 Hr" : "Open Now";
                    ParallelBtnObj.transform.GetComponent<Image>().sprite = BtnSpritePopup[ChestList[currentOpenChest].lefttime > 5400 ? 2 : 1];
                }
                break;

            case EventHandler.UnlockTreasureChest:
                AudioManager.instance.AudioPlay(AudioManager.instance.ChestTimeComplete);
                //button animation show karvanu
                VideoIconChestDialog.SetActive(true);
                ParallelTxt.text = "Skip 1.5 Hr";

                if (ChestPopup.activeInHierarchy)
                {
                    ChestImg.transform.DOKill(true);
                    ChestImg.transform.eulerAngles = Vector3.zero;
                    ChestGlow.transform.DOKill(false);
                    ChestOpenNowGems.text = ChestList[currentOpenChest].lefttime == 0 ? ChestList[currentOpenChest].gems + "" : GetGems(currentOpenChest) + "";
                }
                break;

            case EventHandler.OPENCHESTSLOTE:
                if (ChestList[currentOpenChest].unlock == 1 && ChestList[currentOpenChest].isopen == 1 && ChestPopup.activeSelf)
                {
                    UnlockTimer.gameObject.SetActive(false);

                    //ChestList[currentOpenChest]
                    gemOrObj.gameObject.SetActive(false);
                    gemOpenBtn.gameObject.SetActive(false);
                    ChestImg.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 10), 1, 10, true).SetLoops(-1, LoopType.Yoyo);
                    ChestGlow.transform.DORotate(new Vector3(0f, 0f, -360f), 7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                }
                break;
        }

        Logger.Print(TAG + " VideoSeatIndex " + videoSeatIndex);
        if (videoSeatIndex != -1)
        {
            for (int i = 0; i < ChestList.Count; i++)
            {
                if (i == videoSeatIndex && ChestList[i].lefttime <= 0 && ChestList[i].isopen == 1)
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    EventHandler.OpenChestData(i, 0, 0);
                    break;
                }
            }
        }

        for (int i = 0; i < AllChestList.Count; i++)
        {
            ChestPrefab ChestItem = AllChestList[i];
            ChestItem.openAgainBtn.gameObject.SetActive(false);
        }

        for (int i = 0; i < ChestCount; i++)
        {
            ChestPrefab ChestItem = AllChestList[i];
            ChestItem.index = i;
            Logger.Print(TAG + $" Child Count {ChestItem.transform.childCount} Count {ChestList.Count} | index: {ChestItem.index} | i : {i}");

            ChestItem.EmptyTxtObj.SetActive(i >= ChestList.Count);
            ChestItem.UnlockBtn.interactable = i < ChestList.Count;
            ChestItem.unlockSecBtn.interactable = i < ChestList.Count;
            ChestItem.TimerIcon.SetActive(i < ChestList.Count);
            ChestItem.ChestTimer.gameObject.SetActive(i < ChestList.Count);
            ChestItem.TapToUnlockTooltip.SetActive(i < ChestList.Count);
            ChestItem.TapToUnlockBtnImg.gameObject.SetActive(i < ChestList.Count);
            //ChestItem.ChestDataObj.SetActive(i < ChestList.Count);
            if (i >= ChestList.Count)
            {
                ChestItem.ChestImg.sprite = emptySpr;
            }

            ChestItem.UnlockBtn.onClick.RemoveAllListeners();
            ChestItem.UnlockBtn.onClick.AddListener(() =>
            {
                HandleChestUnlockButtonClick(ChestItem.index);
            });

            ChestItem.unlockSecBtn.onClick.RemoveAllListeners();
            ChestItem.unlockSecBtn.onClick.AddListener(() =>
            {
                HandleChestUnlockButtonClick(ChestItem.index);
            });


            ChestItem.SkipTime.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            ChestItem.SkipTime.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                ChestPopupData(ChestItem.index);
            });
        }

        SetChestData(data["en"].Value.Equals(EventHandler.AddTreasureChest));
    }

    void HandleChestUnlockButtonClick(int chestIndex)
    {
        Logger.Print($"chestIndex:: {chestIndex} | count {ChestList.Count}");
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        // Check if the chest is unlocked
        if (ChestList[chestIndex].isopen == 1)
        {
            Logger.Print($" count {ChestList.Count}");
            Logger.Print($"ChestList[chestIndex].isopen:: {ChestList[chestIndex].isopen} | count {ChestList.Count}");
            Loading_screen.instance.LoaderPanel.SetActive(true);
            EventHandler.OpenChestData(chestIndex, 0, ChestList[chestIndex].isagainopen);
        }
        else
        {
            ChestPopupData(chestIndex);
        }
    }

    private void HandleSurPriseReward()
    {
        AppData.fromChestSurprize = false;
        EventHandler.SurprizeChestReward(chestName, currentOpenChest, isAgainOpen == 1);
    }

    int RequireGemsToOpenAgain = 0, isAgainOpen = 0, HowManyRewardOpen = 0;
    List<ChestReward> AllChestReward = new List<ChestReward>();
    string chestName = "";

    private void HandleOpenChest(JSONNode data)
    {
        Loading_screen.instance.LoaderPanel.SetActive(false);
        Logger.Print(TAG + $" HandleOpenChest called :  {data["reward"].Count} || + Length : {TileObject.Length}");

        CommanAnimations.instance.PopUpAnimation(ChestPopup, ChestPopupImg, ChestPopupTrans, Vector3.zero, false, false);
        CommanAnimations.instance.PanelPopUpOpenAntimation(ChestReward);

        for (int i = 0; i < RewardBox.Length; i++)
        {
            RewardBox[i].SetActive(false);
            RewardBox[i].transform.localScale = Vector3.zero;
        }

        OpenReward = true;
        TapToOpen.SetActive(true);
        ButtonLin.SetActive(false);
        OpenAgainToolTip.SetActive(false);
        //ButtonLin.transform.GetChild(1).gameObject.SetActive(false);

        ChestGlowReward.transform.DORotate(new Vector3(0f, 0f, -360f), 7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

        ChestImgReard.sprite = GetSpriteCHest(data["chest"]["chestname"].Value, 0);
        RequireGemsToOpenAgain = data["chest"]["againopen"].AsInt;

        currentOpenChest = data["index"].AsInt;
        isAgainOpen = data["isagain"].AsInt;
        chestName = data["chest"]["chestname"];

        isSurpriseRewardAvailable = false;
        HowManyRewardOpen = 0;
        AllChestReward.Clear();


        RewardBox[4].transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite("help", "");
        RewardBox[4].transform.GetChild(2).gameObject.SetActive(true);

        GemsValueOpenAgain.text = AppData.numDifferentiation(RequireGemsToOpenAgain);

        for (int i = 0; i < TileObject.Length; i++)
        {
            TileObject[i].SetActive((i + 1) <= data["reward"].Count);
        }

        AllChestReward = JsonConvert.DeserializeObject<List<ChestReward>>(data["reward"].ToString());
        Logger.Print(TAG + " AllChestReward Count " + AllChestReward.Count);

        for (int i = 0; i < data["reward"].Count; i++)
        {
            TileObject[i + 1].SetActive(true);
            RewardBox[data["reward"][i]["index"]].SetActive(true);
            RewardBox[data["reward"][i]["index"]].transform.GetChild(1).transform.GetChild(0).transform.GetComponent<Text>().text = AppData.numDifferentiation(data["reward"][i]["gold"]);

            switch (data["reward"][i]["type"].Value)
            {
                case "gold":
                    break;

                case "gems":

                    RewardBox[data["reward"][i]["index"]].transform.GetChild(2).gameObject.SetActive(long.Parse(PrefrenceManager.GEMS) >= 7);

                    if (long.Parse(PrefrenceManager.GEMS) >= 7) //long.Parse(PrefrenceManager.GEMS) 
                    {
                        RewardBox[data["reward"][i]["index"]].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            EventHandler.TreasureChestRemove(currentOpenChest);
                            CommanAnimations.instance.PanelPopUpCloseAntimation(ChestReward);

                            Loading_screen.instance.LoaderPanel.SetActive(true);
                            EventHandler.StartMiniGame(false);
                        });
                    }
                    break;

                case "leaf":
                    //
                    RewardBox[data["reward"][i]["index"].AsInt].transform.GetChild(2).gameObject.SetActive(long.Parse(PrefrenceManager.LEAF) >= 10);

                    if (long.Parse(PrefrenceManager.LEAF) >= 10)
                    {
                        RewardBox[data["reward"][i]["index"]].transform.GetChild(2).GetChild(0).transform.GetComponent<Text>().text = "Spin";
                        RewardBox[data["reward"][i]["index"]].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            EventHandler.TreasureChestRemove(currentOpenChest);
                            CommanAnimations.instance.PanelPopUpCloseAntimation(ChestReward);

                            Loading_screen.instance.ShowLoadingScreen(true);
                            EventHandler.SendLuckySpin();
                        });
                    }

                    break;


            }
        }

        isSurpriseRewardAvailable = !isSurpriseGot;
        Logger.Print($"isSurpriseRewardAvailable  : {isSurpriseRewardAvailable} || isSurpriseGot: {isSurpriseGot}");

        TileObject[0].SetActive(isSurpriseRewardAvailable);
        RewardBox[4].SetActive(isSurpriseRewardAvailable);

        ButtonLin.transform.GetChild(1).gameObject.SetActive(isAgainOpen != 1);
        ButtonLin.transform.GetChild(2).gameObject.SetActive(isAgainOpen != 1);
    }

    bool isSurpriseGot = false, isSurpriseRewardAvailable = false;
    string couponName = "";
    private void SurpriseRewardServer(JSONNode data)
    {
        Logger.Print(TAG + $" SurpriseRewardServer called : Value :: {data["surprisereward"]["type"].Value}");

        isSurpriseGot = true;

        switch (data["surprisereward"]["type"].Value)
        {
            case "xp":
            case "gold":
            case "gems":
            case "leaf":
                RewardBox[4].transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite(data["surprisereward"]["type"], "");
                RewardBox[4].transform.GetChild(2).transform.gameObject.SetActive(false);
                RewardBox[4].transform.GetChild(1).transform.GetComponentInChildren<Text>().text = AppData.numDifferentiation(data["surprisereward"]["gold"].AsLong);
                break;

            default:
                RewardBox[4].transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite(data["surprisereward"]["type"], "");
                RewardBox[4].transform.GetChild(2).transform.gameObject.SetActive(false);
                RewardBox[4].transform.GetChild(1).transform.GetComponentInChildren<Text>().text = AppData.numDifferentiation(data["surprisereward"]["gold"].AsLong);
                break;
        }

        RewardBox[4].transform.GetChild(0).transform.GetComponent<Image>().SetNativeSize();
    }

    private Sprite GetSurpriseSprite(string name, string color)
    {
        switch (name)
        {
            case "gold":
                return SurpriseRewardImg[0];

            case "gems":
                return SurpriseRewardImg[1];

            case "xp":
                return SurpriseRewardImg[2];

            case "leaf":
                return SurpriseRewardImg[3];

            case "help":
                return SurpriseRewardImg[4];

            default:
                return SurpriseRewardImg[4];
        }
    }

    bool OpenReward = false;
    private void ChestRewardOpenAnimation()
    {
        Logger.Print(TAG + " ChestRewardOpenAnimation " + AllChestReward[HowManyRewardOpen].type + " Index " + HowManyRewardOpen + " Count " + AllChestReward.Count);
        AnimBox.SetActive(true);
        OpenReward = false;

        switch (AllChestReward[HowManyRewardOpen].type)
        {
            case "coupan":
                AnimBox.transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite(AllChestReward[HowManyRewardOpen].type, AllChestReward[HowManyRewardOpen].name);
                break;

            case "stone":
                AnimBox.transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite(AllChestReward[HowManyRewardOpen].type, AllChestReward[HowManyRewardOpen].color);
                break;

            default:
                AnimBox.transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite(AllChestReward[HowManyRewardOpen].type, "");
                break;
        }

        AnimBox.transform.localScale = Vector3.zero;
        Sequence s = DOTween.Sequence();
        s.Append(AnimBox.transform.DOMove(RewardBox[AllChestReward[HowManyRewardOpen].index].transform.position, 0.3f).SetEase(Ease.Linear));
        s.Insert(0, AnimBox.transform.DOScale(1f, 0.3f));

        s.OnStart(() =>
        {
            int temp = AllChestReward.Count - HowManyRewardOpen;
            TileObject[AllChestReward.Count - HowManyRewardOpen].transform.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() =>
            {
                TileObject[temp].SetActive(false);
                TileObject[temp].transform.GetComponent<CanvasGroup>().DOFade(1f, 0f);
            });

            AudioManager.instance.AudioPlay(AudioManager.instance.ItemOpenChest);
            ChestImgReard.sprite = GetSpriteCHest(chestName, HowManyRewardOpen + 1 == AllChestReward.Count ? 2 : 1);
            AnimBox.transform.GetChild(1).transform.GetComponentInChildren<Text>().text = AppData.numDifferentiation(AllChestReward[HowManyRewardOpen].gold);
        }).OnComplete(() =>
        {
            RewardBox[AllChestReward[HowManyRewardOpen].index].SetActive(true);
            RewardBox[AllChestReward[HowManyRewardOpen].index].transform.localScale = Vector3.one;
            HowManyRewardOpen++;

            ChestImgReard.sprite = GetSpriteCHest(chestName, HowManyRewardOpen == AllChestReward.Count ? 2 : 0);

            AnimBox.SetActive(false);
            AnimBox.transform.position = CopyAnimBox.transform.position;
            Logger.Print(TAG + " Animation Complete " + AllChestReward.Count + " Open " + HowManyRewardOpen);

            if (AllChestReward.Count == HowManyRewardOpen)
            {
                Logger.Print(TAG + $" All Box mali gya isSurpriseRewardAvailable : {isSurpriseRewardAvailable} | isSurpriseGot: {isSurpriseGot}");
                if (isSurpriseRewardAvailable)
                {
                    AnimBox.transform.localScale = Vector3.zero;
                    Sequence s = DOTween.Sequence();

                    s.Append(AnimBox.transform.DOMove(RewardBox[4].transform.position, 0.3f).SetEase(Ease.Linear).SetDelay(0.4f).OnStart(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ItemOpenChest);
                    }));
                    s.Insert(0, AnimBox.transform.DOScale(1f, 0.3f).SetDelay(0.4f));

                    s.OnStart(() =>
                    {
                        TileObject[0].transform.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() =>
                        {
                            TileObject[0].SetActive(false);
                            TileObject[0].transform.GetComponent<CanvasGroup>().DOFade(1f, 0f);
                        });

                        AnimBox.SetActive(true);
                        AnimBox.transform.GetChild(0).transform.GetComponent<Image>().sprite = GetSurpriseSprite("help", "");

                        OpenAgainToolTip.transform.localScale = Vector3.zero;

                    }).OnComplete(() =>
                    {
                        RewardBox[4].SetActive(true);
                        RewardBox[4].transform.localScale = Vector3.one;

                        AnimBox.SetActive(false);
                        AnimBox.transform.position = CopyAnimBox.transform.position;

                        ButtonLin.SetActive(true);
                        TapToOpen.SetActive(false);

                        if (isAgainOpen != 1)
                            OpenAgainToolTip.transform.DOScale(1f, 0.5f).OnStart(() =>
                            {
                                OpenAgainToolTip.SetActive(true);
                                AudioManager.instance.AudioPlay(AudioManager.instance.PopupClip);
                            });
                    });
                }
                else
                {
                    ButtonLin.SetActive(true);
                    TapToOpen.SetActive(false);
                }
            }
            OpenReward = true;
        });
    }

    public void RewardDialogClick(int i)
    {
        Logger.Print(TAG + " RewardDialogClick called " + i);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        switch (i)
        {
            case 2:
                Loading_screen.instance.LoaderPanel.SetActive(true);
                AppData.fromChestSurprize = true;
                AdmobManager.instance.ShowRewardedAd();

                break;

            case 3://collect remove

                if (AppData.istreassureFeed && currentOpenChest == 1)
                {
                    AppData.istreassureFeed = false;
                    setting_script.instance.FeedbackPopupShow(true); // chest collect
                }
                EventHandler.TreasureChestRemove(currentOpenChest);
                CommanAnimations.instance.PanelPopUpCloseAntimation(ChestReward);
                isSurpriseGot = false;
                break;

            case 4://open again reward
                if (RequireGemsToOpenAgain > long.Parse(PrefrenceManager.GEMS))
                {
                    OfferPopupController.offerSlotCall?.Invoke(true, RequireGemsToOpenAgain, RequireGemsToOpenAgain);
                    return;
                }
                Loading_screen.instance.LoaderPanel.SetActive(true);
                EventHandler.OpenChestData(currentOpenChest, RequireGemsToOpenAgain, 1);
                break;

            case 5://tap to continue
                Logger.Print($"tap to continue :{AllChestReward.Count == HowManyRewardOpen} | {AllChestReward.Count == HowManyRewardOpen || !OpenReward}");
                if (AllChestReward.Count == HowManyRewardOpen || !OpenReward)
                    return;

                ChestRewardOpenAnimation();
                break;
        }
    }

    private Sprite GetSpriteCHest(string name, int which)//0=Close, 1=OpenShining, 2=OpenClose
    {
        switch (name)
        {
            case "gold":
                return ChestImgReward[which == 0 ? 2 : (which == 1 ? 5 : 8)];

            case "silver":
                return ChestImgReward[which == 0 ? 1 : (which == 1 ? 4 : 7)];

            default:
                return ChestImgReward[which == 0 ? 0 : (which == 1 ? 3 : 6)];
        }
    }

    private void SendChestTimeRemove()
    {
        AppData.fromChest = false;
        EventHandler.ChestTimerRemove(currentOpenChest);
    }

    private void SendChestUnlock()
    {
        AppData.fromChestUnlock = false;
        EventHandler.UnlockChest(ChestUnlockIndex);
    }

    private string GetChestNameCap(string name)
    {
        switch (name)
        {
            case "bronze":
                return "Bronze Chest";

            case "silver":
                return "Silver Chest";

            default:
                return "Gold Chest";
        }
    }

    int currentOpenChest = -1, TotalChestUnlock = 0, ChestUnlockIndex = -1;

    private void ChestPopupData(int i)
    {
        Logger.Print(TAG + " ChestPopupData called " + i);
        currentOpenChest = i;
        ParallelBtnObj.SetActive(true);
        AnotherBeingUnlockObj.SetActive(false);

        CommanAnimations.instance.PopUpAnimation(ChestPopup, ChestPopupImg, ChestPopupTrans, Vector3.one, true, true);

        Logger.Print(TAG + $" ChestPopupData called ChestName :: {ChestList.Count}");
        ChestName.text = GetChestNameCap(ChestList[i].chestname);
        ChestGetGold.text = "Up to " + AppData.numDifferentiation(ChestList[i].rw.gold) + " Gold";
        ChestGetTile.text = "x " + ChestList[i].rw.min + "-" + ChestList[i].rw.max + " Pieces";

        UnlockTimer.gameObject.SetActive(ChestList[i].lefttime > 0);
        UnlockTimer.text = "Unlocking Takes : " + StaticData.GetTimeInFormateHr((long)ChestList[i].lefttime * 1000);
        ChestOpenNowGems.text = ChestList[i].lefttime == 0 ? ChestList[i].gems + "" : GetGems(i) + "";
        ChestImg.sprite = GetChestSprite(ChestList[i].chestname);

        VideoIconChestDialog.SetActive(ChestList[i].lefttime > 0);
        ParallelTxt.text = ChestList[i].lefttime > 5400 ? "Skip 1.5 Hr" : "Open Now";
        ParallelBtnObj.transform.GetComponent<Image>().sprite = BtnSpritePopup[ChestList[currentOpenChest].lefttime < 5400 ? 1 : 2];

        if (ChestList[i].isopen != 1 && ChestList[i].lefttime == 0)
        {
            if (TotalChestUnlock == 1)
            {
                VideoIconChestDialog.SetActive(true);
                ParallelTxt.text = "Watch Video to Unlock";
            }

            else if (TotalChestUnlock > 1)
            {
                AnotherBeingUnlockObj.SetActive(true);
                ParallelBtnObj.SetActive(false);
            }
            else
                ParallelTxt.text = "Start Unlock";

            ParallelBtnObj.transform.GetComponent<Image>().sprite = BtnSpritePopup[2];
        }

        if (ChestList[i].lefttime == 0 && ChestList[i].unlock == 0)
        {
            ChestImg.transform.DOKill(true);
            ChestGlow.transform.DOKill(true);
            ChestImg.transform.eulerAngles = Vector3.zero;
            ChestImg.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 10), 1, 10, true).SetLoops(-1, LoopType.Yoyo);
            ChestGlow.transform.DORotate(new Vector3(0f, 0f, -360f), 7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
        else
        {
            ChestImg.transform.DOKill(true);
            ChestGlow.transform.DOKill(true);
            ChestImg.transform.eulerAngles = Vector3.zero;
        }
        Logger.Print(TAG + $" ChestPopupData called ChestName ||||||||? :: {ChestList[i].chestname}");
    }

    private Sprite GetChestSprite(string name)
    {
        switch (name)
        {
            case "silver":
                return ChestImgTooltip[1];

            case "gold":
                return ChestImgTooltip[2];

            default:
                return ChestImgTooltip[0];
        }
    }

    private int GetGems(int index)
    {
        for (int i = 1; i < ChestList[index].pay.Count; i++)
        {
            if (ChestList[index].pay[i].s < ChestList[index].lefttime)
                return ChestList[index].pay[i - 1].gems;
        }
        return ChestList[index].gems;
    }

    private void SetChestData(bool isFromATC)
    {
        Logger.Print(TAG + $" SetChestData called ChestList: {ChestList.Count}");
        TotalChestUnlock = 0;

        for (int i = 0; i < ChestList.Count; i++)
        {
            if (ChestList[i].lefttime > 0)
                TotalChestUnlock++;

            ChestPrefab ChestItem = AllChestList[i];

            ChestItem.ChestImg.sprite = ChestImgTooltip[GetChestIndex(ChestList[i].chestname)];
            ChestItem.openAgainBtn.gameObject.SetActive(false);

            if (ChestList[i].unlock == 0)//1 = timer start, 0 = timer start karvano baki
            {
                Logger.Print($" AA");
                //button and Skip Pattie
                ChestItem.TapToUnlockBtnImg.gameObject.SetActive(true);
                ChestItem.TapToUnlockBtnImg.sprite = BtnSprite[0];
                ChestItem.BtnTxt.text = "Unlock";
                ChestItem.SkipHrTime.SetActive(false);

                //chest timer
                ChestItem.TimerIcon.SetActive(true);
                ChestItem.ChestTimer.text = StaticData.GetTimeInFormateHr(ChestList[i].time * 1000);

                //green tap to tooltip
                ChestItem.TapToUnlockTooltip.SetActive(true);
                ChestItem.unlockSecBtn.interactable = (true);
            }

            else if (ChestList[i].isopen != 1)//0= Timer chalu che, 1= Timer complete thai gyo che
            {
                Logger.Print($" BB");
                ChestItem.TapToUnlockBtnImg.gameObject.SetActive(false);
                ChestItem.SkipHrTime.SetActive(true);

                //chest timer
                ChestItem.TimerIcon.SetActive(true);
                ChestItem.ChestTimer.text = StaticData.GetTimeInFormateHr((long)ChestList[i].lefttime * 1000);

                //green tap to tooltip
                ChestItem.TapToUnlockTooltip.SetActive(false);
            }

            else if (ChestList[i].iscalim == 0)//claim = 0 kai nai, claim = 1 Open Again baki
            {
                Logger.Print($" CC");
                //button and Skip Pattie
                ChestItem.TapToUnlockBtnImg.gameObject.SetActive(true);
                ChestItem.TapToUnlockBtnImg.sprite = BtnSprite[1];
                ChestItem.BtnTxt.text = "Open Now";
                ChestItem.SkipHrTime.SetActive(false);

                //chest timer
                ChestItem.TimerIcon.SetActive(false);
                ChestItem.ChestTimer.text = "Ready";

                //Tap to unlock Green pattie
                ChestItem.TapToUnlockTooltip.SetActive(true);
                ChestItem.unlockSecBtn.interactable = (true);
            }

            else//Again open baki
            {
                //button and Skip Pattie
                Logger.Print($" DD {i}");

                ChestItem.TapToUnlockBtnImg.gameObject.SetActive(true);
                ChestItem.TapToUnlockBtnImg.sprite = BtnSprite[1];
                ChestItem.BtnTxt.text = "Open Again";
                ChestItem.SkipHrTime.SetActive(false);
                //ChestItem.index

                ChestItem.unlockSecBtn.onClick.RemoveAllListeners();
                ChestItem.unlockSecBtn.onClick.AddListener(() => OpenAgainBtn());

                Logger.Print($" Check Again : {i} ||  {ChestItem.index}");

                no.onClick.RemoveAllListeners();
                no.onClick.AddListener(() => DisebleAskPopup(ChestItem.openAgainBtn, ChestItem.unlockSecBtn, ChestItem.index));

                yes.onClick.RemoveAllListeners();
                yes.onClick.AddListener(() => AgainYesClick(ChestItem.index, ChestList[ChestItem.index].gems));

                if (ChestList.Count > 0)
                    ChestItem.openAgainBtn.gameObject.SetActive(true);
                //chest timer
                ChestItem.TimerIcon.SetActive(false);
                ChestItem.ChestTimer.text = "Ready";

                //Tap to unlock Green pattie
                ChestItem.TapToUnlockTooltip.SetActive(false);
                ChestItem.unlockSecBtn.interactable = (true);
            }

            if (i == ChestList.Count - 1 && isFromATC)
            {
                ChestItem.transform.DOScale(0, 0);
                ChestItem.transform.DOScale(1f, 0.3f).SetDelay(0.4f);
            }
        }
    }

    void DisebleAskPopup(Button openBtn, Button unlockSecBtn, int index)
    {
        openBtn.gameObject.SetActive(false);
        unlockSecBtn.onClick.RemoveAllListeners();
        CommanAnimations.instance.PopUpAnimation(chestAgainOpenPopup, chestAgainPopupBg, chestgainOpenPopupTrans, Vector3.zero, false, false);

        EventHandler.TreasureChestRemove(index);
        CommanAnimations.instance.PanelPopUpCloseAntimation(ChestReward);
        isSurpriseGot = false;
    }

    void AgainYesClick(int chestIndex, int gems)
    {
        if (gems > long.Parse(PrefrenceManager.GEMS))
        {
            OfferPopupController.offerSlotCall?.Invoke(true, gems, RequireGemsToOpenAgain);
            return;
        }
        Loading_screen.instance.LoaderPanel.SetActive(true);
        EventHandler.OpenChestData(chestIndex, gems, 1);
        CommanAnimations.instance.PopUpAnimation(chestAgainOpenPopup, chestAgainPopupBg, chestgainOpenPopupTrans, Vector3.zero, false, false);
    }

    public void OpenAgainBtn()
    {
        CommanAnimations.instance.PopUpAnimation(chestAgainOpenPopup, chestAgainPopupBg, chestgainOpenPopupTrans, Vector3.one, true, true);

    }

    private int GetChestIndex(string name)
    {
        switch (name)
        {
            case "gold":
                return 2;

            case "silver":
                return 1;

            default:
                return 0;
        }
    }

    public void ChestBtnClick(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        switch (i)
        {

            case 4://Chest Popup Close
                ChestImg.transform.DOKill(true);
                ChestGlow.transform.DOKill(true);
                ChestImg.transform.eulerAngles = Vector3.zero;
                CommanAnimations.instance.PopUpAnimation(ChestPopup, ChestPopupImg, ChestPopupTrans, Vector3.zero, false, false);
                break;

            case 5://chest dialog Start Timer && Skip Timer
                if (ChestList[currentOpenChest].unlock == 0 && TotalChestUnlock == 0)
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    EventHandler.UnlockChest(currentOpenChest);
                }
                else if (ChestList[currentOpenChest].unlock == 0 && TotalChestUnlock == 1)
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    AppData.fromChestUnlock = true;
                    ChestUnlockIndex = currentOpenChest;
                    AdmobManager.instance.ShowRewardedAd();
                }
                else if (ChestList[currentOpenChest].isopen != 1)
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    AppData.fromChest = true;
                    AdmobManager.instance.ShowRewardedAd();
                }
                else
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    EventHandler.OpenChestData(currentOpenChest, 0, 0);
                }
                break;

            case 6://chest dialog Open Timer
                if (ChestList[currentOpenChest].lefttime > 0 && GetGems(currentOpenChest) > long.Parse(PrefrenceManager.GEMS) ||
                    ChestList[currentOpenChest].lefttime == 0 && ChestList[currentOpenChest].gems > long.Parse(PrefrenceManager.GEMS))
                {

                    AllCommonGameDialog.instance.ShowNotEnoughDialog(true, 0, GetGems(currentOpenChest), false);
                }
                else
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    EventHandler.OpenChestData(currentOpenChest, GetGems(currentOpenChest), 0);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ChestCont.transform.childCount != 0 && ChestList.Count != 0 && AllChestList.Count != 0)
        {
            for (int i = 0; i < ChestList.Count; i++)
            {
                if (ChestList[i].lefttime > 0)
                {
                    ChestPrefab ChestItem = AllChestList[i];

                    ChestList[i].lefttime -= Time.deltaTime;
                    ChestItem.TimerIcon.SetActive(true);
                    ChestItem.ChestTimer.text = StaticData.GetTimeInFormateHr((long)ChestList[i].lefttime * 1000);

                    if (currentOpenChest == i)
                    {
                        UnlockTimer.gameObject.SetActive(true);
                        UnlockTimer.text = "Unlocking Takes : " + StaticData.GetTimeInFormateHr((long)ChestList[i].lefttime * 1000);
                    }
                }
            }
        }
    }
}
