using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikanParticleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _leafsParticlePrefab = null;
    [SerializeField]
    private GameObject _sparksParticlePrefab = null;
    [SerializeField]
    private float _destroyTime = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestroyablePole")
        {
            GameObject sparks = Instantiate(_sparksParticlePrefab);
            sparks.transform.position = collision.transform.position;
            sparks.GetComponent<ParticleSystem>().Play();
            Destroy(sparks, _destroyTime);
        }
        if (collision.gameObject.tag == "Foliage")
        {
            Debug.Log("Leafs");
            GameObject Leafs = Instantiate(_leafsParticlePrefab);
            Leafs.transform.position = collision.transform.position;
            Leafs.GetComponent<ParticleSystem>().Play();
            Destroy(Leafs, _destroyTime);
        }
    }
}
