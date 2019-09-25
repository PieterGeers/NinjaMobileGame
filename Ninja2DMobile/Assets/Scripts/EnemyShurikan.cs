using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShurikan : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform _character;
    private Vector2 _target;

    void Start()
    {
        _character = GameObject.FindGameObjectWithTag("Player").transform;
        _target = new Vector2(_character.position.x, _character.position.y);
    }

   
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _character.position, _speed * Time.deltaTime);

        if (transform.position.x == _target.x && transform.position.y == _target.y)
        {
            DeleteShurikan();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DeleteShurikan();
        }
    }

    void DeleteShurikan()
    {
        Destroy(gameObject);
    }
}
