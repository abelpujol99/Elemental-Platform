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
        private float _distanceToDetect;
        private float _difference;
        
        public bool _onAir;
        public bool _canMove;
        private bool _knockedUp;
        private bool _attacking;
        private bool _obstacle;

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
            _distanceToDetect = 0.1f;
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
            
            
            float distanceBetweenCharacterAndRino = Vector3.Distance(transform.position, _characterPosition.position);

            _vectorToAvoidObstacles1 = new Vector3(transform.position.x + _distanceFront, transform.position.y - 0.1f, 0);
            _vectorToAvoidObstacles2 = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);

            _vectorToAvoidFall1 = new Vector3(transform.position.x + _distanceToFloor, transform.position.y - 0.2f, 0);
            _vectorToAvoidFall2 = new Vector3(transform.position.x + _distanceToFloor / 4, transform.position.y, 0);
            
            Debug.DrawRay(_vectorToAvoidFall1, new Vector3(transform.position.x + _distanceToFloor / 4, transform.position.y, 0) - transform.position, Color.green);


            _avoidObstacles = Physics2D.Raycast(_vectorToAvoidObstacles1, 
                new Vector3(transform.position.x + 0.3f, transform.position.y, 0) - transform.position,
                Vector3.Distance(_vectorToAvoidObstacles2, transform.position), LayerMask.GetMask("Tilemap2", "Rock", "Enemy"));

            _avoidFall = Physics2D.Raycast(_vectorToAvoidFall1, new Vector3(transform.position.x + _distanceToFloor / 4, transform.position.y, 0) - transform.position,
                0.1f,
                LayerMask.GetMask("Tilemap1"));
            
            _lookScenario = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position,
                distanceBetweenCharacterAndRino,
                LayerMask.GetMask("Tilemap1", "Tilemap2"));
            
            _foundPlayer = Physics2D.Raycast(transform.position, _characterPosition.position - transform.position, 1.3f,
                LayerMask.GetMask("Player"));


            if (_avoidObstacles)
            {
                if (_spriteRenderer.flipX)
                {
                    _spriteRenderer.flipX = false;
                    _distance = Mathf.Abs(_distance) * -1;
                    _distanceToFloor = Mathf.Abs(_distanceToFloor) * -1;
                    _distanceFront = Mathf.Abs(_distanceFront) * -1;
                    _distanceToDetect = Mathf.Abs(_distanceToDetect) * -1;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                    _distance = Mathf.Abs(_distance);
                    _distanceToFloor = Mathf.Abs(_distanceToFloor);
                    _distanceFront = Mathf.Abs(_distanceFront);
                    _distanceToDetect = Mathf.Abs(_distanceToDetect);
                }
                
                if (_distance > 0)
                {
                    //_distanceToFloor = Mathf.Abs(_distanceToFloor);
                }
                else
                {
                    //_distanceToFloor = Mathf.Abs(_distanceToFloor) * -1;
                }
                _obstacle = true;
                _targetPosition = _lastTargetPosition;
            }
            
            
            if (_rb2D.velocity.y != 0)
            {
                _onAir = true;
                _canMove = false;
            }
            else if (_rb2D.velocity.y == 0)
            {
                _onAir = false;
            }

            if (!_lookScenario)
            {
                //Debug.Log("Escenario");
                if (_foundPlayer && !_onAir)
                {
                    //Debug.Log("Ataca");
                    Attack();
                    
                }
            }
            else
            {
                //Debug.Log("no ataca");
                _attacking = false;
            }
            
            
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
                else if (_obstacle || !_avoidFall)
                {
                    Debug.Log("entras");
                    _obstacle = false;
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
                    Debug.Log(_lastTargetPosition);
                }
                Move();
            }
            
            
            
            
            
            /*if (_foundPlayer && _rb2D.velocity.y == 0)
            {
                if (!_lookScenario)
                {
                    Attack();
                }
                else
                {
                    _attacking = false;
                    if (_targetPosition.x > transform.position.x)
                    {
                        _distance = Mathf.Abs(_distance);
                    }
                    else
                    {
                        _distance = Mathf.Abs(_distance) * -1;
                    }
                }
                
            }
            else
            {
                _attacking = false;
                if (_targetPosition.x > transform.position.x)
                {
                    _distance = Mathf.Abs(_distance);
                }
                else
                {
                    _distance = Mathf.Abs(_distance) * -1;
                }
            }

            if (_rb2D.velocity.y != 0)
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
                _distanceToFloor = Mathf.Abs(_distanceToFloor);
            }
            else
            {
                _spriteRenderer.flipX = false;
                _distanceToFloor = Mathf.Abs(_distanceToFloor) * -1;
            }
            
            if (_health <= 0)
            {
                _animator.Play("Die");
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(DestroyRino());
            }
            else if (!_attacking && _distance != 0)
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
            else if (!_avoidFall || _obstacle)
            {
                _obstacle = false;
                _distance *= -1;
                _lastTargetPosition = new Vector3(_lastTargetPosition.x + _distance, _lastTargetPosition.y, 0);
                _targetPosition = _lastTargetPosition;
                Debug.Log(_targetPosition);
            }*/
        }

        private void Attack()
        {
            _attacking = true;

            if (_canMove)
            {
                if (_characterPosition.position.x > transform.position.x)
                {
                    _distance = Mathf.Abs(_distance);
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _distance = Mathf.Abs(_distance) * -1;
                    _spriteRenderer.flipX = false;
                }
                
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_characterPosition.position.x + _distance, transform.position.y, 0), Time.deltaTime * _speed);
            }
            
        }
        
        private void Move()
        {
            if (_canMove)
            {
                Debug.Log("muevete");
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
                collision.transform.GetComponent<PlayerRespawn>().playerDamage();
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
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Player"))
            {
                trigger.transform.GetComponent<PlayerRespawn>().playerDamage();
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