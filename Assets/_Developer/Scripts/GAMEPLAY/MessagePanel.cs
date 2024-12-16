using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
using Newtonsoft.Json;

public class MessagePanel : MonoBehaviour
{
    public static MessagePanel Instance;
    private static string TAG = "MESSAGE PANEL >>> ";
    public GameObject messagePanel;

    [SerializeField] RectTransform messagePanelBG;
    [SerializeField] Image messagePanelImg;
    [SerializeField] GameObject chatPanel, emojiPanel, giftPanel;
    [SerializeField] Sprite chatOn, chatOff, emojiOn, emojiOff, giftOn, giftOff, emojiDisable;
    [SerializeField] Image chatImg, emojiImg, giftImg;
    [SerializeField] GameObject chatPrefab, suggestionPrefab;
    [SerializeField] Transform chatsContent, suggestionContent;
    [SerializeField] GameObject giftCatContent, giftCatPrefab, giftContent, giftPrefab;
    [SerializeField] GameObject gIFContent, gIFPrefab;
    [SerializeField] Sprite catOn, catOff;
    [SerializeField] TMP_InputField typeBox;
    [SerializeField] ScrollRect chatScrollRect, suggestionScrollRect, emojiScroll, categoryScrollRect, giftsScrollRect;

    [SerializeField] Sprite[] emojisprite;

    public int finalSI = -1;
    public JSONNode gGLData;
    List<string> gIFs;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    internal Sprite GetEmojiSprite(int e_num)
    {
        return emojisprite[e_num];
    }

    public void OnClick_OpenMessagePanel(int selectMode = 0)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        OpenMessagePanel(selectMode);
    }

    public void OpenMessagePanel(int selectMode = 0, int gettedSI = -1)
    {
        finalSI = gettedSI;
        Logger.Print(" >> GIF Checking >> " + TAG + " Final Si When Message Panel Open >> " + finalSI);
        messagePanel.SetActive(true);
        ButtonClick(selectMode, false);
        MessagePanelAnimation(true);
    }

    public void OnClick_CloseMessagePanel(bool soundPlay = true)
    {
        currentTap = -1;
        if (soundPlay) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        MessagePanelAnimation(false);
    }

    public void OnClick_Button(int buttonNum)
    {
        ButtonClick(buttonNum);
    }

    int currentTap = -1;
    public void ButtonClick(int buttonNum, bool soundPlay = true)
    {
        if (currentTap == buttonNum) return;

        //if (buttonNum == 1 && (GameManager.instance.mode.Equals(AppData.EMOJISOLO) || GameManager.instance.mode.Equals(AppData.EMOJIPARTNER))) return;
        currentTap = buttonNum;
        if (gGLData == null)
        {
            EventHandler.GetGiftList(GameManager.instance.mode);
        }
        if (soundPlay) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        ResetButtons();
        CloseAllPanels();
        StartCoroutine(DelaySetGGlData(buttonNum));
    }

    private IEnumerator DelaySetGGlData(int buttonNum)
    {
        yield return new WaitUntil(() => (gGLData != null));
        switch (buttonNum)//0=chat,1=emoji,2=gift
        {
            case 0:
                chatImg.sprite = chatOn;
                chatPanel.SetActive(true);
                break;
            case 1:
                emojiImg.sprite = emojiOn;
                ShowGIFData();
                emojiPanel.SetActive(true);
                break;
            case 2:
                giftImg.sprite = giftOn;
                //For Gift Data...
                ShowGiftList(gGLData["png"]["category"][0]);
                giftPanel.SetActive(true);
                break;
        }
    }

    private void MessagePanelAnimation(bool isOpenAnim)
    {
        messagePanelImg.DOFade(isOpenAnim ? 0.4f : 0f, 0.3f);
        messagePanelBG.DOAnchorPos(isOpenAnim ? Vector2.zero : new Vector2(messagePanelBG.rect.width + 10, 0), 0.5f).OnComplete(() =>
        {
            messagePanel.SetActive(isOpenAnim ? true : false);
        });
    }

    public void CloseAllPanels()
    {
        typeBox.text = string.Empty;
        suggestionScrollRect.horizontalNormalizedPosition = 0;
        emojiScroll.verticalNormalizedPosition = 1;
        categoryScrollRect.verticalNormalizedPosition = 1;
        giftsScrollRect.verticalNormalizedPosition = 1;
        chatPanel.SetActive(false);
        emojiPanel.SetActive(false);
        giftPanel.SetActive(false);
    }

    private void ResetButtons()
    {
        chatImg.sprite = chatOff;
        //emojiImg.sprite = (GameManager.instance.mode.Equals(AppData.EMOJISOLO) || GameManager.instance.mode.Equals(AppData.EMOJIPARTNER)) ? emojiDisable : emojiOff;
        emojiImg.sprite = emojiOff;
        giftImg.sprite = giftOff;
    }

    public void OnClick_SendMessage(TMP_InputField text)
    {
        int wordCount = CountWords(text.text);
        Logger.NormalLog("Length : " + wordCount);
        if (text.text.Trim().Equals(""))
        {
            text.text = string.Empty;
            AllCommonGameDialog.instance.SetJustOkDialogData("EMPTY MESSAGE", "PLEASE ENTER A MESSAGE");
            return;
        }
        else if (wordCount > 5)
        {
            text.text = InsertLineBreaks(text.text, 4);
        }
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        EventHandler.ChatOnTable(text.text);
        text.text = string.Empty;
    }

    int CountWords(string text)
    {
        string[] words = text.Split(new char[] { ' ', '\n', '\t', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    public string InsertLineBreaks(string text, int wordsPerLine)
    {
        string[] words = text.Split(' ');
        string result = "";
        int wordCount = 0;

        foreach (string word in words)
        {
            if (wordCount >= wordsPerLine)
            {
                result += "\n";
                wordCount = 0;
            }

            result += word + " ";
            wordCount++;
        }

        return result.Trim();
    }

    public void CreateChat(string playerName, string body)
    {
        GameObject chat = Instantiate(chatPrefab, chatsContent);
        chat.GetComponent<TextMeshProUGUI>().text = "<color=yellow>" + playerName + " : " + "</color>" + "<color=white>" + body;  //Player Name...
        chatScrollRect.DOVerticalNormalizedPos(0, 0.2f);
    }

    public void SetGiftData(JSONNode data)
    {
        gGLData = data;
        List<string> category = JsonConvert.DeserializeObject<List<string>>(gGLData["png"]["category"].ToString());

        if (giftCatContent.transform.childCount > category.Count)
        {
            for (int i = giftCatContent.transform.childCount - 1; i >= category.Count; i--)
            {
                Destroy(giftCatContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < category.Count; i++)
        {
            GameObject catObj;
            int tempIndex = i;
            if (giftCatContent.transform.childCount < i + 1)
            {
                catObj = Instantiate(giftCatPrefab, giftCatContent.transform);
            }
            else
            {
                catObj = giftCatContent.transform.GetChild(i).gameObject;
            }
            catObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = category[i];
            catObj.name = category[i];
            catObj.GetComponent<Button>().onClick.RemoveAllListeners();
            catObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                ShowGiftList(category[tempIndex]);
            });
        }
        ShowGiftList(data["png"]["category"][0]);
    }

    public void ShowGiftList(string catName)
    {
        List<GiftModel> gifts = JsonConvert.DeserializeObject<List<GiftModel>>(gGLData["png"][catName].ToString());
        Reset_ClickedGiftCategory();
        if (giftContent.transform.childCount > gifts.Count)
        {
            for (int i = giftContent.transform.childCount - 1; i >= gifts.Count; i--)
            {
                Destroy(giftContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < gifts.Count; i++)
        {
            GameObject giftObj;
            int tempIndex = i;
            if (giftContent.transform.childCount < i + 1)
            {
                giftObj = Instantiate(giftPrefab, giftContent.transform);
            }
            else
            {
                giftObj = giftContent.transform.GetChild(i).gameObject;
            }

            StartCoroutine(AppData.ProfilePicSet(gifts[i].img, giftObj.transform.GetChild(0).GetComponent<RawImage>()));
            giftObj.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = AppData.numDifferentiation(gifts[i].price);

            giftObj.GetComponent<Button>().interactable = long.Parse(PrefrenceManager.GOLD) >= ((finalSI == -1) ? gifts[tempIndex].price * 4 : gifts[tempIndex].price);
            giftObj.transform.GetChild(2).gameObject.SetActive(long.Parse(PrefrenceManager.GOLD) < ((finalSI == -1) ? gifts[tempIndex].price * 4 : gifts[tempIndex].price));

            giftObj.GetComponent<Button>().onClick.RemoveAllListeners();
            giftObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (AllCommonGameDialog.instance.isHaveGoldGems(((finalSI == -1) ? gifts[tempIndex].price * 4 : gifts[tempIndex].price) / 2, 0))
                {
                    Logger.Print(" >> GIF Checking >> " + TAG + " Sended Si OnClick_SendGift  >> " + finalSI);
                    GameManager.instance.OnClick_SendGif(gifts[tempIndex].img, "png", gifts[tempIndex].price, finalSI);

                }
            });
        }
        giftCatContent.transform.Find(catName).GetComponent<Image>().sprite = catOn;
    }

    public void SetSuggetionData()
    {
        if (AppData.chatSuggetions.Count == 0) return;

        if (suggestionContent.transform.childCount > AppData.chatSuggetions.Count)
        {
            for (int i = suggestionContent.transform.childCount - 1; i >= AppData.chatSuggetions.Count; i--)
            {
                Destroy(suggestionContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < AppData.chatSuggetions.Count; i++)
        {
            GameObject sug;
            int tempIndex = i;
            if (suggestionContent.transform.childCount < i + 1)
            {
                sug = Instantiate(suggestionPrefab, suggestionContent.transform);
            }
            else
            {
                sug = suggestionContent.transform.GetChild(i).gameObject;
            }

            //sug.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = AppData.chatSuggetions[i];
            sug.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InsertLineBreaks(AppData.chatSuggetions[i], 2);
            sug.name = AppData.chatSuggetions[i];
            sug.GetComponent<Button>().onClick.RemoveAllListeners();
            sug.GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                EventHandler.ChatOnTable(AppData.chatSuggetions[tempIndex]);
            });
        }
    }

    public void SetGIFData(JSONNode data)
    {
        gIFs = JsonConvert.DeserializeObject<List<string>>(data["gif"].ToString());

        if (gIFContent.transform.childCount > gIFs.Count)
        {
            for (int i = gIFContent.transform.childCount - 1; i >= gIFs.Count; i--)
            {
                Destroy(gIFContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < gIFs.Count; i++)
        {
            if (gIFContent.transform.childCount < i + 1)
            {
                GameObject emoji = Instantiate(gIFPrefab, gIFContent.transform);
                emoji.gameObject.name = GameManager.instance.emojiClips[i].name;
            }
        }
        ShowGIFData();
    }

    private void ShowGIFData()
    {
        for (int i = 0; i < gIFContent.transform.childCount; i++)
        {
            int tempIndex = i;
            Logger.Print(TAG + " I is : " + i + " >> Child Count >> " + gIFContent.transform.childCount);
            StartCoroutine(AppData.ProfilePicSet(gIFs[i], gIFContent.transform.GetChild(i).GetComponent<RawImage>()));
            gIFContent.transform.GetChild(i).GetComponent<UniGifImage>().myEmojiNumber = i;
            gIFContent.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            gIFContent.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.instance.OnClick_SendGif(gIFs[tempIndex], "gif", 0, finalSI, gIFContent.transform.GetChild(tempIndex).GetComponent<UniGifImage>().myEmojiNumber);
            });
        }
    }

    private void Reset_ClickedGiftCategory()
    {
        for (int i = 0; i < giftCatContent.transform.childCount; i++) giftCatContent.transform.GetChild(i).GetComponent<Image>().sprite = catOff;
    }
}
