using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _rotateSpeed = 200.0f;

    void Start()
    {
 
    }

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;

            transform.up = Vector3.MoveTowards(transform.up, direction, _rotateSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Target is NULL");
        }
        
        
    }

}
