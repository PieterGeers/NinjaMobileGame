using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private void Start()
    {
        if (!AudioManager.instance.IsSongPlaying("GamePlay"))
        {
            AudioManager.instance.StopSong("MainMenu");
            AudioManager.instance.PlaySong("GamePlay");
        }
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("Menu");
    }
}