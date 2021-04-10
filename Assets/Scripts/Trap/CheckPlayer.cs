using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trap
{
    public class CheckPlayer : MonoBehaviour
    {
        public Animator animator;
        private bool active;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                active = true;
                animator.SetBool("Active", active);
                gameObject.transform.GetChild(0).gameObject.SetActive(active);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                active = false;
                animator.SetBool("Active", active);
                gameObject.transform.GetChild(0).gameObject.SetActive(active);
            }
        }
    }

}
