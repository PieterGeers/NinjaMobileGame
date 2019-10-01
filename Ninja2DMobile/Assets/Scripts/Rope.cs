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

    private Rigidbody2D _previousRB = null;
    private bool _initialized = false;

    private void Awake()
    {
        if (_hook == null)
            throw new System.Exception("_hook = NULL");
        if (_link == null)
            throw new System.Exception("_link = NULL");
        _previousRB = _hook;
    }

    private void Start()
    {
        if (_links > 0)
            GenerateRope();
    }


    private void GenerateRope()
    {
        for (uint i = 0; i < _links; ++i)
        {
            Transform trans = transform;
            trans.position = _previousRB.transform.position;
            GameObject newLink = Instantiate(_link, trans);
            HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();
            joint.connectedBody = _previousRB;

            _previousRB = newLink.GetComponent<Rigidbody2D>();
        }
        _initialized = true;
    }

    public Rigidbody2D GetLastLink()
    {
        return _previousRB;
    }

    public bool IsInitialized()
    {
        return _initialized;
    }

    public void GenerateRope(uint nb)
    {
        _links = nb;
        GenerateRope();
    }
}

