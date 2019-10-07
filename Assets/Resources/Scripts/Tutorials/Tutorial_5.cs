using UnityEngine;
using System.Collections;

public class Tutorial_5 : Tutorial
{
    public GameObject title;
    public float delay = 5f;
    private float timer;

    private void Awake()
    {
        title.SetActive(false);
    }

    public override void Begin()
    {
        base.Begin();
        timer = delay;
        title.SetActive(true);
    }

    public override bool ConditionOut()
    {
        if (isBegin)
            return timer <= 0f;
        else
            return base.ConditionOut();
    }

    void Update()
    {
        if (isBegin)
        {
            timer -= Time.deltaTime;
            if (ConditionOut())
                Next();
        }

    }
}
