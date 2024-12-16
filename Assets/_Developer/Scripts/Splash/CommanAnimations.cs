using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommanAnimations : MonoBehaviour
{
    public static CommanAnimations instance;

    public float animationTime = 0.3f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PanelPopUpOpenAntimation(GameObject Panel)
    {
        Panel.SetActive(true);
        Panel.transform.GetComponent<RectTransform>().DOScale(1f, 0.3f).SetEase(Ease.Linear);
    }

    public void PanelPopUpCloseAntimation(GameObject Panel)
    {
        Panel.transform.GetComponent<RectTransform>().DOScale(0f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Panel.SetActive(false);
        });
    }

    public void PopUpAnimation(GameObject panel, Image background, Transform popUp, Vector3 scale, bool isOpenAnim, bool soundPlay = true)
    {
        //Logger.Print($"EG scale 1: {scale} || {panel.name}"); 
        AnimateThisPopUp(panel, background, popUp, scale, isOpenAnim, soundPlay);
    }

    public void FullScreenPanelAnimation(RectTransform panel, bool isOpenAnim, bool soundPlay = true)
    {
        AnimateThisPanel(panel, isOpenAnim, soundPlay);
    }

    public void FullScreenPanelAnimation(GameObject panel, bool isOpenAnim, float t = 0.3f)
    {
        ScreenOpenCloseAnimation(panel, isOpenAnim, t);
    }

    public static void ScreenOpenCloseAnimation(GameObject screen, bool isOpen, float t)
    {
        if (isOpen)
            screen.SetActive(isOpen);
        screen.transform.DOMove(isOpen ? DashboardManager.instance.centerPos.transform.position : DashboardManager.instance.rightpos.transform.position,
            t, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                screen.SetActive(isOpen);
            });
    }

    private void AnimateThisPopUp(GameObject panel, Image background, Transform popUp, Vector3 scale, bool isOpenAnim, bool soundPlay = true)
    {
        if (soundPlay) AudioManager.instance.AudioPlay(AudioManager.instance.aleartPopUp);
        if (isOpenAnim)
        {
            panel.SetActive(true);
            popUp.transform.localScale = Vector3.zero;
        }
        background?.DOFade(isOpenAnim ? 0.7f : 0f, animationTime);
        //Logger.Print($"EG scale : {scale}");
        popUp.DOScale(scale, animationTime).OnComplete(() =>
        {
            if (!isOpenAnim) panel.SetActive(false);
        });
    }
    private void AnimateThisPanel(RectTransform panel, bool isOpenAnim, bool soundPlay = true)
    {
        panel.anchoredPosition = new Vector2(isOpenAnim ? panel.rect.width + 10 : panel.anchoredPosition.x, panel.anchoredPosition.y);
        if (isOpenAnim) panel.gameObject.SetActive(true);

        panel.DOAnchorPos(new Vector2(!isOpenAnim ? panel.rect.width + 10 : 0, panel.anchoredPosition.y), animationTime).OnComplete(() =>
        {
        if (!isOpenAnim) panel.gameObject.SetActive(false);
        });
    }
}
