using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Sprite[] sprites;

    private AudioSource _audioSource;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = gameObject.GetComponent<AudioSource>();

        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL.");
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int playerLives)
    {
        _livesDisplay.sprite = sprites[playerLives];
        if (playerLives == 0)
        {
            _audioSource.Play();
            GameOver();
        }
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (_livesDisplay.sprite != null)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
