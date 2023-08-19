using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText, _noAmmoText;

    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Sprite[] _livesSprites;

    [SerializeField] private Image[] _ammoDisplay;

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
        _livesDisplay.sprite = _livesSprites[playerLives];
        if (playerLives == 0)
        {
            // WILL HAVE AUDIO CLIP FOR 0 AMMO SO ASSIGN EXPLOSION AUDIO CLIP THRU CODE INSTEAD OF HAVING IT IN INSPECTOR DIRECTLY
            _audioSource.Play();
            GameOver();
        }
    }

    public void UpdateAmmo(int playerAmmo)
    {
        _ammoDisplay[playerAmmo].gameObject.SetActive(false);
    }

    public void ResetAmmo()
    {
        foreach (Image img in _ammoDisplay)
            img.gameObject.SetActive(true);

    }

    public void NoAmmoPrompt()
    {
        _noAmmoText.gameObject.SetActive(true);
        StartCoroutine(NoAmmoPromptDeactivate());
    }

    IEnumerator NoAmmoPromptDeactivate()
    {
        yield return new WaitForSeconds(1);
        _noAmmoText.gameObject.SetActive(false);
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
