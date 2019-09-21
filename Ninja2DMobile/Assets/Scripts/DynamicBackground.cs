using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _layer1 = null;
    [SerializeField]
    private List<GameObject> _layer2 = null;
    [SerializeField]
    private List<GameObject> _layer3 = null;
    [SerializeField]
    private List<GameObject> _layer4 = null;

    [SerializeField]
    private float _speed = 0;
    [SerializeField]
    private float _modifier1 = 0;
    [SerializeField]
    private float _modifier2 = 0;
    [SerializeField]
    private float _modifier3 = 0;
    [SerializeField]
    private float _modifier4 = 0;

    [SerializeField]
    private GameState _gameState = null;

    private Camera _mainCam = null;

    private void Awake()
    {
        if (_layer1 == null)
            throw new System.Exception("_layer1 = NULL");
        else if (_layer2 == null)
            throw new System.Exception("_layer2 = NULL");
        else if (_layer3 == null)
            throw new System.Exception("_layer3 = NULL");
        else if (_layer4 == null)
            throw new System.Exception("_layer4 = NULL");
        else if (_gameState == null)
            throw new System.Exception("_gameState = NULL");
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (_gameState.Start())
        {
            Move(_layer1, _modifier1);
            Move(_layer2, _modifier2);
            Move(_layer3, _modifier3);
            Move(_layer4, _modifier4);
        }
    }


    private void Move(List<GameObject> layer, float modifier)
    {
        foreach (var l in layer)
        {
            l.transform.position += Vector3.left * _speed * Time.deltaTime * modifier;
        }
        if (layer[1].transform.position.x < _mainCam.gameObject.transform.position.x)
        {
            GameObject temp = layer[0];
            layer.Remove(layer[0]);
            temp.transform.position = layer[1].transform.position + new Vector3(layer[1].GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
            layer.Add(temp);
        }
    }
}
