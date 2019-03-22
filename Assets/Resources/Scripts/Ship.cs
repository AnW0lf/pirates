using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    private float rise;
    private float angle;
    private bool direction = false;
    private float size;

    public int reward;
    public float raidTime;

    public int rewardModifier = 1;
    public float raidTimeModifier = 0f;

    //Рейд
    private bool visible = false;
    private bool inRaid = false, isRotate = true;
    private float speedAngle, speedLinear, speedRaidModifier, circle = 0f;
    private RectTransform _riseRT, _iconRT;

    [Header("Детали корабля")]
    public Transform _rise;
    public Transform _icon;
    public Transform _coin;

    private float riseOutOfScreen = 1400f;

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
        if(!inRaid && isRotate)
        {
            circle += (speedAngle * PlayerPrefs.GetFloat("GlobalSpeed")) * Time.fixedDeltaTime;
            angle += (speedAngle * PlayerPrefs.GetFloat("GlobalSpeed")) * Time.fixedDeltaTime;
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
        _icon.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * 180f * (direction ? 0f : 1f);
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size + Vector3.up * size + Vector3.forward;
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
            StopCoroutine(Raid());
            StartCoroutine(Raid());
        }
    }

    private IEnumerator Raid()
    {
        isRotate = false;
        do
        {
            _iconRT.localPosition += Vector3.down * (speedLinear * speedRaidModifier * PlayerPrefs.GetFloat("GlobalSpeed")) * Time.deltaTime;
            yield return null;
        } while (Mathf.Abs(_iconRT.localPosition.y) < riseOutOfScreen);
        float seconds = raidTime / Mathf.Pow(2f, raidTimeModifier);
        yield return new WaitForSeconds(seconds);

        _coin.GetComponent<CoinCatcher>().ActivateCoin((int)(reward * rewardModifier * PlayerPrefs.GetFloat("GlobalEarnings")));
        direction = !direction;
        _iconRT.localPosition = Vector3.left * rise + Vector3.up * riseOutOfScreen * (direction ? 1 : -1);
        _icon.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * 180f * (direction ? 0f : 1f);

        inRaid = false;
        UpdateShip();

        while (Mathf.Abs(_iconRT.localPosition.y) > 5f)
        {
            _iconRT.localPosition += Vector3.down * speedLinear * Time.deltaTime;
            yield return null;
        }


        isRotate = true;
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

    public void CreateShip(float rise, float angle, float size, Sprite sprite, float speedAngle, float speedLinear, float speedRaidModifier, float raidTime, int reward)
    {
        SetLocation(rise, angle);
        SetShip(size, sprite);
        SetSpeed(speedAngle, speedLinear, speedRaidModifier);
        SetRaid(raidTime, reward);
    }
}
