using System;
using System.Collections.Generic;

public interface IAchievementSystem
{
    void AchievementUnlocked(AchievementElement achievementElement);
    void GetAchievements(Action<List<AchievementElementData>> callback);
}