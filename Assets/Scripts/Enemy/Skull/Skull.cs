using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy
{
    public class Skull : EnemyWithHealth
    {
        [SerializeField] private float _speed, _distance;
        
        [SerializeField] private Animator _animator;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Rigidbody2D _rb2D;
        
        private RaycastHit2D _avoidUpObstacles, _avoidMidObstacles, _avoidDownObstacles;

        private Vector3 _targetPosition, _lastTargetPosition, _firstVectorToAvoidObstacles, _secondVectorToAvoidObstacles, _thirdVectorToAvoidObstacles;

        private float _auxSpeed, _timeToRestoreFire, _auxTimeToRestoreFire, _timeToRestoreSpeed, _distanceFront, _difference;

        private void Start()
        {
            _auxSpeed = _speed;
            _timeToRestoreFire = 5;
            _auxTimeToRestoreFire = _timeToRestoreFire;
            _distanceFront = 0.25f;
            SetDirection();
            //SetTarget();
            _lastTargetPosition = transform.position;
        }

        private void Update()
        {
            if (_health <= 0)
            {
                StartCoroutine(DestroySkull());
            }
            
            SetDirection();

            RestoreSpeed();

            CheckRestoreFire();

            if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionY)
            {
                FreezeYActions();
            }
            else if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionX)
            {
                FreezeXActions();
            }

            Move();
        }

        private void RestoreSpeed()
        {
            if (_timeToRestoreSpeed > 0)
            {
                _timeToRestoreSpeed -= Time.deltaTime;
            }
            else
            {
                _speed = _auxSpeed;
            }
        }

        private void CheckRestoreFire()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("IdleWithFire"))
            {
                return;
            }
            if (_timeToRestoreFire > 0)
            {
                _timeToRestoreFire -= Time.deltaTime;
            }
            else
            {
                RestoreFire();
                _timeToRestoreFire = _auxTimeToRestoreFire;
            }
        }

        #region FreezeYActions

        private void FreezeYActions()
        {
            
            _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
            
            YVectorsToAvoidObstacles();

            Debug.DrawRay(_firstVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.right).normalized * 0.3f , Color.green);
            Debug.DrawRay(_secondVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.right).normalized * 0.1f , Color.green);
            Debug.DrawRay(_thirdVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.right).normalized * 0.3f , Color.green);
            
            YRaycastsToAvoidObstacles();

            CalculateYDifference();

            if (transform.position == _targetPosition)
            {
                TargetReached();
            }
            else if (_avoidUpObstacles || _avoidMidObstacles || _avoidDownObstacles)
            {
                FlipSpriteAndDistance();
                
                CalculateYDifference();
                
                _targetPosition = _lastTargetPosition;
                _lastTargetPosition = transform.position;
            
                _difference = -(-_distance - _difference);
                _lastTargetPosition = new Vector3(_lastTargetPosition.x + _distance + _difference, _lastTargetPosition.y, 0);
            }
        }

        private void YVectorsToAvoidObstacles()
        {
            _firstVectorToAvoidObstacles = new Vector3(transform.position.x , transform.position.y + _distanceFront, 0);
            _secondVectorToAvoidObstacles = new Vector3(transform.position.x + Mathf.Sign(_distance) * _distanceFront, transform.position.y, 0);
            _thirdVectorToAvoidObstacles = new Vector3(transform.position.x, transform.position.y - _distanceFront, 0);
        }

        private void YRaycastsToAvoidObstacles()
        {
            _avoidUpObstacles = Physics2D.Raycast(_firstVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "Enemy"));
            
            _avoidMidObstacles = Physics2D.Raycast(_secondVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "Enemy"));

            _avoidDownObstacles = Physics2D.Raycast(_thirdVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "Enemy"));
        }

        private void CalculateYDifference()
        {
            if (_lastTargetPosition.x > transform.position.x)
            {
                _difference = transform.position.x - _lastTargetPosition.x;
            }
            else
            {
                _difference = transform.position.x - _lastTargetPosition.x;
            }
        }


        #endregion FreezeYActions

        #region FreezeXActions

        private void FreezeXActions()
        {
            _targetPosition = new Vector3(transform.position.x, transform.position.y + _distance, 0);
            
            XVectorsToAvoidObstacles();

            Debug.DrawRay(_firstVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.up).normalized * 0.3f , Color.green);
            Debug.DrawRay(_secondVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.up).normalized * 0.1f , Color.green);
            Debug.DrawRay(_thirdVectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.up).normalized * 0.3f , Color.green);

            XRaycastsToAvoidObstacles();

            if (transform.position == _targetPosition)
            {
                TargetReached();
            }
            else if (_avoidUpObstacles || _avoidMidObstacles || _avoidDownObstacles)
            {
                FlipSpriteAndDistance();
                
                CalculateXDifference();
                
                _targetPosition = _lastTargetPosition;
                _lastTargetPosition = transform.position;
                
                _difference = -(-_distance - _difference);
                _lastTargetPosition = new Vector3(_lastTargetPosition.x, _lastTargetPosition.y + _distance + _difference, 0);
            }
            
        }

        private void XVectorsToAvoidObstacles()
        {
            _firstVectorToAvoidObstacles = new Vector3(transform.position.x + _distanceFront , transform.position.y, 0);
            _secondVectorToAvoidObstacles = new Vector3(transform.position.x, transform.position.y + Mathf.Sign(_distance) * _distanceFront, 0);
            _thirdVectorToAvoidObstacles = new Vector3(transform.position.x - _distanceFront, transform.position.y, 0);
        }

        private void XRaycastsToAvoidObstacles()
        {
            _avoidUpObstacles = Physics2D.Raycast(_firstVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.up).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "SuperRock", "Enemy"));
            
            _avoidMidObstacles = Physics2D.Raycast(_secondVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.up).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "SuperRock", "Enemy"));

            _avoidDownObstacles = Physics2D.Raycast(_thirdVectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.up).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "SuperRock", "Enemy"));
        }

        #endregion FreezeXActions

        private void CalculateXDifference()
        {
            if (_lastTargetPosition.y > transform.position.y)
            {
                _difference = transform.position.y - _lastTargetPosition.y;
            }
            else
            {
                _difference = transform.position.y - _lastTargetPosition.y;
            }
        }

        private void SetDirection()
        {
            _spriteRenderer.flipX = !(_distance > 0);

            if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionY)
            {
                _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
            }
            else if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionX)
            {
                _targetPosition = new Vector3(transform.position.x, transform.position.y + _distance, 0);
            }
        }

        private void TargetReached()
        {
            _targetPosition = _lastTargetPosition;
            _lastTargetPosition = transform.position;
            _distance *= -1;
        }

        private void FlipSpriteAndDistance()
        {
            if (!_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
                _distance = Mathf.Abs(_distance) * -1;
            }
            else
            {
                _spriteRenderer.flipX = true;
                _distance = Mathf.Abs(_distance);
            }
        }

        private void Move()
        {      
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }

        private void Hit()
        {
            _animator.SetBool("HitWithOutFire", true);
        }

        private void RestoreFire()
        {
            _animator.SetBool("RestoreFire", true);
            _animator.SetBool("PutOutFire", false);
        }

        private void SetHitFalse()
        {
            _animator.SetBool("HitWithOutFire", false);
        }

        private IEnumerator DestroySkull()
        {
            _animator.Play("Die");
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.38f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerRespawn>().PlayerDamage();
            }
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("IdleWithFire"))
            {
                if (trigger.transform.CompareTag("Lightning"))
                {
                    _health -= 0.2f;
                } 
                else if (trigger.transform.CompareTag("SuperLightning"))
                {
                    _health -= 0.6f;
                }  
                else if (trigger.transform.CompareTag("SuperWater"))
                {
                    _animator.SetBool("KillWithFire", true);
                    StartCoroutine(DestroySkull());
                }
                else if (trigger.transform.CompareTag("Water"))
                {
                    _animator.SetBool("PutOutFire", true);
                    _animator.SetBool("RestoreFire", false);
                    _health -= 3;
                }
                else if (trigger.transform.CompareTag("Wind"))
                {
                    if (_speed == _auxSpeed)
                    {
                        _speed *= 1.2f;
                        _timeToRestoreSpeed += 1.5f;
                    }
                }
                else if (trigger.transform.CompareTag("SuperWind"))
                {
                    if (_speed == _auxSpeed || _speed == _auxSpeed * 1.2f)
                    {
                        _speed = _auxSpeed * 1.5f;
                        _timeToRestoreSpeed += 1.5f;
                    }
                }
            }
            else
            {
                if (trigger.transform.CompareTag("Shuriken"))
                {
                    _health -= 0.2f;
                    Hit();
                } 
                else if (trigger.transform.CompareTag("Water"))
                {
                    _health -= 1;
                    Hit();
                }
                else if (trigger.transform.CompareTag("SuperWater"))
                {
                    _health -= 2.5f;
                    Hit();
                }
                else if (trigger.transform.CompareTag("Lightning"))
                {
                    _health -= 0.5f;
                    Hit();
                }
                else if (trigger.transform.CompareTag("SuperLightning"))
                {
                    _health -= 1.2f;
                    Hit();
                }
                else if (trigger.transform.CompareTag("Wind") || trigger.transform.CompareTag("SuperWind"))
                {
                    if (_speed == _auxSpeed)
                    {
                        _speed -= _speed / 2;
                        _timeToRestoreSpeed += 1.5f;
                    }
                }
                else if (trigger.transform.CompareTag("Fire") || trigger.transform.CompareTag("SuperFire"))
                {
                    RestoreFire();
                }
            }
            
        }
    }
}