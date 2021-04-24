using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5.5f;
    [SerializeField] private string _playersInitials;
    public int score;
    [SerializeField] private bool _powerupActive;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _fireTime = -1f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoastActive = false;
    [SerializeField] private float _speedMulitplier = 2.0f;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private int _score;
    private UIManager _uiManager;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSoundClip;

   // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
       
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is Null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

   // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireTime)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(horizontalInput, verticalInput, 0) * _speed;
        transform.Translate(velocity * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.8f, 5.8f), 0);
   }

    void FireLaser()
    {
        _fireTime = Time.time + _fireRate;
        var offset = new Vector3(0, 1.05f, 0);

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _audioSource.Play();

    }

    public void PlayerDamage()
    {
        if(_isShieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _isShieldActive = false;
            return;
        }

        _lives -= 1;

        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoastActive()
    {
        _isSpeedBoastActive = true;
        _speed *= _speedMulitplier;
        StartCoroutine(SpeedBoastPowerDownRoutine());
    }

    IEnumerator SpeedBoastPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoastActive = false;
        _speed /= _speedMulitplier;
    }

    public void ShieldPowerUpActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        Debug.Log("Shields Activated");
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


    
    
}

