
using System.Collections.Generic;


[System.Serializable]
public class StoreData
{
    public string _id;
    public string inapp;
    public string packname;
    public string isextra;
    public string actualprice;
    public double price;
    public long normal_gold;
    public long promo_gold;
    public int gold;
    public List<Card1> card2;
    public int free_gold;
    public int free_gems;
    public int isoffer;
    public int bundle;
    public int isadsremove;
    public int status;
    public string offerimg;
    public string dashoffer;
    public string pp;
    public string title;
    public string desc;
    public int vpoint;
    public int isdayoffer;
    public int isextraoffer;
    public int iscombo;
    public int isother;
    public string txt;
    public int gems;
    public int notes;
    public int usestock;
    public int stock;
    public long timediff;
    public string offer;
}
[System.Serializable]
public class Card1
{
    public string name;
    public int count;
}
[System.Serializable]
public class SloteModel
{
    public int min;
    public int max;
    public int randomNum;
    public int lastTime;
    public List<StoreData> offerPack;
}
[System.Serializable]
public class Offerdata
{
    public string inapp;
    public string _id;
    public double price;
    public int gold;
    public int cancel_gold;
}
[System.Serializable]
public class StockTimerData
{
    public List<StockTimeOffer> StockTimeOffe;
}
[System.Serializable]
public class StockTimeOffer
{
    public string _id;
    public string pp;
    public string hmsg;
    public int stock;
    public int usestock;
    public int status;
    public string offer;
    public int isextra;
    public Offerdata offerdata;
    public string inapp;
    public string pid;
    public double price;
    public double actualprice;
    public int normal_gold;
    public int promo_gold;
    public int gold;
    public int cancel_gold;
    public int usergoldtype;
    public string txt;
    public int cOffe;
    public List<object> uid;
    public string usertype;
    public int offercode;
    public int isupdate;
    public string desc;
    public string title;
    public int timediff;

}

[System.Serializable]
public class FbProduct
{
    public string description;
    public string imageURI;
    public string price;
    public double priceAmount;
    public string priceCurrencyCode;
    public string productID;
    public string title;
}

[System.Serializable]
public class FGSstoreData
{
    public string _id;
    public string inapp;
    public string packname;
    public double price;
    public int gems;
    public int status;
    public int isadsremove;
    public int gold;
    public int isoffer;
    public string txt;
    public int iscombo;


    public int free_gold;
    public int free_gems;
    public int bundle;
    public string offerimg;
    public int isdayoffer;
    public double from;
    public double to;
    public int isextraoffer;
    public int isother;
}
[System.Serializable]
public class Goldstore
{
    public string _id;
    public string inapp;
    public string packname;
    public double price;
    public int gold;
    public int free_gold;
    public int free_gems;
    public int isoffer;
    public int bundle;
    public int isadsremove;
    public int status;
    public string offerimg;
    public int isdayoffer;
    public int from;
    public double to;
    public int isextraoffer;
    public int iscombo;
    public int isother;
    public string txt;
}

