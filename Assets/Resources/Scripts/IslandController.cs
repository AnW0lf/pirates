﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay, tapDelay, modifier;
    public GameObject flyingText;

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

    private void Start()
    {
        if (island.Level >= minLevel)
        {
            StartCoroutine(GenerateMoney());
            active = true;
        }
    }

    private void Update()
    {
        if (!active && island.Level >= minLevel)
        {
            StartCoroutine(GenerateMoney());
            active = true;
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

    private IEnumerator GenerateMoney()
    {
        if (clicked)
        {
            time = tapDelay;
        }
        else if ((delay - (island.GetParameter("Level", 0) - 1) / 10) > tapDelay)
        {
            time = delay - (island.GetParameter("Level", 0) - 1) / 10;
        }
        else
        {
            delay = tapDelay;
        }

        yield return new WaitForSeconds(time);
        anim.Play();

        //int reward = (int)(island.Level * island.Level * modifier);

        int reward = (int)(Mathf.Pow(island.Level, 2.15f) * modifier);

        _flyingText = Instantiate(flyingText, transform);
        _flyingText.transform.localPosition = new Vector3(0f, 50f, 0f);
        _flyingText.GetComponent<FlyingText>().reward = true;
        _flyingText.GetComponent<FlyingText>().rewardText.GetComponent<Text>().text = reward.ToString();

        island.ChangeMoney(reward);
        clicked = false;
        StartCoroutine(GenerateMoney());
    }
}
