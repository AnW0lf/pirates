using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCatcher : MonoBehaviour
{
    public GameObject flyingText;
    public Sprite[] sprites;

    private BigDigit money = BigDigit.zero;
    private Island island;
    private GameObject _flyingText;
    private Image image;
    private IslandController islandController;

    private void Awake()
    {
        island = Island.Instance();
        image = GetComponent<Image>();
        islandController = transform.parent.parent.parent.parent.parent.GetChild(1).GetComponent<IslandController>();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CatchCoin);
    }

    public void ActivateCoin(BigDigit money)
    {
        gameObject.SetActive(true);
        this.money = money;
        //GetComponentInParent<CapsuleCollider2D>().enabled = false;
    }

    private void OnEnable()
    {
        if (sprites.Length >= 3)
        {
            island = Island.Instance();
            if (island.Level < 25)
            {
                image.sprite = sprites[0];
            }
            else if (island.Level < 50)
            {
                image.sprite = sprites[1];
            }
            else
            {
                image.sprite = sprites[2];
            }
        }
    }

    public void CatchCoin()
    {
        //Taptic.Medium();

        //Write Time for Offline Reward
        island.SetParameter("QuitTime", DateTime.Now.ToString());

        // Set modifiers to 1
        GetComponentInParent<ShipClick>().ship.rewardModifier = 1;
        GetComponentInParent<ShipClick>().ship.raidTimeModifier = 0;

        islandController.GenerateBonusExp(money);

        GetComponentInParent<CapsuleCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
