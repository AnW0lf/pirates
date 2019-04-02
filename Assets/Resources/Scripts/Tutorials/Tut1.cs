using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut1 : MonoBehaviour
{
    public PierManager pier;

    private Island island;

    private void OnEnable()
    {
        island = Island.Instance();
    }

    private void Update()
    {
        if (island.Money >= pier.startPrice)
        {
            gameObject.transform.parent.gameObject.GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
