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
    private Vector2 _lineParam1 = Vector2.zero;
    [SerializeField]
    private Vector2 _lineParam2 = Vector2.zero;
    [SerializeField]
    private float _height = 7.0f;
    [SerializeField]
    private float _grapplingJumpHeight = 1.5f;
    [SerializeField]
    private float _grapplingHeight = 1.5f;
    [SerializeField]
    private float _grapplingOffset = 1f;
    [SerializeField]
    private float _verticalOffset = 3.0f;
    [SerializeField]
    private float _horizontalOffset = -6.0f;
    [SerializeField]
    private float _shurikanOffsetMultiplier = 2.0f;

    //*******************************************
    //FOR TESTING ONLY
    [SerializeField]
    private GameObject _canvas = null;
    //*******************************************


    private bool _start = false;
    private bool _grapple = false;
    private bool _pressed = false;
    private uint _score = 0;

    [SerializeField]
    private GameObject _prevPole = null;
    [SerializeField]
    private Rope _prevGrapplingPole = null;

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
            _grapple = false;
            ++_score;
            if (_poleManager.GetNexPole().tag == "DefaultPole")
                CalculateQuadraticParam();
            else
            {
                _grapple = true;
                CalculateQuadraticGrapplingParam();
            }
        }
        else if (collision.gameObject.tag == "Dead")
        {
            Time.timeScale = 0.0f;
            _canvas.SetActive(true);
        }
    }

    /*Function Update does the Input and the automatic movement of the character*/
    private void Update()
    {
        HandleInput();

        if (_start)
        {
            float x = GetDistanceToLastPole();
            if (x <= _poleManager.GetMaxDistance() && !_grapple)
            {
                x += _horizontalOffset;
                float height = _quadraticParam.x * Mathf.Pow(x, 2) + _quadraticParam.y * x + _quadraticParam.z;
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }
            else if (x <= _poleManager.GetMaxDistance() * 2f && _grapple)
            {
                if (x <= _grapplingOffset)
                {
                    x += _horizontalOffset;
                    float height = _lineParam1.x * x + _lineParam1.y;
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);
                }
                else if (x >= (_poleManager.GetMaxDistance() * 2f) - _grapplingOffset)
                {
                    x += _horizontalOffset;
                    float height = _lineParam2.x * x + _lineParam2.y;
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);
                }
                else
                {
                    if (!_pressed)
                        _start = false;
                    if (!_prevGrapplingPole.IsInitialized())
                    {
                        _prevGrapplingPole.GenerateRope(6);
                        //HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
                        //joint.autoConfigureConnectedAnchor = false;
                        //joint.connectedBody = _prevGrapplingPole.GetLastLink();
                    }
                    x += _horizontalOffset;
                    float height = _quadraticParam.x * Mathf.Pow(x, 2) + _quadraticParam.y * x + _quadraticParam.z;
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);
                }
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

    private void CalculateQuadraticGrapplingParam()
    {
        _prevPole = _poleManager.GetCurrentPole();
        _poleManager.RemoveLastPole();
        _prevGrapplingPole = _poleManager.GetCurrentPole().GetComponentInChildren<Rope>();

        Vector2 current = new Vector2(_prevPole.transform.position.x, _prevPole.transform.position.y + _verticalOffset);
        Vector2 next = new Vector2(_poleManager.GetNexPole().transform.position.x, _poleManager.GetNexPole().transform.position.y + _verticalOffset);
        Vector2 fase1End = new Vector2(current.x + _grapplingOffset, _grapplingJumpHeight);
        Vector2 fase2End = new Vector2(next.x - _grapplingOffset, _grapplingJumpHeight);
        Vector2 mid = new Vector2(current.x + (next.x - current.x) / 2.0f, _grapplingHeight);

        //Fase1 [LINE]
        _lineParam1 = LineParameterFromPoints(current, fase1End);

        //Fase2 [Quadratic]
        _quadraticParam.x = ((mid.y - fase1End.y) * (fase1End.x - fase2End.x) + (fase2End.y - fase1End.y) * (mid.x - fase1End.x)) /
            ((fase1End.x - fase2End.x) * (Mathf.Pow(mid.x, 2) - Mathf.Pow(fase1End.x, 2)) + (mid.x - fase1End.x) * (Mathf.Pow(fase2End.x, 2) - Mathf.Pow(fase1End.x, 2)));

        _quadraticParam.y = ((mid.y - fase1End.y) - _quadraticParam.x * (Mathf.Pow(mid.x, 2) - Mathf.Pow(fase1End.x, 2))) / (mid.x - fase1End.x);

        _quadraticParam.z = fase1End.y - _quadraticParam.x * Mathf.Pow(fase1End.x, 2) - _quadraticParam.y * fase1End.x;

        //Fase3 [LINE]
        _lineParam2 = LineParameterFromPoints(fase2End, next);

        
        _poleManager.RemoveLastPole();
    }

    /*Helper functions for calculating path the character will take*/
    private Vector2 LineParameterFromPoints(Vector2 p1, Vector2 p2)
    {
        float A = 0f;
        float B = 0f;
        float C = 0f;

        A = p2.y - p1.y;
        B = -(p2.x - p1.x);
        C = p1.y * p2.x - p1.x * p2.y;

        return new Vector2(-A/B, -C/B);
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
            _pressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _releasePosition = Input.mousePosition;
            _pressed = false;
            if (Vector2.Distance(_pressPosition, _releasePosition) > _minSwipeDistance)
            {
                ThrowShurikan();
            }
        }

    }

    /*Function ThrowShurikan spawns in a shurikan and sets the correct direction*/
    private void ThrowShurikan()
    {
        Vector2 throwDirection = new Vector2(_releasePosition.x - _pressPosition.x, _releasePosition.y - _pressPosition.y).normalized;
        GameObject shurikan = Instantiate(_shurikan);
        shurikan.transform.position = transform.position + new Vector3(throwDirection.x, throwDirection.y, 0f) * _shurikanOffsetMultiplier;
        shurikan.GetComponent<Throwable>().SetDirection(throwDirection);
    }

}
