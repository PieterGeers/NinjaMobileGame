using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScreen : MonoBehaviour
{

    [SerializeField] private Text _scorePoints;
    [SerializeField] private GameObject _character;
    private uint _score = 0;

    void Update()
    {
        _score = _character.GetComponent<Character_Controller>().GetScore();
        _scorePoints.text = _score.ToString();
    }
}
