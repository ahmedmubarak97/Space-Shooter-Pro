using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
            ReloadGame();
        //if (!_isGameOver && Input.GetKeyDown(KeyCode.Escape))
        //    PauseGame();
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }
    public void GameOver()
    {
        _isGameOver = true;
    }

    void PauseGame()
    {
        SceneManager.LoadScene(2); //Pause menu
    }

    void ReloadGame()
    {
        SceneManager.LoadScene(1); //Current game scene
    }
}
