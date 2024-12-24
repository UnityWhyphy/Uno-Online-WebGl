using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FB_IAP : MonoBehaviour
{

    public static FB_IAP instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void InitiatePurchase(string productID)
    {
        Logger.RecevideLog($"======================== >>>> productID:: {productID} <<<< ========================");
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalCall("initiatePurchase", productID);
#endif
    }
}
