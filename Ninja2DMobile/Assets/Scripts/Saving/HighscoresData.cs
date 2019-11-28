[System.Serializable]
public class HighscoresData
{
    public int[] HighScores = new int[5];

    public HighscoresData (HighscoresScript highscores)
    {
        HighScores = highscores.GetHighScores();
    }
}
