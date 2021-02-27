using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakeUpgrade : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        Destroy(gameObject);
        CharacterScript.jumpUpgrade = true;

    }
}
