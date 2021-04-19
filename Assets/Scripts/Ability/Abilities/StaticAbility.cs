using UnityEngine;

namespace Ability.Abilities
{
    public class StaticAbility : Ability
    {
        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            setCast(false);
            gameObject.SetActive(true);
            ability.transform.position = abilityPosition;
            ability.transform.rotation = Quaternion.identity;
        }
    }
}