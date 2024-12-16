using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;
using SimpleJSON;

public class OfferPopupController : CentralPurchase
{
    public static Action<bool, long, long> offerSlotCall;
    public static Action offerBannerSlotCall;

    private string TAG2 = " >> OfferPopupController >>> ";
    [SerializeField] SlotOfferInfo slot_PopUp;

    [Header("SLotBundleBanner")]
    [Space(5)]
    [SerializeField] GameObject slot_Banner_Panel;
    [SerializeField] SlotOfferInfo slot_Banner_PopUp;

    public TextMeshProUGUI b_rewardCoinValueTxt;
    public TextMeshProUGUI b_rewardGemsValueTxt;
    public TextMeshProUGUI b_discountValueTxt;
    public Text b_priceAmountTxt;
    public TextMeshProUGUI b_rewardOfferTxt;

    [Header("SLotBundle")]
    public TextMeshProUGUI rewardCoinValueTxt;
    public TextMeshProUGUI rewardGemsValueTxt;
    public TextMeshProUGUI discountValueTxt;
    public Text priceAmountTxt;
    public TextMeshProUGUI rewardOfferTxt;
    public Button bundleBuyButton;

    public Button buyButton;
    public Button storeGoButton;

    [Header("Slot")]
    public SelfSlot slot1, slot2;

    [Header("Sprites_Banner")]
    [SerializeField] Sprite[] bundleBannerBg;

    [Header("Sprites")]
    [SerializeField] Sprite[] bundleBg;
    [SerializeField] Sprite[] bundleTitleImgs;
    [SerializeField] Sprite[] rewardCoinImgs;
    [SerializeField] Sprite[] dealDeatilsImgs;
    [SerializeField] Sprite[] slotCenterItems;

    private OfferBundle offerBundle;

    public enum OfferBundle
    {
        starterbundle,
        middlebundle,
        probundle
    }

    private void OnEnable()
    {
        offerSlotCall += ShowBundleDialog;
        offerBannerSlotCall += GameStartBannerShow;
    }

    private void OnDisable()
    {
        offerSlotCall -= ShowBundleDialog;
        offerBannerSlotCall -= GameStartBannerShow;
    }

    private void Start()
    {
        storeGoButton.onClick.RemoveAllListeners();
        storeGoButton.onClick.AddListener(() => DashboardManager.instance.TopCenterClick(2));
    }

    private void GameStartBannerShow()
    {
        int no = 0;
        OfferData offerData;
        Product productData;
        Logger.Print($"{TAG2} | Offer Check {long.Parse(PrefrenceManager.GOLD)} || currantLvl = {AppData.currantLvl} || FirstTimeOffer isNULL = {CentralPurchase.FirstTimeOffer == null}");

        if (AppData.currantLvl < 5 || CentralPurchase.FirstTimeOffer != null) return;

        if (slot_Banner_PopUp.mostPopulerTag.activeInHierarchy) slot_Banner_PopUp.mostPopulerTag.gameObject.SetActive(false);
        buyButton.onClick.RemoveAllListeners();
        if (long.Parse(PrefrenceManager.GOLD) > 0 && long.Parse(PrefrenceManager.GOLD) < 20000)
        {
            offerData = CentralPurchase.StarterBundlePack;
            productData = CentralPurchase.StarterBundleProduct;
            no = 0;
            buyButton.onClick.AddListener(() => BundleClick(1));
        }
        else if (long.Parse(PrefrenceManager.GOLD) >= 20000 && long.Parse(PrefrenceManager.GOLD) < 50000)
        {
            offerData = CentralPurchase.MiddelBundle;
            productData = CentralPurchase.MidBundleProduct;
            no = 1;
            buyButton.onClick.AddListener(() => BundleClick(2));
        }
        else if (long.Parse(PrefrenceManager.GOLD) >= 50000 && long.Parse(PrefrenceManager.GOLD) <= 100000)
        {
            offerData = CentralPurchase.Probundle;
            productData = CentralPurchase.ProBundleProduct;
            no = 2;
            buyButton.onClick.AddListener(() => BundleClick(3));
            slot_Banner_PopUp.mostPopulerTag.gameObject.SetActive(true);
        }
        else
        {
            DashboardManager.instance.TryShowSlotAndTimerOffer(20);
            return;
        }
        if (offerData == null) return;


        b_rewardCoinValueTxt.text = AppData.numDifferentiation(offerData.gold);
        b_rewardGemsValueTxt.text = AppData.numDifferentiation(offerData.free_gems);
        b_discountValueTxt.text = "$ " + offerData.actualprice + "";
        b_priceAmountTxt.text = (productData == null) ? "$ " + offerData.actualprice : productData.metadata.localizedPriceString;
        b_rewardOfferTxt.text = offerData.txt;

        slot_Banner_PopUp.bundleBgImg.sprite = bundleBannerBg[no];
        slot_Banner_PopUp.bundleTitleImg.sprite = bundleTitleImgs[no];
        slot_Banner_PopUp.rewardCoinImg.sprite = rewardCoinImgs[no];

        CommanAnimations.instance.PopUpAnimation(slot_Banner_Panel, slot_Banner_Panel.GetComponent<Image>(), slot_Banner_PopUp.transform, Vector3.one, true);
    }

    private void BundleClick(int number)
    {
        Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
        switch (number)
        {
            case 1:
                Logger.Print($"First.inapp :: {CentralPurchase.StarterBundlePack?.inapp}");
                AppData.PURCHASEDID = StarterBundlePack?._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.StarterBundlePack?.inapp);
                break;
            case 2:
                Logger.Print($"Sec.inapp :: {CentralPurchase.MiddelBundle?.inapp}");
                AppData.PURCHASEDID = CentralPurchase.MiddelBundle?._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.MiddelBundle?.inapp);
                break;
            case 3:
                Logger.Print($"Third.inapp :: {CentralPurchase.Probundle?.inapp}");
                AppData.PURCHASEDID = CentralPurchase.Probundle?._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.Probundle?.inapp);
                break;
        }
    }

    private void ShowBundleDialog(bool isGems, long reqGold, long reqGems)
    {
        long gold = reqGold;

        if (!isGems)
        {
            HandleGoldOffers(gold);
        }
        else
        {
            HandleGemsOffers(reqGems);
        }
    }

    int goldSaveIndex = 0;

    private void HandleGoldOffers(long gold)
    {
        goldSaveIndex = 0;
        for (int i = 0; i < CentralPurchase.SlotOfferPack.Count; i++)
        {
            if (gold <= CentralPurchase.SlotOfferPack[i].max)
            {
                Product product = null;
                if (i == 0) product = CentralPurchase.StarterBundleProduct;
                if (i == 1) product = CentralPurchase.MidBundleProduct;
                if (i == 2) product = CentralPurchase.ProBundleProduct;
                SetSlotOfferData(CentralPurchase.SlotOfferPack[i].offerPack[2], CentralPurchase.SlotOfferPack[i].offerPack, false, product);
                goldSaveIndex = i;
                break;
            }
        }
    }

    private void HandleGemsOffers(long reqGems)
    {
        long gems = reqGems;

        if (gems < CentralPurchase.SlotOfferPackForGems[0].max)
        {
            SetSlotOfferData(CentralPurchase.SlotOfferPackForGems[0].offerPack[2], CentralPurchase.SlotOfferPackForGems[0].offerPack, true, CentralPurchase.ProBundleProduct);
            slot1.myButton.onClick.AddListener(() => OnClickSlot(3));
            slot2.myButton.onClick.AddListener(() => OnClickSlot(4));
        }
    }

    internal void SetSlotOfferData(OfferData offerData, List<OfferData> slotOffer, bool isGems, Product bundleProduct)
    {
        if (Enum.TryParse(offerData.inapp, true, out OfferBundle offerBundle))
        {
            Logger.Print($"{TAG2} | SetSlotOfferData {offerData.inapp} | offerBundle = {offerBundle}");
            SetDataInfoImageSprites((int)offerBundle); // Assuming enum values correspond to indices

            SetResData(offerData, slotOffer, isGems, isGems ? CentralPurchase.instance.SlotProductGems : CentralPurchase.instance.SlotProduct, bundleProduct);
        }
        else
        {
            Logger.Print($"{TAG2} | Invalid offer bundle: {offerData.inapp}");
        }
    }

    void SetDataInfoImageSprites(int no)
    {
        slot_PopUp.bundleBgImg.sprite = bundleBg[no];
        slot_PopUp.bundleTitleImg.sprite = bundleTitleImgs[no];
        slot_PopUp.rewardCoinImg.sprite = rewardCoinImgs[no];
        slot_PopUp.dealDeatilsImg.sprite = dealDeatilsImgs[no];
        bundleBuyButton.onClick.RemoveAllListeners();
        bundleBuyButton.onClick.AddListener(() => BundleClick(no + 1));
    }

    private void OnClickSlot(int no)
    {
        Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");

        switch (no)
        {
            case 1:
                Logger.Print($"slotOffer Gold.inapp 1 :: {CentralPurchase.SlotOfferPack[goldSaveIndex].offerPack[0].inapp}");
                AppData.PURCHASEDID = CentralPurchase.SlotOfferPack[goldSaveIndex].offerPack[0]._id;
                CentralPurchase.instance.OnPurchaseClicked(SlotOfferPack[goldSaveIndex].offerPack[0].inapp);
                break;
            case 2:
                Logger.Print($"slotOffer Gold.inapp 2 :: {CentralPurchase.SlotOfferPack[goldSaveIndex].offerPack[1].inapp}");
                AppData.PURCHASEDID = CentralPurchase.SlotOfferPack[goldSaveIndex].offerPack[1]._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.SlotOfferPack[goldSaveIndex].offerPack[1].inapp);
                break;

            // FOR GEMS

            case 3:
                Logger.Print($"slotOffer GEMS.inapp 3 :: {CentralPurchase.SlotOfferPackForGems[0].offerPack[0].inapp}");
                AppData.PURCHASEDID = CentralPurchase.SlotOfferPackForGems[0].offerPack[0]._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.SlotOfferPackForGems[0].offerPack[0].inapp);
                break;
            case 4:
                Logger.Print($"slotOffer GEMS.inapp 4 :: {CentralPurchase.SlotOfferPackForGems[0].offerPack[1].inapp}");
                AppData.PURCHASEDID = CentralPurchase.SlotOfferPackForGems[0].offerPack[1]._id;
                CentralPurchase.instance.OnPurchaseClicked(CentralPurchase.SlotOfferPackForGems[0].offerPack[1].inapp);
                break;

            // FOR Bundle CLICK
            case 5:
                break;
        }
    }

    void SetResData(OfferData offerData, List<OfferData> slotOffer, bool isGems, List<Product> product, Product bundleProduct)
    {
        rewardCoinValueTxt.text = AppData.numDifferentiation(offerData.gold);
        discountValueTxt.text = "$ " + offerData.actualprice + "";
        rewardOfferTxt.text = " " + offerData.txt;
        priceAmountTxt.text = (bundleProduct == null) ? "$ " + offerData.price : bundleProduct.metadata.localizedPriceString;

        try
        {
            // Slot
            slot1.discountAmountTxt.text = "$ " + slotOffer[0].actualprice + "";
            slot1.discounOffertTxt.text = " " + slotOffer[0].txt;
            slot1.priceAmountTxt.text = (product[0] == null) ? "$ " + slotOffer[0].price : product[0].metadata.localizedPriceString;

            slot2.discountAmountTxt.text = "$ " + slotOffer[1].actualprice + "";
            slot2.discounOffertTxt.text = " " + slotOffer[1].txt;
            slot2.priceAmountTxt.text = (product[1] == null) ? "$ " + slotOffer[1].price : product[1].metadata.localizedPriceString;

            if (isGems)
            {
                slot1.rewardCoinAmountTxt.text = AppData.numDifferentiation(slotOffer[0].totalgems);
                slot2.rewardCoinAmountTxt.text = AppData.numDifferentiation(slotOffer[1].totalgems);
                slot1.reward_Slot_Coin.sprite = slotCenterItems[2];
                slot2.reward_Slot_Coin.sprite = slotCenterItems[3];
                slot1._itemImg.sprite = slotCenterItems[5];
                slot2._itemImg.sprite = slotCenterItems[5];
                storeGoButton.onClick.RemoveAllListeners();
                storeGoButton.onClick.AddListener(() => DashboardManager.instance.TopCenterClick(2));
            }
            else
            {
                slot1.rewardCoinAmountTxt.text = AppData.numDifferentiation(slotOffer[0].totalgold);
                slot2.rewardCoinAmountTxt.text = AppData.numDifferentiation(slotOffer[1].totalgold);
                slot1.reward_Slot_Coin.sprite = slotCenterItems[0];
                slot2.reward_Slot_Coin.sprite = slotCenterItems[1];
                slot1._itemImg.sprite = slotCenterItems[4];
                slot2._itemImg.sprite = slotCenterItems[4];
                slot1.myButton.onClick.AddListener(() => OnClickSlot(1));
                slot2.myButton.onClick.AddListener(() => OnClickSlot(2));
                storeGoButton.onClick.RemoveAllListeners();
                storeGoButton.onClick.AddListener(() => DashboardManager.instance.TopCenterClick(1));
            }
        }
        catch (Exception ex)
        {
            JSONNode objects = new JSONObject
            {
                ["slotOfferCount"] = slotOffer.Count,
                ["productCount"] = product.Count,
            };
            Loading_screen.instance.SendExe("OfferPopupController", "SetResData", $"{objects}", ex);
            Logger.Print($"Ex on SetResData : {ex}");
        }
   

        StorePanel.Instance.OnClick_StorePanel(26);
    }

}
