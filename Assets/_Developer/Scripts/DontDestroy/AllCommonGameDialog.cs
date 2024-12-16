using DG.Tweening;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllCommonGameDialog : MonoBehaviour
{
    private static string TAG = ">>ALLDIALOG ";

    public static AllCommonGameDialog instance;

    [Header("Challenge")]
    public GameObject ChallengeDialog;
    [SerializeField] Image challengeDialogBg, frameImg;
    [SerializeField] Transform challengeDialogPopUp;
    [SerializeField] GameObject privateTableTag;
    public TextMeshProUGUI BetValue, betGems, ModeName, InvitePlayerName, noteText;
    public RawImage InvitePlayerImg;
    [SerializeField] GameObject betGemsObj, plusObj, betGemsTxtObj;
    [SerializeField] Sprite defaultFrame;
    [SerializeField] Texture defaultProfile;


    [Header("Aleart Dialog")]
    [SerializeField] GameObject justOkDialog;
    [SerializeField] Image justOkBg;
    [SerializeField] Transform justOkPopUp;
    [SerializeField] TextMeshProUGUI titleText, messageText, okBtnTxt, noBtnTxt;
    [SerializeField] Button okButton, noButton;
    public GameObject deleteAccountPopup;
    [SerializeField] TextMeshProUGUI deleteMessagetxt;

    [Header("Not Enough Dialog")]
    [SerializeField] GameObject nEDialog;
    [SerializeField] Image nEBg;
    [SerializeField] Transform nEPopUp;
    [SerializeField] TextMeshProUGUI nETitle;
    [SerializeField] TextMeshProUGUI nEMsg;
    [SerializeField] Image nEContentImg;
    [SerializeField] Sprite nECoins, nEGems;

    [Header("Connection Lost PopUp")]
    [SerializeField] internal GameObject cLostPanel;
    [SerializeField] Image cLostBg;
    [SerializeField] Transform cLostPopUp;
    [SerializeField] Image cLcontentImg;
    [SerializeField] Sprite cLnetworkSprite, cLknockSprite, cLMultiLoginSprite;
    [SerializeField] TextMeshProUGUI cLtitleTxt, cLnoteTxt;

    [SerializeField] List<GameObject> closablePanels;

    [Header("Maintainence Mode")]
    [SerializeField] TextMeshProUGUI mM_TimerTxt;
    [SerializeField] RectTransform maintainencePanel;

    private void Awake()
    {
        Logger.Print(TAG + " Awake called");

        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_ITPP += HandleChallengeDialog;
        SocketManagergame.OnListner_LUP += HandleLevelUp;
        SocketManagergame.OnListner_MM += HandleMentenenceMode;
        AppData.AllPanelClose += HandleCloseAllPanels;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_ITPP -= HandleChallengeDialog;
        SocketManagergame.OnListner_LUP -= HandleLevelUp;
        SocketManagergame.OnListner_MM -= HandleMentenenceMode;
        AppData.AllPanelClose -= HandleCloseAllPanels;
    }

    private void Update()
    {
        if (AppData.IsMaintance)
        {
            if (AppData.maintenenceStartsAfter > 0) AppData.maintenenceStartsAfter -= Time.deltaTime;
            else if (AppData.maintenenceStartsAfter <= 0 && AppData.maintenenceEndAfter > 0) AppData.maintenenceEndAfter -= Time.deltaTime;

            DoMaintenenceAction();
        }

        AppData.magicBonusTime -= Time.deltaTime;
    }

    private void HandleLevelUp(JSONNode data)
    {
        Logger.Print($"HandleLevelUp call");
        FirebaseData.EventSendWithFirebaseLevel("Levelup", "level", data["new_level"].AsInt - 1);
        AppData.currantLvl = data["new_level"].AsInt;

        StaticData.LEVELUPDATA = data.ToString();

        DashboardManager.instance.HandleLockLevelWise();
    }

    JSONNode ChallengeData;
    private void HandleChallengeDialog(JSONNode data)
    {
        ResetChallengePopUp();

        if (PrefrenceManager.Ch == 0)
        {
            if (!AppData.canShowChallenge) return;

            AudioManager.instance.AudioPlay(AudioManager.instance.challengePopUp);
            ChallengeData = "";
            Logger.Print(TAG + " HandleChallengeDialog called ");
            ChallengeData = data;

            StartCoroutine(AppData.ProfilePicSet(data["pp"], InvitePlayerImg));
            StartCoroutine(AppData.SpriteSetFromURL(data["frameImg"], frameImg, "HandleChallengeDialog"));
            InvitePlayerName.text = data["un"];
            seatIndexTemp = data["si"];
            Logger.Print($"Viren SI is Torre:::: {seatIndexTemp} == {data["si"]}");

            noteText.text = data["title"];
            BetValue.text = (data["gems"].AsLong > 0) ? AppData.PrivateTableBootVal(data["bv"].AsLong) : AppData.numDifferentiation(data["bv"].AsLong);
            betGems.text = AppData.numDifferentiation(data["gems"].AsLong);
            betGemsObj.SetActive(data["gems"].AsLong > 0);
            plusObj.SetActive(data["gems"].AsLong > 0);
            betGemsTxtObj.SetActive(data["gems"].AsLong > 0);
            ModeName.text = data["mode"];
            privateTableTag.SetActive(data["ip"].AsInt == 1);
            CommanAnimations.instance.PopUpAnimation(ChallengeDialog, challengeDialogBg, challengeDialogPopUp, Vector3.one, true);
            StartCoroutine(AutoCloseChallenge());
        }
    }

    public int seatIndexTemp = -1;

    private void ResetChallengePopUp()
    {
        InvitePlayerImg.texture = defaultProfile;
        frameImg.sprite = defaultFrame;
        InvitePlayerName.text = "PLAYER";
        BetValue.text = "";
        ModeName.text = "";
        privateTableTag.SetActive(false);
    }

    public void SetJustOkDialogData(string title, string message, int listenerIndex = 0, bool isTwoBtn = false, string okBtnText = "OK", string NoBtnText = "Not Now")
    {
        titleText.text = title;
        messageText.text = message;
        noButton.gameObject.SetActive(isTwoBtn);
        okBtnTxt.text = okBtnText;
        noBtnTxt.text = NoBtnText;
        okButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() => AleartPopUp_OkPress(listenerIndex));
        noButton.onClick.AddListener(() => AleartPopUp_NoPress(listenerIndex));
        Logger.NormalLog($"EG SetJustOkDialogData 1: {justOkDialog.gameObject.name}");
        if (!justOkDialog.activeInHierarchy)
            CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkBg, justOkPopUp, Vector3.one, true);
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public void SetDeleteDialogData(string message)
    {
        deleteMessagetxt.text = message;
        AudioManager.instance.AudioPlay(AudioManager.instance.aleartPopUp);
        deleteAccountPopup.gameObject.SetActive(true);
        Loading_screen.instance.ShowLoadingScreen(false);
    }


    public void YesDeletClick()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.InstaNTDeleteReq();
        Application.Quit();
    }

    public void AleartPopUp_OkPress(int index)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        switch (index)
        {
            case 0: //Normal Ok Click
                CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkBg, justOkPopUp, Vector3.zero, false, false);
                break;
            case 1: // Reactive Account
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.UpdateCFD(2);
                break;

        }
    }

    public void AleartPopUp_NoPress(int index)
    {
        switch (index)
        {
            case 0: //Normal Ok Click
                CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkBg, justOkPopUp, Vector3.zero, false);
                break;
            case 1: // Not Now On Delete Account
                Logger.Print(TAG + " Application.Quit() Called");
                Application.Quit();
                break;

        }
    }

    public void ShowNotEnoughDialog(bool isGems, long requiredGold = 0, long requiredGems = 0, bool isPlaying = false)
    {

        if (isPlaying)
        {
            nETitle.text = isGems ? "NOT ENOUGH GEMS?" : "NOT ENOUGH GOLD?";
            nEMsg.text = isGems ? "You need more Gems to switch table. Earn or purchase more and try again!" : "You need more Gold to switch table.  Earn or purchase more and try again!";
            nEContentImg.sprite = isGems ? nEGems : nECoins;
            CommanAnimations.instance.PopUpAnimation(nEDialog, nEBg, nEPopUp, Vector3.one, true);
        }
        else
        {
            OfferPopupController.offerSlotCall?.Invoke(isGems, requiredGold, requiredGems);
        }


        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public bool isHaveGoldGems(long requiredGold, long requiredGems, bool isPlaying = false/*bool isWinPanel = false*/)
    {
        if (requiredGold > long.Parse(PrefrenceManager.GOLD))
        {
            ShowNotEnoughDialog(false, requiredGold, 0, isPlaying); // Added

            return false;
        }
        else if (requiredGems > long.Parse(PrefrenceManager.GEMS))
        {
            ShowNotEnoughDialog(true, 0, requiredGems, isPlaying);// Added

            return false;
        }
        else return true;
    }

    public void OnClick_GoToStore()
    {
        CommanAnimations.instance.PopUpAnimation(nEDialog, nEBg, nEPopUp, Vector3.zero, false, false);
        DashboardManager.instance.TopCenterClick(2);
    }

    public void OnClick_CloseNE()
    {
        CommanAnimations.instance.PopUpAnimation(nEDialog, nEBg, nEPopUp, Vector3.zero, false, false);
    }

    public void ChallengeBtnClick(int i)
    {
        switch (i)
        {
            case 0://accept
                if (isHaveGoldGems(ChallengeData["bv"].AsLong, ChallengeData["gems"].AsLong))
                {
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.JoinTableOffriend(ChallengeData["tbid"], ChallengeData["s"], "noti", ChallengeData["isauto"], seatIndexTemp);
                }
                break;

            case 1://reject
                EventHandler.RemoveNotificationDashboard(ChallengeData["s"], ChallengeData["tbid"]);
                break;

            case 2://never
                PrefrenceManager.Ch = 1;
                JSONObject data = new JSONObject();
                data.Add("_ch", PrefrenceManager.Ch);
                EventHandler.UpdateUserProfile(data);
                break;

            case 3://close
                break;
        }
        if (i != 0) CommanAnimations.instance.PopUpAnimation(ChallengeDialog, challengeDialogBg, challengeDialogPopUp, Vector3.zero, false);
    }

    private IEnumerator AutoCloseChallenge()
    {
        yield return new WaitForSeconds(5f);

        if (ChallengeDialog.activeSelf)
            CommanAnimations.instance.PopUpAnimation(ChallengeDialog, challengeDialogBg, challengeDialogPopUp, Vector3.zero, false);
    }

    [SerializeField] Button retryBtn;
    Coroutine animateConnectionText;
    Tween noInternetTween;

    public void ShowConnectionPopUp(bool isKnockKnockPopUp, bool isMultiLogin = false)
    {
        Logger.Print(TAG + " Connection Lost na Pop Up Ni Method Ma Gayu >> ");
        AppData.AllPanelClose?.Invoke();
        CreateTable.instance?.ResetLiveTableKey();
        Loading_screen.instance.ShowLoadingScreen(false);
        cLtitleTxt.text = isKnockKnockPopUp ? "KNOCK KNOCK!" : isMultiLogin ? "Identifying Suspicious!" : "CONNECTION LOST!";
        cLcontentImg.sprite = isKnockKnockPopUp ? cLknockSprite : isMultiLogin ? cLMultiLoginSprite : cLnetworkSprite;
        cLnoteTxt.text = isKnockKnockPopUp ? "ARE YOU THERE? WE HAVEN'T RECEIVED ANY ACTION FROM YOU IN THE LAST 10 MINUTES."
            //: isMultiLogin ? "Please check your account, its currently logged on another device. Thank you!" : "YOUR CONNECTION IS NOT STABLE, Trying to reconnect…";
            : isMultiLogin ? "Please check your account, its currently logged on another device. Thank you!" : "";

        noInternetTween = cLcontentImg.transform.DOScale(new Vector2(1.2f, 1.2f), 1).SetLoops(-1, LoopType.Yoyo);
        if (!isKnockKnockPopUp && !isMultiLogin)
        {
            animateConnectionText = StartCoroutine(AnimateConnectionText());
        }
        retryBtn.gameObject.SetActive(isKnockKnockPopUp);


        CommanAnimations.instance.PopUpAnimation(cLostPanel, cLostBg, cLostPopUp, Vector3.one, true);
    }
    IEnumerator AnimateConnectionText()
    {
        string[] loadingTexts = new string[]
        {
        "YOUR CONNECTION IS NOT STABLE, Trying to reconnect.",
        "YOUR CONNECTION IS NOT STABLE, Trying to reconnect..",
        "YOUR CONNECTION IS NOT STABLE, Trying to reconnect...",
        "YOUR CONNECTION IS NOT STABLE, Trying to reconnect...."
        };

        int index = 0;
        while (true)
        {
            cLnoteTxt.text = loadingTexts[index];
            yield return new WaitForSeconds(.5f);
            index = (index + 1) % loadingTexts.Length;
        }
    }

    internal void HideConnectionPopup()
    {
        if (cLostPanel.activeInHierarchy)
        {
            cLostPanel.gameObject.SetActive(false);
            CommanAnimations.instance.PopUpAnimation(cLostPanel, cLostBg, cLostPopUp, Vector3.zero, false);
        }
        if (animateConnectionText != null)
            StopCoroutine(animateConnectionText);
        noInternetTween?.Kill();
    }

    public void OnClick_CL_Retry()
    {
        SceneManager.LoadScene(EventHandler.SPLASH);
        HideConnectionPopup();
        StartCoroutine(SocketManagergame.Instance.FindServerCallWait());
        Logger.NormalLog($"OnClick_CL_Retry INvoke  OnDisc == CheckInternetConnection");
        SocketManagergame.Instance.CancelInvoke(nameof(SocketManagergame.Instance.CheckInternetConnection));

    }

    private void HandleCloseAllPanels()
    {
        for (int i = 0; i < closablePanels.Count; i++) closablePanels[i].gameObject.SetActive(false);
    }

    public void ShowUnderMaintainencePanel(bool isShow)
    {
        mM_TimerTxt.text = AppData.GetTimeInFormateHr((long)AppData.maintenenceEndAfter * 1000);

        if (!maintainencePanel.gameObject.activeInHierarchy)
        {
            Logger.Print(TAG + " Splash Scene Loaded From Maintenence Mode >>> ");
            CommanAnimations.instance.FullScreenPanelAnimation(maintainencePanel, true);

            AppData.AllPanelClose?.Invoke();
            SocketManagergame.SocketDisConnectManually();
            SocketManagergame.isReconnect = true;
            if (!SceneManager.GetActiveScene().name.Equals(EventHandler.SPLASH))
                SceneManager.LoadScene(EventHandler.SPLASH);
        }
        else if (!isShow)
        {
            Logger.Print(TAG + " Maintenence Over >> ");
            CommanAnimations.instance.FullScreenPanelAnimation(maintainencePanel, false);
            AppData.maintenenceEndAfter = 0;
            AppData.maintenenceStartsAfter = 0;
            SocketManagergame.isReconnect = false;
            SceneManager.LoadScene(EventHandler.SPLASH);
        }
        SocketManagergame.isSingUpGot = false;
    }

    public void ShowMaintenenceToolTip(bool isShow)
    {
        if (SceneManager.GetActiveScene().name.Equals(EventHandler.DASHBOARD))
        {
            DashboardManager.instance.maintenenceToolTip.SetActive(isShow);
            DashboardManager.instance.maintenenceTimerTxt.text = "Please ensure that the game undergoes maintenance after " + AppData.GetTimeInFormateHr((long)AppData.maintenenceStartsAfter * 1000);
        }
        else if (SceneManager.GetActiveScene().name.Equals(EventHandler.GAMEPLAY))
        {
            GameManager.instance.maintenenceToolTip.SetActive(isShow);
            GameManager.instance.maintenenceTimerTxt.text = "Please ensure that the game undergoes maintenance after " + AppData.GetTimeInFormateHr((long)AppData.maintenenceStartsAfter * 1000);
        }
    }

    public void HandleMentenenceMode(JSONNode data)
    {
        AppData.IsMaintance = data["RemoveAfterTimer"] > 0;
        AppData.maintenenceStartsAfter = data["StartAfterTimer"];
        AppData.maintenenceEndAfter = data["RemoveAfterTimer"];
        DoMaintenenceAction();
    }

    private void DoMaintenenceAction()
    {
        if (AppData.maintenenceStartsAfter > 0) ShowMaintenenceToolTip(true);
        else if (AppData.maintenenceEndAfter > 0 && AppData.maintenenceStartsAfter <= 0) ShowUnderMaintainencePanel(true);
        else
        {
            AppData.IsMaintance = false;
            ShowMaintenenceToolTip(false);
            ShowUnderMaintainencePanel(false);
        }
    }
}
