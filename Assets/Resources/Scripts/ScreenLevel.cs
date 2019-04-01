using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : MonoBehaviour
{
    public Transform progress;
    public GameObject button;
    public Text title;
    public int minLevel, maxLevel;

    private Island island;
    private int oldExp;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("AddExp", ShowFill);
        oldExp = island.Exp;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("AddExp", ShowFill);
    }

    private void ShowFill(object[] arg0)
    {
        if (minLevel > island.Level || maxLevel < island.Level)
        {
            title.text = "Level " + (maxLevel - minLevel + 1).ToString() + "/25";
            return;
        }
        if (island.Exp >= island.GetMaxExp())
        {
            button.SetActive(true);
        }
        title.text = "Level " + (island.Level - minLevel).ToString() + "/25";
        StartCoroutine(Fill());
    }
    
    public void LevelUp()
    {
        island.LevelUp();
        oldExp = island.Exp;
    }

    private IEnumerator Fill()
    {
        WaitForSeconds wait = new WaitForSeconds(0.04f);
        Animation progressBar = progress.GetChild(0).GetComponent<Animation>();
        Image fill = progress.GetChild(0).GetChild(0).GetComponent<Image>();
        progressBar.gameObject.SetActive(true);
        progressBar.Play();
        fill.fillAmount = (float)oldExp / island.GetMaxExp();

        for (int i = 0; i < 25; i++)
        {
            fill.fillAmount = ((float)oldExp + i * ((float)(island.Exp - oldExp) / 24f)) / island.GetMaxExp();
            yield return wait;
        }

        progressBar.gameObject.SetActive(false);
        oldExp = island.Exp;
    }

}
