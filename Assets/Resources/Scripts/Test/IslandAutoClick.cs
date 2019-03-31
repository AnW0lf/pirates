using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandAutoClick : MonoBehaviour
{
    public int reward = 1;
    public float delay = 3f;
    public string planeName;

    private Global global;

    private void Awake()
    {
        global = Global.Instance;
    }

    private void OnEnable()
    {
        EventManager.Subscribe(planeName + "IslandAutoClick", AutoClick);
        EventManager.SendEvent(planeName + "IslandAutoClick");
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(planeName + "IslandAutoClick", AutoClick);
    }

    private void Start()
    {
        global.InitParameter(planeName + "IslandAutoClickReward", reward);
        reward = global.GetParameter(planeName + "IslandAutoClickReward", 0);
    }

    public void SetReward(int reward)
    {
        this.reward = reward;
        global.SetParameter(planeName + "IslandAutoClickReward", reward);
    }

    public void AutoClick(object[] arg0)
    {
        StartCoroutine(Auto());
    }

    private IEnumerator Auto()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        int money = reward;
        yield return wait;
        if (global.ChangeMoney(reward))
        {
            EventManager.SendEvent("ChangeMoney");
            EventManager.SendEvent(planeName + "IslandClick", reward);
        }
        EventManager.SendEvent(planeName + "IslandAutoClick");
    }
}
