using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{

    [SerializeField] private int _hits = 3;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _explosionClip;
    private Animator _anim;

    [SerializeField] private float _speed;
    [SerializeField] private int _orbitDistance;
    [SerializeField] private float _currentAngle;
    private float _xPos;
    private float _yPos;
    private bool _isMovingDown;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        
        transform.position = new Vector3(Random.Range(-9, 9), 8, transform.position.z);
        _orbitDistance = 10;
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
        

    }

    private void CircularMovement()
    {
        _currentAngle += 0.01f;
      
        float x = _xPos + Mathf.Cos(_currentAngle * _orbitDistance);
        float y = _yPos + Mathf.Sin(_currentAngle * _orbitDistance);
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject, 0.2f);
            _hits -= 1;
            _anim.SetTrigger("OnEnemyHit");
            _audioSource.PlayOneShot(_hitClip, 0.2f);
            
            if(_hits == 0)
            {
                AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
                _anim.SetTrigger("OnEnemyDeath");
                Destroy(this.gameObject,0.4f);
                
            }
        }
    }

    /*IEnumerator InitialMovement()
    {
        while (transform.position.y >= 2)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
            _speed = 0;
            _xPos = _circleCenter.transform.position.x;
            _yPos = _circleCenter.transform.position.y;
            yield return new WaitForSeconds(.5f);
    }*/

}
