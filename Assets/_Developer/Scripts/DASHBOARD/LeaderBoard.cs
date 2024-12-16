using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    private static string TAG = ">>>Leaderboard ";

    public static LeaderBoard instance;
    public RectTransform leaderboardPanel;
    public GameObject LeaderboardContent, LeaderboardPrefab;
    public LeaderBoardObject lbPrefab;
    public Image[] TapBack;
    public Sprite[] rankNum, rankBg;
    [SerializeField] ScrollRect leaderBoardScroll;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_LBNEW += HandlerLeaderboard;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_LBNEW -= HandlerLeaderboard;
    }

    private void HandlerLeaderboard(JSONNode data)
    {
        Logger.Print(TAG + " HandlerLeaderboard " + data.ToString());
        AppData.canShowChallenge = false;
        if (!leaderboardPanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(leaderboardPanel, true);
        SetTap(data["type"].AsInt);

        List<ITPPdata> PlayerData = JsonConvert.DeserializeObject<List<ITPPdata>>(data["global_lb"].ToString());
        SetLeaderBoardData(PlayerData);
    }

    int currentTap = -1;
    public void TapClick(int i)
    {
        if (currentTap == i) return;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentTap = i;
        Loading_screen.instance.ShowLoadingScreen(true);
        EventHandler.LeaderBoard(i);
    }

    private void SetTap(int p)
    {
        TapBack[0].sprite = BuddiesScreen.instance.TapSprite[p == 0 ? 0 : 1];  //Global
        TapBack[1].sprite = BuddiesScreen.instance.TapSprite[p == 1 ? 0 : 1];  //Friends
        TapBack[2].sprite = BuddiesScreen.instance.TapSprite[p == 2 ? 0 : 1];  //Hall Of Fame
    }

    public void CloseLeaderboard()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentTap = -1;
        AppData.canShowChallenge = true;
        CommanAnimations.instance.FullScreenPanelAnimation(leaderboardPanel, false);
    }

    private void SetLeaderBoardData(List<ITPPdata> data)
    {
        if (LeaderboardContent.transform.childCount > data.Count)
        {
            for (int i = LeaderboardContent.transform.childCount - 1; i >= data.Count; i--)
            {
                Destroy(LeaderboardContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            LeaderBoardObject currentBoard;
            int tempIndex = i;
            if (LeaderboardContent.transform.childCount < i + 1)
            {
                currentBoard = Instantiate(lbPrefab, LeaderboardContent.transform);
            }
            else
            {
                currentBoard = LeaderboardContent.transform.GetChild(i).gameObject.GetComponent<LeaderBoardObject>();
            }

            //For Rank System...
            Sprite rankBG = (data[i]._id == PrefrenceManager._ID) ? rankBg[3] : (i == 0) ? rankBg[0] : (i == 1) ? rankBg[1] : (i == 2) ? rankBg[2] : rankBg[4];
            currentBoard.rankBG.sprite = rankBG;
            currentBoard.rankImg.enabled = (i < 3 || data[i]._id == PrefrenceManager._ID);
            currentBoard.topRankNum.gameObject.SetActive(!(i < 3 || data[i]._id == PrefrenceManager._ID));
            currentBoard.rankImg.sprite = (data[i]._id == PrefrenceManager._ID) ? rankNum[3] : (i == 0) ? rankNum[0] : (i == 1) ? rankNum[1] : rankNum[2];
            currentBoard.topRankNum.text = (i + 1).ToString();

            StartCoroutine(AppData.ProfilePicSet(data[i].ccf, currentBoard.regionFlag));  //Region Flag...
            StartCoroutine(AppData.ProfilePicSet(data[i].pp, currentBoard.profilePic));  //Profile Pic Set...
            currentBoard.profileBtn.onClick.RemoveAllListeners();
            currentBoard.profileBtn.onClick.AddListener(() =>
            {
                Loading_screen.instance.ShowLoadingScreen(true);
                if (PrefrenceManager._ID.Equals(data[tempIndex]._id)) EventHandler.MyProfile();
                else EventHandler.OpponentUserProfile(data[tempIndex]._id);
            });

            // Trim the player name to 14 characters
            currentBoard.playerNameTxt.text = (data[i].pn.Length >= 14) ? data[i].pn.Substring(0, 14) : data[i].pn;
            
            currentBoard.levelTxt.text = "LEVEL : " + data[i].level.clvl.ToString(); //Level Set...            
            currentBoard.goldTxt.text = AppData.numDifferentiation(data[i].gold);  //Coin Set...                    
        }

        leaderBoardScroll.verticalNormalizedPosition = 1;
        Loading_screen.instance.ShowLoadingScreen(false);
    }
}
