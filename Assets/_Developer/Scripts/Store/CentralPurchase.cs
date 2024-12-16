using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class CentralPurchase : MonoBehaviour, IDetailedStoreListener
{
    private string TAG = " >> CENTRAL >>> ";
    public static CentralPurchase instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SocketManagergame.OnListener_SOD += HandleSOD;
    }

    private void OnDisable()
    {
        SocketManagergame.OnListener_SOD -= HandleSOD;
    }

    //slot Offer
    public static List<SlotOffer> SlotOfferPack = new List<SlotOffer>();
    public List<Product> SlotProduct = new List<Product>();
    List<string> InAppSlot = new List<string>();

    public static List<SlotOffer> SlotOfferPackForGems = new List<SlotOffer>();
    public List<Product> SlotProductGems = new List<Product>();
    List<string> InAppSlotGems = new List<string>();

    //bundle
    public static List<StockTimeOffer> StockTimerForStock = new List<StockTimeOffer>();
    public static StockTimeOffer TimerOffeNew, StockOfferNew;

    public static OfferData FirstTimeOffer, StarterBundlePack, MiddelBundle, Probundle;
    public static Product FirstTimeProduct, StarterBundleProduct, MidBundleProduct, ProBundleProduct, TimerOfferProduct, StockOfferProduct;

    [Obsolete]
    private void HandleSOD(JSONNode data)
    {
        Logger.Print(TAG + " Handle SOD called || " + data.HasKey("welcomeoffer"));
        AppData.remaining_LTOTime = 0;

        // Gold 
        SlotOfferPack.Clear();
        SlotProduct.Clear();
        InAppSlot.Clear();

        // Gems 
        SlotOfferPackForGems.Clear();
        SlotProductGems.Clear();
        InAppSlotGems.Clear();

        // Stock & Timer 
        StockTimerForStock.Clear();
        TimerOffeNew = null;
        StockOfferNew = null;
        TimerOfferProduct = null;
        StockOfferProduct = null;

        // Welcome, Starter, Middle, Pro
        FirstTimeOffer = null;
        StarterBundlePack = null;
        MiddelBundle = null;
        Probundle = null;

        FirstTimeProduct = null;
        StarterBundleProduct = null;
        MidBundleProduct = null;
        ProBundleProduct = null;

        //slot offer pack
        SlotOfferPack = JsonConvert.DeserializeObject<List<SlotOffer>>(data["SlotData"].ToString());
        SlotOfferPackForGems = JsonConvert.DeserializeObject<List<SlotOffer>>(data["SlotDataGems"].ToString());

        //timer stock
        StockTimerForStock = JsonConvert.DeserializeObject<List<StockTimeOffer>>(data["StockTimeOffer"].ToString());

        if (data.HasKey("starterbundle") && data["starterbundle"].Count != 0)
            StarterBundlePack = JsonConvert.DeserializeObject<OfferData>(data["starterbundle"].ToString());

        if (data.HasKey("middlebundle") && data["middlebundle"].Count != 0)
            MiddelBundle = JsonConvert.DeserializeObject<OfferData>(data["middlebundle"].ToString());

        if (data.HasKey("probundle") && data["probundle"].Count != 0)
            Probundle = JsonConvert.DeserializeObject<OfferData>(data["probundle"].ToString());

        if (data.HasKey("welcomeoffer") && data["welcomeoffer"].Count != 0)
        {
            FirstTimeOffer = JsonConvert.DeserializeObject<OfferData>(data["welcomeoffer"].ToString());
            Logger.Print(TAG + " Handle SOD called || " + FirstTimeOffer.inapp);
        }

        Logger.Print(TAG + " StockTimerForStock count " + StockTimerForStock.Count);
        for (int i = 0; i < StockTimerForStock.Count; i++)
        {
            switch (StockTimerForStock[i].offer)
            {
                case "Timer":
                    TimerOffeNew = StockTimerForStock[i];
                    AppData.remaining_LTOTime = TimerOffeNew.timediff;
                    break;

                case "Stock":
                    StockOfferNew = StockTimerForStock[i];
                    break;
            }
        }

        if (controller == null)
        {
            Logger.Print(TAG + " Fresh SOD mali");
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            if (FirstTimeOffer != null)
                builder.AddProduct(FirstTimeOffer.inapp, ProductType.Consumable);

            if (StarterBundlePack != null)
                builder.AddProduct(StarterBundlePack.inapp, ProductType.Consumable);

            if (MiddelBundle != null)
                builder.AddProduct(MiddelBundle.inapp, ProductType.Consumable);

            if (Probundle != null)
                builder.AddProduct(Probundle.inapp, ProductType.Consumable);

            for (int i = 0; i < StockTimerForStock.Count; i++)
            {
                builder.AddProduct(StockTimerForStock[i].inapp, ProductType.Consumable);
            }

            for (int i = 0; i < SlotOfferPack.Count; i++)
            {
                for (int j = 0; j < SlotOfferPack[i].offerPack.Count; j++)
                {
                    InAppSlot.Add(SlotOfferPack[i].offerPack[j].inapp);
                    builder.AddProduct(SlotOfferPack[i].offerPack[j].inapp, ProductType.Consumable);
                }
            }
            for (int i = 0; i < SlotOfferPackForGems.Count; i++)
            {
                for (int j = 0; j < SlotOfferPackForGems[i].offerPack.Count; j++)
                {
                    InAppSlotGems.Add(SlotOfferPackForGems[i].offerPack[j].inapp);
                    builder.AddProduct(SlotOfferPackForGems[i].offerPack[j].inapp, ProductType.Consumable);
                }
            }

            UnityPurchasing.Initialize(this, builder);
        }

        else
        {
            var additional = new HashSet<ProductDefinition>();

            if (FirstTimeOffer != null)
                additional.Add(new ProductDefinition(FirstTimeOffer.inapp, ProductType.Consumable));

            if (StarterBundlePack != null)
                additional.Add(new ProductDefinition(StarterBundlePack.inapp, ProductType.Consumable));

            if (MiddelBundle != null)
                additional.Add(new ProductDefinition(MiddelBundle.inapp, ProductType.Consumable));

            if (Probundle != null)
                additional.Add(new ProductDefinition(Probundle.inapp, ProductType.Consumable));

            for (int i = 0; i < StockTimerForStock.Count; i++)
            {
                additional.Add(new ProductDefinition(StockTimerForStock[i].inapp, ProductType.Consumable));
            }

            for (int i = 0; i < SlotOfferPack.Count; i++)
            {
                for (int j = 0; j < SlotOfferPack[i].offerPack.Count; j++)
                {
                    InAppSlot.Add(SlotOfferPack[i].offerPack[j].inapp);
                    additional.Add(new ProductDefinition(SlotOfferPack[i].offerPack[j].inapp, ProductType.Consumable));
                }
            }
            for (int i = 0; i < SlotOfferPackForGems.Count; i++)
            {
                for (int j = 0; j < SlotOfferPackForGems[i].offerPack.Count; j++)
                {
                    InAppSlotGems.Add(SlotOfferPackForGems[i].offerPack[j].inapp);
                    additional.Add(new ProductDefinition(SlotOfferPackForGems[i].offerPack[j].inapp, ProductType.Consumable));
                }
            }

            Action onSuccess = () =>
            {
                Logger.Print(TAG + " Fetch Successfully SOD Again");

                foreach (var product in controller.products.all)
                {
                    if (FirstTimeOffer != null && product.definition.storeSpecificId.Equals(FirstTimeOffer.inapp))                    
                        FirstTimeProduct = product;
                    
                    else if (StarterBundlePack != null && product.definition.storeSpecificId.Equals(StarterBundlePack.inapp))                    
                        StarterBundleProduct = product;
                    
                    else if (MiddelBundle != null && product.definition.storeSpecificId.Equals(MiddelBundle.inapp))                    
                        MidBundleProduct = product;
                    
                    else if (Probundle != null && product.definition.storeSpecificId.Equals(Probundle.inapp))                    
                        ProBundleProduct = product;                    
                }

                for (int i = 0; i < InAppSlot.Count; i++)
                {
                    foreach (var product in controller.products.all)
                    {
                        if (product.definition.storeSpecificId.Equals(InAppSlot[i]))
                        {
                            Logger.Print(TAG + " in-App " + InAppSlot[i] + " SKU " + product.definition.storeSpecificId + " i " + i);
                            SlotProduct.Add(product);
                            break;
                        }
                    }
                }

                for (int i = 0; i < InAppSlotGems.Count; i++)
                {
                    foreach (var product in controller.products.all)
                    {
                        if (product.definition.storeSpecificId.Equals(InAppSlotGems[i]))
                        {
                            Logger.Print(TAG + " in-App " + InAppSlotGems[i] + " SKU " + product.definition.storeSpecificId + " i " + i);
                            SlotProductGems.Add(product);
                            break;
                        }
                    }
                }

                foreach (var product in controller.products.all)
                {
                    for (int i = 0; i < StockTimerForStock.Count; i++)
                    {
                        if (product.definition.storeSpecificId.Equals(StockTimerForStock[i].inapp))
                        {
                            switch (StockTimerForStock[i].offer)
                            {
                                case "Timer":
                                    TimerOfferProduct = product;
                                    break;

                                case "Stock":
                                    StockOfferProduct = product;
                                    break;
                            }
                            break;
                        }
                    }
                }

                DashboardManager.instance.DashOfferButtonStatus(FirstTimeOffer != null);
            };

            Action<InitializationFailureReason> onFailure = (InitializationFailureReason i) =>
            {
                Logger.Print(TAG + " Fetching failed in Again SOD: " + i);
            };

            controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
        }
    }

    public IStoreController controller;
    public IExtensionProvider provider;

    private bool isInitialized = false;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Logger.Print(TAG + " OnInitialized called");

        this.controller = controller;
        provider = extensions;

        foreach (var product in controller.products.all)
        {
            Logger.Print(TAG + " Product ID " + product.definition.storeSpecificId);
            if (FirstTimeOffer != null && product.definition.storeSpecificId.Equals(FirstTimeOffer.inapp))            
                FirstTimeProduct = product;
            
            else if (StarterBundlePack != null && product.definition.storeSpecificId.Equals(StarterBundlePack.inapp))            
                StarterBundleProduct = product;
            
            else if (MiddelBundle != null && product.definition.storeSpecificId.Equals(MiddelBundle.inapp))            
                MidBundleProduct = product;
            
            else if (Probundle != null && product.definition.storeSpecificId.Equals(Probundle.inapp))            
                ProBundleProduct = product;            
        }

        foreach (var product in controller.products.all)
        {
            for (int i = 0; i < StockTimerForStock.Count; i++)
            {
                if (product.definition.storeSpecificId.Equals(StockTimerForStock[i].inapp))
                {
                    switch (StockTimerForStock[i].offer)
                    {
                        case "Timer":
                            TimerOfferProduct = product;
                            break;

                        case "Stock":
                            StockOfferProduct = product;
                            break;
                    }
                    break;
                }
            }
        }

        for (int i = 0; i < InAppSlot.Count; i++)
        {
            foreach (var product in controller.products.all)
            {
                if (product.definition.storeSpecificId.Equals(InAppSlot[i]))
                {                    
                    SlotProduct.Add(product);
                    break;
                }
            }
        }

        for (int i = 0; i < InAppSlotGems.Count; i++)
        {
            foreach (var product in controller.products.all)
            {
                if (product.definition.storeSpecificId.Equals(InAppSlotGems[i]))
                {
                    SlotProductGems.Add(product);
                    break;
                }
            }
        }

        if (controller == null || controller.products == null || controller.products.all == null)
        {
            Logger.Error(TAG + " Initialization failed: controller or products are null.");
            isInitialized = false;
            return;
        }

        isInitialized = true;
        Logger.RecevideLog(TAG + " isInitialized  Sucsessfully Done : "+ isInitialized);
        DashboardManager.instance.DashOfferButtonStatus(FirstTimeOffer != null);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Logger.Print(TAG + " OnInitializeFailed called-1");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Logger.Print(TAG + " OnInitializeFailed called-2");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Logger.Print(TAG + " OnPurchaseFailed called-1");
        PurchaseFailed();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Logger.Print(TAG + " OnPurchaseFailed called-2");
        PurchaseFailed();
    }

    private void PurchaseFailed()
    {
        Loading_screen.instance.ShowLoadingScreen(false);

        //if (StorePanel.Instance.storePanel.gameObject.activeInHierarchy)
        //{
        //    if (StorePanel.Instance.calledEO % 2 == 0 && !StorePanel.Instance.specialDealsPanel.gameObject.activeInHierarchy && StorePanel.Instance.type == 0)
        //    {
        //        Loading_screen.instance.ShowLoadingScreen(true);
        //        EventHandler.SendExitOffer();
        //    }
        //    else
        //        AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.alert, StorePanel.Instance.msg2);

        //    if (StorePanel.Instance.type == 0)
        //        StorePanel.Instance.calledEO++;
        //}
        //else
            AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.alert, StorePanel.Instance.msg2);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Logger.Print(TAG + " PurchaseProcessingResult called ");

        Logger.Print(TAG + $" Attempted purchase details:");
        Logger.Print(TAG + $" Product ID: {e.purchasedProduct.definition.storeSpecificId}"); // Update property name if needed
        Logger.Print(TAG + $" Receipt: {e.purchasedProduct.receipt}");

        var productName = e.purchasedProduct.definition?.storeSpecificId;
        Logger.Print(TAG + $" Product Name: {productName ?? "Unknown"}");

        try
        {
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
            AppleTangle.Data(), Application.identifier);

            var result = validator.Validate(e.purchasedProduct.receipt);

            foreach (IPurchaseReceipt productReceipt in result)
            {
                Logger.Print(TAG + " Ketli Var Loop Far " + productReceipt.productID);

                GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                if (null != google)
                {
                    var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(e.purchasedProduct.receipt);
                    if (wrapper == null)
                        Logger.Print(TAG + " Google Play receipt wrapper is null!");

                    else
                    {
                        string payload = (string)wrapper["Payload"];
                        var receiptDetails = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        Logger.Print(TAG + " Purchase Signature >> " + receiptDetails["signature"].ToString());
                        string receiptSignature = receiptDetails["signature"].ToString();
                        string receiptJson = (string)receiptDetails["json"];
                        PrefrenceManager._purchaseNodeData = "";
                        JSONNode data = new JSONObject
                        {
                            ["packid"] = AppData.PURCHASEDID,
                            ["inapp"] = productReceipt.productID,
                            ["receiptData"] = receiptJson,
                            ["orderId"] = google.transactionID,
                            ["receiptSignature"] = receiptSignature,
                            ["isextra"] = AppData.IsExtra,
                            ["notiid"] = AppData.NOTIiDS,
                            ["uid"] = PrefrenceManager._ID,
                            ["itemName"] = ""
                        };

                        if (productReceipt.productID.Contains("gems"))
                        {
                            data["itemName"] = "gems";
                            PrefrenceManager._purchaseNodeData = data.ToString();
                            //EventHandler.HandlePaymentGems(AppData.PURCHASEDID, productReceipt.productID,
                            //   receiptJson, receiptSignature, google.transactionID, AppData.IsExtra, AppData.NOTIiDS);
                            //EventHandler.HandlePaymentGems(PrefrenceManager._purchaseNodeData);
                            EventHandler.HandlePaymentGems(data);
                        }
                        else
                        {
                            data["itemName"] = "golds";
                            PrefrenceManager._purchaseNodeData = data.ToString();
                            EventHandler.HandlePaymentGold(data);
                        }
                        break;
                    }
                }

                AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                if (null != apple)
                {
                    Loading_screen.instance.ShowLoadingScreen(true, "Purchase is Processing...");
                    Logger.Print($"TAG + OnPurchaseClicked called PURCHASEDID = {AppData.PURCHASEDID}");
                    Logger.Print($"TAG + OnPurchaseClicked called productID = {productReceipt.productID} ");
                    Logger.Print($"TAG + OnPurchaseClicked called Payload = {ApplePurchase.FromJson(e.purchasedProduct.receipt).Payload}");
                    Logger.Print($"TAG + OnPurchaseClicked called transactionID = {apple.transactionID} || AppData.IsExtra {AppData.IsExtra}");
                    Logger.Print($"TAG + OnPurchaseClicked called AppData.NOTIiDS = { AppData.NOTIiDS}");

                    PrefrenceManager._purchaseNodeData = "";
                    JSONNode data = new JSONObject
                    {
                        ["packid"] = AppData.PURCHASEDID,
                        ["inapp"] = productReceipt.productID,
                        ["receiptData"] = ApplePurchase.FromJson(e.purchasedProduct.receipt).Payload,
                        ["orderId"] = apple.transactionID,
                        ["receiptSignature"] = new JSONObject(),
                        ["isextra"] = AppData.IsExtra,
                        ["notiid"] = AppData.NOTIiDS,
                        ["uid"] = PrefrenceManager._ID,
                        ["itemName"] = ""
                    };

                    if (productReceipt.productID.Contains("gems"))
                    {
                        data["itemName"] = "gems";
                        //EventHandler.HandlePaymentGems(AppData.PURCHASEDID, productReceipt.productID,
                        //    ApplePurchase.FromJson(e.purchasedProduct.receipt).Payload, new JSONObject(), apple.transactionID, AppData.IsExtra, AppData.NOTIiDS);
                        //EventHandler.HandlePaymentGems(PrefrenceManager._purchaseNodeData);
                        EventHandler.HandlePaymentGems(data);
                    }
                    else
                    {
                        data["itemName"] = "golds";
                        //EventHandler.HandlePaymentGold(AppData.PURCHASEDID, productReceipt.productID,
                        //  ApplePurchase.FromJson(e.purchasedProduct.receipt).Payload, new JSONObject(), apple.transactionID, AppData.IsExtra, AppData.NOTIiDS);
                        EventHandler.HandlePaymentGold(data);
                    }
                }
                else
                    AllCommonGameDialog.instance.SetJustOkDialogData(MessageClass.alert, StorePanel.Instance.msg1);
            }

        }
        catch (Exception ex)
        {

            JSONNode objects = new JSONObject
            {
                ["PURCHASEDID"] = AppData.PURCHASEDID,
                ["IsExtra"] = AppData.IsExtra,
                ["NOTIiDS"] = AppData.NOTIiDS,
                ["productName"] = productName,
            };
            Loading_screen.instance.SendExe("CentralPurchase", "ProcessPurchase", $"{objects}", ex);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseClicked(string productId)
    {
        Logger.Print(TAG + " OnPurchaseClicked called " + productId + " check null " + (controller == null));
        /*LogAvailableProducts();*/
        try
        {
            var product = controller.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                controller.InitiatePurchase(productId);
            }
            else
            {
                Logger.Print("The product is not available for purchase.");
            }
        }
        catch (NullReferenceException e)
        {
            // Display an error message to the user if the IAP system returns null
            Logger.Print("Failed to purchase small gem: " + e.Message);
        }
    }

}
