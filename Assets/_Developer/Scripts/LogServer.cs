using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LogServer : MonoBehaviour
{

    public static string filename = "";
    [SerializeField] string logFileName;

    //public static string SERVER_URL;
    //public static string path = Application.persistentDataPath + "/UnoLog.text";

    public static LogServer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Make this object persistent
        }
        else
        {
            Destroy(this.gameObject); // If an instance already exists, destroy this one
        }

        filename = Application.persistentDataPath + "/" + logFileName + ".text";

    }

    public void DeleteLogFile()
    {
        if (File.Exists(filename))
            File.Delete(filename);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    public void Log(string LogMsg, string logStack, LogType log)
    {
        if (SystemInfo.deviceName == "Galaxy A13" || SystemInfo.deviceUniqueIdentifier == "5f33502fe67d4f97") return;

        TextWriter tw = new StreamWriter(filename, true);

        tw.WriteLine("[" + System.DateTime.Now + "]" + LogMsg);

        tw.Close();
    }


    public void UploadLogFile(string filePath, string url)
    {
        StartCoroutine(UploadFileCoroutine(filePath, url));
    }

    private IEnumerator UploadFileCoroutine(string filePath, string url)
    {
        if (!File.Exists(filePath))
        {
            Logger.Error("File does not exist.");
            yield break;
        }
        Logger.Print("Is My UID : " + PrefrenceManager._ID);

        byte[] fileData = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("logfile", fileData, Path.GetFileName(filePath), "text/plain");
        form.AddField("uids", PrefrenceManager._ID);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Logger.Print("File uploaded successfully.");
                Logger.Print(www.downloadHandler.text);
            }
            else
            {
                Logger.Error($"Upload failed: {www.error}");
            }
        }
    }
}
