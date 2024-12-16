using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateTable : MonoBehaviour
{
    public static string TAG = ">>PrivateTable ";
    public static PrivateTable instance;

    public RectTransform privateTablePannel;
    public RectTransform customizeTablePannel;
    public GameObject privateTableContent, privateTablePrefab;
    public LevelInfo privateTablePrefabNew;

    [SerializeField] Transform customizeBetSlotContent;
    public CustomizSlot customizeTablePrefab;
    public GameObject rulePopupObj;
    public GameObject rulePopupContent;
    public Image ruleImg;
    public Text ruleTxt, r_title;
    public Sprite c_select, c_deselect, r_select, r_deselect, tagSelect, tagDeselect;
    public Image soloTagImg, teamUpTagImg;

    [SerializeField] TextMeshProUGUI modeName, subModeName;
    [SerializeField] Sprite info, close, tableBoardBg, tableInfoBg, lockGrayImg, unLockImg;
    [SerializeField] GameObject lockmode, locksubMode;
    [SerializeField] RectTransform modeSelector, tableScrollView;
    [SerializeField] ScrollRect tablesScrollRect;
    [SerializeField] TextMeshProUGUI headerText;

    [Space(10)]
    [SerializeField] RectTransform modeSelectClassic;
    public Image soloModeTagImg, teamUpModeTagImg;

    public List<LevelInfo> privetSlotObject = new List<LevelInfo>();
    public List<CustomizSlot> customSlotObject = new List<CustomizSlot>();
    public List<RuleSelect> rulesButtons = new List<RuleSelect>();
    public List<Sprite> rulesSprite = new List<Sprite>();
    [SerializeField] List<string> customRule = new List<string>();
    public List<int> rulasAddList = new List<int>();
    [SerializeField]
    Sprite[] ChestImgReward;

    string _mode = "";
    int selectIndex = -1;

    bool isEmojiClicked = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GameManager.resetGame += ResetCustomSlot;
    }

    private void OnDisable()
    {
        GameManager.resetGame -= ResetCustomSlot;
    }

    private void Start()
    {
        Invoke(nameof(FlipDashDataSet), 0.2f);
    }

    private void FlipDashDataSet()
    {
        gem.SetActive(AppData.NormalEntryGems > 0);
        plus.SetActive(AppData.NormalEntryGems > 0);
        flipGemsTxt.gameObject.SetActive(AppData.NormalEntryGems > 0);

        flipBvTxt.text = $"{ AppData.PrivateTableBootVal(AppData.NormalEntryGold)}";
        if (AppData.NormalEntryGems > 0) flipGemsTxt.text = $"{AppData.numDifferentiation(AppData.NormalEntryGems)}";
    }

    public void OpenBootValuePanel(int firstClick, string mode)
    {
        AppData.canShowChallenge = false;
        Logger.Print(TAG + " >> Current Mode >> " + mode);
        modeSelector.gameObject.SetActive(mode.Equals(AppData.EMOJI) || mode.Equals(AppData.PRIVATE));
        modeSelectClassic.gameObject.SetActive(mode.Equals(AppData.CLASSIC));
        tableScrollView.offsetMax = new Vector2(tableScrollView.offsetMax.x, (mode.Equals(AppData.EMOJI) || mode.Equals(AppData.PRIVATE) || mode.Equals(AppData.CLASSIC)) ? -300f : -165f);
        headerText.text = mode.Equals(AppData.EMOJI) ? "SELECT MODE & BOOT AMOUNT" : mode.Equals(AppData.PRIVATE) ? "CREATE PRIVATE TABLE" : "SELECT BOOT AMOUNT";
        _mode = mode;

        isEmojiClicked = (mode == AppData.EMOJI);
        ModeClick = firstClick;
        if (isEmojiClicked)
        {
            EmojiFirstClick();
        }
        else
            SetPrivateTableData(mode, subModeName.text);
        PrivetTablePanleActiveHandle(true);
        Loading_screen.instance.ShowLoadingScreen(false);
        SubModeClick = 0;
    }

    public void PrivetTablePanleActiveHandle(bool status, float t = 0.3f)
    {
        CommanAnimations.instance.FullScreenPanelAnimation(privateTablePannel.gameObject, status, t);
    }

    public void CustomTablePanleActiveHandle(bool status, float t = 0.3f)
    {
        CommanAnimations.instance.FullScreenPanelAnimation(customizeTablePannel.gameObject, status, t);
    }

    string[] ModeName = new string[3] { "CLASSIC", "PARTNER", "EMOJI" };
    string[] SubMode = new string[2] { "SOLO", "PARTNER" };
    public int ModeClick = 0, SubModeClick = 0;

    public void OnClick_PrivateTable(int click)
    {
        PrivateTableBtnClick(click);
    }

    void EmojiFirstClick()
    {
        ModeClick--;

        if (ModeClick < 0)
            ModeClick = ModeName.Length - 1;

        Logger.Print(TAG + " ModeClick Left " + ModeClick);
        modeName.text = ModeName[ModeClick];
        subModeName.text = SubMode[0];
        lockmode.SetActive(true);
        SetPrivateTableData(_mode, SubMode[0]);
    }

    public void PrivateTableBtnClick(int i, bool soundPlay = true)
    {
        if (i < 0) return;
        if (soundPlay) AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        lockmode.SetActive(isEmojiClicked);

        switch (i)
        {
            case 0://mode left
                ModeClick--;

                if (ModeClick < 0)
                    ModeClick = ModeName.Length - 1;

                Logger.Print(TAG + " ModeClick Left " + ModeClick);
                modeName.text = ModeName[ModeClick];
                locksubMode.SetActive(ModeClick != 2);
                SetPrivateTableData(_mode, ModeName[ModeClick]);
                break;

            case 1://mode right
                ModeClick++;

                if (ModeClick >= ModeName.Length)
                    ModeClick = 0;

                Logger.Print(TAG + " ModeClick Right " + ModeClick);
                modeName.text = ModeName[ModeClick];
                locksubMode.SetActive(ModeClick != 2);
                SetPrivateTableData(_mode, ModeName[ModeClick]);
                break;

            case 2://sub-mode left
                SubModeClick--;

                if (SubModeClick < 0)
                    SubModeClick = SubMode.Length - 1;

                Logger.Print(TAG + " Sub-ModeClick Left " + SubModeClick);
                subModeName.text = SubMode[SubModeClick];
                SetPrivateTableData(_mode, SubMode[SubModeClick]);
                break;

            case 3://sub-mode right
                SubModeClick++;

                if (SubModeClick >= SubMode.Length)
                    SubModeClick = 0;
                Logger.Print(TAG + " Sub-ModeClick Right " + SubModeClick);
                subModeName.text = SubMode[SubModeClick];
                SetPrivateTableData(_mode, SubMode[SubModeClick]);
                break;

            case 4://close
                isEmojiClicked = false;
                AppData.canShowChallenge = true;
                CommanAnimations.instance.FullScreenPanelAnimation(privateTablePannel, false);
                break;

            case 5://Customize close
                AppData.canShowChallenge = true;
                CommanAnimations.instance.FullScreenPanelAnimation(customizeTablePannel, false);
                ResetCustomSlot();
                break;
        }
    }

    private Sprite GetSpriteCHest(string name)
    {
        switch (name)
        {
            case "gold":
                return ChestImgReward[2];

            case "silver":
                return ChestImgReward[1];

            default:
                return ChestImgReward[0];
        }
    }

    public void SetPrivateTableData(string mode, string subMod = "SOLO")
    {
        List<TableBootValue> data = AppData.TableSloteInGame;
        Logger.Print($" Mode ???? = |mode {mode} || subMod :  {subMod} ");

        Logger.Print($"III {data.Count}");
        try
        {
            for (int i = 0; i < data.Count; i++)
            {
                LevelInfo currentBoard;
                if (i < privateTableContent.transform.childCount)
                {
                    currentBoard = privetSlotObject[i];
                }
                else
                {
                    currentBoard = Instantiate(privateTablePrefabNew, privateTableContent.transform);
                    currentBoard.index = i;
                    privetSlotObject.Add(currentBoard);
                }
                //For Table...
                int prizeValue = -1;
                string winingAmt = "";
                currentBoard.soloModObj.SetActive(false);
                currentBoard.parterModObj.SetActive(false);
                if (mode.Equals(AppData.PRIVATE))
                {
                    if (subMod == SubMode[0] || subMod != ModeName[1])
                    {
                        prizeValue = data[i].winbv;
                        //currentBoard.soloModObj.SetActive(subMod == SubMode[0]);
                        currentBoard.soloModObj.SetActive(true);
                        currentBoard.prizeValue.text = AppData.numDifferentiation(data[i].solowinbv1);
                    }
                    else
                    {
                        currentBoard.parterModObj.SetActive(true);
                        prizeValue = data[i].winbv / 2;
                        currentBoard.prizeValue.text = AppData.numDifferentiation(prizeValue);
                    }
                }
                else
                {
                    if ((mode.Equals(AppData.EMOJI) && subMod != SubMode[0]) || mode.Equals(AppData.PARTNER))
                    {
                        prizeValue = data[i].winbv / 2;
                        currentBoard.prizeValue.text = AppData.numDifferentiation(prizeValue);
                        currentBoard.soloModObj.SetActive(true);
                    }
                    else
                    {
                        currentBoard.parterModObj.SetActive(true);
                        prizeValue = data[i].winbv;
                        currentBoard.prizeValue.text = AppData.numDifferentiation(data[i].solowinbv1);
                    }
                }

                if ((subMod == SubMode[0]))
                {
                    //currentBoard.soloModInfo.totalprize.text = $"WIN PRIZE: = ";
                    currentBoard.soloModInfo.firstWinner.text = $"WIN";
                    currentBoard.soloModInfo.secWinner.text = $"2nd";

                    currentBoard.soloModInfo._prize.text = $"{AppData.numDifferentiation(data[i].winbv)}";
                    currentBoard.soloModInfo._1Winner.text = $"{AppData.numDifferentiation(data[i].solowinbv1)}";
                    currentBoard.soloModInfo.par_1Winner.text = $"{ AppData.PrivateTableBootVal(prizeValue / 2)}";
                    //currentBoard.soloModInfo.par_1Winner.text = $"{AppData.numDifferentiation(data[i].solowinbv1)}";
                    currentBoard.soloModInfo._2Winner.text = $"{AppData.numDifferentiation(data[i].solowinbv2)}";
                }

                //if (data[i].chest.Equals())GetSpriteCHest
                currentBoard.chestImg.sprite = GetSpriteCHest(data[i].chest);

                winingAmt = $"{prizeValue}";
                //currentBoard.prizeValue.text = AppData.numDifferentiation(prizeValue);

                currentBoard.bootValue.text = (data[i].gems > 0) ? AppData.PrivateTableBootVal(data[i].bv) : AppData.numDifferentiation(data[i].bv);
                currentBoard.plusIcon.SetActive(data[i].gems > 0);
                currentBoard.gemIcon.SetActive(data[i].gems > 0);
                currentBoard.bootGemsValue.gameObject.SetActive(data[i].gems > 0);
                currentBoard.bootGemsValue.text = AppData.numDifferentiation(data[i].gems);

                if (AppData.currantLvl < data[i].level)
                {
                    currentBoard.lockImg.gameObject.SetActive(true);
                    currentBoard.btnClick.GetComponent<Image>().sprite = lockGrayImg;
                    currentBoard.lockImgTxt.text = $"requird level {data[i].level}\r\nto unlock these pack";
                }
                else
                {
                    currentBoard.lockImg.gameObject.SetActive(false);
                    currentBoard.btnClick.GetComponent<Image>().sprite = unLockImg;
                }

                currentBoard.btnClick.onClick.RemoveAllListeners();
                currentBoard.btnClick.onClick.AddListener(() =>
                {
                    int index = currentBoard.index;

                    try
                    {
                        if (AllCommonGameDialog.instance.isHaveGoldGems(data[index].bv, data[index].gems))
                        {
                            OnClick_CreateTable(mode, data[index].bv, 4, data[index].gems);
                        }
                    }
                    catch (Exception e)
                    {
                        JSONNode objects = new JSONObject
                        {
                            ["dataCount"] = data.Count,
                            ["child_coint"] = privateTableContent.transform.childCount,
                            ["index"] = index,
                        };
                        Loading_screen.instance.SendExe("PrivetTable-Inner", "SetPrivateTableData", $"{objects}", e);
                    }
                });

                currentBoard.maintableInfo.gameObject.SetActive(true);
                currentBoard.tableInfo.gameObject.SetActive(false);
                currentBoard.GetComponent<Image>().sprite = tableBoardBg;
                currentBoard.infoIcon.sprite = info;
                TableCloseBtnClick(currentBoard, currentBoard.infoBtn, true);

                //For Table Info...
                //currentBoard.playXpText.text = "PLAY 2 XP";
                currentBoard.winXpText.text = "+" + (data[i].xp - 2).ToString() + " XP";
                string multiply = (mode.Equals(AppData.EMOJI) || mode.Equals(AppData.PARTNER)) ? " X 2 = " : " X 4 = ";
                currentBoard.calculationTxt.text = "WIN " + AppData.PrivateTableBootVal(data[i].bv) + multiply + winingAmt + " GOLD";
            }
        }
        catch (Exception ex)
        {
            JSONNode objects = new JSONObject
            {
                ["dataCount"] = data.Count,
                ["child_coint"] = privateTableContent.transform.childCount,
            };
            Loading_screen.instance.SendExe("PrivetTable", "SetPrivateTableData", $"{objects}", ex);
        }

        tablesScrollRect.verticalNormalizedPosition = 1;
    }

    public void SetCutomizeTableData(string subMod)
    {
        Logger.Print($"{TAG} SetCutomizeTableData::  {AppData.TableSloteInGame.Count}");
        List<TableBootValue> data = AppData.TableSloteInGame;
        CustomizSlot selectedBoard = null;
        //int highestValidBv = -1;

        RectTransform mainRect = customizeBetSlotContent.GetComponent<RectTransform>();

        for (int i = 0; i < data.Count; i++)
        {
            int tempIndex = i;
            CustomizSlot currentBoard;

            if (customizeBetSlotContent.transform.childCount < i + 1)
            {
                currentBoard = Instantiate(customizeTablePrefab, customizeBetSlotContent);
                customSlotObject.Add(currentBoard);
            }
            else
            {
                currentBoard = customSlotObject[i];
            }
            //For Table...
            int prizeValue;
            currentBoard.soloModObj.SetActive(false);
            currentBoard.parterModObj.SetActive(false);

            if (subMod != ModeName[0]) // Partner no hoi
            {
                currentBoard.parterModObj.SetActive(true);
                prizeValue = data[i].winbv / 2;
                currentBoard.winningAmountTxt.text = AppData.numDifferentiation(prizeValue);
            }
            else
            {
                currentBoard.soloModObj.SetActive(true);
                prizeValue = data[i].winbv;
                currentBoard.winningAmountTxt.text = AppData.numDifferentiation(data[i].solowinbv1);
            }

            currentBoard.entreeFeeTxt.text = AppData.PrivateTableBootVal(data[i].bv);

            currentBoard.plusIcon.SetActive(data[i].gems > 0);
            currentBoard.gemIcon.SetActive(data[i].gems > 0);
            currentBoard.entryGemsTxt.gameObject.SetActive(data[i].gems > 0);
            currentBoard.entryGemsTxt.text = AppData.numDifferentiation(data[i].gems);
            currentBoard.chestImg.sprite = GetSpriteCHest(data[i].chest);

            if (AppData.currantLvl < data[i].level)
            {
                currentBoard.lockStatus = true;
                currentBoard.lockImg.gameObject.SetActive(true);
                currentBoard.lockImgTxt.text = $"requird level {data[i].level}\r\nto unlock these pack";
            }
            else
            {
                currentBoard.lockStatus = false;
                currentBoard.lockImg.gameObject.SetActive(false);

                if (data[tempIndex].bv == AppData.NormalEntryGold)
                {
                    selectedBoard = currentBoard;
                    selectIndex = tempIndex;
                }
            }

            currentBoard.btnClick.onClick.RemoveAllListeners();
            currentBoard.btnClick.onClick.AddListener(() =>
            {
                if (!currentBoard.lockStatus)
                {
                    ReSelect();
                    if (AllCommonGameDialog.instance.isHaveGoldGems(data[tempIndex].bv, data[tempIndex].gems))
                    {
                        selectIndex = tempIndex;
                        currentBoard.bgImg.sprite = c_select;
                    }
                }
            });

            //For Table Info...
            currentBoard.in_Xp.text = "PLAY 2 XP";
            currentBoard.in_winXp.text = "+" + (data[i].xp - 2).ToString() + " XP";
            currentBoard.in_1st.text = $"{ AppData.numDifferentiation(data[i].solowinbv1)}";
            currentBoard.in_2st.text = $"{AppData.numDifferentiation(data[i].solowinbv2)}";

            var val = AppData.PrivateTableBootVal(data[i].winbv / 2);
            currentBoard.partnerIn_1st.text = $"{val}";
        }

        if (selectedBoard != null)
        {
            foreach (var slot in customSlotObject)
            {
                if (slot != selectedBoard)
                {
                    slot.bgImg.sprite = c_deselect;
                }
            }

            selectedBoard.bgImg.sprite = c_select;

            float x = 0;
            if (selectIndex <= 6) x = 0;
            else if (selectIndex > 6 && selectIndex <= 11) x = -1465f;
            else x = -1939f;
            mainRect.anchoredPosition = new Vector2(x, 0);
            Logger.Print($"{TAG} selectIndex::  {selectIndex}");
        }
    }

    void ReSelect()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        foreach (var item in customSlotObject)
        {
            item.bgImg.sprite = c_deselect;
        }
        //selectIndex = -1;
    }

    private void RulesButton()
    {
        Logger.Print($" rule count  {AppData.rules.Count}");
        for (int i = 0; i < rulesButtons.Count; i++)
        {
            Logger.Print($" RulesButton: rule: {AppData.rules[i]}");
            if (i != AppData.rules[i] - 1)
            {
                rulesButtons[i].gameObject.SetActive(false);
                continue;
            }
        }
    }

    public void RuleInfoPopup(int ruleNum)
    {
        ruleTxt.text = customRule[ruleNum];
        ruleImg.sprite = rulesSprite[ruleNum];
        r_title.text = rulesSprite[ruleNum].name;
        CommanAnimations.instance.PopUpAnimation(rulePopupObj, rulePopupObj.GetComponent<Image>(), rulePopupContent.transform, Vector3.one, true, false);
    }

    public void RuleClose()
    {
        CommanAnimations.instance.PopUpAnimation(rulePopupObj, rulePopupObj.GetComponent<Image>(), rulePopupContent.transform, Vector3.zero, false);
    }

    public void ResetCustomSlot()
    {
        foreach (var item in rulesButtons)
        {
            item.gameObject.SetActive(true);
            item.isSelect = false;
            item.btnSelectImg.sprite = r_deselect;
        }
        rulasAddList.Clear();
    }

    public void ShowToolTip(string tx)
    {
        errorToast.SetActive(true);
        errorToastTxt.text = tx;
        errorToast.transform.DOScale(Vector2.one, .2f);
        Invoke(nameof(DisableToolTip), 2);
    }

    public GameObject errorToast;
    public TextMeshProUGUI errorToastTxt;

    private void DisableToolTip()
    {
        errorToast.SetActive(false);
        errorToast.transform.DOScale(Vector2.zero, .2f);
    }

    public void RuleLockRemove()
    {
        for (int i = 0; i < rulesButtons.Count; i++)
        {
            var ruleB = rulesButtons[i];
            ruleB.lockObj.gameObject.SetActive(false);
            ruleB.selectBtn.interactable = true;
        }
    }

    public void CustomizePlayClick()
    {
        if (selectIndex == -1)
            return;

        if (rulasAddList.Count == 0)
        {
            ShowToolTip(MessageClass.SelectRule);
            return;
        }

        JSONArray rulesArray = new JSONArray();
        if (rulasAddList != null && rulasAddList.Count > 0)
        {
            foreach (int rule in rulasAddList)
            {
                rulesArray.Add(rule);
            }
        }

        EventHandler.PlayGame(_mode, AppData.TableSloteInGame[selectIndex].bv, 4, AppData.TableSloteInGame[selectIndex].gems, 0, rulesArray);
        EventHandler.UENGSend(AppData.TableSloteInGame[selectIndex].bv, AppData.TableSloteInGame[selectIndex].gems);
    }

    [SerializeField] TextMeshProUGUI flipBvTxt, flipGemsTxt;
    [SerializeField] GameObject gem, plus;
    public void FlipPlayClick()
    {
        AppData.canShowChallenge = false;
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
        int selectIndexFlip = 0;
        for (int i = 0; i < AppData.TableSloteInGame.Count; i++)
        {
            if (AppData.TableSloteInGame[i].bv == AppData.NormalEntryGold)
            {
                selectIndexFlip = i;
            }
        }

        EventHandler.PlayGame(AppData.FLIP, AppData.TableSloteInGame[selectIndexFlip].bv, 4, AppData.TableSloteInGame[selectIndexFlip].gems, 0, null, 0);
        EventHandler.UENGSend(AppData.TableSloteInGame[selectIndexFlip].bv, AppData.TableSloteInGame[selectIndexFlip].gems);

        DashboardManager.instance.UpdateIntroSet(8, 1);
    }

    public void CustomizeBootPanel(string mode)
    {
        AppData.canShowChallenge = false;
        Logger.Print(TAG + " >> Current Mode >> " + mode);
        _mode = mode;

        SetCutomizeTableData(mode);
        RulesButton();
        CustomTablePanleActiveHandle(true);
        Loading_screen.instance.ShowLoadingScreen(false);
        SubModeClick = 0;
    }

    public void CustomModeSelect(int i)
    {
        Logger.Print($"CustomModeSelect :: {i}");
        soloTagImg.sprite = (i == 1) ? tagSelect : tagDeselect;
        teamUpTagImg.sprite = (i == 2) ? tagSelect : tagDeselect;
        soloModeTagImg.sprite = (i == 3) ? tagSelect : tagDeselect;
        teamUpModeTagImg.sprite = (i == 4) ? tagSelect : tagDeselect;
        switch (i)
        {
            case 1: // Custom SOLO
                CustomizeBootPanel(AppData.CLASSIC);
                break;

            case 2: // Custom PARTNER
                CustomizeBootPanel(AppData.PARTNER);
                break;

            case 3: // SOLO
                SetPrivateTableData(AppData.CLASSIC, SubMode[0]);
                break;

            case 4: // PARTNER
                SetPrivateTableData(AppData.PARTNER, SubMode[1]);
                break;
        }
    }

    public void IsVelidPlayClip()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
    }

    public void AutoSelectRules(List<int> ruleList)
    {
        Logger.Print($"AutoSelectRules :: {rulasAddList.Count} | | ruleList: {ruleList.Count}");
        rulasAddList.Clear();
        rulasAddList.AddRange(ruleList);
        for (int i = 0; i < ruleList.Count; i++)
        {
            rulesButtons[ruleList[i] - 1].isSelect = true;
            rulesButtons[ruleList[i] - 1].btnSelectImg.sprite = r_select;
            //rulesButtons[ruleList[i] - 1].typeTxt.text = rulesButtons[ruleList[i] - 1].btnSelectImg.sprite.name;
        }
    }

    public void AutoSelectBetSlot()
    {
        if (selectIndex == -1) selectIndex = 0;
        customSlotObject[selectIndex].bgImg.sprite = c_select;
    }

    public void TableCloseBtnClick(LevelInfo currentBoard, Button btnObject, bool isInfo)
    {
        btnObject.onClick.RemoveAllListeners();
        btnObject.onClick.AddListener(() =>
        {
            AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);
            currentBoard.maintableInfo.SetActive(!isInfo);
            currentBoard.tableInfo.SetActive(isInfo);
            currentBoard.GetComponent<Image>().sprite = isInfo ? tableInfoBg : tableBoardBg;
            btnObject.gameObject.GetComponent<Image>().sprite = isInfo ? close : info;
            TableCloseBtnClick(currentBoard, btnObject, !isInfo);
        });
    }

    public void OnClick_CreateTable(string mode, long bv, int players, int gems)
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        if (mode.Equals(AppData.EMOJI) || mode.Equals(AppData.PRIVATE)) mode = ModeClick != 2 ? ModeName[ModeClick] : ModeName[ModeClick] + SubMode[SubModeClick];

        Logger.Print(TAG + " >> Current Mode Last >> " + mode);
        Loading_screen.instance.ShowLoadingScreen(true);

        if (_mode.Equals(AppData.PRIVATE)) EventHandler.CreatePrivateTable(bv, gems, 1, mode, players);
        else
        {
            //mode = "";
            flipBvTxt.text = $"{ AppData.PrivateTableBootVal(bv)}";

            gem.SetActive(gems > 0);
            plus.SetActive(gems > 0);
            flipGemsTxt.gameObject.SetActive(gems > 0);

            if (gems > 0) flipGemsTxt.text = $"{AppData.numDifferentiation(gems)}";
            EventHandler.PlayGame(mode, bv, players, gems, 0, null, _mode.Equals(AppData.FLIP) ? 0 : 1);
            EventHandler.UENGSend(bv, gems);
        }
    }
}
