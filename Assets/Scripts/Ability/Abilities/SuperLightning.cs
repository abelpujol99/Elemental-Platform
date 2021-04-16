using UnityEngine;

namespace Ability.Abilities
{
    public class SuperLightning : Ability
    {
        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            float _abilityRange = (abilityPosition - characterPosition).magnitude;
            
            RaycastHit2D hit = Physics2D.Raycast(characterPosition, abilityPosition - characterPosition, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

            if (!hit && maxAbilityRange >= _abilityRange)
            {
                setCast(false);
                gameObject.SetActive(true);
                ability.transform.position = abilityPosition;
                ability.transform.rotation = Quaternion.identity;
            }
        }
    }
}