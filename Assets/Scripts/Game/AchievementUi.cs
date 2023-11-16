using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUi : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    
    public void Configure(Sprite sprite, string text)
    {
        image.sprite = sprite;
        this.text.text = text;
    }
}
