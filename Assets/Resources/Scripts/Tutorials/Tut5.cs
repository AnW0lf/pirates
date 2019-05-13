using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut5 : MonoBehaviour
{
    private Island island;
    private bool active;
    private BigDigit maxExp;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        maxExp = island.GetMaxExp();
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            active = false;
        }
    }

    private void Update()
    {
        if (!active && maxExp <= island.Exp)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                active = true;
            }
        }
        if (island.Level > 1)
        {
            GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
