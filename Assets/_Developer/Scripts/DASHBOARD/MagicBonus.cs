using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicBonus : MonoBehaviour
{
    private string TAG = " >> MAGIC BONUS >>>";

    [SerializeField] TextMeshProUGUI mBTimerText;
    [SerializeField] GameObject video2XIcon;
    [SerializeField] Button claimBtn;
    [SerializeField] Image claimImg;

    private void OnEnable()
    {
        SocketManagergame.OnListner_MB += HandleMagicBonus;
        SocketManagergame.OnListner_CMB += HandleClaimMagicBonus;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_MB -= HandleMagicBonus;
        SocketManagergame.OnListner_CMB -= HandleClaimMagicBonus;
    }

    private void Update()
    {
        SetMagicBonusData();
    }
    private void HandleMagicBonus(JSONNode data)
    {
        Logger.Print(TAG + "Handle Magic Bonus Called >> " + ((AppData.magicBonusTime <= 0) ? data["mbt"].AsFloat * 60 : AppData.magicBonusTime));
        AppData.magicBonusTime = (AppData.magicBonusTime <= 0) ? data["mbt"].AsFloat * 60 : AppData.magicBonusTime;
        magicAmt = data["mbb"].AsInt;
        Logger.Print($" magicAmt { magicAmt}");
        SetMagicBonusData();
    }

    int magicAmt = 0;

    private void HandleClaimMagicBonus(JSONNode data)
    {
        Logger.Print(TAG + "Handle Claim Magic Bonus Called >> " + data["ntime"].AsFloat);
        AppData.magicBonusTime = data["ntime"].AsFloat * 60;
      
        SetMagicBonusData();
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private void SetMagicBonusData()
    {
        if (AppData.magicBonusTime > 0)
        {
            if (claimImg.gameObject.activeInHierarchy) claimImg.gameObject.SetActive(false);
            mBTimerText.text = AppData.GetTimeInFormateMin((long)AppData.magicBonusTime * 1000);
            claimBtn.interactable = false;
        }
        else
        {
            //mBTimerText.text = "CLAIM";
            if(!claimImg.gameObject.activeInHierarchy) claimImg.gameObject.SetActive(true);
            mBTimerText.text = magicAmt.ToString();
            claimBtn.interactable = true;
        }
            //mBTimerText.text = data["mbb"];
        video2XIcon.SetActive(AppData.magicBonusTime <= 0);
    }

    public void ClaimMagicBonus()
    {
        if (AppData.IsCanShowAd)
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        Loading_screen.instance.ShowLoadingScreen(true);
        AppData.isShownAdsFrom = 1;
        AdmobManager.instance.ShowRewardedAd();
    }
}
