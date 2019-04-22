using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBEventController : MonoBehaviour
{
    private Island island;

    private void Awake()
    {
        island = Island.Instance();

        // FACEBOOK SDK
        FB.Init();
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            FB.Init(() => { FB.ActivateApp(); });
        }
    }

    void Start()
    {
        EventManager.Subscribe("ShipBought", ShipBought);

        EventManager.Subscribe("ShipUpgraded", ShipUpgraded);

        EventManager.Subscribe("ShipGoToRaid", ShipGoToRaid);

        EventManager.Subscribe("LevelUp", LevelUp);

        EventManager.Subscribe("DailyBonusCollected", DailyBonusCollected);

        EventManager.Subscribe("WheelSpinned", WheelSpinned);

        EventManager.Subscribe("UpgradeBought", UpgradeBought);

        EventManager.Subscribe("BonusCollected", BonusCollected);
    }

    private void BonusCollected(object[] arg0)
    {
        string shipName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        string bonusName = arg0.Length > 1 ? (string)arg0[1] : "Unknown";
        LogBonusCollectedEvent(island.Level, shipName, bonusName, 1f);

        //Debug.Log("BonusCollectedEvent : " + island.Level + " : " + shipName + " : " + bonusName + " : " + 1f);
    }

    private void UpgradeBought(object[] arg0)
    {
        string modifierName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        int modifierLevel = arg0.Length > 1 ? (int)arg0[1] : 0;
        int islandNumber = arg0.Length > 2 ? (int)arg0[2] : 0;
        LogUpgradeBoughtEvent(island.Level, modifierName, modifierLevel, islandNumber, 0f);

        //Debug.Log("UpgradeBoughtEvent : " + island.Level + " : " + modifierName + " : " + modifierLevel + " : " + islandNumber + " : " + 0f);
    }

    private void WheelSpinned(object[] arg0)
    {
        string wheelName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        int sectorNumber = arg0.Length > 1 ? (int)arg0[1] : 0;
        LogWheelSpinnedEvent(island.Level, wheelName, sectorNumber, 1f);

        //Debug.Log("WheelSpinnedEvent : " + island.Level + " : " + wheelName + " : " + sectorNumber + " : " + 1f);
    }

    private void DailyBonusCollected(object[] arg0)
    {
        int bonusDay = arg0.Length > 0 ? (int)arg0[0] : 0;
        LogDailyBonusCollectedEvent(bonusDay, island.Level, 1f);

        //Debug.Log("DailyBonusCollectedEvent : " + bonusDay + " : " + island.Level + " : " + 1f);
    }

    private void ShipGoToRaid(object[] arg0)
    {
        string shipName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        bool autoRaid = arg0.Length > 1 ? (bool)arg0[1] : true;
        LogShipGoToRaidEvent(shipName, autoRaid, (autoRaid ? 0f : 1f));

        //Debug.Log("ShipGoToRaidEvent : " + shipName + " : " + autoRaid + " : " + (autoRaid ? 0f : 1f));
    }

    private void ShipUpgraded(object[] arg0)
    {
        string shipName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        int shipLevel = arg0.Length > 1 ? (int)arg0[1] : 0;
        LogShipUpgradedEvent(shipName, island.Level, shipLevel, 0f);

        //Debug.Log("ShipUpgradedEvent : " + shipName + " : " + island.Level + " : " + shipLevel + " : " + 0f);
    }

    private void ShipBought(object[] arg0)
    {
        string shipName = arg0.Length > 0 ? (string)arg0[0] : "Unknown";
        LogShipBoughtEvent(shipName, island.Level, 0f);

        //Debug.Log("ShipBoughtEvent : " + shipName + " : " + island.Level + " : " + 0f);
    }

    private void LevelUp(object[] arg0)
    {
        LogLevelUpEvent(island.Level, island.Money, 1f);

        //Debug.Log("LevelUpEvent : " + island.Level + " : " + island.Money + " : " + 1f);
    }

    /// <summary>
    /// Этот метод отправляет событие о повышении уровня
    /// </summary>
    /// <param name="level"></param> значение уровня
    /// <param name="curMoney"></param> текущее количество денег
    /// <param name="valToSum"></param> значение для суммирования
    public void LogLevelUpEvent(int level, BigDigit curMoney, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["Level"] = level,
            ["CurMoney"] = curMoney.Exponent >= 308 ? double.MaxValue : (double)(Math.Pow(10d, curMoney.Exponent) * curMoney.Mantissa)
        };
        FB.LogAppEvent(
            "LevelUp",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о том, что корабль ушел в рейд
    /// </summary>
    /// <param name="shipName"></param> имя корабля
    /// <param name="autoRaid"></param> флаг авторейда
    /// <param name="valToSum"></param> значение для суммирования
    public void LogShipGoToRaidEvent(string shipName, bool autoRaid, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["ShipName"] = shipName,
            ["AutoRaid"] = autoRaid
        };
        FB.LogAppEvent(
            "Raid",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о том, что корабль был куплен
    /// </summary>
    /// <param name="shipName"></param> имя корабля
    /// <param name="level"></param> уровень игрока
    /// <param name="valToSum"></param> значение для суммирования
    public void LogShipBoughtEvent(string shipName, int level, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["ShipName"] = shipName,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "ShipBought",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о том, что корабль был улучшен
    /// </summary>
    /// <param name="shipName"></param> имя корабля
    /// <param name="level"></param> уровень игрока
    /// <param name="shipLevel"></param> уровень корабля
    /// <param name="valToSum"></param> значение для суммирования
    public void LogShipUpgradedEvent(string shipName, int level, int shipLevel, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["ShipName"] = shipName,
            ["ShipLevel"] = shipLevel,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "ShipUpgraded",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о том, что был собран бонус за день
    /// </summary>
    /// <param name="bonusDay"></param> номер дня в стрике
    /// <param name="level"></param> уровень игрока
    /// <param name="valToSum"></param> значение для суммирования
    public void LogDailyBonusCollectedEvent(int bonusDay, int level, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["BonusDay"] = bonusDay,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "DailyBonusCollected",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о прокрутке рулетки
    /// </summary>
    /// <param name="level"></param> уровень игрока
    /// <param name="wheelName"></param> имя рулетки
    /// <param name="sectorNumber"></param> номер сектора
    /// <param name="valToSum"></param> значение для суммирования
    public void LogWheelSpinnedEvent(int level, string wheelName, int sectorNumber, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["WheelName"] = wheelName,
            ["Sector"] = sectorNumber,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "WheelSpinned",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о покупке глобального улучшения
    /// </summary>
    /// <param name="level"></param> уровень игрока
    /// <param name="modifierName"></param> имя улучшения
    /// <param name="modifierLevel"></param> уровень улучшения
    /// <param name="islandNumber"></param> номер острова
    /// <param name="valToSum"></param> значение для суммирования
    public void LogUpgradeBoughtEvent(int level, string modifierName, int modifierLevel, int islandNumber, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["UpgradeName"] = modifierName,
            ["UpgradeLevel"] = modifierLevel,
            ["IslandNumber"] = islandNumber,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "UpgradeBought",
            valToSum,
            parameters
        );
    }

    /// <summary>
    /// Этот метод отправляет событие о том, что бонус на поле был собран
    /// </summary>
    /// <param name="level"></param> уровень игрока
    /// <param name="shipName"></param> имя корабля
    /// <param name="bonusName"></param> имя бонуса
    /// <param name="valToSum"></param> значение для суммирования
    public void LogBonusCollectedEvent(int level, string shipName, string bonusName, float valToSum)
    {
        var parameters = new Dictionary<string, object>
        {
            ["BonusName"] = bonusName,
            ["ShipName"] = shipName,
            ["Level"] = level
        };
        FB.LogAppEvent(
            "BonusCollected",
            valToSum,
            parameters
        );
    }
}
