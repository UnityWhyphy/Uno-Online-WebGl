using DanielLochner.Assets.SimpleScrollSnap;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class setting_script : MonoBehaviour
{
    private static string TAG = ">>>SETTING ";

    public static setting_script instance;

    [Header("Setting")]
    public RectTransform SettingPannel;
    public Sprite[] OnOff;
    public Image[] OnOffBtn;
    public Image facebookBtnImg;
    public TextMeshProUGUI facebookBtnTxt;

    [Header("GoldHistory")]
    [SerializeField] RectTransform goldHistoryPanel;
    [SerializeField] GameObject goldHistoryPrefab, goldHistoryContent;
    [SerializeField] Sprite coin, gem, leaf;
    [SerializeField] Sprite added, minus;
    [SerializeField] ScrollRect goldHistoryScroll;

    [Header("FEEDBACK")]
    [SerializeField] RectTransform feedbackPanel;
    [SerializeField] TMP_InputField playerEmail, feedback;
    [SerializeField] TextMeshProUGUI playerName;

    [Header("FEEDBACKPoup")]
    [SerializeField] Image feedbackbg;
    [SerializeField] Transform feedbacktrans;
    [SerializeField] TMP_InputField p_playerEmail, p_feedback;
    //[SerializeField] TextMeshProUGUI feedbackpop;

    [Header("MORE GAME")]
    [SerializeField] GameObject gamePrefab;
    [SerializeField] Transform moreGameCont;

    [Header("Game Rules")]
    [SerializeField] RectTransform gameRulesPanel;
    [SerializeField] GameObject gameRules1, gameRules2, gameRulesSelectPanel;
    [SerializeField] Image normalBtn, invertBtn;
    [SerializeField] Sprite sel_s, desel_s;

    public GameObject instentDeleteBtn;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_UGH += Onrecieved_UGH;
        SocketManagergame.OnListner_FB += Onrecieved_FEEDBACK;
        SocketManagergame.OnListner_MOREGAME += HandleMoreGame;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_UGH -= Onrecieved_UGH;
        SocketManagergame.OnListner_FB -= Onrecieved_FEEDBACK;
        SocketManagergame.OnListner_MOREGAME -= HandleMoreGame;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    FeedbackPopupShow(true); // Escape
        //}
    }

    //gold History Pannel
    private void Onrecieved_UGH(JSONNode Data)
    {
        Logger.Print(TAG + " Onrecieved_UGH called " + Data.ToString());

        List<GoldHistoryModel> data = JsonConvert.DeserializeObject<List<GoldHistoryModel>>(Data.ToString());
        SetGoldHistoryData(data);
    }

    private void SetGoldHistoryData(List<GoldHistoryModel> data)
    {
        if (goldHistoryContent.transform.childCount > data.Count)
        {
            for (int i = goldHistoryContent.transform.childCount - 1; i >= data.Count; i--)
            {
                Destroy(goldHistoryContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentBoard;
            if (goldHistoryContent.transform.childCount < i + 1)
            {
                currentBoard = Instantiate(goldHistoryPrefab, goldHistoryContent.transform);
            }
            else
            {
                currentBoard = goldHistoryContent.transform.GetChild(i).gameObject;
            }

            currentBoard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data[i].tp;
            currentBoard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = AppData.numDifferentiation(data[i].previous_gold);
            currentBoard.transform.GetChild(3).GetComponent<Image>().sprite = data[i].gold > 0 ? added : minus;
            switch (data[i].isgems)
            {
                case 0:
                    currentBoard.transform.GetChild(1).GetComponent<Image>().sprite = coin;
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = coin;
                    currentBoard.transform.GetChild(6).GetComponent<Image>().sprite = coin;
                    break;
                case 1:
                    currentBoard.transform.GetChild(1).GetComponent<Image>().sprite = gem;
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = gem;
                    currentBoard.transform.GetChild(6).GetComponent<Image>().sprite = gem;
                    break;
                case 6:
                    currentBoard.transform.GetChild(1).GetComponent<Image>().sprite = leaf;
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = leaf;
                    currentBoard.transform.GetChild(6).GetComponent<Image>().sprite = leaf;
                    break;
                    /*
                     gold: 0
                     gems:1  
                     leaf:6
                     */
            }

            currentBoard.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = AppData.numDifferentiation(data[i].gold < 0 ? -data[i].gold : data[i].gold);
            currentBoard.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = AppData.numDifferentiation(data[i].total_gold);
        }
        goldHistoryScroll.verticalNormalizedPosition = 1;
        Loading_screen.instance.ShowLoadingScreen(false);
        CommanAnimations.instance.FullScreenPanelAnimation(goldHistoryPanel, true);
    }

    public void GoldHistoryClose()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        CommanAnimations.instance.FullScreenPanelAnimation(goldHistoryPanel, false);
    }

    private void Onrecieved_FEEDBACK(JSONNode data)
    {
        Logger.Print(TAG + " Onrecieved_FB called " + data.ToString());
        Loading_screen.instance.ShowLoadingScreen(false);
        AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.thanks, "Thank you for your feedback. We greatly appreciate your input and are committed to improving your gaming experience. " +
            "We will contact you via email to address your concerns and suggestions. If you have any further issues or ideas, please don't hesitate to reach out to us.");
        //CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkBg, justOkPopUp, Vector3.one, true);
        CloseFeedbackPanel();
        CloseFeedbackPopup();
    }

    public void OnClick_SendFeedback()
    {
        string email = playerEmail.text.Trim();
        string msg = feedback.text.Trim();
        string MatchEmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Logger.Print(TAG + " Please Enter Valid" + email);

        if (!Regex.IsMatch(email, MatchEmailPattern) || email.Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.invelidEmail, "PLEASE ENTER VALID EMAIL");
            return;
        }
        else if (feedback.text.Trim().Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.invelidFEEDBACK, "PLEASE ENTER YOUR FEEDBACK");
            return;
        }
        Loading_screen.instance.ShowLoadingScreen(true);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.FeedBack(feedback.text, playerEmail.text, false, PrefrenceManager.UNIQUE_ID, 0, "FeedBack", SystemInfo.deviceUniqueIdentifier);
    }

    public void SendFeedbackClick()
    {
        string email = p_playerEmail.text.Trim();
        string msg = p_feedback.text.Trim();
        string MatchEmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Logger.Print(TAG + " Please Enter Valid" + email);

        if (!Regex.IsMatch(email, MatchEmailPattern) || email.Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.invelidEmail, "PLEASE ENTER VALID EMAIL");
            return;
        }
        else if (p_feedback.text.Trim().Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.invelidFEEDBACK, "PLEASE ENTER YOUR FEEDBACK");
            return;
        }
        Loading_screen.instance.ShowLoadingScreen(true);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.FeedBack(p_feedback.text, p_playerEmail.text, false, PrefrenceManager.UNIQUE_ID, 0, "FeedBack", SystemInfo.deviceUniqueIdentifier);
    }

    public void CloseFeedbackPopup()
    {
        if (!SettingPannel.gameObject.activeInHierarchy) AppData.canShowChallenge = true;
        FeedbackPopupShow(false);
    }

    public void CloseFeedbackPanel()
    {
        if (!SettingPannel.gameObject.activeInHierarchy) AppData.canShowChallenge = true;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        CommanAnimations.instance.FullScreenPanelAnimation(feedbackPanel, false);
    }

    public void FeedbackPopupShow(bool status)
    {
        if (status)
            CommanAnimations.instance.PopUpAnimation(feedbackbg.gameObject, feedbackbg, feedbacktrans, Vector3.one, true);
        else
            CommanAnimations.instance.PopUpAnimation(feedbackbg.gameObject, feedbackbg, feedbacktrans, Vector3.zero, false);
    }

    private void HandleMoreGame(JSONNode data)
    {
        if (moreGameCont.childCount > data.Count)
        {
            for (int i = moreGameCont.childCount - 1; i >= data.Count; i--)
            {
                Destroy(moreGameCont.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentGame;
            int tempIndex = i;
            if (moreGameCont.childCount < i + 1)
            {
                currentGame = Instantiate(gamePrefab, moreGameCont);
            }
            else
            {
                currentGame = moreGameCont.GetChild(i).gameObject;
            }

            StartCoroutine(AppData.SpriteSetFromURL(data[i]["icon"], currentGame.transform.GetChild(0).GetComponent<Image>(), "HandleMoreGame"));
            currentGame.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i]["appname"];

            currentGame.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            currentGame.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
#if UNITY_ANDROID
                Application.OpenURL("market://details?id=" + data[tempIndex]["package"]);
#elif UNITY_IPHONE
                Application.OpenURL("itms-apps://itunes.apple.com/app/id" + data[tempIndex]["ios_id"]);
#endif
            });
        }
        Loading_screen.instance.ShowLoadingScreen(false);
        DashboardManager.instance.BottomClick(6);
    }

    //setting pannel
    public void SettingBtnClick(int i)
    {
        if (i != 7) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        switch (i)
        {
            case 0://sound
                PrefrenceManager.Sound = PrefrenceManager.Sound == 1 ? 0 : 1;
                OnOffBtn[0].sprite = OnOff[PrefrenceManager.Sound];
                if (PrefrenceManager.Sound == 0) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                break;

            case 1://vibrate
                PrefrenceManager.Vibrate = PrefrenceManager.Vibrate == 1 ? 0 : 1;
                OnOffBtn[1].sprite = OnOff[PrefrenceManager.Vibrate];
                break;

            case 2://noti
                PrefrenceManager.Noti = PrefrenceManager.Noti == 1 ? 0 : 1;
                OnOffBtn[2].sprite = OnOff[PrefrenceManager.Noti];
                break;

            case 3://challenge
                PrefrenceManager.Ch = PrefrenceManager.Ch == 1 ? 0 : 1;
                JSONObject data = new JSONObject();
                data.Add("_ch", PrefrenceManager.Ch);
                EventHandler.UpdateUserProfile(data);
                OnOffBtn[3].sprite = OnOff[PrefrenceManager.Ch];
                break;

            case 4://weekly winner
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.WeeklyWinnerList();
                break;

            case 5://gold history
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.UserGoldHistory();
                break;

            case 6://feedback
                AppData.canShowChallenge = false;
                playerName.text = PrefrenceManager.PN;
                playerEmail.text = string.Empty;
                feedback.text = string.Empty;
                CommanAnimations.instance.FullScreenPanelAnimation(feedbackPanel, true);
                break;

            case 7://more gmae
                string plateform = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) ? "android" : "ios";
                EventHandler.SendMoreGame(plateform);
                break;

            case 8://how to play
                break;

            case 9://term condition
                Application.OpenURL(AppData.TermsLink);
                break;

            case 10://privacy policy
                Application.OpenURL(AppData.PrivacyLink);
                break;

            case 11://game rule
                CommanAnimations.instance.FullScreenPanelAnimation(gameRulesPanel, true);
                Logger.Print("CenteredPanel A = " + (SimpleScrollSnap.Instance == null));
                RuleSelectClicks(1);
                //gameRules1.SetActive(false);
                //gameRulesSelectPanel.SetActive(true);
                //SimpleScrollSnap.Instance.GoToPanel(0);
                break;

            case 12://login btn
                PrefrenceManager.ULT = "";
                PrefrenceManager.PN = "";
                PrefrenceManager.PP = "";
                PrefrenceManager.GID = "";
                PrefrenceManager.FID = "";
                PrefrenceManager.FB_TOKEN = "";
                CancelInvoke();

#if UNITY_ANDROID
                GoogleLogin.instance.LogOut();
#endif

                SocketManagergame.SocketDisConnectManually();
                SocketManagergame.isSingUpGot = false;
                SocketManagergame.isReconnect = false;
                SocketManagergame.Instance.state = SocketManagergame.State.None;
                Destroy(SocketManagergame.Instance.gameObject);

                SceneManager.LoadScene(EventHandler.SPLASH);
                break;

            case 13://close setting
                AppData.canShowChallenge = true;
                CommanAnimations.instance.FullScreenPanelAnimation(SettingPannel, false);
                break;

            case 14://Delete Account
                Application.OpenURL(AppData.deleteAccountUrl + PrefrenceManager.UNIQUE_ID);
                break;

            case 15: //GameRules Btns
                break;
        }
    }

    public void RuleSelectClicks(int num)
    {
        switch (num)
        {
            case 1:
                gameRules1.SetActive(true);
                gameRulesSelectPanel.SetActive(false);
                //SimpleScrollSnap.Instance.GoToPanel(0);
                gameRules1.GetComponent<SimpleScrollSnap>().GoToPanel(0);
                break;

            case 2:
                AppData.isTutorialPlay = true;
                CommanAnimations.instance.FullScreenPanelAnimation(SettingPannel, false);
                TutorialManager.Instance.HandleNewUserTutorialClick(1);
                GameManager.instance.mode = "";
                break;
        }
    }

    public void OnClickCloseGameRules()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        CommanAnimations.instance.FullScreenPanelAnimation(gameRulesPanel, false);
    }

    public void SetSettingPanelData()
    {
        AppData.canShowChallenge = false;
        Logger.Print(TAG + "Ch Key Is : " + PrefrenceManager.Ch);
        OnOffBtn[0].sprite = OnOff[PrefrenceManager.Sound];
        OnOffBtn[1].sprite = OnOff[PrefrenceManager.Vibrate];
        OnOffBtn[2].sprite = OnOff[PrefrenceManager.Noti];
        OnOffBtn[3].sprite = OnOff[PrefrenceManager.Ch];
        facebookBtnTxt.text = !PrefrenceManager.ULT.Equals(EventHandler.GUEST) ? "LOG OUT" : "FACEBOOK LOGIN";
        CommanAnimations.instance.FullScreenPanelAnimation(SettingPannel, true);
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public void RuleModeSelect(int i)
    {
        switch (i)
        {
            case 0: // Normal
                gameRules1.GetComponent<SimpleScrollSnap>().GoToPanel(0);
                break;
            case 1: // Invert
                gameRules2.GetComponent<SimpleScrollSnap>().GoToPanel(0);
                break;
        }
        normalBtn.sprite = i == 0 ? sel_s : desel_s;
        invertBtn.sprite = i == 1 ? sel_s : desel_s;
        gameRules1.gameObject.SetActive(i == 0);
        gameRules2.gameObject.SetActive(i == 1);
    }
}
