using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField][Range(2.0f, 15.0f)] private float _speed;
    [SerializeField][Range(1.0f, 15.0f)] private float _laserSpeed = 8f;
    [SerializeField] private bool _isDodging = false;
    [SerializeField] private int _sideMove;
    [SerializeField][Range(0f, 5.0f)]private float _timeBetweenFire;
    private float _lastFireTime;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private bool _isFiring = false;
    private Vector3 _direction;
    private float _rotationZ;
    private bool _enemyDead = false;
    private Animator _anim;

    [Header("Game Objects")]
    [SerializeField] private GameObject _shotDetection;
    [SerializeField] private GameObject _laserStart;

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
        _anim = GetComponent<Animator>();

    }

    void Update()
    {
        if(transform.position.y > 3.5f)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        _direction = _playerPos.position - transform.position;
        _rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ + 90);

        if (_isFiring == false && _enemyDead == false)
        {
            _shotDetection.SetActive(true);
            Dodging();
        }
        else if (_isFiring == true && _enemyDead == false)
        {
            _shotDetection.SetActive(false);
            EnemyFire();
        }

        if (_enemyDead == true)
        {
            Destroy(gameObject, 0.4f);
        }
    }

    private void Dodging()
    {
        StartCoroutine(StartFiring());
       

        if (_isDodging == true)
        {
            if (_sideMove == 0)
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);

                if (transform.position.x < -11)
                {
                    transform.position = new Vector3(9, transform.position.y);
                }
            }
            else
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);

                if (transform.position.x > 11)
                {
                    transform.position = new Vector3(-9, transform.position.y);
                }
            }
        }
        else
        {
            transform.Translate(Vector3.zero);
        } 
    }

    public void Dodge()
    {
       _sideMove = Random.Range(0, 2);
       StartCoroutine(StopDodging());
    }

    IEnumerator StopDodging()
    {
       _isDodging = true;
       yield return new WaitForSeconds(.3f);
       _isDodging = false;
       
    }

    void EnemyFire()
    {
        if(_isFiring == true && Time.time > (_lastFireTime + _timeBetweenFire))
        {
            
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab);
            enemyLaser.transform.position = _laserStart.transform.position;
            enemyLaser.transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ + 90);
            enemyLaser.GetComponent<Rigidbody2D>().velocity = _direction * _laserSpeed;
            _lastFireTime = Time.time;
            StartCoroutine(StopFiring());
        }
    }

    IEnumerator StartFiring()
    {
        yield return new WaitForSeconds(3.0f);
        _isFiring = true;
    }

    IEnumerator StopFiring()
    {
        yield return new WaitForSeconds(3.0f);
        _isFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && _isFiring == true)
        {
            Destroy(other.gameObject, 0.1f);
            _powerUpSpawnPosition = transform.position;
            _enemyDead = true;

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            //Destroy(gameObject, 0.4f);
        }
        else if (other.tag == "Laser")
        {
            Dodge();
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
            Destroy(gameObject, 0.4f);
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
            Destroy(gameObject, 0.4f);
        }

        if (other.tag == "LaserBeam")
        {
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
