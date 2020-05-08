using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCtrl : MonoBehaviour
{
    public int minLevel, maxLevel, currentLevel;
    [SerializeField] private IslandLevel[] levels;
    [SerializeField] private ProgressBar forceCoin = null, experience = null;
    [SerializeField] private EffectSettings coin, wood;
    [SerializeField] private Transform effects = null;
    [SerializeField] private Animation islandAnimation = null;
    [SerializeField] private SpriteRenderer islandIcon = null;
    [SerializeField] private GameObject fontain = null;
    [SerializeField] private float clickDelay = 0.2f, clickMaxProgress = 100f, clickIncrease = 1f, clickDecrease = 1f, forcingDuration = 60f;
    [SerializeField] private GameObject fade = null;
    [SerializeField] private ShipCtrl[] ships;
    private float time = 1f, timer = 0f, clickProgress = 0f;
    private bool clicked = false, clickForced = false;

    public int Level { get => currentLevel; set => currentLevel = value; }

    public BigDigit Reward
    {
        get
        {
            if (currentLevel < minLevel) return BigDigit.zero;
            int lvl = Mathf.Clamp(currentLevel, minLevel, maxLevel - 1) - minLevel;
            return levels[lvl].reward;
        }
    }

    public float AutoclickDelay
    {
        get
        {
            if (currentLevel < minLevel) return 0f;
            int lvl = Mathf.Clamp(currentLevel, minLevel, maxLevel - 1) - minLevel;
            return levels[lvl].autoclickDelay;
        }
    }

    public Sprite Sprite
    {
        get
        {
            if (currentLevel < minLevel) return null;
            int lvl = Mathf.Clamp(currentLevel, minLevel, maxLevel - 1) - minLevel;
            return levels[lvl].sprite;
        }
    }

    private void Start()
    {
        if (currentLevel < minLevel) return;
        fade.SetActive(false);
        islandIcon.sprite = Sprite;
    }

    private void Update()
    {
        if (currentLevel < minLevel) return;

        if (clickForced)
        {
            string strTime = time.ToString();
            if (strTime.Length > 4) strTime = strTime.Substring(0, 4);
            forceCoin.Label = string.Format("{0} sec", strTime);
            forceCoin.Progress = clickProgress / clickMaxProgress;

            time -= Time.deltaTime;
            clickProgress -= clickMaxProgress / forcingDuration * Time.deltaTime;

            if (clickProgress <= 0f)
            {
                clickProgress = 0f;
                clickForced = false;
            }

            if (timer <= 0f)
            {
                timer = clickDelay;
                Effect(coin, Reward.ToString());
                Pulse();
            }
        }
        else
        {
            forceCoin.Progress = clickProgress / clickMaxProgress;

            if (clickProgress / clickMaxProgress > 0.02f && !forceCoin.Visible)
            {
                forceCoin.Label = (Reward * 20f).ToString();
                forceCoin.Visible = true;
            }
            else if (clickProgress / clickMaxProgress <= 0.02f && forceCoin.Visible) forceCoin.Visible = false;

            if (clickProgress >= clickMaxProgress)
            {
                time = forcingDuration;
                clickProgress = clickMaxProgress;
                clickForced = true;
                fontain.SetActive(true);
                LeanTween.delayedCall(1.5f, () => fontain.SetActive(false));
            }

            if (clickProgress > 0f)
                clickProgress -= clickDecrease * Time.deltaTime;
            else if (clickProgress < 0f) clickProgress = 0f;
        }

        timer -= Time.deltaTime;

        if (clicked)
        {
            if (timer <= 0f) clicked = false;
        }
        else if (timer <= -AutoclickDelay)
        {
            Autoclick();
        }
    }

    private void Autoclick()
    {
        Island.Instance().ChangeMoney(Reward);
        Effect(coin, Reward.ToString());
        Pulse();
        timer = 0f;
    }

    private void OnMouseUpAsButton()
    {
        if (currentLevel < minLevel) return;
        Click();
    }

    public void RewardRaid(BigDigit reward)
    {
        Effect(wood, reward.ToString());
        Island.Instance().ExpUp(reward);
        experience.Visible = true;
        experience.Label = "Level " + (Island.Instance().Level - minLevel + 1);
    }

    private void Pulse()
    {
        islandAnimation.Stop();
        islandAnimation.Play();
    }

    private void Click()
    {
        if (clicked || clickForced) return;
        clicked = true;
        clickProgress += clickIncrease;
        Effect(coin, Reward.ToString());
        Pulse();

        Transform light = effects.GetChild(0);
        light.SetAsLastSibling();
        light.gameObject.SetActive(true);
        LeanTween.delayedCall(1f, () => light.gameObject.SetActive(false));

        Island.Instance().ChangeMoney(Reward);
        timer = clickDelay;
    }

    private void Effect(EffectSettings effect, string text)
    {
        FlyingObject flying = effect.container.GetChild(0).GetComponent<FlyingObject>();
        if (flying == null) return;

        flying.transform.SetAsLastSibling();
        Vector3 startPos = effect.container.position;
        Vector3 endPos = startPos + (Vector3)effect.endOffset;

        LeanTween.delayedCall(0.1f, () =>
        flying.Fly(startPos, endPos, effect.flyingDuration, string.Format(effect.textPattern, text), effect.ease));
    }

}

[System.Serializable]
public class EffectSettings
{
    public Transform container;
    public Vector2 endOffset;
    public float flyingDuration;
    public LeanTweenType ease;
    public string textPattern = "{0}";

    public EffectSettings(Transform container, Vector2 endOffset, float flyingDuration, LeanTweenType ease, string textPattern)
    {
        this.container = container;
        this.endOffset = endOffset;
        this.flyingDuration = flyingDuration;
        this.ease = ease;
        this.textPattern = textPattern;
    }
}

[System.Serializable]
public class IslandLevel
{
    public float autoclickDelay;
    public BigDigit reward;
    public Sprite sprite;

    public IslandLevel(float autoclickDelay, BigDigit reward, Sprite sprite)
    {
        this.autoclickDelay = autoclickDelay;
        this.reward = reward;
        this.sprite = sprite;
    }
}
