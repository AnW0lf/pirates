using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class Saver : MonoBehaviour
{
    private Island island;

    public bool AuthSuccess { get; private set; } = false;

    private void Start()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
#endif

        SignIn();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            AuthSuccess = success;
            Debug.Log("onProcessAuthentication: " + success);
        });
    }

#region Leaderboard
    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        if (Social.localUser.authenticated)
            Social.ReportScore(score, leaderboardId, success => { Debug.Log("onReportScore: " + success); });
    }

    public static void ShowLeaderbordUI()
    {
        Social.ShowLeaderboardUI();
    }
#endregion \Leaderboard

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save()
    {
        Island.Instance().Save();
    }
}
