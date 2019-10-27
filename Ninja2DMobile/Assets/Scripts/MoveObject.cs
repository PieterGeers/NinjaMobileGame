using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.5f;

    void Update()
    {
            transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
