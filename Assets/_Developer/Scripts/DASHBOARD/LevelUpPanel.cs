using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    private string TAG = " >>> LEVELUP PANEL >> ";

    public static LevelUpPanel instance;

    [SerializeField] GameObject levelUpPanel;
    [SerializeField] Image levelUpBg;
    [SerializeField] Transform levelUpPopUp;
    [SerializeField] TextMeshProUGUI levelTxt, goldTxt, gemsTxt, goldTextCollect, gemsTextCollect, videoGoldTxt;
    [SerializeField] GameObject gemsObject, gemsObjCollect;
    long vBonus = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ShowLevelUpPanel()
    {
        JSONNode data = JSON.Parse(StaticData.LEVELUPDATA);
        Logger.Print(TAG + " ShowLevel Up Pannel Called " + StaticData.LEVELUPDATA);

        levelTxt.text = data["new_level"];
        gemsObject.SetActive(data["gems"].AsInt > 0);
        gemsObjCollect.SetActive(data["gems"].AsInt > 0);

        goldTxt.text = AppData.numDifferentiation(data["gold"].AsLong);
        gemsTxt.text = AppData.numDifferentiation(data["gems"].AsLong);
        goldTextCollect.text = AppData.numDifferentiation(data["gold"].AsLong);
        gemsTextCollect.text = AppData.numDifferentiation(data["gems"].AsLong);
        videoGoldTxt.text = AppData.numDifferentiation(data["vbonus"].AsLong);
        vBonus = data["vbonus"].AsLong + data["gold"].AsLong;        

        CommanAnimations.instance.PopUpAnimation(levelUpPanel, levelUpBg, levelUpPopUp, Vector3.one, true);
    }

    public void LevelScreenfalse()
    {
        CommanAnimations.instance.PopUpAnimation(levelUpPanel, levelUpBg, levelUpPopUp, Vector3.zero, false, false);
    }

    public void LevelUpClick(int click)
    {
        switch (click)
        {
            case 1: //Video Collect
                Loading_screen.instance.ShowLoadingScreen(true);
                AppData.isShownAdsFrom = 0;
                AppData.notiRewardType = "Level up";
                AppData.notiRewardVal = vBonus;
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 2:
                StaticData.LEVELUPDATA = "";
                CommanAnimations.instance.PopUpAnimation(levelUpPanel, levelUpBg, levelUpPopUp, Vector3.zero, false, false);                
                DashboardManager.instance.ChipsAddAnimation(100, int.Parse(PrefrenceManager.GOLD));                
                break;

        }
    }

}
