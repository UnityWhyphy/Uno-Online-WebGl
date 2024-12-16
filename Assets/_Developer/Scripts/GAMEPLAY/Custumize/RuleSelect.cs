using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleSelect : MonoBehaviour
{
    [SerializeField] int ruleNumber;
    public Image btnSelectImg;
    public Button selectBtn;
    public Text typeTxt;
    public bool isSelect;

    public GameObject lockObj;
    public Text lockLevelTxt;

    private void Start()
    {
        btnSelectImg = GetComponent<Image>();
        selectBtn = GetComponent<Button>();
    }

    public void RuleClick()
    {
        if (!isSelect)
        {
            if (PrivateTable.instance.rulasAddList.Count < 6)
            {
                isSelect = true;
                btnSelectImg.sprite = PrivateTable.instance.r_select;
                PrivateTable.instance.rulasAddList.Add(ruleNumber);
            }
            else
            {
                PrivateTable.instance.ShowToolTip(MessageClass.Only6Rule);
            }
        }
        else
        {
            isSelect = false;
            btnSelectImg.sprite = PrivateTable.instance.r_deselect;
            PrivateTable.instance.rulasAddList.Remove(ruleNumber);
        }
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
    }

    public void RuleInfoClick()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        PrivateTable.instance.RuleInfoPopup(ruleNumber - 1);
    }
}
