using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    [SerializeField]
    private PoleManager _poleManager = null;
    [SerializeField]
    private Vector3 _quadraticParam = Vector3.zero;
    [SerializeField]
    private float _height = 7.0f;
    [SerializeField]
    private float _verticalOffset = 3.0f;
    [SerializeField]
    private float _horizontalOffset = -6.0f;

    private bool _start = false;
    private uint _score = 0;

    [SerializeField]
    private GameObject _prevPole = null;

    private Vector2 _pressPosition = Vector2.zero;
    private Vector2 _releasePosition = Vector2.zero;

    [SerializeField]
    private float _minSwipeDistance = 1.0f;
    [SerializeField]
    private GameObject _shurikan = null;

    /*Function Awake checks if some important variables are set*/
    private void Awake()
    {
        if (_poleManager == null)
            throw new System.Exception("_poleSpawner = NULL");
        if (_shurikan == null)
            throw new System.Exception("_shurikan = NULL");
    }

    /*Function Onclollision checks if the player is colliding with a pole and then calculates the jump to the next pole*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DefaultPole")
        {
            if (!_start)
            {
                FindObjectOfType<GameState>().GetComponent<GameState>().SetStart(true);
                _start = true;
            }
            ++_score;
            CalculateQuadraticParam();
        }

    }

    /*Function Update does the Input and the automatic movement of the character*/
    private void Update()
    {
        HandleInput();

        if (_start)
        {
            float x = GetDistanceToLastPole();
            if (x <= 5.0f)
            {
                x += _horizontalOffset;
                float height = _quadraticParam.x * Mathf.Pow(x, 2) + _quadraticParam.y * x + _quadraticParam.z;
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }
        }
    }

    /*Function CalculateQuadraticParam calculates the path the character has to take to the next pole*/
    private void CalculateQuadraticParam()
    {
        _prevPole = _poleManager.GetCurrentPole();

        Vector2 current = new Vector2(_prevPole.transform.position.x, _prevPole.transform.position.y + _verticalOffset);
        Vector2 next = new Vector2(_poleManager.GetNexPole().transform.position.x, _poleManager.GetNexPole().transform.position.y + _verticalOffset);
        Vector2 mid = new Vector2(current.x + (next.x - current.x) / 2.0f, _height);

        _quadraticParam.x = ((mid.y - current.y) * (current.x - next.x) + (next.y - current.y) * (mid.x - current.x)) /
            ((current.x - next.x) * (Mathf.Pow(mid.x, 2) - Mathf.Pow(current.x, 2)) + (mid.x - current.x) * (Mathf.Pow(next.x, 2) - Mathf.Pow(current.x, 2)));

        _quadraticParam.y = ((mid.y - current.y) - _quadraticParam.x * (Mathf.Pow(mid.x, 2) - Mathf.Pow(current.x, 2))) / (mid.x - current.x);

        _quadraticParam.z = current.y - _quadraticParam.x * Mathf.Pow(current.x, 2) - _quadraticParam.y * current.x;

        _poleManager.RemoveLastPole();
    }

    /*Helper function to prevent clipping*/
    private float GetDistanceToLastPole()
    {
        return transform.position.x - _prevPole.transform.position.x;
    }

    /*Accessor to get Score*/
    public uint GetScore()
    {
        return _score;
    }

    /*Function HandleInput does the input givin by the player*/
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _pressPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _releasePosition = Input.mousePosition;
            Debug.Log(Vector2.Distance(_pressPosition, _releasePosition));
            if (Vector2.Distance(_pressPosition, _releasePosition) > _minSwipeDistance)
            {
                ThrowShurikan();
            }
        }
    }

    /*Function ThrowShurikan spawns in a shurikan and sets the correct direction*/
    private void ThrowShurikan()
    {
        Vector3 throwDirection = new Vector3(_releasePosition.x - _pressPosition.x, _releasePosition.y - _pressPosition.y, 0).normalized;
        GameObject shurikan = Instantiate(_shurikan);
        shurikan.transform.position = transform.position;
        shurikan.GetComponent<Throwable>().SetDirection(throwDirection);
    }

}
