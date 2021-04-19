using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy
{
    public class Rino : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _direction;

        private Animator _animator;
        
        private SpriteRenderer _spriteRenderer;
        
        private Rigidbody2D _rb2D;

        private Vector3 _targetPosition;

        private float _initialSpeed;
        private float _knockUp;
        
        private bool _onAir;
        private bool _canMove;
        private bool _knockedUp;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _initialSpeed = _speed;
            _knockUp = 3;
            _canMove = true;
            Move();
        }

        private void Update()
        {
            if (_health <= 0)
            {
                _animator.Play("Die");
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(DestroyRino());
            }
            else
            {
                if (!_onAir && _canMove)
                {
                    Move();
                }
                else
                {
                    _animator.SetBool("Run", false);
                }
            }
            
            if (transform.position.x == _targetPosition.x)
            {
                _direction *= -1;
                _targetPosition = new Vector3(transform.position.x + _direction, transform.position.y, 0);
            }
        }
        private void Move()
        {
            _animator.SetBool("Run", true);            
            if (_direction >= 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().playerDamage();
            }
            if (collision.gameObject.CompareTag("Fire"))
            {
                _health -= 2;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Lightning"))
            {
                _health -= 1;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Water"))
            {
                _health -= 0.1f;
                Hit();
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
                if (_onAir)
                {
                    Fall();
                }

                if (!_knockedUp)
                {
                    _targetPosition = new Vector3(transform.position.x + _direction, transform.position.y, 0);
                }
                
                _knockedUp = false;
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
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperLightning"))
            {
                _health -= 2.5f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperWater"))
            {
                _health -= 0.5f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperWind"))
            {
                _rb2D.velocity = new Vector2(0, _knockUp);
                _knockedUp = true;
            }
        }

        private void Hit()
        {
            _animator.SetBool("Hit", true);
            _animator.SetBool("Run", false);
            _canMove = false;
            StartCoroutine(SetMove("Hit", 0.25f));
        }
        

        private void Fall()
        {
            _animator.SetBool("Fall", true);
            _animator.SetBool("Run", false);
            _canMove = false;
            StartCoroutine(SetMove("Fall", 0.65f));
        }

        private IEnumerator ReturnSpeed()
        {
            yield return new WaitForSeconds(2);
            _speed += _initialSpeed / 3;
        }

        private IEnumerator SetMove(string state, float time)
        {
            yield return new WaitForSeconds(time);
            _animator.SetBool(state, false);
            _canMove = true;
        }

        private IEnumerator DestroyRino()
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
    }
}