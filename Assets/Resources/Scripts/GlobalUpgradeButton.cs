using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeButton : MonoBehaviour
{
    public Button btnSpeed, btnMoney, btnSpin;
    public Text txtSpeed, txtMoney, txtSpin;
    public float offsetY = 200f, offsetX = 200f;
    public BigDigit priceSpeed, priceMoney, priceSpin;
    public GameObject flag;

    private bool opened = false;
    private int lvlSpeed, lvlMoney, lvlSpin;
    private RectTransform rectBtnSpeed, rectBtnMoney, rectBtnSpin;
    private RectTransform rectTxtSpeed, rectTxtMoney, rectTxtSpin;

    private void Start()
    {
        lvlSpeed = Island.Instance.SpeedLevel;
        lvlMoney = Island.Instance.MoneyLevel;
        lvlSpin = Island.Instance.SpinLevel;

        rectBtnSpeed = btnSpeed.GetComponent<RectTransform>();
        rectBtnMoney = btnMoney.GetComponent<RectTransform>();
        rectBtnSpin = btnSpin.GetComponent<RectTransform>();

        rectTxtSpeed = txtSpeed.GetComponent<RectTransform>();
        rectTxtMoney = txtMoney.GetComponent<RectTransform>();
        rectTxtSpin = txtSpin.GetComponent<RectTransform>();

        EventManager.Subscribe("ChangeMoney", CheckActives);

        txtSpeed.text = GetPrice(priceSpeed, lvlSpeed).ToString();
        txtMoney.text = GetPrice(priceMoney, lvlMoney).ToString();
        txtSpin.text = GetPrice(priceSpin, lvlSpin).ToString();
    }

    public void Switch()
    {
        opened = !opened;
    }

    private void Update()
    {
        Move(rectBtnSpeed, rectTxtSpeed);
        Move(rectBtnMoney, rectTxtMoney);
        Move(rectBtnSpin, rectTxtSpin);
    }

    private void Move(RectTransform rect, RectTransform rectTxt)
    {
        if (opened)
        {
            Vector2 v_Y, v_X;
            if (rect.anchoredPosition.y != (v_Y = Vector2.down * offsetY * (rect.GetSiblingIndex() + 1)).y)
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, v_Y, Time.deltaTime * 1000f * (rect.GetSiblingIndex() + 1));
            if (rectTxt.anchoredPosition.x != (v_X = Vector2.left * offsetX).x)
                rectTxt.anchoredPosition = Vector2.MoveTowards(rectTxt.anchoredPosition, v_X, Time.deltaTime * 1000f);
        }
        else
        {
            if (rect.anchoredPosition.y != 0)
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, Vector2.zero, Time.deltaTime * 1000f * (rect.GetSiblingIndex() + 1));
            if (rectTxt.anchoredPosition.x != 0)
                rectTxt.anchoredPosition = Vector2.MoveTowards(rectTxt.anchoredPosition, Vector2.zero, Time.deltaTime * 1000f);
        }
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
                    txtSpeed.text = GetPrice(priceSpeed, lvlSpeed).ToString();
                    break;
                }
            case GlobalUpgradeType.MONEY:
                {
                    Island.Instance.AddMoneyLevel(-GetPrice(priceMoney, lvlMoney));
                    lvlMoney = Island.Instance.MoneyLevel;
                    txtMoney.text = GetPrice(priceMoney, lvlMoney).ToString();
                    break;
                }
            case GlobalUpgradeType.SPIN:
                {
                    Island.Instance.AddSpinLevel(-GetPrice(priceSpin, lvlSpin));
                    lvlSpin = Island.Instance.SpinLevel;
                    txtSpin.text = GetPrice(priceSpin, lvlSpin).ToString();
                    break;
                }
        }
        CheckActives(new object[0]);
    }
}

public enum GlobalUpgradeType { SPEED, MONEY, SPIN }
