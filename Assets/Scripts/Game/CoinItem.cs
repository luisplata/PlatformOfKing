using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private int coinValue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.Instance.GetService<IPlayFabSystem>().AddCoins(coinValue);
            Destroy(gameObject,0.1f);
        }
    }
}
