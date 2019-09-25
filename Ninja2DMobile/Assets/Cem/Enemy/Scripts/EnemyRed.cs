using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRed : MonoBehaviour
{
    //Movement
    [SerializeField] private float _speed;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _backOffDistance;
    public Transform Character;

    //Shooting
    private float _shootingTime;
    public float _startShootingTime;
    public GameObject Shurikans;


    void Start()
    {
        Character = GameObject.FindGameObjectWithTag("Player").transform;
        _speed = 10.0f;
        _stopDistance = 20.0f;
        _backOffDistance = 10.0f;

        _shootingTime = _startShootingTime;
    }

    void Update()
    {
        //check distance between enemy & character
        if (Vector2.Distance(transform.position,Character.position) > _stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position,Character.position,_speed * Time.deltaTime);
        }

        else if (Vector2.Distance(transform.position, Character.position) < _stopDistance
                 && Vector2.Distance(transform.position, Character.position) > _backOffDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, Character.position) < _backOffDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, Character.position, -_speed * Time.deltaTime);
        }



        if (_shootingTime <= 0)
        {
            Instantiate(Shurikans, transform.position, Quaternion.identity);
            _shootingTime = _startShootingTime;
        }
        else
        {
            _shootingTime -= Time.deltaTime;
        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
        
    //}
}
