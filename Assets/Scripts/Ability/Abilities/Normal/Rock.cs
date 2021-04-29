using System;
using System.Collections;
using System.Collections.Generic;
using Scenario;
using UnityEngine;

namespace Ability.Abilities.Normal
{
    public class Rock : Ability
    {

        private Rigidbody2D _rockRigidbody2D;

        private Dictionary<string, Action> _onEnterEffects, _onExitEffects, _onStayEffects;

        private int _rotationZ;

        private float _maxSpeed, _stopTime, _timeToStop, _velocity, _speed;

        private bool _pulling, _canMove;

        private void Start()
        {
            _rockRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _rotationZ = 0;
            _maxSpeed = 0.5f;
            _stopTime = 1;
            FillDictionary();
        }
        
        private void FillDictionary()
        {
            _onEnterEffects = new Dictionary<string, Action>()
            {
                {"Player", PlayerEnterAction}
            };

            _onExitEffects = new Dictionary<string, Action>()
            {
                {"Player", PlayerExitAction},
                {"Tilemap1", Tilemap1ExitAction}
            };
            
            _onStayEffects = new Dictionary<string, Action>()
            {
                {"Tilemap1", Tilemap1StayAction}
            };

        }

        private void Update()
        {
            if (!_pulling)
            {
                _velocity = _rockRigidbody2D.velocity.x;
                
                if (_rockRigidbody2D.velocity.x > 0)
                {
                    _velocity -= Time.deltaTime;
                }
                else if(_rockRigidbody2D.velocity.x < 0)
                {
                    _velocity += Time.deltaTime;
                }
                
                _rockRigidbody2D.velocity = new Vector2(_velocity, _rockRigidbody2D.velocity.y);
                if (!(Mathf.Abs(_velocity) < 0.01f))
                {
                    return;
                }
                _rockRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                _rockRigidbody2D.constraints = RigidbodyConstraints2D.None;
            }

        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(_rockRigidbody2D.velocity.x) > _maxSpeed)
            {
                _rockRigidbody2D.velocity = _rockRigidbody2D.velocity.normalized * _maxSpeed;
            }
        }

        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            float _abilityRange = (abilityPosition - characterPosition).magnitude;
            
            RaycastHit2D hit = Physics2D.Raycast(characterPosition, abilityPosition - characterPosition, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

            if (!hit && maxAbilityRange >= _abilityRange)
            {
                setCast(false);
                UpdateCooldown();
                gameObject.SetActive(true);
                ability.transform.position = abilityPosition;
                ability.transform.rotation = Quaternion.identity;
            }
        }

        #region Actions

        private void PlayerEnterAction()
        {
            _pulling = true;
        }
        
        private void PlayerExitAction()
        {
            _pulling = false;
        }

        private void Tilemap1ExitAction()
        {
            _canMove = false;
        }

        private void Tilemap1StayAction()
        {
            _canMove = true;
        }

        #endregion Actions

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_onEnterEffects.ContainsKey(collision.transform.tag))
            {
                return;
            }
            _onEnterEffects[collision.transform.tag]();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!_onExitEffects.ContainsKey(collision.transform.tag))
            {
                return;
            }
            _onExitEffects[collision.transform.tag]();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_onStayEffects.ContainsKey(collision.transform.tag))
            {
                return;
            }
            _onStayEffects[collision.transform.tag]();
        }
    }
}