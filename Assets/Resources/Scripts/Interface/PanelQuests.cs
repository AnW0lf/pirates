using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelQuests : MonoBehaviour
{
    [SerializeField] private GameObject button = null;
    [SerializeField] private Image progressBar = null;
    [SerializeField] private Text lvlText = null, questText = null, rankText = null;
    [SerializeField] public bool opened = false;
    [SerializeField] private List<int> levels = null;
    [SerializeField] private string textPattern = null;

    private Island island;
    private int questLevel = 0;

    public List<int> Levels { get => levels; }

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter("QuestLevel", 0);
        questLevel = island.GetParameter("QuestLevel", 0);
        UpdateInfo();

        island.InitParameter("QuestOpened", 0);
        opened = island.GetParameter("QuestOpened", 0) != 0;
        if (opened)
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        else
            StartCoroutine(Open());
    }

    public void UpdateInfo()
    {
        if (questLevel >= levels.Count) return;
        progressBar.fillAmount = (float)island.Level / levels[questLevel];
        lvlText.text = island.Level + "/" + levels[questLevel];
        questText.text = string.Format(textPattern, levels[questLevel]);
        rankText.text = (questLevel + 1).ToString();
        if (island.Level >= levels[questLevel])
        {
            button.SetActive(true);
        }
    }

    public void QuestDone()
    {
        questLevel++;
        island.SetParameter("QuestLevel", questLevel);
        button.SetActive(false);
        UpdateInfo();
    }

    private IEnumerator Open()
    {
        RectTransform rect = GetComponent<RectTransform>();
        WaitUntil waitUntil = new WaitUntil(() => { return opened; });

        rect.anchoredPosition = Vector2.up * 200f;

        yield return waitUntil;

        float timer = 0f;
        Vector2 start = rect.anchoredPosition;
        Vector2 end = Vector2.zero;

        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(start, end, timer / 0.5f);
            yield return null;
        }

        island.SetParameter("QuestOpened", 1);
    }
}
