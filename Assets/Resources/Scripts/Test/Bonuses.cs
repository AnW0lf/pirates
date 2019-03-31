using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    public string planeName;
    public float delay = 10;

    private Bonus[] children;

    private void Awake()
    {
        children = GetComponentsInChildren<Bonus>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(planeName + "AddBonus", AddBonus);
        EventManager.Subscribe(planeName + "GenerateBonus", GenerateBonus);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(planeName + "AddBonus", AddBonus);
        EventManager.Unsubscribe(planeName + "GenerateBonus", GenerateBonus);
    }

    private void Start()
    {
        int r = UnityEngine.Random.Range(0, 3);
        Bonus.BonusType type = Bonus.BonusType.MATERIAL;
        switch (r)
        {
            case 1:
                type = Bonus.BonusType.DELAY;
                break;
            case 2:
                type = Bonus.BonusType.SPIN;
                break;
        }
        EventManager.SendEvent(planeName + "AddBonus", type);
    }

    private void GenerateBonus(object[] arg0)
    {
        if (arg0.Length == 0) return;
        Bonus.BonusType type = (Bonus.BonusType)arg0[0];
        for (int i = 0; i < (int)arg0[1]; i++)
        {
            Generate(type);
        }
    }

    private void AddBonus(object[] arg0)
    {
        if (arg0.Length == 0) return;

        Bonus.BonusType type = (Bonus.BonusType)arg0[0];

        Generate(type);

        StartCoroutine(Wait(delay));
    }

    private void Generate(Bonus.BonusType type)
    {
        List<Bonus> available = new List<Bonus>();

        foreach (Bonus child in children)
        {
            if (!child.active) available.Add(child);
        }

        if (available.Count == 0) return;

        int r = UnityEngine.Random.Range(0, available.Count);

        available[r].AddBonus(type);
    }

    private IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        int r = UnityEngine.Random.Range(0, 3);
        Bonus.BonusType type = Bonus.BonusType.MATERIAL;
        switch (r)
        {
            case 1:
                type = Bonus.BonusType.DELAY;
                break;
            case 2:
                type = Bonus.BonusType.SPIN;
                break;
        }
        EventManager.SendEvent(planeName + "AddBonus", type);
    }
}
