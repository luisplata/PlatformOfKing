using System;
using UnityEngine;

// set order of execution
[DefaultExecutionOrder(1200)]
public class UiFactoryMono : MonoBehaviour, IAchievementUiFactory
{ 
    [SerializeField] private AchievementsConfiguration achievementsConfiguration;
    [SerializeField] private AchievementUi prefabAchievement;
    private AchievementsUiFactory _achievementsUiFactory;
    private bool _canSubscribe;

    private void Start()
    {
        if(FindObjectsOfType<UiFactoryMono>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        ServiceLocator.Instance.RegisterService<IAchievementUiFactory>(this);
        DontDestroyOnLoad(gameObject);
        _achievementsUiFactory = new AchievementsUiFactory(Instantiate(achievementsConfiguration), prefabAchievement);
        _canSubscribe = true;
    }

    private void OnDestroy()
    {
        if (!_canSubscribe) return;
        ServiceLocator.Instance.RemoveService<IAchievementUiFactory>();
    }

    public AchievementUi Create(string id)
    {
        return _achievementsUiFactory.Create(id);
    }
}