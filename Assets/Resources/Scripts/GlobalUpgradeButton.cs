using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public string modifierName, descriptionName;
    public int startPrice;
    public float modifier, startReward, maxReward;
    public GameObject cost;
    public Text descriptionText, rewardText, stateText, costText;
    public Image buttonIcon;
    public Color[] buttonColors;
    public LifebuoyManager lifebuoys;

    private int price;
    private bool max = false;
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
        price = (int)(startPrice * Mathf.Pow(1.5f, (island.GetParameter(modifierName + "_level", 0) - 1)));
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
            costText.text = CheckRange(price);
            island.SetParameter(modifierName, reward);
        }
        descriptionText.text = descriptionName;
        rewardText.text = reward.ToString();
    }

    public void Upgrade(GlobalUpgradeButton button)
    {
        if (!max && island.ChangeMoney(-button.price))
        {
            island.SetParameter(modifierName + "_level", island.GetParameter(modifierName + "_level", 0) + 1);
            lifebuoys.UpdateInfo();
            SetButtonPrefs();
        }
    }

    private string CheckRange(int v)
    {
        int degree = 0;
        float value = v;
        while (value >= 1000)
        {
            value /= 1000;
            degree++;
        }
        string strValue = value.ToString();
        if (strValue.Length >= 5)
            strValue = strValue.Substring(0, 5);
        switch (degree)
        {
            case 0:
                return strValue;
            case 1:
                return strValue + "K";
            case 2:
                return strValue + "M";
            case 3:
                return strValue + "B";
            case 4:
                return strValue + "T";
            case 5:
                return strValue + "Q";
            case 6:
                return strValue + "A";
            default:
                return strValue + "?";
        }
    }
}
