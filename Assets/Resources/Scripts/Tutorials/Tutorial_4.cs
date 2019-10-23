using UnityEngine;
using System.Collections;

public class Tutorial_4 : Tutorial
{
    public GameObject handler, title;

    private Panel panel;
    private Inventory inventory;

    private void Awake()
    {
        handler.SetActive(false);
        title.SetActive(false);
    }

    public override void Begin()
    {
        base.Begin();
        handler.SetActive(true);
        title.SetActive(true);
        inventory = Inventory.Instance;
        if (inventory.panels.Count > 0)
            panel = inventory.panels[0];
        else Next();
    }

    public override bool ConditionOut()
    {
        if (isBegin)
            return inventory.GetShipAlltimeCount(panel.list.islandNumber, 0) >= 3;
        else
            return base.ConditionOut();
    }

    void Update()
    {
        if (isBegin && ConditionOut())
            Next();
    }
}
