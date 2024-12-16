using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfferDialog : MonoBehaviour
{
    public static OfferDialog instance;

    private string TAG = ">>OfferDialog ";

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [Header("First Time Offer Dialog")]
    [SerializeField] internal GameObject wO_Panel;
    [SerializeField] internal Image wO_Bg;
    [SerializeField] internal Transform wO_PopUp;
    [SerializeField] internal TextMeshProUGUI wO_OffTxt, wO_CoinsTxt, wO_ActualPriceTxt;
    [SerializeField] internal Text wO_PriceTxt;

    public void SetWelcomeOfferDialogData()
    {
        if (DashboardManager.instance.totalMatch >= 5)
        {
            if (CentralPurchase.FirstTimeOffer == null) return;

            Logger.Print(TAG + $"SetWelcomeOfferDialogData = {CentralPurchase.FirstTimeOffer.txt} | {CentralPurchase.FirstTimeOffer.gold}");
            wO_OffTxt.text = CentralPurchase.FirstTimeOffer.txt + " OFF";
            wO_CoinsTxt.text = AppData.numDifferentiation(CentralPurchase.FirstTimeOffer.gold);
            wO_ActualPriceTxt.text = "$ " + CentralPurchase.FirstTimeOffer.actualprice;
            CommanAnimations.instance.PopUpAnimation(wO_Panel, wO_Bg, wO_PopUp, Vector3.one, true);

        }
        //Logger.Print(TAG + " First Time Product " + CentralPurchase.FirstTimeProduct == null);
    }
}
