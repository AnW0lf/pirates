using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolundraTimer : MonoBehaviour
{
    [SerializeField] private Text timer;
    [SerializeField] private Image fill;
    [SerializeField] private int minLevel;
    [SerializeField] private int seconds;
    [SerializeField] private GameObject pack, message;
    [SerializeField] List<BonusGenerator> bgs;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            StartCoroutine(Timer());
        }
        else
        {
            pack.SetActive(false);
            EventManager.Subscribe("LevelUp", CheckPolundra);
        }
    }

    private void CheckPolundra(object[] arg0)
    {
        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            StartCoroutine(Timer());
            EventManager.Unsubscribe("LevelUp", CheckPolundra);
        }
    }

    private IEnumerator Timer()
    {
        WaitForSeconds sec = new WaitForSeconds(1f);
        for(int i = seconds; i >= 0; i--)
        {
            timer.text = SecondsToTimerString(i);
            fill.fillAmount = 1f - ((float)i / seconds);
            yield return sec;
        }
        timer.text = "POLUNDRA";
        StartCoroutine(Polundra());
    }

    private IEnumerator Polundra()
    {
        message.SetActive(false);
        message.SetActive(true);
        Animation anim = message.GetComponent<Animation>();
        bool b() { return anim.IsPlaying("PolundraAnimation"); }
        yield return new WaitWhile(b);

        WaitForSeconds sec = new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j <= island.Level / 25 && j < bgs.Count; j++)
            {
                bgs[j].RandomBonus(1);
            }
            yield return sec;
        }
        StartCoroutine(Timer());
    }

    private string SecondsToTimerString(int seconds)
    {
        string min = (seconds / 60).ToString();
        string sec = (seconds % 60) < 10 ? "0" + (seconds % 60).ToString() : (seconds % 60).ToString();
        return min + ":" + sec;
    }
}
