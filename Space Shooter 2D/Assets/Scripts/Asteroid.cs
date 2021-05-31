using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private Animator _anim;
    [SerializeField] private AudioClip _explosionEnemyClip;
    

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(0, 0, 3) * _rotationSpeed;
        transform.Rotate(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            AudioSource.PlayClipAtPoint(_explosionEnemyClip, transform.position);
            Destroy(this.gameObject);
        }
    }
}
