using System.Collections;
using TMPro;
using UnityEngine;

//set order of execution
[DefaultExecutionOrder(1020)]
public class CoinsUiSystem : MonoBehaviour, ICoinUiSystem
{
    [SerializeField] private TextMeshProUGUI textCoins;
    [SerializeField] private GameObject content;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService<ICoinUiSystem>(this);
    }

    void Start()
    {
        content.SetActive(false);
    }
    
    public void ShowCoins()
    {
        StartCoroutine(ShowCoinsCoroutine());
    }

    private IEnumerator ShowCoinsCoroutine()
    {
        ServiceLocator.Instance.GetService<IPlayFabSystem>().AddCoins(0);
        while (!ServiceLocator.Instance.GetService<IPlayFabSystem>().IsAvailable())
        {
            Debug.Log($"Waiting for PlayFab {ServiceLocator.Instance.GetService<IPlayFabSystem>().IsAvailable()}");
            yield return new WaitForSeconds(1);
            Debug.Log($"Waiting after for PlayFab {ServiceLocator.Instance.GetService<IPlayFabSystem>().IsAvailable()}");
        }
        yield return new WaitForSeconds(1);
        content.SetActive(true);
    }

    public void UpdateCoins(int coins)
    {
        textCoins.text = $"Coins: {coins}";
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.RemoveService<ICoinUiSystem>();
    }
}