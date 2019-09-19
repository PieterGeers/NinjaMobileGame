using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.5f;


    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
