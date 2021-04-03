using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //private float _spawnTime;
    //[SerializeField] private float _spawnRate = 5f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject [] _powerUps;
    private bool _isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //_spawnTime = Time.time + _spawnRate;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        /*if(_spawnTime < Time.time)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7, 0);
            Instantiate(_enemyPrefab,spawnPosition, Quaternion.identity);
            _spawnTime = Time.time + _spawnRate;
        }*/
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(5.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], spawnPosition, Quaternion.identity);
        }
    }
        

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}

