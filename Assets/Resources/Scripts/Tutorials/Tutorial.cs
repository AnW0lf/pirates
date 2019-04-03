﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public PierManager pier;
    public GameObject[] tutorials;

    private int stage = 0;
    private Island island;
    private string stageParameter = "tutorial_stage";

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter(stageParameter, 0);
        stage = island.GetParameter(stageParameter, 0);
    }

    private void Update()
    {
        if (tutorials.Length > stage)
        {
            if (!tutorials[stage].activeInHierarchy)
                tutorials[stage].SetActive(true);
        }
        else gameObject.SetActive(false);
    }

    public void NextStage()
    {
        tutorials[stage++].SetActive(false);
        island.SetParameter(stageParameter, stage);
    }
}