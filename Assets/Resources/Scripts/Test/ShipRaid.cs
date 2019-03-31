using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipRaid : MonoBehaviour
{
    public string shipName;

    public float speed, speedModifier = 3f, delay;
    public int reward;

    private float distance = 1500f;
    private Button btn;
    private RectTransform rect;
    private float rewardModifier = 1f;
    private bool InRaid = false;

    private void Awake()
    {
        btn = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        EventManager.Subscribe(shipName + "Raid", ToRaid);
    }

    private void ToRaid(object[] parameters)
    {
        if (InRaid) return;
        if (parameters.Length > 0)
        {
            delay = (float)parameters[0];
            reward = (int)parameters[1];
            speed = (float)parameters[2];
            StartCoroutine(Raid(reward, speed));
        }
        else
        {
            EventManager.SendEvent(shipName + "Back", 0);
        }
    }

    private IEnumerator Raid(int reward, float speed)
    {
        btn.interactable = false;
        InRaid = true;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (Mathf.Abs(rect.localPosition.y) < distance)
        {
            rect.localPosition += Vector3.up * speed * speedModifier * Time.deltaTime;
            yield return wait;
        }

        float time = delay;
        yield return new WaitForSeconds(time);

        speed = -speed;
        SwapDirection();

        while (Mathf.Abs(rect.localPosition.y) > 10)
        {
            rect.localPosition += Vector3.up * speed * Time.deltaTime;
            yield return wait;
        }

        EventManager.SendEvent(shipName + "Back", (int)(reward * rewardModifier));
        InRaid = false;
        btn.interactable = true;
    }

    private void SwapDirection()
    {
        rect.localScale = new Vector3(rect.localScale.x, -rect.localScale.y, rect.localScale.z);
    }
}
