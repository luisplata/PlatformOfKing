using UnityEngine;

public class PlayFabSystem : MonoBehaviour, IPlayFabSystem
{
    private void Start()
    {
        ServiceLocator.Instance.RegisterService<IPlayFabCustom>(new PlayFabCustom());
        if(FindObjectsOfType<PlayFabSystem>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}