using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class FBDataRef : MonoBehaviour
{

    public void GetDataCallBack(string result)
    {

        //string rs = result[0];
        //
        //List<Winner> PlayerWinner = JsonConvert.DeserializeObject<List<Winner>>(data["data"].ToString());

        Logger.Print($"-======================================================result From HTML: {result}");

        string[] splitData = result.Split('=');

        PrefrenceManager.FID = splitData[1];
        PrefrenceManager.PN = splitData[0];
        PrefrenceManager.ULT = "FB";
        PrefrenceManager.PP = splitData[2];
        PrefrenceManager.DET = "ios";

        Logger.Print($"-================================FID: {PrefrenceManager.FID} |PN: {PrefrenceManager.PN}  |" +
            $"ULT {PrefrenceManager.ULT} | DET: {PrefrenceManager.DET}  ");

        //EventHandler.SendSignUp();
    }
}
