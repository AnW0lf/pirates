using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut6 : MonoBehaviour
{
    public PierManager pier;

    private Island island;
    private GameObject childUpgrade;


    private void Awake()
    {
        island = Island.Instance();
        childUpgrade = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        childUpgrade.SetActive(false);
    }

    private void Update()
    {
        if (pier.GetUpgradeCost() <= island.Money && !pier.maxLvl)
        {
            if (!childUpgrade.activeInHierarchy)
                childUpgrade.SetActive(true);
        }
        else
        {
            if (childUpgrade.activeInHierarchy)
                childUpgrade.SetActive(false);
        }
        if (island.Level > 2)
        {
            GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
