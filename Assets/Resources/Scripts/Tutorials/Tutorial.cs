using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public PierManager pier;
    public GameObject[] tutorials;
    public GameObject[] activateList;

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
        if (tutorials.Length <= stage)
        {
            foreach (GameObject activate in activateList) activate.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (tutorials.Length > stage)
        {
            if (!tutorials[stage].activeInHierarchy)
                tutorials[stage].SetActive(true);
        }
        else
        {
            foreach (GameObject activate in activateList) activate.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void NextStage()
    {
        tutorials[stage++].SetActive(false);
        island.SetParameter(stageParameter, stage);
    }
}
