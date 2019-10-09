﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ShipMotor))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(ShipRewardController))]
[RequireComponent(typeof(ShipClick))]
public class ShipController : MonoBehaviour
{
    public Image img;
    public ShipInfo item { get; private set; }
    public float duration;

    public GameObject moneyIcon;

    public float rewardModifier { get; set; }
    public float durationModifier { get; set; }

    public bool Destroyed { get; set; }

    private ShipClick shipClick;
    private ShipRewardController rewardController;
    private Island island;

    private float rotationDelay, delay, averageLength = 500f;

    public ShipMotor Motor { get; private set; }

    public void SetShip(ShipInfo item, IslandController islandController)
    {
        Motor = GetComponent<ShipMotor>();
        rewardController = GetComponent<ShipRewardController>();
        shipClick = GetComponent<ShipClick>();

        this.item = item;

        shipClick.islandController = islandController;
        shipClick.color = item.pointerColor;

        Motor.speed = item.speed;
        Motor.goToRaidSpeed = item.goToRaidSpeed;
        Motor.backFromRaidSpeed = item.backFromRaidSpeed;
        Motor.duration = item.raidTime;

        Sprite sprite = item.icon;
        float ratio = ((float)sprite.texture.width / (float)sprite.texture.height);
        float height = GetComponentInParent<RectTransform>().sizeDelta.x * ((float)sprite.texture.height / averageLength) * item.sizeModifier;
        float width = height * ratio;

        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(width, height);

        GetComponent<CapsuleCollider2D>().size = new Vector2(width, height * 1.2f);

        rewardModifier = 1f;
        durationModifier = 0f;

        Motor.duration = duration * durationModifier;

        island = Island.Instance;
        Motor.raidEndActions += new ShipMotor.EmptyAction(Reward);
        Motor.raidEndActions += new ShipMotor.EmptyAction(shipClick.CldrOn);
        Motor.raidMiddleActions += new ShipMotor.EmptyAction(rewardController.EnableIcon);

        rotationDelay = item.distance * Mathf.PI / (item.speed * island.speedBonus);
        delay = rotationDelay;

        Destroyed = false;
    }

    private void Update()
    {
        if (!Motor.isRaid) delay -= Time.deltaTime;
        if (delay < 0f) Click();
    }

    private void Reward()
    {
        if (!island) island = Island.Instance;
        BigDigit reward = item.reward * rewardModifier;
        rewardController.DisableIcon(reward);
        island.ExpUp(reward);
        rewardModifier = 1f;
        durationModifier = 0f;
        Motor.durationModifier = durationModifier;
    }

    public void Click()
    {
        rotationDelay = item.distance * Mathf.PI / (item.speed * island.speedBonus);
        delay = rotationDelay;
        Motor.BeginRaid();
    }

    public float GetRaidTime()
    {
        return duration / Mathf.Pow(2f, durationModifier);
    }

    public void DurationBonus(int bonus)
    {
        durationModifier += bonus;
        Motor.durationModifier = durationModifier;
    }
}
