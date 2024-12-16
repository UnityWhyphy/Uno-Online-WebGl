using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTable : MonoBehaviour
{
    private string TAG = " >> CREATE TABLE >>> ";
    public static CreateTable instance;

    public RectTransform createTablePanel;
    [SerializeField] Slider bootAmtSlider;
    [SerializeField] TextMeshProUGUI bootAmtTxt, gemsAmtTxt;
    [SerializeField] GameObject gemsIcon, plusIcon;
    [SerializeField] List<GameObject> glowObjects;
    string mode = "";
    int bv = 0;
    int gems = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetBootAmount()
    {
        Logger.Print(TAG + " Create Table Slider Value >> " + (int)bootAmtSlider.value);
        bootAmtTxt.text = AppData.numDifferentiation(AppData.TableSloteInGame[(int)bootAmtSlider.value].bv);
        plusIcon.SetActive(AppData.TableSloteInGame[(int)bootAmtSlider.value].gems > 0);
        gemsIcon.SetActive(AppData.TableSloteInGame[(int)bootAmtSlider.value].gems > 0);
        gemsAmtTxt.gameObject.SetActive(AppData.TableSloteInGame[(int)bootAmtSlider.value].gems > 0);
        gemsAmtTxt.text = AppData.numDifferentiation(AppData.TableSloteInGame[(int)bootAmtSlider.value].gems);
        bv = AppData.TableSloteInGame[(int)bootAmtSlider.value].bv;
        gems = AppData.TableSloteInGame[(int)bootAmtSlider.value].gems;
    }

    private void SetTap(int tap)
    {
        for (int i = 0; i < glowObjects.Count; i++) glowObjects[i].SetActive(false);
        glowObjects[tap].SetActive(true);
        bootAmtSlider.maxValue = AppData.TableSloteInGame.Count - 1;
        bootAmtSlider.value = 0;
        bv = AppData.TableSloteInGame[0].bv;
        gems = AppData.TableSloteInGame[0].gems;
        mode = (tap == 0) ? AppData.CLASSIC : (tap == 1) ? AppData.PARTNER : (tap == 2) ? AppData.EMOJISOLO : AppData.EMOJIPARTNER;
    }

    int currentClick = -1;

    public void ResetLiveTableKey()
    {
        currentClick = -1;
    }
    public void CreateTablePanelClick(int Click)
    {
        Logger.NormalLog($"Click :::::: -- {Click}");
        if (currentClick == Click) return;
        if (Click != 6 && Click != 1) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        currentClick = Click;

        switch (Click)
        {
            case 0://Close
                currentClick = -1;
                AppData.canShowChallenge = true;
                CommanAnimations.instance.FullScreenPanelAnimation(createTablePanel, false);
                break;
            case 1: //Create Table
                currentClick = -1;
                Loading_screen.instance.ShowLoadingScreen(true);
                if (AllCommonGameDialog.instance.isHaveGoldGems(bv, gems) && bv > 0 && !mode.Equals(""))
                {
                    AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
                    EventHandler.PlayGame(mode, bv, 4, gems, 0);
                }
                else
                {
                    if (AllCommonGameDialog.instance.isHaveGoldGems(bv, gems) && bv > 0)
                        if (!mode.Equals("")) AllCommonGameDialog.instance.SetJustOkDialogData("INVALID BOOT AMOUNT", "PLEASE SELECT VALID BOOT AMOUNT FOR PLAY");
                        else if (mode.Equals("")) AllCommonGameDialog.instance.SetJustOkDialogData("INVALID MODE", "PLEASE SELECT VALID MODE FOR PLAY");
                    Loading_screen.instance.ShowLoadingScreen(false);
                }
                break;
            case 2: //Classic
                SetTap(0);
                break;
            case 3: //TeamUp
                SetTap(1);
                break;
            case 4: //EmojiSolo
                SetTap(2);
                break;
            case 5: //EmojiPartner
                SetTap(3);
                break;
            case 6: //Create Table Panel Open
                AppData.canShowChallenge = false;
                CommanAnimations.instance.FullScreenPanelAnimation(createTablePanel, true);
                SetTap(0);
                break;
        }
    }
}
