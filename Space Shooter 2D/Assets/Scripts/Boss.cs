using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Shield")]
    private float _searchCountdown = 1.0f;
    [SerializeField] private GameObject _bossShield;
    private Renderer _bossShieldRenderer;
    [SerializeField] private bool _hasShieldGenerators;
    [SerializeField] private int _shieldHealth;
    private bool _isShieldDestroyed;

    [Header("Movement")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int current;
    private float wpRadius = .1f;
    [SerializeField] private float _speed;
    private bool _isMoving;

    [Header("Canons")]
    [SerializeField] private List<GameObject> _canon;
    [SerializeField] private List<GameObject> _firepoint;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _timeBetweenFire;
    private float _lastTimeFired;
    private bool _canFire = true;
    private int _currentCanon = 0;
    [SerializeField] private float _firingDelay;
    [SerializeField] private bool _canonsDestroyed;

    [Header("FP")]
    [SerializeField] private GameObject[] _FPs;

    [Header("Player")]
    private Transform _playerPos;
    private Player _thePlayer;

    void Start()
    {
        _bossShield.SetActive(true);
        _bossShieldRenderer = _bossShield.GetComponent<Renderer>();
        _hasShieldGenerators = true;
        _isShieldDestroyed = false;
        _isMoving = true;
        _canonsDestroyed = false;
       

        _thePlayer = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canonsDestroyed == true)
        {
            StartCoroutine(FinalFiringRoutine());
        }

        if(_isShieldDestroyed == false)
        {
            StartCoroutine(FiringRoutine());
        }
        else if(_isShieldDestroyed == true)
        {
            if(Time.time > _lastTimeFired + _timeBetweenFire)
            {
                foreach (var element in _firepoint)
                {
                    if(element == null)
                    {
                        _firepoint.Remove(element);
                    }
                    else
                    {
                        Instantiate(_projectilePrefab, element.transform.position, element.transform.rotation);
                    }
                    
                }

                _lastTimeFired = Time.time;
            }
        }

        var direction = _playerPos.position - transform.position;

        foreach(var element in _canon)
        {
            if(element == null)
            {
                _canon.Remove(element);

                if(_canon.Count == 0)
                {
                    _canonsDestroyed = true;
                    _canFire = true;
                }
            }
            else
            {
                element.transform.up = Vector3.MoveTowards(element.transform.up, direction, _rotationSpeed * Time.deltaTime);
            }
        }

        if(_isMoving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[current].transform.position, Time.deltaTime * _speed);
        }

        if(_isShieldDestroyed == false)
        {
            if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < wpRadius)
            {
                _isMoving = false;
                current = Random.Range(0, 3);
                StartCoroutine(MovementDelay());
            }
        }
        else if(_isShieldDestroyed == true)
        {
            if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < wpRadius)
            {
                _isMoving = false;
                current = Random.Range(0, wayPoints.Length);
                StartCoroutine(MovementDelay());
            }
        }
        
       
        if (ShieldGeneratorsActive() == false)
        {
            _hasShieldGenerators = false;
            _firingDelay = 1.5f;
        }
        else
        {
            return;
        }
    }

    public bool ShieldGeneratorsActive()
    {
        _searchCountdown -= Time.deltaTime;

        if(_searchCountdown <= 0)
        {
            _searchCountdown = 1.0f;

            if(GameObject.FindGameObjectWithTag("ShieldGenerator") == null)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(_hasShieldGenerators == true && _isShieldDestroyed != true)
        {
            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);
            }
        }
        else if(_canonsDestroyed != true && _hasShieldGenerators == false && _isShieldDestroyed == true)
        {
            if(other.tag == "Laser")
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            if(other.tag == "Laser")
            {
                _shieldHealth -= 1;
                Destroy(other.gameObject);
                _speed += 1;

                if(_shieldHealth < 4)
                {
                    _bossShieldRenderer.material.color = Color.yellow;
                    _speed += 1;
                }
               
                if(_shieldHealth <= 0)
                {
                    _isShieldDestroyed = true;
                    _bossShield.SetActive(false);
                    Destroy(this.GetComponent<CircleCollider2D>());
                }
            }
        }     
    }

    IEnumerator MovementDelay()
    {
        if (_hasShieldGenerators == true)
        {
            yield return new WaitForSeconds(1.5f);
        }
        else if (_hasShieldGenerators == false)
        {
            yield return new WaitForSeconds(3.0f);
        }
        else if (_isShieldDestroyed == true && _hasShieldGenerators == false)
        {
            yield return new WaitForSeconds(5.0f);
        }

        _isMoving = true;
    }

    IEnumerator FiringRoutine()
    {
        if (_canFire == true)
        {
            Instantiate(_projectilePrefab, _firepoint[_currentCanon].transform.position, _firepoint[_currentCanon].transform.rotation);
            _canFire = false;
            yield return new WaitForSeconds(_firingDelay);
            _currentCanon += 1;

            if (_currentCanon == _firepoint.Count)
            {
                _currentCanon = 0;
            }

            _canFire = true;
        }
    }

    IEnumerator FinalFiringRoutine()
    {
        if(_canFire == true)
        {
            foreach (var element in _FPs)
            {
                Instantiate(_projectilePrefab, element.transform.position, element.transform.rotation);
            }

            _canFire = false;
            yield return new WaitForSeconds(3.0f);

            _canFire = true;
        }   
        
    }
}
