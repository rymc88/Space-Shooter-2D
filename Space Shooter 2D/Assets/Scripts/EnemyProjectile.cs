using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] [Range(300f, 500f)] private float _speed;
    [SerializeField] [Range(0f, 10f)] private float _lifeSpan;
    [SerializeField] [Range(0f, 90f)] private float _zRotation;
    [SerializeField] private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(_rigidbody.transform.up * _speed);
        Destroy(gameObject, _lifeSpan);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, _zRotation);
    }

}
