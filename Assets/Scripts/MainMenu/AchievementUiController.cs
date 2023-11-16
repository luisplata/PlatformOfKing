using UnityEngine;

namespace MainMenu
{
    public class AchievementUiController : MonoBehaviour
    {
        [SerializeField] private AchievementUi prefabAchievement;
        [SerializeField] private GameObject content;
        private void Start()
        {
            ServiceLocator.Instance.GetService<IAchievementSystem>().GetAchievements().ForEach(achievement =>
            {
                var achievementItem = Instantiate(prefabAchievement, content.transform);
                achievementItem.Configure(achievement.GetImage(), achievement.GetDescription());
            });
        }
    }
}