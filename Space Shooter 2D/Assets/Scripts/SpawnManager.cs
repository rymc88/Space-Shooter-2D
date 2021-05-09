using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject [] _powerUps;
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
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

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
        yield return new WaitForSeconds(2.0f);

        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 7, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(_powerUps[randomPowerUp], spawnPosition, Quaternion.identity);
        }
    }
        

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}

