using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;

using UnityEngine;

public class AppleLogin : MonoBehaviour
{
    private string TAG = ">>>APLLELOGIN ";

    public static AppleLogin instance;
    IAppleAuthManager m_AppleAuthManager;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Update()
    {
        if (m_AppleAuthManager != null)
            m_AppleAuthManager.Update();
    }

    void InitializeAppleAuth()
    {
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            var deserializer = new PayloadDeserializer();
            m_AppleAuthManager = new AppleAuthManager(deserializer);
            StartLoginProcess();
        }
        else
            AllCommonGameDialog.instance.SetJustOkDialogData("SIGN IN FAILED", "Apple Sign-In Failed. Please try after sometime.");
    }

    public void AppleLoginBtnClick()
    {
        Logger.Print(TAG + " Apple Login Button Click");

        // Initialize the Apple Auth Manager
        if (m_AppleAuthManager == null)
            InitializeAppleAuth();
        else
            StartLoginProcess();
    }

    private void StartLoginProcess()
    {
        // Set the login arguments
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeFullName);//LoginOptions.IncludeEmail |

        // Perform the login
        m_AppleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                if (credential != null)
                {
                    PrefrenceManager.ULT = EventHandler.APPLE;
                    PrefrenceManager.AID = credential.User;

                    Loading_screen.instance.ShowLoadingScreen(false);
                    AppData.FromShowIntro = false;
                    EventHandler.SendSignUp();
                }
                else
                    AllCommonGameDialog.instance.SetJustOkDialogData("SIGN IN FAILED", "Apple Sign-In Failed. Please try after sometime.");
            },
            error =>
            {
                AllCommonGameDialog.instance.SetJustOkDialogData("SIGN IN FAILED", "Apple Sign-In Failed. Please try after sometime.");
            }
        );
    }
}
