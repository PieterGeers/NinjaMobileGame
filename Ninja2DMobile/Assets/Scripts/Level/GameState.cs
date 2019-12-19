
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.instance != null && !AudioManager.instance.IsSongPlaying("GamePlay"))
        {
            AudioManager.instance.StopSong("MainMenu");
            AudioManager.instance.PlaySong("GamePlay");
        }
    }

    public void Restart()
    {
        AdManager.Instance.IncreaseTimesPlayed();
        SceneManager.LoadScene(1);
    }

    public void WatchRewardedVideo()
    {
        AdManager.Instance.WatchAdToContinue();
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
        AdManager.Instance.Home();
        SceneManager.LoadScene("Menu");
    }
}