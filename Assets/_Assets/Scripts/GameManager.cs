using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.retryGame += RestartGame;
    }
    private void OnDisable()
    {
        EventSystem.retryGame -= RestartGame;
    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
