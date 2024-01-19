using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private GameObject _player;
    
    [SerializeField] private float _speed = 3f;

    private float _lowerYLimit = -7.0f;

    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _powerupAudioClip;

    private void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if (transform.position.y < _lowerYLimit)
            Destroy(gameObject);
    }

    public void MoveToPlayer()
    {
        //if(transform.position.y - _player.transform.position.y < 3 && transform.position.x - _player.transform.position.x < 3)
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 3 * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player") && col.GetComponent<Player>() != null)
        {
            AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);
            switch (_powerupID)
            {
                case 0:
                    col.GetComponent<Player>().TripleShotActive(); break;
                case 1:
                    col.GetComponent<Player>().SpeedBoostActive(); break;
                case 2:
                    col.GetComponent<Player>().ShieldActive(); break;
                case 3:
                    col.GetComponent<Player>().AmmoRefill(); break;
                case 4:
                    col.GetComponent<Player>().AddLife(); break;
                case 5: 
                    col.GetComponent<Player>().SuperLaserActive(); break;
                case 6:
                    col.GetComponent<Player>().DepleteAmmo(); break;
                default: break;
            }
            Destroy(gameObject);
        }
    }
}
