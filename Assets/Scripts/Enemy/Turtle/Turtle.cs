using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy.Turtle
{
    public class Turtle : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private Rigidbody2D _rb2D;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Transform _characterPosition;
        
        private float _knockUp;

        private bool _spikes;
        
        private bool _onAir, _stuned;

        private void Start()
        {
            _knockUp = 4;
            
            if (_characterPosition.position.x > transform.position.x)
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
            if (!_onAir && !_stuned)
            {
                
                if (_characterPosition.position.x > transform.position.x)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                }
            }
            else
            {
                _stuned = true;
                _animator.Play("IdleWithOutSpikes");
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.transform.CompareTag("Tilemap1"))
            {
                if (_onAir)
                {
                    _onAir = false;
                    StartCoroutine(StunTime());
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Rock"))
            {
                StartCoroutine(DisableSpikes());
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player") && _animator.GetCurrentAnimatorStateInfo(0).IsName("IdleWithSpikes"))
            {
                collision.transform.GetComponent<PlayerRespawn>().PlayerDamage();
            }
            else if (collision.gameObject.CompareTag("Rock"))
            {
                _animator.SetBool("SpikesIn", false);
                _animator.SetBool("SpikesOut", true);
            }
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("SuperWind"))
            {
                _rb2D.velocity = new Vector2(0, _knockUp);
                _onAir = true;
            }
            else if (trigger.transform.CompareTag("CheckGround"))
            {
                CharacterScript.isGround = true;
                _animator.SetBool("SpikesIn", false);
                _animator.SetBool("SpikesOut", true);
                StartCoroutine(DisableSpikes());
            } 
            else if (trigger.gameObject.CompareTag("Shuriken") || trigger.gameObject.CompareTag("Water") || trigger.gameObject.CompareTag("Fire") || trigger.gameObject.CompareTag("Wind") || trigger.gameObject.CompareTag("Lightning"))
            {
                _animator.SetBool("SpikesIn", false);
                _animator.SetBool("SpikesOut", true);
                StartCoroutine(DisableSpikes());
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                CharacterScript.isGround = false;
            }
        }

        private IEnumerator DisableSpikes()
        {
            yield return new WaitForSeconds(2f);
            _animator.SetBool("SpikesOut", false);
            _animator.SetBool("SpikesIn", true);
        }

        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(2f);
            _stuned = false;
        }
    }
}