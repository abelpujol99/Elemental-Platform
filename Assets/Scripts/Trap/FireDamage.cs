using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Trap
{
    public class FireDamage : MonoBehaviour
    {
        private bool active;
        public void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Player"))
            {
                StartCoroutine(destroyPlayer(trigger));
            }
        }

        private IEnumerator destroyPlayer(Collider2D trigger)
        {
            yield return new WaitForSeconds(0.35f);
            trigger.transform.GetComponent<PlayerRespawn>().PlayerDamage();
        }
    }

}
