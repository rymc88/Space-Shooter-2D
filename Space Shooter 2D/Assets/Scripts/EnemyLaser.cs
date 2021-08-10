using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] [Range(2.0f, 8.0f)] float _speed;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            /*Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.PlayerDamage();
            }*/

            Destroy(gameObject);
        }
    }
}