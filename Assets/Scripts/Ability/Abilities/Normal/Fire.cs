using System;
using UnityEngine;

namespace Ability.Abilities.Normal
{
    public class Fire : Ability
    {

        private Vector3 _characterPosition, _abilityPosition;

        private RaycastHit2D _beforeCollision;

        /*private void Update()
        {
            _beforeCollision = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 10f);

            if (_beforeCollision.collider.name == "Bullet")
            {
                _beforeCollision.collider.gameObject.SetActive(false);
            }

        }*/

        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            _characterPosition = characterPosition;
            _abilityPosition = abilityPosition;
            
            gameObject.SetActive(true);
            setCast(false);
            
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
            else if (!trigger.transform.CompareTag("GreenProjectile"))
            {
                gameObject.SetActive(false);
            }
        }
        
        public string getTag()
        {
            return _tag;
        }

        public GameObject getAbility()
        {
            return _ability;
        }

        public int getSize()
        {
            return _size;
        }

        public float getTimer()
        {
            return _timer;
        }

        public bool isCast()
        {
            return _cast;
        }

        public float getCooldown()
        {
            return _cooldown;
        }

        public void setSize(int size)
        {
            _size = size;
        }

        public void setTimer(float timer)
        {
            _timer = timer;
        }

        public void setCast(bool cast)
        {
            _cast = cast;
        }

        public void setCoolDown(float cooldown)
        {
            _cooldown = cooldown;
        }
    }
}