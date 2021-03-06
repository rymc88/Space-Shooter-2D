using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerUpID;
    //0 = Triple Shot
    //1 = Speed
    //2 = Shield
    [SerializeField] AudioClip _powerUpClip;
    private Player _player;
    [SerializeField] [Range(0f, 10f)] private float _magnetSpeed;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = _player.transform.position;
        float magSpeed = _magnetSpeed * Time.deltaTime;

        if(_player.isMagnetActive == true)
        {
            transform.position = Vector2.MoveTowards(pos, targetPos, magSpeed);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        
        if (transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position, 1.0f);

            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoastActive();
                        break;
                    case 2:
                        player.ShieldPowerUpActive();
                        break;
                    case 3:
                        player.AmmoReload();
                        break;
                    case 4:
                        player.IncreaseHealth();
                        break;
                    case 5:
                        player.RapidFireActive();
                        break;
                    case 6:
                        player.ActivateHomingMissile();
                        break;
                    case 7:
                        player.MagnetReload();
                        break;
                    case 8:
                        player.LaserBeamActivate();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
            
        }
    }
}
