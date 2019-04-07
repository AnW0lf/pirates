using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public TextManager titleTM, upLevelTM, raidTimeTM, rewardTM, detailLevelTM, bonusTM, upBtnTM, fadeLevelTM, descriptionTM;
    public Image icon, miniIcon;
    public GameObject windowFade, iconFade, titleFade, cost;
    public Button exitBtn, upgradeBtn;
    public Text costTxt;

    public Sprite body, sail, gun;

    private PierManager pier;
    private Island island;
    private bool isBlackUpgraded = true;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("ChangeMoney", UpdateInfo);
        EventManager.Subscribe("AddExp", UpdateInfo);
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ChangeMoney", UpdateInfo);
        EventManager.Unsubscribe("AddExp", UpdateInfo);
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    public void GenerateMenu(PierManager pier)
    {
        this.pier = pier;
        if (pier.black)
            isBlackUpgraded = false;

        UpdateInfo();
        upgradeBtn.onClick.RemoveAllListeners();
        if (pier.black)
        {
            BlackShip();
        }
        upgradeBtn.onClick.AddListener(pier.Upgrade);
        upgradeBtn.onClick.AddListener(UpdateInfo);
    }

    private void UpdateInfo()
    {
        if (pier.black)
        {
            BlackShip();

            if (pier.GetBlackMark() > 0 && !pier.shipExist)
                NotBought();
            else if (!pier.shipExist)
                Locked();
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

        if (pier.minLvl <= island.Level && !pier.maxLvl && pier.GetUpgradeCost() <= island.Money)
            upgradeBtn.interactable = true;
        else
            upgradeBtn.interactable = false;
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
        SetState(descriptionTM, pier.shipDescription);
        if (pier.black)
            SetState(upBtnTM, "Catch unlock in Lucky Wheel");
        else
            SetState(upBtnTM, pier.minLvl.ToString(), "LEVEl ");

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

        icon.color = Color.black;
        cost.SetActive(false);
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
        SetState(descriptionTM, pier.shipDescription);

        if (!pier.black)
        {
            SetState(upBtnTM, "Unlock\n");
            cost.SetActive(true);
            costTxt.text = CheckRange(pier.GetUpgradeCost());
        }
        else if (pier.GetBlackMark() > 0)
        {
            SetState(upBtnTM, "Unlock");
            cost.SetActive(false);
        }
        else
        {
            SetState(upBtnTM, "Catch upgrade in Lucky Wheel");
            cost.SetActive(false);
        }

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
        if (pier.black)
            icon.color = Color.black;
        else
            icon.color = Color.white;
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

        if (!pier.black)
        {
            SetState(upBtnTM, "Upgrade\n");
            cost.SetActive(true);
            costTxt.text = CheckRange(pier.GetUpgradeCost());
        }
        else if (pier.GetBlackMark() > 0)
        {
            SetState(upBtnTM, "Upgrade");
            cost.SetActive(false);
        }
        else
        {
            SetState(upBtnTM, "Catch upgrade in Lucky Wheel");
            cost.SetActive(false);
        }

        if (iconFade.activeInHierarchy)
            iconFade.SetActive(false);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);

        icon.color = Color.white;
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

        icon.color = Color.white;
        cost.SetActive(false);
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
        upgradeBtn.onClick.AddListener(IsBlackUpgraded);
    }

    private void IsBlackUpgraded()
    {
        isBlackUpgraded = true;
    }

    private string CheckRange(int v)
    {
        int degree = 0;
        float value = v;
        while (value >= 1000)
        {
            value /= 1000;
            degree++;
        }
        string strValue = value.ToString();
        if (strValue.Length >= 5)
            strValue = strValue.Substring(0, 5);
        switch (degree)
        {
            case 0:
                return strValue;
            case 1:
                return strValue + "K";
            case 2:
                return strValue + "M";
            case 3:
                return strValue + "B";
            case 4:
                return strValue + "T";
            case 5:
                return strValue + "Q";
            case 6:
                return strValue + "A";
            default:
                return strValue + "?";
        }
    }
}
