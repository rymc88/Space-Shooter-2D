using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] private float _speed;
    [SerializeField] private GameObject _explosionAnim;
    
    void Start()
    {
        StartCoroutine(ExplosionCountDown());
    }

    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    IEnumerator ExplosionCountDown()
    {
        if (this.gameObject != null)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
            _explosionAnim.SetActive(true);
            Destroy(this.gameObject, .4f);
        }
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _explosionAnim.SetActive(true);
            Destroy(this.gameObject, .4f);
        }

        if(other.tag == "Player")
        {
            _explosionAnim.SetActive(true);
        }
    }
}
