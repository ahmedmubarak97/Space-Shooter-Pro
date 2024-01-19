using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    private Player _player;
    
    private float _speed = 2.0f;

    private float _lowerYLimit = -7.0f;
    private float _maxY = 7.0f;

    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private Animator _anim;
    private AudioSource _audioSource;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
            Debug.LogError("Player is NULL");
        if (_anim == null)
            Debug.LogError("Animator is NULL");
        if (_audioSource == null)
            Debug.LogError("Audio Source is NULL");
    }

    void Update()
    {
        CalculateMovement();

        StartCoroutine(AggressiveEnemyStartRoutine());

        if(Time.time > _canFire)
        {
          //  if (transform.position.y >= _player.transform.position.y)
          //  {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                StartCoroutine(EnemyLaserFireDelay());
       // }
        //else
        //{
        //    _fireRate = Random.Range(3f, 7f);
        //    _canFire = Time.time + _fireRate;
        //    StartCoroutine(EnemyLaserBackwardsFireDelay());
        //}
           }
    }

    IEnumerator EnemyLaserFireDelay()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
            lasers[i].AssignEnemyLaser();
    }


    //void FireBackwards()
    //{

    //}
    //IEnumerator EnemyLaserBackwardsFireDelay()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

    //    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
    //    for (int i = 0; i < lasers.Length; i++)
    //        lasers[i].MoveUp();
    //}

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _lowerYLimit)
            transform.position = new Vector3(Random.Range(-7.5f, 7.5f), _maxY, 0);

        //if(transform.position.y < _player.transform.position.y)
        //{

        //}
    }

    //void DodgePlayerLaser()
    //{
    //    //IF LASER IS WITHIN RANGE, MOVETOWARDS LASER BUT WITH NEGATIVE MULTIPLE X TIME.DELTATIME
    //}
    
    void RamPlayer()
    {
        if (transform.position.y - _player.transform.position.y < 3 && transform.position.x - _player.transform.position.x < 3)
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 7 * Time.deltaTime);
    }

    IEnumerator AggressiveEnemyStartRoutine()
    {
        yield return new WaitForSeconds(5);
        RamPlayer();
        yield return new WaitForSeconds(7);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Laser") || collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("SuperLaser"))
        {
            if(collision.gameObject.tag.Equals("Player"))
                collision.GetComponent<Player>().Damage();

            else if (collision.gameObject.tag.Equals("Laser"))
            {
                if (collision.gameObject.GetComponent<Laser>().IsEnemyLaser())
                    return;
                else  // if(!collision.gameObject.GetComponent<Laser>().IsEnemyLaser() || transform.position.y >= _player.transform.position.y)
                {
                    Destroy(collision.gameObject);
                    if (_player != null)
                        _player.IncreaseScore(10);
                }
            }

            else
                if (_player != null)
                    _player.IncreaseScore(10);

            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(gameObject, 2f);
        }
    }
}
