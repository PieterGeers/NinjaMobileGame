using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHighScores : MonoBehaviour
{
    [SerializeField]
    private Text[] _text = new Text[5];

    public void Display()
    {
        HighscoresData data = SaveSystem.LoadHighScores();
        if (data != null)
        {
            int[] highscores = data.HighScores;
            for (int i = 0; i < 5; i++)
            {
                _text[i].text = highscores[i].ToString();
            }
        }
    }

    public void ShowGlobalHighScores()
    {
        HighscoresData data = SaveSystem.LoadHighScores();
        if (data == null)
            OnlineHighScores.ShowOnlineHighScores(0);
        else
            OnlineHighScores.ShowOnlineHighScores(data.HighScores[0]);
    }
}
