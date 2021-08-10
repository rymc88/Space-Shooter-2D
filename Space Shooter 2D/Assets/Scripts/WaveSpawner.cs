using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemy;
        //public GameObject enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0; //store the index of the wave that will be created next

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0f;
    public string currentWaveName;

    private float searchCountdown = 1.0f;

    private SpawnState state = SpawnState.Counting;

    [SerializeField] private UIManager _uiManager;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        currentWaveName = waves[nextWave].name;
        //_uiManager = GetComponent<UIManager>();

    }

    private void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if(EnemyIsAlive() == false)
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            _uiManager.UpdateWaveCountdown(waveCountdown);
            _uiManager.WaveNameDisplay(currentWaveName);

            if(waveCountdown < 0)
            {
                _uiManager._waveCountdownText.enabled = false;
                _uiManager._waveNameText.enabled = false;
            }
            
        }

    }

    void WaveCompleted()
    {

        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;
        _uiManager._waveCountdownText.enabled = true;
        _uiManager._waveNameText.enabled = true;

        if (nextWave + 1 > waves.Length - 1) //Check to see if you are on the final wave and make sure it doesn't go over
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE!");
        }
        else
        {
            nextWave++;
            currentWaveName = waves[nextWave].name;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) //if(GameObject.FindGameObjectsWithTag("Enemy").Length==0)
            {
                return false;
            }

        }

        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.Spawning;

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy[i]);
            yield return new WaitForSeconds(_wave.rate);
        }

        state = SpawnState.Waiting;

        yield break; 
    }

    void SpawnEnemy (GameObject _enemy)
    {
        Debug.Log("Spawn Enemy: " + _enemy.name);

        Transform _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _spawnPoint.position, _spawnPoint.rotation);
    }
}
