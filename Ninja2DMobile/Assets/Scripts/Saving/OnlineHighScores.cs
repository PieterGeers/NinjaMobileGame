using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class OnlineHighScores : MonoBehaviour
{
    public static bool IsAuthenticated = false;

    private void Start()
    {
        //DontDestroyOnLoad(this);
        AuthenticateUser();
    }

    private void AuthenticateUser()
    {
        if (!IsAuthenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {
                IsAuthenticated = success;
            }
            );
        }
    }

    public static void AddItemToHighScore(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_high_score, (bool success) => { });
    }

    public static void ShowOnlineHighScores(int score)
    {
        if (!IsAuthenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    IsAuthenticated = true;
                }
            }
            );
        }
        if (IsAuthenticated)
        {
            AddItemToHighScore(score);
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
        }
    }
}
