using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay, tapDelay, modifierMantissa;
    public long modifierExponent;
    public GameObject flyingText;

    public static BigDigit islandReward;

    private Island island;
    private bool clicked = false, active = false;
    private Animation anim;
    private GameObject _flyingText;
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
        else if (island.Level > 50 && island.Level <= 75)
            return new BigDigit(modifierMantissa, modifierExponent) * Mathf.Pow(island.Level, 2.15f) * (island.Level - 50);
        else
            return new BigDigit(modifierMantissa, modifierExponent) * Mathf.Pow(island.Level, 2.15f) * (island.Level - 75);
    }

    private IEnumerator GenerateMoney()
    {
        if (clicked)
        {
            time = tapDelay;
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
        _flyingText.transform.localPosition = new Vector3(0f, 50f, 0f);
        _flyingText.GetComponent<FlyingText>().reward = true;
        _flyingText.GetComponent<FlyingText>().rewardText.GetComponent<Text>().text = reward.ToString();

        island.ChangeMoney(reward);
        yield return new WaitForSeconds(time / 2);
        clicked = false;
        yield return new WaitForSeconds(time / 2);
        StartCoroutine(GenerateMoney());
    }
}
