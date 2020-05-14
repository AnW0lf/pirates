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
    public GameObject fontain;

    public static BigDigit islandReward;

    private Island island;
    private bool active = false, forced = false;
    private RectTransform rect;
    private float clickCounter, autoclickTimer, clickDelay = 0.25f, clickTimer, clickDelayTimer, hideClickProgressTime = 2f;
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
        clickTimer = 0f;
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
            if (clickTimer > 0f) clickTimer -= Time.deltaTime;
            else if (clickTimer < 0f) clickTimer = 0f;

            if (!forced)
            {
                if(clickCounter > 0f && clickTimer == 0f)
                {
                    clickCounter = Mathf.Max(0f, clickCounter - clickCountDecrease * Time.deltaTime);
                }

                if (clickCounter >= 2f)
                {
                    if (!progressbar.Visible && hideClickProgressTime > 0f) progressbar.Visible = true;
                    progressbar.Progress = clickCounter / maxClickCount;
                    progressbar.Label = (GetReward() * 20).ToString();
                }
                else if (progressbar.Visible) progressbar.Visible = false;

                if (progressbar.Visible)
                {
                    if (hideClickProgressTime <= 0f) progressbar.Visible = false;
                    hideClickProgressTime -= Time.deltaTime;
                }
            }
            else
            {
                if (clickCounter > 0f)
                {
                    clickCounter = Mathf.Max(0f, clickCounter - maxClickCount / forcingMoneyDuration * Time.deltaTime);
                    progressbar.Progress = clickCounter / maxClickCount;
                    float duration = clickCounter / maxClickCount * forcingMoneyDuration;
                    progressbar.Timer = Mathf.RoundToInt(duration);
                }
                else
                {
                    forced = false;
                    progressbar.Force = forced;
                    progressbar.Visible = false;
                    fontain.SetActive(false);
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
        progressbar.Force = forced;
        fontain.SetActive(true);
        var reward = GetReward() * 20;
        GenerateBonusMoney(reward);
        island.ChangeMoney(reward);
        StartCoroutine(ForceMoney());

        EventManager.SendEvent("CoinRush", minLevel);
    }

    private IEnumerator ForceMoney()
    {
        WaitForSeconds delay = new WaitForSeconds(0.4f);
        while(clickCounter > 0f)
        {
            yield return delay;
            GenerateMoney();
            GenerateEffect();
        }
    }

    public void Click()
    {
        if (forced || clickDelayTimer > 0f) return;
        clickDelayTimer = clickDelay;
        GenerateMoney();
        Pulse();
        GenerateEffect();
        Taptic.Light();
        clickCounter += 1f;
        hideClickProgressTime = 3f;
        clickTimer = 0.5f;

        if (clickCounter >= maxClickCount)
            ForceClickReward();
    }

    public BigDigit GetReward()
    {
        BigDigit digit;
        if (island.Level <= 25)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (int)(Mathf.Pow(island.Level, 2.15f) / 1.6f + 1);
        else if (island.Level > 25 && island.Level <= 50)
            digit = new BigDigit(modifierMantissa, modifierExponent) * (int)(Mathf.Pow((island.Level - 25), 2.15f) / 1.6f + 1) * 5000;
        else
            digit = new BigDigit(modifierMantissa, modifierExponent) * (int)(Mathf.Pow((island.Level - 50), 2.15f) / 1.6f + 1) * 5000 * 5000;
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
