using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmotion : MonoBehaviour
{
   
    private float duration = 1;

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Pickup()
    {

        Time.timeScale = 0.5f;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("before", duration);

     }

    private void before()
    {

        Time.timeScale = 1;
        Destroy(gameObject);
    }
}

