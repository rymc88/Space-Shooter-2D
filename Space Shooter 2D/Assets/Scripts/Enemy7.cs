using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : MonoBehaviour
{
    [Header("Enemy Properties")]
    private Animator _anim;
    public bool mainShipDestroyed = false;
    private bool _movingLeft = true;
    [SerializeField] [Range(0.0f, 10.0f)] private float _speed;
    private bool _enemyDead = false;

    [Header("Power Ups")]
    [SerializeField] private int[] _percentageChance;
    [SerializeField] private int _total;
    private Vector3 _powerUpSpawnPosition;

    [Header("Audio & SFX")]
    [SerializeField] private AudioClip _explosionAudioClip;

    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }


    void Update()
    {
       if(_movingLeft == true)
        {
            StartCoroutine(MoveLeft());
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(MoveRight());
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            OnDeath();
            _enemyDead = true;
            _powerUpSpawnPosition = transform.position;
            SpawnPowerUp();
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(this.gameObject, .4f);
        }

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            _enemyDead = true;
            _powerUpSpawnPosition = transform.position;
            OnDeath();

            if (player != null)
            {
                player.PlayerDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            Destroy(this.gameObject, 0.4f);
        }

        if (other.tag == "Missile")
        {
            OnDeath();
            Destroy(other.gameObject);
            _enemyDead = true;
            _powerUpSpawnPosition = transform.position;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            SpawnPowerUp();
            Destroy(this.gameObject, 0.4f);
        }

        if (other.tag == "LaserBeam")
        {
            OnDeath();
            _enemyDead = true;
            _powerUpSpawnPosition = transform.position;
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

    public void OnDeath()
    {
        mainShipDestroyed = true;
    }

    IEnumerator MoveLeft()
    {
        yield return new WaitForSeconds(4.0f);
        _movingLeft = false;
    }

    IEnumerator MoveRight()
    {
        yield return new WaitForSeconds(4.0f);
        _movingLeft = true;
    }
}
