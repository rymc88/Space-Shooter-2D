using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField][Range(0f,10.0f)] private float _speed;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] [Range(0f, 5.0f)] private float _timeBetweenFire;
    private float _lastTimeFire;
    private bool _enemyDead = false;
    [SerializeField] private Animator _anim;

    [Header("Canon")]
    [SerializeField] private GameObject _canon;
    [SerializeField] private GameObject _firePoint;
    [SerializeField][Range(0f,10.0f)] private float _rotationSpeed;

    [Header("Power Ups")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioClip _enemyExplosionClip;

    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;

    [Header("Player")]
    private Transform _playerPos;
    private Player _player;
    
    void Start()
    {
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CalculateMovement();

        if(_enemyDead == false)
        {
            Firing();
        }
       
    }

    private void Firing()
    {
        var direction = _playerPos.position - transform.position; //Heading Direction
        _canon.transform.up = Vector3.MoveTowards(_canon.transform.up, direction, _rotationSpeed * Time.deltaTime);

        if (Time.time > _lastTimeFire + _timeBetweenFire)
        {
            Instantiate(_laserPrefab, _firePoint.transform.position, _firePoint.transform.rotation);
            _lastTimeFire = Time.time;
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8, 0);
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

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;

            if (player != null)
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

        if (other.tag == "Missile")
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
        if (_enemyDead == true)
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
