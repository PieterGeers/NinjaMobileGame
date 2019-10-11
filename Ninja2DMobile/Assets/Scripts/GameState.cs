using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private bool _beginOnPlay = false;
    private bool _start = true;

    [SerializeField]
    private float _currentSpeed = -2.5f;
    [SerializeField]
    private float _maxSpeed = -10.0f;
    [SerializeField]
    private float _speedIncrease = -0.1f;
    [SerializeField]
    private float _timeToIncrease = 15f;
    private float _timer = 0f;

    public bool SpeedIncreaseBool = false;

    private void Awake()
    {
        _start = _beginOnPlay;
    }

    public void SetStart(bool value)
    {
        _start = value;
    }

    public bool Start()
    {
        return _start;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (_currentSpeed > _maxSpeed && !SpeedIncreaseBool)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToIncrease)
            {
                SpeedIncreaseBool = true;
                _currentSpeed += _speedIncrease;
                _currentSpeed = Mathf.Round(_currentSpeed * 10f) / 10f;
                _timer = 0f;
            }
        }
    }

    public float GetSpeed()
    {
        return _currentSpeed;
    }
}
