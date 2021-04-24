using UnityEngine;

namespace Ability.Abilities.Super
{
    public class SuperLightning : Ability
    {
        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            setCast(false);
            UpdateCooldown();
            gameObject.SetActive(true);
            ability.transform.position = abilityPosition;
            ability.transform.rotation = Quaternion.identity;
        }
    }
}