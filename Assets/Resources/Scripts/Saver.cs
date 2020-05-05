using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Saver : MonoBehaviour
{
    private Island island;

    public bool AuthSuccess { get; private set; } = false;

    private void Awake() { island = Island.Instance(); }

    private void Start()
    {
#if UNITY_IOS
#elif UNITY_ANDROID
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => { AuthSuccess = success; });
#endif
    }

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save()
    {
        island.Save();
    }
}
