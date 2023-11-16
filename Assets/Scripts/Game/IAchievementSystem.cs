using System.Collections.Generic;

public interface IAchievementSystem
{
    void AchievementUnlocked(AchievementElement achievementElement);
    List<AchievementElement> GetAchievements();
}