using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy
{
    public class Skull : MonoBehaviour
    {
        [SerializeField] private float _health, _speed, _distance;
        
        [SerializeField] private Animator _animator;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private RaycastHit2D _avoidObstacles;

        private Vector3 _targetPosition, _lastTargetPosition, _vectorToAvoidObstacles;

        private float _auxSpeed, _timeToRestoreFire, _auxTimeToRestoreFire, _timeToRestoreSpeed, _distanceFront, _difference;

        private void Start()
        {
            _auxSpeed = _speed;
            _timeToRestoreFire = 5;
            _auxTimeToRestoreFire = _timeToRestoreFire;
            _distanceFront = 0.25f;
            if (_distance > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
            _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
            _lastTargetPosition = transform.position;
        }

        private void Update()
        {
            if (_health <= 0)
            {
                StartCoroutine(DestroySkull());
            }
            
            if (_distance > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }

            if (_timeToRestoreSpeed > 0)
            {
                _timeToRestoreSpeed -= Time.deltaTime;
            }
            else
            {
                _speed = _auxSpeed;
            }
            
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("IdleWithFire"))
            {
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
            
            _vectorToAvoidObstacles = new Vector3(transform.position.x + Mathf.Sign(_distance) * _distanceFront, transform.position.y - 0.1f, 0);

            Debug.DrawRay(_vectorToAvoidObstacles, (Mathf.Sign(_distance) * Vector2.right).normalized * 0.1f , Color.green);

            _avoidObstacles = Physics2D.Raycast(_vectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap1" ,"Tilemap2", "Rock", "Enemy"));

            if (transform.position == _targetPosition)
            {
                _targetPosition = _lastTargetPosition;
                _lastTargetPosition = transform.position;
                _distance *= -1;
            }
            else if (_avoidObstacles)
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
                _lastTargetPosition = transform.position;
            }
            
            Move();
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