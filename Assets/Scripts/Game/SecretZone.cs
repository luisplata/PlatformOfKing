using UnityEngine;

internal class SecretZone: MonoBehaviour
{
    [SerializeField] private SecretZoneHide secretZoneHide;
    
    public void ShowSecretZone()
    {
        secretZoneHide.gameObject.SetActive(true);
    }
}