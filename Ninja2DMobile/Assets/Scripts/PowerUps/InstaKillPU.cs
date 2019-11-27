using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillPU : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup(collision.transform.GetComponent<Player>());
        }
    }

    private void Pickup(Player player)
    {
        player.InstaKillPU = true;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        Destroy(gameObject);
    }
}
