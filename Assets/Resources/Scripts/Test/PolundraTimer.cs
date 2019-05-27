using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolundraTimer : MonoBehaviour
{
    [SerializeField] private Text timer = null;
    [SerializeField] private Image fill = null;
    [SerializeField] private int minLevel = 0;
    [SerializeField] private int seconds = 0;
    [SerializeField] private GameObject pack = null, message = null;
    [SerializeField] List<BonusGenerator> bgs = null;

    private Island island;
    private TimeSpan ts;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            StartCoroutine(Timer(90));
        }
        else
        {
            pack.SetActive(false);
            EventManager.Subscribe("LevelUp", CheckPolundra);
        }
        island.InitParameter("PauseTime", "");
    }

    private void CheckPolundra(object[] arg0)
    {
        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            StartCoroutine(Timer(90));
            EventManager.Unsubscribe("LevelUp", CheckPolundra);
        }
    }

    private IEnumerator Timer(int time)
    {
        WaitForSeconds sec = new WaitForSeconds(1f);
        for(int i = time; i >= 0; i--)
        {
            timer.text = SecondsToTimerString(i);
            fill.fillAmount = 1f - ((float)i / time);
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

        WaitForSeconds sec = new WaitForSeconds(0.2f);
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j <= island.Level / 25 && j < bgs.Count; j++)
            {
                bgs[j].RandomBonus(1);
            }
            yield return sec;
        }
        StartCoroutine(Timer(seconds));
    }

    private string SecondsToTimerString(int seconds)
    {
        string min = (seconds / 60).ToString();
        string sec = (seconds % 60) < 10 ? "0" + (seconds % 60).ToString() : (seconds % 60).ToString();
        return min + ":" + sec;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            island.SetParameter("PauseTime", DateTime.Now.ToString());
        }
        else
        {
            ts = DateTime.Now - DateTime.Parse(island.GetParameter("PauseTime", ""));
            if (ts.TotalMinutes > 10d)
            {
                StopAllCoroutines();
                StartCoroutine(Timer(90));
            }
        }
    }
}
