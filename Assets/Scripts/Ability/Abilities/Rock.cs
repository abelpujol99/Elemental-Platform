using UnityEngine;

namespace Ability.Abilities
{
    public class Rock : Ability, AbilityUtility
    {

        public Rock(string tag, GameObject ability, int size, float timer) : base(tag, ability, size, timer)
        {
        }
        
        public void abilityUtility(string tag, Ability ability, float timer, Vector3 position, GameObject character)
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