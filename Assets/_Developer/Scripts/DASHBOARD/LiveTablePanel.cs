using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveTablePanel : MonoBehaviour
{
    private string TAG = " >>> LIVETABLE >> ";
    public static LiveTablePanel Instance;

    [Header("Live Table")]
    public RectTransform liveTablePanel;
    [SerializeField] GameObject noTable;
    [SerializeField] LiveTableInfo liveTableScriptPrefab;
    [SerializeField] Image tournamentTabImg, liveTableTabImg;
    public GameObject liveTableTab;
    [SerializeField] Sprite panelTabOn, panelTabOff;
    [SerializeField] Transform liveTableContent;
    [SerializeField] Sprite classicType, emojiType, teamUpType, invertType;
    [SerializeField] TextMeshProUGUI noteText;
    [SerializeField] List<LiveTableInfo> liveTableObjectList = new List<LiveTableInfo>();
    [SerializeField] List<LiveTableItemModel> liveTablesList = new List<LiveTableItemModel>();

    [Header("Tournament")]
    [SerializeField] GameObject tournamentTab;
    [SerializeField] GameObject tournamentPrefab;
    [SerializeField] TournamentSlotInfo t_SlotePrefabObject;
    [SerializeField] Transform tournamentContent;
    List<LOTSData> tournamentsTablesList = new List<LOTSData>();

    [SerializeField] Texture defaultProfile;
    [SerializeField] Sprite defaultRing;

    public int currentTap = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_LOPTNEW += HandleLiveTable;
        SocketManagergame.OnListner_LOPT += HandleLiveTable;
        SocketManagergame.OnListner_LOTS += HandleTournament;
        SocketManagergame.OnListner_RGR += HandleRemoveGlobalRoom;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_LOPTNEW -= HandleLiveTable;
        SocketManagergame.OnListner_LOPT -= HandleLiveTable;
        SocketManagergame.OnListner_LOTS -= HandleTournament;
        SocketManagergame.OnListner_RGR -= HandleRemoveGlobalRoom;
    }

    #region LIVETABLE
    private void HandleLiveTable(JSONNode data)
    {
        AppData.canShowChallenge = false;
        //OnClick_LiveTable(3);
        if (data.HasKey("table"))
        {
            Logger.Print(TAG + "All Table Data Set Karva Gayu ");
            SetAllLiveTableData(data);
            SetTap(1);
        }
        else
        {
            LiveTableItemModel model = JsonConvert.DeserializeObject<LiveTableItemModel>(data.ToString());

            int index = GetTableIndex(model);

            Logger.Print(TAG + " Index : " + index + " Child Count " + liveTableContent.transform.childCount + " Table List " + liveTablesList.Count);
            Logger.NormalLog(index + " ListCount = " + liveTableObjectList.Count + " Table List " + liveTablesList.Count);

            if (index != -1)
            {
                if (model.ap == model.pi.Count || model.ap == 0)
                {
                    StopAllCoroutines();
                    Logger.Print(TAG + " Delete Table karva gyu");

                    DestroyImmediate(liveTableObjectList[index].gameObject);
                    liveTablesList.RemoveAt(index);
                    liveTableObjectList.RemoveAt(index);

                    noTable.SetActive(liveTablesList.Count == 0);
                    noteText.text = "There are no any Table right now.";
                    return;
                }
                else
                {
                    Logger.Print(TAG + "Single Table Data Set Karva Gayu");
                    SetSingleLiveTableData(model, liveTableObjectList[index]);
                }
            }
            else
            {
                if (model.ap == 4 || model.ap == 0)
                {
                    Logger.Print(TAG + "Table Empty OR Full Hatu");
                    StopAllCoroutines();
                    return;
                }
                liveTablesList.Add(model);
                Logger.Print(TAG + "New Table Create Karine Data Set Karyo");
                LiveTableInfo lObj = Instantiate(liveTableScriptPrefab, liveTableContent);
                liveTableObjectList.Add(lObj);
                SetSingleLiveTableData(model, lObj);
            }
        }

        noteText.text = "There are no any Table right now.";
        noTable.SetActive(liveTablesList.Count == 0);
    }

    private void SetAllLiveTableData(JSONNode data1)
    {
        liveTablesList.Clear();
        List<LiveTableItemModel> liveTablesListdemo = JsonConvert.DeserializeObject<List<LiveTableItemModel>>(data1["table"].ToString());

        for (int i = 0; i < liveTablesListdemo.Count; i++)
        {
            if (liveTablesListdemo[i].ap == 4) continue;
            liveTablesList.Add(liveTablesListdemo[i]);
        }

        if (liveTableObjectList.Count > liveTablesList.Count)
        {
            for (int i = liveTableObjectList.Count - 1; i >= liveTablesList.Count; i--)
            {
                Destroy(liveTableObjectList[i].gameObject);
                liveTableObjectList.RemoveAt(i);
            }
        }

        for (int i = 0; i < liveTablesList.Count; i++)
        {
            LiveTableInfo currentTable;
            int tempIndex = i;
            if (liveTableContent.childCount < i + 1)
            {
                currentTable = Instantiate(liveTableScriptPrefab, liveTableContent);
                liveTableObjectList.Add(currentTable);
            }
            else
            {
                currentTable = liveTableContent.GetChild(i).GetComponent<LiveTableInfo>();
            }

            SetSingleLiveTableData(liveTablesList[tempIndex], currentTable);
        }

        Loading_screen.instance.ShowLoadingScreen(false);
        if (!liveTablePanel.gameObject.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(liveTablePanel, true);
    }



    private void SetSingleLiveTableData(LiveTableItemModel table, LiveTableInfo currentTableObject)
    {
        //In Case Need to Change In Emoji partner Sprite...
        currentTableObject.typeTableImg.sprite = (table.mode == AppData.CLASSIC) ? classicType : (table.mode == AppData.EMOJISOLO) ? emojiType : teamUpType;
        currentTableObject.typeTableImg.sprite = (table.mode == AppData.FLIP) ? invertType : currentTableObject.typeTableImg.sprite;

        for (int i = 0; i < table.pi.Count; i++)
        {
            int tempIndex = i;
            if (table.pi[i].ui != null)
            {
                StartCoroutine(AppData.ProfilePicSet(table.pi[i].ui.pp, currentTableObject.playerInfo[i].userProfileImg));
                //StartCoroutine(AppData.SpriteSetFromURL(table.pi[i].ui.frameImg, currentTableObject.playerInfo[i].userRingImg, "SetSingleLiveTableData"));
            }
            currentTableObject.playerInfo[i].userMask.gameObject.SetActive(table.pi[i].ui != null);
            //currentTableObject.playerInfo[i].userRingImg.gameObject.SetActive(table.pi[i].ui != null);
            //For Sit Button
            currentTableObject.playerInfo[i].sitClick.gameObject.SetActive(table.pi[i].ui == null);
            currentTableObject.playerInfo[i].sitClick.onClick.RemoveAllListeners();
            currentTableObject.playerInfo[i].sitClick.onClick.AddListener(() =>
            {
                if (AllCommonGameDialog.instance.isHaveGoldGems(table.bv, table.gems))
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.JoinTableOfGlobalRoom(table._id, tempIndex, "LIVETABLE");
                }
            });
        }

        currentTableObject.coinTxt.text = AppData.numDifferentiation(table.bv);
        currentTableObject.gemsValueTxt.text = AppData.numDifferentiation(table.gems);
        currentTableObject.coinImg.SetActive(table.bv > 0);
        currentTableObject.coinTxt.gameObject.SetActive(table.bv > 0);
        currentTableObject.plusImg.gameObject.SetActive(table.gems > 0);
        currentTableObject.gemsImg.gameObject.SetActive(table.gems > 0);
        currentTableObject.gemsValueTxt.gameObject.SetActive(table.gems > 0);

        for (int i = 0; i < table.rules.Count; i++)
        {
            int index = table.rules[i] - 1;
            Logger.Print($" r = {table.rules[i]} || { PrivateTable.instance.rulesSprite.Count} | {index}");
            currentTableObject.rules[i].gameObject.SetActive(true);
            currentTableObject.rules[i].sprite = PrivateTable.instance.rulesSprite[index];
        }
    }

    private int GetTableIndex(LiveTableItemModel table)
    {
        for (int i = 0; i < liveTablesList.Count; i++)
        {
            if (liveTablesList[i]._id.Equals(table._id))
            {
                return i;
            }
        }
        return -1;
    }
    #endregion

    private void HandleRemoveGlobalRoom(JSONNode data)
    {
        Logger.Print(TAG + "Loader Removed From Live Table Remove Globle Room");
        AppData.canShowChallenge = true;
        Loading_screen.instance.ShowLoadingScreen(false);
        CommanAnimations.instance.FullScreenPanelAnimation(liveTablePanel, false);
        currentTap = -1;
    }

    private void HandleTournament(JSONNode data)
    {
        AppData.canShowChallenge = false;
        if (data.HasKey("table"))
        {
            tournamentsTablesList.Clear();
            tournamentsTablesList = JsonConvert.DeserializeObject<List<LOTSData>>(data["table"].ToString());
            SetAllTournamentTableData(tournamentsTablesList);
        }
        else
        {
            LOTSData model = JsonConvert.DeserializeObject<LOTSData>(data.ToString());

            int index = GetTournamentIndex(model);

            Logger.Print(TAG + " Index : " + index + " Child Count " + tournamentContent.childCount + " Table List " + tournamentsTablesList.Count);

            if (model.count == 16)
            {
                if (index == -1)
                {
                    StopAllCoroutines();
                    return;
                }
                Destroy(tournamentContent.GetChild(index).gameObject);
                tournamentsTablesList.RemoveAt(index);
                noTable.SetActive(tournamentsTablesList.Count == 0);
                noteText.text = "There are no any Tournaments right now.";
                return;
            }

            if (index != -1)
            {
                SetSingleTounamentTableData(model, tournamentContent.GetChild(index).gameObject);
            }
            else
            {
                tournamentsTablesList.Add(model);
                GameObject currentTable = Instantiate(tournamentPrefab, tournamentContent);
                SetSingleTounamentTableData(model, currentTable);

                currentTable.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                currentTable.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (AllCommonGameDialog.instance.isHaveGoldGems(model.bv, model.gems))
                    {
                        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                        Loading_screen.instance.ShowLoadingScreen(true);
                        EventHandler.PlayTournament(model.bv, model.mode, model.winbv, model._id, model.gems);
                        AppData.canShowChallenge = false;
                    }
                });
            }
        }

        noTable.SetActive(tournamentsTablesList.Count == 0);
        noteText.text = "There are no any Tournaments right now.";
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!tournamentTab.activeInHierarchy) LiveTablePanelClick(2, false);
    }

    private void SetAllTournamentTableData(List<LOTSData> data)
    {
        if (tournamentContent.childCount > data.Count)
        {
            for (int i = tournamentContent.childCount - 1; i >= data.Count; i--)
            {
                Destroy(tournamentContent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentTable;
            int tempIndex = i;
            if (tournamentContent.childCount < i + 1)
            {
                currentTable = Instantiate(tournamentPrefab, tournamentContent);
            }
            else
            {
                currentTable = tournamentContent.GetChild(i).gameObject;
            }

            SetSingleTounamentTableData(data[tempIndex], currentTable);

            currentTable.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            currentTable.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                if (AllCommonGameDialog.instance.isHaveGoldGems(data[tempIndex].bv, data[tempIndex].gems))
                {
                    Loading_screen.instance.ShowLoadingScreen(true);
                    EventHandler.PlayTournament(data[tempIndex].bv, data[tempIndex].mode, data[tempIndex].winbv, data[tempIndex]._id, data[tempIndex].gems);
                }
            });
        }

        Loading_screen.instance.ShowLoadingScreen(false);
        if (!liveTablePanel.gameObject.activeInHierarchy)
        {
            LiveTablePanelClick(0, false);
        }
    }

    private void SetSingleTounamentTableData(LOTSData table, GameObject currentTableObject)
    {
        currentTableObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "BOOT VALUE : " + AppData.numDifferentiation(table.bv);
        currentTableObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "WIN AMOUNT : " + AppData.numDifferentiation(table.winbv);
        currentTableObject.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "MODE : " + table.mode;

        for (int i = 0; i < table.round1.Count; i++)
        {
            for (int cPlayer = 0; cPlayer < table.round1[i].player.Count; cPlayer++)
            {
                if (table.round1[i].player[cPlayer].uid == null)
                {
                    currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = defaultProfile;
                    currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(2).gameObject.SetActive(false);
                    currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(1).GetComponent<Image>().sprite = defaultRing;
                    currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Player";
                    continue;
                }
                PlayerData playerObj = new PlayerData();
                playerObj.playerImg = currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>();
                playerObj.youTag = currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(2).gameObject;
                playerObj.frameImg = currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(0).GetChild(1).GetComponent<Image>();
                playerObj.playerName = currentTableObject.transform.GetChild(1).GetChild(i).GetChild(cPlayer).GetChild(1).GetComponent<TextMeshProUGUI>();
                Logger.Print(TAG + "Player Image Object >>" + table.round1[i].player[cPlayer].pp);
                SetSingleTournamentPlayerData(table.round1[i].player[cPlayer], playerObj);
            }
        }
    }

    private void SetSingleTournamentPlayerData(Player playerDetails, PlayerData playerObj)
    {
        Logger.Print(TAG + "PlayerDetails >> " + playerDetails.ToString() + " >>> PlayerObject >> " + playerObj.ToString());
        Logger.Print(TAG + "Sprite Set Sennd >> " + playerDetails.frameImg);
        StartCoroutine(AppData.ProfilePicSet(playerDetails.pp, playerObj.playerImg));
        StartCoroutine(AppData.SpriteSetFromURL(playerDetails.frameImg, playerObj.frameImg, "SetSingleLiveTableData"));
        playerObj.playerName.text = playerDetails.pn;
        playerObj.youTag.SetActive(PrefrenceManager._ID.Equals(playerDetails.uid));
    }

    private int GetTournamentIndex(LOTSData table)
    {
        for (int i = 0; i < tournamentsTablesList.Count; i++)
        {
            if (tournamentsTablesList[i]._id == table._id)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetTap(int tap)
    {
        tournamentTab.SetActive(tap == 0);
        liveTableTab.SetActive(tap == 1);
        tournamentTabImg.sprite = tap == 0 ? panelTabOn : panelTabOff;
        liveTableTabImg.sprite = tap == 1 ? panelTabOn : panelTabOff;
        if (tap == 0)// t table
        {
            tournamentTabImg.raycastTarget = true;
            liveTableTabImg.raycastTarget = false;
            Logger.Print($" {TAG} Settup {tap} || Live false");

            //tournamentTabImg.GetComponent<Button>().interactable = false;
            //yield return new WaitForSeconds(1);
            //liveTableTabImg.GetComponent<Button>().interactable = true;
        }
        else if (tap == 1)
        {
            tournamentTabImg.raycastTarget = false;
            liveTableTabImg.raycastTarget = true;
            Logger.Print($" {TAG} Settup {tap} || Tournament false");
            //liveTableTabImg.GetComponent<Button>().interactable = false;
            //yield return new WaitForSeconds(1);
            //tournamentTabImg.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick_LiveTable(int click)
    {
        LiveTablePanelClick(click);
    }

    public void LiveTablePanelClick(int i, bool soundPlay = true)
    {
        if (currentTap == i) return;
        currentTap = i;
        switch (i)
        {
            case 0:
                SetTap(1);
                AppData.canShowChallenge = false;
                CommanAnimations.instance.FullScreenPanelAnimation(liveTablePanel, true);
                break;
            case 1: //Close Live Table
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                AppData.canShowChallenge = true;
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.RemoveGlobalRoom();
                break;
            case 2://Tournament Tab
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.ListOfTournamentSlot();
                SetTap(0);
                break;
            case 3://Live Table Tab
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.ListOfPlayingTable();
                SetTap(1);
                break;
            case 4:
                currentTap = -1;
                CreateTable.instance.CreateTablePanelClick(6);
                break;
        }
    }
}
