using System;
using System.Collections;
using CameraController;
using UnityEngine;
using GameController;

namespace Ability.Abilities.Super.SuperRock
{
    public class SuperRock : Ability
    {

        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rb2D;

        private void Update()
        {
            Attack();
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
                _rb2D.isKinematic = true;
                ability.transform.position = abilityPosition;
                ability.transform.rotation = Quaternion.identity;
            }
        }

        private void Attack()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return;
            }
            _rb2D.isKinematic = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        private void Hit()
        {
            _animator.SetBool("Hit", true);
            StartCoroutine(AbilityDisappear());
            //CameraShake();
        }

        private void CameraShake()
        {
            GameController.GameController.Instance._camera.GetComponent<CameraMoves>().Shake(0.15f);
        }

        private void SetHitFalse()
        {
            _animator.SetBool("Hit", false);
        }

        private IEnumerator AbilityDisappear()
        {
            yield return new WaitForSeconds(getTimer());
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_rb2D.velocity.y <= 0.1f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Hit();
            }
        }
    }
}