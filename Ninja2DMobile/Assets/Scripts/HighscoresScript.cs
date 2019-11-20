using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoresScript : MonoBehaviour
{
    private int[] _highscores = new int[5];

    public void SaveHighScores()
    {
        SaveSystem.SaveHighScores(this);
    }

    public void LoadPlayer()
    {
        HighscoresData data = SaveSystem.LoadHighScores();
        if (data == null)
        {
            for (uint i = 0; i < 5; ++i)
                _highscores[i] = 0;
        }
        else
            _highscores = data.HighScores;
    }

    public int[] GetHighScores()
    {
        return _highscores;
    }
}
