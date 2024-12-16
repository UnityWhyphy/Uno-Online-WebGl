using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class InitialiseService : MonoBehaviour
{
    private string TAG = ">>ServiceInit ";
    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception e)
        {
            Logger.Print(TAG + " Service not initialise " + e.Message);
        }
    }
}
