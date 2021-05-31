using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private bool _isMovingRight;

    void Start()
    {
        _isMovingRight = true;
        transform.position = new Vector3(Random.Range(-9, 9), 6, 0);

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_isMovingRight == true)
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
        }
    }
}