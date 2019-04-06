﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCatcher : MonoBehaviour
{
    public GameObject flyingText;

    private int money = 0;
    private Island island;
    private GameObject _flyingText;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CatchCoin);
    }

    public void ActivateCoin(int money)
    {
        gameObject.SetActive(true);
        this.money = money;
        GetComponentInParent<CapsuleCollider2D>().enabled = false;
    }

    public void CatchCoin()
    {
        island.ExpUp(money);

        //Write Time for Offline Reward
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToString());

        // Set modifiers to 1
        GetComponentInParent<ShipClick>().ship.rewardModifier = 1;
        GetComponentInParent<ShipClick>().ship.raidTimeModifier = 1;

        _flyingText = Instantiate(flyingText, transform.parent.parent.parent.parent.parent.GetChild(1));
        _flyingText.transform.localPosition = new Vector3(-240f, 0f, 0f);
        _flyingText.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        _flyingText.GetComponent<FlyingText>().exp = true;
        _flyingText.GetComponent<FlyingText>().expText.GetComponent<Text>().text = money.ToString();

        GetComponentInParent<CapsuleCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
