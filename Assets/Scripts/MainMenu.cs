using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    //public void LoadPauseMenu()
    //{
    //    SceneManager.LoadScene("PauseMenu");
    //}

    public void EndGame()
    {
        Application.Quit();
    }
}
