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
    private BigDigit oldExp;
    private bool filled = false;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        if (minLevel <= island.Level && maxLevel >= island.Level && island.Exp >= island.GetMaxExp())
        {
            button.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (island == null) island = Island.Instance();

        EventManager.Subscribe("AddExp", ShowFill);
        oldExp = island.Exp;
        if (minLevel > island.Level || maxLevel < island.Level)
            title.text = "Level " + (maxLevel - minLevel + 1).ToString() + "/25";
        else
            title.text = "Level " + (island.Level - minLevel).ToString() + "/25";

        //ShowFill(new object[0]);
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
        }
        else
        {
            title.text = "Level " + (island.Level - minLevel).ToString() + "/25";
            if (oldExp < island.GetMaxExp())
            {
                if (filled)
                    StopAllCoroutines();
                StartCoroutine(Fill());
            }
            else if (minLevel <= island.Level && maxLevel >= island.Level && island.Exp >= island.GetMaxExp())
            {
                button.SetActive(true);
            }
        }
    }

    public void LevelUp()
    {
        island.LevelUp();
        oldExp = island.Exp;
    }

    private IEnumerator Fill()
    {
        filled = true;
        WaitForSeconds wait = new WaitForSeconds(0.04f);
        Animation progressBar = progress.GetChild(0).GetComponent<Animation>();
        Image fill = progress.GetChild(0).GetChild(0).GetComponent<Image>();
        progressBar.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        progressBar.Stop();
        title.GetComponent<Animation>().Stop();
        progressBar.Play();
        title.GetComponent<Animation>().Play();
        fill.fillAmount = (oldExp / island.GetMaxExp()).ToFloat();

        for (int i = 0; i < 25; i++)
        {
            fill.fillAmount = ((oldExp + i * ((island.Exp - oldExp) / 24f)) / island.GetMaxExp()).ToFloat();
            yield return wait;
        }

        progressBar.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        oldExp = island.Exp;
        filled = false;

        if (minLevel <= island.Level && maxLevel >= island.Level && island.Exp >= island.GetMaxExp())
        {
            button.SetActive(true);
        }
    }

}
