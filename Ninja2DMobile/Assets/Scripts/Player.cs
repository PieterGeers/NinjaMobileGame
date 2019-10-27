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

    private Rigidbody2D _rb = null;

    private LineRenderer _grappleLineRenderer = null;

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

    private float _minDistanceForSwipe = 75f;
    private float _currentTime = 0f;


    private Vector3 QuadraticParametersFromPoints(Vector2 p1, Vector2 p2, Vector2 p3) //chronologic points
    {
        Vector3 newVec = Vector3.zero;

        newVec.x = ((p2.y - p1.y) * (p1.x - p3.x) + (p3.y - p1.y) * (p2.x - p1.x)) /
            ((p1.x - p3.x) * (Mathf.Pow(p2.x, 2) - Mathf.Pow(p1.x, 2)) + (p2.x - p1.x) * (Mathf.Pow(p3.x, 2) - Mathf.Pow(p1.x, 2)));

        newVec.y = ((p2.y - p1.y) - newVec.x * (Mathf.Pow(p2.x, 2) - Mathf.Pow(p1.x, 2))) / (p2.x - p1.x);

        newVec.z = p1.y - newVec.x * Mathf.Pow(p1.x, 2) - newVec.y * p1.x;

        Debug.Log(newVec);
        return newVec;
    }

    private void DrawLineWhileGrapple()
    {
        if (_grappleLineRenderer != null)
        {
            Vector3[] positions = new Vector3[2] { transform.position, _grappleLineRenderer.transform.GetChild(0).transform.position };
            _grappleLineRenderer.SetPositions(positions);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DefaultPole")
        {
            if (!_start) { _start = true; }
            else { ++_score; }
            _currentTime = 0f;
            if (_manager.GetPole(2).tag == "DefaultPole")
                CalculateNormalJump();
        }
        else if (collision.gameObject.tag == "Dead" || collision.gameObject.tag == "OutOfBounds")
        {
            Time.timeScale = 0.0f;
            _resetCanvas.SetActive(true);
        }
    }

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private Vector3 NewMovePosition(Vector3 param)
    {
        float x = Mathf.Lerp(_jumpFromPosition.x, _jumpToPosition.x, _currentTime / _jumpTime);
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
            Vector3 newPosition = NewMovePosition(_fase1param);
            _rb.MovePosition(newPosition);
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
