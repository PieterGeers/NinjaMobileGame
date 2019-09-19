using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour
{
    [SerializeField]
    private float _maxDistance = 2.0f;
    [SerializeField]
    private float _height = 1.0f;
    [SerializeField]
    private float _startPositionX = -8.0f;
    [SerializeField]
    private uint _initAmountOfPoles = 5;
    [SerializeField]
    private uint _destroyTime = 15;
    [SerializeField]
    private GameObject _poleReference = null;
    [SerializeField]
    private GameObject _newPole = null;

    /*Function Awake checks if a gameobject reference was given to this script. If not then this will result in a crash*/
    private void Awake()
    {
        if (_poleReference == null)
            throw new System.Exception("_poleReference = NULL");
    }

    /*Function Start creates the initial random level and moves the "Spawner" object to the correct location indicated by the parameters in the editor*/
    private void Start()
    {
        gameObject.transform.position = new Vector3(_startPositionX, gameObject.transform.position.y, gameObject.transform.position.z);

        for (uint i = 0; i < _initAmountOfPoles; ++i)
        {
            SpawnPole();
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + _maxDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    /*Function Update instantiates a new pole object if the previous one has traveled a certain distance*/
    private void Update()
    {
        if ((transform.position.x - _newPole.transform.position.x) >= _maxDistance)
        {
            SpawnPole();
        }
    }

    /*Function SpawnPole creates a new pole object and transforms it to the correct location*/
    private void SpawnPole()
    {
        _newPole = Instantiate(_poleReference);
        _newPole.transform.position = transform.position + new Vector3(0.0f, Random.Range(_height, -_height), 0.0f);
        Destroy(_newPole, _destroyTime);
    }
}
