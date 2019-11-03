using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmotion : MonoBehaviour
{

    private float duration = 4;

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
           StartCoroutine(Pickup(Player1));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {

        Player speed = player.GetComponent<Player>();
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(duration);

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F;

        Destroy(gameObject);
    }

}
