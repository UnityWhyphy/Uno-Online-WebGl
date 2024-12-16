
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveTableInfo : MonoBehaviour
{
    public Image typeTableImg;
    public PlayerInfo[] playerInfo;
    public Image[] rules;

    [System.Serializable]
    public class PlayerInfo
    {
        public RawImage userProfileImg;
        public Image userRingImg;
        public GameObject userMask;
        public Button sitClick;
    }

    public Image gemsImg;
    public GameObject coinImg;
    public GameObject plusImg;
    public TextMeshProUGUI coinTxt;
    public TextMeshProUGUI gemsValueTxt;
}
