//using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    private string TAG = "REWARD PANEL >> ";
    public static RewardPanel instance;

    [SerializeField] GameObject rewardPanel;
    [SerializeField] Image rewardBg;
    [SerializeField] Transform rewardPopUp;
    [SerializeField] GameObject goldObj, gemsObj;
    [SerializeField] TextMeshProUGUI goldTxt, gemsTxt, noteTxt;
    int rewardClaim = 1;
    public JSONNode rewardData;
    private void Awake()
    {
        if (instance == null) instance = this;

        rewardClaim = 1;
    }

    JSONObject sendableObj = new JSONObject();
    public void ShowRewardPopUp(JSONNode reward)
    {
        if (!AppData.isRewardAvailable) return;

        Logger.Print(TAG + " Reward Claimed >> " + rewardClaim + " >> Is Reward Available >> " + AppData.isRewardAvailable);
        
        if (rewardClaim > 2)
        {
            AppData.isRewardAvailable = false;
            rewardClaim = 1;
            rewardData = "";
            return;
        }

        rewardData = reward;
        goldObj.SetActive(false);
        gemsObj.SetActive(false);

        JSONNode weekly = reward["wrw"];
        JSONNode dailymission = reward["dmw"];

        if (weekly.Count != 0 || dailymission.Count != 0)
        {
            switch (rewardClaim)
            {
                case 1:
                    if (weekly.Count == 0)
                    {
                        Logger.Print(TAG + "Skipped Weekly");
                        rewardClaim++;
                        ShowRewardPopUp(rewardData);
                    }
                    else
                    {
                        sendableObj.Clear();
                        sendableObj.Add("wrw", weekly);
                        goldObj.SetActive(weekly["gold"].AsInt > 0);
                        gemsObj.SetActive(weekly["gems"].AsInt > 0);
                        goldTxt.text = AppData.numDifferentiation(weekly["gold"].AsLong);
                        gemsTxt.text = AppData.numDifferentiation(weekly["gems"].AsLong);
                        noteTxt.text = $"{MessageClass.congratulations} You has been came at " + weekly["rank"].AsInt + " Rank in Weekly Winner.";
                        CommanAnimations.instance.PopUpAnimation(rewardPanel, rewardBg, rewardPopUp, Vector3.one, true);
                    }
                    break;
                case 2:
                    if (dailymission.Count == 0)
                    {
                        Logger.Print(TAG + "Skipped Daily");
                        rewardClaim++;
                        ShowRewardPopUp(rewardData);
                    }
                    else
                    {
                        Logger.Print(TAG + "Daily Bonus Here");
                        sendableObj.Clear();
                        sendableObj.Add("dmw", dailymission);
                        goldObj.SetActive(dailymission["gold"].AsInt > 0);
                        gemsObj.SetActive(false);
                        goldTxt.text = AppData.numDifferentiation(dailymission["gold"].AsLong);
                        noteTxt.text = "WE NOTICED THAT YOU FORGOT TO CLAIM YOUR DAILY MISSION BONUS , COLLECT IT :)";
                        CommanAnimations.instance.PopUpAnimation(rewardPanel, rewardBg, rewardPopUp, Vector3.one, true);
                    }
                    break;
            }
        }
    }

    public void OnClick_Collect()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.missionClaim);
        rewardClaim++;
        ShowRewardPopUp(rewardData);
        EventHandler.UpdateUserProfile(sendableObj);
        CommanAnimations.instance.PopUpAnimation(rewardPanel, rewardBg, rewardPopUp, Vector3.zero, false, false);
    }
}
