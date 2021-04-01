using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //private float _spawnTime;
    //[SerializeField] private float _spawnRate = 5f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    private bool _isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //_spawnTime = Time.time + _spawnRate;
        StartCoroutine(SpawnRoutine());
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

    IEnumerator SpawnRoutine()
    {
        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(5.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}

