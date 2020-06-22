using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text text;
    private BigDigit expectedMoney, money, oldMoney;
    private Coroutine coroutine = null;

    private BigDigit Money
    {
        get => money;
        set
        {
            money = value;
            text.text = Money.CustomString();
        }
    }

    private void Start()
    {
        money = new BigDigit(Island.Instance().Money);
        oldMoney = money;
        expectedMoney = money;
        Money = money;
    }

    private void Update()
    {
        if (expectedMoney != Island.Instance().Money)
        {
            expectedMoney = new BigDigit(Island.Instance().Money);
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

        coroutine = null;
    }
}
