using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    private float rise = 400f;
    private float angle = 0f;
    private bool direction = false;
    private float size = 20f;

    public int reward;
    public float raidTime;

    public int rewardModifier = 1;
    public float raidSpeedModifier = 1f;
    public float raidTimeModifier = 1f;

    //Рейд
    private bool visible = false;
    private bool inRaid = false;
    private float speedAngle, speedLinear, circle = 0f;
    private RectTransform _riseRT, _iconRT;

    [Header("Детали корабля")]
    public Transform _rise;
    public Transform _icon;
    public Transform _coin;

    private float riseOutOfScreen = 1000f;

    private void Awake()
    {
        _riseRT = _rise.GetComponent<RectTransform>();
        _iconRT = _icon.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateShip();
    }

    private void FixedUpdate()
    {
        if(!inRaid)
        {
            circle += speedAngle * Time.fixedDeltaTime;
            angle += speedAngle * Time.fixedDeltaTime;
            _riseRT.localEulerAngles = Vector3.forward * angle;
        }
        if(Mathf.Abs(circle) >= 360f)
        {
            circle = 0f;
            if (_coin.gameObject.activeInHierarchy)
                _coin.GetComponent<CoinCatcher>().CatchCoin();
            BeginRaid();
        }
    }

    public void UpdateShip()
    {
        _rise.GetComponent<RectTransform>().sizeDelta = new Vector2(rise, 10f);
        _rise.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * angle;
        _icon.GetComponent<SpriteRenderer>().flipY = direction;
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size + Vector3.up * size + Vector3.forward;
        speedAngle = Math.Abs(speedAngle) * (direction ? 1 : -1);
        speedLinear = Math.Abs(speedLinear) * (direction ? 1 : -1);

        rewardModifier = 1;
        raidTimeModifier = 1f;
        //raidSpeedModifier = 1f;
    }

    public bool InRaid()
    {
        return inRaid;
    }

    public void BeginRaid()
    {
        if (!inRaid)
        {
            inRaid = true;
            StartCoroutine(Raid());
        }
    }

    private IEnumerator Raid()
    {
        do
        {
            visible = _icon.GetComponent<ShipClick>().IsVisible();
            _iconRT.localPosition += Vector3.down * (speedLinear * raidSpeedModifier) * Time.deltaTime;
            yield return null;
        } while (visible);
        float seconds = raidTime / raidTimeModifier;
        yield return new WaitForSeconds(seconds);

        _coin.GetComponent<CoinCatcher>().ActivateCoin(reward * rewardModifier);
        angle = Random.Range(0f, 359f);
        _riseRT.localEulerAngles = Vector3.forward * angle;
        direction = !direction;
        _iconRT.localPosition = Vector3.left * rise + Vector3.up * riseOutOfScreen * (direction ? 1 : -1);
        _icon.GetComponent<SpriteRenderer>().flipY = direction;

        while(Mathf.Abs(_iconRT.localPosition.y) > 5f)
        {
            _iconRT.localPosition += Vector3.up * (speedLinear * raidSpeedModifier) * Time.deltaTime;
            yield return null;
        }
        UpdateShip();
        inRaid = false;
    }


    public void SetLocation(float rise, float angle)
    {
        _rise.GetComponent<RectTransform>().sizeDelta = new Vector2(rise, 10f);
        _rise.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * angle;
    }

    public void SetShip(float size, Sprite sprite)
    {
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size + Vector3.up * size + Vector3.forward;
        _icon.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetSpeed(float speedAngle, float speedLinear)
    {
        this.speedAngle = speedAngle;
        this.speedLinear = speedLinear;
    }

    public void SetRaid(float raidTime, int reward)
    {
        this.raidTime = raidTime;
        this.reward = reward;
    }

    public void CreateShip(float rise, float angle, float size, Sprite sprite, float speedAngle, float speedLinear, float raidTime, int reward)
    {
        SetLocation(rise, angle);
        SetShip(size, sprite);
        SetSpeed(speedAngle, speedLinear);
        SetRaid(raidTime, reward);
    }
}
