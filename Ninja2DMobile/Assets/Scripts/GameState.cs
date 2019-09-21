using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private bool _beginOnPlay = false;
    private bool _start = true;

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
}
