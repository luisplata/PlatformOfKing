using UnityEngine;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private AchievementElement achievementElement;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AchievementUnlocked();
            Destroy(gameObject, 1);
        }
    }

    private void AchievementUnlocked()
    {
        ServiceLocator.Instance.GetService<IAchievementSystem>().AchievementUnlocked(achievementElement);
    }
}