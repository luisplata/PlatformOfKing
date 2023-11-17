using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabCustom : IPlayFabCustom
{
    private readonly IPlayFabSystem _playFabSystem;
    private readonly string _currentId;

    private SystemInfoCustom _systemInfoCustom;
    private IsCreated _isCreatedPlayer;
    private string _playerId;
    private bool _isAvailable;
    private AchievementElementCustom _achievementUnlocked;

    public bool IsAvailable => _isAvailable;

    public PlayFabCustom(IPlayFabSystem playFabSystem, string currentId)
    {
        _playFabSystem = playFabSystem;
        _currentId = currentId;
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
        string iduniq;
#if UNITY_WEBGL && !UNITY_EDITOR
        var lastId = LocalStorageExternal.GetLocalStorageValue("iduniq");
        if (!string.IsNullOrEmpty(lastId))
        {
            iduniq = lastId;
        }
        else
        {
            iduniq = LocalStorageExternal.GenerateUniqueID();
            LocalStorageExternal.SetLocalStorageValue("iduniq", iduniq);
        }
#else
        iduniq = SystemInfo.deviceUniqueIdentifier;
#endif
        Debug.Log($"SystemInfo.deviceUniqueIdentifier {iduniq}");
        var request = new LoginWithCustomIDRequest { CustomId = iduniq, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, resultCallback, errorCallback);
    }

    private void CreatedPlayer()
    {
        var request = new GetTitleDataRequest()
        {
            Keys = new List<string> {"InitialUserData"}
        };
        PlayFabClientAPI.GetTitleData(request, (defaultData) =>
        {
            var initialUserData = JsonUtility.FromJson<InitialUserData>(defaultData.Data["InitialUserData"]);
            string nameDevice;
#if UNITY_WEBGL && !UNITY_EDITOR
            nameDevice = "WebGL";
#else
            nameDevice = SystemInfo.deviceName;
#endif
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"isCreated",JsonUtility.ToJson(new IsCreated {isCreated = true})},
                    {"SystemInfo",JsonUtility.ToJson(new SystemInfoCustom
                    {
                        model = SystemInfo.deviceModel,
                        name = nameDevice,
                        os = SystemInfo.operatingSystem,
                        processor = SystemInfo.processorType,
                        graphicsDeviceName = SystemInfo.graphicsDeviceName
                    })},
                    {"AchievementUnlocked","{}"}
                }
            }, requestCreate =>
            {
                AddCurrency(initialUserData.coins);
            },OnLoginFailure);
        },OnLoginFailure);
    }

    private void AddCurrency(int amount)
    {
        var reqCurrency = new AddUserVirtualCurrencyRequest
        {
            Amount = amount,
            VirtualCurrency = _currentId
        };
        PlayFabClientAPI.AddUserVirtualCurrency(reqCurrency, result =>
        {
            Debug.Log($"AddUserVirtualCurrency Balance: {result.Balance} - BalanceChange: {result.BalanceChange} - VirtualCurrency: {result.VirtualCurrency}");
            ServiceLocator.Instance.GetService<ICoinUiSystem>().UpdateCoins(result.Balance);
            _isAvailable = true;
        }, OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult result)
    {
        GetUserDataRequest requestCreated = new GetUserDataRequest {Keys = new List<string> {"isCreated", "SystemInfo", "AchievementUnlocked"}};
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
                _achievementUnlocked = JsonUtility.FromJson<AchievementElementCustom>(defaultResult.Data["AchievementUnlocked"].Value);
                AddCurrency(0);
            }
        },OnLoginFailure);
    }

    public void AddCoins(int coinValue)
    {
        // adding currency into server
        AddCurrency(coinValue);
    }
    
    public void RemoveCurrency(int coinValue)
    {
        var reqCurrency = new SubtractUserVirtualCurrencyRequest
        {
            Amount = coinValue,
            VirtualCurrency = _currentId
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(reqCurrency, result =>
        {
            Debug.Log($"SubtractUserVirtualCurrency Balance: {result.Balance} - BalanceChange: {result.BalanceChange} - VirtualCurrency: {result.VirtualCurrency}");
            ServiceLocator.Instance.GetService<ICoinUiSystem>().UpdateCoins(result.Balance);
            _isAvailable = true;
        }, SubtractError);
    }

    private void SubtractError(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }
    
    public void AddAchievement(AchievementElement achievementElement)
    {
        GetAchievements(listOfAchievements =>
        {
            listOfAchievements.Add(new AchievementElementData {achievementId = achievementElement.Id});
            //convert list to json
            var json = JsonUtility.ToJson(new AchievementElementCustom {data = listOfAchievements});
            Debug.Log($"json: {json}");
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"AchievementUnlocked",json}
                }
            }, requestCreate =>
            {
                Debug.Log("AchievementUnlocked updated");
            },OnLoginFailure);            
        });
    }

    public void GetAchievements(Action<List<AchievementElementData>> callback)
    {
        //create await for `_playerId` is not null
        GetAchievementsCoroutine(callback).WrapErrors();
    }

    private async Task GetAchievementsCoroutine(Action<List<AchievementElementData>> callback)
    {
        while (string.IsNullOrEmpty(_playerId))
        {
            await Task.Delay(100);
        }
        GetUserDataRequest requestCreated = new GetUserDataRequest {Keys = new List<string> {"isCreated", "SystemInfo", "AchievementUnlocked"}};
        PlayFabClientAPI.GetUserData(requestCreated, defaultResult =>
        {
            if (!defaultResult.Data.ContainsKey("isCreated"))
            {
                CreatedPlayer();
            }
            else
            {
                Debug.Log($"AchievementUnlocked: {defaultResult.Data["AchievementUnlocked"].Value}");
                _achievementUnlocked = JsonUtility.FromJson<AchievementElementCustom>(defaultResult.Data["AchievementUnlocked"].Value);
                Debug.Log($"AchievementUnlocked: {_achievementUnlocked.data.Count}");
                var achievementList = _achievementUnlocked.data.ToList();
                callback?.Invoke(achievementList);
            }
        },OnLoginFailure);
    }
}

[Serializable]
public class AchievementElementCustom
{
    public List<AchievementElementData> data;
}

[Serializable]
public class AchievementElementData
{
    public string achievementId;
}

[Serializable]
public class InitialUserData
{
    public string playerId;
    public int coins;
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