using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using SimpleJSON;

public static class StaticData
{
    public static System.Action cardEnable, cardDisable, allCardDown;


    const string ULT_ID = "ULTID";
    const string SIGNUP_DATA_ID = "SIGNUP_DATA";
    const string GTI_DATA_ID = "GTI_DATA_ID";
    const string CONFIG_DATA_ID = "CONFIG_DATA_ID";
    const string LEVELUP = "LEVELUP";
    const string RGTI_DATA_ID = "RGTI_DATA_ID";
    const string LWU_DATA = "LWU_DATA";


    public static int Test;

    //user login test - karel 6 ke nahi jo login karel hoy to direct dashboard nakar login panel.
    public static string ULT
    {
        get => PlayerPrefs.GetString(ULT_ID);
        set => PlayerPrefs.SetString(ULT_ID, value);
    }

    public static string CONFIG_DATA
    {
        get => PlayerPrefs.GetString(CONFIG_DATA_ID);
        set => PlayerPrefs.SetString(CONFIG_DATA_ID, value);
    }

    public static string LEVELUPDATA
    {
        get => PlayerPrefs.GetString(LEVELUP);
        set => PlayerPrefs.SetString(LEVELUP, value);
    }

    public static string GetTimeInFormateHr(long time)
    {
        long second = time / 1000;
        long sec = second % 60;
        string m = getinNumFormat((second / 60) % 60);
        string h = getinNumFormat(second / 3600);
        return h + ":" + m + ":" + getinNumFormat(sec);
    }

    public static string GetTimeInFormateMin(long time)
    {
        long second = time / 1000;
        long sec = second % 60;
        string m = getinNumFormat(second / 60);
        return m + ":" + getinNumFormat(sec);
    }

    private static string getinNumFormat(long num)
    {
        if (num < 10)
        {
            return "0" + num;
        }
        return "" + num;
    }

    public static string getTimeInHourNew(long time)
    {
        string h = getinNumFormat(time / 3600);
        return h.Equals("00") ? "" : h + "H";
    }

    public static string LWU_DATAS
    {
        get => PlayerPrefs.GetString(LWU_DATA);
        set => PlayerPrefs.SetString(LWU_DATA, value);
    }
}
