using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeSpan;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(_rigidbody.transform.up * _speed);
        Destroy(gameObject, _lifeSpan);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
