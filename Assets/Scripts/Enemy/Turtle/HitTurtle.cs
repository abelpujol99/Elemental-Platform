using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Enemy.Turtle
{
    public class HitTurtle : MonoBehaviour
    {
        [SerializeField] private GameObject _turtle;
        [SerializeField] private Animator _turtleAnimator;
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.gameObject.CompareTag("Player"))
            {
                trigger.gameObject.GetComponent<PlayerRespawn>().PlayerDamage();
            }
            else if (trigger.transform.CompareTag("Fire") || trigger.transform.CompareTag("Lightning") || trigger.transform.CompareTag("Shuriken") || trigger.transform.CompareTag("GreenProjectile"))
            {
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(_turtle.GetComponent<Turtle>().DestroyTurtle());
            }
        }
    }
}