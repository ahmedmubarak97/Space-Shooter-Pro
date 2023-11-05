using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
//using UnityEditorInternal;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 4.5f;
    [SerializeField] private int _score;

    private UIManager _uiManager;

    private float _leftXLimit = -10.5f;
    private float _rightXLimit = 10.5f;
    private float _topYLimit = 0;
    private float _bottomYLimit = -4.75f;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private int _lives = 3;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _superLaser;

    [SerializeField] private GameObject _playerShield, _playerShieldDamaged1, _playerShieldDamaged2;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserAudioClip;    

    private bool _tripleShotActive;
    private bool _shieldActive;
    private bool _superLaserActive;

    private int _shieldLives;

    [SerializeField] private float _rateOfFire = 0.2f;
    private float _canFire = -1f;
    [SerializeField] private int _ammo = 15;
    private bool _hasAmmo;

    [SerializeField] private GameObject _rightEngine, _leftEngine;

    [SerializeField] private CameraShake _camShakeScript;

    [SerializeField] private Slider _thrusterSlider;

    void Start()
    {
        transform.position = Vector3.zero;
        _tripleShotActive = false;
        _shieldActive = false;
        _superLaserActive = false;
        _hasAmmo = true;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _thrusterSlider.value = _thrusterSlider.maxValue;

        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is NULL.");
        if (_uiManager == null)
            Debug.LogError("UI Manager is NULL.");
        if (_audioSource == null)
            Debug.LogError("Audio Source on Player is NULL.");
    }

    void Update()
    {
        MovePlayer();

        ManualSpeedBoost();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _hasAmmo)
            FireWeapon();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && !_hasAmmo)
            _uiManager.NoAmmoPrompt();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _bottomYLimit, _topYLimit), 0);

        if (transform.position.x >= _rightXLimit)
            transform.position = new Vector3(_leftXLimit, transform.position.y, 0);
        else if (transform.position.x <= _leftXLimit)
            transform.position = new Vector3(_rightXLimit, transform.position.y, 0);
    }

    void ManualSpeedBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(_speed == 4.5f)
                _speed *= 1.5f;

            _thrusterSlider.value -= 0.5f * Time.deltaTime;

            if (_thrusterSlider.value == 0)
                _speed /= 1.5f;
        }
        else
        {
            if (_speed != 4.5f)
                _speed /= 1.5f;

            if (_thrusterSlider.value == 0)
                StartCoroutine(ManualSpeedBoostCooldownRoutine());
            else
                _thrusterSlider.value += 0.5f * Time.deltaTime;
        }
    }

    IEnumerator ManualSpeedBoostCooldownRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        _thrusterSlider.value += 0.5f * Time.deltaTime;
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void FireWeapon()
    {
        _canFire = Time.time + _rateOfFire;
        if (!_tripleShotActive && !_superLaserActive)
        {
            Vector3 laserSpawn = new Vector3(transform.position.x, transform.position.y + 1, 0);
            Instantiate(_laserPrefab, laserSpawn, Quaternion.identity);

            _ammo--;
            if (_ammo <= 0)
            {
                _hasAmmo = false;
                _uiManager.UpdateAmmo(0);
            }
            else
                _uiManager.UpdateAmmo(_ammo);
        }

        else if (_tripleShotActive && !_superLaserActive)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        else if (_superLaserActive)
        {
            _superLaser.SetActive(true);
        }

        _audioSource.clip = _laserAudioClip; 
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldLives--;
            _playerShield.SetActive(false);

            if (_shieldLives == 2)
                _playerShieldDamaged1.SetActive(true);
            else if (_shieldLives == 1)
            {
                _playerShieldDamaged1.SetActive(false);
                _playerShieldDamaged2.SetActive(true);
            }
            else
            {
                _shieldActive = false;
                _playerShieldDamaged1.SetActive(false);
                _playerShieldDamaged2.SetActive(false);
            }
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
                _leftEngine.SetActive(true);
            else if(_lives == 1)
                _rightEngine.SetActive(true);
            else 
            {
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
            }
        }

        StartCoroutine(_camShakeScript.CameraShakeCoroutine());
    }

    //public void EnemyLaserDamage()
    //{
    //    if (_shieldActive)
    //    {
    //        _shieldLives--;
    //        _playerShield.SetActive(false);

    //        if (_shieldLives == 2)
    //            _playerShieldDamaged1.SetActive(true);
    //        else if (_shieldLives == 1)
    //        {
    //            _playerShieldDamaged1.SetActive(false);
    //            _playerShieldDamaged2.SetActive(true);
    //        }
    //        else
    //        {
    //            _shieldActive = false;
    //            _playerShieldDamaged1.SetActive(false);
    //            _playerShieldDamaged2.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        float lives = _lives;
    //        lives -= 0.5f;
    //        _uiManager.UpdateLives(_lives);

    //        if (lives == 2)
    //            _leftEngine.SetActive(true);
    //        else if (lives == 1)
    //            _rightEngine.SetActive(true);
    //        else if(lives == 0)
    //        {
    //            _spawnManager.OnPlayerDeath();
    //            Destroy(gameObject);
    //        }
    //    }

    //}

    public void AddLife()
    {
        _lives++;
        if (gameObject != null)
        {
            if (_lives == 2)
            {
                _uiManager.UpdateLives(2);
                _rightEngine.SetActive(false);
            }
            else if (_lives == 3)
            {
                _uiManager.UpdateLives(3);
                _leftEngine.SetActive(false);
            }
            else if (_lives > 3)
                _lives = 3;
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        if (_speed == 4.5)
            _speed *= 3;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _shieldLives = 3;
        _playerShieldDamaged1.SetActive(false);
        _playerShieldDamaged2.SetActive(false);
        _shieldActive = true;
        _playerShield.SetActive(true);
    }

    public void SuperLaserActive()
    {
        _superLaserActive = true;
        StartCoroutine(SuperLaserPowerDownRoutine());
    }

    public void AmmoRefill()
    {
        _ammo = 15;
        _hasAmmo = true;
        _uiManager.ResetAmmo();
    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(7f);
        if (_speed != 4.5)
            _speed /= 3;

    }

    IEnumerator SuperLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _superLaserActive = false;
        _superLaser.SetActive(false);
    }

}
