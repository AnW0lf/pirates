﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipClick : MonoBehaviour
{
    public Ship ship;
    public LifebuoyManager lifebuoys;
    public GameObject flyingText;
    public Transform pointer;
    public Image arrow;
    public Text clock;
    public Color color;
    public TrailRenderer trail;
    public IslandController islandController = null;

    private GameObject _flyingText;
    private Island island;
    private Camera cam;
    private Vector3 startPos;
    private bool isTimerActive;
    private string borderName;

    private void Awake()
    {
        island = Island.Instance();
        cam = Camera.main;
    }

    // Собираем бонус
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bonus") && !isTimerActive)
        {
            other.gameObject.GetComponentInParent<BonusPoint>().active = false;

            //Taptic.Medium();

            _flyingText = Instantiate(flyingText, other.transform.parent);
            _flyingText.transform.localPosition = new Vector3(0f, 0f, 0f);
            FlyingText ft = _flyingText.GetComponent<FlyingText>();

            if (other.gameObject.GetComponent<BonusBehavior>().bonusMaterial)
            {
                ship.rewardModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                ft.exp = true;
                ft.expText.GetComponent<Text>().text = "+" + ship.reward.ToString();

                EventManager.SendEvent("BonusCollected", ship.ShipName, "Material");
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusMoney)
            {
                if (islandController != null)
                {
                    BigDigit reward = islandController.GetReward() * 5 * island.Level;
                    ft.money = true;
                    ft.moneyText.GetComponent<Text>().text = "+" + reward.ToString();
                    islandController.GenerateBonusMoney(reward);
                    EventManager.SendEvent("BonusCollected", ship.ShipName, "Money");
                }
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusSpeed)
            {
                ship.raidTimeModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                ft.speed = true;
                ft.speedText.GetComponent<Text>().text = "-" + (int)(ship.raidTime / Mathf.Pow(2f, ship.raidTimeModifier)) + "s";

                EventManager.SendEvent("BonusCollected", ship.ShipName, "Speed");
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusWheel)
            {
                if (island.Level >= 2)
                {
                    lifebuoys.AddLifebuoy();
                    _flyingText.GetComponent<FlyingText>().wheel = true;

                    EventManager.SendEvent("BonusCollected", ship.ShipName, "WheelSpin");
                }
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Border") && !isTimerActive)
        {
            borderName = other.gameObject.name;
            Invoke("SwitchEmmiting", 0.15f);
            if (other.gameObject.name.Equals("RightBorder") || other.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime(), ship.GetRaidTime() + 1.5f, true));
            else
                StartCoroutine(Timer(ship.GetRaidTime(), ship.GetRaidTime() + 0.5f, false));
        }
        else if (other.CompareTag("Border") && isTimerActive && borderName == other.gameObject.name)
        {
            borderName = "";
            Invoke("SwitchEmmiting", 0.4f);
            isTimerActive = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    private IEnumerator Timer(float time, float realTime, bool isSide)
    {
        isTimerActive = true;

        pointer.gameObject.SetActive(true);

        StartCoroutine(Show(0.33f));

        float height = 2f * cam.orthographicSize, width = height * cam.aspect, xPos = transform.position.x, yPos = transform.position.y;
        Vector3 pointerPos = new Vector3(isSide ? (xPos > 0f ? width / 2f : -width / 2f) : xPos,
            isSide ? yPos : (yPos > 0f ? height / 2f - 0.75f : -height / 2f + 1.7f), transform.position.z);
        pointer.position = pointerPos;
        pointer.eulerAngles = transform.eulerAngles;

        float timer = realTime;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            clock.text = Mathf.RoundToInt(Mathf.Max(time * timer / realTime + 0.45f, 1f)).ToString();
            clock.transform.eulerAngles = Vector3.zero;
            yield return null;
        }

        StartCoroutine(Hide(0.33f));

        LeanTween.delayedCall(1f, () => pointer.gameObject.SetActive(false));
    }

    private IEnumerator Show(float duration)
    {
        Color transparentArrow = color;
        //Color transparentClock = clock.color;

        transparentArrow.a = 0f;
        //transparentClock.a = 0f;

        arrow.color = transparentArrow;
        arrow.transform.localScale = Vector3.one * 0.5f;
        //clock.color = transparentClock;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            transparentArrow.a = Mathf.Lerp(0f, 1f, time / duration);
            //transparentClock.a = Mathf.Lerp(0f, 1f, time / duration);

            arrow.color = transparentArrow;
            arrow.transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1f, time / duration);
            //clock.color = transparentClock;

            yield return null;
        }

        transparentArrow.a = 1f;
        //transparentClock.a = 1f;

        arrow.transform.localScale = Vector3.one;
        arrow.color = transparentArrow;
        //clock.color = transparentClock;
        clock.gameObject.SetActive(true);
    }

    private IEnumerator Hide(float duration)
    {
        clock.gameObject.SetActive(false);
        Color transparentArrow = color;
        //Color transparentClock = clock.color;

        transparentArrow.a = 1f;
        //transparentClock.a = 1f;

        arrow.color = transparentArrow;
        arrow.transform.localScale = Vector3.one;
        //clock.color = transparentClock;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            transparentArrow.a = Mathf.Lerp(1f, 0f, time / duration);
            //transparentClock.a = Mathf.Lerp(1f, 0f, time / duration);

            arrow.color = transparentArrow;
            arrow.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0.5f, time / duration);
            //clock.color = transparentClock;

            yield return null;
        }

        transparentArrow.a = 0f;
        //transparentClock.a = 0f;

        arrow.color = transparentArrow;
        arrow.transform.localScale = Vector3.one * 0.5f;
        //clock.color = transparentClock;
    }

    private void SwitchEmmiting()
    {
        trail.emitting = !trail.emitting;
    }
}
