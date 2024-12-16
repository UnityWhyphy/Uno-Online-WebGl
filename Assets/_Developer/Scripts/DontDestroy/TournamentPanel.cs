using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TournamentPanel : MonoBehaviour
{
    public static TournamentPanel instance;
    private string TAG = " TOURNAMENT PANEL >>> ";

    [SerializeField] internal RectTransform tournamentPanel, t_SlotesPanel, t_MappingPanel;

    [SerializeField] internal GameObject t_Info_Panel, t_Timer_Panel;
    [SerializeField] Image t_InfoBg, t_TimerBg;
    [SerializeField] Transform t_InfoPopUp, t_TimerPopUp;

    [Header("Tournament Slotes")]
    [SerializeField] Transform t_SlotesContent;
    [SerializeField] GameObject t_SlotePrefab;
    [SerializeField] TournamentSlotInfo t_SlotePrefabObject;
    [SerializeField] GameObject tInfoBtn;
    [SerializeField] ScrollRect t_ScrollRect;


    [SerializeField] List<TeamDetails> r1Teams = new List<TeamDetails>();
    [SerializeField] TeamDetails r2Team = new TeamDetails();
    [SerializeField] Texture defaultUser;
    [SerializeField] Sprite defaultFrame;
    [SerializeField] List<GameObject> tableGlows;

    [Header("Tounament Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] PlayerData t_WinnerData;
    [SerializeField] GameObject startNewTourBtn;
    [SerializeField] TextMeshProUGUI winAmount;
    [SerializeField] GameObject winCoinsObj;


    int bv, winbv = 0;
    string mode, tournamentID = "";
    bool isSecondRound = false;
    bool isTimerStart = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_LTNEW += HandleTournamentSlotes;
        SocketManagergame.OnListner_TD += HandleTournamentData;
        SocketManagergame.OnListner_TJ += HandleTournamentJoin;
        SocketManagergame.OnListner_EGT += HandleExitTournament;
        SocketManagergame.OnListner_TIMER += HandleTournamentTimer;
        SocketManagergame.OnListner_SRJU += HandleSecondRoundJoin;
        SocketManagergame.OnListner_WINTOUR += HandleWinTournament;
        SocketManagergame.OnListner_GST += OnRecevied_GST;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_LTNEW -= HandleTournamentSlotes;
        SocketManagergame.OnListner_TD -= HandleTournamentData;
        SocketManagergame.OnListner_TJ -= HandleTournamentJoin;
        SocketManagergame.OnListner_EGT -= HandleExitTournament;
        SocketManagergame.OnListner_TIMER -= HandleTournamentTimer;
        SocketManagergame.OnListner_SRJU -= HandleSecondRoundJoin;
        SocketManagergame.OnListner_WINTOUR -= HandleWinTournament;
        SocketManagergame.OnListner_GST -= OnRecevied_GST;
    }

    private void OnRecevied_GST(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_GST called");
        if (data == null) return;// null means new user

        if (data.HasKey("table") && data["table"].Count > 0 && !GameManager.instance.playingScreen.activeInHierarchy)
        {
            AppData.IsRejoin = false;
            Logger.Print(TAG + " Playing ma data send");
            SocketManagergame.OnListner_GTI?.Invoke(data["table"]);
            CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, true);
            GameManager.instance.allPlayingScreen.gameObject.SetActive(true);
        }
        for (int i = 0; i < tableGlows.Count; i++) tableGlows[i].SetActive(true);
    }

    private void HandleTournamentSlotes(JSONNode data)
    {
        Logger.Print(TAG + "Handle Tournament Slotes Called...");
        List<TournamentSlote> tournamentSlotes = JsonConvert.DeserializeObject<List<TournamentSlote>>(data["bootvalue"].ToString());
        isWin = false;
        if (t_SlotesContent.childCount > tournamentSlotes.Count)
        {
            for (int i = t_SlotesContent.childCount - 1; i >= tournamentSlotes.Count; i--)
            {
                Destroy(t_SlotesContent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < tournamentSlotes.Count; i++)
        {
            TournamentSlotInfo currentBoard;
            int tempIndex = i;
            if (t_SlotesContent.childCount < i + 1)
            {
                currentBoard = Instantiate(t_SlotePrefabObject, t_SlotesContent);
                currentBoard.name = $"TourSlot_{i}";
            }
            else
            {
                currentBoard = t_SlotesContent.GetChild(i).GetComponent<TournamentSlotInfo>();
            }
            currentBoard.bootValueTxt.text = AppData.numDifferentiation(tournamentSlotes[i].bv);
            currentBoard.winValueTxt.text = AppData.numDifferentiation(tournamentSlotes[i].winbv);

            if (tournamentSlotes[i].gems == 0)
            {
                currentBoard.gemsValueTxt.gameObject.SetActive(false);
                currentBoard.gemsImg.gameObject.SetActive(false);
                currentBoard.plusImg.gameObject.SetActive(false);
            }

            currentBoard.gemsValueTxt.text = AppData.numDifferentiation(tournamentSlotes[i].gems);

            currentBoard.playBtn.onClick.RemoveAllListeners();
            currentBoard.playBtn.onClick.AddListener(() =>
            {
                if (AllCommonGameDialog.instance.isHaveGoldGems(tournamentSlotes[tempIndex].bv, tournamentSlotes[tempIndex].gems))
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.PlayTournament(tournamentSlotes[tempIndex].bv, AppData.CLASSIC, tournamentSlotes[tempIndex].winbv, "", tournamentSlotes[tempIndex].gems);
                    AppData.canShowChallenge = false;
                }
            });
        }
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!tournamentPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, true);

        CommanAnimations.instance.FullScreenPanelAnimation(t_SlotesPanel, true);
        CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.zero, false, false);
        CommanAnimations.instance.PopUpAnimation(t_Timer_Panel, t_TimerBg, t_TimerPopUp, Vector3.zero, false, false);
        CommanAnimations.instance.FullScreenPanelAnimation(t_MappingPanel, false);
        t_ScrollRect.verticalNormalizedPosition = 1;
        tInfoBtn.SetActive(true);
        if (SceneManager.GetActiveScene().name != EventHandler.DASHBOARD)
        {
            AppData.FromShowIntro = true;
        }
    }

    [SerializeField] List<Player> r1Players = new List<Player>();
    [SerializeField] List<Player> r2Players = new List<Player>();
    private void HandleTournamentData(JSONNode data)
    {
        r1Players.Clear();
        r2Players.Clear();
        for (int i = 0; i < tableGlows.Count; i++) tableGlows[i].SetActive(false);

        if (SceneManager.GetActiveScene().name != EventHandler.DASHBOARD)
        {
            AppData.FromShowIntro = true;
            SceneManager.LoadScene(EventHandler.DASHBOARD);
        }

        ResetMappingPanel();
        Logger.Print(TAG + "Handle Tournament Data Called...");
        mode = data["mode"];
        bv = data["bv"];
        winbv = data["winbv"];
        tournamentID = data["_id"];
        isTimerStart = data["sts"] == "RoundTimerStart";

        SetPlayerData(data, true);

        isSecondRound = r2Players.Count > 0;
        Loading_screen.instance.ShowLoadingScreen(false);

        if (!tournamentPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, true);
        if (SceneManager.GetActiveScene().name.Equals(EventHandler.DASHBOARD) && LiveTablePanel.Instance.liveTablePanel.gameObject.activeInHierarchy)
            LiveTablePanel.Instance.liveTablePanel.gameObject.SetActive(false);
        t_SlotesPanel.gameObject.SetActive(false);
        CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.zero, false, false);
        Logger.NormalLog($"VVV::: Panel is ON ? => {t_MappingPanel.gameObject.activeInHierarchy} ");
        if (!t_MappingPanel.gameObject.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(t_MappingPanel, true);
        tInfoBtn.SetActive(false);
        startNewTourBtn.SetActive(false);
    }

    private void SetPlayerData(JSONNode data, bool isTD)
    {
        for (int i = 0; i < data["round1"].Count; i++)//Round 1 Teams
        {
            for (int player = 0; player < data["round1"][i]["player"].Count; player++)
            {
                r1Teams[i].Players[player].loader.SetActive(data["round1"][i]["player"][player].Count == 0);

                if (data["round1"][i]["player"][player].Count == 0) continue;
                Player single = JsonConvert.DeserializeObject<Player>(data["round1"][i]["player"][player].ToString());
                StartCoroutine(AppData.ProfilePicSet(single.pp, r1Teams[i].Players[player].playerImg));
                //StartCoroutine(AppData.SpriteSetFromURL(single.frameImg, r1Teams[i].Players[player].frameImg, "SetPlayerData 1"));
                r1Teams[i].Players[player].youTag.SetActive(PrefrenceManager._ID.Equals(single.uid));
                r1Teams[i].Players[player].playerName.text = single.pn;
                Logger.Print(TAG + "Round 1 >> Team >> " + i + " Player >> " + player + " R1Players List Count >>" + r1Players.Count);
                if (isTD) r1Players.Add(single);
            }
        }

        for (int i = 0; i < data["round2"].Count; i++)//Round 2 Teams...
        {
            for (int player = 0; player < data["round2"][i]["player"].Count; player++)
            {
                r2Team.Players[player].loader.SetActive(data["round2"][i]["player"][player].Count == 0);

                if (data["round2"][i]["player"][player].Count == 0) continue;
                Player single = JsonConvert.DeserializeObject<Player>(data["round2"][i]["player"][player].ToString());
                StartCoroutine(AppData.ProfilePicSet(single.pp, r2Team.Players[player].playerImg));
                StartCoroutine(AppData.SpriteSetFromURL(single.frameImg, r2Team.Players[player].frameImg, "SetPlayerData 2"));
                r2Team.Players[player].youTag.SetActive(PrefrenceManager._ID.Equals(single.uid));
                r2Team.Players[player].playerName.text = single.pn;
                if (isTD) r2Players.Add(single);
                tableGlows[single.si].SetActive(false);
                Logger.Print(TAG + "Round 2 >> Team >> " + i + " Player >> " + player);
            }
        }
    }

    private void HandleTournamentJoin(JSONNode data)
    {
        Player currentPlayer = JsonConvert.DeserializeObject<Player>(data["ui"].ToString());
        r1Teams[currentPlayer.group].Players[currentPlayer.si].loader.SetActive(false);
        if (PrefrenceManager._ID.Equals(currentPlayer.uid)) return;

        if (!t_MappingPanel.gameObject.activeInHierarchy)
        {
            CommanAnimations.instance.FullScreenPanelAnimation(t_MappingPanel, true);
            t_SlotesPanel.gameObject.SetActive(false);
        }

        StartCoroutine(AppData.ProfilePicSet(currentPlayer.pp, r1Teams[currentPlayer.group].Players[currentPlayer.si].playerImg));
        //StartCoroutine(AppData.SpriteSetFromURL(currentPlayer.frameImg, r1Teams[currentPlayer.group].Players[currentPlayer.si].frameImg, "HandleTournamentJoin"));
        r1Teams[currentPlayer.group].Players[currentPlayer.si].youTag.SetActive(PrefrenceManager._ID.Equals(currentPlayer.uid));
        r1Teams[currentPlayer.group].Players[currentPlayer.si].playerName.text = currentPlayer.pn;
        r1Players.Add(currentPlayer);
    }

    private void HandleTournamentTimer(JSONNode data)
    {
        isTimerStart = true;
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!tournamentPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, true);
        CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.zero, false, false);
        CommanAnimations.instance.PopUpAnimation(t_Timer_Panel, t_TimerBg, t_TimerPopUp, Vector3.one, true, false);
        StartCoroutine(StartTimer(data["time"].AsInt - 1));
    }

    IEnumerator StartTimer(int time)
    {
        Logger.Print(TAG + "Tornament Timer : " + time);
        timerText.text = time.ToString();
        for (int i = time - 1; i >= 0; i--)
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.countDown);
            yield return new WaitForSeconds(1f);
            timerText.text = (i == 0) ? "GO!" : i.ToString();
        }

        yield return new WaitForSeconds(0.5f);
        Loading_screen.instance.ShowLoadingScreen(SceneManager.GetActiveScene().name.Equals(EventHandler.DASHBOARD));
        StopCoroutine(StartTimer(time));
    }

    private void HandleSecondRoundJoin(JSONNode data)
    {
        Logger.Print(TAG + "Is Secound Round : " + isSecondRound);
        if (isSecondRound)
        {
            Player currentPlayer = JsonConvert.DeserializeObject<Player>(data.ToString());
            r2Team.Players[currentPlayer.si].loader.SetActive(false);
            if (PrefrenceManager._ID.Equals(currentPlayer.uid)) return;
            StartCoroutine(AppData.ProfilePicSet(currentPlayer.pp, r2Team.Players[currentPlayer.si].playerImg));
            StartCoroutine(AppData.SpriteSetFromURL(currentPlayer.frameImg, r2Team.Players[currentPlayer.si].frameImg, "HandleSecondRoundJoin"));
            r2Team.Players[currentPlayer.si].youTag.SetActive(PrefrenceManager._ID.Equals(currentPlayer.uid));
            r2Team.Players[currentPlayer.si].playerName.text = currentPlayer.pn;
            tableGlows[currentPlayer.si].SetActive(false);
            r2Players.Add(currentPlayer);
        }
    }

    bool isWin = false;
    private void HandleWinTournament(JSONNode data)
    {
        SetPlayerData(data["tourdata"], false);
        Logger.Print(TAG + "Handle Win Tour Called ..... ");
        winCoinsObj.SetActive(true);
        winAmount.text = AppData.numDifferentiation(data["winbv"].AsLong);
        Ui currentPlayer = JsonConvert.DeserializeObject<Ui>(data["data"][0].ToString());
        StartCoroutine(AppData.ProfilePicSet(currentPlayer.pp, t_WinnerData.playerImg));
        StartCoroutine(AppData.SpriteSetFromURL(currentPlayer.frameImg, t_WinnerData.frameImg, "HandleWinTournament"));
        t_WinnerData.youTag.SetActive(PrefrenceManager._ID.Equals(currentPlayer.uid));
        t_WinnerData.playerName.text = currentPlayer.pn;
        isWin = true;
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!tournamentPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, true);
        CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.zero, false, false);
        Logger.NormalLog($"VVV::: Panel is ON ? => {t_MappingPanel.gameObject.activeInHierarchy} ");
        CommanAnimations.instance.FullScreenPanelAnimation(t_MappingPanel, true);
        startNewTourBtn.SetActive(true);

        Logger.Print($"HandleWinTournament AppData.isReviewAd  = {AppData.isReviewAd }");
        if (DashboardManager.instance.totalMatch >= 5 && AppData.isReviewAd >= 0)
        {
            StartCoroutine(AppReview.Instance.ReviewPopup());
            AppData.isReviewAd--;
        }
        FirebaseData.EventSendWithFirebase("TournamentWin");

        AdmobManager.instance.ShowInterstitialAd(); //
    }

    private void HandleExitTournament(JSONNode data)
    {
        Logger.Print(TAG + "Exit Tournament Called...");

        if (data["uid"].Equals(PrefrenceManager._ID))
        {
            if (SceneManager.GetActiveScene().name != EventHandler.DASHBOARD)
            {
                AppData.FromShowIntro = true;
                SceneManager.LoadScene(EventHandler.DASHBOARD);
            }

            Loading_screen.instance.ShowLoadingScreen(false);
            if (LiveTablePanel.Instance.liveTablePanel.gameObject.activeInHierarchy) LiveTablePanel.Instance.LiveTablePanelClick(2, false);
            tournamentPanel.gameObject.SetActive(false);
            CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, false);
            if (GameManager.instance.playingScreen.activeInHierarchy)
            {
                GameManager.instance.allPlayingScreen.gameObject.SetActive(false);
                CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, false, 0);
                Loading_screen.instance.ShowLoadingScreen(false);
            }
        }
        else
        {
            Ui currentPlayer = JsonConvert.DeserializeObject<Ui>(data.ToString());
            PlayerData exitedplayer = (currentPlayer.status == 1) ? r1Teams[currentPlayer.group].Players[currentPlayer.si] : r2Team.Players[currentPlayer.si];
            if (currentPlayer.status == 1)
            {
                for (int player = 0; player < r1Players.Count; player++)
                    if (r1Players[player].uid.Equals(currentPlayer.uid)) r1Players.RemoveAt(player);
            }
            else
            {
                for (int player = 0; player < r2Players.Count; player++)
                    if (r2Players[player].uid.Equals(currentPlayer.uid)) r2Players.RemoveAt(player);
            }
            exitedplayer.playerImg.texture = defaultUser;
            exitedplayer.frameImg.sprite = defaultFrame;
            exitedplayer.playerName.text = "PLAYER";
            exitedplayer.youTag.SetActive(false);
        }
    }

    public void TournamentClick(int i)
    {
        switch (i)
        {
            case 0://close
                Logger.NormalLog($" Is click ?> {TAG}");
                if (!t_MappingPanel.gameObject.activeInHierarchy || isWin)
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    Logger.Print(TAG + "Mapping Panel Is Of  Or IsWin IS >> " + isWin);
                    CommanAnimations.instance.FullScreenPanelAnimation(tournamentPanel, false);
                    GameManager.instance.DashBoardOn();
                    EventHandler.LgsReqSend();

                    if (AppData.istourShowFeed && isWin)
                    {
                        AppData.istourShowFeed = false;
                        setting_script.instance.FeedbackPopupShow(true); // Tour win close
                    }
                }
                else
                {
                    if (isSecondRound ? r2Players.Count < 4 : r1Players.Count < 14 && !isTimerStart)
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        Logger.Print(TAG + "Is Second Round  >> " + isSecondRound + "R2 Players >> " + r2Players.Count + "R1 Players Count" + r1Players.Count);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.ExitTournament(tournamentID);
                    }
                    AppData.canShowChallenge = true;
                }
                break;

            case 1://Tournament Info
                CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.one, true);
                break;

            case 2://Start New Tournament
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                t_MappingPanel.gameObject.SetActive(false);
                CommanAnimations.instance.FullScreenPanelAnimation(t_MappingPanel, false);
                EventHandler.ListofTournament();
                EventHandler.LgsReqSend();
                break;
            case 3:
                CommanAnimations.instance.PopUpAnimation(t_Info_Panel, t_InfoBg, t_InfoPopUp, Vector3.zero, false, false);
                break;
        }
    }

    private void ResetMappingPanel()
    {
        for (int i = 0; i < r1Teams.Count; i++)
        {
            for (int player = 0; player < r1Teams[i].Players.Count; player++)
            {
                PlayerData data = r1Teams[i].Players[player];
                data.playerImg.texture = defaultUser;
                data.frameImg.sprite = defaultFrame;
                data.playerName.text = "PLAYER";
                data.youTag.SetActive(false);
                data.loader.SetActive(true);
            }
        }

        for (int player = 0; player < r2Team.Players.Count; player++)
        {
            PlayerData data = r2Team.Players[player];
            data.playerImg.texture = defaultUser;
            data.frameImg.sprite = defaultFrame;
            data.playerName.text = "PLAYER";
            data.youTag.SetActive(false);
            data.loader.SetActive(false);
        }

        t_WinnerData.playerImg.texture = defaultUser;
        t_WinnerData.frameImg.sprite = defaultFrame;
        t_WinnerData.playerName.text = "PLAYER";
        t_WinnerData.youTag.SetActive(false);
        winCoinsObj.SetActive(false);
    }
}

[System.Serializable]
class TeamDetails
{
    public List<PlayerData> Players = new List<PlayerData>();
}

[System.Serializable]
class PlayerData
{
    public RawImage playerImg;
    public Image frameImg;
    public GameObject youTag;
    public TextMeshProUGUI playerName;
    public GameObject loader;
}
