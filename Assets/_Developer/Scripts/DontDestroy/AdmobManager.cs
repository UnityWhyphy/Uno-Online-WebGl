using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using System;
using System.Net.Sockets;


public class AdmobManager : MonoBehaviour
{
    public static string TAG = ">>>VIDEOADS ";
    public static AdmobManager instance;

    //public BannerView _bannerView;
    //public InterstitialAd interstitialAd, splashAds;
    //public RewardedAd rewardedAd;
    //public AppOpenAd appOpenAd;

    public static bool isInterstitialLoad, isSplashLoad, goForward;
    public static bool isRewardLoad, isVideoRewaded = false, isTest = false;
    public static bool isAdmobInit;
    private DateTime _expireTime;

    #region Awake
    private void Awake()
    {
        Logger.Print(TAG + " AdmobManager Awake called");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        AdmobInit();
    }

    public void AdmobInit()
    {
//#if UNITY_ANDROID
//        MobileAds.Initialize((InitializationStatus initStatus) =>
//        {
//            Debug.Log(" === Successfully Integrated Admob ===");
//            isAdmobInit = true;
//            LoadSplashAds(PrefrenceManager.FULL_ADS);
//        });
//#endif
    }

    public void InitAds()
    {
        Logger.Print(TAG + " isAdmobInit " + isAdmobInit);
        if (isAdmobInit)
        {
            LoadInterstitialAd(AppData.interAdsId[0], 0);
            LoadRewardedAd(AppData.rewardAdsId[0], 0);
        }
    }

    private void Start()
    {
#if UNITY_IOS
        LoadSplashAds(isTest ? "ca-app-pub-3940256099942544/1033173712" : PrefrenceManager.FULL_ADS);
#endif
    }
    #endregion

    #region Interstitial

    public void LoadInterstitialAd(string id, int i)
    {
        //if (interstitialAd != null)
        //{
        //    interstitialAd.Destroy();
        //    interstitialAd = null;
        //}

        //var adRequest = new AdRequest();

        //InterstitialAd.Load(id, adRequest,
        //      (InterstitialAd ad, LoadAdError error) =>
        //      {
        //          if (error != null || ad == null)
        //          {
        //              Logger.Print(TAG + " interstitial ad failed to load an ad " +
        //                             "with error : " + error + " " + id + " || " + isInterstitialLoad);
        //              isInterstitialLoad = false;
        //              return;
        //          }

        //          Debug.Log("Interstitial ad loaded with response : "
        //                    + ad.GetResponseInfo());

        //          isInterstitialLoad = true;
        //          interstitialAd = ad;
        //          RegisterEventHandlers(ad, i);
        //      });
    }

    public void ShowInterstitialAd()
    {
        //remove ads purchase hoi to ads nai devani

        //if (PrefrenceManager.PURCHASEADS == 1)
        //{
        //    PerformActionAfterInterCompleted();
        //    return;
        //}

        //if (isInterstitialLoad && interstitialAd != null)
        //{
        //    Logger.Print(TAG + " Showing interstitial ad.");
        //    interstitialAd.Show();
        //}
        //else
        //{
        //    PerformActionAfterInterCompleted();
        //    LoadInterstitialAd(AppData.interAdsId[0], 0);
        //}
    }

 /*   private void RegisterEventHandlers(InterstitialAd ad, int i)
    {
        Logger.Print("Interstitial RegisterEventHandlers.");
        ad.OnAdPaid += (AdValue adValue) =>
        {
        };

        ad.OnAdImpressionRecorded += () =>
        {

        };

        ad.OnAdClicked += () =>
        {

        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Logger.Print("Interstitial ad full screen content opened.");
            isInterstitialLoad = false;
            interstitialAd = null;
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Logger.Print(TAG + "Interstitial OnAdFullScreenContentClosed called ");
            PerformActionAfterInterCompleted();
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Logger.Print(TAG + " OnAdFullScreenContentFailed called " + error);

            if (i + 1 < AppData.interAdsId.Count)
                LoadInterstitialAd(AppData.interAdsId[i + 1], i + 1);
        };
    }
 */
    private void PerformActionAfterInterCompleted()
    {
        Logger.Print(TAG + "Inter Ad Fully Watched & Doing Further Action >> ");
        Logger.Print($"{TAG} PerformActionAfterInterCompleted = AppData.winLossCoins = {AppData.winLossCoins} ||AppData.isShowHourlyBonus = {AppData.isShowHourlyBonus} |AppData.IsShowDailyBonus = {AppData.IsShowDailyBonus}");
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (SceneManager.GetActiveScene().name.Equals(EventHandler.DASHBOARD))
            {
                if (AppData.winLossCoins != 0)
                    WinLossScreen.instance.ShowWinLossPanel();

                else if (AppData.IsShowDailyBonus) //true hovu joye
                {
                    Logger.Print($"AA");
                    EventHandler.MakeDailyBonus();
                }

                //else if (AppData.isShowHourlyBonus)//1 aavu joye
                //{
                //    Logger.Print($"BB");
                //    //HourlyBonus.instance.ShowHourlyBonus(AppData.hourlyBonusGold, AppData.videoHourlyBonusGold);
                //}

                else if (StaticData.LEVELUPDATA != "")
                    LevelUpPanel.instance.ShowLevelUpPanel();

                else
                {
                    Logger.Print($"AppData.winLossCoins= {(AppData.winLossCoins == 0 && AppData.isInstantLeave)}");
                    if (AppData.winLossCoins == 0 && AppData.isInstantLeave)
                    {
                        AppData.isInstantLeave = false;
                        WinLossScreen.instance.ScratchCardScreenPanel();
                    }
                    else
                    {
                        if (!GameWinner.instance.WinnerPannel.activeInHierarchy)
                            DashboardManager.instance.MatchCountShowOffer();
                    }
                    if (AppData.isInstantLeave) AppData.isInstantLeave = false;
                    RewardPanel.instance.ShowRewardPopUp(AppData.SignUpData["reward"]);
                }
            }
            Loading_screen.instance.ShowLoadingScreen(false);
            AppData.IsAdsopen = false;
            LoadInterstitialAd(AppData.interAdsId[0], 0);
        });
    }

    #endregion

    #region REWARD

    public void LoadRewardedAd(string adId, int i)
    {
        //if (rewardedAd != null)
        //{
        //    rewardedAd.Destroy();
        //    rewardedAd = null;
        //}

        //Logger.Print(TAG + " Loading the rewarded ad.");

        //var adRequest = new AdRequest();

        //RewardedAd.Load(adId, adRequest,
        //    (RewardedAd ad, LoadAdError error) =>
        //    {
        //        if (error != null || ad == null)
        //        {
        //            isRewardLoad = false;
        //            Logger.Print(TAG + " Rewarded ad failed to load an ad " +
        //                           "with error : " + error + " " + adId + " || " + isRewardLoad);
        //            return;
        //        }

        //        Logger.Print(TAG + "Rewarded ad loaded with response : " + ad.GetResponseInfo());

        //        rewardedAd = ad;
        //        RegisterEventHandlers(ad, i);
        //        isRewardLoad = true;
        //    });
    }

    public void ShowRewardedAd()
    {
        //Logger.Print($"AppData.IsCanShowAd {AppData.IsCanShowAd}");
        //if (!AppData.IsCanShowAd)
        //{
        //    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.alert, "Video Will be available after 10 sec.");
        //    return;
        //}

        //if (isRewardLoad && rewardedAd != null)
        //{
        //    rewardedAd.Show((Reward reward) =>
        //    {
        //        Logger.Print(TAG + " User Geted Reward Of Seen Video");
        //    });
        //}
        //else
        //{
        //    AllCommonGameDialog.instance.SetJustOkDialogData("Aleart!!!", "Video Not Loaded, Please Try After Sometime.");

        //    AppData.notiId = AppData.notiRewardType = "";
        //    AppData.notiRewardVal = 0;
        //    AppData.isShownAdsFrom = -1;

        //    AppData.IsRewardAdOpen = false;
        //    LoadRewardedAd(AppData.rewardAdsId[0], 0);
        //}
    }

  /*  private void RegisterEventHandlers(RewardedAd ad, int i)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            isVideoRewaded = true;
            Logger.Print(TAG + " OnAdPaid called");//
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Logger.Print(TAG + " OnAdImpressionRecorded Impression");
        };

        ad.OnAdClicked += () =>
        {
            Logger.Print(TAG + " OnAdClicked click");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Logger.Print(" OnAdFullScreenContentOpened called ");//
            AppData.IsRewardAdOpen = true;
            AppData.isVideoClose = true;
            PrefrenceManager.ManageRewardADS += 1;
            switch (PrefrenceManager.ManageRewardADS)
            {
                case 5:
                case 10:
                case 15:
                case 25:
                case 40:
                case 50:
                    break;
            }
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Logger.Print(TAG + " OnAdFullScreenContentClosed called. " + isVideoRewaded);
            Loading_screen.instance.LoaderPanel.SetActive(false);
            AppData.IsRewardAdOpen = false;

            Invoke(nameof(GetRewardOfShowedRewardVideo), 1);

            isVideoRewaded = isRewardLoad = false;
            LoadRewardedAd(AppData.rewardAdsId[0], 0);

            InvokeRepeating(nameof(HandleShowingAdTimer), 0, 1);
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Logger.Print(TAG + " OnAdFullScreenContentFailed called");

            if (i + 1 < AppData.rewardAdsId.Count)
                LoadRewardedAd(AppData.rewardAdsId[i + 1], i + 1);
        };
    }
  */
    public void GetRewardOfShowedRewardVideo()
    {
        Logger.Print(TAG + $" SocketManagergame.MainSocket = {SocketManagergame.Instance}");
        if (SocketManagergame.MainSocket == null) return;

        if (!SocketManagergame.MainSocket.IsOpen)
        {
            return;
        }
        Logger.Print($"{TAG} User Closed The Reward Ad & Getting reward " + AppData.isShownAdsFrom + " Socket open :: " + SocketManagergame.MainSocket.IsOpen);
        if (AppData.fromMiniGame)
        {
            Loading_screen.instance.LoaderPanel.SetActive(true);
            EventHandler.StartMiniGame(true);
            AppData.fromMiniGame = false;
        }

        switch (AppData.isShownAdsFrom)
        {
            case 0: //From Notification
                EventHandler.NotiVideoReward(AppData.notiRewardType, AppData.notiRewardVal);

                if (AppData.notiRewardType.Equals("Level up"))
                    LevelUpPanel.instance.LevelUpClick(2);
                else
                    EventHandler.RemoveNotificationData(AppData.notiId);
                break;

            case 1: // Magic Bonus
                EventHandler.CollectMagicBox();
                break;

            case 2: //2X Daily Bonus
                DailyBonus.instance.OnClick_BailyBonus(4);
                break;

            case 3:// Hourly Bonus
                EventHandler.CollectHourlyBonus(true);
                HourlyBonus.instance.CloseHourlyBonus();
                break;

            case 4: //Dashboard Watch Video
                AppData.videoRewardTimer = 60f;
                EventHandler.VideoReward();
                StorePanel.Instance.OnClick_StorePanel(9);
                WinLossScreen.instance.OnClick_WinLossPopup(0);
                break;

            case 5: //Scratch Watch Video
                WinLossScreen.instance.ScratchPopupOn();
                break;

            case 6:
                EventHandler.WatchVideoReward(AppData.VIDEOBONUS); // Send WatchVideo Reward
                break;

            case 7:
                EventHandler.WatchVideoReward(GameWinner.instance.isTournament ? (int)(GameWinner.instance.bv / 2) : Mathf.Abs((int)AppData.winLossCoins / 2), "isRecovery"); // Send WatchVideo Reward
                GameWinner.instance.LostGoldPopupStatus(false);
                break;
        }

        if (AppData.fromChest)
            SocketManagergame.ChestTimerRemove?.Invoke();

        else if (AppData.fromChestUnlock)
            SocketManagergame.ChestUnlockVideo?.Invoke();

        else if (AppData.fromChestSurprize)
            SocketManagergame.HandleSurPriseReward?.Invoke();

        else if (AppData.isShownAdsFrom != 5)
            AudioManager.instance.AudioPlay(AudioManager.instance.missionClaim);
        Loading_screen.instance.ShowLoadingScreen(false);

        AppData.notiRewardType = "";
        AppData.notiRewardVal = 0;
        AppData.notiId = "";
        AppData.isShownAdsFrom = -1;
        AppData.isVideoClose = AppData.IsRewardAdOpen = false;

        //UnityMainThreadDispatcher.Instance().Enqueue(() =>
        //{

        //});
    }

    #endregion

    #region SplashAds
    public void LoadSplashAds(string id)
    {
        Debug.Log($" === LoadSplashAds Admob === {id} <<");
        //if (PrefrenceManager.PURCHASEADS == 1 || splashAds != null || PrefrenceManager.userFirstTimeOpen == 0)
        //{
        //    goForward = true;
        //    splashAds?.Destroy();
        //    splashAds = null;

        //    if (PrefrenceManager.userFirstTimeOpen == 0) return;
        //}

        //var adRequest = new AdRequest();

        //InterstitialAd.Load(id, adRequest,
        //      (InterstitialAd ad, LoadAdError error) =>
        //      {
        //          Debug.Log($" === LoadSplashAds ad === {ad} <<");
        //          goForward = true;

        //          if (error != null || ad == null)
        //          {
        //              Logger.Print(TAG + " interstitial ad failed to load an ad " +
        //                             "with error : " + error + " || " + isSplashLoad);
        //              isSplashLoad = false;
        //              return;
        //          }
        //          isSplashLoad = true;
        //          splashAds = ad;

        //          Logger.Print(TAG + " Interstitial ad loaded with response : "
        //                    + ad.GetResponseInfo());
        //          RegisterEventHandlersSplash(ad);
        //      });
    }

    public void ShowSplashAds()
    {
        Logger.Print(TAG + " ShowSplash called = " + PrefrenceManager.PURCHASEADS);

        //if (PrefrenceManager.PURCHASEADS == 1)
        //{
        //    if (AppData.IsShowDailyBonus && !AppData.isVideoClose && AppData.isShownAdsFrom != 2)
        //        EventHandler.MakeDailyBonus();
        //    return;
        //}

        //Logger.Print($"{TAG} + Showing interstitial ad. {isSplashLoad} ");
        //if (isSplashLoad && splashAds != null)
        //{
        //    Logger.Print(TAG + " Showing interstitial ad.");
        //    splashAds.Show();
        //}
        //else
        //{
        //    //daily bonus no call marvano
        //    Logger.Print(TAG + " >> Splash Ad is Not Ready Yet >> ");
        //    if (AppData.IsShowDailyBonus && !AppData.isVideoClose && AppData.isShownAdsFrom != 2)
        //        EventHandler.MakeDailyBonus();
        //}
    }

 /*   private void RegisterEventHandlersSplash(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
        };

        ad.OnAdImpressionRecorded += () =>
        {

        };

        ad.OnAdClicked += () =>
        {

        };

        ad.OnAdFullScreenContentOpened += () =>
        {
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Logger.Print($"{TAG} OnAdFullScreenContentClosed called || AppData.IsShowDailyBonus = {!AppData.IsShowDailyBonus} | {AppData.isVideoClose}");
            //daily bonus no call marvano
            if (AppData.IsShowDailyBonus && !AppData.isVideoClose && AppData.isShownAdsFrom != 2)
                EventHandler.MakeDailyBonus();
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Logger.Print(TAG + " OnAdFullScreenContentFailed called " + error);
        };
    }*/
    #endregion

    int currentAdTimer = 0;
    public void HandleShowingAdTimer()
    {
        if (currentAdTimer >= AppData.aDShowingTimer)
        {
            Logger.Print($"{TAG} Now User Can Show Ads >> ");
            currentAdTimer = 0;
            AppData.IsCanShowAd = true;
            CancelInvoke("HandleShowingAdTimer");
        }
        else
        {
            Logger.Print($"{TAG} User Can Not Show Ads During Next {currentAdTimer}");
            AppData.IsCanShowAd = false;
            currentAdTimer++;
        }
    }
}

