using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tut1 : MonoBehaviour
{
    public PierManager pier;

    private Island island;

    private void OnEnable()
    {
        island = Island.Instance();

        if (Screen.safeArea.yMax != Screen.safeArea.height)
        {
            if (GetComponentInChildren<Text>()) GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void Update()
    {
        if (island.Money >= pier.startPrice)
        {
            gameObject.transform.parent.gameObject.GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
