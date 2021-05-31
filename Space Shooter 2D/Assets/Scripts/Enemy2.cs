using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private int _startPos;
    [SerializeField] private bool _isMovingRight;

    private Player _player;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionAudioClip;

    [SerializeField] GameObject _laserPrefab;
    private float _fireRate;
    private float _canFire = -1.0f;
    [SerializeField] private bool _enemyDead = false;
    private Animator _anim;
    

    void Start()
    {
        
        //_isMovingRight = true;
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
        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(_enemyDead != true)
        {
            EnemyFire();
        }

        if(transform.position.x < -11 || transform.position.x > 11)
        {
            Destroy(this.gameObject);
            _enemyDead = true;
        }
    }

    private void CalculateMovement()
    {
        if(_isMovingRight == true)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }


        /*if (_isMovingRight == true)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);

            if (transform.position.x > 10)
            {
                _isMovingRight = false;

            }
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);

            if (transform.position.x < -10)
            {
                _isMovingRight = true;
            }
        }*/
    }

    private void EnemyFire()
    {
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();


            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

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
            //add explosion animation
            _enemyDead = true;
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, .4f); 
        }

        else if(other.tag == "Missile")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore(5);
            }

            _enemyDead = true;
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, .4f);
        }
    }
}