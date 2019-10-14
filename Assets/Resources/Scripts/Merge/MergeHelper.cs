using UnityEngine;
using System.Collections;

public class MergeHelper : MonoBehaviour
{
    public int minLevel = 2, maxLevel = 7;
    public float delay = 5f;
    public GameObject helperPref;

    private Inventory inventory;
    private Island island;
    private float timer;
    private Vector2Int range;
    private bool filled;
    private GameObject helper;
    private Panel targetPanel;
    private int targetCell;

    private void Start()
    {
        inventory = Inventory.Instance;
        island = Island.Instance;
        timer = 0f;
    }

    private void Update()
    {
        if (island.Level >= minLevel && island.Level <= maxLevel)
        {
            if (condition)
            {
                if (timer >= delay)
                    GetHelper();
                timer += Time.deltaTime;
            }
            else timer = 0f;
        }
        else if (island.Level > maxLevel) Destroy();

        if (conditionOut)
        {
            RemoveHelper();
        }
    }

    private bool condition
    {
        get
        {
            if (!filled)
            {
                for (int i = 1; i < 5; i++)
                {
                    ShipInfo a = inventory.selectedPanel.items[i - 1];
                    ShipInfo b = inventory.selectedPanel.items[i];
                    if (a && b && a == b)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    private bool conditionOut
    {
        get
        {
            return !filled
                || targetPanel != inventory.selectedPanel
                || !targetPanel.items[targetCell]
                || !targetPanel.items[targetCell + 1]
                || targetPanel.items[targetCell] != targetPanel.items[targetCell + 1];
        }
    }

    private void Destroy()
    {
        RemoveHelper();
        enabled = false;
    }

    private void GetHelper()
    {
        timer = 0f;
        filled = true;
        targetPanel = inventory.selectedPanel;



        for (int i = 1; i < 5; i++)
        {
            ShipInfo a = targetPanel.items[i - 1];
            ShipInfo b = targetPanel.items[i];
            if (a && b && a == b)
            {
                targetCell = i - 1;
                break;
            }
        }
        helper = Instantiate(helperPref, targetPanel.transform.GetChild(targetCell));
        helper.GetComponent<ITutorial>().Begin();
    }

    private void RemoveHelper()
    {
        if (helper) helper.GetComponent<ITutorial>().Next();
        filled = false;
    }
}
