using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PierManager : MonoBehaviour
{
    [Header("|Информация о корабле|")]
    public Sprite shipIcon;
    public int minLvl;
    [Header("Префаб корабля")]
    public GameObject shipPref;
    [Header("Начальная позиция")]
    public float size;
    public float rise;
    public float angle;
    [Header("Начальные характеристики")]
    public string shipName;
    public float startRaidTime;
    public int startReward;
    [Header("Скорость движения")]
    public float speedAngle, speedLinear;
    [Header("Первый параметр")]
    [Header("|Прокачка|")]
    public string detailName1;
    public int detailMaxLvl1, detailCurrentLvl1, detailStartCost1, detailCostIncrease1, detailChangeReward1;
    public float detailChangeRaidTime1;
    public Sprite detailMiniature1;

    [Header("Второй параметр")]
    public string detailName2;
    public int detailMaxLvl2, detailCurrentLvl2, detailStartCost2, detailCostIncrease2, detailChangeReward2;
    public float detailChangeRaidTime2;
    public Sprite detailMiniature2;

    [Header("Третий параметр")]
    public string detailName3;
    public int detailMaxLvl3, detailCurrentLvl3, detailStartCost3, detailCostIncrease3, detailChangeReward3;
    public float detailChangeRaidTime3;
    public Sprite detailMiniature3;

    [Header("|Информация о пристани|")]
    public bool shipExist;
    public bool maxLvl;
    public bool upgradeAvailable;
    public GameObject flag;
    public UpgradeMenuManager upgradeMenu;
    public Transform ships;

    private Island island;
    private GameObject ship;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        if (shipExist)
        {
            if (ship == null) CreateShip();
            else UpdadeShipInfo();
        }
    }

    private void Update()
    {
        if (maxLvl) return;
        if (flag.activeInHierarchy && island.GetMoney() < GetUpgradeCost())
            flag.SetActive(false);
        else if (!flag.activeInHierarchy && island.GetMoney() >= GetUpgradeCost())
            flag.SetActive(true);
        if(shipExist && ship == null)
        {
            CreateShip();
        }
    }

    private void OnMouseUpAsButton()
    {
        OpenMenu();
    }

    private void OpenMenu()
    {
        upgradeMenu.GenerateMenu(this);
    }

    /// <summary>
    /// Создает корабль на основе префаба со всеми начальными параметрами
    /// </summary>
    public void CreateShip()
    {
        if (ship != null) return;
        ship = Instantiate(shipPref, ships);
        ship.GetComponent<Ship>()
            .CreateShip(rise, angle, size, shipIcon, speedAngle, speedLinear, GetRaidTime(), GetReward());
    }

    /// <summary>
    /// Обновляет информацию о Характеристиках корабля.
    /// </summary>
    public void UpdadeShipInfo()
    {
        if (ship != null)
            ship.GetComponent<Ship>().SetRaid(GetRaidTime(), GetReward());
    }

    public float GetRaidTime()
    {
        float time = startRaidTime
            + (detailChangeRaidTime1 * detailCurrentLvl1)
            + (detailChangeRaidTime2 * detailCurrentLvl2)
            + (detailChangeRaidTime3 * detailCurrentLvl3);
        return time < 0f ? 0f : time;
    }

    public int GetReward()
    {
        return startReward
            + (detailChangeReward1 * detailCurrentLvl1)
            + (detailChangeReward2 * detailCurrentLvl2)
            + (detailChangeReward3 * detailCurrentLvl3);
    }

    public int GetUpgradeCost()
    {
        return detailStartCost1 + (detailCurrentLvl1 * detailCostIncrease1)
            + (detailCurrentLvl1 != detailMaxLvl1 ? 0 : (detailStartCost2 + (detailCurrentLvl2 * detailCostIncrease2)
            + (detailCurrentLvl2 != detailMaxLvl2 ? 0 : (detailStartCost3 + (detailCurrentLvl3 * detailCostIncrease3)))));
    }

    public void Upgrade()
    {
        if (maxLvl) return;
        if (!island.ChangeMoney(-GetUpgradeCost())) return;
        if (!shipExist)
        {
            shipExist = true;
        }
        else if (detailCurrentLvl1 < detailMaxLvl1)
        {
            detailCurrentLvl1++;
        }
        else if (detailCurrentLvl2 < detailMaxLvl2)
        {
            detailCurrentLvl2++;
        }
        else if (detailCurrentLvl3 < detailMaxLvl3)
        {
            detailCurrentLvl3++;
        }
        UpdadeShipInfo();
        if (detailCurrentLvl3 == detailMaxLvl3)
        {
            maxLvl = true;
            flag.SetActive(true);
            flag.GetComponent<Image>().color = Color.black;
            flag.GetComponentInChildren<Text>().text = "?";
        }
    }
}
