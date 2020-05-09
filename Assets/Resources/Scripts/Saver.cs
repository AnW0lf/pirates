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
        PlayGamesPlatform.Activate();
#endif
        Social.localUser.Authenticate((bool success) =>
        {
            AuthSuccess = success;
            Debug.Log("onProcessAuthentication: " + success);
        });
    }

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save()
    {
        Island.Instance().Save();
    }
}
