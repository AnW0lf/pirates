using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuManager : MonoBehaviour
{
    public TextManager labelTM, chRaidTimeTM, chProfitTM, gRaidTimeTM, gProfitTM, upgradeBtnTM, detailTM;
    public Button exitBtn, upgradeBtn;
    public Image icon, miniIcon;
    public GameObject lockedFade, grade, description;
    public Text lockedFadeLevel;

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
        if ((pier.maxLvl || pier.minLvl > island.Level) && upgradeBtn.interactable)
        {
            upgradeBtn.interactable = false;
        }
        else
        {
            if (upgradeBtn.interactable && island.Money < pier.GetUpgradeCost() || pier.minLvl > island.Level)
            {
                upgradeBtn.interactable = false;
            }
            if (!upgradeBtn.interactable && island.Money >= pier.GetUpgradeCost() && pier.minLvl <= island.Level)
            {
                upgradeBtn.interactable = true;
            }
        }
    }

    private void UpdateCost()
    {
        if (pier.minLvl > island.Level)
        {
            upgradeBtnTM.prefix = "";
            upgradeBtnTM.text = "LOCKED";
            upgradeBtnTM.postfix = "";
            return;
        }
        else if (pier.maxLvl)
        {
            upgradeBtnTM.prefix = "";
            upgradeBtnTM.text = "MAX LEVEL";
            upgradeBtnTM.postfix = "";
            return;
        }
        if (!pier.shipExist)
            upgradeBtnTM.prefix = "UNLOCK\n";
        else
            upgradeBtnTM.prefix = "UPGRADE\n";
        upgradeBtnTM.text = pier.GetUpgradeCost().ToString();
        upgradeBtnTM.postfix = "(C)";
    }

    private void UpdateInfo()
    {
        labelTM.text = pier.shipName;

        chRaidTimeTM.text = pier.GetRaidTime().ToString();
        chProfitTM.text = pier.GetReward().ToString();

        if (pier.minLvl > island.Level)
        {
            Locked();
            return;
        }
        else if (!pier.shipExist)
        {
            NotBought();
            return;
        }
        else if (pier.maxLvl)
        {
            MaxLevel();
        }
        else
        {
            Unlocked();
        }

        Bought();

        icon.sprite = pier.shipIcon;
    }

    private void Locked()
    {
        miniIcon.sprite = null;
        gRaidTimeTM.text = "---";
        gProfitTM.text = "---";
        grade.SetActive(false);
        miniIcon.gameObject.SetActive(false);
        description.SetActive(true);
        lockedFade.SetActive(true);
        lockedFadeLevel.text = "LEVEL " + pier.minLvl;
    }

    private void Unlocked()
    {
        grade.SetActive(true);
        miniIcon.gameObject.SetActive(true);
        description.SetActive(false);
        lockedFade.SetActive(false);
    }

    private void NotBought()
    {
        Unlocked();
        miniIcon.sprite = pier.shipIcon;
        gRaidTimeTM.text = "---";
        gProfitTM.text = "---";
        detailTM.prefix = "";
        detailTM.text = pier.shipName;
    }

    private void Bought()
    {
        detailTM.prefix = "SAIL";
        if (pier.detailCurrentLvl1 < pier.detailMaxLvl1)
        {
            miniIcon.sprite = pier.detailMiniature1;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime1).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward1).ToString();
            detailTM.text = pier.detailCurrentLvl1 + "/" + pier.detailMaxLvl1;
        }
        else if (pier.detailCurrentLvl2 < pier.detailMaxLvl2)
        {
            miniIcon.sprite = pier.detailMiniature2;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime2).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward2).ToString();
            detailTM.text = pier.detailCurrentLvl2 + "/" + pier.detailMaxLvl2;
        }
        else if (pier.detailCurrentLvl3 < pier.detailMaxLvl3)
        {
            miniIcon.sprite = pier.detailMiniature3;
            gRaidTimeTM.text = (pier.GetRaidTime() + pier.detailChangeRaidTime3).ToString();
            gProfitTM.text = (pier.GetReward() + pier.detailChangeReward3).ToString();
            detailTM.text = pier.detailCurrentLvl3 + "/" + pier.detailMaxLvl3;
        }
        else
        {
            miniIcon.sprite = null;
            gRaidTimeTM.text = pier.GetRaidTime().ToString();
            gProfitTM.text = pier.GetReward().ToString();
        }
    }

    private void MaxLevel()
    {
        grade.SetActive(false);
        miniIcon.gameObject.SetActive(false);
        description.SetActive(true);
        icon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}
