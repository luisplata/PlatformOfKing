using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//set order of execution
[DefaultExecutionOrder(1010)]
public class AchievementSystem : MonoBehaviour, IAchievementSystem
{
    private bool _canSubscribe;
    private List<AchievementElementData> _achievementElements = new();
    private void Awake()
    {
        if (FindObjectsOfType<AchievementSystem>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        ServiceLocator.Instance.RegisterService<IAchievementSystem>(this);
        _canSubscribe = true;
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Instance.GetService<IPlayFabSystem>().GetAchievements(list =>
        {
            _achievementElements = list;
        });
    }

    private void OnDestroy()
    {
        if (!_canSubscribe) return;
        ServiceLocator.Instance.RemoveService<IAchievementSystem>();
    }

    public void AchievementUnlocked(AchievementElement achievementElement)
    {
        var ele = new AchievementElementData
        {
            achievementId = achievementElement.Id
        };
        ServiceLocator.Instance.GetService<IPlayFabSystem>().GetAchievements(list =>
        {
            var isExist = list.Any(elementData => elementData.achievementId == ele.achievementId);
            if (!isExist)
            {
                _achievementElements.Add(ele);
                ServiceLocator.Instance.GetService<IPlayFabSystem>().AddAchievement(achievementElement);
            }
        });
    }

    public void GetAchievements(Action<List<AchievementElementData>> callback)
    {
        ServiceLocator.Instance.GetService<IPlayFabSystem>().GetAchievements(callback);
    }
}