using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_screen : MonoBehaviour
{
    private string TAG = " >> Loading Screen >> ";
    public static Loading_screen instance;
    public GameObject trapImage;

    public GameObject LoaderPanel;
    [SerializeField] TextMeshProUGUI loaderPanelTxt;

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

    public void ShowLoadingScreen(bool show, string loadingTxt = "Please Wait...")
    {
        Logger.Print(TAG + " Called With >> " + show + " >>>> & Is In Reconnecting ?? " + (SocketManagergame.isReconnect && !show));
        if (SocketManagergame.isReconnect && !show) return;
        LoaderPanel.SetActive(show);
        loaderPanelTxt.text = loadingTxt;
    }

    public void SendExe(string screen, string method, JSONNode data, Exception e)
    {
        StartCoroutine(SendExeToServer(screen, method, data, e));
    }

    public IEnumerator SendExeToServer(string screen, string method, JSONNode data, Exception e)
    {
        Logger.Error(TAG + " SendExce called " + data.ToString());
        WWWForm form = new WWWForm();
        form.AddField("en", method);
        form.AddField("screen", screen);
        form.AddField("version", Application.platform == RuntimePlatform.Android ? AppData.AppVersionAndroid : AppData.AppVersionIOS);
        form.AddField("uid", PrefrenceManager._ID);
        form.AddField("det", Application.platform == RuntimePlatform.Android ? "android" : "ios");
        form.AddField("data", data.ToString());
        form.AddField("exception", e != null ? e.Message : "");
        UnityWebRequest www = UnityWebRequest.Post(AppData.BU_PROFILE_URL + "EXEInsert", form);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Logger.Print(TAG + www.error);
        }
        else
        {
            Logger.Print(TAG + " Form upload complete!");
        }
    }
}
