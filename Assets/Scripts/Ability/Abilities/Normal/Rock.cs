using System;
using System.Collections;
using Scenario;
using UnityEngine;

namespace Ability.Abilities.Normal
{
    public class Rock : Ability
    {

        private Rigidbody2D _rockRigidbody2D;

        private int _rotationZ;

        private void Start()
        {
            _rockRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _rotationZ = 0;
        }

        public override void abilityUtility(GameObject ability, Vector3 abilityPosition, Vector3 characterPosition, float maxAbilityRange)
        {
            float _abilityRange = (abilityPosition - characterPosition).magnitude;
            
            RaycastHit2D hit = Physics2D.Raycast(characterPosition, abilityPosition - characterPosition, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

            if (!hit && maxAbilityRange >= _abilityRange)
            {
                setCast(false);
                gameObject.SetActive(true);
                StartCoroutine(FreezePositionX(ability));
                ability.transform.position = abilityPosition;
                ability.transform.rotation = Quaternion.identity;
            }
        }

        private IEnumerator FreezePositionX(GameObject ability)
        {
            _rockRigidbody2D = ability.GetComponent<Rigidbody2D>();
            
            _rockRigidbody2D.constraints = RigidbodyConstraints2D.None;
            yield return new WaitForSeconds(1);
            
            _rockRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        
    }
}