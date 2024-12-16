using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HourlyBonus : MonoBehaviour
{
    public static HourlyBonus instance;

    [SerializeField] GameObject hourlyBonusPanel;
    [SerializeField] Image hourlyBg;
    [SerializeField] Transform hourlyPopUp;

    [SerializeField] TextMeshProUGUI normalCoinText, videoCoinsText;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ShowHourlyBonus(long Coins, float videoCoins)
    {
        normalCoinText.text = AppData.numDifferentiation(Coins);
        long vCoins = Coins + (long)(Coins * videoCoins);
        videoCoinsText.text = AppData.numDifferentiation(vCoins);
        CommanAnimations.instance.PopUpAnimation(hourlyBonusPanel, hourlyBg, hourlyPopUp, Vector3.one, true);
    }

    public void OnClick_Collect(bool isVideo)
    {
        Loading_screen.instance.ShowLoadingScreen(true);
        if (isVideo)
        {
            AppData.isShownAdsFrom = 3;
            AdmobManager.instance.ShowRewardedAd();
        }
        else
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.missionClaim);
            EventHandler.CollectHourlyBonus(isVideo);
            CloseHourlyBonus();
        }
    }

    public void CloseHourlyBonus()
    {
        CommanAnimations.instance.PopUpAnimation(hourlyBonusPanel, hourlyBg, hourlyPopUp, Vector3.zero, false, false);
        AppData.isShowHourlyBonus = false;
        Loading_screen.instance.ShowLoadingScreen(false);
    }
}
