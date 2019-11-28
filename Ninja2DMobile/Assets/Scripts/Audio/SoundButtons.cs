using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _effects = new GameObject[2];
    [SerializeField]
    private GameObject[] _songs = new GameObject[2];


    private void Start()
    {
        if (!AudioManager.instance.canPlayEffects)
        {
            _effects[0].SetActive(false);
            _effects[1].SetActive(true);
        }
        if (!AudioManager.instance.canPlaySongs)
        {
            _songs[0].SetActive(false);
            _songs[1].SetActive(true);
        }
    }

    public void MuteSongs()
    {
        AudioManager.instance.ChangeSongs();
    }

    public void MuteEffects()
    {
        AudioManager.instance.ChangeEffect();
    }

}
