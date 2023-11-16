using UnityEngine;

[CreateAssetMenu(fileName = "AchievementElement", menuName = "AchievementElement")]
public class AchievementElement : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string text;

    public Sprite GetImage()
    {
        return sprite;
    }

    public string GetDescription()
    {
        return text;
    }
}