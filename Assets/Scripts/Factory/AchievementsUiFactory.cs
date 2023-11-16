using UnityEngine;

public class AchievementsUiFactory
{
    private readonly AchievementsConfiguration powerUpsConfiguration;
    private readonly AchievementUi _prefabAchievement;

    public AchievementsUiFactory(AchievementsConfiguration powerUpsConfiguration, AchievementUi prefabAchievement)
    {
        this.powerUpsConfiguration = powerUpsConfiguration;
        _prefabAchievement = prefabAchievement;
    }
        
    public AchievementUi Create(string id)
    {
        var prefab = powerUpsConfiguration.GetAchievementPrefabById(id);

        var prefabAfter = Object.Instantiate(_prefabAchievement);
        prefabAfter.Configure(prefab.GetImage(), prefab.GetDescription());
        return prefabAfter;
    }
}