using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFill : MonoBehaviour
{
    public int minLevel, maxLevel, curExp;
    public Image fill;
    public Text text;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        text.text = "Level " + island.Level;
        fill.fillAmount = (island.Exp / island.GetMaxExp()).ToFloat();
    }
}
