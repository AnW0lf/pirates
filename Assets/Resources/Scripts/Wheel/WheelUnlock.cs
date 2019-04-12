﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelUnlock : MonoBehaviour
{
    public int minLevel;

    private Island island;
    private Button btn;
    private GameObject child;
    public GameObject unlockMenu; 

    private void Awake()
    {
        island = Island.Instance();
        btn = GetComponent<Button>();
        child = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        if (island.Level < minLevel)
        {
            child.SetActive(false);
        }
        else
        {
            UnlockWheel();
        }
    }

    public void Open()
    {
        if (island == null) island = Island.Instance();
        if (child == null) child = transform.GetChild(0).gameObject;
        if (island.Level >= minLevel && !child.activeInHierarchy)
        {
            unlockMenu.SetActive(true);
        }
    }

    public void UnlockWheel()
    {
        child.SetActive(true);
        //this.enabled = false;
    }
}
