using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelFadeManager : MonoBehaviour
{
    public int level;
    public GameObject bonuses, screenUI, wheel;
    public TextManager text;

    private Island island;
    private Button btn;

    private void Awake()
    {
        island = Island.Instance();
        btn = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        text.text = level.ToString();
        wheel.SetActive(false);
        bonuses.SetActive(false);
        screenUI.SetActive(false);
        btn.interactable = false;
        if (island.Level >= level)
            Unlock();
    }

    private void Update()
    {
        if (island.Level >= level && !btn.interactable)
        {
            btn.interactable = true;
            btn.onClick.AddListener(Unlock);
        }
    }

    private void Unlock()
    {
        wheel.SetActive(true);
        bonuses.SetActive(true);
        screenUI.SetActive(true);
        Destroy(gameObject);
    }
}
