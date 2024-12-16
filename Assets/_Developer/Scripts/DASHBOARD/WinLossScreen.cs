using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLossScreen : MonoBehaviour
{
    private string TAG = " >>> Win/Loss Screen >> ";
    public static WinLossScreen instance;

    [SerializeField] RectTransform winLossScreen;
    [SerializeField] RectTransform scratchCardScreen;
    [SerializeField] TextMeshProUGUI titleTxt, lostTxt, coinTxt;

    [SerializeField] Image watchVideoBG;
    [SerializeField] Transform watchVideoPopUp;
    [SerializeField] TextMeshProUGUI watchRewardGoldTxt;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        watchRewardGoldTxt.text = $"{AppData.VIDEOBONUS}";
    }

    public void ShowWinLossPanel()
    {
        Logger.Print(TAG + " Win Loss Coins >> " + AppData.winLossCoins);
        if (AppData.winLossCoins == 0) return;
        titleTxt.text = AppData.winLossCoins > 0 ? "YOU WIN" : "YOU LOSS";
        lostTxt.text = AppData.winLossCoins > 0 ? "YOU HAVE WIN" : "YOU HAVE LOST";
        coinTxt.text = AppData.numDifferentiation(AppData.winLossCoins);
        if (!winLossScreen.gameObject.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(winLossScreen, true);
    }

    public void WinLossScreenFalse()
    {
        AppData.winLossCoins = 0;
        CommanAnimations.instance.FullScreenPanelAnimation(winLossScreen.gameObject, false, 0);
    }

    public void ShowWatchVideoPopup(bool isActive)
    {
        Logger.Print(TAG + " ShowWatchVideoPopup >> " + isActive);
        if (isActive)
            CommanAnimations.instance.PopUpAnimation(watchVideoBG.gameObject, watchVideoBG, watchVideoPopUp, Vector3.one, true);
        else
        {
            if (watchVideoBG.gameObject.activeInHierarchy)
                CommanAnimations.instance.PopUpAnimation(watchVideoBG.gameObject, watchVideoBG, watchVideoPopUp, Vector3.zero, false);
        }
    }

    public void ScratchCardScreenPanel()
    {
        scratchCardScreen.gameObject.SetActive(true);
    }

    public void OnClick_WinLossPopup(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        Logger.NormalLog($"ScratchPopupOn === OnClick_WinLossPopup ??? 2 AppData.isShowHourlyBonus = {AppData.isShowHourlyBonus} ");
        switch (i)
        {
            case 0: //Close
                AppData.winLossCoins = 0;
                CommanAnimations.instance.FullScreenPanelAnimation(winLossScreen.gameObject, false);

                if (StaticData.LEVELUPDATA != "")
                    LevelUpPanel.instance.ShowLevelUpPanel();

                else
                    RewardPanel.instance.ShowRewardPopUp(AppData.SignUpData["reward"]);
                if (AppData.promoflag == null)
                    ShowWatchVideoPopup(true);
                scratchCardScreen.gameObject.SetActive(false);

                DashboardManager.instance.MatchCountShowOffer(); // 50 % chance to call this

                if (AppData.totalPlayedMatch == 1)
                {
                    setting_script.instance.FeedbackPopupShow(true); // 1 time win close
                }

                // Intro tutorial
                GameManager.instance.ShowIntroOnDashboard();

                Loading_screen.instance.ShowLoadingScreen(false);
                break;

            case 1://Watch Video
                AppData.isShownAdsFrom = 4;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 2://Go To Store
                EventHandler.GoldStore(0);
                OnClick_WinLossPopup(0);
                //
                break;

            case 3://Scratch & Win
                ScratchCardScreenPanel();
                break;

            case 4://Scratch ad
                AppData.isShownAdsFrom = 5;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 5:// claim
                EventHandler.ScratchVideoReward(rewardAmount[amt]);
                scratchImg.gameObject.SetActive(true);
                scratchImg.raycastTarget = false;
                sClosebtn.SetActive(true);
                adButton.SetActive(true);
                claimButton.SetActive(false);
                ScratchCardMaskUGUI.Instance.Reload();
                OnClick_WinLossPopup(0);

                isActive = false;
                isScratchOn = false;
                break;

            case 6:// WinLossClose
                break;
        }
    }
    [Header("Scratch")]
    [SerializeField] RawImage scratchImg;
    public GameObject adButton;
    public GameObject claimButton;
    public GameObject sClosebtn;
    [SerializeField] bool isActive;
    [SerializeField] bool isScratchOn;
    private int[] rewardAmount = { 50, 100, 150, 200, 250, 300, 350, 450, 500, 550, 650, 700, 750, 800, 850, 900, 950, 1000 };
    [SerializeField] TextMeshProUGUI s_RewardTxt;
    int amt;
    public void ScratchPopupOn()
    {
        Logger.NormalLog($"ScratchPopupOn === ??? 1");
        adButton.SetActive(false);
        sClosebtn.SetActive(false);
        scratchImg.raycastTarget = true;
        isScratchOn = true;
        s_RewardTxt.text = string.Empty;

        amt = Random.Range(0, rewardAmount.Length - 1);
        s_RewardTxt.text = rewardAmount[amt].ToString() + "x";
    }

    public void ProgressCheck(float progress)
    {
        if (progress >= 0.6f && !isActive && isScratchOn)
        {
            isActive = true;
            isScratchOn = false;
            claimButton.SetActive(true);
            scratchImg.gameObject.SetActive(false);
        }
    }
}
