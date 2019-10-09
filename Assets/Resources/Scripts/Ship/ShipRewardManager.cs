using UnityEngine;
using System.Collections;

public class ShipRewardManager : MonoBehaviour
{
    public IslandController controller;

    public void AddExp(BigDigit reward)
    {
        controller.GenerateBonusExp(reward);
    }
}
