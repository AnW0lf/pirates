using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    private float rise, globalSpeedModifier, angle, size;
    private bool direction = false;
    private int islandNumber = 1;
    public string ShipName { get; private set; }
    private Island island;

    public int reward;
    public float raidTime;

    public int rewardModifier = 1;
    public float raidTimeModifier = 0f;

    //Рейд
    private bool inRaid = false, isRotate = true;
    private float speedAngle, speedLinear, speedRaidModifier, circle = 0f;
    private RectTransform _riseRT, _iconRT;

    [Header("Детали корабля")]
    public Transform _rise;
    public Transform _icon;
    public Transform _coin;

    private float riseOutOfScreen = 1700f;

    private void Awake()
    {
        _riseRT = _rise.GetComponent<RectTransform>();
        _iconRT = _icon.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateShip();
        riseOutOfScreen = Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(Screen.safeArea.height / 2f, 2f) + Mathf.Pow(Screen.safeArea.width / 2f, 2f)) * 1.05f, riseOutOfScreen, 5000f);
    }

    private void FixedUpdate()
    {
        if (!inRaid && isRotate)
        {
            globalSpeedModifier = island.GetParameter("GlobalSpeed" + islandNumber.ToString(), 0f);
            circle += (speedAngle * globalSpeedModifier) * Time.fixedDeltaTime;
            angle += (speedAngle * globalSpeedModifier) * Time.fixedDeltaTime;
            _riseRT.localEulerAngles = Vector3.forward * angle;
        }
        if (Mathf.Abs(circle) >= 180f)
        {
            circle = 0f;
            if (_coin.gameObject.activeInHierarchy)
                _coin.GetComponent<CoinCatcher>().CatchCoin();
            BeginRaidFromIsland();
        }
    }

    public void UpdateShip()
    {
        _rise.GetComponent<RectTransform>().sizeDelta = new Vector2(rise, 10f);
        _rise.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * angle;
        _icon.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * 180f * (direction ? 0f : 1f);
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size * (!direction ? 1 : -1) + Vector3.up * size + Vector3.forward;
        speedAngle = Math.Abs(speedAngle) * (direction ? 1 : -1);
        speedLinear = Math.Abs(speedLinear) * (direction ? 1 : -1);

        rewardModifier = 1;
        raidTimeModifier = 0f;
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
            if (_coin.gameObject.activeInHierarchy)
                _coin.GetComponent<CoinCatcher>().CatchCoin();
            StopAllCoroutines();
            StartCoroutine(Raid());

            EventManager.SendEvent("ShipGoToRaid", ShipName, false);
        }
    }
    
    public void BeginRaidFromIsland()
    {
        if (!inRaid)
        {
            if (isRotate)
            {
                inRaid = true;
                if (_coin.gameObject.activeInHierarchy)
                    _coin.GetComponent<CoinCatcher>().CatchCoin();
                StopAllCoroutines();
                StartCoroutine(Raid());

                EventManager.SendEvent("ShipGoToRaid", ShipName, true);
            }
        }
    }

    public bool IsRotate()
    {
        return isRotate;
    }

    private IEnumerator Raid()
    {
        isRotate = false;
        globalSpeedModifier = island.GetParameter("GlobalSpeed" + islandNumber.ToString(), 0f);
        do
        {
            _iconRT.localPosition += Vector3.down * (speedLinear * speedRaidModifier * globalSpeedModifier) * Time.deltaTime;
            yield return null;
        } while (Mathf.Abs(_iconRT.localPosition.y) < riseOutOfScreen);
        float seconds = raidTime / Mathf.Pow(2f, raidTimeModifier);
        yield return new WaitForSeconds(seconds);

        float globalEarnings = island.GetParameter("GlobalEarnings" + islandNumber.ToString(), 0f);
        _coin.GetComponent<CoinCatcher>().ActivateCoin((int)(reward * rewardModifier * globalEarnings));
        direction = !direction;
        _iconRT.localPosition = Vector3.left * rise + Vector3.up * riseOutOfScreen * (direction ? 1 : -1);
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size * (!direction ? 1 : -1) + Vector3.up * size + Vector3.forward;
        _icon.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * 180f * (direction ? 0f : 1f);

        inRaid = false;
        UpdateShip();

        float error = Mathf.Abs(speedLinear * speedRaidModifier * Time.deltaTime * 2f);

        while (Mathf.Abs(_iconRT.localPosition.y) > error)
        {
            _iconRT.localPosition += Vector3.down * speedLinear * Time.deltaTime;
            yield return null;
        }


        isRotate = true;
        if (_coin.gameObject.activeInHierarchy)
            _coin.GetComponent<CoinCatcher>().CatchCoin();
    }

    public void SetLocation(float rise, float angle)
    {
        this.rise = rise;
        this.angle = angle;
        //_rise.GetComponent<RectTransform>().sizeDelta = new Vector2(rise, 10f);
        //_rise.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * angle;
    }

    public void SetShip(float size, Sprite sprite)
    {
        //_icon.GetComponent<RectTransform>().localScale = Vector3.right * size + Vector3.up * size + Vector3.forward;
        this.size = size;
        _icon.GetComponent<Image>().sprite = sprite;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public void SetSpeed(float speedAngle, float speedLinear, float speedRaidModifier)
    {
        this.speedAngle = speedAngle;
        this.speedLinear = speedLinear;
        this.speedRaidModifier = speedRaidModifier;

    }

    public void SetRaid(float raidTime, int reward)
    {
        this.raidTime = raidTime;
        this.reward = reward;
    }

    public void CreateShip(float rise, float angle, float size, Sprite sprite, float speedAngle, float speedLinear
        , float speedRaidModifier, float raidTime, int reward, int islandNumber, string shipName, IslandController islandController)
    {
        island = Island.Instance();
        SetLocation(rise, angle);
        SetShip(size, sprite);
        SetSpeed(speedAngle, speedLinear, speedRaidModifier);
        SetRaid(raidTime, reward);
        this.islandNumber = islandNumber;
        this.ShipName = shipName;
        _icon.GetComponent<ShipClick>().islandController = islandController;
    }

    public bool isShipRotating()
    {
        return isRotate;
    }

    public float GetRaidTime()
    {
        return raidTime / Mathf.Pow(2f, raidTimeModifier);
    }
}
