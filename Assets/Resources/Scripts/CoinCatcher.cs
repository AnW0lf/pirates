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

        // Set modifiers to 1
        GetComponentInParent<ShipClick>().ship.rewardModifier = 1;
        GetComponentInParent<ShipClick>().ship.raidTimeModifier = 1;

        //Gain Exp
        Levels.curExp += 1;
        PlayerPrefs.SetInt("Exp", Levels.curExp);

        GetComponentInParent<CapsuleCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
