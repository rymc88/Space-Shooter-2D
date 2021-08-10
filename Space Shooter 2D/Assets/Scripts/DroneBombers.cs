using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBombers : MonoBehaviour
{
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private GameObject _mainShip;
    private Animator _anim;
    [SerializeField] private AudioClip _explosionAudioClip;
    private float _timeBetweenFire;
    private float _lastTimeFire;
    [SerializeField] private GameObject _bombPrefab;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _lastTimeFire = Random.Range(1.0f, 3.0f);

    }


    void Update()
    {
        transform.RotateAround(_mainShip.transform.position, Vector3.back, _orbitSpeed * Time.deltaTime);

        if (_mainShip.GetComponent<Enemy7>().mainShipDestroyed == true)
        {
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 0.4f);
        }

        EnemyFire();
    }

    void EnemyFire()
    {
        if (Time.time > _lastTimeFire + _timeBetweenFire)
        {
            _timeBetweenFire = Random.Range(2.0f, 6.0f);
            Instantiate(_bombPrefab, transform.position, Quaternion.identity);
            _lastTimeFire = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(this.gameObject, 0.4f);

        }

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.PlayerDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.4f);
        }
    }
}
