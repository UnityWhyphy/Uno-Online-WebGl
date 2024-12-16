using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Game.Modules.LocalNotifcation;

public class SplashManager : MonoBehaviour
{
    public static string TAG = " >>> SPLASH ";

    public GameObject splash1, loginScreen, progressScreen;
    public RectTransform progressSlider;
    public RectTransform loginPopup;
    public RectTransform logoRect;
    public HorizontalLayoutGroup harizontalGrid;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Image progressBar;
    public GameObject emojiContent;
    public Image googleLoginImg;
    public Sprite googleLoginSprite, appleLoginSprite;
    public TextMeshProUGUI googleLoginTxt;
    public TextMeshProUGUI fbGoldTxt, guestGoldTxt, googleGoldTxt;

    [Header("ButtonLin")]
    [SerializeField] internal GameObject[] BtnLin;

    public string loadSceneName;
    float time = 0;

    float startTime;
    float totalProgressTime;
    float fillAmount;
    bool loadscene = false;
    private float startTimesec;
    private float endTime;
    public static SplashManager instance;
    public static string LOCALPORT;

    [Obsolete]
    private void Awake()
    {
        Logger.Print(TAG + " Awake called");
        if (instance == null) 
            instance = this;
        else 
            Destroy(gameObject);
        AppData.winLossCoins = 0;

#if UNITY_IOS
        DeviceIDManager.deviceIDHandler += (string deviceid) => {

           if (!string.IsNullOrEmpty(deviceid))            
               PrefrenceManager.DeviceId = deviceid;
        };
        DeviceIDManager.GetDeviceID();
#endif

        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        //Logger.Print($"Scene chamge Time in sec = {DateTime.Now.TimeOfDay}");

        Logger.Print(TAG + $" start called : sevenSelectCounter : {PrefrenceManager.sevenSelectCounter} || sevenCounter : {PrefrenceManager.sevenCounter} || " +
            $"zeroCounter : {PrefrenceManager.zeroCounter} || wildUpStep2 : {PrefrenceManager.wildUpStep2} || " +
            $"wildUpStep : {PrefrenceManager.wildUpStep} || shieldStepCount : {PrefrenceManager.shieldStepCount} || " +
            $"cycloneStepCount : {PrefrenceManager.cycloneStepCount} || highFiveStep : {PrefrenceManager.highFiveStep} || " +
            $"spcificPlayerStepCount : {PrefrenceManager.spcificPlayerStepCount} || spcificOpStepCount : {PrefrenceManager.spcificOpStepCount} || ");
        LOCALPORT = PrefrenceManager.CurrentPort;

        startTime = 0f;
        totalProgressTime = 1f;
        splash1.SetActive(true);

        AppData.FCBonus.Clear();
        AppData.FCCount.Clear();
        AppData.DayBonus.Clear();
        AppData.MainSppinerStoneName.Clear();
        AppData.MainSppiner.Clear();

        StartCoroutine(FindServerWaitForLoadSplash());

        //StartCoroutine(IosNotificaitonController.instance.RequestAuthorization());
        startTimesec = Time.time;

        AndroidNotificaitonController.instance.RequestAutorization();
    }

    private IEnumerator FindServerWaitForLoadSplash()
    {
        yield return new WaitForSeconds(1.5f);

        //yield return new WaitUntil(() => AdmobManager.goForward);

        SocketManagergame.Instance.CalledFindServer();
    }

    private void Update()
    {
        if (splash1.activeSelf == false && !AppData.IsMaintance)
        {
            time += Time.deltaTime;
            if (time < 0.3f)
                loadingText.text = "Please Wait .";
            else if (time < 0.6f)
                loadingText.text = "Please Wait ..";
            else if (time < 0.9f)
                loadingText.text = "Please Wait ...";
            else if (time < 1.2f)
                loadingText.text = "Please Wait ....";
            else
                time = 0;
        }
    }

    private void OnDestroy()
    {
        Logger.Print(TAG + " Ondestroy called");
    }

    public IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            yield return null;
            if (progressBar != null) progressBar.fillAmount = 0;
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                if (!loadscene)
                {
                    StartCoroutine(LoadHomeScene(asyncOperation));
                }
                loadscene = true;
            }
        }
        endTime = Time.time;

        // Calculate the time difference in seconds
        float timeDifference = endTime - startTimesec;
        Debug.Log("Time taken to load scene: " + timeDifference + " seconds.");
        EventHandler.GameLoadTime(timeDifference.ToString("F2"));
    }

    IEnumerator LoadHomeScene(AsyncOperation asyncOperation)
    {
        while (startTime < totalProgressTime)
        {
            yield return null;

            startTime += Time.deltaTime;
            fillAmount = startTime / totalProgressTime;
            progressBar.fillAmount = fillAmount;
        }
        yield return new WaitForSeconds(0.0001f);
        asyncOperation.allowSceneActivation = true;

        yield return new WaitForSeconds(1.5f);
    }
}
