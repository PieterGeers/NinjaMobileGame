using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    private Rigidbody2D _rb = null;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _timeToDestroy = 10.0f;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /*Function SetDirection sets the travel direction and starts the movement of the shurikan*/
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        _rb.AddForce(direction * _speed);
        Destroy(gameObject, _timeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DestroyablePole")
        {
            Destroy(collision.gameObject);
            Destroy(collision.transform.parent.gameObject, 2f);
            for (int i = 0; i < collision.transform.parent.childCount; ++i)
            {
                BoxCollider2D bc = collision.transform.parent.GetChild(i).GetComponent<BoxCollider2D>();
                if (bc != null)
                    bc.enabled = false;
            }

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void EnableCollider()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        DeleteShurikan();
    //    }
    //}
}
