using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveSystem
{
    public static void SaveHighScores(HighscoresScript highscores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highscores.fy";
        FileStream stream = new FileStream(path, FileMode.Create);

        HighscoresData data = new HighscoresData(highscores);

        formatter.Serialize(stream, data);

        stream.Close();
    }


    public static HighscoresData LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.fy";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HighscoresData data = formatter.Deserialize(stream) as HighscoresData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveAudioSettings(AudioManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.fy";
        FileStream stream = new FileStream(path, FileMode.Create);

        SoundData data = new SoundData(manager);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static SoundData LoadAudioSettings()
    {
        string path = Application.persistentDataPath + "/settings.fy";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SoundData data = formatter.Deserialize(stream) as SoundData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
