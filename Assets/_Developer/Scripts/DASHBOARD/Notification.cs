using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : CentralPurchase
{
    private static string TAG = " >>> NOTI >> ";

    public static Notification Instance;

    public RectTransform NotiPannel;
    public GameObject NotiPrefab;
    [SerializeField] Transform NotiContent;
    public Sprite[] Tap;
    public Image[] TapBack;

    [SerializeField] Sprite bigBtnNormal, bigBtnGold, bigBtnGems, actionBtnEnable, actionBtnDisable, actionBtnRed;
    public GameObject notiCounter, messageCounter, specialOfferCounter;
    public TextMeshProUGUI notiCounterText, messageCounterText, specialOfferCounterText;
    [SerializeField] TextMeshProUGUI noteText;

    [SerializeField] Sprite notiIcon;

    int ncCount, uRMCCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_ND += HandleNotificationData;
        SocketManagergame.OnListner_NC += HandleNotificationCount;
        SocketManagergame.OnListner_IS += HandleMessageTab;
        SocketManagergame.OnListner_URMC += HandleUnreadMessageCounter;
        SocketManagergame.OnListner_SUSG += HandleSosu;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_ND -= HandleNotificationData;
        SocketManagergame.OnListner_NC -= HandleNotificationCount;
        SocketManagergame.OnListner_IS -= HandleMessageTab;
        SocketManagergame.OnListner_URMC -= HandleUnreadMessageCounter;
        SocketManagergame.OnListner_SUSG -= HandleSosu;
    }

    #region NOTIFICATION <<====================
    private void HandleNotificationData(JSONNode data)
    {
        Logger.Print(TAG + " HandleNotificationData called ");

        List<NotiModel> model = JsonConvert.DeserializeObject<List<NotiModel>>(data.ToString());
        noteText.gameObject.SetActive(model.Count == 0);
        noteText.text = "No Notifications";
        SetNotificationData(model);
    }

    List<GameObject> friendInvitr = new List<GameObject>();

    public void DeleteNotiReqFriend()
    {
        for (int i = 0; i < friendInvitr.Count; i++)
        {
            Destroy(friendInvitr[i].gameObject);
        }
        friendInvitr.Clear();
    }

    private void SetNotificationData(List<NotiModel> data)
    {
        if (SocketManagergame.ISDEADLOCKACCURE) return;

        if (NotiContent.childCount > data.Count)
        {
            for (int i = NotiContent.childCount - 1; i >= data.Count; i--)
            {
                Destroy(NotiContent.GetChild(i).gameObject);
            }
        }

        friendInvitr.Clear();

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentNoti;
            int tempIndex = i;
            if (NotiContent.childCount < i + 1)
            {
                currentNoti = Instantiate(NotiPrefab, NotiContent);
            }
            else
            {
                currentNoti = NotiContent.GetChild(i).gameObject;
            }

            switch (data[i].t)
            {
                case "Daily Bonus":
                case "Hourly Bonus":
                case "Magic Bonus":
                case "levelUP":
                    //Left Side View...
                    currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);//PlayerImg
                    currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);//Normal Sprite
                    StartCoroutine(AppData.SpriteSetFromURL(data[i].pp, currentNoti.transform.GetChild(0).GetChild(1).GetComponent<Image>(), "SetNotificationData-LevelUp"));//Sprite Set...

                    //Center View...
                    currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data[i].title; //Heading
                    currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].msg; //Normal Note Text
                    currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Normal Note Text
                    currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); // Boot Value Text                    
                    currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);//Slider Parent

                    //Right Side View...
                    currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(false); //Action Buttons
                    currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(true); //Big Btn
                    currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = bigBtnNormal; //Big Btn Sprite
                    currentNoti.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "GET " + AppData.numDifferentiation(data[i].gold) + " GOLD"; //Earn Gold Text
                    currentNoti.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true); //Watch Video
                    currentNoti.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(false); //Buy Price

                    currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners(); //Big Btn Listener Remove
                    currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (AppData.IsCanShowAd) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        AppData.notiRewardType = data[tempIndex].t;
                        AppData.notiRewardVal = data[tempIndex].gold;
                        AppData.notiId = data[tempIndex]._id;
                        AppData.isShownAdsFrom = 0;
                        AdmobManager.instance.ShowRewardedAd();
                        //Daily Bonus Event Handle...
                    }); //Big Btn Listner Add
                    break;

                case "Invite to Friend Bonus":
                case "friend_res":
                    //Left Side View...
                    currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);//Normal Sprite
                    currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);//PlayerImg
                    StartCoroutine(AppData.ProfilePicSet(data[i].pp, currentNoti.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
                    //StartCoroutine(AppData.SpriteSetFromURL(data[i].frameImg, currentNoti.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>(), "SetNotificationData-friend_res"));

                    //Center View...
                    currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data[i].title; //Heading
                    currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].msg; //Normal Note Text
                    currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Normal Note Text                   
                    currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); // Boot Val Text                   
                    currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);//Slider Parent      

                    //Right Side View...
                    currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(false); //Big Btn                   
                    currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Action Buttons
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(false); //Action Button 2
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true); //Action Buttons 1
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DELETE"; //Action Button Text
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = actionBtnRed;

                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.RemoveNotificationData(data[tempIndex]._id);
                    });
                    break;

                case "InviteToPlaying":
                case "friend_req":
                    string currentCase = data[i].t;
                    //Left Side View...
                    currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);//Normal Sprite
                    currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);//PlayerImg
                    StartCoroutine(AppData.ProfilePicSet(data[i].pp, currentNoti.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
                    //StartCoroutine(AppData.SpriteSetFromURL(data[i].frameImg, currentNoti.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>(), "SetNotificationData-friend_res"));

                    //Center View...
                    currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data[i].title; //Heading

                    currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(!currentCase.Equals("InviteToPlaying"));
                    currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(currentCase.Equals("InviteToPlaying"));
                    if (currentCase.Equals("InviteToPlaying"))
                    {
                        string msg = "<color=#FDFF75>Boot Amount : </color>";
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = msg; //Normal Note Text
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(data[i].bv > 0); // Coin Icon
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(2).gameObject.SetActive(data[i].bv > 0); // Coin Text
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = AppData.PrivateTableBootVal(data[i].bv) + ((data[i].gems > 0) ? "" : "    <color=#FDFF75>MODE : </color>" + data[i].mode); //BootVal Text
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(data[i].gems > 0); //+ icon
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(4).gameObject.SetActive(data[i].gems > 0); //Gems Icon
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(5).gameObject.SetActive(data[i].gems > 0); //Gems Text
                        currentNoti.transform.GetChild(1).GetChild(2).GetChild(5).GetComponent<TextMeshProUGUI>().text = AppData.PrivateTableBootVal(data[i].gems) + "    <color=#FDFF75>MODE : </color>" + data[i].mode; //Gems Val
                        friendInvitr.Add(currentNoti);
                    }
                    else currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].msg;

                    currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);//Slider Parent                  

                    //Right Side View...
                    currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(false); //Big Btn                   
                    currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Action Buttons
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true); //Action Buttons 1
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true); //Action Button 2
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = actionBtnEnable;
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().sprite = actionBtnRed;

                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentCase == "InviteToPlaying") ? "JOIN" : "ACCEPT";
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentCase == "InviteToPlaying") ? "DECLINE" : "REJECT";

                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Loading_screen.instance.ShowLoadingScreen(true);
                        if (currentCase == "InviteToPlaying")
                        {
                            if (AllCommonGameDialog.instance.isHaveGoldGems(data[tempIndex].bv, data[tempIndex].gems)) { }
                            //EventHandler.JoinTableOffriend(data[tempIndex].tbid, data[tempIndex].s, "noti", false); //Join
                            EventHandler.HandleNotification(data[tempIndex]._id); //Join
                            AppData.REMOVABLE_NOTIID = data[tempIndex]._id;

                        }
                        else
                        {
                            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                            EventHandler.ResponseFriendRequest(data[tempIndex]._id, 1); //Accept
                            EventHandler.RemoveNotificationData(data[tempIndex]._id);
                        }
                    });
                    currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.ResponseFriendRequest(data[tempIndex]._id, 0); //Reject
                        EventHandler.RemoveNotificationData(data[tempIndex]._id);
                    });
                    break;
            }
        }

        Loading_screen.instance.ShowLoadingScreen(false);
        SetTap(0);
        currentTap = 0;
        if (!NotiPannel.gameObject.activeSelf) CommanAnimations.instance.FullScreenPanelAnimation(NotiPannel, true);
        AppData.canShowChallenge = false;
    }

    private void HandleNotificationCount(JSONNode data)
    {
        Logger.Print($"{TAG} | ON HandleNotificationCount");
        ncCount = data["count"].AsInt;

        DashboardManager.instance.inboxCounter = ncCount + uRMCCount;
        DashboardManager.instance.inboxBtnCounter.SetActive(DashboardManager.instance.inboxCounter > 0);
        //DashboardManager.instance.inboxImg.sprite = (DashboardManager.instance.inboxCounter > 0) ? DashboardManager.instance.haveNoti : DashboardManager.instance.normal;

        notiCounter.SetActive(data["count"].AsInt > 0);
        notiCounterText.text = data["count"];

        Invoke(nameof(LateCallCount), 1);
    }

    private void LateCallCount()
    {
        int count = CentralPurchase.StockTimerForStock.Count;
        Logger.Print($"{TAG}My Counter >> | ON StockTimerForStock = {count} || SEC = {(ncCount)} | m | {messageTabCount}");
        DashboardManager.instance.inboxCounter = messageTabCount + (ncCount) + count;
        specialOfferCounter.gameObject.SetActive(CentralPurchase.TimerOffeNew != null || CentralPurchase.StockOfferNew != null);
        specialOfferCounterText.text = $"{count}";
        DashboardManager.instance.inboxBtnCounter.SetActive(DashboardManager.instance.inboxCounter > 0);
        DashboardManager.instance.inboxBtnCounterText.text = $"!";
    }

    #endregion

    #region MESSAGE TAB HANDLE <<==============

    int messageTabCount;

    private void HandleMessageTab(JSONNode Data)
    {
        Logger.Print($"{TAG} | ON HandleMessageTab");

        List<IS> data = JsonConvert.DeserializeObject<List<IS>>(Data.ToString());
        noteText.gameObject.SetActive(data.Count == 0);
        noteText.text = "No Messages";

        messageTabCount = data.Count;
        if (NotiContent.childCount > data.Count)
        {
            for (int i = NotiContent.childCount - 1; i >= data.Count; i--)
            {
                Destroy(NotiContent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentNoti;
            int tempIndex = i;
            if (NotiContent.childCount < i + 1)
            {
                currentNoti = Instantiate(NotiPrefab, NotiContent);
            }
            else
            {
                currentNoti = NotiContent.GetChild(i).gameObject;
            }

            //Left Side View...
            currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);//Normal Sprite
            currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);//PlayerImg
            StartCoroutine(AppData.ProfilePicSet(data[i].pp, currentNoti.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
            //StartCoroutine(AppData.SpriteSetFromURL(data[i].frameImg, currentNoti.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>(), "HandleMessageTab"));

            //Center View...
            currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data[i].pn + " SENT YOU A MESSAGE"; //Heading            
            currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].last_msg_body; //Normal Note Text
            currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Normal Note Text
            currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); // Boot Val Text
            currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);//Slider Parent

            //Right Side View...
            currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(false); //Big Btn                   
            currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Action Buttons
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true); //Action Buttons 1
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true); //Action Button 2
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = actionBtnEnable;
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().sprite = actionBtnRed;

            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "REPLY";
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DELETE";

            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {//REPLY
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.GetMyAllMsg(data[tempIndex].cov_id);
                //EventHandler.DeleteChatMsg(data[tempIndex].cov_id);
            });
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {//DELETE
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.DeleteChatMsg(data[tempIndex].cov_id);
            });
        }
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private void HandleUnreadMessageCounter(JSONNode data)
    {
        uRMCCount = data["count"].AsInt;
        DashboardManager.instance.inboxCounter = ncCount + uRMCCount + CentralPurchase.StockTimerForStock.Count;
        Logger.Print(TAG + " My Counter >> " + uRMCCount + " >> Check URMC Is Receieved & Special Offer Counter Remaining >> " + data.ToString() + " URM : " + DashboardManager.instance.inboxCounter);
        DashboardManager.instance.inboxBtnCounter.SetActive(DashboardManager.instance.inboxCounter > 0);
        DashboardManager.instance.inboxBtnCounterText.text = $"!";
        //DashboardManager.instance.inboxImg.sprite = (DashboardManager.instance.inboxCounter > 0) ? DashboardManager.instance.haveNoti : DashboardManager.instance.normal;

        messageCounter.SetActive(data["count"].AsInt > 0);
        messageCounterText.text = data["count"];
    }

    private void SetShareandInviteData()
    {
        noteText.gameObject.SetActive(false);

        if (NotiContent.childCount > 2)
        {
            for (int i = NotiContent.childCount - 1; i >= 2; i--)
            {
                Destroy(NotiContent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject currentNoti;
            int tempIndex = i;

            if (NotiContent.childCount < i + 1)
            {
                currentNoti = Instantiate(NotiPrefab, NotiContent);
            }
            else
            {
                currentNoti = NotiContent.GetChild(i).gameObject;
            }

            //Left Side View...
            currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);//Normal Sprite
            currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);//PlayerImg
            currentNoti.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = notiIcon;//Normal Sprite

            //Center View...
            currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = i == 0 ? "FOLLOW ON FACEBOOK " : "FOLLOW ON INSTAGRAM"; //Heading
            currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "TO BE UPDATED WITH US "; //Normal Note Text
            currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Normale Note
            currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); // Boot Value Note    
            currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);//Slider Parent

            //Right Side View...
            currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(false); //Big Btn                   
            currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Action Buttons
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(false); //Action Button 2
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true); //Action Buttons 1
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "JOIN US"; //Action Button Text
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = actionBtnEnable;

            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            currentNoti.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Application.OpenURL(tempIndex == 0 ? AppData.faceBookLink : AppData.instagramLink);
            });
        }

        Loading_screen.instance.ShowLoadingScreen(false);
    }

    #endregion

    private void HandleSosu(JSONNode data)
    {
        Logger.Print(TAG + " HandleSosu");

        CentralPurchase.StockOfferNew = JsonConvert.DeserializeObject<StockTimeOffer>(data["StockTimeOffer"].ToString());

        if (data["StockTimeOffer"]["stock"] == data["StockTimeOffer"]["usestock"])
        {
            CentralPurchase.StockOfferNew = null;
        }

        SetSpecialOfferData();
        DashboardManager.instance.DashOfferButtonStatus(CentralPurchase.FirstTimeOffer != null);
    }

    private void SetSpecialOfferData()
    {
        noteText.gameObject.SetActive(CentralPurchase.TimerOffeNew == null && CentralPurchase.StockOfferNew == null);
        noteText.text = "No Special Offers";

        int count = CentralPurchase.StockTimerForStock.Count;
        specialOfferCounter.gameObject.SetActive(CentralPurchase.TimerOffeNew != null || CentralPurchase.StockOfferNew != null);
        specialOfferCounterText.text = $"{count}";

        Logger.Print($"SetSpecialOfferData || count == {count}");

        if (count == 0) return;

        if (NotiContent.childCount > count)
        {
            for (int i = NotiContent.childCount - 1; i >= count; i--)
            {
                DestroyImmediate(NotiContent.GetChild(i).gameObject);
            }
        }
        try
        {
            for (int i = 0; i < count; i++)
            {
                GameObject currentNoti;
                int tempIndex = i;

                if (NotiContent.childCount < i + 1)
                    currentNoti = Instantiate(NotiPrefab, NotiContent);

                else
                    currentNoti = NotiContent.GetChild(i).gameObject;

                bool isTimerOffer = CentralPurchase.StockTimerForStock[i].offer.Equals("Timer");
                Logger.Print(TAG + " Timer Offer Is Available >> " + isTimerOffer);

                currentNoti.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                currentNoti.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                StartCoroutine(AppData.SpriteSetFromURL(CentralPurchase.StockTimerForStock[i].pp, currentNoti.transform.GetChild(0).GetChild(1).GetComponent<Image>(), "SetSpecialOfferData"));
                currentNoti.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = CentralPurchase.StockTimerForStock[i].title;

                currentNoti.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = CentralPurchase.StockTimerForStock[i].desc;
                currentNoti.transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Normal Note
                currentNoti.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); // Boot Value Note

                currentNoti.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);//Slider Parent
                currentNoti.transform.GetChild(1).GetChild(3).GetChild(0).gameObject.SetActive(!isTimerOffer);//Slider
                currentNoti.transform.GetChild(1).GetChild(3).GetChild(1).gameObject.SetActive(isTimerOffer);//Timer

                if (isTimerOffer)
                {
                    //For Timer CountDown
                    currentNoti.transform.GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);//Timer Text
                    StorePanel.Instance.timerNotiText = currentNoti.transform.GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    //For Quantity Value Slider
                    currentNoti.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Quantity " + CentralPurchase.StockTimerForStock[i].usestock + "/" + CentralPurchase.StockTimerForStock[i].stock;
                    currentNoti.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(1).GetComponent<Slider>().maxValue = CentralPurchase.StockTimerForStock[i].stock;
                    currentNoti.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(1).GetComponent<Slider>().value = CentralPurchase.StockTimerForStock[i].usestock;
                }

                currentNoti.transform.GetChild(2).GetChild(0).gameObject.SetActive(false); //Action Buttons
                currentNoti.transform.GetChild(2).GetChild(1).gameObject.SetActive(true); //Big Btn
                currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = bigBtnGold; //Big Btn Sprite
                currentNoti.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    AppData.numDifferentiation(CentralPurchase.StockTimerForStock[i].gold) + " GOLD";
                currentNoti.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
                currentNoti.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(true);
                currentNoti.transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text =
                    (isTimerOffer ? (CentralPurchase.TimerOfferProduct == null ? "$ " + CentralPurchase.StockTimerForStock[i].price : CentralPurchase.TimerOfferProduct.price)
                    : (CentralPurchase.StockOfferProduct == null ? "$ " + CentralPurchase.StockTimerForStock[i].price : CentralPurchase.StockOfferProduct.price));

                currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                currentNoti.transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {
                    Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                    if ((isTimerOffer && CentralPurchase.TimerOffeNew != null) || (!isTimerOffer && CentralPurchase.StockOfferNew != null))
                    {
                        AppData.PURCHASEDID = isTimerOffer ? CentralPurchase.TimerOffeNew.pid : CentralPurchase.StockOfferNew.pid;

                        AppData.NOTIiDS = isTimerOffer ? CentralPurchase.TimerOffeNew._id : CentralPurchase.StockOfferNew._id;

                        //CentralPurchase.instance.OnPurchaseClicked(isTimerOffer ? CentralPurchase.TimerOffeNew.inapp : CentralPurchase.StockOfferNew.inapp);
                        FB_IAP.instance.InitiatePurchase(isTimerOffer ? CentralPurchase.TimerOffeNew.inapp : CentralPurchase.StockOfferNew.inapp);
                    }
                });

            }
        }
        catch (System.Exception ex)
        {
            JSONNode objects = new JSONObject
            {
                ["StockTimerForStockCount"] = count,
                ["isTimerOffeNew"] = CentralPurchase.TimerOffeNew == null,
                ["isStockOfferNew"] = CentralPurchase.StockOfferNew == null,
            };
            Loading_screen.instance.SendExe("GameManager", "RedrawCard", $"{objects}", ex);
            Logger.Print($"Ex on redraw : {ex}");
        }
       
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    int currentTap = -1;
    public void InboxPanelClick(int k)
    {
        if (currentTap == k) return;

        StorePanel.Instance.timerNotiText = null;
        //for (int i = 0; i < NotiContent.transform.childCount; i++) Destroy(NotiContent.transform.GetChild(i).gameObject); ///For Temporery

        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        if (k != 4) Loading_screen.instance.ShowLoadingScreen(true);
        currentTap = k;

        switch (k)
        {
            case 0://Notification
                SetTap(0);
                EventHandler.NotificationData();
                break;

            case 1://Message
                noteText.gameObject.SetActive(true);
                EventHandler.IndexScreen();
                //noteText.text = "No Messages";
                SetTap(1);
                break;

            case 2://Special Offer
                SetSpecialOfferData();
                SetTap(2);
                break;

            case 3://Share & Invite                                
                SetShareandInviteData();
                SetTap(3);
                break;

            case 4:
                AppData.canShowChallenge = true;
                currentTap = -1;
                CommanAnimations.instance.FullScreenPanelAnimation(NotiPannel, false);
                break;
        }
    }

    private void SetTap(int k)
    {
        for (int i = 0; i < TapBack.Length; i++)
        {
            TapBack[i].sprite = Tap[i == k ? 0 : 1];
        }
    }
}
