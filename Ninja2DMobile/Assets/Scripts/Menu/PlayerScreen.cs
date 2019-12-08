using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScreen : MonoBehaviour
{

    [SerializeField]
    private Text _scorePoints = null;
    [SerializeField]
    private Player _character = null;

    private uint _score = 0;

    private void Awake()
    {
        if (_character == null)
            throw new System.Exception("_character = NULL");
    }

    void Update()
    {
        _score = _character.GetScore();
        _scorePoints.text = _score.ToString();
    }
}
