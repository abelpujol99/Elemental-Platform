﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.transform.CompareTag("CheckGround"))
        {
            CheckGround.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.transform.CompareTag("CheckGround"))
        {
            CheckGround.isGrounded = false;
        }
    }
}
