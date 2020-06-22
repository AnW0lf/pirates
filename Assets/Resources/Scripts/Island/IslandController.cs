using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    [Space(20)]
    public float maxClickCount, clickCountDecrease, autoclickDelay, clickDelay, forcingMoneyDuration, forcingMoneyModifier = 0.5f;
    [Space(20)]
    public float modifierMantissa;
    public long modifierExponent;
    public Transform moneySet, clickEffectSet, experienceSet;
    [Space(20)]
    public ProgressBar progressbar;
    [Space(20)]
    public GameObject fontain;

    public static BigDigit islandReward;

    private Island island;
    private bool active = false, forced = false, clicked = false;
    private RectTransform rect;
    private float clickCounter, hideClickProgressTime = 3f, forcingTimer;
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
        StartCoroutine(Clicking());
    }

    private IEnumerator Clicking()
    {
        float clickTimer, progressHideTimer = 0f;

        while (true)
        {
            if (!active)
            {
                yield return null;
                continue;
            }

            clickTimer = 0f;

            while (clickTimer < clickDelay)
            {
                clickTimer += Time.deltaTime;

                if (!forced)
                {
                    progressHideTimer = Mathf.Max(progressHideTimer - Time.deltaTime, 0f);

                    if (progressHideTimer < 2f) clickCounter = Mathf.Max(clickCounter - Time.deltaTime * clickCountDecrease, 0f);

                    if (!progressbar.Visible && clickCounter > 2f && progressHideTimer > 0f) progressbar.Visible = true;
                    else if (progressbar.Visible && (clickCounter == 0f || progressHideTimer == 0f)) progressbar.Visible = false;

                    progressbar.Progress = clickCounter / maxClickCount;
                    progressbar.Label = (GetReward() * 20).ToString();
                }
                else
                {
                    if (!progressbar.Visible) progressbar.Visible = true;

                    forcingTimer = Mathf.Max(forcingTimer - Time.deltaTime, 0f);

                    if (forcingTimer == 0f)
                    {
                        forced = false;
                        fontain.SetActive(false);
                        progressbar.Force = forced;
                        clickCounter = 0f;
                    }

                    progressbar.Progress = forcingTimer / forcingMoneyDuration;
                    progressbar.Timer = Mathf.CeilToInt(forcingTimer);
                }

                yield return null;
            }

            clickTimer = 0f;

            while (clickTimer < autoclickDelay)
            {
                if (clicked)
                {
                    GenerateMoney();
                    Pulse();
                    GenerateEffect();
                    progressHideTimer = 3f;

                    if (!forced)
                    {
                        clickCounter += 1f;
                        if (clickCounter >= maxClickCount)
                            ForceClickReward();
                    }

                    break;
                }

                clickTimer += Time.deltaTime;

                if (!forced)
                {
                    progressHideTimer = Mathf.Max(progressHideTimer - Time.deltaTime, 0f);

                    if (progressHideTimer < 2f) clickCounter = Mathf.Max(clickCounter - Time.deltaTime * clickCountDecrease, 0f);

                    if (!progressbar.Visible && clickCounter > 2f && progressHideTimer > 0f) progressbar.Visible = true;
                    else if (progressbar.Visible && (clickCounter == 0f || progressHideTimer == 0f)) progressbar.Visible = false;

                    progressbar.Progress = clickCounter / maxClickCount;
                    progressbar.Label = (GetReward() * 20).ToString();
                }
                else
                {
                    if (!progressbar.Visible) progressbar.Visible = true;

                    forcingTimer = Mathf.Max(forcingTimer - Time.deltaTime, 0f);

                    if (forcingTimer == 0f)
                    {
                        forced = false;
                        progressbar.Force = forced;
                        clickCounter = 0f;
                    }

                    progressbar.Progress = forcingTimer / forcingMoneyDuration;
                    progressbar.Timer = Mathf.CeilToInt(forcingTimer);
                }

                yield return null;
            }

            if(!clicked) GenerateMoney();
            clicked = false;
        }
    }

    private void Update()
    {
        if (!active && minLevel <= island.Level) active = true;
    }

    private void ForceClickReward()
    {
        forced = true;
        progressbar.Force = forced;
        forcingTimer = forcingMoneyDuration;
        fontain.SetActive(true);
        var reward = GetReward() * 20;
        GenerateBonusMoney(reward);
        island.ChangeMoney(reward);

        EventManager.SendEvent("CoinRush", minLevel);
    }

    public void Click()
    {
        Taptic.Light();
        clicked = true;
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

        if (forced) digit *= (1f + forcingMoneyModifier);
        else digit *= (1f + (clickCounter / maxClickCount) * forcingMoneyModifier);

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
