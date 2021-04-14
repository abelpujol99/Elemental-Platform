using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Rino : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        
        private SpriteRenderer _spriteRenderer;
        
        private Rigidbody2D _rb2D;

        private Vector3 targetPosition;

        private int _direction = 1;
        
        private float _initialSpeed;
        private float _knockUp;

        [SerializeField] private bool _onAir;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _initialSpeed = _speed;
            _knockUp = 3;
            move();
            targetPosition = new Vector3(transform.position.x + _direction, transform.position.y, 0);
            
        }

        private void Update()
        {

            if (_health <= 0)
            {
                Destroy(gameObject);
            }

            if (!_onAir)
            {
                move();
            }
            
            if (transform.position.x == targetPosition.x)
            {
                _direction *= -1;
                targetPosition = new Vector3(transform.position.x + _direction, transform.position.y, 0);
            }
        }

        private void move()
        {
            
            if (_direction >= 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _speed);

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Fire"))
            {
                _health -= 2;
            }
            else if (collision.gameObject.CompareTag("Lightning"))
            {
                _health -= 1;
            }
            else if (collision.gameObject.CompareTag("Water"))
            {
                _health -= 0.1f;
            }
            else if (collision.gameObject.CompareTag("Wind"))
            {
                if (_speed == _initialSpeed)
                {
                    StartCoroutine(ReturnSpeed());
                    _speed -= _speed / 3;
                }
            }
            else if (collision.gameObject.CompareTag("Tilemap1"))
            {
                _onAir = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Tilemap1"))
            {
                _onAir = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("SuperFire"))
            {
                _health -= 6;
            }
            else if (collision.gameObject.CompareTag("SuperLightning"))
            {
                _health -= 2.5f;
            }
            else if (collision.gameObject.CompareTag("SuperWater"))
            {
                _health -= 0.5f;
            }
            else if (collision.gameObject.CompareTag("SuperWind"))
            {
                _rb2D.velocity = new Vector2(0, _knockUp);
            }
        }

        private IEnumerator ReturnSpeed()
        {
            yield return new WaitForSeconds(2);
            _speed += _initialSpeed / 3;
        }
    }
}