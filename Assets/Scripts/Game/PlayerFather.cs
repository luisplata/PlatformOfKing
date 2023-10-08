using System;
using System.Collections;
using UnityEngine;

public class PlayerFather : MonoBehaviour
{
    public InputFacade inputFacade;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DeadZone"))
        {
            //Game Over
            RestartGame();
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            StartCoroutine(CorroutineToRestartGame());
        }
    }

    private IEnumerator CorroutineToRestartGame()
    {
        yield return new WaitForSeconds(1f);
        RestartGame();
    }

    private static void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}