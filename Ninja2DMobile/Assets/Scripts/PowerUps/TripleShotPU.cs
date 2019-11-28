using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPU : MonoBehaviour
{
    public PowerUpManager PM;

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            Pickup(Player1.transform.GetComponent<Player>());
            PM.ActiveTripleShot();
        }
    }
    

    private void Pickup(Player player)
    {
        player.TrippleShotPU = true;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject);
    }
}
