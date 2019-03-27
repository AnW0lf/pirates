using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuManager : MonoBehaviour
{
    public TextManager titleTM, upLevelTM, raidTimeTM, rewardTM, detailLevelTM, bonusTM, upBtnTM, fadeLevelTM, descriptionTM;
    public Image icon, miniIcon;
    public GameObject windowFade, iconFade, titleFade;
    public Button exitBtn, upgradeBtn;

    public Sprite body, sail, gun;

    private PierManager pier;
    private Island island;
    private bool isBlackUpgraded = true;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Update()
    {
        if (pier.black && !isBlackUpgraded && !pier.maxLvl && island.Money >= pier.GetUpgradeCost())
        {
            if (!upgradeBtn.interactable)
                upgradeBtn.interactable = true;
        }
        else
        {
            if (upgradeBtn.interactable)
                upgradeBtn.interactable = false;
        }
    }

    public void GenerateMenu(PierManager pier)
    {
        gameObject.SetActive(true);
        this.pier = pier;
        UpdateInfo();
        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(pier.Upgrade);
        upgradeBtn.onClick.AddListener(UpdateInfo);
        if (pier.black)
            isBlackUpgraded = false;
    }

    private void UpdateInfo()
    {
        if (pier.black)
        {
            BlackShip();
            if (!pier.shipExist)
                NotBought();
            else if (pier.maxLvl)
                MaxLevel();
            else
                Bought();
        }
        else if (pier.minLvl > island.Level)
            Locked();
        else if (!pier.shipExist)
            NotBought();
        else if (pier.maxLvl)
            MaxLevel();
        else
            Bought();
    }

    private void Locked()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, pier.GetReward().ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        SetState(descriptionTM, "Choice of the true corsair");
        SetState(upBtnTM, pier.minLvl.ToString(), "LEVEl ");
        SetState(fadeLevelTM, pier.minLvl.ToString(), "LEVEL ");

        if (!icon.sprite.Equals(pier.shipIcon))
            icon.sprite = pier.shipIcon;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (iconFade.activeInHierarchy)
            iconFade.SetActive(false);
        if (!windowFade.activeInHierarchy)
            windowFade.SetActive(true);
        if (!titleFade.activeInHierarchy)
            titleFade.SetActive(true);
        if (upgradeBtn.interactable)
            upgradeBtn.interactable = false;
    }

    private void NotBought()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, pier.GetReward().ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        SetState(descriptionTM, "Choice of the true corsair");
        SetState(upBtnTM, pier.GetUpgradeCost().ToString(), "Unlock ");
        SetState(fadeLevelTM, pier.minLvl.ToString(), "LEVEL ");

        if (!icon.sprite.Equals(pier.shipIcon))
            icon.sprite = pier.shipIcon;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (!iconFade.activeInHierarchy)
            iconFade.SetActive(true);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
    }

    private void Bought()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        int curLvl = pier.detailCurrentLvl1 + pier.detailCurrentLvl2 + pier.detailCurrentLvl3 + 1;
        SetState(upLevelTM, curLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, pier.GetReward().ToString());

        if (!miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(true);
        if (!icon.sprite.Equals(pier.shipIcon))
            icon.sprite = pier.shipIcon;

        float a = 0f;
        int b = 0;
        if (pier.detailCurrentLvl1 < pier.detailMaxLvl1)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl1.ToString() + "/" + pier.detailMaxLvl1, "HULL ");
            a = pier.detailChangeRaidTime1;
            b = pier.detailChangeReward1;
            miniIcon.sprite = pier.detailMiniature1;
        }
        else if (pier.detailCurrentLvl2 < pier.detailMaxLvl2)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl2.ToString() + "/" + pier.detailMaxLvl2, "SAIL ");
            a = pier.detailChangeRaidTime2;
            b = pier.detailChangeReward2;
            miniIcon.sprite = pier.detailMiniature2;
        }
        else if (pier.detailCurrentLvl3 < pier.detailMaxLvl3)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl3.ToString() + "/" + pier.detailMaxLvl3, "GUNS ");
            a = pier.detailChangeRaidTime3;
            b = pier.detailChangeReward3;
            miniIcon.sprite = pier.detailMiniature3;
        }

        string bonus = (a == 0f ? "" : (a < 0f ? "" : "+") + a.ToString() + "s")
                + (b == 0 ? "" : (a < 0f ? " " : " +") + b.ToString() + "(C)");
        SetState(bonusTM, bonus);

        descriptionTM.gameObject.SetActive(false);
        SetState(upBtnTM, pier.GetUpgradeCost().ToString(), "Upgrade ");

        if (iconFade.activeInHierarchy)
            iconFade.SetActive(false);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
    }

    private void MaxLevel()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, maxLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, pier.GetReward().ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        SetState(descriptionTM, "Choice of the true corsair");
        SetState(upBtnTM, "MAX LEVEL");

        if (!icon.sprite.Equals(pier.shipIcon))
            icon.sprite = pier.shipIcon;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (!iconFade.activeInHierarchy)
            iconFade.SetActive(true);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
        if (upgradeBtn.interactable)
            upgradeBtn.interactable = false;
    }

    private void SetState(TextManager tm, string text, string prefix = "", string postfix = "")
    {
        if (!tm.gameObject.activeInHierarchy)
            tm.gameObject.SetActive(true);
        tm.text = text;
        tm.prefix = prefix;
        tm.postfix = postfix;
    }

    private void BlackShip()
    {
        exitBtn.gameObject.SetActive(false);
        upgradeBtn.onClick.AddListener(IsBlackUpgraded);
    }

    private void IsBlackUpgraded()
    {
        isBlackUpgraded = true;
        exitBtn.gameObject.SetActive(true);
    }
}
