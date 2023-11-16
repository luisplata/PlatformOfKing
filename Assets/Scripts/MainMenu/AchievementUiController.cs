using UnityEngine;

namespace MainMenu
{
    // set order of execution
    [DefaultExecutionOrder(1100)]
    public class AchievementUiController : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        private void Start()
        {
            ServiceLocator.Instance.GetService<IAchievementSystem>().GetAchievements(achievementList =>
            {
                foreach (var element in achievementList)
                {
                    Debug.Log($"element.Id: {element.achievementId}");
                    var achievementInstantiate = ServiceLocator.Instance.GetService<IAchievementUiFactory>().Create(element.achievementId);
                    Transform transform1;
                    (transform1 = achievementInstantiate.transform).SetParent(content.transform);
                    transform1.localScale = Vector3.one;
                }
            });
        }
    }
}