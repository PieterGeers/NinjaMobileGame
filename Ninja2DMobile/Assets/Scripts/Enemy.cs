using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _target;
    private int _enemyHealth = 2;
    private float _enemySpeed = 4.0f;
    private bool _isAlive = true;
    private Animator _animator = null;

    public void SetVariables(int health, float speed)
    {
        _enemyHealth = health;
        _enemySpeed = speed;
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Shuriken")
        {
            if (_enemyHealth > 1)
            {
                --_enemyHealth;
                GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                Invoke("Default", 0.2f);
            }
            else
            {
                AudioManager.instance.PlaySoundEffect("EnemyDead");
                _isAlive = false;
                _animator.SetBool("Dead", true);
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(1, 0) * 100);
                rb.AddTorque(-500f);
                Destroy(gameObject, 5);
            }

            Destroy(collision.gameObject);
        }
    }

    private void Default()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    void Update()
    {
        if (_isAlive)
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _enemySpeed * Time.deltaTime);
    }
}
