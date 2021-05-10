using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using TMPro;
using UnityEngine;

namespace Enemy.Ghost
{
    public class Ghost : EnemyWithHealth
    {
        [SerializeField] private float _speed;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private BoxCollider2D _bc2D;
 
        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _character, _transition;

        [SerializeField] private TextMeshProUGUI _TMP;
        
        private Dictionary<string, Action> _onHitEffects;

        private bool _canMove, _immaterial, _stunned;

        private void Start()
        {
            _health = 17;
            _canMove = true;
            CheckDirection();
            FillDictionary();
        }
        
        private void FillDictionary()
        {
            _onHitEffects = new Dictionary<string, Action>()
            {
                {"Player", PlayerAction},
                {"Lightning", LightningAction},
                {"SuperLightning", SuperLightningAction},
                {"Fire", FireAction},
                {"SuperFire", SuperFireAction},
                {"Water", WaterAction},
                {"SuperRock", SuperRockAction},
                {"SuperWater", SuperWaterAction},
                {"SuperWind", SuperWindAction},
                {"Shuriken", ShurikenAction}
            };
        }

        private void Update()
        {
            CheckHealth();
            
            CheckDirection();
            
            Move();
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
            StartCoroutine(DestroyGhost());
        }
        
        private void Move()
        {
            if (!_canMove)
            {  
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _character.transform.position, Time.deltaTime * _speed);
        }
        
        private void CheckDirection()
        {
            _spriteRenderer.flipX = transform.position.x < _character.transform.position.x;
        }
        
        private void Hit()
        {
            _animator.Play("Hit");
            _canMove = false;
            _immaterial = true;
            StartCoroutine(SetMove(0.35f));
        }

        private void Materialize()
        {
            _bc2D.enabled = true;
        }

        private IEnumerator Stunned(float time)
        {
            _canMove = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(SetMove(time));
            yield return new WaitForSeconds(time);
            Hit();
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        private IEnumerator SetMove(float time)
        {
            yield return new WaitForSeconds(time);
            _canMove = true;
            StartCoroutine(Immaterial());
        }

        private IEnumerator DestroyGhost()
        {
            _animator.Play("Die");
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
            _transition.SetActive(true);
            _TMP.text = "To be continued...";
            StartCoroutine(ActivateTransition());

        }

        private IEnumerator ActivateTransition()
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(ShowMessage());
        }

        private IEnumerator ShowMessage()
        {
            yield return new WaitForSeconds(1);
        }

        private IEnumerator Immaterial()
        {
            _animator.SetBool("Immaterial", true);
            _bc2D.enabled = false;
            yield return new WaitForSeconds(2);
            _immaterial = false;
            _animator.SetBool("Immaterial", false);
        }
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (!_onHitEffects.ContainsKey(trigger.transform.tag))
            {
                return;
            }
            _onHitEffects[trigger.transform.tag]();
        }
        
        #region Actions

        private void PlayerAction()
        {
            _character.transform.GetComponent<PlayerRespawn>().PlayerDamage();
        }
        private void ShurikenAction()
        {
            _health -= 0.2f;
            Hit();
        }

        private void SuperRockAction()
        {
            StartCoroutine(Immaterial());
        }

        private void LightningAction()
        {
            _health -= 3f;
            Hit();
        }

        private void SuperLightningAction()
        {
            _health -= 7.5f;
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

        private void SuperWindAction()
        {
            _canMove = false;
            StartCoroutine(Stunned(1));
        }
        
        #endregion Actions
        
    }
}