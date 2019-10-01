using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay, tapDelay, modifierMantissa;
    public long modifierExponent;
    public Transform moneySet, clickEffectSet, experienceSet;

    public static BigDigit islandReward;

    private Island island;
    private bool clicked = false, active = false;
    private Animation anim;
    private float time;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private void Start()
    {
        island = Island.Instance;
    }

    private void Update()
    {
        //islandReward = new BigDigit(Mathf.Pow(island.Level, 2.15f) * modifier);

        if (!active && island.Level >= minLevel)
        {
            clicked = true;
            active = true;
            StopAllCoroutines();
            StartCoroutine(GenerateMoney());
        }
    }

    public void Click()
    {
        if (!clicked)
        {
            //Taptic.Light();
            clicked = true;
            StopAllCoroutines();
            StartCoroutine(GenerateMoney());
        }
    }

    public BigDigit GetReward()
    {
        BigDigit digit;
        if (island.Level <= 25)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (int)(Mathf.Pow(island.Level, 2.15f) / 1.6f + 1) * island.moneyBonus;
        else if (island.Level > 25 && island.Level <= 50)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (Mathf.Pow(island.Level, 2.15f) * (island.Level - 25) / 1.5f + 1) * island.moneyBonus;
        else
            digit = new BigDigit(modifierMantissa, modifierExponent) * (Mathf.Pow(island.Level, 2.15f) * (island.Level - 25) * (island.Level - 50) / 1.5f + 1) * island.moneyBonus;
        return digit;
    }

    public void GenerateBonusExp(BigDigit reward)
    {
        if (experienceSet != null && experienceSet.childCount > 0)
        {
            Transform child = experienceSet.GetChild(0);
            child.SetAsLastSibling();
            child.GetComponent<IslandFlyingExperience>().Fly(reward);
        }
        island.ExpUp(reward);
    }

    public void GenerateBonusMoney(BigDigit reward)
    {
        if (moneySet != null && moneySet.childCount > 0)
        {
            Transform child = moneySet.GetChild(0);
            child.SetAsLastSibling();
            child.GetComponent<IslandFlyingCoin>().Fly(reward);
        }
        island.ChangeMoney(reward);
    }


    private IEnumerator GenerateMoney()
    {
        if (clicked)
        {
            time = tapDelay;
            if (clickEffectSet != null && clickEffectSet.childCount > 0)
            {
                Transform child = clickEffectSet.GetChild(0);
                child.SetAsLastSibling();
                child.gameObject.SetActive(false);
                child.gameObject.SetActive(true);
            }
        }
        //else if ((delay - (island.GetParameter("Level", 0) - 1) / 10) > tapDelay)
        //{
        //    time = delay - (island.GetParameter("Level", 0) - 1) / 50;
        //}
        else
        {
            time = island.Level > 10 ? 0.6f : 1f - ((island.Level - 1) * 0.04f);
        }

        anim.Play("OnePulse");

        BigDigit reward = GetReward();

        GenerateBonusMoney(reward);

        if (clicked) EventManager.SendEvent("AddMoneyPulse");
        yield return new WaitForSeconds(time / 2);
        clicked = false;
        yield return new WaitForSeconds(time / 2);
        StartCoroutine(GenerateMoney());
    }
}
