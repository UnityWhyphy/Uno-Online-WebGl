
using UnityEngine;

public class ApplePurchase
{
    public string Store;
    public string TransactionID;
    public string Payload;

    public static ApplePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<ApplePurchase>(json);
        return purchase;
    }
}
