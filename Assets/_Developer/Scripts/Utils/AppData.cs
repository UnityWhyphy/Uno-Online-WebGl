using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UniGif;

//[System.Serializable]
public static class AppData
{
    public static List<string> interAdsId = new List<string>();
    public static List<string> rewardAdsId = new List<string>();
    public static List<float> rules = new List<float>();


    public const string CustomKey = "Custom";
    public const string InvertKey = "invert";
    public const string LiveTableKey = "Live_Table";
    public const string TournamentKey = "Tournament";

    public static string PURCHASEDID = "";
    public static string NOTIiDS = "";
    public static int IsExtra = 0;
    public static long TimeReset = 0;
    public static long ResetTimer = 0;
    public static long NormalEntryGold = 0;
    public static long NormalEntryGems = 0;

    public static bool istreassureFeed = true;
    public static bool istourShowFeed = true;
    public static int gameStartCount = 0;
    public static int totalPlayedMatch = 0;
    public static int isReviewAd = 4;
    public static bool isLevelWiseLockFirebase;
    public static bool isTutorialPlay = false;
    public static bool isFirstTimeUSerEnter = false;
    public static bool isTutorialFirstTimeReward = false;
    public static bool handleLeaveTable = true;
    public static bool isMannualDisConnect = false;
    public static JSONNode configData;
    public static string faceBookLink, instagramLink;
    public static int FB_GOLD, GUEST_GOLD;

    public static string PARTNER = "PARTNER", CLASSIC = "CLASSIC", EMOJISOLO = "EMOJISOLO", EMOJIPARTNER = "EMOJIPARTNER", EMOJI = "EMOJI", PRIVATE = "PRIVATE", FLIP = "INVERT";
    public static bool closePlaying = false;

    public static JSONObject BackToLobbyJason;

    public static bool fromChestSurprize, fromChest, fromChestUnlock = false;

    public static List<string> UpdateNEwVesrion = new List<string>();
    //Dashboard Video Reward
    public static long VIDEOREWARDCOINS = 0;
    public static int VIDEOBONUS = 0;
    public static long USERGOLDRECOVER = 0;
    public static float videoRewardTimer = 0f;

    public static List<string> PlayingDefaultMsg = new List<string>();

    // mini game
    public static int PlayedMiniGame;
    public static int FreeMiniGame;
    public static int VideoTimer = 0;
    public static bool fromMiniGame;

    //star player    
    public static string BU_PROFILE_URL;

    public static int CURRENTUSERVERSION;
    public static bool Iamhere = false;

    //DailyBonus
    public static bool IsShowDailyBonus = false;
    public static bool IsLogfileUpload = false;
    public static bool IsLogSp = false;
    public static bool IsVideoConnectShow = false;
    public static List<int> FCCount = new List<int>();
    public static List<int> FCBonus = new List<int>();
    public static List<int> DayBonus = new List<int>();
    public static List<int> MainSppiner = new List<int>();
    public static List<string> MainSppinerStoneName = new List<string>();


    public static bool isVideoClose = false;

    //Hourly Bonus
    public static bool isShowHourlyBonus = false;
    public static long hourlyBonusGold = 0;
    public static float videoHourlyBonusGold = 0;

    public static bool fromMagicBox = true;
    public static bool FromPlaying, Gemsflag = false;

    //Magic Bonus
    public static float magicBonusTime = 0;

    //Maintance Screen
    public static bool ShowToltip = false;
    public static bool Flag = false;
    public static bool IsRejoin = false;
    public static bool Connect = true;
    public static bool EGFlag = false;

    //RewardPanel
    public static bool isRewardAvailable = true;

    //Ads flag

    public static float aDShowingTimer = 10;
    public static bool FromShowIntro = false, IsCanShowAd = true;
    public static int isShownAdsFrom = -1;
    public static string notiRewardType = "", notiId = "";
    public static long notiRewardVal = 0;

    public static JSONNode SignUpData = "", GTIDATA = "", TournamentData = "";
    public static bool SingleRound = false, IsAdsopen = false, IsRewardAdOpen = false, HandleAdsEvent = true;

    public static string REFRELCODEOTHERUSER, FBPROMOCODE;
    public static string PrivacyLink = "", TermsLink = "";

    public static int NCCount = 0, URMCCount = 0;

    //ChallengePopUp
    public static bool canShowChallenge = true;

    //MBV
    public static List<TableBootValue> TableSloteInGame = new List<TableBootValue>();

    //PML
    public static List<string> chatSuggetions = new List<string>();

    //MaintainenceMode
    public static bool IsMaintance = false;
    public static float maintenenceStartsAfter = 0, maintenenceEndAfter = 0;

    //Delete Account
    public static string deleteAccountUrl = "";

    //Win Loss Screen
    public static long winLossCoins = 0;

    public static string promoflag = "";
    public static string promoType = "";

    public static bool isInstantLeave;

    //Temp Variable
    public static string REMOVABLE_NOTIID = "";

    //KnockKnock PopUp
    public static float knockKnockTimer = 0;

    //Offer Dialog Timer
    public static float remaining_LTOTime = 0;

    public static int currantLvl = 0;
    public static int AppVersionAndroid = 8, AppVersionIOS = 2;


    public static Action AllPanelClose;


    public static string GetFilpName(string card, bool isFlipSide)
    {
        if (card.Contains("*"))
        {
            string[] parts = card.Split(new char[] { '*' }, 2); // Limit to 2 parts

            return isFlipSide ? parts[1] : parts[0];
        }
        else
        {
            return card;
        }
    }


    public static void DoubleXpDialog(bool isSpecial, string Feature)
    {
        //Game_WinnerScreen.instance.InstnatDisTxt.text = "You need " + (RequiredXp - GetCurrentXp) + "XP" + " to unlock " + Feature + ",Watch video you will get instant unlock " + Feature + ".";
        if (isSpecial)
        {

            switch (Feature)
            {
                case "Wild Card":
                    break;

                case "Point Gap":
                    break;

                case "Live table":
                    break;

                case "Tournament":
                    break;

                case "Star player":
                    break;

                case "Laboratory":
                    break;

                case "House of luck":
                    break;

                case "Central Museum":
                    break;
            }
        }
    }

    public static IEnumerator ProfilePicSet(string url, RawImage rawImage)
    {
        if (string.IsNullOrEmpty(url))
        {
            yield break; // Exit early if URL is null or empty
        }

        string profilePicURL = url;

        if (!url.Contains("http"))
        {
            profilePicURL = AppData.BU_PROFILE_URL + url;
        }

        Logger.Print($"_islogupload VVV= profilePicURL {profilePicURL} ");

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(profilePicURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Logger.Error($"_islogupload profilePicURL {request.result} ");
                yield break; // Exit if request failed
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            if (rawImage != null)
            {
                byte[] textureBytes = texture.EncodeToPNG();
                string Profile_Image_string = Convert.ToBase64String(textureBytes);
                rawImage.texture = GetTexturepp(Profile_Image_string); // Assuming GetTexture is a method that converts base64 string to texture
            }
        }
    }

    private static Texture2D GetTexturepp(string base64String)
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }

    public static IEnumerator SpriteSetFromURL(string url, Image image, string mName)
    {
        // Construct the full URL if needed
        if (url == null) yield break;

        string profilePicURL = url.StartsWith("http") ? url : AppData.BU_PROFILE_URL + url;

        if (string.IsNullOrEmpty(profilePicURL))
        {
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(profilePicURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download image. Status: {request.result}. URL: {profilePicURL}");
                yield break;
            }

            // Extract texture from response
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (texture == null)
            {
                Debug.LogError("Failed to create texture from response.");
                yield break;
            }

            // Create and assign sprite          
            try
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                if (image != null)
                {
                    image.sprite = sprite;
                }
            }
            catch (Exception e)
            {
                JSONNode data = new JSONObject
                {
                    ["url"] = url,
                    ["imageNull"] = image == null,
                    ["Methode"] = mName
                };
                Loading_screen.instance.SendExe("Appdata", "SpriteSetFromURL", data, e);
            }
        }
    }

    public static string numDifferentiation(long val)
    {
        try
        {
            double d = 0;
            string iteration = null;
            string returnVal = "";
            if (val >= 1000000000000L)
            {
                d = ((double)val) / 1000000000000.0d;
                iteration = "T";
            }
            else if (val >= 1000000000L)
            {

                d = ((double)val) / 1000000000.0d;
                iteration = "B";


            }
            else if (val >= 1000000L)
            {
                d = ((double)val) / 1000000.0d;
                iteration = "M";
            }
            else if (val < 1000)
            {

                return val.ToString();
            }


            if (d != 0 && (d * 10.0d) % 10.0d == 0.0d)
            {
                // return ((d * 10.0d) / 10.0d) + " " + iteration;
                return ((d * 10.0d) / 10.0d) + iteration;

            }
            else if (d == 0)
            {
                return FormatNum((int)val);
            }

            //return RoundTo2Decimals(d) + " " + iteration;
            return RoundTo2Decimals(d) + iteration;
        }
        catch (Exception e)
        {
            Logger.Print("Biolon ma ave " + e);
            return val.ToString();
        }
    }

    public static string PrivateTableBootVal(long val)
    {
        try
        {
            double d = 0;
            string iteration = null;
            string returnVal = "";
            if (val >= 1000000000000L)
            {
                d = ((double)val) / 1000000000000.0d;
                iteration = "T";
            }
            else if (val >= 1000000000L)
            {

                d = ((double)val) / 1000000000.0d;
                iteration = "B";
            }
            else if (val >= 1000000L)
            {
                d = ((double)val) / 1000000.0d;
                iteration = "M";
            }
            else if (val >= 1000)
            {
                d = ((double)val) / 1000.0d;
                iteration = "K";
            }
            else if (val < 1000)
            {
                return val.ToString();
            }


            if (d != 0 && (d * 10.0d) % 10.0d == 0.0d)
            {
                //return ((d * 10.0d) / 10.0d) + " " + iteration;
                return ((d * 10.0d) / 10.0d) + iteration;

            }
            else if (d == 0)
            {
                return FormatNum((int)val);
            }

            return RoundTo2Decimals(d) + iteration;
            //  return RoundTo2Decimals(d) + " " + iteration;
        }
        catch (Exception e)
        {
            Logger.Print("Biolon ma ave " + e);
            return val.ToString();
        }
    }

    public static string FormatNum(int val)
    {
        return val.ToString("C", new CultureInfo("en-US")).Replace("$", "").Replace(".00", "");
    }

    public static double RoundTo2Decimals(double val)
    {
        return double.Parse(val.ToString("C", new CultureInfo("en-US")).Replace("$", "").Replace(".00", ""));
    }

    public static string GetTimeInFormateHr(long time)
    {
        long second = time / 1000;
        long sec = second % 60;
        string m = getinNumFormat((second / 60) % 60);
        string h = getinNumFormat(second / 3600);
        return h + ":" + m + ":" + getinNumFormat(sec);
    }
    public static string GetTimeInFormateMin(long time)
    {
        long second = time / 1000;
        long sec = second % 60;
        string m = getinNumFormat(second / 60);
        return m + ":" + getinNumFormat(sec);
    }

    public static string getTimeInHourNew(long time)
    {
        string h = getinNumFormat(time / 3600);
        return h.Equals("00") ? "" : h + "H";
    }

    private static string getinNumFormat(long num)
    {
        if (num < 10)
        {
            return "0" + num;
        }
        return "" + num;
    }
}
