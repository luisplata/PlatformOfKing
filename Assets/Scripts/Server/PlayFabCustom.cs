using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabCustom : IPlayFabCustom
{
    
        private SystemInfoCustom _systemInfoCustom;
        private IsCreated _isCreatedPlayer;
        private string _playerId;

        public PlayFabCustom()
        {
            Login(OnLoginSuccess, OnLoginFailure);
        }

        private void Login(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback)
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "42";
            }
            Debug.Log($"SystemInfo.deviceUniqueIdentifier {SystemInfo.deviceUniqueIdentifier}");
            var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, resultCallback, errorCallback);
        }

        private void CreatedPlayer()
        {
            var request = new GetTitleDataRequest()
            {
                Keys = new List<string>(){"InitialUserData"}
            };
            PlayFabClientAPI.GetTitleData(request, (defaultData) =>
            {
                var initialUserData = JsonUtility.FromJson<InitialUserData>(defaultData.Data["InitialUserData"]);
                PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"isCreated",JsonUtility.ToJson(new IsCreated(){isCreated = true})},
                        {"SystemInfo",JsonUtility.ToJson(new SystemInfoCustom()
                        {
                            model = SystemInfo.deviceModel,
                            name = SystemInfo.deviceName,
                            os = SystemInfo.operatingSystem,
                            processor = SystemInfo.processorType,
                            graphicsDeviceName = SystemInfo.graphicsDeviceName
                        })}
                    }
                }, requestCreate =>
                {
                    AddUserVirtualCurrencyRequest reqCurrenci = new AddUserVirtualCurrencyRequest()
                    {
                        Amount = 0,
                        VirtualCurrency = "10"
                    };
                    PlayFabClientAPI.AddUserVirtualCurrency(reqCurrenci, result =>{},OnLoginFailure);
                },OnLoginFailure);
            },OnLoginFailure);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your first API call.  :(");
            Debug.LogError("Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }

        private void OnLoginSuccess(LoginResult result)
        {
            GetUserDataRequest requestCreated = new GetUserDataRequest(){Keys = new List<string>(){"isCreated","SystemInfo"}};
            _playerId = result.PlayFabId;
            PlayFabClientAPI.GetUserData(requestCreated, defaultResult =>
            {
                if (!defaultResult.Data.ContainsKey("isCreated"))
                {
                    CreatedPlayer();
                }
                else
                {
                    _isCreatedPlayer = JsonUtility.FromJson<IsCreated>(defaultResult.Data["isCreated"].Value);
                    _systemInfoCustom = JsonUtility.FromJson<SystemInfoCustom>(defaultResult.Data["SystemInfo"].Value);
                }
            },OnLoginFailure);
        }

        public void AddCoins(int coinValue)
        {
            // adding currency into server
            Debug.Log($"AddCoins {coinValue}");
        }
}

[Serializable]
public class InitialUserData
{
    public string playerId;
}
[Serializable]
public class IsCreated
{
    public bool isCreated;
}
[Serializable]
public class SystemInfoCustom
{
    public string model;
    public string name;
    public string os;
    public string processor;
    public string graphicsDeviceName;
}