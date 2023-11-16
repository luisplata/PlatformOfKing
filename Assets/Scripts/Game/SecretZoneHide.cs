using System;
using UnityEngine;

internal class SecretZoneHide : MonoBehaviour
{
    [SerializeField] private GameObject[] elementsToShow;

    private void Start()
    {
        gameObject.SetActive(false);
        foreach (var element in elementsToShow)
        {
            element.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ShowSecretZone();
    }

    public void ShowSecretZone()
    {
        foreach (var element in elementsToShow)
        {
            element.SetActive(true);
        }
    }
}