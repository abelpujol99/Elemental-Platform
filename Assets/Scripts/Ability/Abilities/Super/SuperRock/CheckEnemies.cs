using System;
using Enemy;
using Enemy.Plant;
using UnityEngine;

namespace Ability.Abilities.Super.SuperRock
{
    public class CheckEnemies : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (!trigger.transform.CompareTag("Enemy"))
            {
                return;
            }
            EnemyWithHealth component = trigger.GetComponent<EnemyWithHealth>();
            component.setHealth(component.getHealth() - component.getHealth());
            
        }
    }
}