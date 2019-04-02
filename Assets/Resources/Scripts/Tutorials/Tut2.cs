using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut2 : MonoBehaviour
{
    public PierManager pier;

    private Island island;

    private void OnEnable()
    {
        island = Island.Instance();
    }

    private void Update()
    {
        if (pier.shipExist)
        {
            gameObject.transform.parent.gameObject.GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
