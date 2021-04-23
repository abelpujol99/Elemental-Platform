using System;
using UnityEngine;

namespace Enemy.Rino
{
    public class RinoCheckGround : MonoBehaviour
    {
        [SerializeField] private Rino _rino;
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Tilemap1"))
            {
                _rino._onAir = false;
                _rino._canMove = true;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Tilemap1"))
            {
                _rino._onAir = true;
                _rino._canMove = false;
            }
        }
    }
}