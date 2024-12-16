using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SimpleJSON;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Image tutorialPopupBG;
    public GameObject playingTutorialScreen;

    [Header("Tutorial Dialog")]
    [SerializeField]
    TextMeshProUGUI tu_titleText, tu_messageText;

    [SerializeField] Transform tutorialPopUp;
    [Space(10)]
    public PlayerController[] players;
    public UserDetails[] userDetails;

    [Space(10)]
    public List<string> myCards = new();
    public List<string> leftPlayerCard = new();
    public List<string> topPlayerCard = new();
    public List<string> rightPlayerCard = new();

    private List<string> myCardsStore = new List<string>();
    private List<string> leftPlayerStore = new List<string>();
    private List<string> topPlayerCardStore = new List<string>();
    private List<string> rightPlayerCardStore = new List<string>();


    [Space(10)]
    public T_Stpe[] steps;
    [SerializeField] Button continueBtn1, continueBtn2, continueBtn3;

    [SerializeField] Button drawButton;
    public GameObject topRightGrid;
    public GameObject topGoldGemsObj;
    public GameObject t_dack1, t_dack2Glow;

    private float typeSpeed = 0.01f;
    public static int stepCount = 0;

    public TutorialWinnerMain tutorialWinnerMain;
    public List<Winner> winnerData = new();

    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        myCardsStore.AddRange(myCards);
        leftPlayerStore.AddRange(leftPlayerCard);
        topPlayerCardStore.AddRange(topPlayerCard);
        rightPlayerCardStore.AddRange(rightPlayerCard);
        //Logger.Print($"Tutorial  start Call...! {myCards.Count} || {myCardsStore.Count}");
    }

    public void HandleNewUserTutorialClick(int index)
    {
        switch (index)
        {
            case 1: // new user
                GameManager.instance.ResetTable(true); // RESET 
                GameManager.instance.allPlayingScreen.gameObject.SetActive(true);
                CommanAnimations.instance.FullScreenPanelAnimation(GameManager.instance.playingScreen, true, 0);
                CommanAnimations.instance.PopUpAnimation(tutorialPopupBG.gameObject, tutorialPopupBG, tutorialPopUp, Vector3.zero, false);
                topRightGrid.gameObject.SetActive(false);
                HandleTutorial(1);
                AppData.canShowChallenge = false;
                GameManager.instance.leaveButton.gameObject.SetActive(false);
                topGoldGemsObj.SetActive(false);
                break;

            case 2: // i'm Pro
                CommanAnimations.instance.PopUpAnimation(tutorialPopupBG.gameObject, tutorialPopupBG, tutorialPopUp, Vector3.zero, false);
                AppData.isTutorialPlay = false;
                AppData.isTutorialFirstTimeReward = false;
                break;

            case 3: // Continue Click
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                playingTutorialScreen.gameObject.SetActive(false);
                HandleTutorial(5);
                break;

            case 5: // Continue Click 2
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                HandleTutorial(20);
                break;

            case 6: // Continue Click 2
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                HandleTutorial(32);
                break;

            case 4: // Open Tutorial Popup

                SetTutorialBg(MessageClass.tutorialBGTitle, MessageClass.tutorialInfomation);
                CommanAnimations.instance.PopUpAnimation(tutorialPopupBG.gameObject, tutorialPopupBG, tutorialPopUp, Vector3.one, true);
                break;
        }
    }

    private void SetTutorialBg(string title, string message)
    {
        tu_titleText.text = title;
        tu_messageText.text = message;
    }

    public void HandleTutorial(int stat)
    {
        switch (stat)
        {
            case 1: // Player Spawn
                StartCoroutine(PlayerSpawn());

                break;

            case 2: // Start Count down
                SocketManagergame.OnListner_SMC?.Invoke(null);
                SocketManagergame.OnListner_GST?.Invoke(null);

                break;

            case 3: // Player Cards Spawn
                CardDistribute();

                break;

            case 4: // Player instraction FTUE
                playingTutorialScreen.gameObject.SetActive(true);
                continueBtn1.interactable = false;
                StepActive(0);
                StartCoroutine(AnimateText(steps[0].tooltipTxt, TutorialStepsTexts.step1));
                break;

            case 5: //My turn Card Throw FTUE
                StartCoroutine(Step5Handle());
                stepCount = 6;
                break;

            case 6: // Bot Turn Left
                StartCoroutine(players[1].TurnUTS(leftPlayerCard));
                GameManager.instance.TutorialUTSHandle(1, GameManager.instance.LeftPlayerTurnRing);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 7: // Bot Top Turn
                StartCoroutine(players[2].TurnUTS(topPlayerCard));
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 8: // Bot Right Turn
                GameManager.instance.TutorialUTSHandle(3, GameManager.instance.RightPlayerTurnRing);
                StartCoroutine(players[3].TurnUTS(rightPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 9: // my turn 2
                playingTutorialScreen.gameObject.SetActive(true);
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(2);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                StartCoroutine(AnimateText(steps[2].tooltipTxt, TutorialStepsTexts.step3, true, 2));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                stepCount = 10;
                break;

            case 10: // Bot Turn Left
                StartCoroutine(players[1].TurnUTS(leftPlayerCard));
                GameManager.instance.TutorialUTSHandle(1, GameManager.instance.LeftPlayerTurnRing);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 11: // Bot Top Turn
                StartCoroutine(players[2].TurnUTS(topPlayerCard));
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 12: // Bot Right Turn
                StartCoroutine(players[3].TurnUTS(rightPlayerCard));
                GameManager.instance.TutorialUTSHandle(3, GameManager.instance.RightPlayerTurnRing);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 13: // my turn 3
                playingTutorialScreen.gameObject.SetActive(true);
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(3);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                StartCoroutine(AnimateText(steps[3].tooltipTxt, TutorialStepsTexts.step4, true, 3));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                stepCount = 14;
                break;

            case 14: // Bot Top Turn
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                StartCoroutine(players[2].TurnUTS(topPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 15: // my turn 4
                playingTutorialScreen.gameObject.SetActive(true);
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(4);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                StartCoroutine(AnimateText(steps[4].tooltipTxt, TutorialStepsTexts.step5, true, 4));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                stepCount = 16;
                break;

            case 16:// Bot Right Turn || Reverse
                GameManager.instance.TutorialUTSHandle(3, GameManager.instance.RightPlayerTurnRing);
                StartCoroutine(players[3].TurnUTS(rightPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 17:// Bot Top Turn
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                StartCoroutine(players[2].TurnUTS(topPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 18:// Bot Left Turn
                GameManager.instance.TutorialUTSHandle(1, GameManager.instance.LeftPlayerTurnRing);
                StartCoroutine(players[1].TurnUTS(leftPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 19: // my turn 5
                     //playingTutorialScreen.gameObject.SetActive(true);
                     //GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                     //StepActive(5);
                     //continueBtn2.interactable = false;
                     //StartCoroutine(AnimateText(steps[5].tooltipTxt, TutorialStepsTexts.step6));
                     //break;

            case 20: // my turn Open Challange Popup
                     //StepActive(6);
                     //steps[7].gameObject.SetActive(true);
                     //StartCoroutine(AnimateText(steps[6].tooltipTxt, TutorialStepsTexts.step6_1));
                     //StartCoroutine(AnimateText(steps[7].tooltipTxt, TutorialStepsTexts.step6_2));
                     //drawButton.interactable = false;
                     //Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                     //GameManager.instance.TutorialKeepPlayPopupon();
                     //break;

            case 21: // my turn 6
                playingTutorialScreen.gameObject.SetActive(true);
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(8);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                StartCoroutine(AnimateText(steps[8].tooltipTxt, TutorialStepsTexts.step7, true, 8));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                stepCount = 22;
                break;

            case 22: // Draw +2 Card on right player
                Logger.Print("22");
                GameManager.instance.TutorialDrawCard(2, 3);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 23: // Bot Turn Top
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                StartCoroutine(players[2].TurnUTS(topPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 24:// Bot left Left
                GameManager.instance.TutorialUTSHandle(1, GameManager.instance.LeftPlayerTurnRing);
                StartCoroutine(players[1].TurnUTS(leftPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 25: // My turn 7 Last card Say
                playingTutorialScreen.gameObject.SetActive(true);
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(9);
                StartCoroutine(AnimateText(steps[9].tooltipTxt, TutorialStepsTexts.step8, true, 9));
                GameManager.instance.UnoBtn.gameObject.SetActive(true);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);

                break;

            case 26:// My turn +4 wild card Throw
                StepActive(10);
                StartCoroutine(AnimateText(steps[10].tooltipTxt, TutorialStepsTexts.step9));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                stepCount = 27;

                break;

            case 27:// Bot Right Turn Draw +4 card
                GameManager.instance.TutorialDrawCard(4, 3);
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 28:// Bot Top Turn
                GameManager.instance.TutorialUTSHandle(2, GameManager.instance.TopPlayerTurnRing);
                StartCoroutine(players[2].TurnUTS(topPlayerCard, true)); // Forget Say Last card
                                                                         // Set new tool tip to say last cad               
                break;

            case 29:// Bot Left Turn
                GameManager.instance.TutorialUTSHandle(1, GameManager.instance.LeftPlayerTurnRing);
                StartCoroutine(players[1].TurnUTS(leftPlayerCard));
                Invoke(nameof(PassTurn), 2.5f);
                break;

            case 30: // My turn Last Wild Card 2
                playingTutorialScreen.gameObject.SetActive(true);
                StepActive(14);
                continueBtn3.interactable = false;
                StartCoroutine(AnimateText(steps[14].tooltipTxt, TutorialStepsTexts.step10));

                t_dack2Glow.SetActive(true);
                AddOrRemoveComponetOnGameObject(t_dack1);
                AddOrRemoveComponetOnGameObject(t_dack2Glow);
                break;

            case 32: // My turn Last Wild Card throw
                GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);
                StepActive(12);
                StartCoroutine(AnimateText(steps[12].tooltipTxt, TutorialStepsTexts.step11, true, 12));
                AddComponetOnGameObject(CardDeckController.instance.playerData[0].myCards[0]);
                Invoke(nameof(DelayMyTurnUpdate), 0.5f);
                stepCount = 31;
                AddOrRemoveComponetOnGameObject(t_dack1, true);
                AddOrRemoveComponetOnGameObject(t_dack2Glow, true);
                break;

            case 31: // Win
                t_dack2Glow.SetActive(false);

                playingTutorialScreen.gameObject.SetActive(false);
                GameManager.instance.WinnerAnimation(winnerData);
                SocketManagergame.OnListner_WIN?.Invoke(null);
                if (AppData.isTutorialFirstTimeReward)
                    EventHandler.VideoReward("tutorial", 200);
                break;
        }
    }

    private void DelayMyTurnUpdate()
    {
        //GameManager.instance.MyTurn = true;
    }

    public void TopUserForgetLastCard()
    {
        playingTutorialScreen.gameObject.SetActive(true);
        StepActive(13);
        StartCoroutine(AnimateText(steps[13].tooltipTxt, TutorialStepsTexts.step9_1));
        stepCount = 29;
    }

    private void PassTurn()
    {
        stepCount++;
        HandleTutorial(stepCount);
    }

    private IEnumerator PlayerSpawn()
    {
        myCards.Clear();
        leftPlayerCard.Clear();
        topPlayerCard.Clear();
        rightPlayerCard.Clear();

        GameManager.instance.TutorialSeatIndexSet();
        yield return new WaitForSeconds(1);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].profilePic.texture = userDetails[i].profilePic;
            //players[i].profileFrm.sprite = userDetails[i].profileFrm;
            players[i].playerName.text = (i == 0) ? DashboardManager.instance.PlayerName.text : userDetails[i].playerName;
            yield return new WaitForSeconds(1);
        }

        myCards.AddRange(myCardsStore);
        leftPlayerCard.AddRange(leftPlayerStore);
        topPlayerCard.AddRange(topPlayerCardStore);
        rightPlayerCard.AddRange(rightPlayerCardStore);

        yield return new WaitForSeconds(1);
        HandleTutorial(2);
    }

    private void CardDistribute()
    {
        JSONNode data = new JSONObject
        {
            ["d"] = 0,
            ["c"] = "r-7-1"
        };
        SocketManagergame.OnListner_SDC?.Invoke(data);
    }

    public IEnumerator AnimateText(TextMeshProUGUI toolTipText, string txt, bool ismypl = false, int i = -1)
    {
        toolTipText.text = "";

        foreach (char c in txt)
        {
            toolTipText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        if (txt.Equals(TutorialStepsTexts.step1))
            continueBtn1.interactable = true;
        else if (txt.Equals(TutorialStepsTexts.step6))
            continueBtn2.interactable = true;
        else if (txt.Equals(TutorialStepsTexts.step10))
            continueBtn3.interactable = true;

        if (ismypl)
        {
            yield return new WaitForSeconds(txt.Length * typeSpeed);
            GameManager.instance.MyTurn = true;
            //StepActive(i);
        }
    }

    void StepActive(int activeIdx)
    {
        foreach (var st in steps)
        {
            st.gameObject.SetActive(false);
        }
        steps[activeIdx].gameObject.SetActive(true);
    }

    private IEnumerator Step5Handle()
    {
        // My Player turn start
        GameManager.instance.TutorialUTSHandle(0, GameManager.instance.BottomPlayerTurnRing);

        yield return new WaitForSeconds(2);
        //GameManager.instance.MyTurn = true;
        playingTutorialScreen.gameObject.SetActive(true);
        StepActive(1);
        StartCoroutine(AnimateText(steps[1].tooltipTxt, TutorialStepsTexts.step2, true));

        var card = CardDeckController.instance.playerData[0].myCards[0];
        AddComponetOnGameObject(card);
    }

    void AddOrRemoveComponetOnGameObject(GameObject c, bool isRemove = false)
    {
        if (!isRemove)
        {
            Canvas can = c.gameObject.AddComponent<Canvas>();
            can.overrideSorting = true;
            can.sortingOrder = 10;
        }
        else
        {
            Canvas canvas = c.GetComponent<Canvas>();
            if (canvas != null)
            {
                Destroy(canvas);
            }
        }
    }

    void AddComponetOnGameObject(CardController c)
    {
        if (c.GetComponent<Canvas>() == null)
        {
            Canvas can = c.gameObject.AddComponent<Canvas>();

            can.overrideSorting = true;
            c.gameObject.AddComponent<GraphicRaycaster>();
            can.sortingOrder = 10;
            c.SetCardGlowAnimation(true);
        }
    }

    public void MyCardThrowAnimation(CardController selectCard, Transform destination)
    {
        selectCard.glowImg.gameObject.SetActive(false);
        selectCard.transform.SetParent(CardDeckController.instance.selectCardParent);
        AudioManager.instance.AudioPlay(AudioManager.instance.cardThrow);

        selectCard.transform.DOMove(destination.transform.position, .3f).OnStart(() =>
        {
            selectCard.transform.DOScale(Vector3.one, 0.1f);
            selectCard.transform.DORotate(new Vector3(35, 0, 0), 0.01f);
            selectCard.transform.DOScale(Vector3.one, .31f).OnComplete(() =>
            {
                if (selectCard.cardValue.Equals("g-10-1"))
                {
                    GameManager.instance.TutorialSkipAnim("g", players[1].playerAnimCard.transform, GameManager.instance.LeftPlayerImg.transform);
                }
                // if value is 11 then work
                GameManager.instance.TutorialReverseAnimation(selectCard.cardValue, true);

                GameManager.instance.CenterGlowCard.SetRingColor(GameManager.getCardColor(selectCard.cardValue));

                selectCard.gameObject.SetActive(false);
                destination.transform.SetSiblingIndex(4);
                destination.gameObject.SetActive(true);

                destination.GetComponent<Image>().sprite = selectCard.cardImage.sprite;
                CardDeckController.instance.RemoveSelfPlayerCard(selectCard.gameObject);
                CardDeckController.instance.ResetMyCardPos();

                if (selectCard.cardValue.Equals("k-1-1"))
                {
                    GameManager.instance.SelectWildCardCall(null);
                    GameManager.instance.allColorButtons[0].interactable = true;
                    playingTutorialScreen.gameObject.SetActive(true);
                    destination.gameObject.GetComponent<Image>().sprite = GameManager.instance.wildPlusAllColor[0];
                    GameManager.instance.CenterGlowCard.SetRingColor("r");
                    StepActive(11);
                }
                else if (selectCard.cardValue.Equals("k-0-1"))
                {
                    GameManager.instance.SelectWildCardCall(null);
                    for (int i = 0; i < GameManager.instance.allColorButtons.Length; i++)
                        GameManager.instance.allColorButtons[i].interactable = true;
                }
                else
                    HandleTutorial(stepCount);
            });
        });
        playingTutorialScreen.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class UserDetails
{
    public Texture profilePic;
    public Sprite profileFrm;
    public string playerName;
}

[System.Serializable]
public class WinData
{
    public int si;
    public int points;
    public string pn;
    public string frameImg;
    public string pp;
    public string uid;
    public int _iscom;
    public List<string> cards;
    public int w;
    public int rw;
    public object newdxp;
}

[System.Serializable]
public class TutorialWinnerMain
{
    public string en;
    public List<WinData> data = new();
    public string tbid;
    public string mode;
    public bool istournament;
    public int status;
    public int isleave;
    public int stargame;
    public int bv;
    public int gems;
    public int Boosteramt;
    public int _ip;
    public List<string> isSingleround;
    public bool flag;
}
