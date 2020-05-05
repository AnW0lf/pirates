using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

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
        gameObject.SetActive(Island.Instance().Level >= minLevel);

#if UNITY_ANDROID
        btn.interactable = saver.AuthSuccess;
        if(btn.interactable) btn.onClick.AddListener(() => Social.ShowLeaderboardUI());
#endif
    }
}
