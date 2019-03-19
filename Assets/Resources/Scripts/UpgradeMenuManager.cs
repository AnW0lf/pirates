﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuManager : MonoBehaviour
{
    public TextManager labelTM, chRaidTimeTM, chProfitTM, gRaidTimeTM, gProfitTM, upgradeBtnTM;
    public Button exitBtn, upgradeBtn;
    public Image icon, miniIcon;

    public Sprite body, sail, gun;

    private PierManager pier;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    public void GenerateMenu(PierManager pier)
    {
        gameObject.SetActive(true);
        this.pier = pier;
        UpdateInfo();
        UpdateCost();
        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(pier.Upgrade);
        upgradeBtn.onClick.AddListener(UpdateCost);
        upgradeBtn.onClick.AddListener(UpdateInfo);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        if ((pier.maxLvl || pier.minLvl > Levels.level) && upgradeBtn.interactable)
        {
            upgradeBtn.interactable = false;
        }
        else
        {
            if (upgradeBtn.interactable && island.GetMoney() < pier.GetUpgradeCost() || pier.minLvl > Levels.level)
            {
                upgradeBtn.interactable = false;
            }
            if (!upgradeBtn.interactable && island.GetMoney() >= pier.GetUpgradeCost() && pier.minLvl <= Levels.level)
            {
                upgradeBtn.interactable = true;
            }
        }
    }

    private void UpdateCost()
    {
        if (pier.minLvl > Levels.level || pier.maxLvl) return;
        upgradeBtnTM.text = pier.GetUpgradeCost().ToString();
    }

    private void UpdateInfo()
    {
        labelTM.text = pier.shipName;

        chRaidTimeTM.text = pier.GetRaidTime().ToString();
        chProfitTM.text = pier.GetReward().ToString();

        if (pier.minLvl > Levels.level)
        {
            miniIcon.sprite = null;
            gRaidTimeTM.text = "---";
            gProfitTM.text = "---";
            upgradeBtnTM.text = "LEVEL " + pier.minLvl;
            return;
        }
        else if (!pier.shipExist)
        {
            miniIcon.sprite = pier.shipIcon;
            gRaidTimeTM.text = "---";
            gProfitTM.text = "---";
            upgradeBtnTM.text = "UNLOCK";
            return;
        }

        if (pier.detailCurrentLvl1 < pier.detailMaxLvl1)
        {
            miniIcon.sprite = pier.detailMiniature1;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime1).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward1).ToString();
        }
        else if (pier.detailCurrentLvl2 < pier.detailMaxLvl2)
        {
            miniIcon.sprite = pier.detailMiniature2;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime2).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward2).ToString();
        }
        else if (pier.detailCurrentLvl3 < pier.detailMaxLvl3)
        {
            miniIcon.sprite = pier.detailMiniature3;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime3).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward3).ToString();
        }
        else
        {
            miniIcon.sprite = null;
            gRaidTimeTM.text = pier.GetRaidTime().ToString();
            gProfitTM.text = pier.GetReward().ToString();
            upgradeBtnTM.text = "MAX LEVEL";
        }

        icon.sprite = pier.shipIcon;
    }
}
