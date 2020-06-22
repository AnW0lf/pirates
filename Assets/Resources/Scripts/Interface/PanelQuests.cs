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
    [SerializeField] private LevelReward[] levels = null;
    [SerializeField] private string textPattern = null;
    [SerializeField] private GameObject message = null;
    [SerializeField] List<BonusGenerator> bgs = null;

    private Island island;
    private int questLevel = 0;

    public LevelReward[] Levels { get => levels; }

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
        if (questLevel >= levels.Length) return;
        progressBar.fillAmount = (float)island.Level / levels[questLevel].level;
        lvlText.text = island.Level + "/" + levels[questLevel].level;
        questText.text = string.Format(textPattern, levels[questLevel].level);
        rankText.text = (questLevel + 1).ToString();
        if (island.Level >= levels[questLevel].level)
        {
            button.SetActive(true);
        }
    }

    public void QuestDone()
    {
        StartCoroutine(Reward(levels[questLevel].rewardType));
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

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(start, end, timer / 0.5f);
            yield return null;
        }

        island.SetParameter("QuestOpened", 1);
    }

    private IEnumerator Reward(QuestRewardType type)
    {
        message.SetActive(false);
        message.SetActive(true);
        Animation anim = message.GetComponent<Animation>();

        yield return new WaitWhile(() => { return anim.IsPlaying("PolundraAnimation"); });

        WaitForSeconds sec = new WaitForSeconds(0.2f);

        int bonusIndex = 4;

        switch (type)
        {
            case QuestRewardType.MONEY:
                bonusIndex = 2;
                break;
            case QuestRewardType.SPEED:
                bonusIndex = 1;
                break;
            case QuestRewardType.EXP:
                bonusIndex = 0;
                break;
            default: break;
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j <= island.Level / 25 && j < bgs.Count; j++)
            {
                if (type == QuestRewardType.RANDOM)
                    bgs[j].InstantiateRandomBonus(1);
                else
                    bgs[j].Bonus(bonusIndex, 1);
            }
            yield return sec;
        }

        yield return null;
    }

    public enum QuestRewardType { MONEY, SPEED, EXP, RANDOM }

    [Serializable]
    public class LevelReward
    {
        public int level;
        public QuestRewardType rewardType;
    }
}
