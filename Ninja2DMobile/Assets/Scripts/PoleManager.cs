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
    private Vector2 _chanceForDestroyable = new Vector2(0, 10);
    [SerializeField]
    private Vector2 _chanceForGrappling = new Vector2(90, 100);
    [SerializeField]
    private GameObject _poleReference = null;
    [SerializeField]
    private GameObject _grapplingPoleReference = null;

    private GameObject _newPole = null;
    [SerializeField]
    private List<GameObject> _polesOnScreen = null;
    private GameState _gameState = null;

    /*Function Awake checks if a gameobject reference was given to this script. If not then this will result in a crash*/
    private void Awake()
    {
        if (_gameState == null)
            _gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (_poleReference == null)
            throw new System.Exception("_poleReference = NULL");
        if (_grapplingPoleReference == null)
            throw new System.Exception("_grapplingPoleReference = NULL");
    }

    /*Function Start creates the initial random level and moves the "Spawner" object to the correct location indicated by the parameters in the editor*/
    private void Start()
    {
        gameObject.transform.position = new Vector3(_startPositionX, gameObject.transform.position.y, gameObject.transform.position.z);

        for (uint i = 0; i < _initAmountOfPoles; ++i)
        {
            SpawnPole(-1);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + _maxDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        _polesOnScreen[0].GetComponent<BoxCollider2D>().enabled = true;
    }

    /*Function Update instantiates a new pole object if the previous one has traveled a certain distance*/
    private void Update()
    {
        if ((transform.position.x - _newPole.transform.position.x) >= _maxDistance)
        {
            int chance = Random.Range(0, 100);
            if (_newPole.tag == "GrapplingPole")
            {
                SpawnPole(-1);
            }
            else
            {
                SpawnPole(chance);
                SpawnGrapplingPole(chance);
            }
        }
        if (_gameState.SpeedIncreaseBool)
        {
            GameObject[] poles = GameObject.FindGameObjectsWithTag("DefaultPole");
            GameObject[] grapple = GameObject.FindGameObjectsWithTag("GrapplingPole");
            foreach (GameObject p in poles)
            {
                p.GetComponent<MoveObject>().SetSpeed(_gameState.GetSpeed());
            }
            foreach (GameObject g in grapple)
            {
                g.GetComponent<MoveObject>().SetSpeed(_gameState.GetSpeed());
            }
            _gameState.SpeedIncreaseBool = false;
        }
    }

    /*Function SpawnPole creates a new pole object and transforms it to the correct location*/
    private void SpawnPole(int chance)
    {
        _newPole = Instantiate(_poleReference);
        _newPole.transform.position = transform.position + new Vector3(0.0f, Random.Range(_height, -_height), 0.0f);
        _newPole.GetComponent<MoveObject>().SetSpeed(_gameState.GetSpeed());
        Destroy(_newPole, _destroyTime);
        _polesOnScreen.Add(_newPole);
        if (chance >= _chanceForDestroyable.x && chance < _chanceForDestroyable.y)
        {
            _newPole.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void SpawnGrapplingPole(int chance)
    {
        if (chance >= _chanceForGrappling.x && chance < _chanceForGrappling.y)
        {
            _polesOnScreen.Remove(_newPole);
            Destroy(_newPole);
            _newPole = Instantiate(_grapplingPoleReference);
            _newPole.transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
            _newPole.GetComponent<MoveObject>().SetSpeed(_gameState.GetSpeed());
            Destroy(_newPole, _destroyTime);
            _polesOnScreen.Add(_newPole);
        }
    }

    /*Functions below are used for calculating the path of the jump*/
    public GameObject GetCurrentPole()
    {
        return _polesOnScreen[0];
    }

    public GameObject GetNexPole()
    {
        return _polesOnScreen[1];
    }

    public void RemoveLastPole()
    {
        _polesOnScreen.Remove(_polesOnScreen[0]);
    }

    public float GetMaxDistance()
    {
        return _maxDistance;
    }
    /*--------------------------------------------------------------*/


}
