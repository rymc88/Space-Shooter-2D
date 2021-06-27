using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] [Range(2.0f,15.0f)] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _timeBetweenFire;
    [SerializeField] [Range(0f, 3.0f)] private float _minRandomTimeBetweenFire;
    [SerializeField] [Range(4.0f, 7.0f)] private float _maxRandomTimeBetweenFire;
    private float _lastTimeFire;
    private bool _enemyDead = false;
    private Animator _anim;

    [Header("Power Ups")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioClip _enemyExplosionClip;
    [SerializeField] private AudioSource _audioSource;

    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is Null");
        }
        else
        {
            _audioSource.clip = _enemyExplosionClip;
        }

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }

        if(_anim == null)
        {
            Debug.LogError("Enemy Explode Animator is Null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if(_enemyDead == false)
        {
            EnemyFire();
        }
    }

    void EnemyFire()
    {
        if (Time.time > _lastTimeFire + _timeBetweenFire)
        {
            _timeBetweenFire = Random.Range(_minRandomTimeBetweenFire, _maxRandomTimeBetweenFire);
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            _lastTimeFire = Time.time;
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject, 0.1f);
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            SpawnPowerUp();

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.4f); 
        }

        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;

            if(player != null)
            {
                player.PlayerDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            Destroy(this.gameObject, 0.4f);
        }

        if(other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            Destroy(this.gameObject, 0.4f);
        }
    }

    public void SpawnPowerUp()
    {
        if(_enemyDead == true)
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
                    Instantiate(_spawnManager._powerUps[i], _powerUpSpawnPosition, Quaternion.identity);
                    Debug.Log(_spawnManager._powerUps[i]);
                    return;
                }
                else
                {
                    randomNumber -= _percentageChance[i];
                }
            }

        }
    }

}
