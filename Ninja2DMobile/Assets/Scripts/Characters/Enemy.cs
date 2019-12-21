﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _target;
    private int _enemyHealth = 2;
    private float _enemySpeed = 4.0f;
    private bool _isAlive = true;
    private Animator _animator = null;
    [SerializeField]
    private List<GameObject> _lives = null;


    public void SetVariables(int health, float speed)
    {
        _enemyHealth = health;
        _enemySpeed = speed;
        if (health == 2)
        {
            _lives[0].SetActive(false);
            _lives.RemoveAt(0);
        }
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
            if (collision.GetComponent<Throwable>().InstantKill)
            {
                int count = _lives.Count;
                for (int i = 0; i < count; ++i)
                {
                    RemoveLive();
                }
                Dead();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().InstaKillPU = false;
                FindObjectOfType<PowerUpManager>().InactiveInstaKill();
            }
            else if (_enemyHealth > 1)
            {
                RemoveLive();
                --_enemyHealth;
                GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                Invoke("Default", 0.2f);
            }
            else
            {
                RemoveLive();
                Dead();
            }

            Destroy(collision.gameObject);
        }
    }

    private void RemoveLive()
    {
        _lives[0].GetComponent<Rigidbody2D>().simulated = true;
        _lives.RemoveAt(0);
    }

    private void Dead()
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
