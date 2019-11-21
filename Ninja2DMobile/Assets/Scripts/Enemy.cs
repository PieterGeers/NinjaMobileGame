using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _target;
    private int _enemyHealth = 2;
    private float _enemySpeed = 4.0f;
    private bool _isAlive = true;

    public void SetVariables(int health, float speed)
    {
        _enemyHealth = health;
        _enemySpeed = speed;
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Shuriken")
        {
            if (_enemyHealth > 1)
            {
                --_enemyHealth;
            }
            else
            {
                _isAlive = false;
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(1, 0) * 100);
                rb.AddTorque(-500f);
                Destroy(gameObject, 5);
                //play dead animation
            }

            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        if (_isAlive)
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _enemySpeed * Time.deltaTime);
    }
}
