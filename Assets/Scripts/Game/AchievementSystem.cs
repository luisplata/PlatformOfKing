using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour, IAchievementSystem
{
    private bool _canSubscribe;
    private readonly List<AchievementElement> _achievementElements = new();
    private void Start()
    {
        if (FindObjectsOfType<AchievementSystem>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        ServiceLocator.Instance.RegisterService<IAchievementSystem>(this);
        _canSubscribe = true;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (!_canSubscribe) return;
        ServiceLocator.Instance.RemoveService<IAchievementSystem>();
    }

    public void AchievementUnlocked(AchievementElement achievementElement)
    {
        if (_achievementElements.Contains(achievementElement)) return;
        _achievementElements.Add(achievementElement);
    }

    public List<AchievementElement> GetAchievements()
    {
        return _achievementElements;
    }
}