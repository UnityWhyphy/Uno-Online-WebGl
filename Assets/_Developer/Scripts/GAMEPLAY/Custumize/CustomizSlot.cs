using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static LevelInfo;

public class CustomizSlot : MonoBehaviour
{
    public bool lockStatus;
    public Image bgImg;

    [SerializeField] GameObject infoScreen;
    public Text winningAmountTxt, entreeFeeTxt, entryGemsTxt;
    public GameObject lockImg;
    public TextMeshProUGUI lockImgTxt;
    public Button btnClick;
    public GameObject plusIcon;
    public GameObject gemIcon;
    public Image chestImg;

    [Header("Mode")]
    public GameObject soloModObj;
    public GameObject parterModObj;

    //public int custom_Index;

    [Header("Info")]
    public TextMeshProUGUI in_Xp;
    public TextMeshProUGUI in_winXp;
    public Text in_1st, in_2st, partnerIn_1st;

    public void InfoHandleClick(int i)
    {
        switch (i)
        {
            case 1: // Info
                //infoScreen.SetActive(i == 1);
                break;

            case 2: // Info Close 
                break;
        }
        infoScreen.SetActive(i == 1);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
    }

}
