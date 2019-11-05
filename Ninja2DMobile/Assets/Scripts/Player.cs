using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PoleManager _manager = null;
    [SerializeField]
    private GameObject _resetCanvas = null;
    [SerializeField]
    private GameObject _playingCanvas = null;
    [SerializeField]
    private GameObject _shurikan = null;
    [SerializeField]
    private float _jumpHeight = 3f;
    [SerializeField]
    private float _grappleHeight = -1f;
    [SerializeField]
    private float _grappleJumpOffset = 2f;
    [SerializeField]
    private float _shurikanOffsetMult = 1.2f;
    [SerializeField]
    private float _jumpTime = 1f;
    [SerializeField]
    private uint _scoreForSpeedIncrease = 10;


    private Rigidbody2D _rb = null;

    private GameObject _grapplePole = null;

    private Vector3 _fase1param = Vector3.zero;
    private Vector3 _fase2param = Vector3.zero;
    private Vector3 _fase3param = Vector3.zero;
    private Vector3 _pressPosition = Vector3.zero;
    private Vector3 _releasePosition = Vector3.zero;
    private Vector3 _jumpFromPosition = Vector3.zero;
    private Vector3 _jumpToPosition = Vector3.zero;

    private uint _score = 0;

    private bool _start = false;
    private bool _isGrappling = false;
    private bool _pressed = false;
    private bool _failedToGrapple = false;

    private float _minDistanceForSwipe = 75f;
    private float _currentTime = 0f;
    private float _maxSpeed = 0.5f;
    private float _speedIncrease = 0.1f;

    private Vector3 QuadraticParametersFromPoints(Vector2 p1, Vector2 p2, Vector2 p3) //chronologic points
    {
        Vector3 newVec = Vector3.zero;

        newVec.x = ((p2.y - p1.y) * (p1.x - p3.x) + (p3.y - p1.y) * (p2.x - p1.x)) /
            ((p1.x - p3.x) * (Mathf.Pow(p2.x, 2) - Mathf.Pow(p1.x, 2)) + (p2.x - p1.x) * (Mathf.Pow(p3.x, 2) - Mathf.Pow(p1.x, 2)));

        newVec.y = ((p2.y - p1.y) - newVec.x * (Mathf.Pow(p2.x, 2) - Mathf.Pow(p1.x, 2))) / (p2.x - p1.x);

        newVec.z = p1.y - newVec.x * Mathf.Pow(p1.x, 2) - newVec.y * p1.x;

        return newVec;
    }

    private void DrawLineWhileGrapple()
    {
        if (_grapplePole != null)
        {
            Vector3[] positions = new Vector3[2] { transform.position, GetJumpToPosition(_grapplePole).transform.position };
            _grapplePole.GetComponent<LineRenderer>().SetPositions(positions);
        }
    }

    private void StopDrawLineWhileGrapple()
    {
        if (_grapplePole != null)
        {
            Destroy(_grapplePole.GetComponent<LineRenderer>());
            _grapplePole = null;
        }
    }

    public uint GetScore()
    {
        return _score;
    }

    private Transform GetJumpToPosition(GameObject pole)
    {
        for (int i = 0; i < pole.transform.childCount; ++i)
        {
            if (pole.transform.GetChild(i).tag == "Position")
            {
                return pole.transform.GetChild(i);
            }
        }
        return null;
    }

    private void CalculateNormalJump()
    {
        _jumpFromPosition = GetJumpToPosition(_manager.GetPole(1)).position;
        _jumpToPosition = GetJumpToPosition(_manager.GetPole(2)).position;

        Vector2 start = new Vector2(_jumpFromPosition.x, _jumpFromPosition.y);
        Vector2 end = new Vector2(_jumpToPosition.x, _jumpToPosition.y);
        Vector2 middle = new Vector2(start.x + ((end.x - start.x) / 2.0f), _jumpHeight);

        _fase1param = QuadraticParametersFromPoints(start, middle, end);

        _manager.SpawnNewPole();
    }

    private void CalculateGrapple()
    {
        _isGrappling = true;

        _jumpFromPosition = GetJumpToPosition(_manager.GetPole(1)).position;
        _jumpToPosition = GetJumpToPosition(_manager.GetPole(3)).position;
        _grapplePole = _manager.GetPole(2);

        Vector2 fase1start = new Vector2(_jumpFromPosition.x, _jumpFromPosition.y);
        Vector2 fase1middle = new Vector2(fase1start.x + _grappleJumpOffset, _jumpHeight);
        Vector2 fase1end = new Vector2(fase1middle.x + _grappleJumpOffset, _jumpFromPosition.y);

        Vector2 fase3end = new Vector2(_jumpToPosition.x, _jumpToPosition.y);
        Vector2 fase3middle = new Vector2(fase3end.x - _grappleJumpOffset, _jumpHeight);
        Vector2 fase3start = new Vector2(fase3middle.x - _grappleJumpOffset, _jumpToPosition.y);

        Vector2 fase2start = fase1middle;
        Vector2 fase2middle = new Vector2(_jumpFromPosition.x + ((_jumpToPosition.x - _jumpFromPosition.x) / 2.0f), _grappleHeight);
        Vector2 fase2end = fase3middle;

        _fase1param = QuadraticParametersFromPoints(fase1start, fase1middle, fase1end);
        _fase2param = QuadraticParametersFromPoints(fase2start, fase2middle, fase2end);
        _fase3param = QuadraticParametersFromPoints(fase3start, fase3middle, fase3end);
        
        _manager.SpawnNewPole();
    }

    private void CheckSpeedIncrease()
    {
        if (_score % _scoreForSpeedIncrease == 0 && _jumpTime > _maxSpeed) 
        {
            _jumpTime -= _speedIncrease;
            _jumpTime = Mathf.Round(_jumpTime * 10f) / 10f;
            if (_jumpTime < _maxSpeed)
                _jumpTime = _maxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DefaultPole")
        {
            if (!_start) { _start = true; }
            else { ++_score; CheckSpeedIncrease(); }
            if (_isGrappling)
            {
                _isGrappling = false;
                _manager.SpawnNewPole();
            }
            _currentTime = 0f;
            if (_manager.GetPole(2).tag == "DefaultPole")
                CalculateNormalJump();
            else if (_manager.GetPole(2).tag == "GrapplingPole")
                CalculateGrapple();
        }
        else if (collision.gameObject.tag == "Dead" || collision.gameObject.tag == "OutOfBounds")
        {
            Time.timeScale = 0.0f;
            _resetCanvas.SetActive(true);
            _playingCanvas.SetActive(false);
        }
    }

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private Vector3 NewMovePosition(Vector3 param, Vector3 frompos, Vector3 topos, float currenttime, float time)
    {
        float x = Mathf.Lerp(frompos.x, topos.x, currenttime / time);
        float y = param.x * Mathf.Pow(x, 2) + param.y * x + param.z;
        return new Vector3(x, y + 0.5f, 0);
    }

    private void Update()
    {
        if (_start)
        {
            //Input
            HandleInput();
            //Time update
            _currentTime += Time.deltaTime;
            //Normal jump position update
            if (!_isGrappling)
            {
                Vector3 newPosition = NewMovePosition(_fase1param, _jumpFromPosition, _jumpToPosition, _currentTime, _jumpTime);
                _rb.MovePosition(newPosition);
            }
            //Jump with grappling hook
            else
            {
                Vector3 newPosition = Vector3.zero;
                if (transform.position.x < _jumpFromPosition.x + _grappleJumpOffset)
                    newPosition = NewMovePosition(_fase1param,
                                                  _jumpFromPosition,
                                                  new Vector3(_jumpFromPosition.x + _grappleJumpOffset, _jumpHeight, 0),
                                                  _currentTime,
                                                  _jumpTime / 2.0f);
                else if (transform.position.x >= _jumpToPosition.x - _grappleJumpOffset && !_failedToGrapple)
                    newPosition = NewMovePosition(_fase3param,
                                                  new Vector3(_jumpToPosition.x - _grappleJumpOffset, _jumpHeight, 0),
                                                  _jumpToPosition,
                                                  _currentTime - (_jumpTime + (_jumpTime / 2.0f)),
                                                  _jumpTime / 2.0f);
                else if (_pressed && !_failedToGrapple)
                    newPosition = NewMovePosition(_fase2param,
                                                  new Vector3(_jumpFromPosition.x + _grappleJumpOffset, _jumpHeight, 0),
                                                  new Vector3(_jumpToPosition.x - _grappleJumpOffset, _jumpHeight, 0),
                                                  _currentTime - (_jumpTime / 2.0f),
                                                  _jumpTime);
                else if (!_failedToGrapple)
                    _failedToGrapple = true;
                if (!_failedToGrapple)
                {
                    _rb.MovePosition(newPosition);
                    if (transform.position.x > _jumpFromPosition.x + _grappleJumpOffset &&
                        transform.position.x <= _jumpToPosition.x - _grappleJumpOffset)
                        DrawLineWhileGrapple();
                    else if (transform.position.x > _jumpToPosition.x - _grappleJumpOffset)
                        StopDrawLineWhileGrapple();
                }
            }
        }
    }

    private void ThrowShurikan()
    {
        Vector2 throwDirection = new Vector2(_releasePosition.x - _pressPosition.y, _releasePosition.y - _pressPosition.y).normalized;
        GameObject shurikan = Instantiate(_shurikan);
        shurikan.transform.position = transform.position + (new Vector3(throwDirection.x, throwDirection.y, 0f) * _shurikanOffsetMult);
        shurikan.GetComponent<Throwable>().SetDirection(throwDirection);
    }

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
            if (Vector2.Distance(_pressPosition, _releasePosition) > _minDistanceForSwipe)
            {
                ThrowShurikan();
            }
        }
    }
}
