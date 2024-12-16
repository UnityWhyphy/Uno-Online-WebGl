using System.Collections.Generic;


[System.Serializable]
public class Data
{
    public List<object> StockTimeOffer;
    public List<SlotDatum> SlotData;
    public Welcomeoffer welcomeoffer;
    public Starterbundle starterbundle;
    public Middlebundle middlebundle;
    public Probundle probundle;
}

[System.Serializable]
public class Middlebundle
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
    public int iscombo;
    public int isextraoffer;
    public int isother;
    public double actualprice;
    public string offer;
    public int stock;
    public int usestock;
    public string txt;
}

[System.Serializable]
public class OfferPack
{
    public string _id;
    public string inapp;
    public string packname;
    public double price;
    public int gold;
    public int free_gold;
    public int free_gems;
    public int isadsremove;
    public string offerimg;
    public int offerdata;
    public string txt;
    public string actualprice;
    public int totalgold;
}

[System.Serializable]
public class Probundle
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
    public int iscombo;
    public int isextraoffer;
    public int isother;
    public double actualprice;
    public string offer;
    public int stock;
    public int usestock;
    public string txt;
}

[System.Serializable]
public class SodDataRes
{
    public bool flag;
    public string msg;
    public string en;
    public Data data;
    public string errcode;
}
[System.Serializable]
public class SlotDatum
{
    public int min;
    public int max;
    public int randomNum;
    public int lastTime;
    public List<OfferPack> offerPack;
}
[System.Serializable]
public class Starterbundle
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
    public int iscombo;
    public int isextraoffer;
    public int isother;
    public double actualprice;
    public string offer;
    public int stock;
    public int usestock;
    public string txt;
}

[System.Serializable]
public class Welcomeoffer
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
    public string txt;
    public double actualprice;
    public int isextraoffer;
    public int iscombo;
    public int isother;
}

[System.Serializable]
public class OfferData
{
    public string _id;
    public string inapp;
    public string packname;
    public double price;
    public long gold;
    public long gems;
    public long free_gold;
    public long free_gems;
    public long isoffer;
    public int bundle;
    public int isadsremove;
    public int status;
    public string offerimg;
    public string txt;
    public double actualprice;
    public int isextraoffer;
    public int iscombo;
    public int isother;
    public string offer;
    public int stock;
    public int usestock;
    public int offerdata;
    public long totalgold;
    public long totalgems;
}

[System.Serializable]
public class SlotOffer
{
    public int min;
    public long max;
    public int randomNum;
    public List<OfferData> offerPack;
}


