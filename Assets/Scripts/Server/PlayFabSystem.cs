using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//set order of execution
[DefaultExecutionOrder(1005)]
public class PlayFabSystem : MonoBehaviour, IPlayFabSystem
{
    [SerializeField] private string currentId;
    private PlayFabCustom _playFabCustom;
    private bool _canSubscribe;
    private void Awake()
    {
        if(FindObjectsOfType<PlayFabSystem>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        _playFabCustom = new PlayFabCustom(this, currentId);
        ServiceLocator.Instance.RegisterService<IPlayFabSystem>(this);
        _canSubscribe = true;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (!_canSubscribe) return;
        ServiceLocator.Instance.RemoveService<IPlayFabSystem>();
    }

    public void AddCoins(int coinValue)
    {
        _playFabCustom.AddCoins(coinValue);
    }

    public bool IsAvailable()
    {
        return _playFabCustom.IsAvailable;
    }

    public void GetAchievements(Action<List<AchievementElementData>> callback)
    {
        StartWentAvailable(callback).WrapErrors();
    }

    private async Task StartWentAvailable(Action<List<AchievementElementData>> callback)
    {
        while (!IsAvailable())
        {
            await Task.Delay(100);
        }
        _playFabCustom.GetAchievements(callback);
    }

    public void AddAchievement(AchievementElement achievementElement)
    {
        _playFabCustom.AddAchievement(achievementElement);
    }
}