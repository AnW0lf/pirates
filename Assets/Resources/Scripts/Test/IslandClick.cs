using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandClick : MonoBehaviour
{
    public int reward = 1;
    public string planeName;

    private Global global;

    private void Awake()
    {
        global = Global.Instance;
    }

    private void Start()
    {
        global.InitParameter(planeName + "IslandClickReward", reward);
        reward = global.GetParameter(planeName + "IslandClickReward", 0);
    }

    public void SetReward(int reward)
    {
        this.reward = reward;
        global.SetParameter(planeName + "IslandClickReward", reward);
    }

    public void Click()
    {
        if (global.ChangeMoney(reward))
        {
            EventManager.SendEvent("ChangeMoney");
            EventManager.SendEvent(planeName + "IslandClick", reward);
        }
    }
}
