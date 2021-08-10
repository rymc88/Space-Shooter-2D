using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField][Range(2.0f,15.0f)] private float _speed = 4.0f;
    [SerializeField] private int _startPos;
    [SerializeField] private bool _isMovingRight;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField][Range(0f, 3.0f)] float _minRandomTimeBetweenFire;
    [SerializeField][Range(4.0f, 7.0f)] float _maxRandomTimeBetweenFire;
    [SerializeField] float _timeBetweenFire;
    private float _lastTimeFire;
    private bool _enemyDead = false;
    private Animator _anim;

    [Header("Power Ups")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioClip _explosionAudioClip;
    
    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;

    private Player _player;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _startPos = Random.Range(0, 2);
        if(_startPos == 0)
        {
            transform.position = new Vector3(-10, Random.Range(3, 5), 0);
            _isMovingRight = true;
        }
        else
        {
            transform.position = new Vector3(10, Random.Range(3, 5), 0);
            _isMovingRight = false;
        }


        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    void Update()
    {
        CalculateMovement();

        if(_enemyDead != true)
        {
            EnemyFire();
        }

        /*if(transform.position.x < -11 || transform.position.x > 11)
        {
            Destroy(this.gameObject);
            _enemyDead = true;
        }*/
    }

    private void CalculateMovement()
    {
        if(_isMovingRight == true)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);

            if(transform.position.x > 11)
            {
                transform.position = new Vector3(-10, Random.Range(3, 5), 0);
            }

        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);

            if(transform.position.x < -11)
            {
                transform.position = new Vector3(10, Random.Range(3, 5), 0);
            }

        }
    }

    private void EnemyFire()
    {
        if(Time.time > _lastTimeFire + _timeBetweenFire)
        {
            _timeBetweenFire = Random.Range(_minRandomTimeBetweenFire, _maxRandomTimeBetweenFire);
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            _lastTimeFire = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            
            Destroy(other.gameObject, 0.1f);

            if(_player != null)
            {
                _player.AddScore(5);
            }

            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");
            SpawnPowerUp();
            Destroy(this.gameObject, .4f); 
        }

        else if(other.tag == "Missile")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore(5);
            }

            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");
            SpawnPowerUp();
            Destroy(this.gameObject, .4f);
        }

        if (other.tag == "LaserBeam")
        {
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            Destroy(this.gameObject, 0.4f);
        }
    }

    public void SpawnPowerUp()
    {
        if (_enemyDead == true)
        {

            //int randomPowerUp = Random.Range(0, 7);
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