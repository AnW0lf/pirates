using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipClick : MonoBehaviour
{
    public ShipController ship;
    public LifebuoyManager lifebuoys;
    public GameObject flyingText;
    public Transform pointer, arrow;
    public Image clock;
    public Color color;
    public IslandController islandController = null;
    public TrailRenderer trail;

    private GameObject _flyingText;
    private Island island;
    private Camera cam;
    private Vector3 startPos;
    private bool isTimerActive;
    private string borderName;

    private CapsuleCollider2D cldr;

    private void Awake()
    {
        island = Island.Instance;
        cam = Camera.main;
        cldr = GetComponent<CapsuleCollider2D>();
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

            BonusBehavior bonus = other.gameObject.GetComponent<BonusBehavior>();

            if (bonus.bonusMaterial)
            {
                ship.rewardModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                ft.exp = true;
                ft.expText.GetComponent<Text>().text = "+" + ship.item.reward.ToString();

                EventManager.SendEvent("BonusCollected", ship.item.name, "Material");
            }
            if (bonus.bonusMoney)
            {
                if (islandController != null)
                {
                    BigDigit reward = ship.item.price * (island.GetParameter("ShipAlltimeCount_" + GetComponentInParent<ShipsManager>().islandNumber + "_0" ,0) + 1) / 2f;
                    ft.money = true;
                    ft.moneyText.GetComponent<Text>().text = "+" + reward.ToString();
                    islandController.GenerateBonusMoney(reward);
                    EventManager.SendEvent("BonusCollected", ship.item.name, "Money");
                }
            }
            if (bonus.bonusSpeed)
            {
                ship.DurationBonus(other.gameObject.GetComponent<BonusBehavior>().modifier);
                ft.speed = true;
                ft.speedText.GetComponent<Text>().text = "-" + (int)(ship.duration / Mathf.Pow(2f, ship.durationModifier)) + "s";

                EventManager.SendEvent("BonusCollected", ship.item.name, "Speed");
            }
            if (bonus.bonusWheel)
            {
                if (island.Level >= 2)
                {
                    lifebuoys.AddLifebuoy();
                    _flyingText.GetComponent<FlyingText>().wheel = true;

                    EventManager.SendEvent("BonusCollected", ship.item.name, "WheelSpin");
                }
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Border") && !isTimerActive)
        {
            borderName = other.gameObject.name;
            Invoke("SwitchEmitting", 0.15f);
            if (other.gameObject.name.Equals("RightBorder") || other.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime() + 1.5f, true));
            else
                StartCoroutine(Timer(ship.GetRaidTime(), false));
        }
        else if (other.CompareTag("Border") && isTimerActive && borderName == other.gameObject.name)
        {
            borderName = "";
            Invoke("SwitchEmitting", 0.4f);
            isTimerActive = false;
            cldr.enabled = false;
        }
    }

    private IEnumerator Timer(float time, bool isSide)
    {
        isTimerActive = true;
        arrow.GetComponent<Image>().color = color;
        pointer.gameObject.SetActive(true);

        float height = 2f * cam.orthographicSize, width = height * cam.aspect, xPos = transform.position.x, yPos = transform.position.y;

        Vector3 pointerPos = new Vector3(isSide ? (xPos > 0f ? width / 2f : -width / 2f) : xPos,
            isSide ? yPos : (yPos > 0f ? height / 2f - 0.7f : -height / 2f + 2f), transform.position.z);

        pointer.position = pointerPos;
        pointer.eulerAngles = transform.eulerAngles;
        pointer.SetParent(transform.parent);

        for (float i = 0f; i < time; i += Time.deltaTime)
        {
            clock.fillAmount = i / time;
            yield return null;
        }

        pointer.gameObject.SetActive(false);
        pointer.localScale = ship.img.transform.localScale;
        pointer.SetParent(transform);
    }

    public void CldrOn()
    {
        cldr.enabled = true;
    }

    private void SwitchEmitting()
    {
        if (trail) trail.emitting = !trail.emitting;
    }
}
