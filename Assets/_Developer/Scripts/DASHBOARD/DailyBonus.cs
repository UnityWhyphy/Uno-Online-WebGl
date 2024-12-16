using DG.Tweening;
using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
    public static DailyBonus instance;
    private string TAG = ">>> DAILY BONUS ";
    [SerializeField] internal RectTransform dailyBonusPanel;
    [SerializeField] GameObject rewardpanel;
    [SerializeField] List<TextMeshProUGUI> fC, fC_B, day_B, spinner;
    [SerializeField] TextMeshProUGUI frdBonusTxt, dayTxt, dayBonusTxt, spinnerBonusTxt, totalBonusText;
    [SerializeField] Button spinBtn;
    [SerializeField] Transform wheelCircle, wheelCircle1, wheelCircle2;

    [Header("Double Reward Dialog")]
    [SerializeField] GameObject dBdialog;
    [SerializeField] Image dBBg;
    [SerializeField] Transform dBPopUp;
    [SerializeField] TextMeshProUGUI goldText, videoText, calculationTxt;

    bool _isSpinning = false;
    float spinDuration = 4.5f;
    int friend, day, spinnerIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_MDB += HandleDailyBonus;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_MDB -= HandleDailyBonus;
    }

    private void HandleDailyBonus(JSONNode data)
    {
        if (!AppData.IsShowDailyBonus) return;

        if (AppData.IsLogfileUpload)
        {
            LogServer.instance.UploadLogFile(LogServer.filename, AppData.BU_PROFILE_URL + "logUpload");
        }

        AppData.canShowChallenge = false;
        Logger.Print(TAG + "Handle Daily Bonus Called...");
        for (int i = 0; i < AppData.configData["FC"].Count; i++) fC[i].text = AppData.numDifferentiation(AppData.configData["FC"][i]);
        for (int i = 0; i < AppData.configData["FC_b"].Count; i++) fC_B[i].text = AppData.numDifferentiation(AppData.configData["FC_b"][i]);
        for (int i = 0; i < AppData.configData["day_b"].Count; i++) day_B[i].text = AppData.numDifferentiation(AppData.configData["day_b"][i]);
        for (int i = 0; i < AppData.configData["newspinner"].Count; i++) spinner[i].text = AppData.numDifferentiation(AppData.configData["newspinner"][i]["gold"]);

        frdBonusTxt.text = AppData.numDifferentiation(data["freind_bonus"]);
        dayTxt.text = "DAY " + data["day"];
        dayBonusTxt.text = AppData.numDifferentiation(data["day_bonus"]);
        spinnerBonusTxt.text = AppData.numDifferentiation(data["spinner_bonus"]);
        totalBonusText.text = AppData.numDifferentiation(data["total"]);
        goldText.text = AppData.numDifferentiation(data["total"]);
        videoText.text = AppData.numDifferentiation(data["vbonus"]);

        friend = data["friend"].AsInt;
        day = data["day"].AsInt;
        Logger.Print($"DAYY ::: {day}");
        spinnerIndex = data["spinner_index"];

        long totalReward = data["total"].AsLong + data["vbonus"].AsLong;
        calculationTxt.text = data["total"].ToString() + " + " + data["vbonus"].ToString() + " = " + AppData.numDifferentiation(totalReward);
        Loading_screen.instance.ShowLoadingScreen(false);
        AudioManager.instance.AudioPlay(AudioManager.instance.dailySpin);
        if (!dailyBonusPanel.gameObject.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(dailyBonusPanel, true);
    }

    public void FalseDailyBScreen()
    {
        CommanAnimations.instance.FullScreenPanelAnimation(dailyBonusPanel, false);
        CommanAnimations.instance.PopUpAnimation(dBdialog, dBBg, dBPopUp, Vector3.zero, false, false);
    }

    public void OnClick_BailyBonus(int i)
    {
        switch (i)
        {
            case 0: //Spin
                _isSpinning = false;
                TurnWheel();
                break;

            case 1: //Collect               
                CommanAnimations.instance.PopUpAnimation(dBdialog, dBBg, dBPopUp, Vector3.one, true);
                break;

            case 2://2X Close
                AppData.canShowChallenge = true;
                EventHandler.CollectDailyBonus(false);
                AppData.IsShowDailyBonus = false;
                AudioManager.instance.AudioPlay(AudioManager.instance.missionClaim);
                FalseDailyBScreen();

                if (CentralPurchase.FirstTimeOffer != null)
                    DashboardManager.instance.ShowWelcomeOffer();  // Welcome offer 

                OfferPopupController.offerBannerSlotCall?.Invoke();

                //day = 2;
                Logger.Print($"AppData.isReviewAd  = {AppData.isReviewAd} | day: {day}");
                if (day == 2 || day == 4 || day == 7)
                {
                    setting_script.instance.FeedbackPopupShow(true); // Daily bounce
                }

                if ((day == 3 || day == 6))
                {
                    StartCoroutine(AppReview.Instance.ReviewPopup());
                }
                day = 0;
                break;

            case 3: //2X Claim
                Loading_screen.instance.ShowLoadingScreen(true);
                AppData.isShownAdsFrom = 2;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 4: //After Show 2X Ad
                AppData.canShowChallenge = true;
                EventHandler.CollectDailyBonus(true);
                AppData.IsShowDailyBonus = false;
                FalseDailyBScreen();
                Loading_screen.instance.ShowLoadingScreen(false);

                if (CentralPurchase.FirstTimeOffer != null)
                    DashboardManager.instance.ShowWelcomeOffer();  // Welcomr offer 

                OfferPopupController.offerBannerSlotCall?.Invoke();
                Logger.Print($"AppData.isReviewAd  = {AppData.isReviewAd} | day: {day}");

                if ((day == 3 || day == 6))
                {
                    StartCoroutine(AppReview.Instance.ReviewPopup());
                }
                day = 0;
                break;
        }
    }

    public void TurnWheel()
    {
        if (!_isSpinning)
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.spinner);
            _isSpinning = true;
            spinBtn.interactable = false;

            // jyare SpinDuration ma change karo tyare  + 180 karela chhe e remove kari dejo...[agar spinduration 4.5 chhe to 180+ karvana and agar integer ma chhe to che to
            // kay plus karavani jarur nathi]
            //First Spin
            float angle = (360 * spinDuration + 45 * (7 - friend)) + 180;
            Vector3 targetRotation = Vector3.back * (angle + 2 * 360 * spinDuration);
            Logger.Print(TAG + "Friend Index Is  >> " + friend + " >> Old Angle Is  >. " + targetRotation + " Current Angle >> " + wheelCircle.eulerAngles.z);

            //Second Spin
            float angle1 = (360 * spinDuration + 45 * day) + 180;
            Vector3 targetRotation1 = Vector3.forward * (angle1 + 2 * 360 * spinDuration);
            Logger.Print(TAG + "Day Index Is  >> " + day + " >> Old Angle Is1  >. " + targetRotation1 + " Current Angle1 >> " + wheelCircle1.eulerAngles.z);

            //Last Spin
            float angle2 = (360 * spinDuration + 45 * (8 - spinnerIndex)) + 180;
            Vector3 targetRotation2 = Vector3.back * (angle2 + 2 * 360 * spinDuration);
            Logger.Print(TAG + "Spinner Index Is  >> " + spinnerIndex + " >> Old Angle Is2  >. " + targetRotation2 + " Current Angle2 >> " + wheelCircle2.eulerAngles.z);

            //Frist spin Handle
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(wheelCircle
               .DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart));

            //Second Spin handle
            mySequence.Insert(0, wheelCircle1
               .DORotate(targetRotation1, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart));
            //Last Spin handle
            mySequence.Insert(0, wheelCircle2
               .DORotate(targetRotation2, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart).OnComplete(() =>
               {
                   friend = 0;
                   //day = 0;
                   spinnerIndex = 0;
                   rewardpanel.SetActive(true);
               }));
        }
    }
}
