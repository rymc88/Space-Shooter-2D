using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    private Vector3 _startPosition;
    [SerializeField] private float _offset;
    [SerializeField] private int _xDir;
    [SerializeField] private int _yDir;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(_xDir, _yDir, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if(transform.position.y < _offset)
        {
            transform.position = _startPosition;
        }
    }
}
