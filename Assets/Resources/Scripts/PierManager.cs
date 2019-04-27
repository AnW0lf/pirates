using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PierManager : MonoBehaviour
{
    [Header("|Информация о корабле|")]
    public int islandNumber = 1;
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
    public double startPriceMantissa;
    public long startPriceExponent;
    public float startRaidTime;
    public int startReward;
    [Header("Скорость движения")]
    public float speedAngle, speedLinear, speedRaidModifier;
    [Header("Первый параметр")]
    [Header("|Прокачка|")]
    public string detailName1;
    public int detailMaxLvl1, detailCurrentLvl1, detailChangeReward1;
    public float detailChangeRaidTime1, detailSizeModifier1;
    public double detailStartCostMantissa1;
    public long detailStartCostExponent1;
    public double detailCostIncreaseMantissa1;
    public long detailCostIncreaseExponent1;
    public Sprite detailMiniature1;
    public List<Sprite> detailShipSprites1;
    private BigDigit detailStartCost1, detailCostIncrease1;

    [Header("Второй параметр")]
    public string detailName2;
    public int detailMaxLvl2, detailCurrentLvl2, detailChangeReward2;
    public float detailChangeRaidTime2, detailSizeModifier2;
    public double detailStartCostMantissa2;
    public long detailStartCostExponent2;
    public double detailCostIncreaseMantissa2;
    public long detailCostIncreaseExponent2;
    public Sprite detailMiniature2;
    public List<Sprite> detailShipSprites2;
    private BigDigit detailStartCost2, detailCostIncrease2;

    [Header("Третий параметр")]
    public string detailName3;
    public int detailMaxLvl3, detailCurrentLvl3, detailChangeReward3;
    public float detailChangeRaidTime3, detailSizeModifier3;
    public double detailStartCostMantissa3;
    public long detailStartCostExponent3;
    public double detailCostIncreaseMantissa3;
    public long detailCostIncreaseExponent3;
    public Sprite detailMiniature3;
    public List<Sprite> detailShipSprites3;
    private BigDigit detailStartCost3, detailCostIncrease3;

    [Header("|Информация о пристани|")]
    public bool shipExist;
    public bool maxLvl;
    public UpgradeMenuManager upgradeMenu;
    public Transform ships;

    [Header("Цвет указателя")]
    public Color color;
    [Header("Спрайт для меню")]
    public Sprite spriteForMenu;

    private Island island;
    private GameObject ship;
    private int blackMark;

    public BigDigit startPrice { get; private set; }

    //Счетчик спасательных кругов. Для расчета бонусов
    public LifebuoyManager lifebuoys;

    private void Awake()
    {
        island = Island.Instance();
        if (spriteForMenu == null) spriteForMenu = shipIcon;
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
        if(black)
        {
            island.InitParameter(shipName + "_blackMark", 0);
            blackMark = island.GetParameter(shipName + "_blackMark", 0);
        }

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

        startPrice = new BigDigit(startPriceMantissa, startPriceExponent);

        detailStartCost1 = new BigDigit(detailStartCostMantissa1, detailStartCostExponent1);
        detailStartCost2 = new BigDigit(detailStartCostMantissa2, detailStartCostExponent2);
        detailStartCost3 = new BigDigit(detailStartCostMantissa3, detailStartCostExponent3);

        detailCostIncrease1 = new BigDigit(detailCostIncreaseMantissa1, detailCostIncreaseExponent1);
        detailCostIncrease2 = new BigDigit(detailCostIncreaseMantissa2, detailCostIncreaseExponent2);
        detailCostIncrease3 = new BigDigit(detailCostIncreaseMantissa3, detailCostIncreaseExponent3);
    }

    private void Update()
    {
        if (maxLvl) return;
        if (shipExist && ship == null)
        {
            CreateShip();
        }
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
            .CreateShip(rise, angle, size, shipIcon, speedAngle, speedLinear, speedRaidModifier, GetRaidTime(), GetReward(), islandNumber, shipName);
        ship.GetComponentInChildren<ShipClick>().color = color;
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

    public BigDigit GetUpgradeCost()
    {
        //return shipExist ? detailStartCost1 + (detailCurrentLvl1 * detailCostIncrease1)
        //    + (detailCurrentLvl1 != detailMaxLvl1 ? BigDigit.zero : (detailStartCost2 + (detailCurrentLvl2 * detailCostIncrease2)
        //    + (detailCurrentLvl2 != detailMaxLvl2 ? BigDigit.zero : (detailStartCost3 + (detailCurrentLvl3 * detailCostIncrease3)))))
        //    : startPrice;

        if (shipExist)
        {
            if (detailCurrentLvl1 != detailMaxLvl1)
            {
                return detailStartCost1 + (detailCurrentLvl1 * detailCostIncrease1);
            }
            else if (detailCurrentLvl2 != detailMaxLvl2)
            {
                return detailStartCost2 + (detailCurrentLvl2 * detailCostIncrease2);
            }
            else
            {
                return detailStartCost3 + (detailCurrentLvl3 * detailCostIncrease3);
            }
        }
        else
        {
            return startPrice;
        }

    }

    public void Upgrade()
    {
        if (maxLvl) return;
        if (!black && !island.ChangeMoney(BigDigit.Reverse(GetUpgradeCost())) || black && !ChangeBlackMark(-1)) return;
        if (!shipExist)
        {
            shipExist = true;
            island.SetParameter(shipName + "_shipExist", 1);

            EventManager.SendEvent("ShipBought", shipName);
        }
        else if (detailCurrentLvl1 < detailMaxLvl1)
        {
            detailCurrentLvl1++;
            island.SetParameter(shipName + "_" + detailName1, detailCurrentLvl1);

            EventManager.SendEvent("ShipUpgraded", shipName, 1 + detailCurrentLvl1);

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

            EventManager.SendEvent("ShipUpgraded", shipName, 1 + detailCurrentLvl1 + detailCurrentLvl2);

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

            EventManager.SendEvent("ShipUpgraded", shipName, 1 + detailCurrentLvl1 + detailCurrentLvl2 + detailCurrentLvl3);

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
            
        }
        island.SetParameter(shipName + "_size", size);
    }

    public int GetBlackMark()
    {
        return blackMark;

    } 

    public bool ChangeBlackMark(int value)
    {
        if (!maxLvl && blackMark + value >= 0)
        {
            blackMark += value;
            island.SetParameter(shipName + "_blackMark", blackMark);
            return true;
        }
        return false;
    }
}
