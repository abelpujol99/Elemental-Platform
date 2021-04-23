using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Trap
{
    public class DamageObject : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().PlayerDamage();
            }
        }

    }

}
