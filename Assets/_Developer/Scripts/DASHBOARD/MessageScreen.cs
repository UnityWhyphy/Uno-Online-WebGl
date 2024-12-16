using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageScreen : MonoBehaviour
{
    public static MessageScreen instance;
    private string TAG = " >>> MessageScreen >>> ";

    public RectTransform messagePanel;
    [SerializeField] GameObject myMsgPrefab, oppMsgPrefab;
    [SerializeField] Transform msgContent;
    [SerializeField] TMP_InputField msgInput;

    public string chatID = "";

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListner_GMAM += HandleGetedAllMsgs;
        SocketManagergame.OnListner_PC += HandleSendedChat;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_GMAM -= HandleGetedAllMsgs;
        SocketManagergame.OnListner_PC -= HandleSendedChat;
    }

    private void HandleGetedAllMsgs(JSONNode Data)
    {
        for (int i = 0; i < msgContent.childCount; i++)
        {
            Destroy(msgContent.GetChild(i).gameObject);
        }

        if (Data.Equals("")) return;

        for (int j = 0; j < Data["uid"].Count; j++) //For ChatID{
        {
            if (!Data["uid"][j].Equals(PrefrenceManager._ID)) chatID = Data["uid"][j];
        }

        if (Data["msg"].Count > 0)
        {
            List<GMAM> msgData = JsonConvert.DeserializeObject<List<GMAM>>(Data["msg"].ToString());

            for (int i = 0; i < msgData.Count; i++)
            {
                GameObject currentBoard = Instantiate(msgData[i].s.Equals(PrefrenceManager._ID) ? myMsgPrefab : oppMsgPrefab, msgContent);

                StartCoroutine(AppData.ProfilePicSet(msgData[i].spp, currentBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
                StartCoroutine(AppData.SpriteSetFromURL(msgData[i].sframeImg, currentBoard.transform.GetChild(0).GetChild(1).GetComponent<Image>(), "HandleGetedAllMsgs"));

                currentBoard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = msgData[i].spn;
                currentBoard.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = msgData[i].body;
                currentBoard.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = msgData[i].cd.ToString();
            }
        }
        msgInput.text = string.Empty;
        ProfilePanel.instance.ProfilePanelClick(10, false);
        Loading_screen.instance.ShowLoadingScreen(false);
        CommanAnimations.instance.FullScreenPanelAnimation(messagePanel, true);
    }

    private void HandleSendedChat(JSONNode data)
    {
        Logger.Print(TAG + "HandleSendedChat Called");
        PC msgData = JsonConvert.DeserializeObject<PC>(data.ToString());

        msgInput.text = string.Empty;
        GameObject currentBoard = Instantiate(msgData.s.Equals(PrefrenceManager._ID) ? myMsgPrefab : oppMsgPrefab, msgContent);

        StartCoroutine(AppData.ProfilePicSet(msgData.pp, currentBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>()));
        StartCoroutine(AppData.SpriteSetFromURL(msgData.frameImg, currentBoard.transform.GetChild(0).GetChild(1).GetComponent<Image>(), "HandleSendedChat"));

        currentBoard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = msgData.pn;
        currentBoard.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = msgData.body;
        currentBoard.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = msgData.cd.ToString();
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    public void OnClick_MessageScreen(int click)
    {
        switch (click)
        {
            case 0://Send
                if (msgInput.text.Trim().Equals(""))
                {
                    msgInput.text = string.Empty;
                    AllCommonGameDialog.instance.SetJustOkDialogData("EMPTY MESSAGE", "PLEASE ENTER A MESSAGE");
                    return;
                }
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.PersnoalChat(chatID, msgInput.text.Trim());
                break;

            case 1: //Close
                msgInput.text = string.Empty;
                CommanAnimations.instance.FullScreenPanelAnimation(messagePanel, false);
                break;
        }
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
    }

}
