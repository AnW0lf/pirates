using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ShipMotor))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(ShipRewardController))]
public class ShipController : MonoBehaviour
{
    public int shipLevel = 1;
    public float moneyMantissa;
    public int moneyExponent;
    public float moneyModifier, duration, durationModifier;

    public GameObject moneyIcon;

    private ShipMotor motor;
    private ShipRewardController rewardController;
    private Island island;

    public ShipMotor Motor { get => motor; private set => motor = value; }

    private void Awake()
    {
        island = Island.Instance;
        Motor = GetComponent<ShipMotor>();
        rewardController = GetComponent<ShipRewardController>();
        Motor.duration = duration * durationModifier;
    }

    private void Start()
    {
        Motor.AddRaidEndAction(new ShipMotor.EmptyAction(Reward));
        Motor.AddRaidMiddleAction(new ShipMotor.EmptyAction(MoneyIconOn));
    }

    private void Reward()
    {
        MoneyIconOff();
        island.ExpUp(new BigDigit(moneyMantissa, moneyExponent) * moneyModifier);
    }

    private void MoneyIconOn()
    {
        rewardController.EnableIcon();
    }

    private void MoneyIconOff()
    {
        rewardController.DisableIcon();
    }

    public void Click()
    {
        Motor.BeginRaid();
    }
}
