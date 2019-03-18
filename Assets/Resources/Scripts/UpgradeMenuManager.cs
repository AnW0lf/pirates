using System.Collections;
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

    private void FixedUpdate()
    {
        if (gameObject.activeInHierarchy == false) return;
        if (upgradeBtn.interactable == true && island.GetMoney() < pier.GetUpgradeCost())
        {
            upgradeBtn.interactable = false;
        }
        if (upgradeBtn.interactable == false && island.GetMoney() >= pier.GetUpgradeCost())
        {
            upgradeBtn.interactable = true;
        }
    }

    private void UpdateCost()
    {
        upgradeBtnTM.text = pier.GetUpgradeCost().ToString();
    }

    private void UpdateInfo()
    {
        labelTM.text = pier.label;

        int bodyLvl = pier.GetBodyLevel();
        int sailLvl = pier.GetSailLevel();
        int gunLvl = pier.GetGunLevel();

        chRaidTimeTM.text = pier.GetRaidTime(sailLvl).ToString();
        chProfitTM.text = pier.GetReward(bodyLvl, gunLvl).ToString();

        if (bodyLvl < pier.GetMaxLevel())
        {
            miniIcon.sprite = body;
            bodyLvl++;
        }
        else if (sailLvl < pier.GetMaxLevel())
        {
            miniIcon.sprite = sail;
            sailLvl++;
        }
        else if (gunLvl < pier.GetMaxLevel())
        {
            miniIcon.sprite = gun;
            gunLvl++;
        }

        gRaidTimeTM.text = pier.GetRaidTime(sailLvl).ToString();
        gProfitTM.text = pier.GetReward(bodyLvl, gunLvl).ToString();

        icon.sprite = pier.ship.sprite;
    }
}
