using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfilePanel : MonoBehaviour
{
    public static string TAG = ">>PROFILE PANEL ";
    public static ProfilePanel instance;

    [Header("My Profile")]
    [SerializeField] internal RectTransform myProfilePanel;
    public RawImage myprofilePic, myProfileFrame;
    [SerializeField]
    TextMeshProUGUI myNameText, myGoldText, myGemsText, myCurrentLevel, myLevelPercentText, myLevelProgressTxt
        , myPlayerIdText, myPlayerMemSinceText, myPlayertotalMatchText, myPlayertotalWonText, myPlayertotalLossText,
        myPlayerLast5MatchText;
    [SerializeField] Slider mylevelSliderFill;
    [SerializeField] ScrollRect mPScrollView;

    [SerializeField] List<Image> tapBg, smalltapBg;
    [SerializeField] Sprite clicked, nonClicked, coin, gem;
    [SerializeField] GameObject basePrefab, scrollViewContent;

    [Header("Edit Name PopUp")]
    [SerializeField] GameObject editNamePanel;
    [SerializeField] Image editNameBg;
    [SerializeField] Transform editNamePopUp;
    [SerializeField] TMP_InputField editNameInputField;

    List<DeckListClass> avatarList, deckList, frameList;

    [Header("Opponent Profile Panel")]
    [SerializeField] internal GameObject opPanel, actionButtonsParent;
    [SerializeField] Image opPanelBg;
    [SerializeField] Transform opPopUp;
    [SerializeField] RawImage op_ProfilePicture, op_ProfileFrame;
    [SerializeField]
    TextMeshProUGUI op_PlayerName, op_Gold, op_Gems, op_LevelPercentText, op_LevelProgressTxt, op_CurrentLevelText, op_MemSinceTextN, op_MemSinceTextP,
        op_Last5GameText, op_ID;
    [SerializeField] Slider op_LevelSliderFill;
    [SerializeField] Button addRemoveFrdBtn, messageUserBtn, blockUserBtn, reportUserBtn, addFriendBtnP;
    [SerializeField] Image addRemoveFrdImg, messageFrdImg, blockUserImg, addFriendImgP;
    [SerializeField] Sprite addFrd, addFrdP, removeFrdP, sendFrdRequest, sendFrdRequestP, removeFrd, blockUser, unBlockUser, messageFrd, unMessageFrd;

    [SerializeField] Texture2D defaultUser, defaultFrame;

    string oppID;

    int currentTab = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_MP += HandleMyprofile;
        SocketManagergame.OnListner_OUP += HandleOpponentProfile;
        SocketManagergame.OnListner_DL += HandleDeckList;
        SocketManagergame.OnListner_FL += HandleFrameList;
        SocketManagergame.OnListner_PurchaseDeck += HandlePurchasedDeck;
        SocketManagergame.OnListner_PurchaseFrame += HandlePurchasedFrame;
        SocketManagergame.OnListner_PurchaseAvatar += HandlePurchasedAvatar;
        SocketManagergame.OnListner_UUP += HandleUUP;
        SocketManagergame.OnListner_BU += HandleBlockUser;
        SocketManagergame.OnListner_RF += HandleRemoveFriend;
        SocketManagergame.OnListner_SFR += HandleSendFriendRequest;
        SocketManagergame.OnListner_UG += OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE += OnRecevied_USERGEMS;
        AppData.AllPanelClose += HandleCloseAllPanels;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_MP -= HandleMyprofile;
        SocketManagergame.OnListner_OUP -= HandleOpponentProfile;
        SocketManagergame.OnListner_DL -= HandleDeckList;
        SocketManagergame.OnListner_FL -= HandleFrameList;
        SocketManagergame.OnListner_PurchaseDeck -= HandlePurchasedDeck;
        SocketManagergame.OnListner_PurchaseFrame -= HandlePurchasedFrame;
        SocketManagergame.OnListner_PurchaseAvatar -= HandlePurchasedAvatar;
        SocketManagergame.OnListner_UUP -= HandleUUP;
        SocketManagergame.OnListner_BU -= HandleBlockUser;
        SocketManagergame.OnListner_RF -= HandleRemoveFriend;
        SocketManagergame.OnListner_SFR -= HandleSendFriendRequest;
        SocketManagergame.OnListner_UG -= OnRecevied_USERGOLD;
        SocketManagergame.OnListner_UGE -= OnRecevied_USERGEMS;
        AppData.AllPanelClose -= HandleCloseAllPanels;
    }

    JSONNode mPData = new JSONObject();
    //For MP Data Set...
    private void HandleMyprofile(JSONNode data)
    {
        AppData.canShowChallenge = false;
        mPData = data;
        myprofilePic.texture = defaultUser;
        //myProfileFrame.texture = defaultFrame;
        StartCoroutine(AppData.ProfilePicSet(data["pp"], myprofilePic));
        //StartCoroutine(AppData.ProfilePicSet(data["frameImg"], myProfileFrame));
        myNameText.text = data["pn"];
        myGoldText.text = AppData.numDifferentiation(data["gold"].AsLong);
        myGemsText.text = AppData.numDifferentiation(data["gems"].AsLong);
        mylevelSliderFill.value = data["level"]["per"].AsInt;
        myLevelPercentText.text = data["level"]["per"] + "%";
        myLevelProgressTxt.text = data["level"]["cp"] + "/" + data["level"]["nlvp"];
        myCurrentLevel.text = "CURRENT LVL : " + data["level"]["clvl"];
        myPlayerIdText.text = "PLAYER ID : <color=yellow>" + data["unique_id"] + "</color>";
        myPlayerMemSinceText.text = "MEMBER SINCE : <color=yellow>" + data["cd"] + "</color>";

        ProfilePanelClick(11, false);
        Logger.Print(data["avatarList"][0]["isAvatar"]);
        avatarList = JsonConvert.DeserializeObject<List<DeckListClass>>(data["avatarList"].ToString());
        ProfilePanelClick(0, false);
    }

    //For MP ScrollView Set...
    private void SetScrollViewData(List<DeckListClass> data, int tab)
    {
        if (scrollViewContent.transform.childCount > data.Count)
        {
            for (int i = scrollViewContent.transform.childCount - 1; i >= data.Count; i--)
            {
                Destroy(scrollViewContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currentBoard;
            int tempIndex = i;
            if (scrollViewContent.transform.childCount < i + 1)
            {
                currentBoard = Instantiate(basePrefab, scrollViewContent.transform);
            }
            else
            {
                currentBoard = scrollViewContent.transform.GetChild(i).gameObject;
            }

            StartCoroutine(AppData.ProfilePicSet(data[i].img, currentBoard.transform.GetChild(0).GetComponent<RawImage>()));
            currentBoard.transform.GetChild(1).gameObject.SetActive(data[i].isselect == 0);//UseNow            
            currentBoard.transform.GetChild(1).GetChild(0).gameObject.SetActive(data[i].price > 0);
            currentBoard.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = data[i].name == "gems" ? gem : coin;
            currentBoard.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data[i].price > 0 ? AppData.numDifferentiation(data[i].price) : "USE NOW";

            currentBoard.transform.GetChild(2).gameObject.SetActive(data[i].isselect != 0);//Using

            currentBoard.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            currentBoard.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                Loading_screen.instance.ShowLoadingScreen(true);
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

                if (data[tempIndex].price > 0)
                {
                    if (AllCommonGameDialog.instance.isHaveGoldGems(data[tempIndex].name.Equals("gems") ? 0 : data[tempIndex].price / 2, data[tempIndex].name.Equals("gems") ? data[tempIndex].price : 0))
                    {
                        if (tab == 0) EventHandler.AvatarPurchase(data[tempIndex]._id);
                        else if (tab == 1) EventHandler.DeckPurchase(data[tempIndex]._id);
                        else if (tab == 2) EventHandler.FramePurchase(data[tempIndex]._id);
                    }
                }
                else if (data[tempIndex].isselect != 1)
                {
                    JSONObject obj = new JSONObject();
                    if (tab == 0)
                    {
                        obj = new JSONObject
                        {
                            ["pp"] = data[tempIndex].img
                        };
                    }
                    else if (tab == 1)
                    {
                        obj = new JSONObject
                        {
                            ["deckImg"] = data[tempIndex].img
                        };
                    }
                    else if (tab == 2)
                    {
                        obj = new JSONObject
                        {
                            ["frameImg"] = data[tempIndex].img
                        };
                    }
                    currentTab = tab;
                    EventHandler.UpdateUserProfile(obj);
                }
            });
        }
        mPScrollView.horizontalNormalizedPosition = 0;
        if (editNamePanel.activeInHierarchy) CommanAnimations.instance.PopUpAnimation(editNamePanel, editNameBg, editNamePopUp, Vector3.zero, false, false);
        if (!myProfilePanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(myProfilePanel, true);
        if (SceneManager.GetActiveScene().name.Equals("GAMEPLAY")) GameManager.instance.BacktoLobby = false;
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private void OnRecevied_USERGOLD(JSONNode data)
    {
        PrefrenceManager.GOLD = data["gold"];
        myGoldText.text = AppData.numDifferentiation(data["gold"].AsLong);
    }

    private void OnRecevied_USERGEMS(JSONNode data)
    {
        PrefrenceManager.GEMS = data["gems"];
        myGemsText.text = AppData.numDifferentiation(data["gems"].AsLong);
    }

    string copyTxt;
    [SerializeField] Button copyBtn;

    public void CopyTextToClipboard()
    {
        // Set the text to the clipboard
        copyBtn.interactable = false;

#if UNITY_ANDROID
        UniClipboard.SetText(copyTxt);
#elif UNITY_EDITOR
        GUIUtility.systemCopyBuffer = copyTxt;
#endif
        Debug.Log("Text copied to clipboard: " + copyTxt);
    }

    //For OUP Data Set...
    private void HandleOpponentProfile(JSONNode data)
    {
        AppData.canShowChallenge = false;
        op_ProfilePicture.texture = defaultUser;
        //op_ProfileFrame.texture = defaultFrame;
        Logger.Print(TAG + "OnListner_OUP Called...");
        StartCoroutine(AppData.ProfilePicSet(data["pp"], op_ProfilePicture));
        //StartCoroutine(AppData.ProfilePicSet(data["frameImg"], op_ProfileFrame));
        op_PlayerName.text = data["pn"];
        op_Gold.text = AppData.numDifferentiation(data["gold"].AsLong);
        op_Gems.text = AppData.numDifferentiation(data["gems"].AsLong);
        op_LevelSliderFill.value = data["level"]["per"].AsInt;
        op_LevelPercentText.text = data["level"]["per"] + "%";
        op_LevelProgressTxt.text = data["level"]["cp"] + "/" + data["level"]["nlvp"];
        op_CurrentLevelText.text = "CURRENT LVL : " + data["level"]["clvl"];
        op_MemSinceTextN.text = "MEMBER SINCE : " + data["cd"];
        op_MemSinceTextP.text = "MEMBER SINCE : " + data["cd"];
        op_ID.text = "PLAYER ID: " + data["unique_id"];
        copyTxt = data["unique_id"];
        copyBtn.interactable = true;
        //For Gameplay Scene...
        op_MemSinceTextN.gameObject.SetActive(!SceneManager.GetActiveScene().name.Equals("GAMEPLAY"));
        op_MemSinceTextP.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals("GAMEPLAY"));
        actionButtonsParent.SetActive(!SceneManager.GetActiveScene().name.Equals("GAMEPLAY") && !PrefrenceManager._ID.Equals(data["_id"]));
        addFriendImgP.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals("GAMEPLAY") && !PrefrenceManager._ID.Equals(data["_id"]));

        if (!GameManager.instance.playingScreen.activeInHierarchy)
            messageUserBtn.gameObject.SetActive(true);
        else
            messageUserBtn.gameObject.SetActive(false);

        List<string> winLoss = JsonConvert.DeserializeObject<List<string>>(data["track"]["lastWL"].ToString());
        System.Text.StringBuilder lastWL = new System.Text.StringBuilder();
        foreach (string str in winLoss)
        {
            if (str.Contains("L"))
            {
                lastWL.Append("<color=red>" + str + " </color>");
            }
            else if (str.Contains("W"))
            {
                lastWL.Append("<color=green>" + str + " </color>");
            }
        }
        op_Last5GameText.text = lastWL.ToString();

        oppID = data["_id"];
        addRemoveFrdImg.sprite = data["MyFreind"].AsBool ? removeFrd : data["IBlockOppUser"] || data["SendFreindRequest"] ? sendFrdRequest : addFrd;
        addFriendImgP.sprite = data["MyFreind"].AsBool ? removeFrdP : data["IBlockOppUser"] || data["SendFreindRequest"] ? sendFrdRequestP : addFrdP;
        messageFrdImg.sprite = data["IBlockOppUser"].AsBool ? unMessageFrd : messageFrd;
        blockUserImg.sprite = data["IBlockOppUser"].AsBool ? unBlockUser : blockUser;

        OnClick_AddRemoveFriend(data["_id"], data["MyFreind"].AsBool, data["flags"]["_iscom"], data["IBlockOppUser"].AsBool, data["SendFreindRequest"].AsBool);
        OnClick_BlockUser(data["_id"], data["IBlockOppUser"].AsBool);
        if (!SceneManager.GetActiveScene().name.Equals("GAMEPLAY")) OnClick_MessageFriend(data["IBlockOppUser"].AsBool);

        reportUserBtn.onClick.RemoveAllListeners();

        Loading_screen.instance.ShowLoadingScreen(false);
        CommanAnimations.instance.PopUpAnimation(opPanel, opPanelBg, opPopUp, Vector3.one, true);
        if (SceneManager.GetActiveScene().name.Equals("GAMEPLAY")) GameManager.instance.BacktoLobby = false;
    }

    public void OnClick_AddRemoveFriend(string oppId, bool isMyFriend, int isCom, bool isBlocked, bool isFrdReqSended)
    {
        addRemoveFrdBtn.onClick.RemoveAllListeners();
        addFriendBtnP.onClick.RemoveAllListeners();
        if (!isBlocked && !isFrdReqSended)
        {
            addRemoveFrdBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                if (isMyFriend) EventHandler.RemoveFriend(oppId);
                else EventHandler.FrienRequestSend(PrefrenceManager._ID, oppId, isCom);
            });

            addFriendBtnP.onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                if (isMyFriend) EventHandler.RemoveFriend(oppId);
                else EventHandler.FrienRequestSend(PrefrenceManager._ID, oppId, isCom);
            });
        }
    }

    public void OnClick_MessageFriend(bool isBlocked)
    {
        messageUserBtn.onClick.RemoveAllListeners();
        if (!isBlocked)
        {
            messageUserBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                MessageScreen.instance.chatID = oppID;
                EventHandler.OldchatHistory(oppID);
            });
        }

    }

    private void HandleRemoveFriend(JSONNode data)
    {
        addRemoveFrdImg.sprite = data["flag"].AsBool ? addFrd : removeFrd;
        addFriendImgP.sprite = data["flag"].AsBool ? addFrdP : removeFrdP;
        OnClick_AddRemoveFriend(oppID, false, 0, false, false);
    }

    private void HandleSendFriendRequest(JSONNode data)
    {
        addRemoveFrdImg.sprite = data["errcode"].AsInt == 0000 ? sendFrdRequest : addFrd;
        addRemoveFrdBtn.onClick.RemoveAllListeners();

        addFriendImgP.sprite = data["errcode"].AsInt == 0000 ? sendFrdRequestP : addFrdP;
        addFriendBtnP.onClick.RemoveAllListeners();
    }

    public void OnClick_BlockUser(string oppId, bool isUserBlocked)
    {
        blockUserBtn.onClick.RemoveAllListeners();
        blockUserBtn.onClick.AddListener(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            if (!isUserBlocked) EventHandler.BlockUser(oppId);
        });
    }

    private void HandleBlockUser(JSONNode data)
    {
        blockUserImg.sprite = !data["flag"].AsBool ? unBlockUser : blockUser;
        addRemoveFrdImg.sprite = !data["flag"].AsBool ? addFrd : removeFrd;
        messageFrdImg.sprite = !data["flag"].AsBool ? unMessageFrd : messageFrd;
        messageUserBtn.onClick.RemoveAllListeners();
        addRemoveFrdBtn.onClick.RemoveAllListeners();
        blockUserBtn.onClick.RemoveAllListeners();
        CommanAnimations.instance.PopUpAnimation(opPanel, opPanelBg, opPopUp, Vector3.zero, false);
    }

    private void HandlePurchasedAvatar(JSONNode data)
    {
        Logger.Print(data.ToString());
        DeckListClass newAvatar = JsonConvert.DeserializeObject<DeckListClass>(data.ToString());
        SetButtonToUseNow(newAvatar, "pp", avatarList, 0);
    }

    private void HandlePurchasedDeck(JSONNode data)
    {
        Logger.Print(data.ToString());
        DeckListClass newDeck = JsonConvert.DeserializeObject<DeckListClass>(data.ToString());
        SetButtonToUseNow(newDeck, "deckImg", deckList, 1);
    }

    private void HandlePurchasedFrame(JSONNode data)
    {
        Logger.Print(data.ToString());
        DeckListClass newFrame = JsonConvert.DeserializeObject<DeckListClass>(data.ToString());
        SetButtonToUseNow(newFrame, "frameImg", frameList, 2);
    }

    private void SetButtonToUseNow(DeckListClass newObj, string key, List<DeckListClass> list, int tab)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].img == newObj.img)
            {
                list[i].price = 0;
                scrollViewContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
                scrollViewContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "USE NOW";
                scrollViewContent.transform.GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                scrollViewContent.transform.GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    Loading_screen.instance.ShowLoadingScreen(true);
                    JSONObject obj = new JSONObject
                    {
                        [key] = newObj.img
                    };
                    currentTab = tab;
                    EventHandler.UpdateUserProfile(obj);
                });
            }
        }
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private void HandleDeckList(JSONNode data)
    {
        deckList = JsonConvert.DeserializeObject<List<DeckListClass>>(data.ToString());
        SetScrollViewData(deckList, 1);
    }

    private void HandleFrameList(JSONNode data)
    {
        frameList = JsonConvert.DeserializeObject<List<DeckListClass>>(data.ToString());
        SetScrollViewData(frameList, 2);
    }

    private void SetPlayerStatisticData(int tap)
    {
        myPlayertotalMatchText.text = AppData.numDifferentiation(mPData[tap == 0 ? "solo" : tap == 1 ? "teamup" : "tour"]["gp"].AsLong);
        myPlayertotalWonText.text = AppData.numDifferentiation(mPData[tap == 0 ? "solo" : tap == 1 ? "teamup" : "tour"]["gw"].AsLong);
        myPlayertotalLossText.text = AppData.numDifferentiation(mPData[tap == 0 ? "solo" : tap == 1 ? "teamup" : "tour"]["gp"].AsLong - mPData[tap == 0 ? "solo" : tap == 1 ? "teamup" : "tour"]["gw"].AsLong);

        if (mPData[tap == 2 ? "lastTWL" : "lastWL"].Count > 0)
        {
            List<string> winLoss = JsonConvert.DeserializeObject<List<string>>(mPData[tap == 2 ? "lastTWL" : "lastWL"].ToString());
            System.Text.StringBuilder lastWL = new System.Text.StringBuilder();
            foreach (string str in winLoss)
            {
                if (str.Contains("L"))
                {
                    lastWL.Append("<color=red>" + str + " </color>");
                }
                else if (str.Contains("W"))
                {
                    lastWL.Append("<color=green>" + str + " </color>");
                }
            }
            myPlayerLast5MatchText.text = lastWL.ToString();
        }
        else myPlayerLast5MatchText.text = string.Empty;
    }

    private void HandleUUP(JSONNode data)
    {
        Logger.Print(TAG + "On Receieved UUP Called");
        PrefrenceManager.PP = data["pp"];
        PrefrenceManager.PN = data["pn"];
        PrefrenceManager.DeckImage = data["deckImg"];
        PrefrenceManager.FrameImage = data["frameImg"];
        StartCoroutine(AppData.ProfilePicSet(data["pp"], DashboardManager.instance.PlayerImg));
        StartCoroutine(AppData.ProfilePicSet(data["pp"], myprofilePic));
        DashboardManager.instance.PlayerName.text = data["pn"];
        myNameText.text = data["pn"];
        //StartCoroutine(AppData.ProfilePicSet(data["frameImg"], myProfileFrame));
        //StartCoroutine(AppData.ProfilePicSet(data["frameImg"], DashboardManager.instance.playerFrame));
        UpdateScrollview(data);
    }

    private void UpdateScrollview(JSONNode data)
    {
        if (currentTab >= 0)
        {
            for (int i = 0; i < (currentTab == 0 ? avatarList.Count : currentTab == 1 ? deckList.Count : frameList.Count); i++)
            {
                if (currentTab == 0 ? avatarList[i].img.Equals(data["pp"]) : currentTab == 1 ? deckList[i].img.Equals(data["deckImg"]["deck"]) : frameList[i].img.Equals(data["frameImg"]))
                {
                    if (currentTab == 0) avatarList[i].isselect = 1;
                    if (currentTab == 1) deckList[i].isselect = 1;
                    if (currentTab == 2) frameList[i].isselect = 1;

                    scrollViewContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    scrollViewContent.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    if (currentTab == 0) avatarList[i].isselect = 0;
                    if (currentTab == 1) deckList[i].isselect = 0;
                    if (currentTab == 2) frameList[i].isselect = 0;

                    scrollViewContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    scrollViewContent.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
                }
            }
        }
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    int currentTap = -1;

    public void OnClick_ProfilePanel(int click)
    {
        ProfilePanelClick(click);
    }

    public void ProfilePanelClick(int i, bool SoundPlay = true)
    {
        Logger.Print(TAG + "Current Click >> " + i);
        if (currentTap == i) return;
        if (SoundPlay && i != 7 && i != 8 && i != 10) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        currentTap = i;
        switch (i)
        {
            case 0://avatar
                SetTap(0);
                SetScrollViewData(avatarList, 0);
                break;
            case 1://deck
                SetTap(1);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.DeckListEvent();
                break;
            case 2://frame           
                SetTap(2);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.FrameListEvent();
                break;
            case 7://edit name
                editNameInputField.text = string.Empty;
                CommanAnimations.instance.PopUpAnimation(editNamePanel, editNameBg, editNamePopUp, Vector3.one, true, SoundPlay);
                break;
            case 8://edit name Close
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                currentTap = -1;
                CommanAnimations.instance.PopUpAnimation(editNamePanel, editNameBg, editNamePopUp, Vector3.zero, false, false);
                break;
            case 9://close MyProfile
                currentTap = -1;
                AppData.canShowChallenge = true;
                CommanAnimations.instance.FullScreenPanelAnimation(myProfilePanel, false, SoundPlay);
                GameManager.instance.BacktoLobby = true;
                break;
            case 10://Opponent Panel Close
                currentTap = -1;
                //AppData.canShowChallenge = true;
                CommanAnimations.instance.PopUpAnimation(opPanel, opPanelBg, opPopUp, Vector3.zero, false, false);
                GameManager.instance.BacktoLobby = true;
                break;
            case 11: //PS Classic
                SetSmallTap(0);
                break;
            case 12: //PS TEAM UP
                SetSmallTap(1);
                break;
            case 14: //PS TOURNAMENT
                SetSmallTap(2);
                break;
        }
    }

    public void OnClick_SaveName(TMP_InputField name)
    {
        //AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentTap = -1;
        if (name.text.Equals(""))
        {
            AllCommonGameDialog.instance.SetJustOkDialogData("INVALID NAME", "PLEASE ENTER VALID NAME");
            //CommanAnimations.instance.PopUpAnimation(editNamePanel, editNameBg, editNamePopUp, Vector3.zero, false);
            return;
        }
        Loading_screen.instance.ShowLoadingScreen(true);
        JSONObject obj = new JSONObject
        {
            ["pn"] = name.text
        };
        EventHandler.UpdateUserProfile(obj);
        ProfilePanelClick(8);
        //CommanAnimations.instance.PopUpAnimation(editNamePanel, editNameBg, editNamePopUp, Vector3.zero, false);
    }

    private void SetTap(int i)
    {
        for (int j = 0; j < tapBg.Count; j++)
        {
            //Logger.Print(TAG + "I is " + i + "J is " + j + "Condition >> " + (j == i));
            tapBg[j].sprite = (j == i) ? clicked : nonClicked;
        }
    }

    private void SetSmallTap(int i)
    {
        for (int j = 0; j < smalltapBg.Count; j++)
        {
            //Logger.Print(TAG + "I is " + i + "J is " + j + "Condition >> " + (j == i));
            smalltapBg[j].sprite = (j == i) ? clicked : nonClicked;
        }
        SetPlayerStatisticData(i);
    }

    private void HandleCloseAllPanels()
    {
        myProfilePanel.gameObject.SetActive(false);
        editNamePanel.SetActive(false);
        opPanel.SetActive(false);
    }

}
