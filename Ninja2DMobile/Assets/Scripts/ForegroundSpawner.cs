using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _foreground = null;
    [SerializeField]
    private Vector2 _minMaxScale = Vector2.zero;
    [SerializeField]
    private Vector2 _minMaxDistance = Vector2.zero;
    [SerializeField]
    private float _height = 0.5f;
    [SerializeField]
    private float _startPositionX = -8.0f;
    [SerializeField]
    private float _endPositionX = 15.0f;
    [SerializeField]
    private uint _initAmountOfObject = 3;
    [SerializeField]
    private uint _destroyTime = 15;
    [SerializeField]
    private GameObject _newObject = null;

    private float _maxDistance = 0.0f;

    /*Function Awake checks certain parameters*/
    private void Awake()
    {
        if (_foreground == null)
            throw new System.Exception("_foreground = NULL");
    }

    /*Function Starts creates and initial foreground and sets the spawner in the correct location*/
    private void Start()
    {
        gameObject.transform.position = new Vector3(_startPositionX, gameObject.transform.position.y, gameObject.transform.position.z);

        for (uint i = 0; i < _initAmountOfObject; ++i)
        {
            SpawnObject();
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + _maxDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        gameObject.transform.position = new Vector3(_endPositionX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    /*Function Update spawns a new foreground object is the last on is far enough away*/
    private void Update()
    {
        if ((transform.position.x - _newObject.transform.position.x) >= _maxDistance)
        {
            SpawnObject();
        }
    }

    /*Function SpawnObject spawn a random new foreground object*/
    private void SpawnObject()
    {
        int randomObj = Random.Range(0, _foreground.Count);
        float randomScale = Random.Range(_minMaxScale.x, _minMaxScale.y);
        _newObject = Instantiate(_foreground[randomObj]);
        _newObject.transform.position = transform.position + new Vector3(0.0f, Random.Range(_height, -_height), 0.0f);
        _newObject.transform.localScale = new Vector3(randomScale, randomScale, 1.0f);
        Destroy(_newObject, _destroyTime);
        _maxDistance = Random.Range(_minMaxDistance.x, _minMaxDistance.y);
    }


}
