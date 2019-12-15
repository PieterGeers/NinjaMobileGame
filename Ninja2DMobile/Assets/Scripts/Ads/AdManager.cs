using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance = null;

    private string _store_id = "3388784";

    private string _video_ad = "video";
    private string _reward_ad = "rewardedVideo";

    private List<uint> _scores = new List<uint>();

    private int _timesPlayed;

    public bool HasWatchedAd = false;

    public uint Score = 0;
    public float JumpTime = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Monetization.Initialize(_store_id, false);
    }

    private IEnumerator WaitForVideoAd()
    {
        while (!Monetization.IsReady(_video_ad))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(_video_ad) as ShowAdPlacementContent;

        if (ad != null)
            ad.Show(AdSkipped);
    }

    private IEnumerator WaitForRewardedVideoAd()
    {
        while (!Monetization.IsReady(_reward_ad))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(_reward_ad) as ShowAdPlacementContent;

        if (ad != null)
            ad.Show(AdFinished);
    }

    private void DisplayVideoAd()
    {
        Time.timeScale = 0.0f;
        StartCoroutine(WaitForVideoAd());
    }

    public void WatchAdToContinue()
    {
        if (!HasWatchedAd)
        {
            HasWatchedAd = true;
            Time.timeScale = 0.0f;
            StartCoroutine(WaitForRewardedVideoAd());
        }
    }

    public void Home()
    {
        HasWatchedAd = false;
        Time.timeScale = 0.0f;
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        _scores.Add(player.GetScore());
        foreach (var score in _scores)
        {
            if (score >= 50)
            {
                DisplayVideoAd();
                _scores = new List<uint>();
                return;
            }
        }
        Time.timeScale = 1.0f;
    }

    public void IncreaseTimesPlayed()
    {
        ++_timesPlayed;
        HasWatchedAd = false;
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        _scores.Add(player.GetScore());
        if (_timesPlayed >= 5)
        {
            DisplayVideoAd();
            _timesPlayed = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private void AdFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Time.timeScale = 1.0f;
            Player player = FindObjectOfType<Player>().GetComponent<Player>();
            Score = player.GetScore();
            JumpTime = player.GetJumpTime();
            SceneManager.LoadScene(1);
        }
    }


    private void AdSkipped(ShowResult result)
    {
        if (result == ShowResult.Finished || result == ShowResult.Skipped || result == ShowResult.Failed)
        {
            Time.timeScale = 1.0f;
        }
        
    }


}
