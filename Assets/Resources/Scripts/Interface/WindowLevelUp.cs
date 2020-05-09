using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevelUp : WindowBase
{
    [SerializeField] protected Text textField;
    [SerializeField] protected List<IslandController> islandsList;
    [SerializeField] protected List<PiersUpgrade> piersList;
    [SerializeField] protected PanelQuests quest = null;
    [SerializeField] protected Image islandBackground = null, islandFill = null;
    [SerializeField] protected Text islandProgressText = null;

    protected int levelsToShip;
    protected Island island;
    protected BigDigit money;

    private void Awake()
    {
        island = Island.Instance();
        Opened = false;
    }

    public override void Open(object[] args)
    {
        base.Open(args);
        Reward();
        IslandProgress();
    }

    private void IslandProgress()
    {
        float progress = 0f;
        int lessLevel = quest.Levels[quest.Levels.Count - 2],
            greaterLevel = quest.Levels[quest.Levels.Count - 1],
            curLevel = Island.Instance().Level,
            spriteId = 0;

        for(int i = 0; i< quest.Levels.Count; i++)
        {
            if(curLevel <= quest.Levels[i])
            {
                greaterLevel = quest.Levels[i];
                if (i == 0) lessLevel = 0;
                else lessLevel = quest.Levels[i - 1];
                spriteId = i + 1;
                break;
            }
        }

        progress = (float)(curLevel - lessLevel) / (greaterLevel - lessLevel);
        Sprite sprite = null;
        int counter = 0;
        foreach(IslandController ic in islandsList)
        {
            IslandSpriteController isc = ic.GetComponent<IslandSpriteController>();
            if (counter + isc.sprites.Count > spriteId)
            {
                sprite = isc.sprites[spriteId - counter];
                break;
            }
            counter += isc.sprites.Count;
        }

        islandBackground.sprite = sprite;
        islandFill.sprite = sprite;
        islandFill.fillAmount = progress;
        islandProgressText.text = string.Format("Island up: {0}%", Mathf.RoundToInt(progress * 100f));
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }

    public void AddLevelReward(float modifier)
    {
        if (modifier > 0)
            island.ChangeMoney(money * modifier);
        else if (modifier < 0)
            island.ChangeMoney(money * -modifier);
    }

    private void Reward()
    {
        money = BigDigit.zero;

        if (island.Level <= 25)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * island.Level * 16f);
            }
        }
        else if (island.Level > 25 && island.Level <= 50)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * (island.Level - 25) * 180f);
            }
        }
        else
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * (island.Level - 50) * 3000f);
            }
        }



        levelsToShip = 999;
        foreach (PiersUpgrade piers in piersList)
        {
            foreach (PierManager pier in piers.piers)
            {
                if (island.Level + 1 < pier.minLvl)
                {
                    if ((pier.minLvl - (island.Level + 1)) < levelsToShip)
                    {
                        levelsToShip = pier.minLvl - (island.Level + 1);
                    }
                }
            }
        }

        textField.text = money.ToString();
    }
}
