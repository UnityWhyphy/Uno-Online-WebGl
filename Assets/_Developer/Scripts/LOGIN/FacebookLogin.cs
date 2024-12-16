using System.Collections.Generic;
using DG.Tweening;
//using Facebook.Unity;
using UnityEngine;

public class FacebookLogin : MonoBehaviour
{
    private static string TAG = ">>>FACEBOOK";
    string UserName = "";

    public static FacebookLogin instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //FB.Init(SetInit, onHideUnity);
        }
        else
        {
            Destroy(this);
        }
    }

    private void SetInit()
    {
        //Logger.Print(TAG + " SetInit called " + FB.IsInitialized);

        //if (FB.IsInitialized)
        //    FB.ActivateApp();

        //else
        //    Logger.Print(TAG + "Failed to Initialized");
    }

    private void onHideUnity(bool isGameShown)
    {
        Time.timeScale = !isGameShown ? 0 : 1;
    }

    public void FaceBookLogin()
    {
        Logger.Print(TAG + " Login Button Click");
        //AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        //var permission = new List<string>() { "public_profile", "email", "user_friends" };
        //FB.LogInWithReadPermissions(permission, Authcallback);
    }

    //private void Authcallback(ILoginResult result)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        var Token = Facebook.Unity.AccessToken.CurrentAccessToken;

    //        Logger.Print(TAG + "atoken Token " + Token.TokenString);
    //        Logger.Print(TAG + "atoken " + Token.UserId);

    //        foreach (string perm in AccessToken.CurrentAccessToken.Permissions)
    //        {
    //            Logger.Print(TAG + " Permission " + perm);
    //        }

    //        FB.API("/me?fields=id,name,first_name,last_name", HttpMethod.GET, DisplayUsername);
    //        WWW url = new WWW("https" + "://graph.facebook.com/" + Token.UserId + "/picture?type=large");

    //        PrefrenceManager.FID = Token.UserId;
    //        PrefrenceManager.PP = url.url;
    //        PrefrenceManager.FB_TOKEN = Token.TokenString;
    //    }

    //    else
    //    {
    //        Logger.Print(TAG + "User cancel Login");
    //        Loading_screen.instance.ShowLoadingScreen(false);
    //    }
    //}

    public void FBlogout()
    {
        //FB.LogOut();
    }

    //void DisplayUsername(IResult result)
    //{
    //    if (result.Error == null)
    //    {
    //        UserName = result.ResultDictionary["first_name"] + " " + result.ResultDictionary["last_name"];


    //        PrefrenceManager.GID = "";
    //        PrefrenceManager.ULT = "";
    //        PrefrenceManager.PN = "";

    //        PrefrenceManager.ULT = "FB";
    //        PrefrenceManager.PN = UserName;

    //        SplashManager.instance.loginScreen.SetActive(false);
    //        SplashManager.instance.emojiContent.SetActive(true);
    //        SplashManager.instance.progressSlider.anchoredPosition = new Vector2(-435, 0);
    //        SplashManager.instance.progressScreen.SetActive(true);
    //        Loading_screen.instance.LoaderPanel.SetActive(true);
    //        EventHandler.SendSignUp();
    //    }
    //    else
    //    {
    //        Debug.Log(result.Error);
    //    }
    //}

}
