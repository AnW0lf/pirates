using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    [Header("Unlock Level")]
    public int unlockLevel = 4;
    public Button btnOpen;
    [Header("Speed")]
    public Button btnSpeed;
    public Text txtSpeedBonus, txtSpeedPrice;
    public Image imgSpeedPriceIcon;
    public BigDigit priceSpeed;
    public int speedMaxLevel = 10;
    [Header("Money")]
    public Button btnMoney;
    public Text txtMoneyBonus, txtMoneyPrice;
    public Image imgMoneyPriceIcon;
    public BigDigit priceMoney;
    public int moneyMaxLevel = 10;
    [Header("Spin")]
    public Button btnSpin;
    public Text txtSpinBonus, txtSpinPrice;
    public Image imgSpinPriceIcon;
    public BigDigit priceSpin;
    public int spinMaxLevel = 10;
    [Header("Flag")]
    public GameObject flag;
    [Header("Flag")]
    public Text counter;

    private int lvlSpeed, lvlMoney, lvlSpin;
    private float symbolLength = 35f;

    private void Awake()
    {
        btnOpen.gameObject.SetActive(false);
    }

    private void Start()
    {
        lvlSpeed = Island.Instance.SpeedLevel;
        lvlMoney = Island.Instance.MoneyLevel;
        lvlSpin = Island.Instance.SpinLevel;

        EventManager.Subscribe("ChangeMoney", CheckActives);
        EventManager.Subscribe("ChangePremium", UpdateCounter);
        EventManager.Subscribe("LevelUp", CheckUpgradeUnlock);

        string str = Island.Instance.speedBonus.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtSpeedPrice.text = GetPrice(priceSpeed, lvlSpeed).ToString();
        txtSpeedPrice.rectTransform.sizeDelta = new Vector2(txtSpeedPrice.text.Length * symbolLength, txtSpeedPrice.rectTransform.sizeDelta.y);
        txtSpeedBonus.text = "X" + str;

        str = Island.Instance.moneyBonus.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtMoneyPrice.text = GetPrice(priceMoney, lvlMoney).ToString();
        txtMoneyPrice.rectTransform.sizeDelta = new Vector2(txtMoneyPrice.text.Length * symbolLength, txtMoneyPrice.rectTransform.sizeDelta.y);
        txtMoneyBonus.text = "X" + str;

        str = Island.Instance.LifebuoyMax.ToString();
        if (str.Length > 4) str = str.Substring(0, 4);

        txtSpinPrice.text = GetPrice(priceSpin, lvlSpin).ToString();
        txtSpinPrice.rectTransform.sizeDelta = new Vector2(txtSpinPrice.text.Length * symbolLength, txtSpinPrice.rectTransform.sizeDelta.y);
        txtSpinBonus.text = str + "/10";

        CheckUpgradeUnlock(new object[0]);
    }

    private BigDigit GetPrice(BigDigit startPrice, int level)
    {
        return startPrice * (level + 1);
    }

    private void UpdateCounter(object[] args)
    {
        counter.text = Island.Instance.Premium.ToString();
        float width = counter.text.Length * 40f;
        if (counter.text.Contains(".")) width -= 25f;
        counter.rectTransform.sizeDelta = new Vector2(width, counter.rectTransform.sizeDelta.y);
    }

    private void CheckActives(object[] args)
    {
        BigDigit money = Island.Instance.Premium;
        BigDigit prSpeed = GetPrice(priceSpeed, lvlSpeed);
        BigDigit prMoney = GetPrice(priceMoney, lvlMoney);
        BigDigit prSpin = GetPrice(priceSpin, lvlSpin);

        btnSpeed.interactable = money >= prSpeed && speedMaxLevel > lvlSpeed;
        btnMoney.interactable = money >= prMoney && moneyMaxLevel > lvlMoney;
        btnSpin.interactable = money >= prSpin && spinMaxLevel > lvlSpin;

        if (money >= prSpeed && speedMaxLevel > lvlSpeed
            || money >= prMoney && moneyMaxLevel > lvlMoney
            || money >= prSpin && spinMaxLevel > lvlSpin)
        {
            if (!flag.activeSelf) flag.SetActive(true);
        }
        else if (flag.activeSelf) flag.SetActive(false);
    }

    private void CheckUpgradeUnlock(object[] args)
    {
        if(Island.Instance.Level >= unlockLevel)
        {
            btnOpen.gameObject.SetActive(true);
        }
    }

    public void Upgrade(int n)
    {
        if (n == 0) Upgrade(GlobalUpgradeType.SPEED);
        else if (n == 1) Upgrade(GlobalUpgradeType.MONEY);
        else if (n == 2) Upgrade(GlobalUpgradeType.SPIN);
    }

    public void Upgrade(GlobalUpgradeType type)
    {
        string str;
        switch (type)
        {
            case GlobalUpgradeType.SPEED:
                {
                    Island.Instance.AddSpeedLevel(-GetPrice(priceSpeed, lvlSpeed));
                    lvlSpeed = Island.Instance.SpeedLevel;
                    if (speedMaxLevel > lvlSpeed)
                    {
                        txtSpeedPrice.text = GetPrice(priceSpeed, lvlSpeed).ToString();
                    }
                    else
                    {
                        txtSpeedPrice.text = "Max Grade";
                        imgSpeedPriceIcon.gameObject.SetActive(false);
                    }
                    txtSpeedPrice.rectTransform.sizeDelta = new Vector2(txtSpeedPrice.text.Length * symbolLength, txtSpeedPrice.rectTransform.sizeDelta.y);

                    str = Island.Instance.speedBonus.ToString();
                    if (str.Length > 4) str = str.Substring(0, 4);

                    txtSpeedBonus.text = "X" + str;
                    break;
                }
            case GlobalUpgradeType.MONEY:
                {
                    Island.Instance.AddMoneyLevel(-GetPrice(priceMoney, lvlMoney));
                    lvlMoney = Island.Instance.MoneyLevel;

                    if (moneyMaxLevel > lvlMoney)
                    {
                        txtMoneyPrice.text = GetPrice(priceMoney, lvlMoney).ToString();
                    }
                    else
                    {
                        txtMoneyPrice.text = "Max Grade";
                        imgMoneyPriceIcon.gameObject.SetActive(false);
                    }
                    txtMoneyPrice.rectTransform.sizeDelta = new Vector2(txtMoneyPrice.text.Length * symbolLength, txtMoneyPrice.rectTransform.sizeDelta.y);

                    str = Island.Instance.moneyBonus.ToString();
                    if (str.Length > 4) str = str.Substring(0, 4);

                    txtMoneyBonus.text = "X" + str;
                    break;
                }
            case GlobalUpgradeType.SPIN:
                {
                    Island.Instance.AddSpinLevel(-GetPrice(priceSpin, lvlSpin));
                    lvlSpin = Island.Instance.SpinLevel;

                    if (spinMaxLevel > lvlSpin)
                    {
                        txtSpinPrice.text = GetPrice(priceSpin, lvlSpin).ToString();
                    }
                    else
                    {
                        txtSpinPrice.text = "Max Grade";
                        imgSpinPriceIcon.gameObject.SetActive(false);
                    }
                    txtSpinPrice.rectTransform.sizeDelta = new Vector2(txtSpinPrice.text.Length * symbolLength, txtSpinPrice.rectTransform.sizeDelta.y);

                    str = Island.Instance.LifebuoyMax.ToString();
                    if (str.Length > 4) str = str.Substring(0, 4);

                    txtSpinBonus.text = str + "/10";
                    break;
                }
        }
        CheckActives(new object[0]);
    }
}

public enum GlobalUpgradeType { SPEED, MONEY, SPIN }
