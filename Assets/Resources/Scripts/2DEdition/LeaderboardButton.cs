using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

[RequireComponent(typeof(Button))]
public class LeaderboardButton : MonoBehaviour
{
    public Saver saver;
    public int minLevel = 5;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    void Start()
    {
        CheckLevel();

        if (!gameObject.activeSelf) EventManager.Subscribe("LevelUp", CheckLevel);

#if UNITY_ANDROID
        btn.interactable = saver.AuthSuccess;
        if (btn.interactable)
        {
            (Social.Active as GooglePlayGames.PlayGamesPlatform).SetDefaultLeaderboardForUI("LEADERBOARD_ID");
        }
#endif

#if UNITY_ANDROID || UNITY_IPHONE
        btn.onClick.AddListener(() => Social.ShowLeaderboardUI());
        btn.onClick.AddListener(() => EventManager.SendEvent("RankingsOpened"));
#endif
    }

    private void CheckLevel(object[] arg0)
    {
        CheckLevel();
        if (gameObject.activeSelf) EventManager.Unsubscribe("LevelUp", CheckLevel);
    }

    private void CheckLevel()
    {
        gameObject.SetActive(Island.Instance().Level >= minLevel);
    }
}
