using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public string modifierName, descriptionName;
    public int startPrice;
    public float modifier, startReward;
    public Text descriptionText, rewardText, priceText;
    public Image buttonIcon;
    public Color[] buttonColors;

    private int price;
    private float reward;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(modifierName))
            PlayerPrefs.SetFloat(modifierName, 1);
        if (!PlayerPrefs.HasKey(modifierName + "_level"))
            PlayerPrefs.SetInt(modifierName + "_level", 1);

        SetButtonPrefs();
    }

    private void Update()
    {
        if (island.GetMoney() >= price)
            buttonIcon.color = buttonColors[0];
        else
            buttonIcon.color = buttonColors[1];
    }

    private void SetButtonPrefs()
    {
        price = (int)(startPrice * Mathf.Pow(1.3f, (PlayerPrefs.GetInt(modifierName + "_level") - 1)));
        reward = startReward + modifier * (PlayerPrefs.GetInt(modifierName + "_level") - 1);

        descriptionText.text = descriptionName;
        rewardText.text = reward.ToString();
        priceText.text = "Upgrade\n$" + price.ToString();
        PlayerPrefs.SetFloat(modifierName, reward);
    }

    public void Upgrade(GlobalUpgradeButton button)
    {
        if (island.GetMoney() >= button.price)
        {
            PlayerPrefs.SetInt(modifierName + "_level", PlayerPrefs.GetInt(modifierName + "_level") + 1);
            SetButtonPrefs();
        }
    }
}
