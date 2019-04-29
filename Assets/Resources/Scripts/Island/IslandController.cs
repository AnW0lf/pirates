using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay, tapDelay, modifierMantissa;
    public long modifierExponent;
    public GameObject flyingText, clickEffect;

    public static BigDigit islandReward;

    private Island island;
    private bool clicked = false, active = false;
    private Animation anim;
    private GameObject _flyingText, _clickEffect;
    private float time;

    private void Awake()
    {
        island = Island.Instance();
        anim = GetComponent<Animation>();
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
            Taptic.Light();
            clicked = true;
            StopAllCoroutines();
            StartCoroutine(GenerateMoney());
        }
    }

    public BigDigit GetReward()
    {
        if (island.Level <= 25)
            return new BigDigit(modifierMantissa, modifierExponent) * Mathf.Pow(island.Level, 2.15f);
        else if (island.Level > 25 && island.Level <= 50)
            return new BigDigit(modifierMantissa, modifierExponent) * Mathf.Pow(island.Level, 2.15f) * (island.Level - 25);
        else
            return new BigDigit(modifierMantissa, modifierExponent) * Mathf.Pow(island.Level, 2.15f) * (island.Level - 25) * (island.Level - 50);
    }

    private IEnumerator GenerateMoney()
    {
        if (clicked)
        {
            time = tapDelay;
            _clickEffect = Instantiate(clickEffect, transform);
            _clickEffect.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            _clickEffect.GetComponent<RectTransform>().anchorMax = Vector2.one;
            _clickEffect.GetComponent<RectTransform>().offsetMin = Vector2.up * 100f;
            _clickEffect.GetComponent<RectTransform>().offsetMax = Vector2.up * 100f;
            _clickEffect.SetActive(true);
        }
        else if ((delay - (island.GetParameter("Level", 0) - 1) / 10) > tapDelay)
        {
            time = delay - (island.GetParameter("Level", 0) - 1) / 50;
        }
        else
        {
            time = tapDelay;
        }

        anim.Play("OnePulse");

        BigDigit reward = GetReward();

        _flyingText = Instantiate(flyingText, transform);

        //-110 -60
        BigDigit firstOffset = new BigDigit(9.9f, 1);
        BigDigit secondOffset = new BigDigit(9.99f, 2);
        if (reward.LessThen(firstOffset))
        {
            _flyingText.transform.localPosition = new Vector3(-110f, 50f, 0f);
        }
        else if (reward.LessThen(secondOffset))
        {
            _flyingText.transform.localPosition = new Vector3(-60f, 50f, 0f);
        }
        else
        {
            _flyingText.transform.localPosition = new Vector3(0f, 50f, 0f);
        }


        _flyingText.GetComponent<FlyingText>().reward = true;
        _flyingText.GetComponent<FlyingText>().rewardText.GetComponent<Text>().text = reward.ToString();

        island.ChangeMoney(reward);
        yield return new WaitForSeconds(time / 2);
        clickEffect.SetActive(false);
        clicked = false;
        yield return new WaitForSeconds(time / 2);
        StartCoroutine(GenerateMoney());
    }
}
