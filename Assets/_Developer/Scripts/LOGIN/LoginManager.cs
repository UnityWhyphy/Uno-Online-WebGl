using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleJSON;

public class LoginManager : MonoBehaviour
{
    private string TAG = " >>> Login ";

    private void Awake()
    {
        Logger.Print(TAG + " Awake called");
    }

    public void OnClick_Guest()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        Logger.Print(TAG + " Guest Click");

        PrefrenceManager.ULT = EventHandler.GUEST;
        PrefrenceManager.DET = EventHandler.DeviceType;

        Logger.Print(TAG + " SP send My side");
        //SplashManager.instance.emojiContent.SetActive(true);
        //SplashManager.instance.loginScreen.SetActive(false);
        SplashManager.instance.progressScreen.SetActive(true);
        SplashManager.instance.progressSlider.anchoredPosition = new Vector2(-435, 0);
        AppData.FromShowIntro = false;
        EventHandler.SendSignUp();
    }

    public void OnClick_Google()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

#if UNITY_ANDROID
        GoogleLogin.instance.GoogleSignInClick();
#elif UNITY_IOS
    AppleLogin.instance.AppleLoginBtnClick();
#endif
        Loading_screen.instance.ShowLoadingScreen(true);
    }

    public void OnClick_FaceBook()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        //FacebookLogin.instance.FaceBookLogin();
        Loading_screen.instance.ShowLoadingScreen(true);
    }
}
