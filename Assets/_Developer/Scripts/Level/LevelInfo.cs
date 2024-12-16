using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI prizeValue;
    public TextMeshProUGUI bootValue;
    public TextMeshProUGUI bootGemsValue;
    public TextMeshProUGUI playXpText;
    public TextMeshProUGUI winXpText;

    public GameObject plusIcon;
    public GameObject gemIcon;
    public GameObject maintableInfo;
    public GameObject tableInfo;

    public Button btnClick;
    public Button infoBtn;

    public Image infoIcon;
    public Image lockImg;
    public TextMeshProUGUI lockImgTxt;
    public TextMeshProUGUI calculationTxt;
    public Image chestImg;

    [Header("Mode")]
    public GameObject soloModObj;
    public SoloInfo soloModInfo;
    public GameObject parterModObj;

    [System.Serializable]
    public class SoloInfo
    {
        public TextMeshProUGUI totalprize;
        public TextMeshProUGUI firstWinner;
        public TextMeshProUGUI secWinner;

        public TextMeshProUGUI _prize;
        public TextMeshProUGUI _1Winner;
        public TextMeshProUGUI _2Winner;
        public TextMeshProUGUI par_1Winner;
    }

}
