﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikanPowerUP : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            Pickup(Player1.transform.GetComponent<Player>());
        }
    }
    

    private void Pickup(Player player)
    {
        Debug.Log("Picked up");

        player.PowerUpshurikan = true;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject);
    }
}
