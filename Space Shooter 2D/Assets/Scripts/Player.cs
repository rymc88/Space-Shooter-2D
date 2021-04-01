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
    

   // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
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

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.5f, 0), 0);

        /*if(transform.position.x < -10)
            {
                transform.position = new Vector3(10, transform.position.y, 0);
            }
        else if (transform.position.x > 10)
            {
                transform.position = new Vector3(-10, transform.position.y, 0);
            }
        else if(transform.position.y > 7)
            {
                transform.position = new Vector3(transform.position.x, -5, 0);
            }
        else if (transform.position.y < -5)
            {
                transform.position = new Vector3(transform.position.x, 7, 0);
            }*/
   }

    void FireLaser()
    {
        _fireTime = Time.time + _fireRate;
        var offset = new Vector3(0, 1, 0);
        Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
    }

  public void PlayerDamage()
    {
        _lives -= 1;

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
