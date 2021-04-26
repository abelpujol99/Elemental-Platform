using System;
using System.Collections;
using UnityEngine;

namespace Enemy.Turtle
{
    public class HitTurtle : MonoBehaviour
    {
        [SerializeField] private GameObject _turtle;
        [SerializeField] private Animator _turtleAnimator;
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Fire") || trigger.transform.CompareTag("Lightning") || trigger.transform.CompareTag("Shuriken"))
            {
                StartCoroutine(DestroyTurtle());
            }
        }

        private IEnumerator DestroyTurtle()
        {
            _turtleAnimator.Play("Die");
            yield return new WaitForSeconds(0.35f);
            Destroy(_turtle);
        }
        
    }
}