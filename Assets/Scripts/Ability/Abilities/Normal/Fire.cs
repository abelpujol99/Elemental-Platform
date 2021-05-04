using System;
using UnityEngine;

namespace Ability.Abilities.Normal
{
    public class Fire : Ability
    {

        private Vector3 _characterPosition, _abilityPosition;

        private RaycastHit2D _beforeCollision;

        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            _characterPosition = characterPosition;
            _abilityPosition = abilityPosition;
            
            gameObject.SetActive(true);
            setCast(false);
            UpdateCooldown();
            
            float angle = Mathf.Atan2(_abilityPosition.y - _characterPosition.y, _abilityPosition.x - _characterPosition.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            ability.transform.rotation = targetRotation;
            ability.transform.position = (_abilityPosition - _characterPosition).normalized * 0.3f + _characterPosition;
            ability.GetComponent<Rigidbody2D>().AddForce(new Vector2(_abilityPosition.x - _characterPosition.x, _abilityPosition.y - _characterPosition.y).normalized * 300);
         
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("GreenProjectile"))
            {
                trigger.gameObject.SetActive(false);
            }
            else if (!(trigger.transform.CompareTag("Door") || trigger.transform.CompareTag("Key") || trigger.transform.CompareTag("Upgrade") || trigger.transform.CompareTag("Canvas")))
            {
                gameObject.SetActive(false);
            }
        }
    }
}