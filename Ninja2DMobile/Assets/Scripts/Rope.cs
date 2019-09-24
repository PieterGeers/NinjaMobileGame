using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _hook = null;
    [SerializeField]
    private GameObject _link = null;
    [SerializeField]
    private uint _links = 7;

    private void Awake()
    {
        if (_hook == null)
            throw new System.Exception("_hook = NULL");
        if (_link == null)
            throw new System.Exception("_link = NULL");
    }

    private void Start()
    {
        GenerateRope();
    }


    private void GenerateRope()
    {
        Rigidbody2D previousRB = _hook;
        for (uint i = 0; i < _links; ++i)
        {
            GameObject newLink = Instantiate(_link, transform);
            HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;

            previousRB = newLink.GetComponent<Rigidbody2D>();
        }
    }
}

