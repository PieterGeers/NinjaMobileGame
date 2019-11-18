using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikanPowerUP : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            Pickup(Player1);
        }


    }

    void Pickup(Collider2D player)
    {
       Debug.Log("Picked Up.");

        //empty

        Destroy(gameObject);
    }


}
