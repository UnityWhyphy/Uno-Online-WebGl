using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

using UnityEngine.Networking;
using DG.Tweening;

public class SocketManagergame : MonoBehaviour
{
    private static string TAG = " > Socket >>> ";
    public static SocketManagergame Instance;

    public static SocketManager socketManager;
    public static Socket MainSocket;


    public static Action<JSONNode> OnListner_SP,
        OnListner_EG, OnListner_JT, OnListner_GST, OnListner_SMC, OnListner_CBV, OnListner_SDC, OnListner_UTS, OnListner_PFCD, OnListner_SAYUNO,
        OnListner_TC, OnListner_WIN, OnListner_TE, OnListner_PENALTY, OnListner_TRGTI, OnListner_SELECTCARD,
        OnListner_ITPL, OnListner_FFBU, OnListner_OUP, OnListner_MP, OnListner_AD, OnListner_UUP, OnListner_PA, OnListner_GGL, OnListner_SGTU, OnListner_COT, OnListner_QD, OnListner_QI,
        OnListner_GQD, OnListner_ST, OnListner_GTI, OnListner_Tonk, OnListner_BH, OnListner_JTOF, OnListner_RF, OnListner_SFR, OnListner_RFR, OnListner_BU, OnListner_UBU, OnListner_OFC, OnListner_LBNEW,
        OnListner_UGH, OnListner_ITP, OnListner_NWWL, OnListner_FB, OnListner_UGE, OnListner_ND, OnListner_JPT, OnListner_RND, OnListner_HN, OnListner_SPG, OnListner_IS, OnListner_GMAM, OnListner_OCH,
        OnListner_PC, OnListner_DCM, OnListner_LOPT, OnListner_LOPTNEW, OnListner_RGR, OnListner_LOTS, OnListner_JTOGR, OnListner_LTNEW,
        OnListner_PT, OnListner_TD, OnListner_TJ, OnListner_EGT, OnListner_TIMER, OnListner_TTIMER, OnListner_SRJU, OnListner_WINTOUR, OnListner_JSP, OnListner_CVIP, OnListner_CPT, OnListner_LD, OnListner_AS,
        OnListner_GLL, OnListner_LTD, OnListner_OTN, OnListener_FGS, OnListener_GSEO, OnListener_SOD, OnListener_HPG, OnListner_HPG, OnListner_UG, OnListner_UUN, OnListner_SRAG, OnListner_USERTURN, OnListner_RSH, OnListner_RJMINI, OnListner_CWA,
        OnListner_GCTGEMS,
        OnListner_DL, OnListner_FL, OnListner_PurchaseDeck, OnListner_PurchaseFrame, OnListner_PurchaseAvatar,
        OnListner_SHG, OnListner_TA,
        OnListner_SD, OnListner_SMI,
        OnListner_VCD, OnListner_CHALLENGE, OnListner_CHALLENGERES,
        OnListener_SGTU,
        OnListner_MDB, OnListner_CDB,
        OnListner_LI, OnListner_LLA, OnListner_GTIFL, OnListner_JTFL, OnListner_LR, OnListner_LE,
        OnListner_GLS, OnListner_CLSR, OnListner_LST, OnListner_PLAYSPIN, OnListner_LSO, OnListner_LS, OnListner_LSI,
        OnListner_ED, OnListner_JE, OnListner_ER, OnListner_KEEP,
        OnListner_GTD, OnListner_PTHEME,
        OnListner_ITPP, OnListner_RNID, OnListner_CVG, OnListner_VBD, OnListner_CVB, OnListner_VBN,
        OnListner_MB, OnListner_CMB, OnListner_MM, OnListner_RGTI, OnListner_LUP, OnListner_LWU, OnListner_LINFO, OnListner_MOREGAME,
        OnListner_MI, OnListner_ULSF, OnListner_CMR, OnListner_NC, OnListner_URMC, OnListner_ULE, OnListner_DWL, OnListner_LOPST, OnListner_MCR,
        OnListner_LLW, OnListner_MBV, OnListner_UENG, OnListner_DOB, OnListner_SC, OnListner_GHL, OnListner_CRH, OnListner_CRC, OnListner_MIA, OnListner_UUC, OnListner_QC, OnListner_DAILYMISSION, OnListner_CLAIMMISSION,
        OmListner_DMI, OnListner_OTCDACK,
        OnListner_SUSG, OnListner_SWAPCARD, OnListner_HighFive,

        Onlistner_TCL, Onlistner_OTC, Onlistner_SCR, Onlistner_ATC

        ;

    public static Action DismisLoader,
        ChestUnlockVideo, ChestTimerRemove, HandleSurPriseReward;


    static bool isEncrypted = true;
    bool DIsconnect = false;

    string Name = "";


    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Input.multiTouchEnabled = false;
        UnHandleEventWhileBackground.Clear();
        UnHandleEventWhileBackground.Add(EventHandler.JT);
        UnHandleEventWhileBackground.Add(EventHandler.GST);
        UnHandleEventWhileBackground.Add(EventHandler.CBV);
        UnHandleEventWhileBackground.Add(EventHandler.SDC);
        /*UnHandleEventWhileBackground.Add(EventHandler.SMC);*/
        UnHandleEventWhileBackground.Add(EventHandler.EG);
        UnHandleEventWhileBackground.Add(EventHandler.UTS);
        UnHandleEventWhileBackground.Add(EventHandler.SELECTCARD);
        UnHandleEventWhileBackground.Add(EventHandler.TC);
        UnHandleEventWhileBackground.Add(EventHandler.PFCD);
        UnHandleEventWhileBackground.Add(EventHandler.PENALTY);
        UnHandleEventWhileBackground.Add(EventHandler.KEEP);
        UnHandleEventWhileBackground.Add(EventHandler.GTI);
        UnHandleEventWhileBackground.Add(EventHandler.RGTI);
        UnHandleEventWhileBackground.Add(EventHandler.CHALLENGE);
        UnHandleEventWhileBackground.Add(EventHandler.CHALLENGERES);
        UnHandleEventWhileBackground.Add(EventHandler.SENDGIFTTOUSER);
        UnHandleEventWhileBackground.Add(EventHandler.TIMER);
        UnHandleEventWhileBackground.Add(EventHandler.TOURNAMENTDATA);
        UnHandleEventWhileBackground.Add(EventHandler.EXITTOURNAMENT);
        UnHandleEventWhileBackground.Add(EventHandler.TIMER);
        UnHandleEventWhileBackground.Add(EventHandler.SRJU);
        UnHandleEventWhileBackground.Add(EventHandler.WINTOUR);
        UnHandleEventWhileBackground.Add(EventHandler.WIN);

        //

        videoEvents.Clear();
        videoEvents.Add(EventHandler.NOTIVIDEOREWARD);
        videoEvents.Add(EventHandler.REMOVENOTIFICATIONDATA);
        videoEvents.Add(EventHandler.COLLECTMAGICBOX);
        videoEvents.Add(EventHandler.COLLECTDAILYBONUS);
        videoEvents.Add(EventHandler.GETHOURLYBONUS);
        videoEvents.Add(EventHandler.VIDEOREWARD);
    }

    void Start()
    {
        //InvokeRepeating(nameof(CheckInternetConnection), 0, 1);
    }

    public void CalledFindServer()
    {
        InvokeRepeating(nameof(CheckInternetConnection), 0, 1);
    }

    public IEnumerator FindServerCallWait()
    {
        yield return new WaitForSeconds(0.5f);
        Logger.Print(TAG + " FindServerCallWait Call Now");
        yield return new WaitUntil(() => (Application.internetReachability != NetworkReachability.NotReachable));
        Logger.Print(TAG + " FindServerCallWait Called after check internet | Check Internet wifi: " + (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork));
        Logger.Print(TAG + " FindServerCallWait Called after check internet | Check Internet Data: " + (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork));
        StartCoroutine(FindServer_APICALL(GetLink()));
        yield return new WaitForSeconds(3);

        Logger.Print(TAG + "  | Check Internet wifi: " + (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork));
        Logger.Print(TAG + "  | Check Internet Data: " + (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork));
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public string GetLink()
    {
        return CanData("Z0oTHcStItGuVoPhjqH7NrB+yVflXACGrW2WXmG4n4KFfXo+7+SY3/1NcA9lDJmb");
    }

    private string CanData(string data)
    {
        return Crypto.Decrypt3(Convert.FromBase64String(data), keyl, ivBytesl);
    }

    //encryption decryption
    private static byte[] ivBytesl = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0 };
    static byte[] keyl = new byte[] { 49, 75, 119, 85, 82, 98, 99, 121, 68, 76, 54, 84, 103, 51, 98, 116, 87, 70, 66, 70, 114, 71, 73, 50, 57, 55, 116, 57, 119, 71, 110, 79 };

    public static string LOCALPORT;
    //find server calle
    public static readonly string FINDAPI_URL = "http://192.168.0.203:4001/findserver";
    public static string SERVER_URL = "http://192.168.0.203:1302" /*+ LOCALPORT*/;

    public static readonly string SOCKET_URL = "/socket.io/";
    public static bool local = true;
    public static string SOCKETURL;
    static List<string> LoginType = new List<string>();

    public IEnumerator FindServer_APICALL(string Url)
    {
        Url = local ? FINDAPI_URL : Url;
        Logger.Print(TAG + " FindServer_APICALL called " + Url);

        using (UnityWebRequest request = UnityWebRequest.Get(Url))
        {
            yield return request.SendWebRequest();
            Logger.Print(TAG + " request.result >> + " + request.result);
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                switch (request.error)
                {
                    case "No Internet Connection":
                    case "Cannot connect to destination host":
                        Logger.Print($"{TAG} | {(SplashManager.instance != null)} Error FindServer_APICALL:  call internet is on? || {Application.internetReachability == NetworkReachability.NotReachable}");
                        if (SplashManager.instance != null)
                        {
                            if (Application.internetReachability == NetworkReachability.NotReachable)
                            {
                                StartCoroutine(FindServer_APICALL(Url));
                            }
                            else
                            {
                                CancelInvoke(nameof(CheckInternetConnection));
                                InvokeRepeating(nameof(CheckInternetConnection), 1, 1);
                            }
                        }

                        else
                            AllCommonGameDialog.instance.ShowConnectionPopUp(false);

                        yield break;
                }

                Logger.Print(TAG + " Error FindServerAPI: " + request.error);
                yield break;
            }

            else
            {
                //Logger.Print(TAG + "Api FOund " + request.downloadHandler.text);
                string res = Crypto.Decrypt3(Convert.FromBase64String(request.downloadHandler.text), key, ivBytes);
                Logger.Print(TAG + " FindServer: " + res);

                Defective.JSON.JSONObject jSONObject = new Defective.JSON.JSONObject(res);
                Logger.Print(TAG + " new Method " + jSONObject.ToString());

                JSONNode Data = JSONString.Parse(res);
                SetSocketURL(Data);
            }

            request.Dispose();
        }
    }

    public static void SetSocketURL(JSONNode jsonNode)
    {
        Logger.Print(TAG + "<color=yellow> SERVER URL >> </color>" + SERVER_URL);

        string proto = jsonNode["proto"];
        string host = jsonNode["host"];
        string port = jsonNode["port"];

        //Locle
        SOCKETURL = (local ? SERVER_URL : (proto + "://" + host + ":" + port)) + SOCKET_URL;
        //Static
        //SOCKETURL = (local ? SERVER_URL : (proto + "://" + host + ":9999")) + SOCKET_URL;//


        SocketManagergame.Instance.ConnectToServer(SOCKETURL);

        AppData.BU_PROFILE_URL = jsonNode["config"]["BU"];
        Logger.Print("----->BU" + AppData.BU_PROFILE_URL);

        //configue data store.
        StaticData.CONFIG_DATA = jsonNode["config"].ToString();
        AppData.configData = jsonNode["config"];
        AppData.ResetTimer = AppData.TimeReset = jsonNode["config"]["SOCKETIDEAL"];
        AppData.PrivacyLink = jsonNode["config"]["PRIVACY"];
        AppData.TermsLink = jsonNode["config"]["TERMS"];
        AppData.hourlyBonusGold = jsonNode["config"]["HourlyBonusGold"].AsLong;
        AppData.videoHourlyBonusGold = jsonNode["config"]["VideoHourlyBonus"].AsFloat;
        AppData.deleteAccountUrl = jsonNode["config"]["DELETEURL"].Value;

        AppData.knockKnockTimer = jsonNode["config"]["SOCKETIDEAL"];
        AppData.VIDEOREWARDCOINS = jsonNode["config"]["VIDEO_REWARD"].AsLong;
        AppData.VIDEOBONUS = jsonNode["config"]["VIDEOBONUS"].AsInt;

        AppData.USERGOLDRECOVER = jsonNode["config"]["USERGOLDRECOVER"].AsLong;

        AppData.faceBookLink = jsonNode["config"]["PRIVACY"];
        AppData.instagramLink = jsonNode["config"]["TERMS"];

        AppData.FreeMiniGame = jsonNode["config"]["TOTALFREEMINI"];

#if UNITY_ANDROID
        AppData.interAdsId = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["FullAd"].ToString());
        AppData.rewardAdsId = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["Reward"].ToString());
        PrefrenceManager.FULL_ADS = jsonNode["config"]["full_add_id_splash"];
#elif UNITY_IOS
        AppData.interAdsId = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["FullAdIOS"].ToString());
        AppData.rewardAdsId = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["RewardIOS"].ToString());
        PrefrenceManager.FULL_ADS = jsonNode["config"]["full_add_id_splashIOS"];
#endif
        Logger.Print($"full_add_id_splash : {jsonNode["config"]["full_add_id_splash"]}");

        LoginType = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["logintype"].ToString());

        AppData.UpdateNEwVesrion = JsonConvert.DeserializeObject<List<string>>(jsonNode["config"]["UpdateNEwVesrion"].ToString());

        AppData.FB_GOLD = jsonNode["config"]["FB_GOLD"];
        AppData.GUEST_GOLD = jsonNode["config"]["GUEST_GOLD"];

        Logger.Print(TAG + " Gold " + AppData.FB_GOLD + " Guest " + AppData.GUEST_GOLD);
        AllCommonGameDialog.instance.HideConnectionPopup();
        if (SplashManager.instance != null)
        {
            SplashManager.instance.fbGoldTxt.text = AppData.numDifferentiation(AppData.FB_GOLD);
            SplashManager.instance.googleGoldTxt.text = AppData.numDifferentiation(AppData.FB_GOLD);
            SplashManager.instance.guestGoldTxt.text = AppData.numDifferentiation(AppData.GUEST_GOLD);

            SplashManager.instance.BtnLin[0].SetActive(LoginType.Contains("guest"));
            SplashManager.instance.BtnLin[1].SetActive(LoginType.Contains("fb"));
            SplashManager.instance.BtnLin[2].SetActive(LoginType.Contains("google"));
        }
        AppData.rules = JsonConvert.DeserializeObject<List<float>>(jsonNode["config"]["RULES"].ToString());
    }


    internal void CheckInternetConnection()
    {
        if (SceneManager.GetActiveScene().name.Equals(EventHandler.SPLASH))
        {
            if (state == State.Connect)
            {
                Invoke(nameof(LoaderOff), 1);
            }
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Logger.NormalLog($"========================================|| NO INTERNET ||================================");
            if (!AllCommonGameDialog.instance.cLostPanel.activeInHierarchy)
            {
                if (CardDeckController.instance != null)
                {
                    if (CardDeckController.instance.selectCard != null)
                    {
                        CardDeckController.instance.TrappedCardDestroy(CardDeckController.instance.selectCard);
                    }
                }
                AllCommonGameDialog.instance.ShowConnectionPopUp(false);
                SocketDisConnectManually();
                Logger.SendLog($"=====================|| DISCONNECT  SERVER||=================");
                state = State.Disconnect;
            }
        }

        else
        {
            if (state == State.Disconnect || state == State.None)
            {
                Logger.NormalLog($"====================================|| INTERNET IS ON ||================================");
                StartCoroutine(FindServerCallWait());

                Logger.NormalLog($"INvoke  OnDisc == CheckInternetConnection");
                CancelInvoke(nameof(CheckInternetConnection));
            }
        }
    }

    private void LoaderOff()
    {
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public void ConnectToServer(string url)
    {
        Logger.Print(TAG + " FINAL SOCKET URL READY : " + url);

        try
        {
            SocketOptions options = new SocketOptions();
            options.AutoConnect = true;
            options.Reconnection = true;
            options.ReconnectionAttempts = 5;
            options.ReconnectionDelay = TimeSpan.FromMilliseconds(5000);
            options.Timeout = TimeSpan.FromMilliseconds(25000);
            options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;

            socketManager = new SocketManager(new Uri(url), options);
            socketManager.Open();

            MainSocket = socketManager.GetSocket();
            MainSocket.On(SocketIOEventTypes.Connect, OnConnect);
            MainSocket.On(SocketIOEventTypes.Disconnect, OnDisconnect);
            MainSocket.On(SocketIOEventTypes.Error, OnError);

            MainSocket.On("res", OnRecevied_PONG);

        }
        catch (Exception e)
        {
            Logger.NormalLog(e.ToString());
        }
    }

    public static bool isReconnect = false, isSingUpGot = false;

    public void OnConnect(Socket socket, Packet packet, object[] args)
    {
        AppData.isMannualDisConnect = false;
        if (SocketManagergame.Instance != null)
            SocketManagergame.Instance.Invoke(nameof(SocketManagergame.Instance.StartKnockKnock), AppData.knockKnockTimer);
        OnSending_PING();

        state = State.Connect;

        if (SplashManager.instance != null)
        {
            SplashManager.instance?.splash1.SetActive(false);
            if (PrefrenceManager.ULT.Equals(""))
            {
                SplashManager.instance.googleLoginImg.sprite = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) ?
                    SplashManager.instance.googleLoginSprite : SplashManager.instance.appleLoginSprite;
                SplashManager.instance.googleLoginTxt.text = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) ?
                    "GOOGLE LOGIN" : "APPLE LOGIN";

                SplashManager.instance.harizontalGrid.enabled = false;

                Sequence seq = DOTween.Sequence();
                seq.Append(SplashManager.instance.logoRect.DOAnchorPos(new Vector2(617.795f, -540), 0.5f).OnComplete(() =>
                {
                    SplashManager.instance.harizontalGrid.enabled = true;
                    SplashManager.instance.loginScreen.SetActive(true);
                    SplashManager.instance.loginPopup.anchoredPosition = new Vector2(800, SplashManager.instance.loginPopup.anchoredPosition.y);
                }));
                seq.AppendInterval(0.5f);

                seq.Append(SplashManager.instance.loginPopup.DOAnchorPosX(0, 0.8f).SetEase(Ease.Linear).OnComplete(() =>
                {

                }).SetDelay(0.1f));

                SplashManager.instance.emojiContent.SetActive(false);
            }
            else
            {
                SplashManager.instance.progressScreen.SetActive(true);
                EventHandler.SendSignUp();
            }
        }
        else
        {
            Logger.NormalLog($"=====================|| SEND SIGNUP REQ ||=================");
            if (state == State.Connect)
                EventHandler.SendSignUp();
        }
    }

    public enum State
    {
        None,
        Connect,
        Disconnect
    }

    public State state;

    private void OnDisconnect(Socket socket, Packet packet, object[] args)
    {
        try
        {
            CancelInvoke(nameof(CheckInternetConnection));
            Logger.NormalLog($"INvoke  OnDisc == OnDisconnect");
            InvokeRepeating(nameof(CheckInternetConnection), 0, 1);
            Logger.NormalLog($"INvoke == OnDisconnect");

            Logger.NormalLog($"socketManager.Socket.IsOpen 2 || {SocketManagergame.MainSocket.IsOpen} ");

            isSingUpGot = false;

            if (AppData.maintenenceEndAfter <= 0 && !AppData.isMannualDisConnect)
            {
                Loading_screen.instance.ShowLoadingScreen(true, "Reconnecting...");
            }
        }
        catch (Exception e)
        {
            e.StackTrace.ToString();
        }
    }

    static List<string> NotUseEvent = new List<string>() { EventHandler.SINGUP, EventHandler.OFC };

    private void OnError(Socket socket, Packet packet, object[] args)
    {
        try
        {
            Error error = args[0] as Error;
            switch (error.Code)
            {
                case SocketIOErrors.User:
                    Logger.Error(TAG + " SocketIOSErrors user " + error.Message);
                    break;

                case SocketIOErrors.Internal:
                    Logger.Error(TAG + " SocketIOSErrors Internal " + error.Message);
                    break;

                default:
                    Logger.Error(TAG + " SocketIOSErrors default " + error.Message);
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.Error($" EX = {e}");

        }
    }

    public void StartKnockKnock()
    {
        Logger.Print(TAG + " Show Knock Knock PopUp >>> ");
        CancelInvoke(nameof(StartKnockKnock));
        SocketDisConnectManually();
        AllCommonGameDialog.instance.ShowConnectionPopUp(true);
        CancelInvoke("ReconnectHandle");

    }

    public static void SendDataToServer(string data, string en)
    {
        if (en != "OFC")
        {
            Logger.SendLog($" en = {en} ");
            Logger.SendLog(TAG + " SendDataToServer called " + en + " data " + data + " Socket ");
            Logger.Print(TAG + " Cond-1 " + (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) +
                " Cond-2 " + (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork));
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            //Logger.Print($"MainSocket is null? => : {MainSocket == null}");
            string reqData = isEncrypted ? Crypto.Encrypt3(data, key, ivBytes) : data;
            MainSocket?.Emit("req", reqData);
        }
        else if (!NotUseEvent.Contains(en))
        {
            Logger.Print(TAG + " Else Part called not SP");
            if (MainSocket != null && MainSocket.IsOpen)
            {
                Logger.Print(TAG + " Reconnect loader mathi event gai");
                string reqData = isEncrypted ? Crypto.Encrypt3(data, key, ivBytes) : data;
                MainSocket?.Emit("req", reqData);
            }
            else Loading_screen.instance.ShowLoadingScreen(true);
        }
        else
            Logger.Print(TAG + " Else Part called SP");
    }

    public static IEnumerator DalayEventSend(string data)
    {
        yield return new WaitForSeconds(1f);
        Logger.Print($"Main socket is open = {MainSocket.IsOpen}");
        if (MainSocket.IsOpen)
        {
            string reqData = isEncrypted ? Crypto.Encrypt3(data, key, ivBytes) : data;
            MainSocket?.Emit("req", reqData);
            Logger.Print($"MainSocket is null? => : {MainSocket == null}");
        }
    }


    private static byte[] ivBytes = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0 };
    static byte[] key = new byte[] { 49, 75, 119, 85, 82, 98, 99, 121, 68, 76, 54, 84, 103, 51, 98, 116, 87, 70, 66, 70, 114, 71, 73, 50, 57, 55, 116, 57, 119, 71, 110, 79 };
    List<string> UnHandleEventWhileBackground = new List<string>();
    public static string UIDCFD = "";

    private void OnRecevied_PONG(Socket socket, Packet packet, object[] args)
    {
        string d = Crypto.Decrypt3(Convert.FromBase64String(args[0].ToString()), key, ivBytes); ; // Crypto.decodeString();
        JSONNode jsonNode = JSON.Parse(d);
        string en = jsonNode["en"];

        if (!en.Equals(EventHandler.PONG))
        {
            if (en != "OFC")
                Logger.RecevideLog(TAG + " En " + en);
            if (!jsonNode["flag"].AsBool)
            {
                if (!en.Equals(EventHandler.OFC))
                {
                    Logger.Print(TAG + " Knock Knock Timer Stop Because " + en + " Receieved >>> ");
                    CancelInvoke(nameof(StartKnockKnock));
                    Invoke(nameof(StartKnockKnock), AppData.knockKnockTimer);
                }

                if (en != "OFC")
                {
                    Logger.RecevideLog(TAG + " Event Recived " + jsonNode.ToString());
                }
                if (!Backgroundhandle)
                {
                    LodingControl(Loading_screen.instance.LoaderPanel.activeInHierarchy);
                }

                if (Backgroundhandle && UnHandleEventWhileBackground.Contains(en))
                {
                    Logger.Print(TAG + " Event Return from Background " + en);
                    switch (en)
                    {
                        case EventHandler.WIN:

                            List<Winner> PlayerWinner = JsonConvert.DeserializeObject<List<Winner>>(jsonNode["data"].ToString());
                            bool isTournament = jsonNode["istournament"].AsBool;

                            if (isTournament)
                            {
                                for (int i = 0; i < PlayerWinner.Count; i++)
                                {
                                    if (PlayerWinner[i].w == 1 && !PrefrenceManager._ID.Equals(PlayerWinner[i].uid))
                                    {
                                        Logger.Print(TAG + "Winner Return");

                                        Invoke(nameof(DelayShowDashBoard), 1);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (jsonNode["_ip"] == 1)
                                {
                                    //OnListner_WIN?.Invoke(jsonNode);
                                    //GameWinner.instance.OpenWinnerScreen(false);
                                }
                                else
                                {

                                    for (int i = 0; i < PlayerWinner.Count; i++)
                                    {
                                        Logger.Print(TAG + "Winner Return Else " + (PlayerWinner[i].w == 1 && PrefrenceManager._ID.Equals(PlayerWinner[i].uid)) + " || " + (PlayerWinner[i].w == 1 && !PrefrenceManager._ID.Equals(PlayerWinner[i].uid)));
                                        if (PlayerWinner[i].w == 1 && PrefrenceManager._ID.Equals(PlayerWinner[i].uid))
                                        {
                                            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.congratulations, "You won your last game! Fantastic job! Keep up the great work and aim for even more victories in the next game!");
                                        }
                                        else if (PlayerWinner[i].w == 1 && !PrefrenceManager._ID.Equals(PlayerWinner[i].uid))
                                        {
                                            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.dontGiveUp, "You didn’t win this time, but don't worry! Keep playing, and your next victory is just around the corner. Best of luck for your next game!");
                                        }
                                    }

                                    GameManager.instance.DashBoardOn();
                                    AppData.GTIDATA = new JSONObject();
                                    EventHandler.LgsReqSend();
                                }

                                WinData = jsonNode;
                            }

                            break;

                        case EventHandler.RGTI:
                        case EventHandler.GTI:
                            Logger.Print(TAG + "GTI Data EMPTY");
                            if (GameManager.instance != null && !GameManager.instance.playingScreen.activeInHierarchy && AppData.GTIDATA.Count == 0)
                            {
                                Logger.Print(TAG + " Call Background");
                                AppData.GTIDATA = jsonNode;
                                WhichEventIRecieveWhileIBackground.Add(en);
                                Loading_screen.instance.LoaderPanel.SetActive(true);
                                Invoke(nameof(BackgroundWork), 1f);
                                return;
                            }
                            break;

                        case EventHandler.TOURNAMENTDATA:
                            AppData.GTIDATA = new JSONObject();
                            Logger.Print(TAG + "GTI Data EMPTY");
                            AppData.TournamentData = jsonNode["data"];
                            break;

                        case EventHandler.WINTOUR:
                            AppData.GTIDATA = new JSONObject();
                            Logger.Print(TAG + "GTI Data EMPTY");
                            GameManager.instance.DashBoardOn();
                            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.tournamentWin, "You Win Your Last Tournament.Your won rewards was credited to your account.You can check it from Gold-History.");
                            EventHandler.LgsReqSend();
                            break;
                    }

                    if (en.Equals(EventHandler.EG))
                    {
                        if (!jsonNode["data"]["UID"].Equals(PrefrenceManager._ID))
                        {
                            Logger.Print(TAG + "Eg Handle Sended >>>");
                            OnListner_EG?.Invoke(jsonNode["data"]);
                            return;
                        }
                        else
                        {
                            Logger.Print(TAG + "Eg Data Stored >>> ");
                            AppData.GTIDATA = new JSONObject();
                            OnListner_EG?.Invoke(jsonNode["data"]);
                        }
                    }
                    WhichEventIRecieveWhileIBackground.Add(en);
                    return;
                }

                switch (en)
                {
                    case EventHandler.SINGUP:
                        //action call nathi thati km ke signup na data dashboard vala scene load thay atle onenble ma no thay atle a karel 6.
                        Logger.Print(TAG + " Is Reconnect >> " + isReconnect);
                        Logger.NormalLog($"INvoke  OnDisc == SINGUP");
                        CancelInvoke(nameof(CheckInternetConnection));
                        isReconnect = false;

                        if (!isSingUpGot)
                        {
                            AppData.SignUpData = jsonNode["data"];
                            Logger.Print(TAG + "singup got evereytime" + jsonNode["data"].ToString());
                            OnRecevied_SP(jsonNode["data"]);
                            isSingUpGot = true;
                        }

                        break;

                    case EventHandler.TreassureChestList:
                    case EventHandler.UnlockTreasureChest:
                    case EventHandler.TreasureChestStopTimer:
                    case EventHandler.OPENCHESTSLOTE:
                        Onlistner_TCL?.Invoke(jsonNode);
                        break;

                    case EventHandler.AddTreasureChest:
                        Onlistner_ATC?.Invoke(jsonNode);
                        break;

                    case EventHandler.OpenTreasureChest:
                        Onlistner_OTC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SurpriseChestReward:
                        Onlistner_SCR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.RGTI:
                        AppData.IsRejoin = true;
                        Logger.Print(TAG + " Game kill mari ne avvu tyare");

                        if (!GameManager.instance.playingScreen.activeInHierarchy)
                            CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, true, 0);

                        GameManager.instance.allPlayingScreen.gameObject.SetActive(true);
                        OnListner_RGTI?.Invoke(jsonNode);
                        AppData.GTIDATA = jsonNode;

                        break;

                    case EventHandler.GTI:
                        Logger.Print("EVenet Received >> " + en);
                        AppData.IsRejoin = false;
                        dedlockCnt = 0;
                        ISDEADLOCKACCURE = false;
                        AppData.GTIDATA = jsonNode;
                        OnListner_GTI?.Invoke(jsonNode);
                        break;

                    case EventHandler.JT:
                        OnListner_JT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.GST:
                        OnListner_GST?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.CBV:
                        OnListner_CBV?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SMC:
                        OnListner_SMC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.UTS:
                        OnListner_UTS?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SELECTCARD:
                        OnListner_SELECTCARD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SDC:
                        OnListner_SDC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PFCD:
                        OnListner_PFCD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.OTCDECK:///*****************************************************************************
                        OnListner_OTCDACK?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SAYUNO:
                        OnListner_SAYUNO?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SWAPCARD:
                        OnListner_SWAPCARD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.HIGHFIVE:
                        OnListner_HighFive?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TC:
                        OnListner_TC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.EG:
                        OnListner_EG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PENALTY:
                        OnListner_PENALTY?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.CHALLENGE:
                        OnListner_CHALLENGE?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.CHALLENGERES:
                        OnListner_CHALLENGERES?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SENDGIFTTOUSER:
                        OnListener_SGTU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TE:
                        OnListner_TE?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.KEEP:
                        OnListner_KEEP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.WIN:
                        List<Winner> PlayerWinner = JsonConvert.DeserializeObject<List<Winner>>(jsonNode["data"].ToString());
                        GameManager.instance.WinnerAnimation(PlayerWinner);
                        OnListner_WIN?.Invoke(jsonNode);
                        break;

                    case EventHandler.ITPL:
                        OnListner_ITPL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.FFBU:
                        OnListner_FFBU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.OUP:
                        OnListner_OUP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MP:
                        OnListner_MP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.DeckList:
                        OnListner_DL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.FrameList:
                        OnListner_FL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PurchaseDeck:
                        OnListner_PurchaseDeck?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PurchaseFrame:
                        OnListner_PurchaseFrame?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PurchaseAvatar:
                        OnListner_PurchaseAvatar?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.AD:
                        OnListner_AD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.UUP:
                        OnListner_UUP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PA:
                        OnListner_PA?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.GGL:
                        OnListner_GGL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COT:
                        OnListner_COT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.QD:
                        OnListner_QD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.QI:
                        OnListner_QI?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.GQD:
                        OnListner_GQD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TONK:
                        OnListner_Tonk?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.BUDDIESHUB:
                        OnListner_BH?.Invoke(jsonNode);
                        break;

                    case EventHandler.JOINTTABLEOFFRIEND:
                        OnListner_JTOF?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SENDFRIENDREQUEST:
                        OnListner_SFR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REMOVEFRIEND:
                        OnListner_RF?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.RESPONCEFRIENDREQUEST:
                        OnListner_RFR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.BLOCKUSER:
                        OnListner_BU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.UNBLOCKUSER:
                        OnListner_UBU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.OFC:
                        OnListner_OFC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LEADERBOARD:
                        OnListner_LBNEW?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.USERGOLDHISTORY:
                        OnListner_UGH?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.INVITETOPLAYING:
                        OnListner_ITP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.WEEKLYWINNERLIST:
                        OnListner_NWWL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.DAILYWINNERLIST:
                        OnListner_DWL?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.FEEDBACK:
                        OnListner_FB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.USERGEMS:
                        OnListner_UGE?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.NOTIFICATIODATA:
                        OnListner_ND?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REMOVENOTIFICATIONDATA:
                        OnListner_RND?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.HANDLENOTIFICATION:
                        OnListner_HN?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.INDEXSCREEN:
                        OnListner_IS?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.GETMYALLMSG:
                        OnListner_GMAM?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.OLDCHATHISTORY:
                        OnListner_OCH?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PERSONALCHAT:
                        OnListner_PC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.DELETECHATMSG:
                        OnListner_DCM?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LISTOFPLAYINGTABLE:
                        OnListner_LOPT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LOPTNEW:
                        OnListner_LOPTNEW?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REMOVEGLOBALROOM:
                        OnListner_RGR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LISTOFTOURNAMENTSLOT:
                        OnListner_LOTS?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.JOINTABLEOFGLOBALROOM:
                        OnListner_JTOGR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LISTOFTOURNAMENT:
                        OnListner_LTNEW?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.PLAYTURNAMENT:
                        OnListner_PT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TOURNAMENTDATA:
                        OnListner_TD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TOURNAMENTJOIN:
                        OnListner_TJ?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REJOINTOURNAMENT:
                        OnListner_TRGTI?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.EXITTOURNAMENT:
                        OnListner_EGT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.TIMER:
                        OnListner_TIMER?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SRJU:
                        OnListner_SRJU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.WINTOUR:
                        OnListner_WINTOUR?.Invoke(jsonNode);
                        break;

                    case EventHandler.CHECKVIP:
                        OnListner_CVIP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.CREATEPRIVATETABLE:
                        OnListner_CPT?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.USERGOLD:
                        OnListner_UG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.STARTREDGREENGAME:
                        OnListner_SRAG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.USERTURN:
                        OnListner_USERTURN?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REMOVEREDGREEN:
                        OnListner_RSH?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REJOINMINIGAME:
                        OnListner_RJMINI?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COLLECTWINAMMOUNT:
                        OnListner_CWA?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MYDAILYBONUS:
                        OnListner_MDB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COLLECTDAILYBONUS:
                        FirebaseData.EventSendWithFirebase("Collectdailybonus");
                        AppData.PlayedMiniGame = 0;
                        OnListner_CDB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.INVITETOPLAYINGPOPUP:
                        OnListner_ITPP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.REMOVENOTIFICATIONINDASHBOARD:
                        OnListner_RNID?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COLLECTVIDEOGOLD:
                        OnListner_CVG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.VIDEOBONUSDATA:
                        OnListner_VBD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COLLECTVIDEOBONUS:
                        OnListner_CVB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.VIDEOBONUSNOTES:
                        OnListner_VBN?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MAGICBOX:
                        OnListner_MB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.COLLECTMAGICBOX:
                        OnListner_CMB?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MaintanaceManage:
                        OnListner_MM?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LEVELUP:
                        OnListner_LUP?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LEVELWISEUNLOCK:
                        OnListner_LWU?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LEVELINFO:
                        OnListner_LINFO?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MOREGAME:
                        OnListner_MOREGAME?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MUSEUMINFO:
                        OnListner_MI?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.UNLOCKSPECIALFEATURES:
                        OnListner_ULSF?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.NOTIFICATIONCOUNT:
                        OnListner_NC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.USERMSGCOUNTER:
                        OnListner_URMC?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LISTOFPLAYINGSTARTABLE:
                        OnListner_LOPST?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MAKECOUPANREADY:
                        OnListner_MCR?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LOTTOLANDWINNER:
                        OnListner_LLW?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.MYBOOTVALUE:
                        OnListner_MBV?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.UPDATENTRYGOLD:
                        OnListner_UENG?.Invoke(jsonNode["data"]);
                        AppData.NormalEntryGold = jsonNode["data"]["bv"];
                        AppData.NormalEntryGems = jsonNode["data"]["gems"];

                        break;

                    case EventHandler.DAILYMISSION:
                        OnListner_DAILYMISSION?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.DAILYMISSIONINDEX:
                        OmListner_DMI?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.MULTILOGIN:
                        CancelInvoke("ReconnectHandle");
                        SocketDisConnectManually();
                        AllCommonGameDialog.instance.ShowConnectionPopUp(false, true);
                        break;

                    case EventHandler.CFD:
                        Loading_screen.instance.ShowLoadingScreen(false);
                        string m = "You have placed account deletion request to us. You request is in progress.It will be resolved in " + jsonNode["data"]["daydiff"].AsInt +
                            " days.In mean time if you want to reactive your account you can.If you want to take your action quick you can contact us on support@sixacegames.com.Have a nice day!";
                        AllCommonGameDialog.instance.SetDeleteDialogData(m);
                        UIDCFD = jsonNode["data"]["uid"];
                        PrefrenceManager.ULT = "";
                        break;
                    case EventHandler.UCFD:
                        AllCommonGameDialog.instance.deleteAccountPopup.gameObject.SetActive(false);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.SendSignUp();
                        break;

                    case EventHandler.CLAIMDAILYMISSION:
                        OnListner_CLAIMMISSION?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.GOLDSTORE:
                        OnListener_FGS?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.GoldStoreExitOffer:
                    case EventHandler.ExitOffer:
                        OnListener_GSEO?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SpecialOfferData:
                        OnListener_SOD?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.HANDLEPAYMENTGOLD:
                    case EventHandler.HANDLEPAYMENTGEMS:
                        OnListener_HPG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.SOSU:
                        OnListner_SUSG?.Invoke(jsonNode["data"]);
                        break;

                    case EventHandler.LUCKYSPIN:
                        OnListner_LS?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.COLLECTLUCKYSPIN:
                        OnListner_CLSR?.Invoke(jsonNode["data"]);
                        break;
                    case EventHandler.USERLEAFS:
                        OnListner_ULE?.Invoke(jsonNode["data"]);
                        break;

                }
            }
            else
            {
                Logger.Print(TAG + "FLAG TRUE " + jsonNode.ToString());
                //error code
                int code = jsonNode["errcode"];

                if (en.Equals(EventHandler.EG))
                {
                    Logger.RecevideLog("Popup karo  En " + en);
                    GameManager.instance.BackgroundEGDataHandle();  // change
                }
                if (code == 7007)
                {
                    //deadlock issue resolve
                    Loading_screen.instance.ShowLoadingScreen(true);
                    DeadLockRemoveTimer(jsonNode);
                    return;
                }
                if (code == 7012)//EG ma error && Signup ma Error
                {
                    //load dashbord from here
                    AppData.GTIDATA = new JSONObject();
                    Logger.Print(TAG + "GTI Data EMPTY");
                    EventHandler.SendSignUp();
                    isSingUpGot = false;
                    return;
                }
                if (code == 3005)//not enough gold
                {
                    AllCommonGameDialog.instance.ShowNotEnoughDialog(false);
                    return;
                }
                else if (code == 6009 && !SceneManager.GetActiveScene().name.Equals(EventHandler.GAMEPLAY))//not enough gems
                {
                    AllCommonGameDialog.instance.ShowNotEnoughDialog(true);
                    return;
                }
                else if (en.Equals(EventHandler.JOINTTABLEOFFRIEND))
                {
                    if (AllCommonGameDialog.instance.ChallengeDialog.activeSelf)
                        AllCommonGameDialog.instance.ChallengeBtnClick(-1);

                    ISDEADLOCKACCURE = false;
                    if (!AppData.REMOVABLE_NOTIID.Equals(""))
                    {
                        EventHandler.RemoveNotificationData(AppData.REMOVABLE_NOTIID);
                        AppData.REMOVABLE_NOTIID = "";
                    }
                    AllCommonGameDialog.instance.SetJustOkDialogData("ALERT", jsonNode["msg"]);
                    return;
                }

                if (UnHandleEventWhileBackground.Contains(en))
                    return;
                else if (en.Equals(EventHandler.RGTI))
                {
                    if (GameManager.instance.playingScreen.activeInHierarchy)
                        GameManager.instance.DashBoardOn();
                    return;
                }
                if (en.Equals(EventHandler.FFBU))
                {
                    AllCommonGameDialog.instance.SetJustOkDialogData("ALERT", "Invalid Id. Please Enter valid Id.");
                    return;
                }

                if (!en.Equals(EventHandler.OFC))
                    AllCommonGameDialog.instance.SetJustOkDialogData("ALERT", jsonNode["msg"]);

                //if (en.Equals(EventHandler.HANDLENOTIFICATION))
                if (en.Equals(EventHandler.HIPN))
                {
                    EventHandler.RemoveNotificationData(AppData.REMOVABLE_NOTIID);
                }
            }
        }

        else
        {
            OnSending_PING();
        }

    }

    private static void DelayShowDashBoard()
    {
        GameManager.instance.DashBoardOn();
        Logger.Print(TAG + "GTI Data EMPTY");
        AppData.GTIDATA = new JSONObject();
        AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.tournamentLoss, "You lost your last tournament. Better luck next time! The rest of the tournament continues between the winning players.");
        EventHandler.LgsReqSend();
        return;
    }

    private void LodingControl(bool active, int sec = 1)
    {
        if (active)
        {
            CancelInvoke(nameof(FalseLoading));
            Invoke(nameof(FalseLoading), sec);
        }
    }

    void FalseLoading()
    {
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public bool Backgroundhandle = false;
    List<string> WhichEventIRecieveWhileIBackground = new List<string>();
    private static List<string> videoEvents = new List<string>();

    private void OnApplicationPause(bool pause)
    {
        if (SceneManager.GetActiveScene().name.Equals("SPLASH")) return;

        if (AppData.isTutorialPlay) return;

        Logger.Print(TAG + " Scoket OnApplicationPause " + Backgroundhandle + " Count " + WhichEventIRecieveWhileIBackground.Count + " pause " + pause);
        if (pause)
        {
            Backgroundhandle = true;
            if (CardDeckController.instance.selectCard != null)
            {
                CardDeckController.instance.TrappedCardDestroy(CardDeckController.instance.selectCard);
            }
        }
        else
        {
            Logger.Print(TAG + "Onpause false + |" + Backgroundhandle);

            if (!SceneManager.GetActiveScene().name.Equals(EventHandler.SPLASH))
            {
                CancelInvoke(nameof(BackgroundWork));
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    //InvokeRepeating(nameof(CheckInternetConnection), 1, 1);
                    CheckInternetConnection();
                    Backgroundhandle = false;
                }
                else if (AppData.GTIDATA != null && AppData.GTIDATA.Count != 0)
                {
                    Logger.Print(TAG + "Onpause false GTI HAS <><><|" + Backgroundhandle);
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    Invoke(nameof(BackgroundWork), 2f);
                }
                else
                    Backgroundhandle = false;
                Logger.Print(TAG + "Onpause false Last line+ |" + Backgroundhandle);
            }
            else
                Backgroundhandle = false;

        }
    }

    public void ResetBackgroundFlag()
    {
        Backgroundhandle = false;
        Logger.Print(TAG + "Onpause false Last line+ |" + Backgroundhandle);
    }

    public static JSONNode WinData = "", EGData = "";

    public void BackgroundWork()
    {
        Backgroundhandle = false;
        Logger.Print(TAG + " Socket background flag " + Backgroundhandle + " Count " + WhichEventIRecieveWhileIBackground.Count + " Value " + string.Join(", ", WhichEventIRecieveWhileIBackground));
        Logger.Print(TAG + " Tournament Data " + AppData.TournamentData.Count);
        Logger.Print(TAG + " GTI " + (AppData.GTIDATA != null) + " Condition 2 >> " + (AppData.GTIDATA.Count != 0) + " GTI DATA " + AppData.GTIDATA.ToString());

        if (AppData.GTIDATA != null && AppData.GTIDATA.Count != 0)
        {
            if (!WhichEventIRecieveWhileIBackground.Contains(EventHandler.EG) && !WhichEventIRecieveWhileIBackground.Contains(EventHandler.WIN))
            {
                Logger.Print(TAG + "Event recive ");
                if (WhichEventIRecieveWhileIBackground.Count != 0)
                {
                    int mySI = -1;
                    Logger.Print(TAG + " GTI Data Player Count >> " + AppData.GTIDATA);

                    for (int i = 0; i < AppData.GTIDATA["data"]["pi"].Count; i++)
                    {
                        if (AppData.GTIDATA["data"]["pi"][i]["ui"] != null)
                        {
                            Logger.Print(TAG + " User Is Not Null While Checking GTI Data After Reconnect >>");
                            if (AppData.GTIDATA["data"]["pi"][i]["ui"]["uid"].Equals(PrefrenceManager._ID))
                            {
                                Logger.Print(TAG + " Geted Prefrence Manager Id While Reconnect & Calling Rejoin >> ");
                                mySI = AppData.GTIDATA["data"]["pi"][i]["ui"]["si"].AsInt;
                                break;
                            }
                        }
                    }
                    EventHandler.RejoinTable(AppData.GTIDATA["data"]["_id"], mySI);
                    WhichEventIRecieveWhileIBackground.Clear();
                }
                else
                {
                    Logger.Print(TAG + " Just Loader Off While Reconnect >>");
                    Loading_screen.instance.ShowLoadingScreen(false);
                }
            }
            else
            {
                Logger.Print(TAG + " EG and Winner Declared " + WinData.Count);
                if (WinData.Count != 0)
                {
                    List<string> SingleRoundPlayerIds = JsonConvert.DeserializeObject<List<string>>(WinData["isSingleround"].ToString());
                    bool isSingleRound = SingleRoundPlayerIds.Contains(PrefrenceManager._ID);

                    if (isSingleRound && !WinData["istournament"])
                    {
                        OnListner_WIN?.Invoke(WinData);
                        GameWinner.instance.OpenWinnerScreen(false);
                    }
                    else
                    {
                        int mySI = -1;
                        Logger.Print(TAG + " WINNER Data Player Count >> " + WinData.ToString());

                        for (int i = 0; i < AppData.GTIDATA["data"]["pi"].Count; i++)
                        {
                            if (AppData.GTIDATA["data"]["pi"][i]["ui"] != null)
                            {
                                Logger.Print(TAG + " User Is Not Null While Checking GTI Data After Reconnect >>");
                                if (AppData.GTIDATA["data"]["pi"][i]["ui"]["uid"].Equals(PrefrenceManager._ID))
                                {
                                    Logger.Print(TAG + " Geted Prefrence Manager Id While Reconnect & Calling Rejoin >> ");
                                    mySI = AppData.GTIDATA["data"]["pi"][i]["ui"]["si"].AsInt;
                                    break;
                                }
                            }
                        }
                        EventHandler.RejoinTable(AppData.GTIDATA["data"]["_id"], mySI);
                        WhichEventIRecieveWhileIBackground.Clear();
                    }
                    Loading_screen.instance.ShowLoadingScreen(false);
                    WinData = "";
                }
                else if (WhichEventIRecieveWhileIBackground.Contains(EventHandler.EG))
                {
                    AppData.SignUpData = AppData.GTIDATA = new JSONObject();
                    Logger.Print(TAG + "My EG Handled");
                }
                WhichEventIRecieveWhileIBackground.Clear();
            }
        }

        else if (WhichEventIRecieveWhileIBackground.Contains(EventHandler.TOURNAMENTDATA) && AppData.TournamentData.Count != 0)//mapping ma betho
        {
            EventHandler.RejoinTournament("", 0, AppData.TournamentData["_id"], 0);
            AppData.TournamentData = "";
            WhichEventIRecieveWhileIBackground.Clear();
        }
        else
        {
            WhichEventIRecieveWhileIBackground.Clear();
            if (GameManager.instance.playingScreen.activeInHierarchy)// For not to stuck in gameplay that's why to do this........
            {
                Logger.Print(TAG + " Normal screen mathi background ma gyo|| Playing is on");
                GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
                GameManager.instance.playingScreen.gameObject.SetActive(false);
                CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
                CommanAnimations.instance.FullScreenPanelAnimation(GameWinner.instance.WinnerPannel, false, 0);
            }
            Logger.Print(TAG + " Normal screen mathi background ma gyo");
        }
    }

    private void OnRecevied_SP(JSONNode Data)
    {
        Logger.RecevideLog(TAG + " OnRecevied_SP called SOcketmanager" + Data.ToString());

        Scene scene = SceneManager.GetActiveScene();

        switch (scene.name)
        {
            case EventHandler.SPLASH:
                Logger.Print(TAG + " CASE-1 called");
                StartCoroutine(SplashManager.instance.LoadScene(EventHandler.DASHBOARD));
                break;

            case EventHandler.DASHBOARD:
                Logger.Print(TAG + " CASE-2 called");
                OnListner_SP?.Invoke(Data);
                break;

            default:
                Logger.Print(TAG + " CASE default called");
                SceneManager.LoadScene("DASHBOARD");
                break;
        }
    }

    static int dedlockCnt = 0, doDeadlock = 2;
    public static bool ISDEADLOCKACCURE = false;

    public void DeadLockRemoveTimer(JSONNode data)
    {//2 time call marva mate ek var ave ne direct biji var call marvano
        Logger.Print(TAG + " DeadLockRemoveTimer remove Ded lockCnt >> " + dedlockCnt + " >> DoDead Lock >> " + doDeadlock);
        if (dedlockCnt != doDeadlock)
        {
            ISDEADLOCKACCURE = true;
            StartCoroutine(Deadwait(data));
        }
        else
        {
            Logger.Print(TAG + " Dealock no call no lago else " + dedlockCnt);
            dedlockCnt = 0;
            ISDEADLOCKACCURE = false;
            if (!AppData.REMOVABLE_NOTIID.Equals(""))
            {
                EventHandler.RemoveNotificationData(AppData.REMOVABLE_NOTIID);
                AppData.REMOVABLE_NOTIID = "";
            }
            Loading_screen.instance.ShowLoadingScreen(false);
            AllCommonGameDialog.instance.SetJustOkDialogData("Seat OCCUPIED", data["msg"]);
        }
    }

    int DeadLockWaitTime = 2;
    public IEnumerator Deadwait(JSONNode node)
    {
        yield return new WaitForSeconds(DeadLockWaitTime);
        JSONNode data = node["data"];
        Logger.Print(TAG + " deadlock no call lago.count " + dedlockCnt + " lock " + doDeadlock);
        if (/*!AppData.isReconnect &&*/ dedlockCnt != doDeadlock)
        {
            dedlockCnt++;
            Logger.Print(TAG + " En " + node["en"].Value);
            switch (node["en"].Value)
            {
                case EventHandler.JOINTABLEOFGLOBALROOM:
                    Logger.Print(TAG + " Global Room Ma enter");
                    doDeadlock = 2;
                    DeadLockWaitTime = doDeadlock;
                    EventHandler.JoinTableOfGlobalRoom(data["tbid"], data["s"], data["type"]);
                    break;
                case EventHandler.JOINTTABLEOFFRIEND:
                    Logger.Print(TAG + "JTOF enter");
                    doDeadlock = 2;
                    DeadLockWaitTime = doDeadlock;
                    EventHandler.JoinTableOffriend(data["tbid"], data["s"], data["type"], false);
                    break;
                case EventHandler.HANDLENOTIFICATION:
                    Logger.Print(TAG + " HN Ma enter");
                    doDeadlock = 2;
                    DeadLockWaitTime = doDeadlock;
                    EventHandler.JoinTableOffriend(data["tbid"], data["s"], "noti", false);
                    break;
            }
        }
        else
        {
            ISDEADLOCKACCURE = false;
            if (!AppData.REMOVABLE_NOTIID.Equals(""))
            {
                EventHandler.RemoveNotificationData(AppData.REMOVABLE_NOTIID);
                AppData.REMOVABLE_NOTIID = "";
            }
            Loading_screen.instance.ShowLoadingScreen(false);
        }
    }

    public void OnSending_PING()
    {
        JSONNode data = new JSONObject
        {
            ["en"] = EventHandler.PONG,
        };

        MainSocket?.Emit(EventHandler.PING, data.ToString());
    }


    IEnumerator WaitLR(JSONNode jsonNode)
    {
        yield return new WaitForSeconds(0.3f);
        OnListner_LR?.Invoke(jsonNode);
    }

    IEnumerator WaitER(JSONNode jsonNode)
    {
        yield return new WaitForSeconds(0.3f);
        OnListner_ER?.Invoke(jsonNode);
    }

    public static void SocketDisConnectManually()
    {
        Logger.Print(TAG + " SocketDisConnectManually Manually is Null =" + (MainSocket == null));
        if (MainSocket == null) return;

        socketManager.Socket.Disconnect();
        Logger.NormalLog($"============= Socket Is Disconnect ============");
        MainSocket.Off();
        MainSocket = null;
        isReconnect = false;
        AppData.isMannualDisConnect = true;

    }
}
