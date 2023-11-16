using Cinemachine;
using UnityEngine;

public class RulesOfGame : MonoBehaviour
{
    [SerializeField] private FactoryOfPlayers factoryOfPlayers;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private GameObject _player;

    private void Start()
    {
        //read from player prefs the selected character
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        //create the player
        _player = factoryOfPlayers.CreatePlayer(selectedCharacterIndex);
        cinemachineVirtualCamera.Follow = _player.transform;
        cinemachineVirtualCamera.LookAt = _player.transform;
        ServiceLocator.Instance.GetService<ICoinUiSystem>().ShowCoins();
    }
}