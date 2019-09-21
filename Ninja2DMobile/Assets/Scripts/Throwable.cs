using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Vector3 _direction = Vector3.zero;
    private bool _directionSet = false;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _timeToDestroy = 10.0f;
    private float _timer = 0.0f;

    /*Function SetDirection sets the travel direction and starts the movement of the shurikan*/
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _directionSet = true;
    }

    /*Function Update translates the shurikan in the correct direction*/
    private void Update()
    {
        if (_directionSet)
        {
            transform.position += _direction * _speed * Time.deltaTime;
            _timer += Time.deltaTime;
            if (_timer >= _timeToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
