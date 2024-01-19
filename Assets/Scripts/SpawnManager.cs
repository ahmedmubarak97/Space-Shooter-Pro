using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerups;

    [SerializeField] private GameObject _player;

    private int _wave;
    private int _enemyCount;
  
    private List<GameObject> _pickupsInScene;

    private void Start()
    {
        _wave = 1;
    }

    private void Update()
    {
        if(_wave > 3)
            StopAllCoroutines();
    }

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
        int tempEnemyCount = 0;
        _enemyCount = _wave * 5;

        while (_player != null && tempEnemyCount < _enemyCount)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-7.5f, 7.5f), 7.0f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3f);
            tempEnemyCount++;
        }

        _wave++;

        yield return new WaitForSeconds(5f);

        StartSpawning();
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.75f);

        while (_player != null)
        {
            int randomPowerupIndex = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[randomPowerupIndex], new Vector3(Random.Range(-7.5f, 7.5f), 7.0f, 0), Quaternion.identity);

            _pickupsInScene.Add(_powerups[randomPowerupIndex]);
            
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public List<GameObject> PickupsInScene()
    {
        return _pickupsInScene;
    }

}
