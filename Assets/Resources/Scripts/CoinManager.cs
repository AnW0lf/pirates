using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public string prefix = "";

    [SerializeField]
    private BigDigit money;
    private TextManager tm;
    private Island island;

    private void Awake()
    {
        island = Island.Instance;
    }

    private void Start()
    {
        money = BigDigit.zero;
        tm = GetComponent<TextManager>();
        tm.postfix = "";
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        tm.prefix = prefix;
        tm.text = money.ToString();
    }

    private void Update()
    {
        if (money != island.Money)
        {
            money = new BigDigit(island.Money);
            UpdateMoney();
        }
    }
}
