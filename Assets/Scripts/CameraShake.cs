using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shakeTime = 0.2f;
    private float _camTimer = 0;
    private Vector3 _initialPos;

    void Start()
    {
        _initialPos = transform.position;
    }

    public IEnumerator CameraShakeCoroutine()
    {
        while (_camTimer < _shakeTime)
        {
            float shakeMagnitude = 0.5f;
            float x = Random.Range(-0.25f, 0.25f) * shakeMagnitude;
            float y = Random.Range(-0.25f, 0.25f) * shakeMagnitude;

            transform.position = new Vector3(x, y, _initialPos.z);
            _camTimer += Time.deltaTime;

            yield return null;
        }

        transform.position = _initialPos;
        _camTimer = 0;
    }
}
