using System.Collections;
[System.Serializable]
public class SoundData
{
    public bool canPlaySongs;
    public bool canPlayEffects;

    public SoundData(AudioManager manager)
    {
        canPlayEffects = manager.canPlayEffects;
        canPlaySongs = manager.canPlaySongs;
    }
}
