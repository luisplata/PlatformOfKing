using UnityEngine;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private AchievementElement achievementElement;
    [SerializeField] private PlayerId playerId;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.gameObject.GetComponent<PlayerFather>().PlayerId.Id == playerId.Id)
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