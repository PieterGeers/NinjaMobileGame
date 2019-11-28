using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _followObject = null;
    private float _offset = -5;

    private void Start()
    {
        _offset = transform.position.x - _followObject.position.x;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(_followObject.position.x + _offset, transform.position.y, transform.position.z);
    }
}
