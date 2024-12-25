using System;
using UnityEngine;

public static class Logger
{
    public static string TAG = ">>UNO ";
    static bool isLog = false;

    public static void Print(string msg)
    {
        if (!isLog)        
            return;
        
        Debug.Log($"{TAG} +  {msg}");
    }

    public static void Error(string msg)
    {
        if (!isLog)
            return;

        Debug.LogError($"{TAG} + {msg}");
    }

    public static void SendLog(string msg)
    {
        if (!isLog)
            return;

        Debug.Log($"{TAG} + <color=red><b> {msg} </b></color>");
    }

    public static void RecevideLog(string msg)
    {
        if (!isLog)
            return;

#if UNITY_EDITOR
        Debug.Log($"{TAG} + <color=yellow><b> {msg} </b></color>");
#else
        Debug.Log($"{TAG} + <color=green><b> {msg} </b></color>");
#endif
    }

    public static void NormalLog(string msg)
    {
        if (!isLog)
            return;
        Debug.Log($"{TAG} + <color=#FF69B4><b>{msg}</b></color>");
    }

    internal static void Print(object p)
    {
        throw new NotImplementedException();
    }
}
