using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakeJumpUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        Destroy(gameObject);
        CharacterScript.jumpUpgrade = true;

    }
}
