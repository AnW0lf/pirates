using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class FreeShipController : MonoBehaviour
{
    public Text timer;
    public Image fill;
    public int minLevel;
    public int delay;
    public GameObject pack;

    private Island island;
    private Inventory inventory;
    private TimeSpan ts;
    private float _delay;
    public bool Active { get; private set; }

    private void Start()
    {
        island = Island.Instance;
        inventory = Inventory.Instance;

        _delay = delay;

        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            Active = true;
        }
        else
        {
            pack.SetActive(false);
            Active = false;
            EventManager.Subscribe("LevelUp", CheckUnlock);
        }
        island.InitParameter("PauseTime", DateTime.Now.ToString());
    }

    private void Update()
    {
        if (Active)
        {
            _delay -= Time.deltaTime;
            timer.text = SecondsToTimerString((int)_delay);
            fill.fillAmount = 1f - _delay / delay;

            if(_delay <= 0f)
            {
                AddShip();
                _delay = delay;
            }
        }
    }

    private void CheckUnlock(object[] args)
    {
        if (island.Level >= minLevel)
        {
            pack.SetActive(true);
            Active = true;
            EventManager.Unsubscribe("LevelUp", CheckUnlock);
        }
    }

    public void AddShip()
    {
        int panelNumber = Mathf.Clamp(island.Level / 25, 0, inventory.panels.Count - 1);
        int r = UnityEngine.Random.Range(0, 5);
        int shipNumber = Mathf.Clamp(inventory.currentShips[panelNumber] - (r == 0 ? 0 : 1), 0, inventory.panels[panelNumber].list.ships.Count - 1);
        ShipInfo item = inventory.panels[panelNumber].list.ships[shipNumber];
        inventory.Add(panelNumber, item, true);
    }

    private string SecondsToTimerString(int seconds)
    {
        string min = (seconds / 60).ToString();
        string sec = (seconds % 60) < 10 ? "0" + (seconds % 60).ToString() : (seconds % 60).ToString();
        return min + ":" + sec;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!island) island = Island.Instance;
        if (focus)
        {
            island.InitParameter("PauseTime", DateTime.Now.ToString());
            ts = DateTime.Now - DateTime.Parse(island.GetParameter("PauseTime", ""));
            if (ts.TotalMinutes > 10d)
            {
                StopAllCoroutines();
                _delay = 0f;
            }
        }
        else island.SetParameter("PauseTime", DateTime.Now.ToString());
    }
}
