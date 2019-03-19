using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    [Header("Положение корабля")]
    public float rise;
    public float angle;
    public bool direction;
    public float size;

    [Header("Движение")]
    public float speed;
    public float raidSpeedModifier;

    [Header("Характеристики")]
    public int reward;
    public int rewardModifier;
    public float raidTime;
    public float raidTimeModifier;

    [Header("Вид корабля")]
    public Sprite sprite;

    //Рейд
    private bool visible = false;
    private bool inRaid = false;
    private float angleSpeed, linearSpeed, circle = 0f;
    private RectTransform _riseRT, _iconRT;

    [Header("Детали корабля")]
    public Transform _rise;
    public Transform _icon;
    public Transform _coin;

    private void Awake()
    {
        _riseRT = _rise.GetComponent<RectTransform>();
        _iconRT = _icon.GetComponent<RectTransform>();
        rewardModifier = 1; raidTimeModifier = 1;
    }

    private void Start()
    {
        UpdateShip();
    }

    public void UpdateShip()
    {
        _rise.GetComponent<RectTransform>().sizeDelta = new Vector2(rise, 10f);
        _rise.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * angle;
        _icon.GetComponent<SpriteRenderer>().sprite = sprite;
        _icon.GetComponent<SpriteRenderer>().flipY = direction;
        _icon.GetComponent<RectTransform>().localScale = Vector3.right * size + Vector3.up * size + Vector3.forward;
        _icon.GetComponent<CapsuleCollider2D>().size = Vector2.right * size * 0.25f + Vector2.up * size * 0.5f;
        angleSpeed = 360f * speed / (2f * rise * Mathf.PI) * (direction ? 1 : -1);
        linearSpeed = speed * (direction ? 1 : -1);
    }

    private void FixedUpdate()
    {
        if(!inRaid)
        {
            circle += angleSpeed * Time.fixedDeltaTime;
            angle += angleSpeed * Time.fixedDeltaTime;
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
            _iconRT.localPosition += Vector3.down * (linearSpeed * raidSpeedModifier) * Time.deltaTime;
            yield return null;
        } while (visible);
        float seconds = raidTime / raidTimeModifier;
        yield return new WaitForSeconds(seconds);

        _coin.GetComponent<CoinCatcher>().ActivateCoin((reward * rewardModifier));
        angle = Random.Range(0f, 359f);
        _riseRT.localEulerAngles = Vector3.forward * angle;
        direction = !direction;
        _iconRT.localPosition = Vector3.left * rise + Vector3.up * (Screen.height * 0.6f + size) * (direction ? 1 : -1);
        _icon.GetComponent<SpriteRenderer>().flipY = direction;

        while(Mathf.Abs(_iconRT.localPosition.y) > 5f)
        {
            _iconRT.localPosition += Vector3.up * (linearSpeed) * Time.deltaTime;
            yield return null;
        }
        UpdateShip();
        inRaid = false;
    }
}
