using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public int islandNumber = 1;
    public string modifierName, descriptionName;
    public double startPriceMantissa;
    public long startPriceExponent;
    public float modifier, increase, startReward, maxReward;
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
        island.InitParameter(modifierName + islandNumber.ToString(), 1.0f);
        island.InitParameter(modifierName + islandNumber.ToString() + "_level", 1);
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

            price = (startPrice * Mathf.Pow(increase, (island.GetParameter(modifierName + islandNumber.ToString() + "_level", 0) - 1)));
            reward = startReward + modifier * (island.GetParameter(modifierName + islandNumber.ToString() + "_level", 0) - 1);

            if (reward >= maxReward)
            max = true;

        if (max)
        {
            stateText.text = "Max";
            cost.SetActive(false);
        }
        else
        {
            stateText.text = "Upgrade\n";
            costText.text = price.ToString();
            island.SetParameter(modifierName + islandNumber.ToString(), reward);
        }
        descriptionText.text = descriptionName;

        if (modifierName == "GlobalSpins")
        {
            rewardText.text = reward.ToString() + "/" + maxReward;
        }
        else
        {
            rewardText.text = reward.ToString();
        }


    }

    public void Upgrade(GlobalUpgradeButton button)
    {
        if (!max && island.ChangeMoney(-(button.price)))
        {
            island.SetParameter(modifierName + islandNumber.ToString() + "_level", island.GetParameter(modifierName + islandNumber.ToString() + "_level", 0) + 1);
            lifebuoys.UpdateInfo();
            SetButtonPrefs();
        }
    }
}
