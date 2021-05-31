using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    [SerializeField] private AudioClip _enemyExplosionClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    [SerializeField] private bool _enemyDead = false;
    [SerializeField] private int _enemyID;
    


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        

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

        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Enemy Explode Animator is Null");
        }
    }

    // Update is called once per frame
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
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();


            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
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
            _enemyDead = true;
            
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            //_audioSource.PlayOneShot(_audioSource.clip);
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.4f);
            
        }

        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            _enemyDead = true;

            if(player != null)
            {
                player.PlayerDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            //_audioSource.PlayOneShot(_audioSource.clip);
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.4f);
        }

        if(other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _enemyDead = true;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            //_audioSource.PlayOneShot(_audioSource.clip);
            AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.4f);
        }
    }
}
