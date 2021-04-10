using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Character
{
    public class CheckGround : MonoBehaviour
    {
        //[SerializeField] private TilemapCollider2D tilemap;
        public static bool isGrounded;

        /*private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger == tilemap)
            {
                Debug.Log("suelo");
                isGrounded = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger == tilemap)
            {
                Debug.Log("no suelo");
                isGrounded = false;
            }
        }*/
    }

}

