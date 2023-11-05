using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    private float _lowerYLimit = -7.0f;

    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _powerupAudioClip;

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if (transform.position.y < _lowerYLimit)
            Destroy(gameObject);
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
                default: break;
            }
            Destroy(gameObject);
        }
    }
}
