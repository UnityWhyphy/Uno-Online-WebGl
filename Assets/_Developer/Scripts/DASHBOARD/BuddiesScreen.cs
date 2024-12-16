using BestHTTP.Examples;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuddiesScreen : MonoBehaviour
{
    private static string TAG = ">>BUDDIES ";

    public static BuddiesScreen instance;
    public GameObject BuddiesPannel;
    [SerializeField] GameObject commanBuddiesSmallPrefab, commanBuddiesBigPrefab, onlineBuddiesPrefab;
    [SerializeField] GameObject cBSmallContent, cBBigContent, oBContent;
    [SerializeField] GameObject commanBuddiesPanel, onlineBuddiesPanel;
    [SerializeField] Sprite btnEnable, btnDisable;
    public Image[] TapBack;
    public Sprite[] TapSprite;
    [SerializeField] TMP_InputField searchInput;
    [SerializeField] TextMeshProUGUI YourIDText, listEmptyText;
    [SerializeField] ScrollRect oBScroll, bigScroll, smallScroll;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_BH += HandleBH;
        SocketManagergame.OnListner_FFBU += HandleFFBU;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_BH -= HandleBH;
        SocketManagergame.OnListner_FFBU -= HandleFFBU;
    }

    private void HandleFFBU(JSONNode data)
    {
        Logger.Print(TAG + " HandleFFBU called " + data.ToString());
        Loading_screen.instance.ShowLoadingScreen(false);

        ITPPdata SinglePlayer = JsonConvert.DeserializeObject<ITPPdata>(data.ToString());


        List<ITPPdata> SearPlayerId = new List<ITPPdata>();
        SearPlayerId.Add(SinglePlayer);
        listEmptyText.text = "Player Not Found!!";
        listEmptyText.gameObject.SetActive(SearPlayerId.Count == 0);
        SetBuddiesHubData(SearPlayerId, true, true, -1);
    }

    private void HandleBH(JSONNode data)
    {
        Logger.Print(TAG + " Handle BH called " + data.ToString());
        AppData.canShowChallenge = false;

        if (!BuddiesPannel.activeSelf)
            CommanAnimations.instance.FullScreenPanelAnimation(BuddiesPannel, true);
            //CommanAnimations.instance.FullScreenPanelAnimation(BuddiesPannel.GetComponent<RectTransform>(), true);

        SetTap(data["data"]["type"].AsInt);

        List<ITPPdata> PlayerData = JsonConvert.DeserializeObject<List<ITPPdata>>(data["data"]["offline_list"].ToString());

        Logger.Print(TAG + " HandleBh Called " + PlayerData.Count);
        SetBuddiesHubData(PlayerData, data["data"]["type"].AsInt == 3, false, data["data"]["type"].AsInt);
    }

    private void SetTap(int p)
    {
        TapBack[0].sprite = TapSprite[p == 0 ? 0 : 1];
        TapBack[1].sprite = TapSprite[p == 1 ? 0 : 1];
        TapBack[2].sprite = TapSprite[p == 2 ? 0 : 1];
        TapBack[3].sprite = TapSprite[p == 3 ? 0 : 1];

        if (p > 0)
        {
            searchInput.text = "";
            YourIDText.text = "YOUR ID : " + PrefrenceManager.UNIQUE_ID;
        }
        if (p == 3)
        {
            for (int i = 0; i < cBBigContent.transform.childCount; i++)
            {
                Destroy(cBBigContent.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < cBSmallContent.transform.childCount; i++)
            {
                Destroy(cBSmallContent.transform.GetChild(i).gameObject);
            }
        }

        commanBuddiesPanel.SetActive(p < 3 ? false : true);
        onlineBuddiesPanel.SetActive(p == 3 ? false : true);
    }

    int currentTap = -1;
    public void TapClick(int p)
    {
        if (currentTap == p) return;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        currentTap = p;
        switch (p)
        {
            case 0:
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.BuddiesHub(p);
                break;

            case 1:
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.BuddiesHub(p);
                break;

            case 2:
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.BuddiesHub(p);
                break;

            case 3:
                SetTap(3);
                listEmptyText.gameObject.SetActive(true);
                listEmptyText.text = "There Are No Suggestions For You.";
                break;
        }
    }

    public void BuddiesClose()
    {
        //AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentTap = -1;
        AppData.canShowChallenge = true;
        CommanAnimations.instance.FullScreenPanelAnimation(BuddiesPannel.GetComponent<RectTransform>(), false);
    }

    private void SetBuddiesHubData(List<ITPPdata> data, bool isComman, bool isSearchedPlayer, int tap)
    {
        listEmptyText.gameObject.SetActive(data.Count == 0);
        listEmptyText.text = (tap == 0) ? "YOU HAVE NO ANY ONLINE FRIENDS." : tap == 1 ? "Upp! You don't make Any Buddies." : tap == 2 ? "All Good!" : "There Are No Suggestions For You.";

        if (cBSmallContent.transform.childCount > 0 && !isSearchedPlayer) for (int i = 0; i < cBSmallContent.transform.childCount; i++) Destroy(cBSmallContent.transform.GetChild(i).gameObject); // For Search player

        if ((isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform.childCount > data.Count)
        {
            for (int i = (isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform.childCount - 1; i >= data.Count; i--)
            {
                Destroy((isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentBoard;
            int tempIndex = i;

            if ((isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform.childCount < i + 1)
            {
                currentBoard = Instantiate((isComman ? isSearchedPlayer ? commanBuddiesSmallPrefab : commanBuddiesBigPrefab : onlineBuddiesPrefab), (isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform);
            }
            else
            {
                currentBoard = (isComman ? isSearchedPlayer ? cBSmallContent : cBBigContent : oBContent).transform.GetChild(i).gameObject;
            }

            currentBoard.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            currentBoard.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                Loading_screen.instance.ShowLoadingScreen(true);
                if (PrefrenceManager._ID.Equals(data[tempIndex]._id)) EventHandler.MyProfile();
                else EventHandler.OpponentUserProfile(data[tempIndex]._id);
            });
            StartCoroutine(AppData.ProfilePicSet(data[i].pp, currentBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
            currentBoard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].pn.ToString();
            currentBoard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = AppData.numDifferentiation(data[i].gold);
            currentBoard.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();

            if (PrefrenceManager._ID != data[tempIndex]._id)
            {
                if (tap == 0 || tap == 1 || (data[i].MyFreind && !isSearchedPlayer))
                {
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = data[i].tbid.Equals("") ? btnDisable : btnEnable;
                    currentBoard.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "JOIN TABLE";
                    currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = data[i].tbid.Equals("") ? false : true;
                    currentBoard.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = btnDisable;
                        currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = false;
                        Loading_screen.instance.ShowLoadingScreen(true);

                        EventHandler.JoinTableOffriend(data[tempIndex].tbid, data[tempIndex]._id, "bh", false);
                    });

                }
                else if (tap == 2 || (data[i].IBlockOppUser && isSearchedPlayer))
                {
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = btnEnable;
                    currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = true;
                    currentBoard.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UNBLOCK";
                    currentBoard.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        EventHandler.UnblockBlockUser(data[tempIndex]._id);
                        Destroy(currentBoard);
                        //currentBoard.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ADD BUDDY";
                        //currentBoard.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                        //{
                        //    EventHandler.FrienRequestSend(PrefrenceManager._ID, data[tempIndex]._id, data[tempIndex].iscom);
                        //    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = btnDisable;
                        //    currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = false;
                        //});
                    });
                }
                else
                {
                    currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = (data[i].SendFreindRequest || data[i].MyFreind) ? btnDisable : btnEnable;
                    currentBoard.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ADD BUDDY";
                    currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = (data[i].SendFreindRequest || data[i].MyFreind) ? false : true;
                    currentBoard.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        EventHandler.FrienRequestSend(PrefrenceManager._ID, data[tempIndex]._id, data[tempIndex].iscom);
                        currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = btnDisable;
                        currentBoard.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    });
                }
            }
            else
            {
                if (tap > 0) currentBoard.transform.GetChild(4).GetComponent<Image>().sprite = btnDisable;
            }
        }

        oBScroll.verticalNormalizedPosition = 1;
        bigScroll.verticalNormalizedPosition = 1;
        smallScroll.verticalNormalizedPosition = 1;
        Loading_screen.instance.ShowLoadingScreen(false);
    }


    public void SearchIds(TMP_InputField text)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        string enterId = text.text.ToString().Trim();
        if (enterId.Equals(PrefrenceManager.UNIQUE_ID) || enterId.Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData("INVALID ID", "PLEASE ENTER VALID ID");
            return;
        }
        //if (!enterId.Contains("UNO")) enterId = "UNO" + enterId;
        Logger.Print(TAG + " Enter Id " + enterId);

        Loading_screen.instance.ShowLoadingScreen(true);
        EventHandler.FindFrienByUniqueId(enterId);
    }
}
