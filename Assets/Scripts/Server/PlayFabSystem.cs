using System;
using UnityEngine;

public class PlayFabSystem : MonoBehaviour, IPlayFabSystem
{
    [SerializeField] private string currentId;
    private PlayFabCustom _playFabCustom;
    private void Start()
    {
        if(FindObjectsOfType<PlayFabSystem>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        _playFabCustom = new PlayFabCustom(this, currentId);
        ServiceLocator.Instance.RegisterService<IPlayFabSystem>(this);
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.RemoveService<IPlayFabCustom>();
    }

    public void AddCoins(int coinValue)
    {
        _playFabCustom.AddCoins(coinValue);
    }
}