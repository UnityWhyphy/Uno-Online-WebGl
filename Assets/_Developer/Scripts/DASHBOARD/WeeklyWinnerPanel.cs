using DG.Tweening;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyWinnerPanel : MonoBehaviour
{
    private string TAG = "WEEKLY WINNER >> ";

    [SerializeField] RectTransform wWPanel;
    [SerializeField] Image thisWeekImg, lastWeekImg;
    [SerializeField] Sprite tapOn, tapOff, defaultFrame;
    [SerializeField] Texture defaultPlayerImg;
    [SerializeField] List<RawImage> playerImgs;
    [SerializeField] List<Image> playerFrames;
    [SerializeField] List<TextMeshProUGUI> playerCoins, playerGems, playerName, playerXP;
    [SerializeField] TextMeshProUGUI timerText;


    [Header("Weekly Winner Info")]
    [SerializeField] GameObject infoPanel;
    [SerializeField] Image infoBg;
    [SerializeField] Transform infoPopUp, girlImg;

    List<WeeklyWinnerData> thisWeekList, lastWeekList;
    float remainingTime = 0;


    private void OnEnable()
    {
        remainingTime = 0;
        SocketManagergame.OnListner_NWWL += HandleWeeklyWinnerList;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_NWWL -= HandleWeeklyWinnerList;
        remainingTime = 0;
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = StaticData.GetTimeInFormateHr(Convert.ToInt64(remainingTime * 1000));
        }
    }

    private void HandleWeeklyWinnerList(JSONNode data)
    {
        DashboardManager.instance.settingBtnCounter.SetActive(false);
        DashboardManager.instance.weeklyWinnerCounter.SetActive(false);

        if (data["lefttime"] > 86400)
            timerText.text = (data["lefttime"] / 86400) + "D " + AppData.getTimeInHourNew(data["lefttime"] % 86400);
        else
        {
            remainingTime = data["lefttime"].AsFloat;
            timerText.text = StaticData.GetTimeInFormateHr(data["lefttime"] * 1000);
        }
        DashboardManager.instance.settingImg.sprite = DashboardManager.instance.normal;
        thisWeekList = JsonConvert.DeserializeObject<List<WeeklyWinnerData>>(data["cwl"]["users"].ToString());
        lastWeekList = JsonConvert.DeserializeObject<List<WeeklyWinnerData>>(data["pwl"]["users"].ToString());
        OnClick_WeeklyWinnerPanel(0, false);
    }

    private void SetPlayersData(List<WeeklyWinnerData> data)
    {
        Logger.Print(TAG + "Set PLayer Data Called... Data Count : " + data.Count);
        ResetPlayers();
        for (int i = 0; i < data.Count; i++)
        {
            int rank = data[i].rank - 1;
            StartCoroutine(AppData.ProfilePicSet(data[rank].pp, playerImgs[rank]));
            StartCoroutine(AppData.SpriteSetFromURL(data[rank].frameImg, playerFrames[rank], "SetPlayersData"));
            playerName[rank].text = data[rank].pn;
            playerCoins[rank].text = AppData.numDifferentiation(data[rank].rw.gold);
            playerGems[rank].text = AppData.numDifferentiation(data[rank].rw.gems);
            playerXP[rank].text = AppData.numDifferentiation(data[rank].wxp);
        }
    }

    private void ResetPlayers()
    {
        for (int i = 0; i < playerImgs.Count; i++)
        {
            playerImgs[i].texture = defaultPlayerImg;
            playerFrames[i].sprite = defaultFrame;
            playerName[i].text = "PLAYER";
            playerCoins[i].text = "0";
            playerGems[i].text = "0";
            playerXP[i].text = "0";
        }
    }

    public void WeeklyWinnerClick(int i)
    {
        OnClick_WeeklyWinnerPanel(i);
    }

    int currentTap = -1;
    public void OnClick_WeeklyWinnerPanel(int click, bool soundPlay = true)
    {
        if (currentTap == click) return;
        if (soundPlay && click != 4 && click != 5) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        currentTap = click;
        switch (click)
        {
            case 0: //Open
                Loading_screen.instance.ShowLoadingScreen(false);
                CommanAnimations.instance.FullScreenPanelAnimation(wWPanel, true);
                OnClick_WeeklyWinnerPanel(DashboardManager.instance.isWWAnounced ? 3 : 2, false);
                break;
            case 1://Close
                currentTap = -1;
                thisWeekList.Clear();
                lastWeekList.Clear();
                CommanAnimations.instance.FullScreenPanelAnimation(wWPanel, false);
                break;
            case 2://This Week
                SetPlayersData(thisWeekList);
                SetTap(0);
                break;
            case 3://Last Week
                DashboardManager.instance.isWWAnounced = false;
                SetPlayersData(lastWeekList);
                SetTap(1);
                break;
            case 4://WW Help
                CommanAnimations.instance.PopUpAnimation(infoPanel, infoBg, infoPopUp, Vector3.one, true);
                girlImg.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuint);
                break;
            case 5:
                CommanAnimations.instance.PopUpAnimation(infoPanel, infoBg, infoPopUp, Vector3.zero, false);
                girlImg.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InQuint);
                break;
            case 6: //Share Facebook
                currentTap -= 1;
                break;
        }
    }

    private void SetTap(int tap)
    {
        thisWeekImg.sprite = (tap == 0) ? tapOn : tapOff;
        lastWeekImg.sprite = (tap == 1) ? tapOn : tapOff;
    }
}
