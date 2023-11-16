using UnityEngine;
[CreateAssetMenu(fileName = "PlayerId", menuName = "PlayerId")]
public class PlayerId : ScriptableObject
{
    [SerializeField] private string id;
    public string Id => id;
}