using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortManager : MonoBehaviour
{
    private string TAG = "PORT MANAGER >>";
    public static PortManager instance;
    [SerializeField] GameObject portChangepanel;
    [SerializeField] Image pCPanelBg;
    [SerializeField] Transform pCPopup;
    [SerializeField] TMP_InputField portInput;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PortChangePanelClick(int click)
    {
        switch (click)
        {
            case 0: //Open
                CommanAnimations.instance.PopUpAnimation(portChangepanel, pCPanelBg, pCPopup, Vector3.one, true);
                break;
            case 1: //Close
                CommanAnimations.instance.PopUpAnimation(portChangepanel, pCPanelBg, pCPopup, Vector3.zero, false, false);
                break;
            case 2: //Change Click             
                if (portInput.text.Trim().Equals(""))
                {
                    AllCommonGameDialog.instance.SetJustOkDialogData("INVALID PORT", "PLEASE ENTER VALID PORT");
                    return;
                }
                PrefrenceManager.CurrentPort = portInput.text.Trim();
                Logger.Print(TAG + "<color=yellow> CHANGED PORT >> </color>" + PrefrenceManager.CurrentPort);
                SocketManagergame.SocketDisConnectManually();
                SocketManagergame.isSingUpGot = false;
                SocketManagergame.isReconnect = false;
                SceneManager.LoadScene(EventHandler.SPLASH);
                PortChangePanelClick(1);
                break;

        }
    }
}
