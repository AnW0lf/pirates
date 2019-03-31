using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pier : MonoBehaviour
{
    public GameObject markers, ship;
    public string planeName, shipName, title;
    public int unlockLevel;
    public bool direction;
    [Header("Speeds")]
    public float linearSpeed;
    public float angleSpeed;
    public float linearSpeedModifier;
    public float angleSpeedModifier;
    [Header("Levels")]
    [Range(0, 100)]
    public int level;
    [Range(0, 100)]
    public int maxLevel;
    [Header("Start Position")]
    [Range(0f, 360f)]
    public float startAngle;
    [Range(250f, 500f)]
    public float rise;
    [Header("Scale")]
    public float startScale;
    [Range(1f, 3f)]
    public float scaleModifier;
    [Header("Reward")]
    public int startReward;
    public int rewardUpgrade;
    public int rewardModifier;
    [Header("Delay")]
    public float startDelay;
    public float delayUpgrade;
    public float delayModifier;
    [Header("Cost")]
    public int startCost;
    public int upgradeCost;
    [Header("Sprites")]
    public Sprite[] sprites;
    [Range(1, 100)]
    public int spriteDelay;

    public enum Marker { LOCK, CHECK, UPGRADE, MAXLEVEL };

    private Global global;
    private Pier pier;

    private void Awake()
    {
        global = Global.Instance;
        pier = GetComponent<Pier>();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ChangeMoney", UpdateMarker);
        EventManager.Unsubscribe(shipName + "Upgrade", Upgrade);
        EventManager.Unsubscribe("LevelUp", UpdateMarker);
        EventManager.Unsubscribe(shipName + "Click", Raid);
        EventManager.Unsubscribe(shipName + "Back", Back);
    }

    private void Start()
    {
        global.InitParameter(shipName + "Level", 0);
        level = global.GetParameter(shipName + "Level", 0);
        OpenShip();
        if (global.Level < unlockLevel) EventManager.SendEvent(shipName + "Marker", Marker.LOCK);
        else if (level == maxLevel) EventManager.SendEvent(shipName + "Marker", Marker.MAXLEVEL);
        else if (global.Money >= GetUpgradeCost()) EventManager.SendEvent(shipName + "Marker", Marker.UPGRADE);
        else EventManager.SendEvent(shipName + "Marker", Marker.CHECK);
    }

    public void OpenUpgradeMenu()
    {
        EventManager.SendEvent("OpenUpgradeMenu", pier);
    }

    private void Subscribe()
    {
        EventManager.Subscribe("ChangeMoney", UpdateMarker);
        EventManager.Subscribe(shipName + "Upgrade", Upgrade);
        EventManager.Subscribe("LevelUp", UpdateMarker);
        EventManager.Subscribe(shipName + "Click", Raid);
        EventManager.Subscribe(shipName + "Back", Back);
    }

    private void Back(object[] parameters)
    {
        if (parameters.Length > 0 && global.AddMaterial((int)parameters[0]))
        {
            EventManager.SendEvent("AddMaterial");
            EventManager.SendEvent(planeName + "AddMaterial", (int)parameters[0]);
        }
        direction = !direction;
        EventManager.SendEvent(shipName + "StartRotate", direction);
    }

    private void Raid(object[] parameters)
    {
        EventManager.SendEvent(shipName + "StopRotate");
        float delay = GetDelay() * delayModifier;
        int reward = GetReward() * rewardModifier;
        float speed = linearSpeed * linearSpeedModifier * (direction ? 1 : -1);
        EventManager.SendEvent(shipName + "Raid", delay, reward, speed);
    }

    private void OpenShip()
    {
        if (level > 0)
        {
            ship.SetActive(true);
            EventManager.SendEvent(shipName + "StartRotate", direction);
            EventManager.SendEvent(shipName + "SetSprite", GetSprite());
            EventManager.SendEvent(shipName + "SetAngle", startAngle);
            EventManager.SendEvent(shipName + "SetAngleSpeed", angleSpeed * angleSpeedModifier);
            EventManager.SendEvent(shipName + "SetRise", rise);
            EventManager.SendEvent(shipName + "SetScale", startScale * Mathf.Pow(scaleModifier, level));
        }
    }

    private void Upgrade(object[] parameters)
    {
        if (level == 0)
        {
            ship.SetActive(true);
            EventManager.SendEvent(shipName + "StartRotate", direction);
            EventManager.SendEvent(shipName + "SetSprite", GetSprite());
            EventManager.SendEvent(shipName + "SetAngle", startAngle);
            EventManager.SendEvent(shipName + "SetAngleSpeed", angleSpeed * angleSpeedModifier);
            EventManager.SendEvent(shipName + "SetRise", rise);
            EventManager.SendEvent(shipName + "SetScale", startScale);
        }
        level = level < maxLevel ? level + 1 : level;
        global.SetParameter(shipName + "Level", level);
        EventManager.SendEvent(shipName + "SetSprite", GetSprite());
        EventManager.SendEvent(shipName + "SetScale", startScale * Mathf.Pow(scaleModifier, level));
        OpenUpgradeMenu();
    }

    private void UpdateMarker(object[] parameters)
    {
        if (global.Level < unlockLevel) EventManager.SendEvent(shipName + "Marker", Marker.LOCK);
        else if (level == maxLevel) EventManager.SendEvent(shipName + "Marker", Marker.MAXLEVEL);
        else if (global.Money >= GetUpgradeCost()) EventManager.SendEvent(shipName + "Marker", Marker.UPGRADE);
        else EventManager.SendEvent(shipName + "Marker", Marker.CHECK);
    }

    public int GetUpgradeCost()
    {
        return startCost + (level * upgradeCost);
    }

    public int GetReward()
    {
        return level == 0 ? startReward : startReward + (rewardUpgrade * (level - 1));
    }

    public float GetDelay()
    {
        return level == 0 ? startDelay : startDelay + (delayUpgrade * (level - 1));
    }

    public Sprite GetSprite()
    {
        if (sprites.Length > level / spriteDelay)
            return sprites[level / spriteDelay];
        else
            return sprites[sprites.Length - 1];
    }
}
