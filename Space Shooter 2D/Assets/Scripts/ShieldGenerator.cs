using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _bossShip;
    [SerializeField] private int _health;
    private bool _rotatingClockwise;
    [SerializeField] private float _orbitSpeed;


    // Start is called before the first frame update
    void Start()
    {
        _rotatingClockwise = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotatingClockwise == true)
        {
            StartCoroutine(RotateClockwise());
        }
        else
        {
            StartCoroutine(RotateCounterClockwise());
        }

        if(_orbitSpeed < 100)
        {
            StartCoroutine(IncreaseOrbitSpeed());
        }
        else
        {
            StartCoroutine(DecreaseOrbitSpeed());
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            _health -= 1;
            Debug.Log(_health);
            if(_health <= 0)
            {
                Destroy(this.gameObject);
            }
            Destroy(other.gameObject);
        }
    }

    IEnumerator IncreaseOrbitSpeed()
    {
        yield return new WaitForSeconds(5.0f);

        while(_orbitSpeed < 100)
        {
            _orbitSpeed += .5f;
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator DecreaseOrbitSpeed()
    {
        yield return new WaitForSeconds(5.0f);

        while(_orbitSpeed > 5)
        {
            _orbitSpeed -= .5f;
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator RotateClockwise()
    {
        transform.RotateAround(_bossShip.transform.position, Vector3.forward, _orbitSpeed * Time.deltaTime);

        yield return new WaitForSeconds(20.0f);

        _rotatingClockwise = false;
    }

    IEnumerator RotateCounterClockwise()
    {
        transform.RotateAround(_bossShip.transform.position, Vector3.back, _orbitSpeed * Time.deltaTime);

        yield return new WaitForSeconds(20.0f);

        _rotatingClockwise = true;
    }

   
}
