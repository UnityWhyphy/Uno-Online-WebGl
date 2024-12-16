using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefrenceManager
{
    const string _ID_ID = "_ID_ID";
    const string PLAYERID_ID = "PLAYERID_ID";
    const string ULT_ID = "ULT_ID";
    const string UE_ID = "UE_ID";
    const string GENDER_ID = "GENDER_ID";
    const string FID_ID = "FID_ID";
    const string GID_ID = "GID_ID";
    const string FB_TOKEN_ID = "FB_TOKEN_ID";
    const string PN_ID = "PN_ID";
    const string PP_ID = "PP_ID";
    const string DET_ID = "DET_ID";
    const string CC_ID = "CC_ID";
    const string RFC_ID = "RFC_ID";
    const string DID_ID = "DID_ID";
    const string FCM_TOKEN_ID = "FCM_TOKEN_ID";
    const string ISRATE_ID = "ISRATE_ID";
    const string COMMUNITY_JOIN_ID = "COMMUNITY_JOIN_ID";
    const string FULL_ADS_ID = "FULL_ADS_ID";
    const string UNIQUE_ID_ID = "UNIQUE_ID_ID";
    const string CD_ID = "CD_ID";
    const string GOLD_ID = "GOLD_ID";
    const string GEMS_ID = "GEMS_ID";
    const string LEAF_ID = "LEAF_ID";
    const string RFLCODE_ID = "RFLCODE_ID";
    const string NORMALENTRYGOLD_ID = "NORMALENTRYGOLD_ID";
    const string NOTESGAME = "NOTESGAME";
    const string CANDYGAME = "CANDYGAME";
    const string REDSTONE = "REDSTONE";
    const string GREENSTONE = "GREENSTONE";
    const string BLUESTONE = "BLUESTONE";
    const string CHALLANGE = "CHALLANGE";
    const string PURCHASEAD = "PURCHASEAD";
    const string SOUND = "SOUDN";
    const string VIBRATE = "VIBRATE";
    const string NOTI = "NOTI";
    const string CH = "CH";
    const string DECKIMG = "DeckImg";
    const string FRAMEIMG = "FrameImg";
    public static string AID = "AID";
    public static string THEME = "THEME";
    const string CURRENTPORT = "CURRENTPORT";
    const string TOTALSHOWEDINTERADS = "TOTALINTERADS";
    const string TOTALSHOWEDREWARDADS = "TOTALREWARDADS";
    const string TOTALGAMEPLAY = "TOTALGAMEPLAYED";

    const string SpcificOpStepCount = "SPCIFICOPSTEPCOUNT";
    const string SpcificPlayerStepCount = "SpcificPlayerStepCount";
    const string CycloneStepCount = "CycloneStepCount";
    const string ShieldStepCount = "ShieldStepCount";
    const string HighFiveStep = "HighFiveStep";
    const string WildUpStep = "WildUpStep";
    const string WildUpStep2 = "WildUpStep2";
    const string ZeroCounter = "ZeroCounter";
    const string SevenCounter = "SevenCounter";
    const string SevenCounter2 = "SevenCounter2";

    const string USERFIRSTTIMEOPEN = "USERFIRSTTIMEOPEN";

    const string COLORCARDCHANGE = "COLORCARDCHANGE";
    const string MATCHNUMBERSTATUS = "MATCHNUMBERSTATUS";
    const string PICKSTATUS = "PICKSTATUS";
    const string PLUSONESHOW = "PLUSONESHOW";
    const string FOURPLUSPENALTY = "FOURPLUSPENALTY";
    const string FOURPLUSOWNSHOW = "FOURPLUSOWNSHOW";
    const string COLORCHOOSESHOW = "COLORCHOOSESHOW";
    const string DISCARDALLSHOW = "DISCARDALLSHOW";
    const string CLICKSWIPE = " CLICKSWIPE";

    const string SKIPSHOW = " SKIPSHOW";
    const string REVERSESHOW = " REVERSESHOW";
    const string TWOPLUSSHOW = " TWOPLUSSHOW";

    static string purchaseStoreNode = "StoreNode";
    static string DID1 = "DID";

    public static int userFirstTimeOpen
    {
        get => PlayerPrefs.GetInt(USERFIRSTTIMEOPEN);
        set => PlayerPrefs.SetInt(USERFIRSTTIMEOPEN, value);
    }

    public static int spcificOpStepCount
    {
        get => PlayerPrefs.GetInt(SpcificOpStepCount);
        set => PlayerPrefs.SetInt(SpcificOpStepCount, value);
    }
    public static int spcificPlayerStepCount
    {
        get => PlayerPrefs.GetInt(SpcificPlayerStepCount);
        set => PlayerPrefs.SetInt(SpcificPlayerStepCount, value);
    }
    public static int cycloneStepCount
    {
        get => PlayerPrefs.GetInt(CycloneStepCount);
        set => PlayerPrefs.SetInt(CycloneStepCount, value);
    }
    public static int highFiveStep
    {
        get => PlayerPrefs.GetInt(HighFiveStep);
        set => PlayerPrefs.SetInt(HighFiveStep, value);
    }
    public static int shieldStepCount
    {
        get => PlayerPrefs.GetInt(ShieldStepCount);
        set => PlayerPrefs.SetInt(ShieldStepCount, value);
    }
    public static int wildUpStep
    {
        get => PlayerPrefs.GetInt(WildUpStep);
        set => PlayerPrefs.SetInt(WildUpStep, value);
    }
    public static int wildUpStep2
    {
        get => PlayerPrefs.GetInt(WildUpStep2);
        set => PlayerPrefs.SetInt(WildUpStep2, value);
    }
    public static int zeroCounter
    {
        get => PlayerPrefs.GetInt(ZeroCounter);
        set => PlayerPrefs.SetInt(ZeroCounter, value);
    }
    public static int sevenCounter
    {
        get => PlayerPrefs.GetInt(SevenCounter);
        set => PlayerPrefs.SetInt(SevenCounter, value);
    }
    public static int sevenSelectCounter
    {
        get => PlayerPrefs.GetInt(SevenCounter2);
        set => PlayerPrefs.SetInt(SevenCounter2, value);
    }

    public static int colorChangeStatus
    {
        get => PlayerPrefs.GetInt(COLORCARDCHANGE);
        set => PlayerPrefs.SetInt(COLORCARDCHANGE, value);
    }

    public static int matchNumberStatus
    {
        get => PlayerPrefs.GetInt(MATCHNUMBERSTATUS);
        set => PlayerPrefs.SetInt(MATCHNUMBERSTATUS, value);
    }

    public static int cardPicStatus
    {
        get => PlayerPrefs.GetInt(PICKSTATUS);
        set => PlayerPrefs.SetInt(PICKSTATUS, value);
    }

    public static int clickSwipeStatus
    {
        get => PlayerPrefs.GetInt(CLICKSWIPE);
        set => PlayerPrefs.SetInt(CLICKSWIPE, value);
    }



    public static int skipShow
    {
        get => PlayerPrefs.GetInt(SKIPSHOW);
        set => PlayerPrefs.SetInt(SKIPSHOW, value);
    }
    public static int reverseShow
    {
        get => PlayerPrefs.GetInt(REVERSESHOW);
        set => PlayerPrefs.SetInt(REVERSESHOW, value);
    }

    public static int twoPlusShow
    {
        get => PlayerPrefs.GetInt(TWOPLUSSHOW);
        set => PlayerPrefs.SetInt(TWOPLUSSHOW, value);
    }

    public static int plusOneShow
    {
        get => PlayerPrefs.GetInt(PLUSONESHOW);
        set => PlayerPrefs.SetInt(PLUSONESHOW, value);
    }    

    public static int fourPlusPenalty
    {
        get => PlayerPrefs.GetInt(FOURPLUSPENALTY);
        set => PlayerPrefs.SetInt(FOURPLUSPENALTY, value);
    }

    public static int fourPlusOwnShow
    {
        get => PlayerPrefs.GetInt(FOURPLUSOWNSHOW);
        set => PlayerPrefs.SetInt(FOURPLUSOWNSHOW, value);
    }
    
    public static int colorChooseShow
    {
        get => PlayerPrefs.GetInt(COLORCHOOSESHOW);
        set => PlayerPrefs.SetInt(COLORCHOOSESHOW, value);
    }
    
    public static int discardAllShow
    {
        get => PlayerPrefs.GetInt(DISCARDALLSHOW);
        set => PlayerPrefs.SetInt(DISCARDALLSHOW, value);
    }




    public static string DeviceId
    {
        get => PlayerPrefs.GetString(DID1);
        set => PlayerPrefs.SetString(DID1, value);
    }

    public static string _purchaseNodeData
    {
        get => PlayerPrefs.GetString(purchaseStoreNode);
        set => PlayerPrefs.SetString(purchaseStoreNode, value);
    }

    public static string _ID
    {
        get => PlayerPrefs.GetString(_ID_ID);
        set => PlayerPrefs.SetString(_ID_ID, value);
    }

    //player id
    public static string PLAYER_ID
    {
        get => PlayerPrefs.GetString(PLAYERID_ID);
        set => PlayerPrefs.SetString(PLAYERID_ID, value);
    }

    //user login test - karel 6 ke nahi jo login karel hoy to direct dashboard nakar login panel.
    public static string ULT
    {
        get => PlayerPrefs.GetString(ULT_ID);
        set => PlayerPrefs.SetString(ULT_ID, value);
    }

    public static string UE
    {
        get => PlayerPrefs.GetString(UE_ID);
        set => PlayerPrefs.SetString(UE_ID, value);
    }

    //gender
    public static string GENDER
    {
        get => PlayerPrefs.GetString(GENDER_ID);
        set => PlayerPrefs.SetString(GENDER_ID, value);
    }

    //facebook id
    public static string FID
    {
        get => PlayerPrefs.GetString(FID_ID);
        set => PlayerPrefs.SetString(FID_ID, value);
    }

    //google id
    public static string GID
    {
        get => PlayerPrefs.GetString(GID_ID);
        set => PlayerPrefs.SetString(GID_ID, value);
    }

    //facebook token number
    public static string FB_TOKEN
    {
        get => PlayerPrefs.GetString(FB_TOKEN_ID);
        set => PlayerPrefs.SetString(FB_TOKEN_ID, value);
    }

    //player name or user name
    public static string PN
    {
        get => PlayerPrefs.GetString(PN_ID);
        set => PlayerPrefs.SetString(PN_ID, value);
    }

    //profile picture
    public static string PP
    {
        get => PlayerPrefs.GetString(PP_ID);
        set => PlayerPrefs.SetString(PP_ID, value);
    }

    //device check android or ios
    public static string DET
    {
        get => PlayerPrefs.GetString(DET_ID);
        set => PlayerPrefs.SetString(DET_ID, value);
    }

    //country code
    public static string CC
    {
        get => PlayerPrefs.GetString(CC_ID);
        set => PlayerPrefs.SetString(CC_ID, value);
    }

    //referal code
    public static string RFC
    {
        get => PlayerPrefs.GetString(RFC_ID);
        set => PlayerPrefs.SetString(RFC_ID, value);
    }

    //Device Unique Indefier  
    public static string DID
    {
        get => PlayerPrefs.GetString(DID_ID);
        set => PlayerPrefs.SetString(DID_ID, value);
    }

    //firebase token id  
    public static string FCM_TOKEN
    {
        get => PlayerPrefs.GetString(FCM_TOKEN_ID);
        set => PlayerPrefs.SetString(FCM_TOKEN_ID, value);
    }

    //is rate api didha ke.  
    public static string ISRATE
    {
        get => PlayerPrefs.GetString(ISRATE_ID);
        set => PlayerPrefs.SetString(ISRATE_ID, value);
    }

    //false  
    public static string COMMUNITY_JOIN
    {
        get => PlayerPrefs.GetString(COMMUNITY_JOIN_ID);
        set => PlayerPrefs.SetString(COMMUNITY_JOIN_ID, value);
    }

    //full ads counter 
    public static string FULL_ADS
    {
        get => PlayerPrefs.GetString(FULL_ADS_ID);
        set => PlayerPrefs.SetString(FULL_ADS_ID, value);
    }

    //unique_id top ma dekhay te dashboard ma
    public static string UNIQUE_ID
    {
        get => PlayerPrefs.GetString(UNIQUE_ID_ID);
        set => PlayerPrefs.SetString(UNIQUE_ID_ID, value);
    }

    //cd - 
    public static string CD
    {
        get => PlayerPrefs.GetString(CD_ID);
        set => PlayerPrefs.SetString(CD_ID, value);
    }

    //game ketla gold 6 te show thase
    public static string GOLD
    {
        get => PlayerPrefs.GetString(GOLD_ID);
        set => PlayerPrefs.SetString(GOLD_ID, value);
    }

    //game ketla jems 6 te show thase
    public static string GEMS
    {
        get => PlayerPrefs.GetString(GEMS_ID);
        set => PlayerPrefs.SetString(GEMS_ID, value);
    }

    public static string LEAF
    {
        get => PlayerPrefs.GetString(LEAF_ID);
        set => PlayerPrefs.SetString(LEAF_ID, value);
    }

    public static string RFLCODE
    {
        get => PlayerPrefs.GetString(RFLCODE_ID);
        set => PlayerPrefs.SetString(RFLCODE_ID, value);
    }

    public static string NORMALENTRYGOLD
    {
        get => PlayerPrefs.GetString(NORMALENTRYGOLD_ID);
        set => PlayerPrefs.SetString(NORMALENTRYGOLD_ID, value);
    }

    public static string NOTES
    {
        get => PlayerPrefs.GetString(NOTESGAME);
        set => PlayerPrefs.SetString(NOTESGAME, value);
    }

    public static string CANDY
    {
        get => PlayerPrefs.GetString(CANDYGAME);
        set => PlayerPrefs.SetString(CANDYGAME, value);
    }

    public static string RED
    {
        get => PlayerPrefs.GetString(REDSTONE);
        set => PlayerPrefs.SetString(REDSTONE, value);
    }

    public static string GREEN
    {
        get => PlayerPrefs.GetString(GREENSTONE);
        set => PlayerPrefs.SetString(GREENSTONE, value);
    }

    public static string BLUE
    {
        get => PlayerPrefs.GetString(BLUESTONE);
        set => PlayerPrefs.SetString(BLUESTONE, value);
    }

    public static string CHALLANGES
    {
        get => PlayerPrefs.GetString(CHALLANGE);
        set => PlayerPrefs.SetString(CHALLANGE, value);
    }

    public static int PURCHASEADS
    {
        get => PlayerPrefs.GetInt(PURCHASEAD);
        set => PlayerPrefs.SetInt(PURCHASEAD, value);
    }

    public static string Themes
    {
        get => PlayerPrefs.GetString(THEME);
        set => PlayerPrefs.SetString(THEME, value);
    }

    public static int Sound
    {
        get => PlayerPrefs.GetInt(SOUND);
        set => PlayerPrefs.SetInt(SOUND, value);
    }

    public static int Vibrate
    {
        get => PlayerPrefs.GetInt(VIBRATE);
        set => PlayerPrefs.SetInt(VIBRATE, value);
    }

    public static int Noti
    {
        get => PlayerPrefs.GetInt(NOTI);
        set => PlayerPrefs.SetInt(NOTI, value);
    }

    public static int Ch
    {
        get => PlayerPrefs.GetInt(CH);
        set => PlayerPrefs.SetInt(CH, value);
    }

    public static string DeckImage
    {
        get => PlayerPrefs.GetString(DECKIMG);
        set => PlayerPrefs.SetString(DECKIMG, value);
    }

    public static string FrameImage
    {
        get => PlayerPrefs.GetString(FRAMEIMG);
        set => PlayerPrefs.SetString(FRAMEIMG, value);
    }

    public static string CurrentPort
    {
        get => PlayerPrefs.GetString(CURRENTPORT, "1303");
        //get => PlayerPrefs.GetString(CURRENTPORT, "1302");
        set => PlayerPrefs.SetString(CURRENTPORT, value);
    }

    public static int ManageInternADS
    {
        get => PlayerPrefs.GetInt(TOTALSHOWEDINTERADS, 0);
        set => PlayerPrefs.GetInt(TOTALSHOWEDINTERADS, value);
    }

    public static int ManageRewardADS
    {
        get => PlayerPrefs.GetInt(TOTALSHOWEDREWARDADS, 0);
        set => PlayerPrefs.GetInt(TOTALSHOWEDREWARDADS, value);
    }

    public static int ManageGameplay
    {
        get => PlayerPrefs.GetInt(TOTALGAMEPLAY, 0);
        set => PlayerPrefs.GetInt(TOTALGAMEPLAY, value);
    }

}
