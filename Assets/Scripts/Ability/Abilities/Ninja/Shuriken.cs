using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Ability.Abilities.Ninja
{
    public class Shuriken : Ability
    {
        private void Update()
        {
            transform.Rotate(Vector3.back * -10);
        }

        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            gameObject.SetActive(true);
            setCast(false);
            
            float angle = Mathf.Atan2(abilityPosition.y - characterPosition.y, abilityPosition.x - characterPosition.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            ability.transform.position = (abilityPosition - characterPosition).normalized * 0.3f + characterPosition;
            ability.transform.rotation = targetRotation;
            ability.GetComponent<Rigidbody2D>().AddForce(new Vector2(abilityPosition.x - characterPosition.x, abilityPosition.y - characterPosition.y).normalized * 300);
         
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (!(trigger.transform.CompareTag("Door") || trigger.transform.CompareTag("Key") || trigger.transform.CompareTag("Upgrade")
                  || trigger.transform.CompareTag("Canvas") || trigger.transform.CompareTag("Platform")))
            {
                gameObject.SetActive(false);
            }
        }
    }
}