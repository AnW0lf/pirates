using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCatcher : MonoBehaviour
{
    private int money = 0;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnMouseUpAsButton()
    {
        CatchCoin();
    }

    public void ActivateCoin(int money)
    {
        gameObject.SetActive(true);
        this.money = money;
        GetComponentInParent<CapsuleCollider2D>().enabled = false;
    }

    public void CatchCoin()
    {
        island.ChangeMoney(money);
        GetComponentInParent<CapsuleCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
