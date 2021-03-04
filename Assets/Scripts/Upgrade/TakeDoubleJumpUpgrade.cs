using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDoubleJumpUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        Destroy(gameObject);
        CharacterScript.doubleJumpUpgrade = true;

    }
}
