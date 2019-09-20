using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevelUp : WindowBase
{
    [SerializeField] protected Text textField;
    [SerializeField] protected List<IslandController> islandsList;

    protected Island island;
    protected BigDigit money;

    private void Start()
    {
        island = Island.Instance;
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
        InterfaceIerarchy ierarchy;
        if ((ierarchy = transform.GetComponentInParent<InterfaceIerarchy>())) ierarchy.Next();
    }

    public void AddLevelReward(float modifier)
    {
        if (!island) island = Island.Instance;

        if (modifier > 0)
            island.ChangeMoney(money * modifier);
        else if (modifier < 0)
            island.ChangeMoney(money * -modifier);
    }

    private void Reward()
    {
        money = BigDigit.zero;

        if (!island) island = Island.Instance;

        float mod;
        switch (island.Level / 25)
        {
            case 0:
                mod = 16f;
                break;
            case 1:
                mod = 180f;
                break;
            case 2:
                mod = 3000f;
                break;
            default:
                mod = 9000f;
                break;
        }

        foreach (IslandController islandCont in islandsList)
        {
            if (islandCont.minLevel <= island.Level)
                money += (islandCont.GetReward() * (island.Level % 25) * mod);
        }

        textField.text = money.ToString();
    }
}
