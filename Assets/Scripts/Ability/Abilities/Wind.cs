using UnityEngine;

namespace Ability.Abilities
{
    public class Wind : Ability
    {
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            gameObject.SetActive(false);
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