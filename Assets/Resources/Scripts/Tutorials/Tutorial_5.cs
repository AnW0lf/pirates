using UnityEngine;
using System.Collections;

public class Tutorial_5 : Tutorial
{
    public GameObject title;
    public float delay = 5f;
    private float timer;
    private bool isTutorialOver;

    private void Awake()
    {
        title.SetActive(false);
    }

    public override void Begin()
    {
        base.Begin();
        timer = delay;
        title.SetActive(true);
        isTutorialOver = Island.Instance.GetParameter("IsTutorialOver", 0) != 0;
    }

    public override bool ConditionOut()
    {
        if (isBegin)
            return timer <= 0f || isTutorialOver;
        else
            return base.ConditionOut();
    }

    public override void Next()
    {
        Island.Instance.SetParameter("IsTutorialOver", 1);
        base.Next();
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
