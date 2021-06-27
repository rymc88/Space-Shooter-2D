using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] [Range(2.0f, 15.0f)] private float _speed;
    [SerializeField] private int _hits = 3;
    [SerializeField] [Range(10f, 30f)] private int _orbitDistance;
    [SerializeField] private float _currentAngle;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField][Range(0f, 8.0f)] float _timeBetweenFire;
    private float _lastTimeFire;
    private float _xPos;
    private float _yPos;
    private bool _isMovingDown;
    private bool _enemyDead = false;
    private Animator _anim;

    [Header("Power Ups")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _explosionClip;

    [Header("Audio & SFX")]
    [SerializeField] private SpawnManager _spawnManager;
   
    private Player _player;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        transform.position = new Vector3(Random.Range(-9, 9), 8, transform.position.z);
        _isMovingDown = true; 
    }

    void Update()
    {

        if (transform.position.y >= 2 && _isMovingDown == true)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if(transform.position.y < 2 && _isMovingDown == true)
        {
            _xPos = transform.position.x;
            _yPos = transform.position.y;
            _isMovingDown = false;
        }

        if(_isMovingDown == false)
        {
            CircularMovement();
        }

        if(_enemyDead == false)
        {
            EnemyFire();
        }

    }

    private void CircularMovement()
    {
        _currentAngle += 0.01f;
      
        float x = _xPos + Mathf.Cos(_currentAngle * _orbitDistance);
        float y = _yPos + Mathf.Sin(_currentAngle * _orbitDistance);
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);
    }

    private void EnemyFire()
    {
        if(Time.time > _lastTimeFire + _timeBetweenFire)
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            _lastTimeFire = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject, 0.2f);
            _hits -= 1;
            _anim.SetTrigger("OnEnemyHit");
            _audioSource.PlayOneShot(_hitClip, 0.2f);
            

            if (_hits == 0)
            {
                if(_player != null)
                {
                    _player.AddScore(10);
                }
                _powerUpSpawnPosition = transform.position;
                _enemyDead = true;
                AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
                _anim.SetTrigger("OnEnemyDeath");
                SpawnPowerUp();
                Destroy(this.gameObject,0.4f);
                
            }
        }
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
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
