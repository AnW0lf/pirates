using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    [Header("Speed")]
    public Button btnSpeed;
    public Text txtSpeedBonus, txtSpeedPrice;
    public BigDigit priceSpeed;
    [Header("Money")]
    public Button btnMoney;
    public Text txtMoneyBonus, txtMoneyPrice;
    public BigDigit priceMoney;
    [Header("Spin")]
    public Button btnSpin;
    public Text txtSpinBonus, txtSpinPrice;
    public BigDigit priceSpin;
    [Header("Flag")]
    public GameObject flag;

    private bool opened = false;
    private int lvlSpeed, lvlMoney, lvlSpin;

    private void Start()
    {
        lvlSpeed = Island.Instance.SpeedLevel;
        lvlMoney = Island.Instance.MoneyLevel;
        lvlSpin = Island.Instance.SpinLevel;

        EventManager.Subscribe("ChangeMoney", CheckActives);

        string str = Island.Instance.speedBonus.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtSpeedPrice.text = GetPrice(priceSpeed, lvlSpeed).ToString();
        txtSpeedBonus.text = "X" + str;

        str = Island.Instance.moneyBonus.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtMoneyPrice.text = GetPrice(priceMoney, lvlMoney).ToString();
        txtMoneyBonus.text = "X" + str;

        str = Island.Instance.LifebuoyMax.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtSpinPrice.text = GetPrice(priceSpin, lvlSpin).ToString();
        txtSpinBonus.text = str + "/10";
    }

    private BigDigit GetPrice(BigDigit startPrice, int level)
    {
        return startPrice * (level + 1);
    }

    private void CheckActives(object[] args)
    {
        BigDigit money = Island.Instance.Money;
        BigDigit prSpeed = GetPrice(priceSpeed, lvlSpeed);
        BigDigit prMoney = GetPrice(priceMoney, lvlMoney);
        BigDigit prSpin = GetPrice(priceSpin, lvlSpin);

        if (money >= prSpeed)
        {
            btnSpeed.interactable = true;
        }
        else
        {
            btnSpeed.interactable = false;
        }

        if (money >= prMoney)
        {
            btnMoney.interactable = true;
        }
        else
        {
            btnMoney.interactable = false;
        }

        if (money >= prSpin)
        {
            btnSpin.interactable = true;
        }
        else
        {
            btnSpin.interactable = false;
        }

        if (money >= prSpeed || money >= prMoney || money >= prSpin)
        {
            if (!flag.activeSelf) flag.SetActive(true);
        }
        else if (flag.activeSelf) flag.SetActive(false);
    }

    public void Upgrade(int n)
    {
        if (n == 0) Upgrade(GlobalUpgradeType.SPEED);
        else if (n == 1) Upgrade(GlobalUpgradeType.MONEY);
        else if (n == 2) Upgrade(GlobalUpgradeType.SPIN);
    }

    public void Upgrade(GlobalUpgradeType type)
    {
        switch (type)
        {
            case GlobalUpgradeType.SPEED:
                {
                    Island.Instance.AddSpeedLevel(-GetPrice(priceSpeed, lvlSpeed));
                    lvlSpeed = Island.Instance.SpeedLevel;
                    txtSpeedPrice.text = GetPrice(priceSpeed, lvlSpeed).ToString();
                    txtSpeedBonus.text = "X" + Island.Instance.speedBonus.ToString().Substring(0, 4);
                    break;
                }
            case GlobalUpgradeType.MONEY:
                {
                    Island.Instance.AddMoneyLevel(-GetPrice(priceMoney, lvlMoney));
                    lvlMoney = Island.Instance.MoneyLevel;
                    txtMoneyPrice.text = GetPrice(priceMoney, lvlMoney).ToString();
                    txtMoneyBonus.text = "X" + Island.Instance.moneyBonus.ToString().Substring(0, 4);
                    break;
                }
            case GlobalUpgradeType.SPIN:
                {
                    Island.Instance.AddSpinLevel(-GetPrice(priceSpin, lvlSpin));
                    lvlSpin = Island.Instance.SpinLevel;
                    txtSpinPrice.text = GetPrice(priceSpin, lvlSpin).ToString();
                    txtSpinBonus.text = Island.Instance.LifebuoyMax + "/10";
                    break;
                }
        }
        CheckActives(new object[0]);
    }
}

public enum GlobalUpgradeType { SPEED, MONEY, SPIN }
