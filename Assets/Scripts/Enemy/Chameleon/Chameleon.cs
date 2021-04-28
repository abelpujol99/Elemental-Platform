using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Enemy.Turtle;
using UnityEngine;
using UnityEngine.Timeline;

namespace Enemy.Chameleon
{
    public class Chameleon : MonoBehaviour
    {
        [SerializeField] private Transform _characterTransform;

        [SerializeField] private float _health, _distance, _speed;

        [SerializeField] private bool _camouflage;

        [SerializeField] private Rigidbody2D _rb2D;

        [SerializeField] private Animator _animator;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Color _opacity;

        private Vector3 _targetPosition,
            _lastTargetPosition,
            _vectorToAvoidObstacles,
            _vectorToAvoidFall;

        private RaycastHit2D _lookScenario, _foundPlayer, _avoidFall, _avoidObstacles, _characterCloser;

        private Dictionary<string, Action> _onHitEffects;

        private float _initialSpeed, _knockUp, _slow, _distanceToFloor, _distanceFront, _difference;

        private bool _onAir, _knockedUp;

        private void Start()
        {
            _initialSpeed = _speed;
            _knockUp = 3;
            _slow = 3;
            _distanceToFloor = 0.3f;
            _distanceFront = 0.38f;
            _opacity = _spriteRenderer.color;
            CheckDirection();
            FillDictionary();
        }
        
        private void FillDictionary()
        {
            _onHitEffects = new Dictionary<string, Action>()
            {
                {"Lightning", LightningAction},
                {"SuperLightning", SuperLightningAction},
                {"Fire", FireAction},
                {"SuperFire", SuperFireAction},
                {"Water", WaterAction},
                {"SuperWater", SuperWaterAction},
                {"Wind", WindAction},
                {"SuperWind", SuperWindAction},
                {"Shuriken", ShurikenAction}
            };
        }
        
        private void Update()
        {
            Debug.DrawRay(transform.position, (Mathf.Sign(_distance) * Vector2.right) * 0.38f, Color.green);

            Camouflage();

            CheckHealth();

            CheckDirection();

            CheckObstacle();
            
            CheckFall();
            
            CheckPlayer();

            CheckAttack();

            Move();

        }

        private void Camouflage()
        {
            if (_camouflage)
            {
                _speed = 0;
                _characterCloser = Physics2D.Raycast(transform.position, _characterTransform.position - transform.position, 1f, LayerMask.GetMask("Player"));
                
                if (_characterCloser)
                {
                    _opacity.a = 0.2f;
                }
                else
                {
                    _opacity.a = 0f;
                }
                
                _spriteRenderer.color = _opacity;
                
            }
            else
            {
                _speed = _initialSpeed;
                _opacity.a = 1f;

                _spriteRenderer.color = _opacity;
            }
        }
        
        private void Move()
        {
            if (_speed == 0 && _camouflage)
            {
                return;
            }
            _animator.SetBool("Run", true);         
            Debug.Log(_targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }
        
        private void CheckHealth()
        {
            if (_health > 0)
            {
                return;
            }
            _animator.Play("Die");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            StartCoroutine(DestroyChameleon());
        }
        private void CheckDirection()
        {
            _spriteRenderer.flipX = _distance > 0;
        }

        private void CheckObstacle()
        {
            _avoidObstacles = Physics2D.Raycast(_vectorToAvoidObstacles, 
                (Mathf.Sign(_distance) * Vector2.right).normalized,
                0.1f, LayerMask.GetMask("Tilemap2", "Rock", "Enemy"));
        }

        private void CheckFall()
        {
            _avoidFall = Physics2D.Raycast(_vectorToAvoidFall, (Mathf.Sign(_distance) * Vector2.right + Vector2.down).normalized,
                0.14f,
                LayerMask.GetMask("Tilemap1"));
        }

        private void CheckPlayer()
        {
            _foundPlayer = Physics2D.Raycast(transform.position, (Mathf.Sign(_distance) * Vector2.right), 0.38f,
                LayerMask.GetMask("Player"));

            if (!_foundPlayer)
            {
                _animator.SetBool("Attack", false);
                return;
            }
            Attack();

        }

        private void Hit()
        {
            _animator.SetBool("Hit", true);
            _speed = 0;
            StartCoroutine(SetMove("Hit", 0.25f));
        }

        private void Attack()
        {
            Debug.Log("ataca");
            _animator.SetBool("Attack", true);
            _speed = 0;
            _camouflage = false;
        }

        private void CheckAttack()
        {
            if (_camouflage || !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }
            _speed = 0;
        }

        private void Fall()
        {
            _animator.SetBool("Fall", true);
            _speed = 0;
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
            _speed = _initialSpeed;
        }

        private IEnumerator DestroyChameleon()
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                return;
            }
            _onHitEffects[trigger.transform.tag]();
        }

        #region Actions
        private void ShurikenAction()
        {
            _health -= 0.2f;
            Hit();
        }

        private void LightningAction()
        {
            _health -= 3f;
            Hit();
        }

        private void SuperLightningAction()
        {
            _health -= 5.5f;
            Hit();
        }

        private void FireAction()
        {
            _health -= 2.5f;
            Hit();
        }

        private void SuperFireAction()
        {
            _health -= 4.3f;
            Hit();
        }

        private void WaterAction()
        {
            _health -= 1.3f;
            Hit();
        }

        private void SuperWaterAction()
        {
            _health -= 3f;
            Hit();
        }

        private void WindAction()
        {
            if (_speed != _initialSpeed)
            {
                return;
            }
            _speed -= _speed / _slow;
            StartCoroutine(ReturnSpeed());
        }

        private void SuperWindAction()
        {
            _rb2D.velocity = new Vector2(0, _knockUp);
            _knockedUp = true;
        }
        
        #endregion Actions
        
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
    }
}