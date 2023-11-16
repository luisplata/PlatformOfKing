using UnityEngine;

internal class SecretZone: MonoBehaviour
{
    [SerializeField] private SecretZoneHide secretZoneHide;
    [SerializeField] private PlayerId tagPlayer;
    
    public void ShowSecretZone(PlayerFather playerIncoming)
    {
        if(playerIncoming.PlayerId.Id == tagPlayer.Id)
        {
            secretZoneHide.gameObject.SetActive(true);
        }
    }
}