using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseRemoteConfigService
{

    public ConfigValue GetValue(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
    }

    public FirebaseRemoteConfigService()
    {

        Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            //{ AdsConfig.CappingTimeRemoteConfigKey, AdsConfig.CappingTimeDefaultValue },
            { AdsConfig.InterBackupRemoteConfigKey, AdsConfig.InterBackupDefaultValue }
        };

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

        FetchDataAsync();
    }

    #region Fetch
    private void FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync()
            .ContinueWithOnMainThread(task =>
            {
                // TODO Load remote config o day
                //AdManager.Instance.SetShowInterBackupFromFirebase();
            });
    }
    #endregion
}