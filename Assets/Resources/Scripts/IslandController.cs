﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay = 0.5f, modifier;
    public GameObject flyingText;

    private Island island;
    private bool clicked = false, active = false;
    private Animation anim;
    private GameObject _flyingText;

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
        if(!active && island.Level >= minLevel)
        {
            StartCoroutine(GenerateMoney());
            active = true;
        }
    }

    public void Click()
    {
        clicked = true;
    }

    private IEnumerator GenerateMoney()
    {
        float time = clicked ? delay / 2f : delay;
        clicked = false;
        yield return new WaitForSeconds(time);
        anim.Play();

        int reward = (int)(island.Level * island.Level * modifier);

        _flyingText = Instantiate(flyingText, transform);
        _flyingText.transform.localPosition = new Vector3(0f, 0f, 0f);
        _flyingText.GetComponent<FlyingText>().reward = true;
        _flyingText.GetComponent<FlyingText>().rewardText.GetComponent<Text>().text = reward.ToString();

        island.ChangeMoney(reward);
        StartCoroutine(GenerateMoney());
    }
}
