using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFill : MonoBehaviour
{
    private Global global;
    private Image image;

    private void Awake()
    {
        global = Global.Instance;
        image = GetComponent<Image>();
    }

    private void Start()
    {
        EventManager.Subscribe("AddMaterial", AddMaterial);
        EventManager.Subscribe("LevelUp", NewLevel);
        AddMaterial(new object[0]);
    }

    private void NewLevel(object[] arg0)
    {
        image.fillAmount = 0;
    }

    private void AddMaterial(object[] arg0)
    {
        image.fillAmount = (float)global.Material / global.GetMaxMaterial();
    }
}
