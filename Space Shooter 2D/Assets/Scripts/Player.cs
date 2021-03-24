using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
        [SerializeField] private float _speed = 5.0f;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = new Vector3(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            CalculateMovement();
        }

        void CalculateMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 velocity = new Vector3(horizontalInput, verticalInput, 0) * _speed;

            transform.Translate(velocity * Time.deltaTime);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.5f, 0), 0);
        }
  
}
