using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(destroyPlayer(collision));
        }
    }

    private IEnumerator destroyPlayer(Collider2D collision)
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(collision.gameObject);
    }
}
