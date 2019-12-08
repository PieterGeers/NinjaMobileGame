using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBreakerPU : MonoBehaviour
{
    private float duration = 10;
    private Player _player;

    public PowerUpManager PM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup(collision.transform.GetComponent<Player>());
            PM.ActiveSuperBreaker();
        }
    }

    private void Pickup(Player player)
    {
        AudioManager.instance.PlaySoundEffect("PowerUp");

        player.SuperBreakerPU = true;
        _player = player;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("Before", duration);

    }

    private void Before()
    {
        PM.InactiveSuperBreaker();
        _player.SuperBreakerPU = false;
        Destroy(gameObject);
    }
}
