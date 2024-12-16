using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPrefab : MonoBehaviour
{
    public GameObject ChestDataObj, EmptyTxtObj, SkipHrTime, VideoIcon, TimerIcon, TapToUnlockTooltip;
    public Image ChestImg, TapToUnlockBtnImg;
    public Text ChestTimer, BtnTxt;
    public Button SkipTime, UnlockBtn, unlockSecBtn;
    public Button openAgainBtn;
    public int index;
}
