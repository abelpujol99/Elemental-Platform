using System;
using System.Collections;
using Character;
using UnityEngine;
using UnityEngine.Timeline;

namespace Enemy
{
    public class Rino : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _distance;
        [SerializeField] private Transform _characterPosition;

        private Animator _animator;
        
        private SpriteRenderer _spriteRenderer;
        
        private Rigidbody2D _rb2D;

        private Vector3 _targetPosition;
        private Vector3 _lastTargetPosition;

        private RaycastHit2D _lookScenario;
        private RaycastHit2D _foundPlayer;
        private RaycastHit2D _avoidFall;
        private RaycastHit2D _avoidObstacle;

        private float _initialSpeed;
        private float _knockUp;
        private float _slow;
        
        private bool _onAir;
        private bool _canMove;
        private bool _knockedUp;
        private bool _attacking;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _initialSpeed = _speed;
            _knockUp = 3;
            _slow = 3;
            _canMove = true;
            Move();
        }

        private void Update()
        {            

            Vector3 vectorToAvoidObstacles = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
            float distanceBetweenCharacterAndRino = Vector3.Distance(transform.position, _characterPosition.position);
            
            _avoidObstacle = Physics2D.Raycast(vectorToAvoidObstacles,new Vector3(transform.position.x + _distance, transform.position.y - 0.1f, 0) - vectorToAvoidObstacles, 0.33f,
                LayerMask.GetMask("Tilemap1", "Tilemap2"));
            
            _avoidFall = Physics2D.Raycast(transform.position, new Vector3(transform.position.x + _distance, transform.position.y - 1, 0) - transform.position, 0.4f,
                LayerMask.GetMask("Tilemap1", "Tilemap2"));

            _lookScenario = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position,
                distanceBetweenCharacterAndRino,
                LayerMask.GetMask("Tilemap1", "Tilemap2"));
            
            _foundPlayer = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position, 1.3f,
                LayerMask.GetMask("Player"));

            if (_foundPlayer && _rb2D.velocity.y >= 0)
            {
                if (!_lookScenario)
                {
                    Attack();
                }
                else
                {
                    _attacking = false;
                    if (_targetPosition.x - transform.position.x < 0)
                    {
                        _distance = Mathf.Abs(_distance) * -1;
                    }
                    else
                    {
                        _distance = Mathf.Abs(_distance);
                    }
                }
                
            }
            else
            {
                _attacking = false;
                if (_targetPosition.x - transform.position.x < 0)
                {
                    _distance = Mathf.Abs(_distance) * -1;
                }
                else
                {
                    _distance = Mathf.Abs(_distance);
                }
            }

            if (_rb2D.velocity.y < 0)
            {
                _onAir = true;
            }
            else if (_rb2D.velocity.y == 0)
            {
                _onAir = false;
            }
               
            if (_distance > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
            
            if (_health <= 0)
            {
                _animator.Play("Die");
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(DestroyRino());
            }
            else if (!_attacking)
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

            if (transform.position.x == _targetPosition.x && !_attacking) 
            {
                _distance *= -1;
                _lastTargetPosition = transform.position;
                _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
            }
            else if (!_avoidFall || _avoidObstacle)
            {
                _distance *= -1;
                _targetPosition = _lastTargetPosition; 
            }
        }

        private void Attack()
        {
            _attacking = true;

            if (_canMove)
            {
                if (_characterPosition.position.x - transform.position.x < 0)
                {
                    _distance = Mathf.Abs(_distance) * -1;
                }
                else
                {
                    _distance = Mathf.Abs(_distance);
                }
                
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_characterPosition.position.x + _distance, transform.position.y, 0), Time.deltaTime * _speed);
            }
            
        }
        
        private void Move()
        {
            _animator.SetBool("Run", true);         
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }
        
        private void Hit()
        {
            _animator.SetBool("Hit", true);
            _canMove = false;
            StartCoroutine(SetMove("Hit", 0.25f));
        }

        private void Fall()
        {
            _animator.SetBool("Fall", true);
            _canMove = false;
            StartCoroutine(SetMove("Fall", 0.65f));
        }

        private IEnumerator ReturnSpeed()
        {
            yield return new WaitForSeconds(2);
            _speed += _initialSpeed / _slow;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().playerDamage();
            }

            if (collision.transform.CompareTag("Shuriken"))
            {
                _health -= 0.5f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Fire"))
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
                    _speed -= _speed / _slow;
                }
            }
            else if (collision.gameObject.CompareTag("Tilemap1"))
            {
                _lastTargetPosition = transform.position;
                
                if (_onAir)
                {
                    Fall();
                }

                if (!_knockedUp)
                {
                    _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
                }
                
                _knockedUp = false;
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

        
    }
}