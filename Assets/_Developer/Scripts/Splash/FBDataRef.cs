using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;

public class FBDataRef : MonoBehaviour
{
    private string TAG = " >> FBDataRef >>> ";


    public void GetDataCallBack(string result)
    {
        Logger.Print($"-======================================================result From HTML: {result}");
        
        JSONNode data = JSON.Parse(result);

        PrefrenceManager.FID = data["playerId"];
        PrefrenceManager.PN = data["playerName"];
        PrefrenceManager.ULT = "FB";
        PrefrenceManager.PP = data["playerPic"];
        PrefrenceManager.DET = "WEBFB";

        Logger.Print($"-================================FID: {PrefrenceManager.FID} |PN: {PrefrenceManager.PN}  |" +
            $"ULT {PrefrenceManager.ULT} | DET: {PrefrenceManager.DET}  ");
    }

    public void GetFBProductData(string fproduct)
    {
        JSONNode fPro = JSON.Parse(fproduct);
        Logger.Print($"{TAG} | GetFBProductData===> >> {fPro} | count : {fPro.Count}");

        for (int i = 0; i < fPro.Count; i++)
        {
            FbProduct fb = JsonConvert.DeserializeObject<FbProduct>(fPro[i].ToString());
            StorePanel.Instance.fbProduct.Add(fb);

            Logger.Print($"{TAG} | GetFBProductData===> >> {i} | price: {fb.price}");
        }

        Logger.Print($"{TAG} |  StorePanel.Instance.fbProduct===> >> { StorePanel.Instance.fbProduct.Count}");

        foreach (var product in StorePanel.Instance.fbProduct)
        {
            Logger.Print($"Product: {product.title}, Price: {product.priceAmount} {product.priceCurrencyCode}");
        }
    }

    //purchase
    public void GetPurchaseRes(string inAppname)
    {
        Logger.Print($"GetPurchaseRes===> >> {inAppname}");
        JSONNode purchaseJson = JSON.Parse(inAppname);

        // You can log the parsed JSON object to check its structure
        Logger.Print($"Parsed Purchase Data: {purchaseJson.ToString()}");
        AppData.HPGTokan = purchaseJson["purchaseToken"];

        JSONNode data = new JSONObject
        {
            ["packid"] = AppData.PURCHASEDID,
            ["inapp"] = purchaseJson["productID"],
            ["receiptData"] = purchaseJson["signedRequest"],
            ["orderId"] = purchaseJson["paymentID"],
            ["receiptSignature"] = "",
            ["isextra"] = AppData.IsExtra,
            ["notiid"] = AppData.NOTIiDS,
            ["uid"] = PrefrenceManager._ID,
            //["ispromo"] = 0,
            ["purchaseToken"] = AppData.HPGTokan,
            ["itemName"] = ""
        };

        data["itemName"] = "golds";
        PrefrenceManager._purchaseNodeData = data.ToString();
        EventHandler.HandlePaymentGold(data);

    }

}
[System.Serializable]
public class ProductListWrapper
{
    public List<FbProduct> products;
}