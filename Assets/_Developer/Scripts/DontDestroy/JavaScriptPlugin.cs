using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Facebook.Unity;

public class JavaScriptPlugin : MonoBehaviour
{
    public static JavaScriptPlugin I;

    private void Awake()
    {
        I = this;
    }
    // The static method to handle the Facebook login callback

    //[DllImport("__Internal")]
    //private static extern void InitializeFacebook();

    //[DllImport("__Internal")]
    //private static extern string FacebookLogin();

    public void LoginToFacebook()
    {
        // Start the login process when called
        //StartCoroutine(FacebookLoginCoroutine());
        //FacebookLoginCoroutine();
    }

  

    /*
       public void LoginToFacebook()
    {
        // Start the login process when called
        //StartCoroutine(FacebookLoginCoroutine());
        //FacebookLoginCoroutine();
        CallJavaScriptWithCallback();
    }

    [DllImport("__Internal")]
    private static extern void PerformActionWithCallback(string gameObjectName, string callbackFunctionName, string message);

    // Function to call JavaScript with a callback
    public void CallJavaScriptWithCallback() 
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PerformActionWithCallback(gameObject.name, "OnJavaScriptCallback", "Hello from Unity!");
#else
        Debug.Log("JavaScript calls only work in WebGL builds.");
#endif
    }

    // Callback function to handle JavaScript responses
    public void OnJavaScriptCallback(string message)
    {
        Debug.Log("Callback from JavaScript: " + message);
    }
*/
}

