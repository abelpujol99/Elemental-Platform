using System;
using Character;
using UnityEngine;

namespace Scenario
{
    public class Ground : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                CharacterScript.isGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                CharacterScript.isGround = false;
            }
        }
        
    }

}
