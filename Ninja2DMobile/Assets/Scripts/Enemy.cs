using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float EnemySpeed;
    private Transform _target;
    private bool _isEnemyAlive;
    private int _enemyHealth;
    float randomPosY;
    float randomPosX;
    private TextMeshProUGUI _textLives;
   
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _textLives = gameObject.GetComponentInChildren<TextMeshProUGUI>();


        _isEnemyAlive = false;
        
    }

   

    void Update()
    {
        if (_isEnemyAlive)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, EnemySpeed * Time.deltaTime);
        }
        else
        {
            Respawn();
        }
        
    }

    void Respawn()
    {
        randomPosX = Random.Range(20, 25);
        randomPosY = Random.Range(-5, 10);
        _enemyHealth = Random.Range(3, 8);
        _textLives.text = _enemyHealth.ToString();
        transform.position = new Vector2(randomPosX + _target.position.x, randomPosY + _target.position.y);
        _isEnemyAlive = true;
    }

}
