
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Product = UnityEngine.Purchasing.Product;

public class StorePanel : CentralPurchase
{
    private string TAG2 = " >>> Store Panel >> ";
    public static StorePanel Instance;

    [SerializeField] internal RectTransform storePanel;

    [SerializeField] GameObject storeDataView, offerDataView;
    [SerializeField] Text[] BuyText;
    [SerializeField] Image[] GoldImg;
    [SerializeField] TextMeshProUGUI[] GoldValue, SaveText;
    [SerializeField] GameObject[] RemoveAdsTag;
    [SerializeField] Image[] GoldValueImg, TapImg;
    [SerializeField] Image specialDealImg;
    [SerializeField] List<Sprite> goldSprites, gemsSprites, TapSprite, GoldGems;
    [SerializeField] internal TextMeshProUGUI myGoldText, myGemsText;

    [Header("Offer Tap")]
    [SerializeField] GameObject store_LTO;
    [SerializeField] GameObject store_WO, store_LSO;
    [SerializeField]
    TextMeshProUGUI sLTO_TimerTxt, sLTO_ChestCoinTxt, sLTO_GoldCoinsText, sLTO_ExtraSaveTxt, sLTO_DisTxt, sLTO_TotalCoinVal, sLTO_ActualPriceVal,
                                     sWO_TotalCoinTxt, sWO_ActualPriceTxt, sWO_OffTxt,
                                     sLSO_StockCntTxt, sLSO_BoxCoinTxt, sLSO_BagCoinTxt, sLSO_ExtraOffTxt, sLSO_DisTxt, sLSO_TotalCoinTxt, sLSO_ActualPriceTxt;
    [SerializeField]
    Text sLTO_Price, sWO_PriceTxt, sLSO_PriceTxt;

    [Header("SpecialDeal")]
    [SerializeField] internal RectTransform specialDealsPanel;
    [SerializeField] Image[] SpecialDealImg;

    [Header("CheckDeal")]
    [SerializeField] internal GameObject checkDealPanel;
    [SerializeField] internal Image checkDealBG;
    [SerializeField] internal Transform checkDealPopUp;
    [SerializeField] TextMeshProUGUI GoldTxt;

    [Header("Purchase Pannel")]
    [SerializeField] GameObject purchansePanel;
    [SerializeField] Image purchasePanelBG;
    [SerializeField] Transform purchasePanelPopUp;
    [SerializeField] GameObject plusImageBG;
    [SerializeField] GameObject goldsImageBG;
    [SerializeField] GameObject gemsImageBG;
    [SerializeField] TextMeshProUGUI[] GoldValuePurchase, TotalGoldValuePurchase;
    [SerializeField] GameObject TotalPurchaseBg;
    [SerializeField] GameObject[] PurchasePack;
    [SerializeField] Image[] PurchaseGoldImg;

    [Header("Limited Offer Dialog")]
    [SerializeField] GameObject lO_Panel;
    [SerializeField] Image lO_Bg;
    [SerializeField] Transform lO_PopUp;
    [SerializeField] TextMeshProUGUI lO_timerTxt, lO_ChestCoinTxt, lO_CoinsCoinTxt, lO_ExtraSaveTxt, lO_DiscTxt, lO_CoinVal, lO_ActualPriceVal;
    [SerializeField] Text lO_Price;

    [Header("Limited Stock Offer")]
    [SerializeField] GameObject lSO_Panel;
    [SerializeField] Image lSO_Bg;
    [SerializeField] Transform lSO_PopUp;
    [SerializeField] TextMeshProUGUI lSO_StockCntTxt, lSO_BoxCoinTxt, lSO_BagCoinTxt, lSO_ExtraOffTxt, lSO_DisTxt, lSO_TotalCoinTxt, lSO_ActualPriceTxt;
    [SerializeField] Text lSO_PriceTxt;

    [Header("Slot Offer")]
    [SerializeField] GameObject slot_Panel;
    [SerializeField] Image slot_Bg1;
    [SerializeField] SlotOfferInfo slot_PopUp1;
    [Header("SLotBundleBanner")]
    [Space(5)]
    [SerializeField] GameObject slot_Banner_Panel;
    [SerializeField] SlotOfferInfo slot_Banner_PopUp;

    public FGSstoreData GoldOffer, GemsOffer;
    public static List<FGSstoreData> GemsStoreData = new List<FGSstoreData>();
    public static List<FGSstoreData> GoldStoreData = new List<FGSstoreData>();

    Product GoldOfferDetail, GemsOfferDetail;
    List<Product> GoldSKUDetails = new List<Product>();
    List<Product> GemsSKUDetails = new List<Product>();
    public int type = 0;
    int currentIndex = -1;


    public string msg = "Something went wrong please try again after sometime.";
    public string msg1 = "Your Purchase has not completed. If your money has debited from your account, you can contact with us with the order id.";
    public string msg2 = "Your Purchase is Not complete so please try it again Thank You.";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SocketManagergame.OnListener_FGS += HandleStoreData;
        SocketManagergame.OnListener_GSEO += HandleExitOffer;
        SocketManagergame.OnListener_HPG += HandlePaymentGold;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListener_FGS -= HandleStoreData;
        SocketManagergame.OnListener_GSEO -= HandleExitOffer;
        SocketManagergame.OnListener_HPG -= HandlePaymentGold;
    }

    private void HandlePaymentGold(JSONNode data)
    {
        Logger.Print(TAG2 + " HandlePaymentGold called");
        isAnyThinkPurchase = true;

        PrefrenceManager._purchaseNodeData = "";

        if (specialDealsPanel.gameObject.activeInHierarchy)
            CommanAnimations.instance.FullScreenPanelAnimation(specialDealsPanel, false);

        if (OfferDialog.instance.wO_Panel.gameObject.activeInHierarchy) OnClick_StorePanel(22);
        if (lSO_Panel.gameObject.activeInHierarchy) OnClick_StorePanel(23);
        if (lO_Panel.gameObject.activeInHierarchy) OnClick_StorePanel(19);
        if (slot_Banner_Panel.gameObject.activeInHierarchy) OnClick_StorePanel(27);
        if (slot_Panel.gameObject.activeInHierarchy) OnClick_StorePanel(25);

        CommanAnimations.instance.PopUpAnimation(purchansePanel, purchasePanelBG, purchasePanelPopUp, Vector3.one, true);
        PurchaseData purchase = JsonConvert.DeserializeObject<PurchaseData>(data.ToString());
        Logger.Print(tag + " InitGold " + purchase.initgold + " free gold " + purchase.free_gold + " totalgems " + purchase.totalgems);

        PurchasePack[0].SetActive(purchase.initgold != 0);
        PurchasePack[1].SetActive(purchase.free_gold != 0 || purchase.promogold != 0);
        PurchasePack[2].SetActive(purchase.totalgems != 0);

        Logger.Print($"  AppData.NOTIiDS = {AppData.NOTIiDS}");
        GoldValuePurchase[0].text = AppData.numDifferentiation(purchase.initgold);

        if (AppData.NOTIiDS == "")
            GoldValuePurchase[1].text = AppData.numDifferentiation(purchase.free_gold);
        else
        {
            AppData.NOTIiDS = "";
            GoldValuePurchase[1].text = AppData.numDifferentiation(purchase.promogold);
        }

        GoldValuePurchase[2].text = AppData.numDifferentiation(purchase.totalgems);

        TotalGoldValuePurchase[0].text = AppData.numDifferentiation(purchase.totalgold);
        TotalGoldValuePurchase[1].text = AppData.numDifferentiation(purchase.totalgems);

        //TotalPurchaseBg.SetActive(purchase.totalgold != 0 && purchase.totalgems != 0);
        TotalPurchaseBg.SetActive(purchase.totalgold != 0 || purchase.totalgems != 0);
        if (purchase.totalgems == 0)
        {
            plusImageBG.gameObject.SetActive(false);
            goldsImageBG.gameObject.SetActive(true);
            gemsImageBG.gameObject.SetActive(false);
            TotalGoldValuePurchase[1].gameObject.SetActive(false);

        }
        else if (purchase.totalgold == 0)
        {
            plusImageBG.gameObject.SetActive(false);
            gemsImageBG.gameObject.SetActive(true);
            goldsImageBG.gameObject.SetActive(false);
            TotalGoldValuePurchase[0].gameObject.SetActive(false);
        }
        else
        {
            TotalGoldValuePurchase[0].gameObject.SetActive(true);
            TotalGoldValuePurchase[1].gameObject.SetActive(true);
            goldsImageBG.gameObject.SetActive(true);
            gemsImageBG.gameObject.SetActive(true);
            plusImageBG.gameObject.SetActive(true);
        }
        if (FirstTimeOffer != null)
        {
            FirstTimeOffer = null;
            DashboardManager.instance.DashOfferButtonStatus(false);
        }
        PurchaseGoldImg[0].sprite = GetGoldGemsSprite(0, purchase.initgold);
        PurchaseGoldImg[1].sprite = GetGoldGemsSprite(0, purchase.free_gold);
        Loading_screen.instance.ShowLoadingScreen(false);
    }

    private Sprite GetGoldGemsSprite(int goldGems, long gold)
    {
        if (gold > 440000)
            return goldGems == 0 ? goldSprites[0] : gemsSprites[0];

        else if (gold > 265000)
            return goldGems == 0 ? goldSprites[1] : gemsSprites[1];

        else if (gold > 165000)
            return goldGems == 0 ? goldSprites[2] : gemsSprites[2];

        else if (gold > 85000)
            return goldGems == 0 ? goldSprites[3] : gemsSprites[3];

        else if (gold > 20000)
            return goldGems == 0 ? goldSprites[4] : gemsSprites[4];

        else
            return goldGems == 0 ? goldSprites[5] : gemsSprites[5];
    }

    public List<StoreData> DealOffer = new List<StoreData>();
    int showOfferCounter = 0;
    [Obsolete]
    private void HandleExitOffer(JSONNode data)
    {
        Logger.Print(TAG2 + " HandleExitOffer called | showOfferCounter  " + showOfferCounter);

        if (AppData.isTutorialPlay) return;

        if (showOfferCounter != 0)
        {
            showOfferCounter++;
            if (showOfferCounter >= 3)
                showOfferCounter = 0;
            return;
        }
        showOfferCounter++;
        if (DealOffer.Count != 0)
        {
            bool isSame = true;
            for (int i = 0; i < DealOffer.Count; i++)
            {
                if (!DealOffer[i].inapp.Equals(data["data"][i]["inapp"]))
                {
                    isSame = false;
                    break;
                }
            }

            if (isSame)
            {
                Logger.Print(TAG2 + " Special Deal Openend >>");
                Loading_screen.instance.ShowLoadingScreen(false);
                CommanAnimations.instance.FullScreenPanelAnimation(specialDealsPanel, true);
                CommanAnimations.instance.PopUpAnimation(checkDealPanel, checkDealBG, checkDealPopUp, Vector3.zero, false, false);
                return;
            }
        }

        DealOffer = JsonConvert.DeserializeObject<List<StoreData>>(data["data"].ToString());
        Logger.Print(TAG2 + $" HandleExitOffer DealOffer = {DealOffer.Count}");
        var additional = new HashSet<ProductDefinition>();
        Logger.Print(TAG2 + $" HandleExitOffer DealOffer = A");

        for (int i = 0; i < DealOffer.Count; i++)
        {
            Logger.Print(TAG2 + $" HandleExitOffer DealOffer = A {i}");
            StartCoroutine(AppData.SpriteSetFromURL(DealOffer[i].offerimg, SpecialDealImg[i], "HandleExitOffer"));
            additional.Add(new ProductDefinition(DealOffer[i].inapp, ProductType.Consumable));
        }

        Action onSuccess = () =>
        {
            Logger.Print(TAG2 + " Fetched successfully GSEO Offer!!!");
            Loading_screen.instance.ShowLoadingScreen(false);
            CommanAnimations.instance.PopUpAnimation(checkDealPanel, checkDealBG, checkDealPopUp, Vector3.zero, false, false);
            CommanAnimations.instance.FullScreenPanelAnimation(specialDealsPanel, true);
        };

        Action<InitializationFailureReason> onFailure = (InitializationFailureReason i) =>
        {
            Logger.Print(TAG2 + " Fetching failed GSEO Offer reason: " + i);
            Loading_screen.instance.ShowLoadingScreen(false);
            AllCommonGameDialog.instance.SetJustOkDialogData("Alert", msg1);
        };

        CentralPurchase.instance.controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
    }

    [Obsolete]
    private void HandleStoreData(JSONNode data)
    {
        Logger.Print($"{TAG2} | OnHandleStoreData Called");

        isAnyThinkPurchase = false;
        type = data["type"].AsInt;

        if (!storePanel.gameObject.activeInHierarchy) CommanAnimations.instance.FullScreenPanelAnimation(storePanel, true);
        Loading_screen.instance.ShowLoadingScreen(false);

        myGoldText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GOLD));
        myGemsText.text = AppData.numDifferentiation(long.Parse(PrefrenceManager.GEMS));

        var additional = new HashSet<ProductDefinition>();

        GoldStoreData = JsonConvert.DeserializeObject<List<FGSstoreData>>(data["goldstore"].ToString());
        Logger.RecevideLog($"GoldStoreData = {GoldStoreData.Count} || ");
        //GoldStore data
        for (int i = 0; i < GoldStoreData.Count; i++)
        {
            additional.Add(new ProductDefinition(GoldStoreData[i].inapp, ProductType.Consumable));
        }

        GemsStoreData = JsonConvert.DeserializeObject<List<FGSstoreData>>(data["gemsstore"].ToString());
        //GemsStore data
        for (int i = 0; i < GemsStoreData.Count; i++)
        {
            additional.Add(new ProductDefinition(GemsStoreData[i].inapp, ProductType.Consumable));
        }

        GoldOffer = JsonConvert.DeserializeObject<FGSstoreData>(data["promo"].ToString());
        GemsOffer = JsonConvert.DeserializeObject<FGSstoreData>(data["gemspromo"].ToString());

        if (GoldOffer.inapp != null)
            additional.Add(new ProductDefinition(GoldOffer.inapp, ProductType.Consumable));

        if (GemsOffer.inapp != null)
            additional.Add(new ProductDefinition(GemsOffer.inapp, ProductType.Consumable));

        Settap(type);
        if (CentralPurchase.instance.controller != null)
        {
            Action onSuccess = () =>
            {
                for (int i = 0; i < GoldStoreData.Count; i++)
                {
                    foreach (var product in CentralPurchase.instance.controller.products.all)
                    {
                        if (GoldOfferDetail == null && GoldOffer.inapp.Equals(product.definition.storeSpecificId))
                            GoldOfferDetail = product;

                        else if (GemsOfferDetail == null && GemsOffer.inapp.Equals(product.definition.storeSpecificId))
                            GemsOfferDetail = product;

                        if (GoldStoreData[i].inapp.Equals(product.definition.storeSpecificId) && GoldSKUDetails.Count == i)
                            GoldSKUDetails.Add(product);

                        else if (GemsStoreData[i].inapp.Equals(product.definition.storeSpecificId) && GemsSKUDetails.Count == i)
                            GemsSKUDetails.Add(product);
                    }
                }

                Logger.Print(TAG2 + " Fetch Successful");
                Loading_screen.instance.ShowLoadingScreen(false);
                Settap(type);
            };

            Action<InitializationFailureReason> onFailure = (InitializationFailureReason i) =>
            {
                Logger.Print(TAG2 + " Fetching failed GSEO Offer reason: " + i);
                Loading_screen.instance.ShowLoadingScreen(false);
                AllCommonGameDialog.instance.SetJustOkDialogData("Alert", msg1);
            };

            CentralPurchase.instance.controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
        }
        else
        {
            Logger.Print(TAG2 + " Controller null male che");
            Loading_screen.instance.ShowLoadingScreen(false);
        }
    }

    bool isAnyThinkPurchase = false;

    public void OnClick_StorePanel(int i)
    {
        Logger.Print(TAG2 + " OnClick_StorePanel called " + i);
        AudioManager.instance.AudioPlay(AudioManager.instance.ButtonClickClip);

        switch (i)
        {
            case 0: // Buy Now Click
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                AppData.NOTIiDS = "";
                if (GoldStoreData[i] == null) return;
                AppData.PURCHASEDID = type == 0 ? GoldStoreData[i]._id : GemsStoreData[i]._id;
                CentralPurchase.instance.OnPurchaseClicked(type == 0 ? GoldStoreData[i].inapp : GemsStoreData[i].inapp);
                break;

            case 6://close Store
                //if (!isAnyThinkPurchase)
                //{
                //    GoldTxt.text = AppData.numDifferentiation(AppData.VIDEOREWARDCOINS);
                //    CommanAnimations.instance.PopUpAnimation(checkDealPanel, checkDealBG, checkDealPopUp, Vector3.one, true);
                //}
                //else
                {
                    CommanAnimations.instance.FullScreenPanelAnimation(storePanel, false);
                }
                break;

            case 7://check deal Btn Click
                CommanAnimations.instance.PopUpAnimation(checkDealPanel, checkDealBG, checkDealPopUp, Vector3.zero, false, false);
                Loading_screen.instance.ShowLoadingScreen(true);
                EventHandler.GetCheckDealoffer();
                showOfferCounter = 0;
                break;

            case 8://watch video Btn
                AppData.isShownAdsFrom = 4;
                //AdsManager.instance.Show_RewardedAd();
                AdmobManager.instance.ShowRewardedAd();
                break;

            case 9://dialog close
                CommanAnimations.instance.PopUpAnimation(checkDealPanel, checkDealBG, checkDealPopUp, Vector3.zero, false, false);
                CommanAnimations.instance.FullScreenPanelAnimation(storePanel, false);
                break;

            case 10://Special deal close
                CommanAnimations.instance.FullScreenPanelAnimation(specialDealsPanel, false);
                break;

            case 11://Special deal Offers click
            case 12:
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                if (DealOffer[i - 11] != null)
                {
                    AppData.PURCHASEDID = DealOffer[i - 11]._id;
                    CentralPurchase.instance.OnPurchaseClicked(DealOffer[i - 11].inapp);
                }
                break;

            case 14://tap click
            case 15:
            case 16:
                Settap(i - 14);
                break;

            case 17://offer click
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                if (GoldOffer == null) return;
                AppData.PURCHASEDID = type == 0 ? GoldOffer._id : GemsOffer._id;
                CentralPurchase.instance.OnPurchaseClicked(type == 0 ? GoldOffer.inapp : GemsOffer.inapp);
                break;

            case 18://purchase collect
                DashboardManager.instance.ChipsAddAnimation(100, int.Parse(PrefrenceManager.GOLD));
                CommanAnimations.instance.PopUpAnimation(purchansePanel, purchasePanelBG, purchasePanelPopUp, Vector3.zero, false);
                break;

            case 19: // Limited Timer Offer Dialog Close
                CommanAnimations.instance.PopUpAnimation(lO_Panel, lO_Bg, lO_PopUp, Vector3.zero, false, false);
                WatchVideoShowHanlde();
                break;

            case 20: //Limited Timer Offer Dialog Purchase Click
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                if (TimerOffeNew == null) return;
                Logger.Print($"TimerOffeNew.inapp :: {TimerOffeNew.inapp}");
                AppData.PURCHASEDID = TimerOffeNew.pid;
                AppData.NOTIiDS = TimerOffeNew._id;
                CentralPurchase.instance.OnPurchaseClicked(TimerOffeNew.inapp);
                break;

            case 21: //First Welcome Time Offer Dialog Purchase Click
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                if (FirstTimeOffer == null) return;
                Logger.Print($"FirstTimeOffer.inapp :: {FirstTimeOffer.inapp}");
                AppData.PURCHASEDID = FirstTimeOffer._id;
                CentralPurchase.instance.OnPurchaseClicked(FirstTimeOffer.inapp);
                break;

            case 24: //Limited Stock Offer Dialog Purchase Click
                Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                //AppData.PURCHASEDID = (i == 20) ? TimerOffeNew.pid : (i == 24) ? StockOfferNew.pid : "";
                //AppData.NOTIiDS = isTimerOffer ? TimerOffeNew._id : StockOfferNew._id;
                //OnPurchaseClicked((i == 20) ? TimerOffeNew.inapp : (i == 24) ? StockOfferNew._id : "");
                //OnPurchaseClicked((i == 20) ? TimerOffeNew.inapp : (i == 24) ? StockOfferNew.inapp : "");
                if (StockOfferNew == null) return;

                Logger.Print($"StockOfferNew.inapp :: {StockOfferNew.inapp}");

                AppData.PURCHASEDID = StockOfferNew.pid;
                Logger.Print($"StockOfferNew.inapp :: {StockOfferNew._id}");
                AppData.NOTIiDS = StockOfferNew._id;
                CentralPurchase.instance.OnPurchaseClicked((StockOfferNew.inapp));

                break;

            case 22: // Welcome Offer Dialog Close
                CommanAnimations.instance.PopUpAnimation(OfferDialog.instance.wO_Panel,
                    OfferDialog.instance.wO_Bg, OfferDialog.instance.wO_PopUp, Vector3.zero, false, false);
                WatchVideoShowHanlde();
                break;
            case 23: //Limited Stock Offer Dialog Close
                CommanAnimations.instance.PopUpAnimation(lSO_Panel, lSO_Bg, lSO_PopUp, Vector3.zero, false, false);
                WatchVideoShowHanlde();
                break;
            // Slot clicks

            case 25: //BUNDLE Close
                CommanAnimations.instance.PopUpAnimation(slot_Panel, slot_Bg1, slot_PopUp1.transform, Vector3.zero, false, false);
                break;

            case 26: //BUNDLE Open
                CommanAnimations.instance.PopUpAnimation(slot_Panel, slot_Bg1, slot_PopUp1.transform, Vector3.one, true);
                break;
            case 27: //Banner Slot close
                CommanAnimations.instance.PopUpAnimation(slot_Banner_Panel, slot_Banner_Panel.GetComponent<Image>(), slot_Banner_PopUp.transform, Vector3.zero, false, false);
                break;
        }
    }

    void WatchVideoShowHanlde()
    {
        if (long.Parse(PrefrenceManager.GOLD) <= 500)
            WinLossScreen.instance.ShowWatchVideoPopup(true);
    }

    public int calledEO = 0;
    private void Settap(int tap)
    {
        this.type = tap;
        TapImg[0].sprite = TapSprite[tap == 0 ? 0 : 1];
        TapImg[1].sprite = TapSprite[tap == 1 ? 0 : 1];
        TapImg[2].sprite = TapSprite[tap == 2 ? 0 : 1];

        storeDataView.SetActive(tap < 2);
        offerDataView.SetActive(tap == 2);

        if (tap < 2)
        {
            for (int i = 0; i < GoldSKUDetails.Count; i++)
            {
                BuyText[i].text = (GoldSKUDetails[i] == null) ? "$ " + GoldStoreData[i].price : GoldSKUDetails[i].metadata.localizedPriceString;
                GoldImg[i].sprite = tap == 0 ? goldSprites[i] : gemsSprites[i];
                GoldValue[i].text = tap == 0 ? AppData.numDifferentiation(GoldStoreData[i].gold) : AppData.numDifferentiation(GemsStoreData[i].gems);
                RemoveAdsTag[i].SetActive(tap == 0 ? GoldStoreData[i].isadsremove == 1 : GemsStoreData[i].isadsremove == 1);
                SaveText[i].text = tap == 0 ? GoldStoreData[i].txt : GemsStoreData[i].txt;
                GoldValueImg[i].sprite = GoldGems[tap];
            }
            Logger.Print(TAG2 + " Gold Offer Image >> " + specialDealImg.name + " >>> URL is >> " + GoldOffer.offerimg);
            Logger.Print(TAG2 + " Gems Offer Image >> " + specialDealImg.name + " >>> URL is >> " + GemsOffer.offerimg);
            Logger.Print(TAG2 + " GoldOffer >> " + (GoldOffer == null));
            StartCoroutine(AppData.SpriteSetFromURL(tap == 0 ? GoldOffer.offerimg : GemsOffer.offerimg, specialDealImg, "Settap"));
        }
        else
            SetOffersTapData();
    }

    public void SetOffersTapData()
    {
        Logger.Print($"==== >>   SetOffersTapData  << ======== A");

        // TODO: WelcomeOffer
        store_WO.SetActive(FirstTimeOffer != null);
        if (FirstTimeOffer != null)
            SetData_OfferTap_WelcomeOffer();

        // TODO: LimitedStockOffer
        store_LSO.gameObject.SetActive(StockOfferNew != null);
        if (StockOfferNew != null)
            SetData_OfferTap_LimitedStockOffer();

        // TODO: LimitedTimerOffer
        store_LTO.gameObject.SetActive(TimerOffeNew != null);
        if (TimerOffeNew != null)
            SetData_OfferTap_LimitedTimerOffer();
    }

    private void SetData_OfferTap_LimitedTimerOffer()
    {
        sLTO_TimerTxt.text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);

        sLTO_ChestCoinTxt.text = AppData.numDifferentiation(TimerOffeNew.normal_gold);
        sLTO_GoldCoinsText.text = AppData.numDifferentiation(TimerOffeNew.promo_gold);
        sLTO_ExtraSaveTxt.text = "+ " + TimerOffeNew.isextra + "% EXTRA";
        sLTO_DisTxt.text = TimerOffeNew.txt;
        sLTO_TotalCoinVal.text = AppData.numDifferentiation(TimerOffeNew.gold);
        sLTO_ActualPriceVal.text = "$ " + TimerOffeNew.actualprice;

        sLTO_Price.text = (CentralPurchase.TimerOfferProduct == null) ? "$ " + CentralPurchase.TimerOffeNew.price : CentralPurchase.TimerOfferProduct.metadata.localizedPriceString;
    }

    public void SetData_OfferTap_WelcomeOffer()
    {
        if (FirstTimeOffer == null) return;
        sWO_OffTxt.text = FirstTimeOffer.txt + " OFF";
        sWO_TotalCoinTxt.text = AppData.numDifferentiation(FirstTimeOffer.gold);
        sWO_ActualPriceTxt.text = "$ " + FirstTimeOffer.actualprice;
        //sWO_PriceTxt.text = "$ " + FirstTimeOffer.price;

        Logger.Print($"==== >>   SetData_OfferTap_WelcomeOffer  << ======== c(welcome)");
        sWO_PriceTxt.text = (CentralPurchase.TimerOfferProduct == null) ? "$ " + CentralPurchase.FirstTimeOffer.price : CentralPurchase.FirstTimeProduct.metadata.localizedPriceString;
    }

    private void SetData_OfferTap_LimitedStockOffer()
    {
        sLSO_StockCntTxt.text = AppData.numDifferentiation(StockOfferNew.stock - StockOfferNew.usestock);
        sLSO_BoxCoinTxt.text = AppData.numDifferentiation(StockOfferNew.normal_gold);
        sLSO_BagCoinTxt.text = AppData.numDifferentiation(StockOfferNew.promo_gold);
        sLSO_ExtraOffTxt.text = "+ " + StockOfferNew.isextra + "% EXTRA";
        sLSO_DisTxt.text = StockOfferNew.txt;
        sLSO_TotalCoinTxt.text = AppData.numDifferentiation(StockOfferNew.gold);
        sLSO_ActualPriceTxt.text = "$ " + StockOfferNew.actualprice;
        //sLSO_PriceTxt.text = "$ " + StockOfferNew.price;
        sLSO_PriceTxt.text = (CentralPurchase.StockOfferProduct == null) ? "$ " + CentralPurchase.StockOfferNew.price : CentralPurchase.StockOfferProduct.metadata.localizedPriceString;
    }

    public TextMeshProUGUI timerNotiText = null;

    private void Update()
    {
        if (AppData.remaining_LTOTime > 0)
        {
            AppData.remaining_LTOTime -= Time.deltaTime;
            lO_timerTxt.text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);
            sLTO_TimerTxt.text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);

            if (timerNotiText)
                timerNotiText.text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);
        }
    }

    public void SetLimitedTimerOfferDialogData()
    {
        if (TimerOffeNew == null) return;

        lO_timerTxt.text = AppData.GetTimeInFormateHr((long)AppData.remaining_LTOTime * 1000);

        lO_ChestCoinTxt.text = AppData.numDifferentiation(CentralPurchase.TimerOffeNew.normal_gold);
        lO_CoinsCoinTxt.text = AppData.numDifferentiation(CentralPurchase.TimerOffeNew.promo_gold);
        lO_ExtraSaveTxt.text = "+ " + CentralPurchase.TimerOffeNew.isextra + "% EXTRA";
        lO_DiscTxt.text = CentralPurchase.TimerOffeNew.txt;
        lO_CoinVal.text = AppData.numDifferentiation(CentralPurchase.TimerOffeNew.gold);
        lO_ActualPriceVal.text = "$ " + CentralPurchase.TimerOffeNew.actualprice;
        lO_Price.text = CentralPurchase.TimerOfferProduct == null ? ("$ " + CentralPurchase.TimerOffeNew.price) : CentralPurchase.TimerOfferProduct.metadata.localizedPriceString;
        CommanAnimations.instance.PopUpAnimation(lO_Panel, lO_Bg, lO_PopUp, Vector3.one, true);
    }

    public void SetLimitedStockOfferDialogData()
    {
        if (StockOfferNew == null) return;

        lSO_StockCntTxt.text = AppData.numDifferentiation(CentralPurchase.StockOfferNew.stock - StockOfferNew.usestock);
        lSO_BoxCoinTxt.text = AppData.numDifferentiation(CentralPurchase.StockOfferNew.normal_gold);
        lSO_BagCoinTxt.text = AppData.numDifferentiation(CentralPurchase.StockOfferNew.promo_gold);
        lSO_ExtraOffTxt.text = "+ " + CentralPurchase.StockOfferNew.isextra + "% EXTRA";
        lSO_DisTxt.text = CentralPurchase.StockOfferNew.txt;
        lSO_TotalCoinTxt.text = AppData.numDifferentiation(CentralPurchase.StockOfferNew.gold);
        lSO_ActualPriceTxt.text = "$ " + CentralPurchase.StockOfferNew.actualprice;
        lSO_PriceTxt.text = CentralPurchase.StockOfferProduct == null ? "$ " + (CentralPurchase.StockOfferNew.price) : CentralPurchase.StockOfferProduct.metadata.localizedPriceString;
        CommanAnimations.instance.PopUpAnimation(lSO_Panel, lSO_Bg, lSO_PopUp, Vector3.one, true);
    }

    public void ShowRandomOfferDialog(int clamp)
    {
        int randomIndex = UnityEngine.Random.Range(0, 10); // Generates a random number between 0 and 2
        Logger.Print($"|| chance randomIndex = {randomIndex} || {clamp}");
        switch (randomIndex)
        {
            case 0:
                SetLimitedTimerOfferDialogData();
                break;
            case 1:
                SetLimitedStockOfferDialogData();
                break;
        }
    }
}
