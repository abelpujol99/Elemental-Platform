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
        [SerializeField] private Transform _characterPosition;

        [SerializeField] private float _health, _distance, _speed;

        [SerializeField] private Rigidbody2D _rb2D;

        [SerializeField] private Animator _animator;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector3 _targetPosition,
            _lastTargetPosition,
            _vectorToAvoidObstacles,
            _vectorToAvoidFall;

        private RaycastHit2D _lookScenario, _foundPlayer, _avoidFall, _avoidObstacles;

        private Dictionary<string, Action> _onHitEffects;

        private float _initialSpeed, _knockUp, _slow, _distanceToFloor, _distanceFront, _difference, _auxSpeed;

        private bool _onAir, _canMove, _knockedUp;

        private void Start()
        {
            _initialSpeed = _speed;
            _knockUp = 3;
            _slow = 3;
            _distanceToFloor = 0.3f;
            _distanceFront = 0.38f;
            _canMove = true;
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

            CheckHealth();

            CheckDirection();

            CheckObstacle();
            
            CheckFall();
            
            CheckPlayer();

            Move();

        }

        private void CheckDirection()
        {
            _spriteRenderer.flipX = _distance > 0;
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
                return;
            }
            Attack();

        }

        private void Move()
        {
            if (!_canMove)
            {
                return;
            }
            _animator.SetBool("Run", true);         
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }

        private void Hit()
        {
            _animator.SetBool("Hit", true);
            _canMove = false;
            StartCoroutine(SetMove("Hit", 0.25f));
        }

        private void Attack()
        {
            _animator.SetBool("Attack", true);
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

        private IEnumerator DestroyChameleon()
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
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
            if (_speed != _auxSpeed)
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