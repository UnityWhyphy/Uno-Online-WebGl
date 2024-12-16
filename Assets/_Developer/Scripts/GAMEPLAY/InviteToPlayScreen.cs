using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InviteToPlayScreen : MonoBehaviour
{
    public static InviteToPlayScreen instance;

    public GameObject inviteToPlayPanel;
    [SerializeField] Sprite[] TapSprite;
    [SerializeField] Image[] TapBack;
    [SerializeField] GameObject searchPanel, buddyScrollView;
    [SerializeField] InviteBuddy inviteBuddyPrefab;
    [SerializeField] GameObject playersContent;
    [SerializeField] TMP_InputField searchInputField;
    [SerializeField] GameObject inviteAllBtnObj;
    [SerializeField] Button inviteAllBtn;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_ITPL += HandleInviteToPlayerList;
        SocketManagergame.OnListner_FFBU += HandleFFBU;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_ITPL -= HandleInviteToPlayerList;
        SocketManagergame.OnListner_FFBU -= HandleFFBU;
    }

    List<ITPPdata> allPlayerList, FriensList, SearPlayerId;
    private void HandleInviteToPlayerList(JSONNode data)
    {
        allPlayerList = JsonConvert.DeserializeObject<List<ITPPdata>>(data["pl_list"].ToString());
        FriensList = JsonConvert.DeserializeObject<List<ITPPdata>>(data["fr_list"].ToString());

        TapClick(0);
    }

    private void HandleFFBU(JSONNode data)
    {
        Logger.Print(" HandleFFBU called " + data.ToString());

        ITPPdata SinglePlayer = JsonConvert.DeserializeObject<ITPPdata>(data.ToString());

        SearPlayerId = new List<ITPPdata>();
        SearPlayerId.Add(SinglePlayer);
        SetScrollViewData(SearPlayerId);
    }

    private void SetScrollViewData(List<ITPPdata> data)
    {
        Logger.Print($" SetScrollViewData called : {playersContent.transform.childCount}");

        for (int i = playersContent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(playersContent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < data.Count; i++)
        {
            var player = data[i];

            var currentBoard = Instantiate(inviteBuddyPrefab, playersContent.transform);
            currentBoard.index = i;
            StartCoroutine(AppData.ProfilePicSet(player.pp, currentBoard.pic));

            // Set up player details
            currentBoard.playerName.text = player.pn;
            currentBoard.inviteImg.sprite = TapSprite[2];
            currentBoard.inviteBtn.interactable = true;

            // Add button listener
            currentBoard.inviteBtn.onClick.RemoveAllListeners();
            currentBoard.inviteBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

                int index = currentBoard.index;

                if (!string.IsNullOrEmpty(data[index]._id))
                    GameManager.instance.OnClick_Invite(data[index]._id);

                currentBoard.inviteImg.sprite = TapSprite[3];
                currentBoard.inviteBtn.interactable = false;
            });
        }

        Loading_screen.instance.ShowLoadingScreen(false);
        if (!inviteToPlayPanel.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(inviteToPlayPanel.GetComponent<RectTransform>(), true);
    }

    int currentTap = -1;
    public void TapClick(int p)
    {
        if (currentTap == p) return;

        currentTap = p;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        TapBack[0].sprite = TapSprite[p == 0 ? 0 : 1];
        TapBack[1].sprite = TapSprite[p == 1 ? 0 : 1];
        TapBack[2].sprite = TapSprite[p == 2 ? 0 : 1];

        if (p == 0) SetScrollViewData(allPlayerList);
        else if (p == 1) SetScrollViewData(FriensList);

        if (p == 2)
        {
            for (int i = playersContent.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(playersContent.transform.GetChild(i).gameObject);
            }
        }
        searchPanel.SetActive(p == 2);
        if (p == 2) searchInputField.text = string.Empty;
        buddyScrollView.GetComponent<RectTransform>().offsetMax = p == 2 ? new Vector2(-800f, buddyScrollView.GetComponent<RectTransform>().offsetMin.y) : Vector2.zero;
    }

    public void CloseInvitePanel()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentTap = -1;
        CommanAnimations.instance.FullScreenPanelAnimation(inviteToPlayPanel.GetComponent<RectTransform>(), false);
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

        Logger.Print(" Enter Id " + enterId);

        Loading_screen.instance.ShowLoadingScreen(true);
        EventHandler.FindFrienByUniqueId(enterId);
    }

}
