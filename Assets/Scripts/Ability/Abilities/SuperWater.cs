using System;
using Character;
using UnityEngine;

namespace Ability.Abilities
{
    public class SuperWater : Ability
    {

        private float visibleTime;
        private float cooldownTime;

        private void Start()
        {
            Debug.Log(getTimer() + " Timer");
        }

        private void Update()
        {
            if (visibleTime == 0f)
            {
                Debug.Log("Disponible");
                gameObject.SetActive(false);
            }
            else 
            {
                Debug.Log("No Disponible");
                visibleTime -= Time.deltaTime * 1;
            }

            if (cooldownTime == 0)
            {
                setCast(true);
            }
            else
            {
                cooldownTime -= Time.deltaTime * 1;
            }
            
        }

        public override void abilityUtility(GameObject ability, Vector3 position, GameObject character)
        {
            if (isCast())
            {
                float xPositionSpawn;
            
                if (character.transform.position.x < position.x)
                {
                    xPositionSpawn = 0.3f;
                }
                else
                {
                    xPositionSpawn = -0.3f;
                }
                float angle = Mathf.Atan2(position.y - character.transform.position.y, position.x - character.transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                ability.transform.position = new Vector3( character.transform.position.x + xPositionSpawn, character.transform.position.y, 0);
                //abilityToSpawn.transform.position = transform.position;
                ability.transform.rotation = targetRotation;
                ability.GetComponent<Rigidbody2D>().AddForce(new Vector2(position.x - character.transform.position.x, position.y - character.transform.position.y).normalized * 300);

                visibleTime = getTimer();
                cooldownTime = getCooldown();
                setCast(false);

            }
            
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