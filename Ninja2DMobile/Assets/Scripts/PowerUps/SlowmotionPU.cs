using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmotionPU : MonoBehaviour
{
   
    private float duration = 1;
    public PowerUpManager PM;

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            Pickup();
            PM.ActiveSlowMotion();
        }
    }

    private void Pickup()
    {
        AudioManager.instance.PlaySoundEffect("PowerUp");

        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("before", duration);
     }

    private void before()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        PM.InactiveSlowMotion();
        Destroy(gameObject);
    }
}

