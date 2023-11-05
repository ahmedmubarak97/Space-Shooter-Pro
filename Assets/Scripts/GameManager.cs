using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;
    public static bool isGamePaused;

    [SerializeField] private GameObject _pauseMenuUI; 

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
            ReloadGame();
        if (!_isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
                PauseGame();
            else
                ResumeGame();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); //Main menu
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1); //Current game scene
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
