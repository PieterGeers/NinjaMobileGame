using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillPU : MonoBehaviour
{
    public PowerUpManager PM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup(collision.transform.GetComponent<Player>());
            PM.ActiveInstaKill();
        }
    }

    private void Pickup(Player player)
    {
        AudioManager.instance.PlaySoundEffect("PowerUp");

        player.InstaKillPU = true;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        Destroy(gameObject);
    }
}
