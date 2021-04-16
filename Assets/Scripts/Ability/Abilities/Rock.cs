using UnityEngine;

namespace Ability.Abilities
{
    public class Rock : Ability
    {

        public Rock(string tag, GameObject ability, int size, float timer, bool cast, float cooldown) : base(tag, ability, size, timer, cast, cooldown)
        {
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