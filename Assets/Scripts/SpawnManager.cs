using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerups;

    [SerializeField] private GameObject _player;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void OnPlayerDeath()
    {
        Destroy(gameObject);
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.75f);
        
        while (_player != null)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-7.5f, 7.5f), 7.0f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.75f);

        while (_player != null)
        {
            int randomPowerupIndex = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[5], new Vector3(Random.Range(-7.5f, 7.5f), 7.0f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

}
