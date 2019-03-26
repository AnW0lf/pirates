using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public string modifierName, descriptionName;
    public int startPrice;
    public float modifier, startReward;
    public Text descriptionText, rewardText;
    public PriceManager priceText;
    public Image buttonIcon;
    public Color[] buttonColors;
    public LifebuoyManager lifebuoys;

    private int price;
    private float reward;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter(modifierName, 1.0f);
        island.InitParameter(modifierName + "_level", 1);
        SetButtonPrefs();
    }

    private void Update()
    {
        if (island.Money >= price)
            buttonIcon.color = buttonColors[0];
        else
            buttonIcon.color = buttonColors[1];
    }

    private void SetButtonPrefs()
    {
        price = (int)(startPrice * Mathf.Pow(1.5f, (island.GetParameter(modifierName + "_level", 0) - 1)));
        reward = startReward + modifier * (island.GetParameter(modifierName + "_level", 0) - 1);

        descriptionText.text = descriptionName;
        rewardText.text = reward.ToString();
        priceText.prefix = "Upgrade\n";
        priceText.postfix = "$";
        priceText.SetPrice(price);
        island.SetParameter(modifierName, reward);
    }

    public void Upgrade(GlobalUpgradeButton button)
    {
        if (island.ChangeMoney(-button.price))
        {
            island.SetParameter(modifierName + "_level", island.GetParameter(modifierName + "_level", 0) + 1);
            lifebuoys.UpdateInfo();
            SetButtonPrefs();
        }
    }
}
