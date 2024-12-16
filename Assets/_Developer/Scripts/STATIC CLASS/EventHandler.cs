using System.Collections.Generic;
using BestHTTP.SocketIO;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

public static class EventHandler
{
    //-------------------SCENE NAME-----------------------

    public const string SPLASH = "SPLASH";
    public const string LOGIN = "LOGIN";
    public const string DASHBOARD = "DASHBOARD";
    public const string GAMEPLAY = "GAMEPLAY";
    public const string MYPROFILE = "MYPROFILE";

    public const string GLT = "GLT";

    //----------------SOCKET EVENT NAME------------------ 

    public const string PING = "PING";
    public const string PONG = "PONG";
    public const string SINGUP = "SP";
    public static string HDD = "HDD";                         //HDD - Handle Default Dashboard

    //playing
    public const string PLAYGAME = "PLAYGAME";                //PLAYGAME - Play Game Button Click.    
    public const string RGTI = "RGTI";
    public const string GTI = "GTI";                          //GTI - Play Game Response All Data.
    public const string JT = "JT";                            //JT - Player Malse.
    public const string GST = "GST";                          //GST - Game Start Timer
    public const string SMC = "SMC";                          //See My Card
    public const string CBV = "CBV";                          //Collect Boot Value
    public const string SDC = "SDC";                          //Start Deal Card
    public const string PFCD = "PFCD";
    public const string PENALTY = "PENALTY";
    public const string CHALLENGE = "CHALLENGE";
    public const string CHALLENGERES = "CHALLENGERES";
    public const string UTS = "UTS";                          //User Turn Start
    public const string TE = "TE";
    public const string TC = "TCOD";                        //Throw Card
    public const string DISCARDALL = "DISCARDALL";                        //Throw Card
    public const string KEEP = "KEEP";
    public const string EG = "EG";                            //EG - Exit Game..
    public const string WIN = "WIN";
    public const string SENDGIFTTOUSER = "SGTU";
    public const string SAYUNO = "SAYUNO";
    public const string SWAPCARD = "SWAPCARD";
    public const string HIGHFIVE = "HIGHFIVE";
    public const string HIGHFIVESHOW = "HIGHFIVESHOW";
    public const string SELECTCARD = "SELECTCARD";
    //public const string USERLASTGAMESTATUS = "ULGS";
    public const string MUP = "MUP";
    public const string OTCDECK = "OTCDECK";
    public const string KEEPPENALTY = "KEEPPENALTY";

    //profile
    public const string MP = "MP";
    public const string DeckList = "DL";
    public const string FrameList = "FL";
    public const string PurchaseDeck = "PDECK";
    public const string PurchaseFrame = "PFRAME";
    public const string PurchaseAvatar = "PAVATAR";

    public const string ITPL = "ITPL";
    public const string ITP = "ITP";
    public const string FFBU = "FFBU";
    public const string OUP = "OUP";

    public const string AD = "AD";
    public const string UUP = "UUP";
    public const string PA = "PA";
    public const string GGL = "GGL";
    public const string COT = "COT";
    public const string QD = "QD";
    public const string QI = "QI";
    public const string GQBC = "GQBC";
    public const string GQD = "GQD";                        //WIN - Win Panel Data MAlse.
    public const string DeviceType = "ios";
    public const string ST = "ST";
    public const string TONK = "TONK";
    public const string BUDDIESHUB = "BH";
    public const string JOINTTABLEOFFRIEND = "JTOF";
    public const string SENDFRIENDREQUEST = "SFR";
    public const string RESPONCEFRIENDREQUEST = "RFR";
    public const string REMOVEFRIEND = "RF";
    public const string BLOCKUSER = "BU";
    public const string UNBLOCKUSER = "UBU";
    public const string OFC = "OFC";
    public const string LEADERBOARD = "LB";
    public const string USERGOLDHISTORY = "UGH";
    public const string INVITETOPLAYING = "ITP";
    public const string WEEKLYWINNERLIST = "WWL";
    public const string DAILYWINNERLIST = "DWL";
    public const string FEEDBACK = "FB";

    //
    public const string HIPN = "HIPN";

    //User Gol,Gems,Notes
    public const string USERGEMS = "UGE";
    public const string USERGOLD = "UG";
    public const string USERLEAFS = "ULE";

    //notification
    public const string NOTIFICATIODATA = "ND";
    public const string REMOVENOTIFICATIONDATA = "RND";
    public const string HANDLENOTIFICATION = "HN";
    public const string COLLECTREWARD = "CR";
    public const string INDEXSCREEN = "IS";
    public const string GETMYALLMSG = "GMAM";
    public const string OLDCHATHISTORY = "OCH";
    public const string PERSONALCHAT = "PC";
    public const string DELETECHATMSG = "DCM";

    //live table
    public const string LISTOFPLAYINGTABLE = "LOPT";
    public const string LOPTNEW = "LOPTNEW";
    public const string LISTOFTOURNAMENTSLOT = "LOTS";
    public const string REMOVEGLOBALROOM = "RGR";
    public const string JOINTABLEOFGLOBALROOM = "JTOGR";

    //Tournament
    public const string LISTOFTOURNAMENT = "LT";
    public const string PLAYTURNAMENT = "PT";
    public const string TOURNAMENTDATA = "TD";
    public const string TOURNAMENTJOIN = "TJ";
    public const string EXITTOURNAMENT = "EGT";
    public const string TIMER = "TIMER";
    public const string SRJU = "SRJU";
    public const string WINTOUR = "WINTOUR";
    public const string REJOINTOURNAMENT = "TRGTI";


    //-------------------LOGIN TYPE----------------------

    public const string GUEST = "guest";
    public const string FACEBOOK = "facebook";
    public const string GOOGLE = "google";
    public const string APPLE = "apple";

    //-------------------PRIVATE TABLE---------------------
    public const string CHECKVIP = "CVIP";
    public const string CREATEPRIVATETABLE = "CPT";

    public const string GOLDSTORE = "FGS";
    public const string HANDLEPAYMENTGOLD = "HPG";
    public const string HANDLEPAYMENTGEMS = "HPGEMS";
    public const string GoldStoreExitOffer = "GSEO";
    public const string ExitOffer = "EO";
    public const string SpecialOfferData = "SOD";
    public const string SOSU = "SOSU";

    //-------------------MINI GAME--------------------
    public const string STARTREDGREENGAME = "SRNG";
    public const string USERTURN = "UserTurn";
    public const string REMOVEREDGREEN = "RRG";
    public const string REJOINMINIGAME = "RJMINI";
    public const string COLLECTWINAMMOUNT = "CWA";

    //-------------------DailyBonus--------------------
    public const string MYDAILYBONUS = "MDB";
    public const string COLLECTDAILYBONUS = "CDB";

    //----------------- Hourly Bonus ------------------
    public const string GETHOURLYBONUS = "GHB";

    //PopUp
    public const string INVITETOPLAYINGPOPUP = "ITPP";
    public const string REMOVENOTIFICATIONINDASHBOARD = "RNID";
    public const string COLLECTVIDEOGOLD = "CVG";
    public const string VIDEOBONUSDATA = "VBD";
    public const string COLLECTVIDEOBONUS = "CVB";
    public const string VIDEOBONUSNOTES = "VBN";

    //Magic Bonus
    public const string MAGICBOX = "MB";
    public const string COLLECTMAGICBOX = "CMB";

    public const string MaintanaceManage = "MM";
    public const string VIDEOREWARD = "VR";
    public const string NOTIVIDEOREWARD = "VR";

    public const string LEVELUP = "LUP";

    public const string LEVELWISEUNLOCK = "LWU";
    public const string COLLECTRESULTREWARD = "CRR";
    public const string LEVELINFO = "LINFO";

    public const string MUSEUMINFO = "MI";
    public const string UNLOCKSPECIALFEATURES = "ULSF";
    public const string NOTIFICATIONCOUNT = "NC";
    public const string USERMSGCOUNTER = "URMC";
    public const string LISTOFPLAYINGSTARTABLE = "LOPST";
    public const string MAKECOUPANREADY = "MCR";
    public const string LOTTOLANDWINNER = "LLW";
    public const string MYBOOTVALUE = "MBV";
    public const string UPDATENTRYGOLD = "UENG";
    public const string CLAIMDAILYMISSION = "CLAIMMISSION";

    //Dialy Mission...
    public const string DAILYMISSION = "DM";
    public const string DAILYMISSIONINDEX = "DMI";

    //More Game
    public const string MOREGAME = "MOREGAME";

    //OLD DATA LOSS [Multi Login Attempt]
    public const string MULTILOGIN = "ODL";

    //delete account event
    public const string CFD = "CFD";
    public const string UCFD = "UCFD";
    public const string PDELETE = "PDELETE";

    //Lucky Spin
    public const string LUCKYSPIN = "LS";
    public const string COLLECTLUCKYSPIN = "CLSR";

    //Teassure Chest
    public const string TreassureChestList = "TCL";
    public const string UnlockTreasureChest = "UNTC";
    public const string AddTreasureChest = "ATC";
    public const string TreasureChestStopTimer = "TCST";
    public const string OPENCHESTSLOTE = "TOCS";
    public const string OpenTreasureChest = "OTC";
    public const string RemoveTreasureChest = "RTC";
    public const string SurpriseChestReward = "SCR";

    public static void SendSignUp()
    {
        JSONNode JSONNode = new JSONObject
        {
            ["model"] = SystemInfo.deviceModel,
            ["player_id"] = PrefrenceManager.PLAYER_ID,
            ["ult"] = PrefrenceManager.ULT,
            ["ue"] = PrefrenceManager.UE,
            ["gender"] = PrefrenceManager.GENDER,
            ["fid"] = PrefrenceManager.FID,
            ["gid"] = PrefrenceManager.GID,
            ["aid"] = PrefrenceManager.AID,
            ["fb_token"] = PrefrenceManager.FB_TOKEN,
            ["aVersion"] = Application.platform == RuntimePlatform.Android ? AppData.AppVersionAndroid : AppData.AppVersionIOS,
            ["pn"] = PrefrenceManager.PN,
            ["pp"] = PrefrenceManager.PP,
            ["det"] = PrefrenceManager.DET,
            ["cc"] = PrefrenceManager.CC,
            ["rfc"] = PrefrenceManager.RFC,
            //["isnewVersion"] = 1,//1 Lock, 0 Unlock
            //["isnewVersion"] = AppData.isLevelWiseLockFirebase,//1 Lock, 0 Unlock
#if UNITY_EDITOR
            ["isnewVersion"] = 1,//1 Lock, 0 Unlock
#else
            ["isnewVersion"] = (AppData.isLevelWiseLockFirebase ? 1 : 0),//1 Lock, 0 Unlock
#endif

#if UNITY_EDITOR
            ["DId"] = SystemInfo.deviceUniqueIdentifier, // device id : Guest login ana upar thi thai che
#else
            ["DId"] = Application.platform == RuntimePlatform.Android ? SystemInfo.deviceUniqueIdentifier : PrefrenceManager.DeviceId,
#endif
            ["fcm_token"] = PrefrenceManager.FCM_TOKEN,
            ["aov"] = SystemInfo.operatingSystem,
            ["isRate"] = PrefrenceManager.ISRATE,
            ["Community_Join"] = PrefrenceManager.COMMUNITY_JOIN,
            ["Full_Ads"] = PrefrenceManager.FULL_ADS,
            ["rflcode"] = AppData.REFRELCODEOTHERUSER,
            ["det"] = (Application.platform == RuntimePlatform.Android) ? "android" : "ios",
            //["det"] = "ios",
            ["theme"] = PrefrenceManager.Themes
        };

        JSONNode data = new JSONObject
        {
            ["en"] = EventHandler.SINGUP,
            ["data"] = JSONNode,
        };
        Loading_screen.instance.ShowLoadingScreen(true);
        SocketManagergame.SendDataToServer(data.ToString(), EventHandler.SINGUP);
    }

    public static void SendPickFromCloseDeck()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PFCD,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PFCD);
    }

    public static void ThrowCard(string Card, string color, bool isUno, int targetSi = -1, int swapersi = -1)
    {
        JSONNode data = new JSONObject
        {
            ["c"] = Card,
            ["color"] = color,
            ["sayUno"] = isUno,
            ["targetSi"] = targetSi,
            ["swapersi"] = swapersi
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.TC,
            ["data"] = data,

        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.TC);
    }
    public static void SelectCardReq(string Card)
    {
        JSONNode data = new JSONObject
        {
            ["card"] = Card
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.SELECTCARD,
            ["data"] = data,

        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.SELECTCARD);
    }

    public static void SendChallenge(string color, bool flag, bool isover, int si, int senderSI, bool isColorcard)
    {
        JSONNode data = new JSONObject
        {
            ["color"] = color,
            ["flag"] = flag,
            ["isover"] = isover,
            ["si"] = si,
            ["senderSi"] = senderSI,
            ["isColorcard"] = isColorcard
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.CHALLENGE,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.CHALLENGE);
    }

    public static void SendGiftToUser(string tBid, string img, string imgType, string type, long price, string rid, string mode, bool isauto, int rsi, int emojinumber = 0)
    {
        JSONNode data = new JSONObject
        {
            ["tbid"] = tBid,
            ["img"] = img,
            ["type"] = type,
            ["imgType"] = imgType,
            ["price"] = price,
            ["rid"] = rid,
            ["rsi"] = rsi,
            ["mode"] = mode,
            ["isauto"] = isauto,
            ["emojinumber"] = emojinumber
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.SENDGIFTTOUSER,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.SENDGIFTTOUSER);
    }

    public static void Report(string tbId)
    {

        JSONNode data = new JSONObject
        {
            ["tbid"] = tbId
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = "REPORT",
            ["data"] = data,

        };

        SocketManagergame.SendDataToServer(objects.ToString(), "REPORT");
    }

    public static void KeepCard()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.KEEP,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.KEEP);
    }

    public static void SendHDD()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.HDD,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.HDD);
    }

    public static void UENGSend(long bv, long gems)
    {
        JSONNode data = new JSONObject
        {
            ["bv"] = bv,
            ["gems"] = gems
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.UPDATENTRYGOLD,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.UPDATENTRYGOLD);
    }

    public static void PlayGame(string mode, long bv, int player, int gems, int ip, JSONArray rulesArray = null, int playingV = 1)
    {
        //ip==1==Private Table
        JSONNode data = new JSONObject
        {
            ["bv"] = bv,
            ["mode"] = mode,
            ["player"] = player,
            ["gems"] = gems,
            ["ip"] = ip,
            ["rules"] = rulesArray,
            ["playingV"] = playingV
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PLAYGAME,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PLAYGAME);
    }

    public static void ExitGame()
    {
        JSONNode data = new JSONObject
        {
            ["isEg"] = 1
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.EG,
            ["data"] = data,

        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.EG);
    }

    public static void InvitePlayer(long bv)
    {
        JSONNode data = new JSONObject
        {
            ["bv"] = bv
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.ITPL,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.ITPL);
    }

    public static void SearchPlayer(string id)
    {
        JSONNode data = new JSONObject
        {
            ["unid"] = id,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.ITPL,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.ITPL);
    }

    public static void OpponentUserProfile(string UserId)
    {
        JSONNode data = new JSONObject
        {
            ["oppid"] = UserId,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.OUP,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.OUP);
    }

    public static void MyProfile()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MP,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MP);
    }

    public static void DeckListEvent()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.DeckList,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.DeckList);
    }

    public static void FrameListEvent()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.FrameList,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.FrameList);
    }

    public static void FramePurchase(string id)
    {
        JSONNode data = new JSONObject
        {
            ["aid"] = id
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PurchaseFrame,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PurchaseFrame);
    }

    public static void AvatarPurchase(string id)
    {
        JSONNode data = new JSONObject
        {
            ["aid"] = id
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PurchaseAvatar,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PurchaseAvatar);
    }

    public static void DeckPurchase(string id)
    {
        JSONNode data = new JSONObject
        {
            ["aid"] = id
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PurchaseDeck,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PurchaseDeck);
    }

    public static void AvatarClcik()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.AD,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.AD);
    }

    public static void UpdateUserProfile(JSONObject obj)
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.UUP,
            ["data"] = obj,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.UUP);
    }

    public static void PurchasedAvatar(string Aid)
    {
        JSONNode data = new JSONObject
        {
            ["aid"] = Aid,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PA,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PA);
    }

    public static void GetGiftList(string mode)
    {
        JSONNode data = new JSONObject
        {
            ["mode"] = mode,
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GGL,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GGL);
    }

    public static void ChatOnTable(string Body)
    {
        JSONNode data = new JSONObject
        {
            ["body"] = Body,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COT,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COT);
    }

    public static void QuestCollectBonus(int index, int noti)
    {
        JSONNode data = new JSONObject
        {
            ["index"] = index,
            ["isnoti"] = noti
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GQBC,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GQBC);
    }

    public static void SwitchTable(string mode, long bv, int player, int gems, int ip, string currentTbid, JSONArray rulesArray = null, int playingV = 0)
    {
        JSONNode data = new JSONObject
        {
            ["bv"] = bv,
            ["mode"] = mode,
            ["player"] = player,
            ["gems"] = gems,
            ["ip"] = ip,
            ["ltbid"] = currentTbid,
            ["rules"] = rulesArray,
            ["playingV"] = playingV
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.ST,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.ST);
    }

    public static void BuddiesHub(int type)
    {
        JSONNode data = new JSONObject
        {
            ["type"] = type
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.BUDDIESHUB,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.BUDDIESHUB);
    }

    public static void OnlineFriendsCounter()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.OFC,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.OFC);
    }

    public static void JoinTableOffriend(string tbid, string _id, string isFrom, bool isauto, int si = -1)
    {
        JSONNode data = new JSONObject
        {
            ["isauto"] = isauto,
            ["tbid"] = tbid,
            ["s"] = _id,
            ["si"] = si,
            ["type"] = isFrom,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.JOINTTABLEOFFRIEND,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.JOINTTABLEOFFRIEND);
    }

    public static void BlockUser(string oppid)
    {
        JSONNode data = new JSONObject
        {
            ["oppid"] = oppid,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.BLOCKUSER,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.BLOCKUSER);
    }

    public static void UnblockBlockUser(string oppid)
    {
        JSONNode data = new JSONObject
        {
            ["oppid"] = oppid,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.UNBLOCKUSER,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.UNBLOCKUSER);
    }

    public static void FindFrienByUniqueId(string id)
    {
        JSONNode data = new JSONObject
        {
            ["unid"] = id,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.FFBU,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.FFBU);
    }

    public static void FrienRequestSend(string s, string r, int iscom)
    {
        JSONNode data = new JSONObject
        {
            ["s"] = s,
            ["r"] = r,
            ["iscom"] = iscom,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.SENDFRIENDREQUEST,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.SENDFRIENDREQUEST);
    }

    public static void RemoveFriend(string oppid)
    {
        JSONNode data = new JSONObject
        {
            ["oppid"] = oppid,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REMOVEFRIEND,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REMOVEFRIEND);
    }

    public static void LeaderBoard(int type)
    {
        JSONNode data = new JSONObject
        {
            ["type"] = type
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.LEADERBOARD,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LEADERBOARD);
    }

    public static void UserGoldHistory()
    {//accept 1 else 0

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.USERGOLDHISTORY,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.USERGOLDHISTORY);
    }

    public static void InviteToPlaying(string uid, string TableId, long bootValue, int si, int gems, int ip, string mode)
    {
        JSONNode data = new JSONObject
        {
            ["uid"] = uid,
            ["tbid"] = TableId,
            ["bv"] = bootValue,
            ["si"] = si,
            ["gems"] = gems,
            ["ip"] = ip,
            ["mode"] = mode,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.INVITETOPLAYING,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.INVITETOPLAYING);
    }

    public static void WeeklyWinnerList()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.WEEKLYWINNERLIST,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.WEEKLYWINNERLIST);
    }

    public static void DailyWinnerList()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.DAILYWINNERLIST,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.DAILYWINNERLIST);
    }


    public static void GameQuestData()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GQD,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GQD);
    }

    public static void FeedBack(string msg, string Email, bool isReport, string UID, int iscomp, string from, string DId)
    {
        JSONNode data = new JSONObject
        {
            ["body"] = msg,
            ["ue"] = Email,
            ["from"] = from,
            ["Did"] = DId
        };

        if (isReport)
        {
            JSONNode Report = new JSONObject();
            Report.Add("uid", UID);
            Report.Add("iscomp", iscomp);
            data.Add("report", Report);
        }

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.FEEDBACK,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.FEEDBACK);
    }


    //Notification Data
    public static void NotificationData()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.NOTIFICATIODATA,
            ["data"] = new JSONObject(),

        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.NOTIFICATIODATA);
    }

    public static void ResponseFriendRequest(string nid, int res)
    {//accept 1 else 0

        JSONNode data = new JSONObject
        {
            ["nid"] = nid,
            ["res"] = res,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.RESPONCEFRIENDREQUEST,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.RESPONCEFRIENDREQUEST);
    }

    public static void RemoveNotificationData(string NID)
    {
        JSONNode data = new JSONObject
        {
            ["nid"] = NID,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REMOVENOTIFICATIONDATA,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REMOVENOTIFICATIONDATA);
    }

    public static void HandleNotification(string NID)
    {
        JSONNode data = new JSONObject
        {
            ["nid"] = NID,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.HANDLENOTIFICATION,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.HANDLENOTIFICATION);
    }

    public static void IndexScreen()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.INDEXSCREEN,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.INDEXSCREEN);
    }

    public static void OldchatHistory(string r)
    {
        JSONNode data = new JSONObject
        {
            ["r"] = r,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.OLDCHATHISTORY,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.OLDCHATHISTORY);
    }

    public static void PersnoalChat(string r, string body)
    {
        JSONNode data = new JSONObject
        {
            ["r"] = r,
            ["body"] = body
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PERSONALCHAT,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PERSONALCHAT);
    }

    public static void GetMyAllMsg(string conId)
    {
        JSONNode data = new JSONObject
        {
            ["conId"] = conId,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GETMYALLMSG,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GETMYALLMSG);
    }

    public static void DeleteChatMsg(string conId)
    {
        JSONNode data = new JSONObject
        {
            ["conId"] = conId,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.DELETECHATMSG,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.DELETECHATMSG);
    }

    public static void ListOfPlayingTable()
    {
        JSONNode objects = new JSONObject
        {
            //["en"] = EventHandler.LISTOFPLAYINGTABLE,
            ["en"] = EventHandler.LOPTNEW,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LOPTNEW);
        //SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LISTOFPLAYINGTABLE);
    }

    public static void ListOfTournamentSlot()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.LISTOFTOURNAMENTSLOT,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LISTOFTOURNAMENTSLOT);
    }

    public static void RejoinTournament(string tbid, int si, string tuid, int tsi)
    {
        JSONNode data = new JSONObject
        {
            ["tbid"] = tbid,
            ["si"] = si,
            ["tuid"] = tuid,
            ["tsi"] = tsi,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REJOINTOURNAMENT,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REJOINTOURNAMENT);
    }

    public static void RemoveGlobalRoom()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REMOVEGLOBALROOM,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REMOVEGLOBALROOM);
    }

    public static void JoinTableOfGlobalRoom(string tbid, int si, string isFrom)
    {
        JSONNode data = new JSONObject
        {
            ["tbid"] = tbid,
            ["si"] = si,
            ["type"] = isFrom,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.JOINTABLEOFGLOBALROOM,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.JOINTABLEOFGLOBALROOM);
    }

    public static void ListofTournament()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.LISTOFTOURNAMENT,
            ["data"] = new JSONObject(),
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LISTOFTOURNAMENT);
    }

    public static void PlayTournament(int bv, string mode, int winbv, string id, long gems)
    {
        JSONNode data = new JSONObject
        {
            ["mode"] = mode,
            ["bv"] = bv,
            ["winbv"] = winbv,
            ["touId"] = id,
            ["gems"] = gems,
            ["playingV"] = 1
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PLAYTURNAMENT,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PLAYTURNAMENT);
    }

    public static void ExitTournament(string tid)
    {
        JSONNode data = new JSONObject
        {
            ["touId"] = tid,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.EXITTOURNAMENT,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.EXITTOURNAMENT);
    }

    public static void CreatePrivateTable(long bv, long gems, int _ip, string mode, int players)
    {

        JSONNode data = new JSONObject
        {
            ["bv"] = bv,
            ["gems"] = gems,
            ["_ip"] = _ip,
            ["mode"] = mode,
            ["player"] = players,
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.CREATEPRIVATETABLE,
            ["data"] = data,
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.CREATEPRIVATETABLE);
    }

    public static void GoldStore(int type)
    {
        JSONNode data = new JSONObject
        {
            ["type"] = type
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GOLDSTORE,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GOLDSTORE);
    }

    //public static void HandlePaymentGold(string packid, string inapp, string receiptData, string receiptSignature, string orderId, int ispromo, string notiId)
    public static void HandlePaymentGold(JSONNode jdata)
    {
        JSONNode data = jdata;
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.HANDLEPAYMENTGOLD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.HANDLEPAYMENTGOLD);
    }

    //public static void HandlePaymentGems(string packid, string inapp, string receiptData, string receiptSignature, string orderId, int ispromo, string notiId)
    public static void HandlePaymentGems(JSONNode jdata)
    {
        JSONNode data = jdata;

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.HANDLEPAYMENTGEMS,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.HANDLEPAYMENTGEMS);
    }

    //MiniGame Event
    public static void StartMiniGame(bool flag)
    {
        JSONNode data = new JSONObject
        {
            ["IsFree"] = flag
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.STARTREDGREENGAME,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.STARTREDGREENGAME);
    }

    public static void UserTurn(string tID, string type)
    {
        JSONNode data = new JSONObject
        {
            ["type"] = type,
            ["id"] = tID
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.USERTURN,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.USERTURN);
    }

    public static void RemoveRedGreen(string gid)
    {
        JSONNode data = new JSONObject
        {
            ["gid"] = gid
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REMOVEREDGREEN,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REMOVEREDGREEN);
    }

    public static void RejoinMiniGame(int num)
    {
        JSONNode data = new JSONObject
        {
            ["Rjmini"] = num
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REJOINMINIGAME,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REJOINMINIGAME);
    }

    public static void CollectiWinAmmount(string gID, bool flag)
    {
        JSONNode data = new JSONObject
        {
            ["gid"] = gID
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTWINAMMOUNT,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTWINAMMOUNT);
    }

    //Daily Bonus
    public static void MakeDailyBonus()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MYDAILYBONUS,
            ["data"] = new JSONObject(),

        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MYDAILYBONUS);
    }

    public static void CollectDailyBonus(bool isvideo)
    {
        JSONNode data = new JSONObject
        {
            // ["isshare"] = isshare,
            //  ["total"] = chips,
            ["isvideo"] = isvideo
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTDAILYBONUS,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTDAILYBONUS);
        //        CustomFirebaseEvents.SendEvent("DailyBonusCollect");
    }

    //Hourly Bonus
    public static void CollectHourlyBonus(bool isVideo)
    {
        JSONNode data = new JSONObject
        {
            ["isvideo"] = isVideo,
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GETHOURLYBONUS,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GETHOURLYBONUS);
    }

    //ITPP
    public static void RemoveNotificationDashboard(string s, string tbid)
    {
        JSONNode data = new JSONObject
        {
            ["s"] = s,
            ["tbid"] = tbid,

        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.REMOVENOTIFICATIONINDASHBOARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.REMOVENOTIFICATIONINDASHBOARD);
    }

    public static void CollectVideoGold()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTVIDEOGOLD,
            ["data"] = new JSONObject(),

        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTVIDEOGOLD);
    }

    public static void CollectVideoBonus()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTVIDEOBONUS,
            ["data"] = new JSONObject(),

        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTVIDEOBONUS);
    }

    //Magic Bonus
    public static void CollectMagicBox()
    {
        JSONNode data = new JSONObject
        {

        };
        data.Add("video", AppData.fromMagicBox);
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTMAGICBOX,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTMAGICBOX);
    }

    //Daily Mission

    public static void SendDailyMission()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.DAILYMISSION,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.DAILYMISSION);
    }

    //Rejoin
    public static void RejoinTable(string tbid, int si)
    {
        JSONNode data = new JSONObject
        {
            ["tbid"] = tbid,
            ["si"] = si,
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.RGTI,
            ["data"] = data,
        };
        Loading_screen.instance.ShowLoadingScreen(true);
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.RGTI);
    }

    public static void VideoReward(string type = "", int vr = 0)
    {
        JSONNode data = new JSONObject
        {
            ["type"] = type,
            ["vr"] = vr
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.VIDEOREWARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.VIDEOREWARD);
    }

    public static void ScratchVideoReward(int amount)
    {
        JSONNode data = new JSONObject
        {
            ["vr"] = amount,
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.VIDEOREWARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.VIDEOREWARD);
    }

    public static void WatchVideoReward(int amount, string isRecovery = "")
    {
        JSONNode data = new JSONObject
        {
            ["wr"] = amount,
            ["type"] = isRecovery
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.VIDEOREWARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.VIDEOREWARD);
    }

    public static void NotiVideoReward(string type, long value)
    {
        JSONNode data = new JSONObject();
        data.Add("vr", value);
        data.Add("type", type);
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.NOTIVIDEOREWARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.NOTIVIDEOREWARD);
    }

    public static void CollectRewardGold(JSONNode videoReward)
    {

        JSONNode data = new JSONObject
        {

        };
        data.Add("videoReward", videoReward);

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTRESULTREWARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTRESULTREWARD);
    }

    public static void MyBootValue()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MYBOOTVALUE,
            ["data"] = new JSONObject(),

        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MYBOOTVALUE);
    }

    public static void SendClaimMision()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.CLAIMDAILYMISSION,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.CLAIMDAILYMISSION);
    }

    public static void SendMoreGame(string isFrom)
    {
        JSONNode data = new JSONObject
        {
            ["isfrom"] = isFrom
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MOREGAME,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MOREGAME);
    }

    public static void UpdateCFD(int status)
    {
        JSONNode data = new JSONObject
        {
            ["status"] = status,
            ["uid"] = SocketManagergame.UIDCFD
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.UCFD,
            ["data"] = data
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.UCFD);
    }

    public static void InstaNTDeleteReq()
    {
        JSONNode data = new JSONObject
        {
            ["uid"] = SocketManagergame.UIDCFD
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PDELETE,
            ["data"] = data
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PDELETE);
    }



    public static void GetCheckDealoffer()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GoldStoreExitOffer,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GoldStoreExitOffer);
    }

    public static void SendExitOffer()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.ExitOffer,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.ExitOffer);
    }

    public static void SendSayUNO(int sayerSI)
    {
        JSONNode data = new JSONObject
        {
            ["si"] = sayerSI,
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.SAYUNO,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.SAYUNO);
    }

    public static void SendPanelty(int si, int catcherSi)
    {
        JSONNode data = new JSONObject
        {
            ["si"] = si,
            ["CatcherSI"] = catcherSi
        };
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.PENALTY,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.PENALTY);
    }

    public static void SendLuckySpin()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.LUCKYSPIN,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.LUCKYSPIN);
    }
    public static void CollectLuckySpinReward()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.COLLECTLUCKYSPIN,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.COLLECTLUCKYSPIN);
    }

    public static void SendMaintenence()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MaintanaceManage,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MaintanaceManage);
    }

    public static void SendKEEPPENALTY()
    {
        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.KEEPPENALTY,
            ["data"] = new JSONObject(),
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.KEEPPENALTY);
    }

    public static void SendSwapSi(int si)
    {
        JSONNode data = new JSONObject
        {
            ["si"] = si
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.SWAPCARD,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.SWAPCARD);
    }

    public static void SendHigh(int si)
    {
        JSONNode data = new JSONObject
        {
            ["si"] = si
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.HIGHFIVE,
            ["data"] = data,
        };
        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.HIGHFIVE);
    }

    public static void UnlockChest(int index)
    {
        JSONNode data = new JSONObject
        {
            ["index"] = index
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = UnlockTreasureChest,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), UnlockTreasureChest);
    }

    public static void OpenChestData(int index, int gems, int isagain)
    {
        JSONNode data = new JSONObject
        {
            ["index"] = index,
            ["gems"] = gems,
            ["isagain"] = isagain
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = OpenTreasureChest,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), OpenTreasureChest);
    }

    public static void ChestTimerRemove(int index)
    {
        JSONNode data = new JSONObject
        {
            ["index"] = index
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = TreasureChestStopTimer,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), TreasureChestStopTimer);
    }

    public static void TreasureChestRemove(int index)
    {
        JSONNode data = new JSONObject
        {
            ["index"] = index
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = RemoveTreasureChest,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), RemoveTreasureChest);
    }

    public static void GameLoadTime(string time)
    {
        JSONNode data = new JSONObject
        {
            ["time"] = time
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.GLT,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.GLT);
    }

    public static void LgsReqSend()
    {
        JSONNode reqData = new JSONObject
        {
            ["lgs"] = "0",
            ["ulgs"] = "0"
        };

        JSONNode data = new JSONObject
        {
            ["en"] = EventHandler.MUP,
            ["data"] = reqData,
        };
        SocketManagergame.SendDataToServer(data.ToString(), EventHandler.MUP);
    }

    public static void ManageUserProfile(string name)
    {
        JSONNode data = new JSONObject
        {
            [$"{name}"] = true
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = EventHandler.MUP,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), EventHandler.MUP);
    }

    public static void SurprizeChestReward(string name, int index, bool isagain)
    {
        JSONNode data = new JSONObject
        {
            ["chest"] = name,
            ["index"] = index,
            ["isagain"] = isagain
        };

        JSONNode objects = new JSONObject
        {
            ["en"] = SurpriseChestReward,
            ["data"] = data
        };

        SocketManagergame.SendDataToServer(objects.ToString(), SurpriseChestReward);
    }
}