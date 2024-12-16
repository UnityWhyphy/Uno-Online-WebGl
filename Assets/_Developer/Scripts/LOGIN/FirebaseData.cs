
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using System;
using SimpleJSON;
//using Firebase.Messaging;
//using Firebase.Extensions;
//using Firebase.DynamicLinks;
//using Firebase.Crashlytics;
//using Firebase.RemoteConfig;


public class FirebaseData : MonoBehaviour
{
    // Il Its required for google signin and we can find this api key from firebase here
    private static string TAG = ">>>FIREBASE ";
    public static FirebaseData instance;
    //Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Logger.Print(TAG + " REFRELCODEOTHERUSER " + AppData.REFRELCODEOTHERUSER + " FBPROMOCODE " + AppData.FBPROMOCODE);
    }

    void Start()
    {
      /*  FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessage;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(TAG +
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });*/
    }

    #region Firebase Token Genrate  

    //Notification
/*    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        PrefrenceManager.FCM_TOKEN = token.Token;
        Logger.Print(TAG + " Token Generate " + PrefrenceManager.FCM_TOKEN);
    }

    private void OnMessage(object sender, MessageReceivedEventArgs e)
    {

    }
    //Remote Config
    void InitializeFirebase()
    {
        Logger.Print(TAG + "Remote Config Ready.");
        DynamicLinks.DynamicLinkReceived += OnDynamicLink;
        FetchDataAsync();
    }

    public Task FetchDataAsync()
    {
        Logger.Print(TAG + "Fetching data...");
        Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
*/

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Logger.Print(TAG + "Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Logger.Print(TAG + "Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Logger.Print(TAG + "Fetch completed successfully!");
            Showdata();

        }
/*
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    Logger.Print(TAG + string.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));

                    Logger.RecevideLog($"Total values: {remoteConfig.AllValues.Count} | AppData.isLevelWiseLockFirebase : {AppData.isLevelWiseLockFirebase} | {remoteConfig.GetValue("LevleWiseUnlock").BooleanValue}");
                    //AppData.isLevelWiseLockFirebase = int.Parse(remoteConfig.GetValue("LevleWiseUnlock").StringValue);
                    AppData.isLevelWiseLockFirebase = remoteConfig.GetValue("LevleWiseUnlock").BooleanValue;

                    Logger.RecevideLog($"Total values: {remoteConfig.AllValues.Count} | AppData.isLevelWiseLockFirebase : {AppData.isLevelWiseLockFirebase} | {remoteConfig.GetValue("LevleWiseUnlock").StringValue}");

                    foreach (var item in remoteConfig.AllValues)
                    {
                        Logger.RecevideLog("Key :" + item.Key);
                        Logger.RecevideLog("Value: " + item.Value.StringValue);
                        AppData.isLevelWiseLockFirebase = item.Value.StringValue.Equals("1") ? true : false;

                    }
                    Logger.RecevideLog($"Total values | AppData.isLevelWiseLockFirebase : {AppData.isLevelWiseLockFirebase} | {remoteConfig.GetValue("LevleWiseUnlock").StringValue}");
                });
                break;

            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Logger.Print(TAG + "Fetch failed for unknown reason");
                        break;

                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Logger.Print(TAG + "Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;

            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Logger.Print(TAG + "Latest Fetch call still pending.");
                break;
        }*/
    }

    public void Showdata()
    {
        //Logger.Print(TAG + "Current Data:");
        //Logger.Print(TAG + "maintanance: " +
        //         Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        //         .GetValue("maintanance").StringValue);

        //JSONNode obj = new JSONObject();
        //obj.Clear();
        //obj.Add(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        //         .GetValue("maintanance").StringValue);

        //if (AppData.IsMaintance)
        //{

        //}
    }

   /* ShortDynamicLink links;

    public void DynamickLinkGenrate()
    {
        if (!PrefrenceManager.RFLCODE.Equals(""))
        {
            Logger.Print(TAG + "GetShortLinkAsync was Return");
            return;
        }

        string link = "https://com.sixace.tonkking/?invitedby=" + PrefrenceManager.RFLCODE;

        var components = new DynamicLinkComponents(
        // The base Link.
        new Uri(link),
        // The dynamic link URI prefix.
        "https://tonkmultiplayer.page.link")
        {
            IOSParameters = new IOSParameters("com.example.ios"),
            AndroidParameters = new AndroidParameters(
        "com.sixace.tonkmultiplayer"),

            SocialMetaTagParameters = new Firebase.DynamicLinks.SocialMetaTagParameters()
            {
                Title = "Tonk Online Card Game",
                Description = "Tonk Online Is No 1 card Game With Some Unique Play Mode #JOKERMODE",
                ImageUrl = new Uri("https://mysite.com/someimage.jpg")
            },
        };


        var options = new DynamicLinkOptions
        {
            PathLength = DynamicLinkPathLength.Default
        };

        DynamicLinks.GetShortLinkAsync(components, options).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Logger.Print(TAG + "GetShortLinkAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Logger.Print(TAG + "GetShortLinkAsync encountered an error: " + task.Exception);
                return;
            }

            // Short Link has been created.
            links = task.Result;

            PrefrenceManager.RFLCODE = task.Result.Url.ToString();
            Logger.Print(TAG + " result " + PrefrenceManager.RFLCODE);


            var warnings = new List<string>(links.Warnings);

            if (warnings.Count > 0)
            {
                // Debug logging for warnings generating the short link.
            }

        });

    }

    */
 /*   void OnDynamicLink(object sender, EventArgs args)
    {
        var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
        Debug.Log(TAG + string.Format("Received dynamic link {0}",
                               dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString));

        string url = dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;

        Uri myUri = new Uri(url);
        //string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("invitedby");
        //Logger.Print(TAG + "Invite Code " + param1);
        //AppData.REFRELCODEOTHERUSER = param1;

    }*/
    #endregion


    public static void EventSendWithFirebase(string eventName)
    {
//#if !UNITY_EDITOR
//        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
//#endif
    }

    public static void EventSendWithFirebaseLevel(string eventName, string key, int level)
    {
        //#if !UNITY_EDITOR
        //        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName,key,level);
        //#endif
    }
}
