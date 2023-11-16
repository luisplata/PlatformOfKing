using UnityEngine;

internal class SecretZone: MonoBehaviour
{
    [SerializeField] private SecretZoneHide secretZoneHide;
    [SerializeField] private string tagPlayer;
    
    public void ShowSecretZone(PlayerFather playerIncoming)
    {
        if(playerIncoming.NameOfPlayer == tagPlayer)
        {
            secretZoneHide.gameObject.SetActive(true);
        }
    }
}