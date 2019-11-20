using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikanPowerUP : MonoBehaviour
{
    private float duration = 4;

    //Throwable Powerup = _powerup.GetComponent<Throwable>();


    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            StartCoroutine(Pickup(Player1));
            //Powerup = true;



        }
    }

    IEnumerator Pickup(Collider2D player)
    {

        //shuriken = GameObject.FindWithTag("Shuriken");

        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;


        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
