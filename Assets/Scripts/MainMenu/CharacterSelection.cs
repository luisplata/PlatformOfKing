using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<Button> characterButtons;
    [SerializeField] private Button playButton;
    private int selectedCharacterIndex = -1;

    private void Start()
    {
        characterButtons.ForEach(button => button.onClick.AddListener(() => SelectCharacter(characterButtons.IndexOf(button))));
        playButton.onClick.AddListener(Play);
    }

    private void Play()
    {
        if (selectedCharacterIndex != -1)
        {
            PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
