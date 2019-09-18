using System.Collections;
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

    public int shipLevel = 1;
    public ShipInfo item { get; private set; }
    public float duration;

    public GameObject moneyIcon;

    public float rewardModifier { get; set; }
    public float durationModifier { get; set; }

    private ShipClick shipClick;
    private ShipRewardController rewardController;
    private Island island;

    private float rotationDelay, delay;

    public ShipMotor Motor { get; private set; }

    public void SetShip(ShipInfo item, IslandController islandController)
    {
        Motor = GetComponent<ShipMotor>();
        rewardController = GetComponent<ShipRewardController>();
        shipClick = GetComponent<ShipClick>();

        shipClick.islandController = islandController;

        this.item = item;
        Motor.speed = item.speed;
        Motor.duration = item.raidTime;
        img.sprite = item.icon;

        rewardModifier = 1f;
        durationModifier = 0f;

        Motor.duration = duration * durationModifier;

        island = Island.Instance;
        Motor.AddRaidEndAction(new ShipMotor.EmptyAction(Reward));
        Motor.AddRaidEndAction(new ShipMotor.EmptyAction(shipClick.CldrOn));
        Motor.AddRaidMiddleAction(new ShipMotor.EmptyAction(rewardController.EnableIcon));

        rotationDelay = item.distance * 2f * Mathf.PI / item.speed;
        delay = rotationDelay;
    }

    private void Update()
    {
        if (!Motor.isRaid) delay -= Time.deltaTime;
        if (delay < 0f) Click();
    }

    private void Reward()
    {
        if (!island) island = Island.Instance;
        rewardController.DisableIcon();
        island.ExpUp(item.reward * rewardModifier);
        rewardModifier = 1f;
        durationModifier = 0f;
        Motor.durationModifier = durationModifier;
    }

    public void Click()
    {
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
