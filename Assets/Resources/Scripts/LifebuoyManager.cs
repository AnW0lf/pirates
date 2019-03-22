using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifebuoyManager : MonoBehaviour
{
    public GlobalUpgradeButton upgrade;

    private Island island;
    private string modifierName;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        modifierName = upgrade.modifierName;
        island.InitParameter(modifierName + "_level", 1);
        island.InitParameter(modifierName + "_current", 1);
    }
}
