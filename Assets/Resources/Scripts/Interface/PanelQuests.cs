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
    [SerializeField] private List<string> texts = null;

    private Island island;
    private int questLevel = 0;

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
            transform.localPosition += Vector3.down * 200f;
        else
            StartCoroutine(Open());
    }

    public void UpdateInfo()
    {
        if (questLevel >= texts.Count || questLevel >= levels.Count) return;
        progressBar.fillAmount = (float)island.Level / levels[questLevel];
        lvlText.text = island.Level + "/" + levels[questLevel];
        questText.text = texts[questLevel];
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
        bool Opened() { return opened; }
        yield return new WaitUntil(Opened);
        RectTransform rect = GetComponent<RectTransform>();
        WaitForSeconds wait = new WaitForSeconds(0.025f);
        for (int i = 0; i < 20; i++)
        {
            rect.localPosition += Vector3.down * 10f;
            yield return wait;
        }
        island.SetParameter("QuestOpened", 1);
    }
}
