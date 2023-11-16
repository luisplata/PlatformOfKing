using System;
using System.Collections.Generic;

public interface IPlayFabSystem
{
    void AddCoins(int coinValue);
    bool IsAvailable();
    void GetAchievements(Action<List<AchievementElementData>> callback);
    void AddAchievement(AchievementElement achievementElement);
}