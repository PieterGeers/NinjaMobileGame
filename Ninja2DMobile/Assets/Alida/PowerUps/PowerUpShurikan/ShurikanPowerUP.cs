using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikanPowerUP : MonoBehaviour
{
    private float duration = 4;
    public GameObject shuriken;
    private GameObject[] gos;
    private int array = 10;

    void OnTriggerEnter2D(Collider2D Player1)
    {
        if (Player1.CompareTag("Player"))
        {
            StartCoroutine(Pickup(Player1));

        }
    }

    IEnumerator Pickup(Collider2D player)
    {

        shuriken = GameObject.FindWithTag("Shuriken");

        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        gos = new GameObject[array];
        for (int i = 0; i < array; i++)
        {
            GameObject clone = Instantiate(shuriken, new Vector3((float)i, 1, 0), Quaternion.identity) as GameObject;
            clone.transform.localScale = Vector3.one;
            gos[i] = clone;
        }

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
