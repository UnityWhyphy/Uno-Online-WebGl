using DG.Tweening;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMission : MonoBehaviour
{
    private string TAG = " >>> DAILY MISSION >> ";

    public RectTransform dailyMissionPanel;
    [SerializeField] DailyTask taskPrefab;
    [SerializeField] Transform taskContent;
    [SerializeField] Sprite taskBtnComplete, taskBtnPlayNow, /*taskBtnDiable,*/ gemsIcon, goldIcon;
    [SerializeField] TextMeshProUGUI goldText, gemsText, totalGoldRewardText, totalGemsRewardTxt, sliderCountText, remainingTimerText, taskNoteTxt;
    [SerializeField] Slider progressSlider;
    [SerializeField] ScrollRect dMScroll;

    [SerializeField] GameObject claimBtn, taskSlider, taskNote, timer;
    [SerializeField] Button claimButton;
    [SerializeField] Image glowBgImg;
    public List<RewardAnimControl> coinsRewardList = new List<RewardAnimControl>();
    public List<RewardAnimControl> gemsRewardList = new List<RewardAnimControl>();


    [SerializeField] int cAnimIndex, gAnimIndex;
    [SerializeField] float speed = 0.2f;

    [Header("DM Notification")]
    [SerializeField] private DMIControl dmNotiPrefab;
    [SerializeField] private RectTransform dParent;

    List<GameObject> dmiObjectList = new();

    public static DailyMission instance;

    float remainingTime = 0;

    private void Start()
    {
        if (instance == null)
            instance = this;

        for (int i = 0; i < coinsRewardList.Count; i++)
        {
            coinsRewardList[i].AddInList();
        }
        for (int i = 0; i < gemsRewardList.Count; i++)
        {
            gemsRewardList[i].AddInList();
        }
    }

    private void OnEnable()
    {
        remainingTime = 0;
        SocketManagergame.OnListner_DAILYMISSION += HandleDailyMission;
        SocketManagergame.OnListner_CLAIMMISSION += HandleClaimMission;
        SocketManagergame.OmListner_DMI += HandleGanrateDMI;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_DAILYMISSION -= HandleDailyMission;
        SocketManagergame.OnListner_CLAIMMISSION -= HandleClaimMission;
        SocketManagergame.OmListner_DMI -= HandleGanrateDMI;
        remainingTime = 0;
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            remainingTimerText.text = AppData.GetTimeInFormateHr((long)Mathf.Abs(remainingTime) * 1000);
        }
    }

    private void HandleGanrateDMI(JSONNode json)
    {
        Logger.NormalLog($"Json = {json}");

        DMIControl dmObject = Instantiate(dmNotiPrefab, new Vector2(dParent.anchoredPosition.x, dParent.anchoredPosition.y + 300), Quaternion.identity);
        dmObject.transform.SetParent(dParent, false);
        dmiObjectList.Add(dmObject.gameObject);
        Logger.Print($"Txt = {json["txt"]}");

        dmObject.dmiTxt.text = json["txt"];

        dmObject.transform.DOLocalMove(Vector3.zero, 0.5f)
            .OnComplete(() =>
            {
                dmObject.transform.DOLocalMove(new Vector2(dParent.anchoredPosition.x, dParent.anchoredPosition.y + 300), 0.5f).SetDelay(3).OnComplete(() => Destroy(dmObject.gameObject));
            });
    }


    internal void DmListDestroy()
    {
        foreach (var dm in dmiObjectList)
        {
            Destroy(dm.gameObject);
        }
    }


    private void HandleDailyMission(JSONNode data)
    {
        cAnimIndex = gAnimIndex = 0;
        List<DailyMissionData> dMData = JsonConvert.DeserializeObject<List<DailyMissionData>>(data["missiondata"].ToString());
        Logger.Print($"dMData = {dMData.Count}");
        AppData.canShowChallenge = false;
        claimBtn.SetActive(data["rewardClaim"] == 1);
        taskSlider.SetActive(data["rewardClaim"] != 1);
        taskNote.SetActive(data["rewardClaim"] != 1);
        timer.SetActive(data["rewardClaim"] != 1);
        claimButton.gameObject.SetActive(data["rewardClaim"] == 1);
        claimButton.interactable = data["rewardClaim"] == 1;
        taskNoteTxt.text = "COMPLETE ALL " + data["missiondata"].Count + " MISSIONS TO <br>GET THE MEGA PRIZE!";

        if (taskContent.childCount > dMData.Count)
        {
            for (int i = taskContent.childCount - 1; i >= dMData.Count; i--)
            {
                Destroy(taskContent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < dMData.Count; i++)
        {
            DailyTask currentBoard;
            int tempIndex = i;
            if (taskContent.childCount < i + 1)
            {
                currentBoard = Instantiate(taskPrefab, taskContent);
            }
            else
            {
                currentBoard = taskContent.GetChild(i).GetComponent<DailyTask>();
            }

            currentBoard.taskName.text = dMData[i].txt;
            currentBoard.taskSliderBg.maxValue = dMData[i].total;
            currentBoard.taskSliderBg.value = dMData[i].count;
            currentBoard.missionPendingText.text = dMData[i].count + "/" + dMData[i].total;

            currentBoard.coinIcon.sprite = (data["reward"][i]["type"] == "gold") ? goldIcon : gemsIcon;
            currentBoard.coinVal.text = AppData.numDifferentiation(data["reward"][i]["value"].AsLong);

            currentBoard.currntStatusTxt.text = (data["missionResult"][i] == 1) ? "COMPLETED" : "PLAY NOW";

            currentBoard.click.GetComponent<Image>().sprite = (data["missionResult"][i] == 1) ? taskBtnComplete : taskBtnPlayNow;//Test
            currentBoard.lockImg.SetActive(false);

            currentBoard.click.onClick.RemoveAllListeners();
            currentBoard.click.interactable = (data["missionResult"][i] == 1) ? false : true;
            currentBoard.click.onClick.AddListener(() =>
            {
                if (AllCommonGameDialog.instance.isHaveGoldGems(dMData[tempIndex].gameplay.bv, dMData[tempIndex].gameplay.gems))
                {
                    Loading_screen.instance.ShowLoadingScreen(true);
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    EventHandler.PlayGame(dMData[tempIndex].gameplay.mode, dMData[tempIndex].gameplay.bv, dMData[tempIndex].gameplay.player, dMData[tempIndex].gameplay.gems, 0);
                }
            });
        }

        StartCoroutine(CheckAnimComplete(data, dMData));

        goldText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));
        gemsText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));
        totalGoldRewardText.text = AppData.numDifferentiation(data["userReward"]["gold"].AsLong);
        totalGemsRewardTxt.text = AppData.numDifferentiation(data["userReward"]["gems"].AsLong);

        int progress = 0;
        for (int j = 0; j < data["missionResult"].Count; j++)
        {
            if (data["missionResult"][j] == 1) progress++;
        }

        progressSlider.maxValue = data["missionResult"].Count;
        progressSlider.value = progress;
        sliderCountText.text = progress + "/" + data["missionResult"].Count;
        remainingTimerText.text = AppData.GetTimeInFormateHr(data["leftime"].AsLong * 1000);
        remainingTime = data["leftime"].AsFloat;

        dMScroll.verticalNormalizedPosition = 1;
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!dailyMissionPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(dailyMissionPanel, true);
    }

    private IEnumerator CheckAnimComplete(JSONNode data, List<DailyMissionData> dMData)
    {
        for (int i = 0; i < dMData.Count; i++) // Has corutine
        {
            // TODO Task complete
            if (dMData[i].isresult == 1)
            {
                Logger.NormalLog($"HandleCompleteAnim::: isanimation = {dMData[i].isanimation}");
                // TODO 1 aetle Animation batava nu | 2 aetle nai batavacu .
                if (dMData[i].isanimation == 1)
                {
                    if (!glowBgImg.gameObject.activeInHierarchy)
                    {
                        glowBgImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1).SetLoops(-1, LoopType.Yoyo);
                        glowBgImg.gameObject.SetActive(true);
                    }

                    Logger.NormalLog($"HandleCompleteAnim:::b = type = {data["reward"][i]["type"]} | cAnimIndex = {cAnimIndex} | {gAnimIndex}");

                    if ((data["reward"][i]["type"] == "gold"))
                    {
                        Logger.NormalLog($"HandleCompleteAnim::: c = {coinsRewardList[cAnimIndex].childObjects.Count}");

                        for (int c = 0; c < coinsRewardList[cAnimIndex].childObjects.Count; c++)
                        {
                            Logger.NormalLog($"HandleCompleteAnim::: D = {c}");
                            coinsRewardList[cAnimIndex].childObjects[c].SetActive(true);

                            yield return new WaitForSeconds(speed);
                        }
                        Logger.NormalLog($"HandleCompleteAnim::: E = {coinsRewardList[cAnimIndex].name}");
                        if (cAnimIndex <= coinsRewardList.Count - 1)
                            cAnimIndex = (cAnimIndex + 1) % coinsRewardList.Count;
                    }
                    else
                    {
                        for (int g = 0; g < gemsRewardList[gAnimIndex].childObjects.Count; g++)
                        {
                            gemsRewardList[gAnimIndex].childObjects[g].SetActive(true);
                            yield return new WaitForSeconds(speed);
                        }
                        Logger.NormalLog($"HandleCompleteAnim::: G = {gemsRewardList[gAnimIndex].name}");
                        if (gAnimIndex <= gemsRewardList.Count - 1)
                            gAnimIndex = (gAnimIndex + 1) % gemsRewardList.Count;
                    }


                }
                else // 2
                {
                    //Logger.Print($"last = {data["lastAnimationIndex"].AsInt} || current = {data["currentAnimationIndex"].AsInt}");

                    //if (data["lastAnimationIndex"].AsInt == data["currentAnimationIndex"].AsInt)
                    {
                        if (data["reward"][i]["type"] == "gold")
                        {
                            Logger.NormalLog($"cAnimIndex = {cAnimIndex} | <> | {i} Count = { coinsRewardList[cAnimIndex].childObjects.Count}");
                            for (int k = 0; k < coinsRewardList[cAnimIndex].childObjects.Count; k++)
                            {
                                coinsRewardList[cAnimIndex].childObjects[k].SetActive(true);
                            }
                            cAnimIndex = (cAnimIndex + 1) % coinsRewardList.Count;
                        }
                        else
                        {
                            Logger.NormalLog($"gAnimIndex = {gAnimIndex} | <> | {i} Count = { gemsRewardList[gAnimIndex].childObjects.Count}");
                            for (int k = 0; k < gemsRewardList[gAnimIndex].childObjects.Count; k++)
                            {
                                gemsRewardList[gAnimIndex].childObjects[k].SetActive(true);
                            }
                            gAnimIndex = (gAnimIndex + 1) % gemsRewardList.Count;
                        }

                    }
                }
            }
            else // Task is not complate
            {
                Logger.Print($"Task is not complate = {i} || cAnimIndex = {cAnimIndex} | gAnimIndex = {gAnimIndex}");
                if (data["reward"][i]["type"] == "gold")
                {
                    Logger.NormalLog($"cAnimIndex = {cAnimIndex} | <> | {i} Count = { coinsRewardList[cAnimIndex].childObjects.Count}");
                    for (int k = 0; k < coinsRewardList[cAnimIndex].childObjects.Count; k++)
                    {
                        coinsRewardList[cAnimIndex].childObjects[k].SetActive(false);
                    }
                    cAnimIndex = (cAnimIndex + 1) % coinsRewardList.Count;
                }
                else
                {
                    Logger.NormalLog($"gAnimIndex = {gAnimIndex} | <> | {i} Count = { gemsRewardList[gAnimIndex].childObjects.Count}");
                    for (int k = 0; k < gemsRewardList[gAnimIndex].childObjects.Count; k++)
                    {
                        gemsRewardList[gAnimIndex].childObjects[k].SetActive(false);
                    }
                    gAnimIndex = (gAnimIndex + 1) % gemsRewardList.Count;
                }
            }

        }
    }

    private IEnumerator HandleCompleteAnim(JSONNode data, int i)
    {
        yield return new WaitForSeconds(speed);

        if (data["missionResult"][i] == 1)
        {
            if (!glowBgImg.gameObject.activeInHierarchy)
            {
                glowBgImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1).SetLoops(-1, LoopType.Yoyo);
                glowBgImg.gameObject.SetActive(true);
            }

            Logger.NormalLog($"HandleCompleteAnim:::b = type = {data["reward"][i]["type"]} | cAnimIndex = {cAnimIndex}");

            if ((data["reward"][i]["type"] == "gold"))
            {
                Logger.NormalLog($"HandleCompleteAnim::: c = {coinsRewardList[cAnimIndex].childObjects.Count}");

                for (int c = 0; c < coinsRewardList[cAnimIndex].childObjects.Count; c++)
                {
                    Logger.NormalLog($"HandleCompleteAnim::: D = {c}");
                    coinsRewardList[cAnimIndex].childObjects[c].SetActive(true);

                    yield return new WaitForSeconds(speed);
                }
                Logger.NormalLog($"HandleCompleteAnim::: E = {coinsRewardList[cAnimIndex].name}");
                if (cAnimIndex <= coinsRewardList.Count - 1)
                    cAnimIndex = (cAnimIndex + 1) % coinsRewardList.Count;
            }
            else
            {
                for (int g = 0; g < gemsRewardList[gAnimIndex].childObjects.Count; g++)
                {
                    gemsRewardList[gAnimIndex].childObjects[g].SetActive(true);
                    yield return new WaitForSeconds(speed);
                }
                Logger.NormalLog($"HandleCompleteAnim::: G = {gemsRewardList[gAnimIndex].name}");
                if (gAnimIndex <= gemsRewardList.Count - 1)
                    gAnimIndex = (gAnimIndex + 1) % gemsRewardList.Count;
            }
        }
    }

    public void OnClick_CloseDailyMission()
    {
        if (!AppData.canShowChallenge) AppData.canShowChallenge = true;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        CommanAnimations.instance.FullScreenPanelAnimation(dailyMissionPanel, false);
    }

    public void OnClick_Claimreward()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.missionClaim);
        claimButton.interactable = false;
        Loading_screen.instance.ShowLoadingScreen(true);
        EventHandler.SendClaimMision();
    }

    private void HandleClaimMission(JSONNode data)
    {
        DOTween.Kill(glowBgImg);
        glowBgImg.gameObject.SetActive(false);

        EventHandler.SendDailyMission();

        if (AppData.isReviewAd >= 0)
        {
            AppData.isReviewAd--;
            StartCoroutine(AppReview.Instance.ReviewPopup());
        }

        setting_script.instance.FeedbackPopupShow(true); // Claim Mission

        FirebaseData.EventSendWithFirebase("DailyMissionComplete");
    }
}
