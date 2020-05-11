using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public string prefix = "";

    private BigDigit expectedMoney, money, oldMoney;
    private TextManager tm;
    private Island island;
    private Coroutine coroutine = null;

    private BigDigit Money
    {
        get => money;
        set
        {
            money = value;
            SetMoneyText();
        }
    }

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        money = new BigDigit(island.Money);
        oldMoney = money;
        expectedMoney = money;
        tm = GetComponent<TextManager>();
        tm.postfix = "";
        Money = money;
    }

    private void SetMoneyText()
    {
        tm.prefix = prefix;
        tm.text = Money.ToString();
    }

    private void Update()
    {
        if (expectedMoney != island.Money)
        {
            expectedMoney = new BigDigit(island.Money);
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(ForceMoney(0.5f));
        }
    }

    private IEnumerator ForceMoney(float duration)
    {
        float time = 0f;
        oldMoney = new BigDigit(Money);

        while (time < duration)
        {
            time += Time.deltaTime;
            if (expectedMoney > oldMoney)
                Money = oldMoney + (time / duration) * (expectedMoney - oldMoney);
            else
                Money = oldMoney - (time / duration) * (oldMoney - expectedMoney);
            yield return null;
        }

        Money = expectedMoney;
        SetMoneyText();

        coroutine = null;
    }
}
