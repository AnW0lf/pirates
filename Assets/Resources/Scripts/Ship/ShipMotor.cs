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
    public bool isBack { get; private set; }

    private bool direction = true, outOfVisible = false;
    private float distance, delay;
    public EmptyAction raidEndActions, raidMiddleActions, raidBeginActions;

    private Island island;
    private ShipController ship;

    private void Start()
    {
        island = Island.Instance;
        ship = GetComponent<ShipController>();
        isBack = false;
    }

    private void Update()
    {
        if (isRaid)
            Raid();
        else
            RotateAroundTarget();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BorderController border;
        if (isRaid && other.CompareTag("Border") && (border = other.GetComponent<BorderController>()) && border.islandNumber == ship.islandNumber)
        {
            outOfVisible = true;
        }
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
            if (distance < 0f || outOfVisible)
            {
                if (distance < 0f) step += distance;
                isBack = true;
            }
            transform.localPosition += transform.up * (direction ? -1f : 1f) * step;
            if (isBack)
            {
                direction = !direction;
                icon.localScale = new Vector3(icon.localScale.x, (direction ? 1f : -1f), icon.localScale.z);
                distance = range - distance;
                delay = RaidTime;
                raidMiddleActions?.Invoke();
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
            if (!isRaid)
            {
                raidEndActions?.Invoke();
            }
        }
    }

    public void BeginRaid()
    {
        if (isRaid && !isBack) return;
        if (isRaid) raidEndActions?.Invoke();
        else raidBeginActions?.Invoke();
        isRaid = true;
        isBack = false;
        outOfVisible = false;
        distance += range;
    }

    public float RaidTime { get { return duration / Mathf.Pow(2f, durationModifier); } }

    private void OnDestroy()
    {
        raidBeginActions = null;
        raidEndActions = null;
        raidMiddleActions = null;
    }
}
