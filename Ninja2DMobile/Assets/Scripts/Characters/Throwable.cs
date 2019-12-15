using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1100.0f;
    [SerializeField]
    private float _timeToDestroy = 10.0f;

    private Rigidbody2D _rb = null;

    public bool SuperBreakerActive = false;
    public bool InstantKill = false;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /*Function SetDirection sets the travel direction and starts the movement of the shurikan*/
    public void SetDirection(Vector2 direction)
    {
         _rb.AddForce(direction * _speed);
         _rb.AddTorque(10);

        Invoke("DestroyGameObject", _timeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DestroyablePole")
        {
            Destroy(collision.gameObject);
            for (int i = 0; i < collision.transform.parent.childCount; ++i)
            {
                BoxCollider2D bc = collision.transform.parent.GetChild(i).GetComponent<BoxCollider2D>();
                if (bc != null)
                    bc.enabled = false;
            }

            if (!SuperBreakerActive)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                _rb.AddForce(new Vector3(-1, 0, 0) * _speed / 2f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            AudioManager.instance.PlaySoundEffect("Wood");
            CancelInvoke("DestroyGameObject");

            gameObject.transform.parent = collision.transform;
            _rb.simulated = false;
        }
        if (collision.transform.tag == "DefaultPole")
        {
            AudioManager.instance.PlaySoundEffect("Stone");
        }
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

}
