using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.5f;
    private GameState _gameState = null;

    private void Start()
    {
        _gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (_gameState == null)
            throw new System.Exception("_gameState = NULL");
    }

    void Update()
    {
        if (_gameState.Start())
            transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
