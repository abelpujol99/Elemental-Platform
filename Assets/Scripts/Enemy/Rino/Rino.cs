using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy.Rino
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

        private Vector3 _targetPosition, _lastTargetPosition, _vectorToAvoidObstacles1 , _vectorToAvoidObstacles2, _vectorToAvoidFall1, _vectorToAvoidFall2;

        private RaycastHit2D _lookScenario;
        private RaycastHit2D _foundPlayer;
        private RaycastHit2D _avoidFall;
        private RaycastHit2D _avoidObstacles;

        private float _initialSpeed;
        private float _knockUp;
        private float _slow;
        private float _distanceToFloor;
        private float _distanceFront;
        private float _difference;
        
        public bool _onAir;
        public bool _canMove;
        private bool _knockedUp;
        public bool _attacking;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _initialSpeed = _speed;
            _knockUp = 3;
            _slow = 3;
            _distanceToFloor = 0.3f;
            _distanceFront = 0.38f;
            _canMove = true;
            if (_distance > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }

        private void Update()
        {
            if (_health <= 0)
            {
                _animator.Play("Die");
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(DestroyRino());
            }
            

            if (_distance > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            //float distanceBetweenCharacterAndRino = Vector3.Distance(transform.position, _characterPosition.position);

            _vectorToAvoidObstacles1 = new Vector3(transform.position.x + Mathf.Sign(_distance) * _distanceFront, transform.position.y - 0.1f, 0);
            _vectorToAvoidObstacles2 = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);

            _vectorToAvoidFall1 = new Vector3(transform.position.x + Mathf.Sign(_distance) *  _distanceToFloor, transform.position.y - 0.1f, 0);
            _vectorToAvoidFall2 = new Vector3(transform.position.x + Mathf.Sign(_distance) *  _distanceToFloor / 4, transform.position.y, 0);
            
            Debug.DrawRay(_vectorToAvoidFall1, (Mathf.Sign(_distance) * Vector2.right + Vector2.down).normalized * 0.14f, Color.green);
            Debug.DrawRay(_vectorToAvoidObstacles1, (Mathf.Sign(_distance) * Vector2.right).normalized * 0.1f , Color.green);


            _avoidObstacles = Physics2D.Raycast(_vectorToAvoidObstacles1, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap2", "Rock", "Enemy"));

            _avoidFall = Physics2D.Raycast(_vectorToAvoidFall1, (Mathf.Sign(_distance) * Vector2.right + Vector2.down).normalized,
                0.14f,
                LayerMask.GetMask("Tilemap1"));
            
            /*_lookScenario = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position,
                distanceBetweenCharacterAndRino,
                LayerMask.GetMask("Tilemap1", "Tilemap2"));*/
            
            _foundPlayer = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position, 1.3f,
                LayerMask.GetMask("Player"));
            

            if ((_avoidObstacles || !_avoidFall) && !_knockedUp)
            {
                if (_spriteRenderer.flipX)
                {
                    _spriteRenderer.flipX = false;
                    _distance = Mathf.Abs(_distance) * -1;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                    _distance = Mathf.Abs(_distance);
                }
                
                _targetPosition = _lastTargetPosition;
            }
            

            /*if (!_lookScenario)
            {
                if (_foundPlayer && !_onAir)
                {
                        //Attack();
                }
                else
                {
                    _attacking = false;
                }
            }
            else
            {
                _attacking = false;
            }*/
            
            
            if (!_attacking)
            {
                if (_targetPosition == transform.position)
                {
                    if (_spriteRenderer.flipX)
                    {
                        _spriteRenderer.flipX = false;
                    }
                    else
                    {
                        _spriteRenderer.flipX = true;
                    }
                    _distance *= -1;
                    _lastTargetPosition = transform.position;
                    _targetPosition = new Vector3(_lastTargetPosition.x + _distance, _lastTargetPosition.y, 0);
                
                }
                else if ((_avoidObstacles || !_avoidFall) && !_knockedUp)
                {
                    
                    if (_spriteRenderer.flipX)
                    {
                        _spriteRenderer.flipX = false;
                    }
                    else
                    {
                        _spriteRenderer.flipX = true;
                    }
                    
                    if (_lastTargetPosition.x > transform.position.x)
                    {
                        _difference = transform.position.x - _lastTargetPosition.x;
                    }
                    else
                    {
                        _difference = transform.position.x - _lastTargetPosition.x;
                    }
                    _difference = -(-_distance - _difference);
                    _lastTargetPosition = new Vector3(_lastTargetPosition.x + _distance + _difference, _lastTargetPosition.y, 0);
                    _targetPosition = _lastTargetPosition;
                }
                Move();
            }
        }

        private void Attack()
        {
            _attacking = true;

            if (_canMove)
            {
                if (_characterPosition.position.x > transform.position.x)
                {
                    _distance = Mathf.Abs(_distance);
                }
                else
                {
                    _distance = Mathf.Abs(_distance) * -1;
                }
                
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_characterPosition.position.x + _distance, transform.position.y, 0), Time.deltaTime * _speed);
            }
            
        }
        
        private void Move()
        {
            if (_canMove)
            {
                _animator.SetBool("Run", true);         
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
            }
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
                collision.transform.GetComponent<PlayerRespawn>().PlayerDamage();
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

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Player"))
            {
                trigger.transform.GetComponent<PlayerRespawn>().PlayerDamage();
            }
            else if (trigger.transform.CompareTag("Shuriken"))
            {
                _health -= 0.5f;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("Fire"))
            {
                _health -= 2;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("Lightning"))
            {
                _health -= 1;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("Water"))
            {
                _health -= 0.1f;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("Wind"))
            {
                if (_speed == _initialSpeed)
                {
                    StartCoroutine(ReturnSpeed());
                    _speed -= _speed / _slow;
                }
            }
            else if (trigger.gameObject.CompareTag("SuperFire"))
            {
                _health -= 6;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("SuperLightning"))
            {
                _health -= 2.5f;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("SuperWater"))
            {
                _health -= 0.5f;
                Hit();
            }
            else if (trigger.gameObject.CompareTag("SuperWind"))
            {
                _rb2D.velocity = new Vector2(0, _knockUp);
                _knockedUp = true;
            }
        }
    }
}