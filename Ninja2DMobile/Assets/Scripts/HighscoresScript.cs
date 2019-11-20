using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoresScript : MonoBehaviour
{
    [SerializeField]
    private int[] _highscores = new int[5];

    private void Start()
    {
        LoadHighScores();
    }

    private void SaveHighScores()
    {
        SaveSystem.SaveHighScores(this);
    }

    private void LoadHighScores()
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

    public bool EveluateScore(uint score)
    {
        if (score <= _highscores[4])
            return false;

        if (score > _highscores[0])
        {
            for (int i = 3; i >= 0; --i)
            {
                _highscores[i + 1] = _highscores[i];
            }
            _highscores[0] = (int)score;
            SaveHighScores();
            return true;
        }   


        for (uint i = 0; i < 5; ++i)
        {
            if (score > _highscores[i])
            {
                for (int j = 3; j >= i; --j)
                {
                    _highscores[j + 1] = _highscores[j];
                }
                _highscores[i] = (int)score;
                SaveHighScores();
                return false;
            }
        }

        return false;
    }
}
