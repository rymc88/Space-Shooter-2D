using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private int _lives = 3;
    [SerializeField][Range(2.0f,15.0f)] private float _speed = 3.0f;
    [SerializeField][Range(0.25f, 1.0f)] private float _timeBetweenFire = 0.5f;
    private float _lastFireTime;
    public int _currentAmmo;
    public int _maxAmmo = 15;
    public int _currentThrusters;
    public int _maxThrusters = 100;
    public int _currentMagnet;
    public int _maxMagnet = 100;
    [SerializeField] private int _score;
    [SerializeField] private bool _hasAmmo = true;
    private Animator _animator;

    [Header("Game Object")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _thrusters;
    private GameObject _enemy;

    [Header("Damage Indictors")]
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;

    [Header("Ammo Bar")]
    [SerializeField] private AmmoBar _ammoBar;

    [Header("Thruster Bar")]
    [SerializeField] private ThrusterBar _thrusterBar;
    private bool _canUseThruster = true;

    [Header("Triple Shot Power Up")]
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private bool _isTripleShotActive = false;

    [Header("Speed Boost Power Up")]
    [SerializeField] private bool _isSpeedBoostActive = false;

    [Header("Shield Power Up")]
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private int _shieldStrength;
    [SerializeField] SpriteRenderer _spriteRender;

    [Header("Magnet Power Up")]
    [SerializeField] private GameObject _magnetVisualizer;
    [SerializeField] public bool isMagnetActive = false;
    [SerializeField] private MagnetBar _magnetBar;

    [Header("Rapid Fire Power")]
    [SerializeField] private bool _rapidFire = false;
    [SerializeField] [Range(0.05f, 0.2f)] private float _rapidFireRate;

    [Header("Missile")]
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private bool _homingMissileActive = false;

    [Header("Managers")]
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [Header("Audio and SFX")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSoundClip;

    [Header("Camera")]
    public CameraShake cameraShake;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        _currentAmmo = _maxAmmo;
        _currentThrusters = _maxThrusters;
        _currentMagnet = _maxMagnet;

        if(_magnetBar == null)
        {
            Debug.LogError("Magnet Bar is NULL");
        }
            
        if(_thrusterBar == null)
        {
            Debug.LogError("Thruster Bar is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_animator == null)
        {
            Debug.LogError("Animator is Null");
        }

    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _lastFireTime + _timeBetweenFire)
        {
            if(_hasAmmo == true)
            {
                if(_rapidFire == true)
                {
                    RapidFire();
                }
                else
                {
                    FireLaser();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && _homingMissileActive == true)
        {
            _enemy = GameObject.FindGameObjectWithTag("Enemy");

            if(_enemy != null)
            {
                var offset = new Vector3(0, 1.75f, 0);
                Instantiate(_missilePrefab, transform.position + offset, Quaternion.identity);
                _homingMissileActive = false;
            }
            else
            {
                Debug.Log("Enemy Target is NULL");
            }
        }

        if (Input.GetKey(KeyCode.C))
        {
            _magnetBar.UseMagnet(1);
        }
        else
        {
            DeactivateMagnet();
        }
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _animator.SetFloat("Turning", horizontalInput);

        if(_isSpeedBoostActive == true)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _canUseThruster == true)
            {
                _speed = 9.0f;
                _animator.SetBool("Thrusters", true);
                _thrusterBar.UseThruster(1);
            }
            else
            {
                _speed = 6.0f;
                _animator.SetBool("Thrusters", true);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && _canUseThruster == true)
            {
                _speed = 6.0f;
                _animator.SetBool("Thrusters", true);
                _thrusterBar.UseThruster(1);
                
            }
            else
            {
                _speed = 3.0f;
                _animator.SetBool("Thrusters", false);
            }
        }

        Vector3 velocity = new Vector3(horizontalInput, verticalInput, 0) * _speed;
        transform.Translate(velocity * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.8f, 5.8f), 0);
   }

    void FireLaser()
    {
        var offset = new Vector3(0, 1.05f, 0);

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _lastFireTime = Time.time;
        _audioSource.Play();
        _currentAmmo--;
        _ammoBar.UseAmmo(1);

        if (_currentAmmo <= 0)
        {
            _hasAmmo = false;
        }
    }

    void RapidFire()
    {
        _timeBetweenFire -= _rapidFireRate;
        var offset = new Vector3(0, 1.05f, 0);

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _lastFireTime = Time.time;
        _audioSource.Play();
        _currentAmmo--;
        _ammoBar.UseAmmo(1);


        if (_currentAmmo <= 0)
        {
            _hasAmmo = false;
        }
    }

    public void PlayerDamage()
    {
        if(_isShieldActive == true && _shieldStrength <= 3)
        {
            _shieldStrength--;

            if(_shieldStrength == 2)
            {
                _spriteRender.color = Color.magenta;
            }
            else if(_shieldStrength == 1)
            {
                _spriteRender.color = Color.red;
            }
            else if(_shieldStrength <= 0)
            {
                _shieldVisualizer.SetActive(false);
                _isShieldActive = false;
                return;
            }
        }

        else if(_isShieldActive == false)
        {
            _lives -= 1;
            //get handle on Healthbar script
            //subtract 10 from health
            //if current health < and > 

            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_lives == 1)
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

        StartCoroutine(cameraShake.Shake(.5f, .3f));
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
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoastPowerDownRoutine());
    }

    IEnumerator SpeedBoastPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldPowerUpActive()
    {
        if(_isShieldActive != true)
        {
            _isShieldActive = true;
            _shieldStrength = 3;
            _shieldVisualizer.SetActive(true);
            _spriteRender.color = Color.cyan;
        }
       
    }

    public void IncreaseHealth()
    {
        if(_lives < 3)
        {
            _lives++;

            if(_lives == 3)
            {
                _leftEngine.SetActive(false);
            }
            else if(_lives == 2)
            {
                _rightEngine.SetActive(false);
            }

            _uiManager.UpdateLives(_lives);
        }
        else
        {
            _lives = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            PlayerDamage();
        }
    }

    public void AmmoReload()
    {
        _currentAmmo = _maxAmmo;
        _hasAmmo = true;
        _ammoBar.ReloadAmmo();
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ActivateThruster()
    {
        _canUseThruster = true;
    }

    public void DeactivateThruster()
    {
        _canUseThruster = false;
    }

    public void RapidFireActive()
    {
        _rapidFire = true;
        StartCoroutine(RapidFirePowerDown());
    }

    IEnumerator RapidFirePowerDown()
    {
         yield return new WaitForSeconds(5.0f);
         _rapidFire = false;
    }

    public void ActivateHomingMissile()
    {
        _homingMissileActive = true;
    }

    public void ActivateMagnet()
    {
        _magnetVisualizer.SetActive(true);
        isMagnetActive = true;
    }

    public void DeactivateMagnet()
    {
        _magnetVisualizer.SetActive(false);
        isMagnetActive = false;
    }

    public void MagnetReload()
    {
        _currentMagnet = _maxMagnet;
        _magnetBar.ReloadMagnet();
    }
}

