using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotor : MonoBehaviour
{
    public Transform target;
    public float speed = 200f, goToRaidSpeed = 1000f, backFromRaidSpeed = 500f, range = 2000f, duration = 5f, durationModifier = 0f;
    public RectTransform icon;

    public delegate void EmptyAction();

    public bool isRaid { get; private set; }

    private bool isBack = false, direction = true;
    private float distance, delay;
    public EmptyAction raidEndActions, raidMiddleActions, raidBeginActions;

    private Island island;

    private void Start()
    {
        island = Island.Instance;
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
        transform.RotateAround(target.position, Vector3.forward * (direction ? -1f : 1f), speed * island.speedBonus / Vector3.Distance(target.position, transform.position) / Mathf.PI * Time.deltaTime);
    }

    private void Raid()
    {
        if (!isRaid) return;
        if (!isBack)
        {
            float step = Mathf.Abs(goToRaidSpeed * island.speedBonus * Time.deltaTime);
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
                raidMiddleActions();
            }
        }
        else if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            float step = Mathf.Abs(backFromRaidSpeed * island.speedBonus * Time.deltaTime);
            distance -= step;
            if (distance < 0f)
            {
                step += distance;
                isBack = false;
                isRaid = false;
            }
            transform.localPosition += transform.up * (direction ? -1f : 1f) * step;
            if (!isRaid) raidEndActions();
        }
    }

    public void BeginRaid()
    {
        if (isRaid && !isBack) return;
        if (isRaid) raidEndActions();
        else raidBeginActions();
        isRaid = true;
        isBack = false;
        distance += range;
        delay = duration / Mathf.Pow(2f, durationModifier);
    }
}
