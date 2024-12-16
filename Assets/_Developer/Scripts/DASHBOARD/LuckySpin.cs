using DG.Tweening;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LuckySpin : MonoBehaviour
{
    private string TAG = " >>> LUCKY SPIN >> ";

    [SerializeField] RectTransform luckySpinPanel;
    [SerializeField] List<TextMeshProUGUI> spinnerValue;
    [SerializeField] List<Image> valueType;
    [SerializeField] Sprite goldSprt, gemsSprt, spinDisable, spinEnable;
    [SerializeField] TextMeshProUGUI myLeafs, myGolds, myGems, spinnerLeaf, infoSliderTxt, infoNoteTxt;
    [SerializeField] Image spinBtnImg;
    [SerializeField] Button spinBtn;
    [SerializeField] Transform wheelCircle;

    [Header("Info PopUp")]
    [SerializeField] GameObject infoPanel;
    [SerializeField] Image infoPanelBg;
    [SerializeField] Transform infoPopUp;
    [SerializeField] Slider infoSlider;

    [Header("Gold & Coin Cut Animation")]
    [SerializeField] GameObject animationTxtObjCoin;
    [SerializeField] GameObject animationTxtObjGems;
    [SerializeField] TextMeshProUGUI animationTextCoin, animationTextGems;

    [Header("LuckyDot Animation")]
    [SerializeField] Image[] dotImg;
    [SerializeField] Sprite n_dot;
    [SerializeField] Sprite g_dot;
    [SerializeField] Image[] lighingAnim;
    [SerializeField] Image blackBG;

    [Header("LuckySpin Animation")]
    [SerializeField] RectTransform lightingImgRect;
    [SerializeField] RectTransform titleImg;
    [SerializeField] Image bgLeftLightingImg;
    [SerializeField] Image bgRightLightingImg;
    [SerializeField] ParticleSystem effect;


    [SerializeField] GameObject pinImage;

    private float[] fillAmounts;
    public float fillDuration = .5f;
    Coroutine wheelAnimCoroutine;
    bool isRevers;

    bool isSpinable = false;
    long requireLeafs = 0;

    private void OnEnable()
    {
        SocketManagergame.OnListner_LS += HandleLuckySpin;
        SocketManagergame.OnListner_CLSR += HandleCollectLuckySpinReward;
        SocketManagergame.OnListner_UG += HandleUpdateGold;
        SocketManagergame.OnListner_UGE += HandleUpdateGems;
        SocketManagergame.OnListner_ULE += HandleUpdateLeaf;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListner_LS -= HandleLuckySpin;
        SocketManagergame.OnListner_CLSR -= HandleCollectLuckySpinReward;
        SocketManagergame.OnListner_UG -= HandleUpdateGold;
        SocketManagergame.OnListner_UGE -= HandleUpdateGems;
        SocketManagergame.OnListner_ULE -= HandleUpdateLeaf;
    }

    private void Start()
    {
        fillAmounts = new float[lighingAnim.Length];
        pinImage.gameObject.SetActive(false);
        spinBtn.interactable = false;
    }

    IEnumerator SpinAnim()
    {
        for (int i = 0; i < dotImg.Length; i++)
        {
            dotImg[i].sprite = isRevers ? n_dot : g_dot;
            yield return new WaitForSeconds(0.1f);

            if (i == dotImg.Length - 1)
            {
                isRevers = !isRevers;

                StartCoroutine(SpinAnim());
            }
        }
    }

    public void FillImagesSimultaneously()
    {
        bgLeftLightingImg.DOFade(1, fillDuration);
        bgRightLightingImg.DOFade(1, fillDuration).OnComplete(() => AudioManager.instance.AudioPlay(AudioManager.instance.lightingClip));
        titleImg.gameObject.SetActive(true);

        titleImg.sizeDelta = new Vector2(1500, 400);
        Sequence s = DOTween.Sequence();
        s.Append(titleImg.DOSizeDelta(new Vector2(553, 141), 0.3f));
        s.Append(lightingImgRect.DOSizeDelta(new Vector2(1285, 845), fillDuration).OnComplete(() =>
        {
            bgLeftLightingImg.DOFade(0, fillDuration);
            bgRightLightingImg.DOFade(0, fillDuration);
            effect.gameObject.SetActive(true);
            spinBtn.interactable = true;
        }));
        s.Append(lightingImgRect.DOSizeDelta(new Vector2(0, 0), fillDuration).SetDelay(0.5f));
    }

    void ResetImages()
    {
        foreach (Image image in lighingAnim)
        {
            image.DOFillAmount(.5f, fillDuration).SetEase(Ease.OutBack);
        }
        pinImage.gameObject.SetActive(false);
    }

    private void HandleLuckySpin(JSONNode data)
    {
        if (wheelAnimCoroutine != null) StopCoroutine(wheelAnimCoroutine);

        foreach (var d in dotImg)
            d.sprite = n_dot;

        wheelAnimCoroutine = StartCoroutine(SpinAnim());
        effect.gameObject.SetActive(false);

        Invoke(nameof(FillImagesSimultaneously), fillDuration);

        isSpinable = false;
        AppData.canShowChallenge = false;
        Logger.Print(TAG + " Lucky Spin Receieved >> ");

        myLeafs.text = AppData.numDifferentiation(data["leaf"].AsLong);
        spinnerLeaf.text = AppData.numDifferentiation(data["requiredLeaf"].AsLong);
        requireLeafs = data["requiredLeaf"].AsLong;
        myGolds.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));
        myGems.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));

        for (int i = 0; i < data["spindata"].Count; i++)
        {
            spinnerValue[i].text = AppData.PrivateTableBootVal(data["spindata"][i]["total"].AsLong);
            valueType[i].sprite = data["spindata"][i]["name"].Equals("gold") ? goldSprt : gemsSprt;
        }

        spinBtnImg.sprite = data["spinactive"].AsBool ? spinEnable : spinDisable;
        isSpinable = data["spinactive"].AsBool;

        infoSlider.maxValue = data["requiredLeaf"];
        infoSlider.value = data["leaf"];
        infoSliderTxt.text = AppData.numDifferentiation(data["leaf"].AsLong) + "/" + AppData.numDifferentiation(data["requiredLeaf"]);
        infoNoteTxt.text = "YOU NEED " + (data["requiredLeaf"] - data["leaf"]) + " MORE LEAF TO SPIN !";

        Loading_screen.instance.ShowLoadingScreen(false);
        CommanAnimations.instance.FullScreenPanelAnimation(luckySpinPanel, true);
    }

    public void OnClick_LuckySpin(int index)
    {
        Logger.Print(TAG + " Click Index Is >> " + index);
        switch (index)
        {
            case 0: //Close Lucky Spin
                if (_isSpinning) return;
                if (wheelAnimCoroutine != null) StopCoroutine(wheelAnimCoroutine);
                AppData.canShowChallenge = true;
                BlackBgStatus(false);
                isSpinable = false;
                titleImg.gameObject.SetActive(false);
                //if (wheelAnimCoroutine != null)
                //    StopCoroutine(wheelAnimCoroutine);
                AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                CommanAnimations.instance.FullScreenPanelAnimation(luckySpinPanel, false);
                break;
            case 1: //Spin Button Click
                if (isSpinable)
                {
                    BlackBgStatus(false);
                    pinImage.gameObject.SetActive(false);
                    EventHandler.CollectLuckySpinReward();
                }
                else CommanAnimations.instance.PopUpAnimation(infoPanel, infoPanelBg, infoPopUp, Vector3.one, true);
                break;
            case 2: //Info PopUp Ok Click
                CommanAnimations.instance.PopUpAnimation(infoPanel, infoPanelBg, infoPopUp, Vector3.zero, false);
                break;
        }
    }

    bool _isSpinning = false;
    float spinDuration = 4.5f;

    private void HandleCollectLuckySpinReward(JSONNode data)
    {
        Logger.Print(TAG + " Collected Lucky Spin Receieved For Index >> " + data["spindex"]);
        if (!_isSpinning)
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.luckSpiner);
            _isSpinning = true;
            spinBtn.interactable = false;

            //Spin
            float angle1 = (360 * spinDuration + 45 * data["spindex"].AsInt) + 180 - 90;
            Vector3 targetRotation = Vector3.forward * (angle1 + 2 * 360 * spinDuration);
            Logger.Print(TAG + "Spinner Index Is  >> " + data["spindex"].AsInt + " >> Old Angle Is1  >. " + targetRotation + " Current Angle1 >> " + wheelCircle.eulerAngles.z);

            wheelCircle.DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart).OnComplete(() =>
            {
                pinImage.gameObject.SetActive(true);
                _isSpinning = false;
                spinBtn.interactable = true;
            });
            FirebaseData.EventSendWithFirebase("LuckeySpin");
        }
    }

    [Header(" Coin Content")]
    [SerializeField] Transform coinPrent;
    [SerializeField] Transform gemsPrent;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform endPosition;
    [SerializeField] Transform endGemsPosition;
    [SerializeField] TextMeshProUGUI userTextGold;
    [SerializeField] TextMeshProUGUI userTextGems;

    private void HandleUpdateGold(JSONNode data)
    {
        if (data["tp"].Equals("Lucky Spin Reward"))
        {
            Action action = () =>
            {
                userTextGold.text = AppData.numDifferentiation(data["gold"].AsLong);
            };
            CollectCoinAnimation.instance.SetConfiguration(coinPrent, spawnLocation, endPosition, userTextGold, data["goldAdded"].AsLong, action, 0, 20);
        }

        PrefrenceManager.GOLD = data["gold"];
        myGolds.text = AppData.numDifferentiation(data["gold"].AsLong);
        CoinAndGemsUpdateAnimation(animationTxtObjCoin, animationTextCoin, data["goldAdded"].AsLong);
        BlackBgStatus(true);
        Invoke(nameof(FalseTransperantImage), 1.5f);

    }

    void FalseTransperantImage()
    {
        pinImage.SetActive(false);
        BlackBgStatus(false);
    }

    private void HandleUpdateGems(JSONNode data)
    {
        if (!data["tp"].Equals("Collect from Red & Green Mini Game"))
        {
            Action action = () =>
            {
                userTextGems.text = AppData.numDifferentiation(data["gems"].AsLong);
            };
            CollectCoinAnimation.instance.SetConfiguration(gemsPrent, spawnLocation, endGemsPosition, userTextGems, data["gemAdded"].AsLong, action, 1, 10);
        }

        PrefrenceManager.GEMS = data["gems"];
        myGems.text = AppData.numDifferentiation(data["gems"].AsLong);
        CoinAndGemsUpdateAnimation(animationTxtObjGems, animationTextGems, data["gemsAdded"].AsLong);
        // anim
        BlackBgStatus(true);
        Invoke(nameof(FalseTransperantImage), 1);

    }

    private void BlackBgStatus(bool status)
    {
        blackBG.gameObject.SetActive(status);
    }

    private void CoinAndGemsUpdateAnimation(GameObject animObject, TextMeshProUGUI textObject, long val)
    {
        if (val == 0) return;

        string text = AppData.numDifferentiation(val);
        Logger.Print("Boot Amount Cuted >> " + text);
        animObject.transform.localPosition = new Vector3(animObject.transform.position.x, 0, animObject.transform.position.z);
        textObject.text = val > 0 ? "<color=green>" + text + "</color>" : "<color=red>" + text + "</color>";
        textObject.DOFade(1, 0f);
        animObject.SetActive(true);
        animObject.transform.DOLocalMoveY(-75, 1f);
        textObject.DOFade(0, 0.3f).SetDelay(0.7f).OnComplete(() =>
        {
            animObject.SetActive(false);
        });
    }

    private void HandleUpdateLeaf(JSONNode data)
    {
        myLeafs.text = AppData.numDifferentiation(data["leaf"].AsLong);
        isSpinable = (data["leaf"].AsLong >= requireLeafs);
        spinBtnImg.sprite = isSpinable ? spinEnable : spinDisable;

        infoSlider.value = data["leaf"];
        infoSliderTxt.text = AppData.numDifferentiation(data["leaf"].AsLong) + "/" + AppData.numDifferentiation(requireLeafs);
        infoNoteTxt.text = "YOU NEED " + AppData.numDifferentiation(requireLeafs - data["leaf"]) + " MORE LEAF TO SPIN !";
    }
}
