using System;
using UnityEngine;

namespace Ability.Abilities
{
    public class Rock : Ability
    {

        private void Start()
        {
            
            _tag = getTag();
            _ability = getAbility();
            _size = getSize();
            _timer = getTimer();
            _cast = isCast();
            _cooldown = getCooldown();
        }


        public override void abilityUtility(GameObject ability, Vector3 position, GameObject character)
        {
            if (transform.position.x < position.x)
            {
                character.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                character.GetComponent<SpriteRenderer>().flipX = true;
            }
            ability.transform.position = position;
            ability.transform.rotation = Quaternion.identity;
        }
    }
}