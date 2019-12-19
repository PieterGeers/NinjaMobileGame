using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField]
    private PoleManagerTutorial _manager = null;
    [SerializeField]
    private TutorialManager _tutorial = null;
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
    private int _polesBehindCharacter = 2;

    private Rigidbody2D _rb = null;

    private Animator _am = null;

    private GameObject _grapplePole = null;

    private Camera _camera = null;

    private Vector3 _fase1param = Vector3.zero;
    private Vector3 _fase2param = Vector3.zero;
    private Vector3 _fase3param = Vector3.zero;
    private Vector3 _jumpFromPosition = Vector3.zero;
    private Vector3 _jumpToPosition = Vector3.zero;
    private Vector3 _previous = Vector3.zero;
    private Vector3[] _touch = new Vector3[2];

    private bool _start = false;
    private bool _isGrappling = false;
    private bool _canChangeGrappleState = true;
    private bool _failedToGrapple = false;
    private bool _wasGamePaused = false;
    private bool[] _tap = new bool[2] { false, false };

    private float _currentTime = 0f;
    private float[] _tapTime = new float[2] { 0f, 0f };
    private float _minHoldTime = 0.2f;

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
            Vector3[] positions = new Vector3[2] { transform.GetChild(0).transform.position, GetJumpToPosition(_grapplePole).transform.position };
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

    public void SetBoolResume()
    {
        _wasGamePaused = true;
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
        _jumpFromPosition = GetJumpToPosition(_manager.GetPole(_polesBehindCharacter)).position;
        _jumpToPosition = GetJumpToPosition(_manager.GetPole(_polesBehindCharacter + 1)).position;

        Vector2 start = new Vector2(_jumpFromPosition.x, _jumpFromPosition.y);
        Vector2 end = new Vector2(_jumpToPosition.x, _jumpToPosition.y);
        Vector2 middle = new Vector2(start.x + ((end.x - start.x) / 2.0f), _jumpHeight);

        _fase1param = QuadraticParametersFromPoints(start, middle, end);

        _manager.SpawnNewPole(0, true);
    }

    private void CalculateGrapple()
    {
        _isGrappling = true;

        _jumpFromPosition = GetJumpToPosition(_manager.GetPole(_polesBehindCharacter)).position;
        _jumpToPosition = GetJumpToPosition(_manager.GetPole(_polesBehindCharacter + 2)).position;
        _grapplePole = _manager.GetPole(_polesBehindCharacter + 1);

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

        _manager.SpawnNewPole(0, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DefaultPole")
        {
            if (!_start) { _start = true; }
            if (_isGrappling)
            {
                _isGrappling = false;
                _manager.SpawnNewPole(0, true);
            }
            _currentTime = 0f;
            if (_manager.GetPole(_polesBehindCharacter + 1).tag == "DefaultPole")
                CalculateNormalJump();
            else if (_manager.GetPole(_polesBehindCharacter + 1).tag == "GrapplingPole")
                CalculateGrapple();
            if (_manager.GetPole(_polesBehindCharacter + 1).tag == "GrapplingPole")
                _tutorial.ShowTutorial(0);
            else if (_manager.GetPole(_polesBehindCharacter + 1).tag == "DefaultPole" && _manager.GetPole(_polesBehindCharacter + 1).transform.GetChild(0).gameObject.activeSelf == true)
                _tutorial.ShowTutorial(1);
        }
        else if (collision.gameObject.tag == "Dead" || collision.gameObject.tag == "OutOfBounds" || collision.gameObject.tag == "Obstacle")
        {
            if (collision.gameObject.tag == "Obstacle")
                TutorialManager.FailedToThrow = true;
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Dead();
        }
    }

    private void Dead()
    {
        _am.SetBool("Dead", true);
        SceneManager.LoadScene(2);
    }

    private void Start()
    {
        _am = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
            //Animator update
            Vector3 current = transform.position;
            Vector3 diff = current - _previous;
            _am.SetFloat("Direction", diff.y);
            _previous = current;

            //Input
            if (Time.timeScale > 0)
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
                //Section 1
                if (transform.position.x < _jumpFromPosition.x + _grappleJumpOffset)
                {
                    newPosition = NewMovePosition(_fase1param, _jumpFromPosition, new Vector3(_jumpFromPosition.x + _grappleJumpOffset, _jumpHeight, 0), _currentTime, _jumpTime / 2.0f);
                    _rb.MovePosition(newPosition);

                    //check so that the player cant infinite hold 1 finger on screen to grapple
                    if (!_canChangeGrappleState)
                        _canChangeGrappleState = true;
                }
                //Section 3
                else if (transform.position.x >= _jumpToPosition.x - _grappleJumpOffset && !_failedToGrapple)
                {
                    StopDrawLineWhileGrapple();

                    if (_am.GetBool("Grapple"))
                        _am.SetBool("Grapple", false);

                    newPosition = NewMovePosition(_fase3param, new Vector3(_jumpToPosition.x - _grappleJumpOffset, _jumpHeight, 0), _jumpToPosition, _currentTime - (_jumpTime + (_jumpTime / 2.0f)), _jumpTime / 2.0f);
                    _rb.MovePosition(newPosition);
                }
                //Section 2
                else if (_tap[0] || _tap[1])
                {
                    if (!_failedToGrapple)
                    {
                        //set animation
                        if (!_am.GetBool("Grapple"))
                            _am.SetBool("Grapple", true);

                        //calculate new position
                        newPosition = NewMovePosition(_fase2param, new Vector3(_jumpFromPosition.x + _grappleJumpOffset, _jumpHeight, 0), new Vector3(_jumpToPosition.x - _grappleJumpOffset, _jumpHeight, 0), _currentTime - (_jumpTime / 2.0f), _jumpTime);

                        //move to new position
                        _rb.MovePosition(newPosition);

                        //Draw grapple line 
                        DrawLineWhileGrapple();
                    }
                }
                else
                {
                    _failedToGrapple = true;
                    TutorialManager.FailedToGrapple = true;
                }
            }
        }
    }

    private void ThrowShurikan(int idx)
    {
        AudioManager.instance.PlaySoundEffect("ThrowShurikan");
        Vector3 playerPosition = _camera.WorldToScreenPoint(transform.position);
        Vector2 throwDirection = new Vector2(_touch[idx].x - playerPosition.y, _touch[idx].y - playerPosition.y).normalized;
        GameObject shurikan = Instantiate(_shurikan);
        shurikan.transform.position = transform.position + (new Vector3(throwDirection.x, throwDirection.y, 0f) * _shurikanOffsetMult);
        shurikan.GetComponent<Throwable>().SetDirection(throwDirection);
        shurikan.GetComponent<Throwable>().InstantKill = false;
        shurikan.GetComponent<Throwable>().SuperBreakerActive = false;
    }


    private void CheckThrow(int idx)
    {
        if (_tapTime[idx] < _minHoldTime && !_wasGamePaused)
        {
            ThrowShurikan(idx);
        }
        _wasGamePaused = false;
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            int touchCount = Input.touchCount;
            if (touchCount > 2)
                touchCount = 2;
            for (int i = 0; i < touchCount; ++i)
            {
                Touch current = Input.touches[i];
                if (current.fingerId >= 2)
                    continue;
                switch (current.phase)
                {
                    case TouchPhase.Began:
                        _tap[current.fingerId] = true;
                        break;
                    case TouchPhase.Moved:
                        _tapTime[current.fingerId] += current.deltaTime;
                        break;
                    case TouchPhase.Stationary:
                        _tapTime[current.fingerId] += current.deltaTime;
                        break;
                    case TouchPhase.Ended:
                        EndTouch(i, current);
                        break;
                    case TouchPhase.Canceled:
                        EndTouch(i, current);
                        break;
                }
            }
        }
    }

    private void EndTouch(int i, Touch current)
    {
        _touch[current.fingerId] = Input.touches[i].position;
        _tap[current.fingerId] = false;
        CheckThrow(current.fingerId);
        _tapTime[current.fingerId] = 0.0f;
    }
}
