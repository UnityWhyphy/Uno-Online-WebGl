using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchCardManager : MonoBehaviour
{
    public ScratchCardMaskUGUI Mask;

    
    void Update()
    {
        var progress = Mask.GetRevealProgress();
        WinLossScreen.instance.ProgressCheck(progress);
    }
}
