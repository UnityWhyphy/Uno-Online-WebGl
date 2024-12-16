using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using Cysharp.Threading.Tasks;
using TMPro;

public class Dashboard_MiniGame : MonoBehaviour
{
    private static string TAG = ">>MINIGAMESCREEN";

    public static Dashboard_MiniGame instance;

    public GameObject MiniGamePanel;

    [Header("Starting Pannel")]
    [SerializeField] GameObject StaringPanel;
    [SerializeField] GameObject VideoBtn, GemsImg, GemsBtn, goldImg;
    [SerializeField] Text OrTxt;
    public GameObject halpBtn;
    public Text FreeminiGameTxt, SSAHGems;
    public TextMeshProUGUI sRGGoldTxt;
    [SerializeField] Button CloseBtn;

    [Header("Playing Panel")]
    [SerializeField] GameObject PlayingPanel;
    [SerializeField] GameObject HelpPanel, HelpContent, Hcard, HcardSec, Scard, Sseccard, CollectBtn, SendCard, Hendcard, SstartCard, HstartCard;
    //[SerializeField] Text USGold, TotalLeaf;
    [SerializeField] Text[] WinnerTowerTxt;
    [SerializeField] Sprite[] CorrectImg, Greendot, StarSprites;
    [SerializeField] GameObject[] CorrectMainImg, StarFill;
    [SerializeField] GameObject AnimatioCard, TutoTrans, CarButton, CorrecttxtImg;
    [SerializeField] Image StarLiner;

    [Header("CoinAnimation")]
    [SerializeField] GameObject GoldAnimParent;
    [SerializeField] GameObject EndPostion;

    [Header("Aleart Dialog")]
    [SerializeField] GameObject justOkDialog;
    [SerializeField] Image justOkBg;
    [SerializeField] Transform justOkPopUp;
    [SerializeField] TextMeshProUGUI titleText, messageText, okBtnTxt, noBtnTxt;
    [SerializeField] Button okBtn, noBtn;
    [SerializeField] GameObject gemExitObj;
    [SerializeField] Image redLableOkBtn;


    public string red = "Red", green = "Green", _id;
    List<long> Winreward = new List<long>();
    public List<Sprite> redCards = new List<Sprite>();
    public List<Sprite> greenCards = new List<Sprite>();
    int miniWinIndex, UserWinInex;

    public Sprite backcard;
    bool animTuo = false;
    UserTurnData userTurn;

    void OnEnable()
    {
        SocketManagergame.OnListner_SRAG += OnRecevied_SRAG;
        SocketManagergame.OnListner_USERTURN += OnRecevied_UserTurn;
        SocketManagergame.OnListner_RSH += OnRecevied_RRG;
        SocketManagergame.OnListner_CWA += OnRecevied_CWA;
        SocketManagergame.OnListner_ULE += OnRecevied_ULE;
    }

    void OnDisable()
    {
        SocketManagergame.OnListner_SRAG -= OnRecevied_SRAG;
        SocketManagergame.OnListner_USERTURN -= OnRecevied_UserTurn;
        SocketManagergame.OnListner_RSH -= OnRecevied_RRG;
        SocketManagergame.OnListner_CWA -= OnRecevied_CWA;
        SocketManagergame.OnListner_ULE -= OnRecevied_ULE;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        SSAHGems.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));
        Hcard.SetActive(false);
        Scard.SetActive(false);
        FreeminiGameTxt.text = "Free Game " + AppData.PlayedMiniGame + "/" + AppData.FreeMiniGame;
        Logger.Print($"AppData.PlayedMiniGame  {AppData.PlayedMiniGame } ||  AppData.FreeMiniGame = { AppData.FreeMiniGame}");
        if (AppData.PlayedMiniGame >= AppData.FreeMiniGame)
        {
            OrTxt.gameObject.SetActive(false);
            VideoBtn.SetActive(false);
        }
    }

    public void SetMiniGameData()
    {
        GemsImg.SetActive(true);
        halpBtn.SetActive(true);
        goldImg.SetActive(false);
        StaringPanel.SetActive(true);
        PlayingPanel.SetActive(false);
        if (AppData.PlayedMiniGame >= AppData.FreeMiniGame)
        {
            OrTxt.gameObject.SetActive(false);
            VideoBtn.SetActive(false);
        }
        else
        {
            OrTxt.gameObject.SetActive(true);
            VideoBtn.SetActive(true);
        }
        FreeminiGameTxt.text = "Free Game " + AppData.PlayedMiniGame + "/" + AppData.FreeMiniGame;
        Logger.Print($"AppData.PlayedMiniGame  {AppData.PlayedMiniGame} ||  AppData.FreeMiniGame = { AppData.FreeMiniGame}");
    }

    Tween collectBtnTween;

    public void MiniGameBtnClick(int i)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        switch (i)
        {
            case 0://close
                Logger.Print(TAG + " Id " + _id);

                if (_id == null || _id == "")
                    CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, false);

                else
                {
                    Logger.Print(TAG + " miniWinIndex " + miniWinIndex + " UserWinIndex " + UserWinInex);
                    if (miniWinIndex < 0 || UserWinInex < 0)
                    {
                        Action yes = () =>
                        {
                            Loading_screen.instance.ShowLoadingScreen(true);
                            EventHandler.RemoveRedGreen(_id);
                            CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.zero, false, false);
                        };

                        Action no = () =>
                        {
                            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                            CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.zero, false, false);
                            //CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, false);
                        };
                        okBtn.onClick.RemoveAllListeners();
                        noBtn.onClick.RemoveAllListeners();
                        okBtn.onClick.AddListener(() =>
                        {
                            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                            yes.Invoke();
                        });
                        noBtn.onClick.AddListener(() =>
                        {
                            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                            no.Invoke();
                        });
                        titleText.text = MessageClass.exit;
                        messageText.text = "Are you sure you want to exit the minigame?";
                        okBtnTxt.text = "Yes";
                        noBtnTxt.text = "No";
                        redLableOkBtn.enabled = false;
                        gemExitObj.SetActive(false);
                        CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.one, true, false);
                    }

                    else
                    {
                        if (collectBtnTween != null)
                            collectBtnTween.Kill();
                        Loading_screen.instance.LoaderPanel.SetActive(true);
                        EventHandler.CollectiWinAmmount(_id, false);
                    }
                }
                break;

            case 1://help
                HelpContent.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                HelpPanel.SetActive(true);
                break;

            case 2://help close
                HelpPanel.SetActive(false);
                break;

            case 3://video btn
                Loading_screen.instance.LoaderPanel.SetActive(true);

                if (AppData.VideoTimer == 0)
                {
                    AppData.fromMiniGame = true;
                    AdmobManager.instance.ShowRewardedAd();
                }
                break;

            case 4://play mini game
                if (long.Parse(PrefrenceManager.GEMS) < 7)
                    AllCommonGameDialog.instance.ShowNotEnoughDialog(true, long.Parse(PrefrenceManager.GOLD), long.Parse(PrefrenceManager.GEMS));
                else
                {
                    Loading_screen.instance.LoaderPanel.SetActive(true);
                    EventHandler.StartMiniGame(false);
                }
                break;

            case 5://collect
                Loading_screen.instance.LoaderPanel.SetActive(true);
                EventHandler.CollectiWinAmmount(_id, false);

                if (collectBtnTween != null)
                    collectBtnTween.Kill();
                break;

            case 6://Gems store no call marvano
                Loading_screen.instance.LoaderPanel.SetActive(true);
                EventHandler.GoldStore(1);
                break;

            case 7://Tap to Continue
                //RewardCollectAnim(MainLeafImg, ClaimDestination, new Vector2(30f, 30f), new Vector2(190f, 194f));
                break;

            case 8: // mini exit popup close
                CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.zero, false, false);
                break;
        }
    }

    public void MiniGameClick()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, true);

        GemsImg.SetActive(true);
        halpBtn.SetActive(true);
        goldImg.SetActive(false);
        StaringPanel.SetActive(true);
        PlayingPanel.SetActive(false);
    }

    public void OnRecevied_SRAG(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_SRAG called" + data.ToString());
        Loading_screen.instance.ShowLoadingScreen(false);
        CancelInvoke();
        AnimatioCard.transform.GetComponent<BoxCollider2D>().enabled = false;

        if (DashboardManager.IsRJMini)
        {
            DashboardManager.IsRJMini = false;
            CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, true, 0);
        }
        CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, true);
        CancelInvoke(nameof(StarframChange));
        InvokeRepeating(nameof(StarframChange), 0f, 0.8f);

        Hcard.SetActive(false);
        Scard.SetActive(false);

        StaringPanel.SetActive(false);
        PlayingPanel.SetActive(true);
        GemsImg.SetActive(false);
        halpBtn.SetActive(false);
        goldImg.SetActive(true);
        SSAHGems.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));
        sRGGoldTxt.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));

        MiniGameData miniGame = JsonConvert.DeserializeObject<MiniGameData>(data.ToString());
        FreeminiGameTxt.text = "Free Game " + miniGame.isfreemini + "/" + miniGame.play_try;
        _id = miniGame._id;
        miniWinIndex = miniGame.win_index;
        AppData.fromMiniGame = false;
        Logger.Print(TAG + "Mini over " + miniGame.isover);
        Logger.Print(TAG + "Mini Free game: " + miniGame.isfreemini);
        AppData.PlayedMiniGame = miniGame.isfreemini;
        if (miniGame.isfreemini >= AppData.FreeMiniGame)
        {
            OrTxt.gameObject.SetActive(false);
            VideoBtn.SetActive(false);
        }

        for (int i = 0; i < WinnerTowerTxt.Length; i++)
        {
            WinnerTowerTxt[i].text = AppData.numDifferentiation(miniGame.win_reward[i]);
        }

        Winreward = miniGame.win_reward;
        Logger.Print(TAG + "WinReward " + Winreward.Count);
        SetAllData(miniGame.play_try, miniGame.win_index);

        AnimatioCard.SetActive(!miniGame.isover);

        TutoTrans.transform.position = AnimatioCard.transform.parent.position;
        TutoTrans.SetActive(false);
        animTuo = false;
        CarButton.SetActive(true);

        if (miniGame.isover)
        {
            if (miniGame.win_index != -1)
            {
                CollectBtn.transform.GetComponent<Button>().interactable = true;
                StartUpDownAnimation(CollectBtn);
                Logger.Print(TAG + " CollectBtn" + (userTurn.isover && miniWinIndex >= 0));
            }
            else
            {
                //MiniGameBtnClick(0);
                ExitPopup(MessageClass.opps, "Sorry, you didn't earn any gold this round. Please try again!");
            }
            TransImageAnimFalse();
        }
        else
        {
            Invoke(nameof(TutotialAnim), 3f);
            CollectBtn.transform.GetComponent<Button>().interactable = false;
        }
        HcardSec.SetActive(false);
        Sseccard.SetActive(false);
        AnimatioCard.transform.position = AnimatioCard.transform.parent.position;
        Hcard.transform.position = HcardSec.transform.position;
        Scard.transform.position = Sseccard.transform.position;
    }

    public async void OnRecevied_UserTurn(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_UserTurn called" + data.ToString());
        Loading_screen.instance.LoaderPanel.SetActive(false);
        userTurn = JsonConvert.DeserializeObject<UserTurnData>(data.ToString());
        miniWinIndex = userTurn.win_index;

        UniTask action;

        if (userTurn.type.Equals(green))
            action = CardAnimation(Hcard, AnimatioCard, userTurn.type, userTurn.win_type, HcardSec);
        else
            action = CardAnimation(Scard, AnimatioCard, userTurn.type, userTurn.win_type, Sseccard);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        SetAllData(userTurn.play_try, userTurn.win_index);
        CorrectTxtAnimation(userTurn.win_type);

        //CollectBtn.transform.GetComponent<Button>().interactable = userTurn.isover && miniWinIndex >= 0;
        //Logger.Print(TAG + " CollectBtn" + (userTurn.isover && miniWinIndex >= 0));
        if (miniWinIndex == -1 && userTurn.isover)
        {
            ExitPopup(MessageClass.opps, "Sorry, you didn't earn any gold this round. Please try again!");
        }
    }

    void ExitPopup(string title, string message)
    {
        Action yes = () =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            if (long.Parse(PrefrenceManager.GEMS) < 7)
                AllCommonGameDialog.instance.ShowNotEnoughDialog(true, long.Parse(PrefrenceManager.GOLD), long.Parse(PrefrenceManager.GEMS));
            else
            {
                Loading_screen.instance.LoaderPanel.SetActive(true);
                EventHandler.StartMiniGame(false);
                CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.zero, false, false);
            }
        };
        Action no = () =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.zero, false, false);
            CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, false);
            if (title.Equals(MessageClass.opps))
            {
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.RemoveRedGreen(_id);
            }
        };
        okBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            yes.Invoke();
        });
        noBtn.onClick.AddListener(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            no.Invoke();
        });
        titleText.text = title;
        messageText.text = message;
        okBtnTxt.text = "Yes";
        noBtnTxt.text = "No";
        gemExitObj.SetActive(true);
        redLableOkBtn.enabled = true;
        CommanAnimations.instance.PopUpAnimation(justOkDialog, justOkDialog.GetComponent<Image>(), justOkBg.transform, Vector3.one, true, false);
    }

    public Sprite GetSprite(string card)
    {
        switch (getCardColor(card))
        {
            case "r":

                return redCards[(getCardValue(card) == 0) ? 0 : getCardValue(card) - 1];

            case "g":
                return greenCards[(getCardValue(card) == 0) ? 0 : getCardValue(card) - 1];
        }

        return null;
    }

    public int getCardValue(string card)
    {
        char[] sepratore = { '-' };
        return int.Parse(card.Split(sepratore, 3)[1]);
    }

    public static string getCardColor(string card)
    {
        char[] sepratore = { '-' };
        return card.Split(sepratore, 3)[0];
    }

    public void OnRecevied_RRG(JSONNode data)
    {
        Logger.Print(TAG + " OnRecevied_RSH called" + data);
        Loading_screen.instance.ShowLoadingScreen(false);
        if (!halpBtn.activeInHierarchy) halpBtn.SetActive(true);
        CommanAnimations.instance.FullScreenPanelAnimation(MiniGamePanel, false);
        _id = "";
    }

    public void OnRecevied_CWA(JSONNode data)
    {
        _id = "";
        CloseBtn.interactable = CollectBtn.transform.GetComponent<Button>().interactable = false;
        StartCoroutine(CoinAnimation(data["win_amonut"].AsLong));
    }

    public void SetAllData(int PlayTry, int winIndex)
    {
        for (int i = 0; i < StarFill.Length; i++)
        {
            StarFill[i].SetActive(i < PlayTry);
            Logger.Print($"PlayTry = {PlayTry} || StarFill = {StarFill[i].activeInHierarchy} | i = {i}");
        }

        for (int i = 0; i < CorrectMainImg.Length; i++)
        {
            CorrectMainImg[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = CorrectImg[i <= winIndex ? 0 : 1];
            CorrectMainImg[i].transform.GetChild(1).transform.GetComponent<Image>().sprite = Greendot[i <= winIndex ? 0 : 1];
            CorrectMainImg[i].transform.GetChild(2).transform.GetComponent<Image>().sprite = Greendot[i <= winIndex ? 0 : 1];

            if (i <= winIndex)
            {
                CorrectMainImg[winIndex].transform.GetChild(0).GetComponent<RectTransform>().DOSizeDelta(new Vector2(285, 62), 0.4f).SetEase(Ease.InOutBounce).OnComplete(() =>
                {
                    CorrectMainImg[winIndex].transform.GetChild(0).transform.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 57f);
                });
            }
        }
    }

    public IEnumerator CoinAnimation(long Bonus)
    {
        yield return new WaitForSeconds(1f);
        Loading_screen.instance.LoaderPanel.SetActive(false);

        Logger.Print(TAG + " CoinAnimation called");

        Action GoldAnimComplete = () =>
      {
          Logger.Print(TAG + " Animation Complete Callback MiniGame");
          CloseBtn.interactable = true;
          sRGGoldTxt.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));

          //if (LeafRewardPanel.activeSelf)
          //    AppData.PopupAnimation(Loading_screen.instance.ExitPanel, Loading_screen.instance.ExitPanelImage, Loading_screen.instance.ExitPanelTransform,
          //        Vector3.zero, false, false);

          //else
          //    ShowExitDialog("Congratulations!!!", "Great job!! Would you like to play again?", "Play Now", "Cancel", true);
          ExitPopup(MessageClass.congratulations, "Great job! Would you like to play again?");
      };

        CollectCoinAnimation.instance.SetConfiguration(GoldAnimParent.transform, DashboardManager.instance.spawnLocation.transform, EndPostion.transform, sRGGoldTxt, Bonus, GoldAnimComplete, 0, 20);
    }

    public void LeafRewardAnim(GameObject Source, GameObject Destination, Vector2 ZeroSize, Vector2 NormalSize, GameObject ReturnPos)
    {
        Source.SetActive(true);
        //GlowRound.SetActive(false);
        //SmallGlow.SetActive(false);
        //LeafShadow.transform.GetComponent<RectTransform>().sizeDelta = ZeroSize;
        Destination.gameObject.SetActive(false);
        Source.transform.GetComponent<RectTransform>().sizeDelta = ZeroSize;
        //Source.transform.position = StartPos.transform.position;
        //LeafShadow.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(507f, 470f), 1.3f);
        Source.transform.GetComponent<RectTransform>().DOSizeDelta(NormalSize, 1.3f);

        Source.transform.DOMove(Destination.transform.position, 1.3f, false).OnComplete(() =>
        {
            Source.SetActive(false);
            Source.transform.DOMove(ReturnPos.transform.position, 0.5f, false);
            Source.transform.GetComponent<RectTransform>().sizeDelta = ZeroSize;
            Destination.gameObject.SetActive(true);
            //GlowRound.SetActive(true);
            //GlowRound.transform.DORotate(new Vector3(0f, 0f, -360f), 3f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }).OnStart(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.questReward);
        });
    }

    public void RewardCollectAnim(GameObject Source, GameObject Destination, Vector2 ZeroSize, Vector2 NormalSize)
    {
        //GlowRound.SetActive(false);
        //SmallGlow.SetActive(true);
        //Source.transform.GetComponent<RectTransform>().DOSizeDelta(ZeroSize, 1.3f);
        //SmallGlow.transform.GetComponent<RectTransform>().DOSizeDelta(ZeroSize, 1.3f);
        //SmallGlow.transform.DOMove(Destination.transform.position, 1.3f, false);

        Source.transform.DOMove(Destination.transform.position, 1.3f, false).OnComplete(() =>
        {
            //TotalLeaf.text = AppData.numDifferentiation(int.Parse(PrefrenceManager.LEAF));
            //LeafRewardPanel.SetActive(false);
            //SmallGlow.SetActive(false);

            Source.SetActive(false);
            Source.transform.GetComponent<RectTransform>().sizeDelta = NormalSize;
            Source.transform.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            //SmallGlow.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(490f, 490f);
            //SmallGlow.transform.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);

            Action yes = () =>
            {
                if (long.Parse(PrefrenceManager.GEMS) < 7)
                {
                    //OffeDialog.instance.ShowAllGemsPack(1);
                    return;
                }
                Loading_screen.instance.LoaderPanel.SetActive(true);
                EventHandler.StartMiniGame(false);
                //AppData.PopupAnimation(Loading_screen.instance.ExitPanel, Loading_screen.instance.ExitPanelImage, Loading_screen.instance.ExitPanelTransform,
                //    Vector3.zero, false, false);
            };

            //Action no = () => {
            //    AppData.PopupAnimation(Loading_screen.instance.ExitPanel, Loading_screen.instance.ExitPanelImage, Loading_screen.instance.ExitPanelTransform,
            //        Vector3.zero, false, false);
            //    AppData.ScreenOpenCloseAnimation(MiniGamePanel, false);
            //};

            //Loading_screen.instance.ShowExitDialog("Congratulations!!!", "Great job!! Would you like tomplay again?", "Play Now", "Cancel", yes, no);
        });
    }

    public void OnRecevied_ULE(JSONNode data)
    {
        Loading_screen.instance.LoaderPanel.SetActive(false);
        PrefrenceManager.LEAF = data["leaf"];
        //LeafRewardPanel.SetActive(true);
        //LeafRewardAnim(RewardAnimImg, Destination, new Vector2(0f, 0f), new Vector2(190, 194), LeafStartPos);
    }

    public void CorrectTxtAnimation(string userwintype)
    {
        if (userwintype == "L")
        {
            CorrecttxtImg.SetActive(false);
            CorrecttxtImg.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
        }
        else
        {
            CorrecttxtImg.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            CorrecttxtImg.SetActive(true);
            CorrecttxtImg.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(258f, 79f), 0.7f).SetEase(Ease.InOutBounce).OnComplete(() =>
            {
                CorrecttxtImg.SetActive(false);
            });
        }
    }

    IEnumerator OneTwo()
    {
        yield return new WaitForSeconds(0.3f);
        StarLiner.sprite = StarSprites[0];
        yield return new WaitForSeconds(0.3f);
        StarLiner.sprite = StarSprites[1];
    }

    void StarframChange()
    {
        StartCoroutine(OneTwo());
    }

    void CardRedraw()
    {
        if (userTurn.type.Equals(green))
            Hcard.transform.GetComponent<Image>().sprite = GetSprite(userTurn.card);

        else
            Scard.transform.GetComponent<Image>().sprite = GetSprite(userTurn.card);
    }

    public async UniTask CardAnimation(GameObject Hcard, GameObject AnimationCard, string UserType, string WinType, GameObject HcardSec)
    {
        Hcard.transform.position = AnimationCard.transform.position;
        Hcard.SetActive(true);
        AnimationCard.SetActive(false);
        Logger.Print(TAG + " Animation Start");

        //new
        UniTask Rotate = Hcard.transform.DORotate(new Vector3(0f, 90f, 0f), 0.5f).OnStart(async () =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.card_open_mining);//
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            CardRedraw();
        }).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).ToUniTask();

        UniTask Scale = Hcard.transform.DOScale(1.3f, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).ToUniTask();

        UniTask[] TaskList = new UniTask[] { Rotate, Scale };

        await UniTask.WhenAll(TaskList);

        await Hcard.transform.DOMove(HcardSec.transform.position, 0.5f, false).SetEase(Ease.OutBack).ToUniTask();

        if (UserType.Equals(green))
        {
            if (WinType == "L")
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.minigame_lose);//
                await Hcard.transform.DOMove(Hendcard.transform.position, 1f, false).SetEase(Ease.OutElastic).SetDelay(0.5f).ToUniTask();
                Hcard.transform.GetComponent<Image>().sprite = backcard;
                Hcard.SetActive(false);
                AnimatioCard.transform.position = AnimatioCard.transform.parent.position;
                Hcard.transform.position = HcardSec.transform.position;
                AnimatioCard.SetActive(!userTurn.isover);
                AnimatioCard.transform.GetComponent<BoxCollider2D>().enabled = false;

                if (userTurn.isover)
                {
                    //if (userTurn.red_deck.Count == 0 && userTurn.red_deck.Count == 0)
                    //    ShowExitDialog("Opps!!!", "No worries! Try again for that gold in the next round. Let's play again!", "Play Now", "Cancel", false);
                }
            }
            else
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.minigame_won);//
                HcardSec.SetActive(true);
                Hcard.SetActive(false);
                AnimatioCard.transform.position = AnimatioCard.transform.parent.position;
                AnimatioCard.SetActive(!userTurn.isover);
                AnimatioCard.transform.GetComponent<BoxCollider2D>().enabled = false;
                HcardSec.transform.GetComponent<Image>().sprite = GetSprite(userTurn.green_deck[userTurn.green_deck.Count - 1]);
                Hcard.transform.GetComponent<Image>().sprite = backcard;
            }
        }
        else
        {
            if (WinType == "L")
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.minigame_lose);//
                await Hcard.transform.DOMove(SendCard.transform.position, 1f, false).SetEase(Ease.OutElastic).SetDelay(0.5f).ToUniTask();
                Hcard.SetActive(false);
                AnimatioCard.transform.position = AnimatioCard.transform.parent.position;
                Hcard.transform.position = Sseccard.transform.position;
                AnimatioCard.SetActive(!userTurn.isover);
                AnimatioCard.transform.GetComponent<BoxCollider2D>().enabled = false;
                Hcard.transform.GetComponent<Image>().sprite = backcard;
                if (userTurn.isover)
                {
                    //if (userTurn.green_deck.Count == 0 && userTurn.green_deck.Count == 0)
                    //    ShowExitDialog("Opps!!!", "No worries! Try again for that gold in the next round. Let's play again!", "Play Now", "Cancel", false);
                }
            }
            else
            {
                AudioManager.instance.AudioPlay(AudioManager.instance.minigame_won);//
                Sseccard.SetActive(true);
                Hcard.SetActive(false);
                AnimatioCard.transform.position = AnimatioCard.transform.parent.position;
                AnimatioCard.SetActive(!userTurn.isover);
                AnimatioCard.transform.GetComponent<BoxCollider2D>().enabled = false;
                Sseccard.transform.GetComponent<Image>().sprite = GetSprite(userTurn.red_deck[userTurn.red_deck.Count - 1]);
                Hcard.transform.GetComponent<Image>().sprite = backcard;
            }
        }
        CollectBtn.transform.GetComponent<Button>().interactable = userTurn.isover && miniWinIndex >= 0;
        Logger.Print(TAG + " CollectBtn" + (userTurn.isover && miniWinIndex >= 0));
        if (userTurn.isover && miniWinIndex >= 0)
            StartUpDownAnimation(CollectBtn);

        Logger.Print(TAG + " Animation End");
    }

    private void StartUpDownAnimation(GameObject collectB)
    {
        if (collectBtnTween != null)
            collectBtnTween.Kill();
        collectB.transform.localScale = Vector3.one;
        collectBtnTween = collectB.transform.DOScale(1.1f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    float timeSpeed = 1f;

    public void TransImageAnimFalse()
    {
        CancelInvoke(nameof(TutotialAnim));
        CancelInvoke(nameof(WaitTime));
        TutoTrans.SetActive(false);
        TutoTrans.transform.position = AnimatioCard.transform.parent.position;
        animTuo = true;
    }

    public void TutotialAnim()
    {
        if (!animTuo)
        {
            TutoTrans.SetActive(true);
            CarButton.SetActive(false);

            TutoTrans.transform.DOMove(Scard.transform.position, timeSpeed, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                TutoTrans.transform.DOMove(AnimatioCard.transform.position, timeSpeed, false).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Invoke(nameof(WaitTime), timeSpeed);
                });
            });
        }
    }

    public void WaitTime()
    {
        TutoTrans.transform.DOMove(Hcard.transform.position, timeSpeed, false).SetEase(Ease.Linear).OnComplete(() =>
        {
            TutoTrans.transform.DOMove(AnimatioCard.transform.position, timeSpeed, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                Invoke(nameof(TutotialAnim), timeSpeed);
            });
        });
    }

    public void CardClick()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        TutoTrans.SetActive(false);
        TutoTrans.transform.position = AnimatioCard.transform.parent.position;
        CancelInvoke(nameof(TutotialAnim));
        CarButton.SetActive(false);
        animTuo = true;
    }
}