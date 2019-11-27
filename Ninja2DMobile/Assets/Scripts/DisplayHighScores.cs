using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHighScores : MonoBehaviour
{
    [SerializeField]
    private Text _text = null;

    public void Display()
    {
        int[] highscores = SaveSystem.LoadHighScores().HighScores;
        _text.text = "Highscores\n";
        foreach (var item in highscores)
        {
            _text.text += "\n" + item ;
        }
    }
}
