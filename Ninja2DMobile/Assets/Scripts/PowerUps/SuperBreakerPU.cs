using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBreakerPU : MonoBehaviour
{
    private float duration = 10;
    private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup(collision.transform.GetComponent<Player>());
        }
    }

    private void Pickup(Player player)
    {
        player.SuperBreakerPU = true;
        _player = player;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("Before", duration);

        Destroy(gameObject);
    }

    private void Before()
    {
        _player.SuperBreakerPU = false;
    }
}
