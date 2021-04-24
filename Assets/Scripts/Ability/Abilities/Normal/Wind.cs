using System;
using UnityEngine;

namespace Ability.Abilities.Normal

{
    public class Wind : Ability
    {
        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            gameObject.SetActive(true);
            setCast(false);
            UpdateCooldown();
            
            float angle = Mathf.Atan2(abilityPosition.y - characterPosition.y, abilityPosition.x - characterPosition.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            ability.transform.position = (abilityPosition - characterPosition).normalized * 0.3f + characterPosition;
            ability.transform.rotation = targetRotation;
            ability.GetComponent<Rigidbody2D>().AddForce(new Vector2(abilityPosition.x - characterPosition.x, abilityPosition.y - characterPosition.y).normalized * 300);
         
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("GreenProjectile"))
            {
                trigger.gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity * 1; 
                gameObject.SetActive(false);
            }
            else if (!(trigger.transform.CompareTag("Door") || trigger.transform.CompareTag("Key") || trigger.transform.CompareTag("Upgrade")))
            {
                gameObject.SetActive(false);
            }
        }
    }
}