﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PierManager : MonoBehaviour
{
    [Header("|Информация о корабле|")]
    public Sprite shipIcon;
    public int minLvl;
    public string shipDescription;
    public bool black = false;
    [Header("Префаб корабля")]
    public GameObject shipPref;
    [Header("Начальная позиция")]
    public float size;
    public float rise;
    public float angle;
    [Header("Начальные характеристики")]
    public string shipName;
    public int startPrice;
    public float startRaidTime;
    public int startReward;
    [Header("Скорость движения")]
    public float speedAngle, speedLinear, speedRaidModifier;
    [Header("Первый параметр")]
    [Header("|Прокачка|")]
    public string detailName1;
    public int detailMaxLvl1, detailCurrentLvl1, detailStartCost1, detailCostIncrease1, detailChangeReward1;
    public float detailChangeRaidTime1, detailSizeModifier1;
    public Sprite detailMiniature1;
    public List<Sprite> detailShipSprites1;

    [Header("Второй параметр")]
    public string detailName2;
    public int detailMaxLvl2, detailCurrentLvl2, detailStartCost2, detailCostIncrease2, detailChangeReward2;
    public float detailChangeRaidTime2, detailSizeModifier2;
    public Sprite detailMiniature2;
    public List<Sprite> detailShipSprites2;

    [Header("Третий параметр")]
    public string detailName3;
    public int detailMaxLvl3, detailCurrentLvl3, detailStartCost3, detailCostIncrease3, detailChangeReward3;
    public float detailChangeRaidTime3, detailSizeModifier3;
    public Sprite detailMiniature3;
    public List<Sprite> detailShipSprites3;

    [Header("|Информация о пристани|")]
    public bool shipExist;
    public bool maxLvl;
    public GameObject flag;
    public UpgradeMenuManager upgradeMenu;
    public Transform ships;

    private Island island;
    private GameObject ship;

    //Счетчик спасательных кругов. Для расчета бонусов
    public LifebuoyManager lifebuoys;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter(shipName + "_" + detailName1, detailCurrentLvl1);
        island.InitParameter(shipName + "_" + detailName2, detailCurrentLvl2);
        island.InitParameter(shipName + "_" + detailName3, detailCurrentLvl3);
        island.InitParameter(shipName + "_maxLvl", 0);
        island.InitParameter(shipName + "_shipExist", 0);
        island.InitParameter(shipName + "_size", size);
        island.InitParameter(shipName + "_shipIcon", shipIcon.name);

        detailCurrentLvl1 = island.GetParameter(shipName + "_" + detailName1, detailCurrentLvl1);
        detailCurrentLvl2 = island.GetParameter(shipName + "_" + detailName2, detailCurrentLvl2);
        detailCurrentLvl3 = island.GetParameter(shipName + "_" + detailName3, detailCurrentLvl3);
        maxLvl = island.GetParameter(shipName + "_maxLvl", 0) != 0;
        shipExist = island.GetParameter(shipName + "_shipExist", 0) != 0;
        //size = island.GetParameter(shipName + "_size", size);
        //shipIcon = Resources.Load<Sprite>("Sprites\\" + island.GetParameter(shipName + "_shipIcon", ""));

        if (shipExist)
        {
            if (ship == null) CreateShip();
            else UpdadeShipInfo();
        }

        if (maxLvl)
        {
            flag.GetComponent<Image>().color = Color.black;
            flag.GetComponentInChildren<Text>().text = "?";
        }

        GetComponent<Button>().onClick.AddListener(OpenMenu);
    }

    private void Update()
    {
        if (maxLvl) return;
        if (flag.activeInHierarchy && (island.Money < GetUpgradeCost() || minLvl > island.Level))
            flag.SetActive(false);
        else if (!flag.activeInHierarchy && island.Money >= GetUpgradeCost() && minLvl <= island.Level)
            flag.SetActive(true);
        if (shipExist && ship == null)
        {
            CreateShip();
        }
    }

    public void OpenMenu()
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
        ship.transform.SetAsFirstSibling();
        ship.GetComponent<Ship>()._icon.GetComponent<ShipClick>().lifebuoys = lifebuoys;
        ship.GetComponent<Ship>()
            .CreateShip(rise, angle, size, shipIcon, speedAngle, speedLinear, speedRaidModifier, GetRaidTime(), GetReward());
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
        return shipExist ? detailStartCost1 + (detailCurrentLvl1 * detailCostIncrease1)
            + (detailCurrentLvl1 != detailMaxLvl1 ? 0 : (detailStartCost2 + (detailCurrentLvl2 * detailCostIncrease2)
            + (detailCurrentLvl2 != detailMaxLvl2 ? 0 : (detailStartCost3 + (detailCurrentLvl3 * detailCostIncrease3)))))
            : startPrice;
    }

    public void Upgrade()
    {
        if (maxLvl) return;
        if (!black && !island.ChangeMoney(-GetUpgradeCost())) return;
        if (!shipExist)
        {
            shipExist = true;
            island.SetParameter(shipName + "_shipExist", 1);
        }
        else if (detailCurrentLvl1 < detailMaxLvl1)
        {
            detailCurrentLvl1++;
            island.SetParameter(shipName + "_" + detailName1, detailCurrentLvl1);
            if (detailCurrentLvl1 == 1)
                size += detailSizeModifier1;
            if (detailShipSprites1.Count >= detailCurrentLvl1)
            {
                ship.GetComponent<Ship>().SetShip(size, detailShipSprites1[detailCurrentLvl1 - 1]);
                island.SetParameter(shipName + "_shipIcon", detailShipSprites1[detailCurrentLvl1 - 1].name);
            }
            else
                ship.GetComponent<Ship>().SetSize(size);
        }
        else if (detailCurrentLvl2 < detailMaxLvl2)
        {
            detailCurrentLvl2++;
            island.SetParameter(shipName + "_" + detailName2, detailCurrentLvl2);
            if (detailCurrentLvl2 == 1)
                size += detailSizeModifier2;
            if (detailShipSprites2.Count >= detailCurrentLvl2)
            {
                ship.GetComponent<Ship>().SetShip(size, detailShipSprites1[detailCurrentLvl2 - 1]);
                island.SetParameter(shipName + "_shipIcon", detailShipSprites1[detailCurrentLvl2 - 1].name);
            }
            else
                ship.GetComponent<Ship>().SetSize(size);
        }
        else if (detailCurrentLvl3 < detailMaxLvl3)
        {
            detailCurrentLvl3++;
            island.SetParameter(shipName + "_" + detailName3, detailCurrentLvl3);
            if (detailCurrentLvl3 == 1)
                size += detailSizeModifier3;
            if (detailShipSprites2.Count >= detailCurrentLvl2)
            {
                ship.GetComponent<Ship>().SetShip(size, detailShipSprites1[detailCurrentLvl2 - 1]);
                island.SetParameter(shipName + "_shipIcon", detailShipSprites1[detailCurrentLvl3 - 1].name);
            }
            else
                ship.GetComponent<Ship>().SetSize(size);
        }
        UpdadeShipInfo();
        if (detailCurrentLvl3 == detailMaxLvl3)
        {
            maxLvl = true;
            island.SetParameter(shipName + "_maxLvl", 1);
            flag.SetActive(true);
            flag.GetComponent<Image>().color = Color.black;
            flag.GetComponentInChildren<Text>().text = "?";
        }
        island.SetParameter(shipName + "_size", size);
    }
}
