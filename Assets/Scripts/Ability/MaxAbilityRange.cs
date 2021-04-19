using System;
using Character;
using UnityEngine;

namespace Ability.Abilities
{
    public class MaxAbilityRange : MonoBehaviour
    {

        [SerializeField] private Transform _targetPosition;
        
        private void Update()
        {
            transform.position = _targetPosition.position;
        }

        public void SetScale()
        {
            transform.localScale = new Vector3(transform.localScale.x * CharacterScript._maxAbilityRange, transform.localScale.y * CharacterScript._maxAbilityRange, 0);
        }
        
    }
}