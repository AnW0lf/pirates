using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevelUp : WindowBase
{
    [SerializeField] protected Text textField;
    [SerializeField] protected List<IslandController> islandsList;
    [SerializeField] protected List<PiersUpgrade> piersList;

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
