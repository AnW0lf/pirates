using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotor : MonoBehaviour
{
    public Transform target;
    public float speed = 200f, range = 2000f, duration = 5f, durationModifier = 0f, raidSpeedBonus = 10f;
    public RectTransform icon;

    public delegate void EmptyAction();

    public bool isRaid { get; private set; }

    private bool isBack = false, direction = true;
    private float distance, delay;
    private List<EmptyAction> raidEndActions, raidMiddleActions, raidBeginActions;

    private void Awake()
    {
        raidEndActions = new List<EmptyAction>();
        raidMiddleActions = new List<EmptyAction>();
        raidBeginActions = new List<EmptyAction>();
    }

    private void Update()
    {
        if (isRaid)
            Raid();
        else
            RotateAroundTarget();
    }

    private void RotateAroundTarget()
    {
        if (target == null) return;
        transform.RotateAround(target.position, Vector3.forward * (direction ? -1f : 1f), speed / Vector3.Distance(target.position, transform.position) / Mathf.PI * Time.deltaTime);
    }

    private void Raid()
    {
        if (!isRaid) return;
        if (!isBack)
        {
            float step = Mathf.Abs(raidSpeedBonus * speed * Time.deltaTime);
            distance -= step;
            if (distance < 0f)
            {
                step += distance;
                isBack = true;
            }
            transform.localPosition += transform.up * (direction ? -1f : 1f) * step;
            if (isBack)
            {
                direction = !direction;
                icon.localScale = new Vector3(icon.localScale.x, (direction ? 1f : -1f), icon.localScale.z);
                distance = range;
                foreach (EmptyAction ea in raidMiddleActions) ea();
            }
        }
        else if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            float step = Mathf.Abs(speed * Time.deltaTime);
            distance -= step;
            if (distance < 0f)
            {
                step += distance;
                isBack = false;
                isRaid = false;
            }
            transform.localPosition += transform.up * (direction ? -1f : 1f) * step;
            if (!isRaid) foreach (EmptyAction ea in raidEndActions) ea();
        }
    }

    public void BeginRaid()
    {
        if (isRaid && !isBack) return;
        foreach (EmptyAction ea in (isRaid ? raidEndActions : raidBeginActions)) ea();
        isRaid = true;
        isBack = false;
        distance += range;
        delay = duration / Mathf.Pow(2f, durationModifier);
    }

    public void AddRaidEndAction(EmptyAction action)
    {
        if (!raidEndActions.Contains(action))
            raidEndActions.Add(action);
    }

    public void AddRaidMiddleAction(EmptyAction action)
    {
        if (!raidMiddleActions.Contains(action))
            raidMiddleActions.Add(action);
    }

    public void AddRaidBeginAction(EmptyAction action)
    {
        if (!raidBeginActions.Contains(action))
            raidBeginActions.Add(action);
    }

    public void ClearRaidEndActions()
    {
        raidEndActions.Clear();
    }

    public void ClearRaidMiddleActions()
    {
        raidMiddleActions.Clear();
    }

    public void ClearRaidBeginActions()
    {
        raidBeginActions.Clear();
    }
}
