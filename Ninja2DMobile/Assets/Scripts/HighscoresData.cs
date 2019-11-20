using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoresData
{
    public int[] HighScores = new int[5];

    public HighscoresData (HighscoresScript highscores)
    {
        HighScores = highscores.GetHighScores();
    }
}
