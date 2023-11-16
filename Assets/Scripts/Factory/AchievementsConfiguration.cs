using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Achievement configuration")]
public class AchievementsConfiguration : ScriptableObject
{
    [SerializeField] private AchievementElement[] achievements;
    private Dictionary<string, AchievementElement> idToAchievement;

    private void Awake()
    {
        idToAchievement = new Dictionary<string, AchievementElement>(achievements.Length);
        foreach (var achievementElement in achievements)
        {
            idToAchievement.Add(achievementElement.Id, achievementElement);
        }
    }

    public AchievementElement GetAchievementPrefabById(string id)
    {
        if (!idToAchievement.TryGetValue(id, out var achievementElement))
        {
            throw new Exception($"Achievement with id {id} does not exit");
        }
        return achievementElement;
    }
}