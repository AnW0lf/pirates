using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float maxClickCount, clickCountDecrease, forcingMoneyDuration, autoclickDelay, modifierMantissa;
    public long modifierExponent;
    public Transform moneySet, clickEffectSet, experienceSet;
    public ProgressBar progressbar;

    public static BigDigit islandReward;

    private Island island;
    private bool active = false, forced = false;
    private RectTransform rect;
    private float clickCounter, autoclickTimer, clickDelay = 0.2f, clickDelayTimer;
    private Vector2 original;
    private IslandSpriteController islandSpriteController;

    private void Awake()
    {
        island = Island.Instance();
        rect = GetComponent<RectTransform>();
        islandSpriteController = GetComponent<IslandSpriteController>();
    }

    private void Start()
    {
        original = rect.sizeDelta;
        autoclickTimer = autoclickDelay;
        clickDelayTimer = 0f;
    }

    private void Update()
    {
        if (!active && island.Level >= minLevel)
        {
            active = true;
        }

        if (active)
        {
            if(autoclickTimer > 0f)
            {
                autoclickTimer -= Time.deltaTime;
            }
            else
            {
                autoclickTimer = autoclickDelay;
                Autoclick();
            }

            if(clickDelayTimer > 0f) clickDelayTimer -= Time.deltaTime;

            if (!forced)
            {
                if(clickCounter > 0f)
                {
                    clickCounter = Mathf.Max(0f, clickCounter - clickCountDecrease * Time.deltaTime);
                }

                if (clickCounter >= 2f)
                {
                    if (!progressbar.Visible) progressbar.Visible = true;
                    progressbar.Progress = clickCounter / maxClickCount;
                    progressbar.Label = (GetReward() * 20).ToString();
                }
            }
            else
            {
                if (clickCounter > 0f)
                {
                    clickCounter = Mathf.Max(0f, clickCounter - maxClickCount / forcingMoneyDuration * Time.deltaTime);
                    progressbar.Progress = clickCounter / maxClickCount;
                    string duration = (clickCounter / maxClickCount * forcingMoneyDuration).ToString();
                    if (duration.Length > 4) duration = duration.Substring(0, 4);
                    progressbar.Label = duration;
                }
                else
                {
                    forced = false;
                    progressbar.Visible = false;
                }
            }
        }
    }

    private void Autoclick()
    {
        if (forced || clickDelayTimer > 0f) return;
        GenerateMoney();
    }

    private void ForceClickReward()
    {
        forced = true;
        GenerateBonusMoney(GetReward() * 20);
        StartCoroutine(ForceMoney());
    }

    private IEnumerator ForceMoney()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        while(clickCounter > 0f)
        {
            yield return delay;
            GenerateBonusMoney(GetReward());
        }
    }

    public void Click()
    {
        if (forced || clickDelayTimer > 0f) return;
        clickDelayTimer = clickDelay;
        GenerateMoney();
        Pulse();
        GenerateEffect();
        clickCounter += 1f;

        if (clickCounter >= maxClickCount)
            ForceClickReward();
    }

    public BigDigit GetReward()
    {
        BigDigit digit;
        if (island.Level <= 25)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (int)(Mathf.Pow(island.Level, 2.15f) / 1.6f + 1);
        else if (island.Level > 25 && island.Level <= 50)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (Mathf.Pow(island.Level, 2.15f) * (island.Level - 25) / 1.5f + 1);
        else
            digit = new BigDigit(modifierMantissa, modifierExponent) * (Mathf.Pow(island.Level, 2.15f) * (island.Level - 25) * (island.Level - 50) / 1.5f + 1);
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
    }

    public void GenerateEffect()
    {
        if (clickEffectSet != null && clickEffectSet.childCount > 0)
        {
            Transform child = clickEffectSet.GetChild(0);
            child.SetAsLastSibling();
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
        }
    }

    private void Pulse()
    {
        rect.LeanSize((islandSpriteController != null ? islandSpriteController.Original : original) * 1.025f, 0.09f);
        LeanTween.delayedCall(0.11f, () => rect.LeanSize(islandSpriteController != null ? islandSpriteController.Original : original, 0.09f));
    }


    private void GenerateMoney()
    {
        BigDigit reward = GetReward();
        GenerateBonusMoney(reward);
        island.ChangeMoney(reward);
    }
}
