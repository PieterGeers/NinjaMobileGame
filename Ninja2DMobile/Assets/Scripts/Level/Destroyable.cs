using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField]
    private Rope _rope = null;
    [SerializeField]
    private GameObject _weight = null;

    private bool _initialized = false;

    private void Awake()
    {
        if (_rope == null)
            throw new System.Exception("_rope = NULL");
        if (_weight == null)
            throw new System.Exception("_weight = NULL");
    }

    private void Update()
    {
        if (!_initialized && _rope.IsInitialized())
        {
            GameObject weight = Instantiate(_weight, transform);
            HingeJoint2D poleJoint = weight.GetComponent<HingeJoint2D>();
            poleJoint.connectedBody = _rope.GetLastLink();
            _initialized = true;
        }
    }
}
