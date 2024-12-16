using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeButtonModle : MonoBehaviour
{
    public GameObject lockImg;
    public Button click;
    public Text lockLevelTxt;

    private void Start()
    {
        click = GetComponent<Button>();
    }
}
