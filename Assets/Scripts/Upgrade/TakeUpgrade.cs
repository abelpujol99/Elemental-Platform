using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using UnityEngine.Serialization;

namespace Upgrade
{
    public class TakeUpgrade : MonoBehaviour
    {
        [SerializeField] private CharacterScript.Abilities ability;
        [SerializeField] private CharacterScript player;

        private void Start()
        {
            player.DeactivateAbility(ability);
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (!trigger.CompareTag("Player"))
            {
                return;
            }
            Destroy(gameObject);
            player.ActiveAbility(ability);
        }
    }
}

