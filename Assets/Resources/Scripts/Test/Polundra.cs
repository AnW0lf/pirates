using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Polundra : MonoBehaviour
{
    [SerializeField] List<BonusGenerator> bgs = null;

    public static Polundra Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    public void BeginPolundra()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        int bgNum = Island.Instance.Level / 25;
        WaitForSeconds sec = new WaitForSeconds(0.3f);
        for (int i = 0; i < 10; i++)
        {
            bgs[bgNum].RandomBonus(1);
            yield return sec;
        }
    }
}
