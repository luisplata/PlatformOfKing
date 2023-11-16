using System.Collections.Generic;
using UnityEngine;

public class FactoryOfPlayers : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerPrefab;
    [SerializeField] private InputFacade inputFacade;

    public GameObject CreatePlayer(int selectedCharacterIndex)
    {
        var player = Instantiate(playerPrefab[selectedCharacterIndex]);
        player.GetComponent<PlayerFather>().InputFacade = inputFacade;
        return player;
    }
}