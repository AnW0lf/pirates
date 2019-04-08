using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public string modifierName, descriptionName;
    public double startPriceMantissa;
    public long startPriceExponent;
    public float modifier, startReward, maxReward;
    public GameObject cost;
    public Text descriptionText, rewardText, stateText, costText;
    public Image buttonIcon;
    public Color[] buttonColors;
    public LifebuoyManager lifebuoys;

    private BigDigit startPrice, price;
    private bool max = false;
    private float reward;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        startPrice = new BigDigit(startPriceMantissa, startPriceExponent);
        island.InitParameter(modifierName, 1.0f);
        island.InitParameter(modifierName + "_level", 1);
        SetButtonPrefs();
    }

    private void Update()
    {
        if (!max && island.Money >= price)
        {
            buttonIcon.color = buttonColors[0];
        }
        else
        {
            buttonIcon.color = buttonColors[1];
        }
    }

    private void SetButtonPrefs()
    {
        price = (startPrice * Mathf.Pow(1.5f, (island.GetParameter(modifierName + "_level", 0) - 1)));
        reward = startReward + modifier * (island.GetParameter(modifierName + "_level", 0) - 1);

        if (reward >= maxReward)
            max = true;

        if (max)
        {
            stateText.text = "Max grade";
            cost.SetActive(false);
        }
        else
        {
            stateText.text = "Upgrade\n";
            costText.text = price.ToString();
            island.SetParameter(modifierName, reward);
        }
        descriptionText.text = descriptionName;
        rewardText.text = reward.ToString();
    }

    public void Upgrade(GlobalUpgradeButton button)
    {
        if (!max && island.ChangeMoney(BigDigit.Reverse(button.price)))
        {
            island.SetParameter(modifierName + "_level", island.GetParameter(modifierName + "_level", 0) + 1);
            lifebuoys.UpdateInfo();
            SetButtonPrefs();
        }
    }
}
