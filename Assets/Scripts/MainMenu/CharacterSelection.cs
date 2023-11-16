using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<Button> characterButtons;
    [SerializeField] private Button playButton, continueButton;
    [SerializeField] private GameObject panelOfCharacterSelection, panelOfMessage, panelToContainMessage;
    private int selectedCharacterIndex = -1;
    private bool _isPlaying, _continueToSelecction;

    private TeaTime selecctionOfCharacterTeaTime, messageToPlayerTeaTime;
    private bool _isStartGame;

    private void Awake()
    {
        panelOfCharacterSelection.SetActive(false);
        playButton.gameObject.SetActive(false);
        panelToContainMessage.SetActive(false);
    }

    private void Start()
    {
        characterButtons.ForEach(button => button.onClick.AddListener(() => SelectCharacter(characterButtons.IndexOf(button))));
        continueButton.onClick.AddListener(() =>
        {
            _continueToSelecction = true;
        });
        playButton.onClick.AddListener(Play);
        selecctionOfCharacterTeaTime = this.tt().Pause().Add(() =>
        {
            panelOfCharacterSelection.SetActive(true);
            playButton.gameObject.SetActive(true);
        }).Wait(()=>_isPlaying).Add(()=>
        {
            PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        });
        messageToPlayerTeaTime = this.tt().Pause().Add(() =>
        {
            panelToContainMessage.SetActive(true);
            //using dotween to shake the panel
            panelOfMessage.transform.DOShakePosition(100f, 5f);
        }).Wait(() => _continueToSelecction).Add(() =>
        {
            panelToContainMessage.SetActive(false);
            selecctionOfCharacterTeaTime.Play();
        });
        
        messageToPlayerTeaTime.Play();
    }

    private void Play()
    {
        if (selectedCharacterIndex != -1)
        {
            _isPlaying = true;
        }
    }

    private void SelectCharacter(int indexOf)
    {
        if (selectedCharacterIndex != -1)
        {
            characterButtons[selectedCharacterIndex].GetComponent<Image>().color = new Color(1, 1, 1, 0.3921569f);
        }

        selectedCharacterIndex = indexOf;
        characterButtons[selectedCharacterIndex].GetComponent<Image>().color = Color.green;
    }
}