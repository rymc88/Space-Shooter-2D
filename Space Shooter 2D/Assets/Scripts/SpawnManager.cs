using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Spawner")]
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [Header("Power Spawner")]
    public GameObject[] _powerUps;
    
    private bool _isPlayerDead = false;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        //StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(5.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-9, 9), 7, 0);
            int enemyNumber = Random.Range(0, 7);
            GameObject newEnemy = Instantiate(_enemyPrefab[enemyNumber], spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    /*public void SpawnPowerUp()
    {
       
            foreach (var weight in _percentageChance)
            {
                _total += weight;
            }

            int randomNumber = Random.Range(0, _total);

            for (int i = 0; i < _percentageChance.Length; i++)
            {
                if (randomNumber <= _percentageChance[i])
                {
                    Instantiate(_powerUps[i], _powerUpSpawnPosition, Quaternion.identity);
                    Debug.Log(_powerUps[i]);
                    return;
                }
                else
                {
                    randomNumber -= _percentageChance[i];
                }
            }

    }*/

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}

