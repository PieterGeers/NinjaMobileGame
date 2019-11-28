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
        HighscoresData data = SaveSystem.LoadHighScores();
        if (data != null)
        {
            int[] highscores = data.HighScores;
            _text.text = "Highscores\n";
            foreach (var item in highscores)
            {
                _text.text += "\n" + item;
            }
        }
    }
}
