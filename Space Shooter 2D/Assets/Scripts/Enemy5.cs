using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField][Range(2.0f, 10.0f)] float _speed;
    [SerializeField] [Range(2.0f, 10.0f)] float _rotationSpeed;
    private Animator _anim;

    [Header("PowerUps")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioClip _enemyExplosionClip;

    [Header("Managers")]
    private SpawnManager _spawnManager;

    [Header("Player")]
    private Transform _playerPos;
    private Player _player;

    

    void Start()
    {
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

    }

    // Slower Rotation Speed, More Time to Dodge
    void Update()
    {
        var direction = _playerPos.position - transform.position; //Look Direction
        transform.up = Vector3.MoveTowards(transform.up, direction, _rotationSpeed * Time.deltaTime); //Move the Up Position Towards the Player

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, _speed * Time.deltaTime);
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject, 0.1f);
            _powerUpSpawnPosition = transform.position;
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
            //_enemyDead = true;

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
    }
 
    public void SpawnPowerUp()
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
