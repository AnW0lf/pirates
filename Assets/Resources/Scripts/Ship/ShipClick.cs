using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipClick : MonoBehaviour
{
    public ShipController ship;
    public GameObject flyingText;
    public Transform pointer;
    public Image clock, arrow;
    public Color color;
    public IslandController islandController = null;
    public TrailRenderer trail;

    private GameObject _flyingText;
    private Island island;
    private Camera cam;
    private Vector3 startPos;
    private bool isTimerActive;
    private int islandNumber;

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
        BorderController border;

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
                ft.expText.text = "+" + ship.item.reward.ToString();

                EventManager.SendEvent("BonusCollected", ship.item.name, "Material");
            }
            if (bonus.bonusMoney)
            {
                if (islandController != null)
                {
                    ShipInfoList list = GetComponentInParent<ShipsManager>().list;
                    Inventory inventory = Inventory.Instance;
                    BigDigit reward = inventory.GetShipPrice(inventory.selectedPanel.list, inventory.currentShips[inventory.panels.IndexOf(inventory.selectedPanel)]);

                    ft.money = true;
                    ft.moneyText.text = "+" + reward.ToString();
                    islandController.GenerateBonusMoney(reward);
                    EventManager.SendEvent("BonusCollected", ship.item.name, "Money");
                }
            }
            if (bonus.bonusSpeed)
            {
                ship.DurationBonus();
                ft.speed = true;
                ft.speedText.text = "-" + (int)(ship.duration / Mathf.Pow(2f, ship.durationModifier)) + "s";

                EventManager.SendEvent("BonusCollected", ship.item.name, "Speed");
            }
            if (bonus.bonusWheel)
            {
                if (island.Level >= 2)
                {
                    island.ChangeLifebuoy(1);
                    ft.wheel = true;
                    ft.wheelText.text = "+1";

                    EventManager.SendEvent("BonusCollected", ship.item.name, "WheelSpin");
                }
            }

            Destroy(other.gameObject);
        }
        else if (ship.Motor.isBack && isTimerActive && other.CompareTag("Border") && other.CompareTag("Border") && (border = other.GetComponent<BorderController>()) && border.islandNumber == ship.islandNumber)
        {
            Invoke("SwitchEmitting", 0.15f);
            isTimerActive = false;
            cldr.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BorderController border;
        if (!ship.Motor.isBack && other.CompareTag("Border") && (border = other.GetComponent<BorderController>()) && border.islandNumber == ship.islandNumber && !isTimerActive)
        {
            Invoke("SwitchEmitting", 0.15f);
            StartCoroutine(Timer(ship.GetRaidTime()));
        }
    }

    private IEnumerator Timer(float time)
    {
        isTimerActive = true;
        arrow.GetComponent<Image>().color = color;
        pointer.gameObject.SetActive(true);

        RectTransform rect = GetComponent<RectTransform>();
        float k = 0.5f;
        Vector3 pointerPos = rect.position + rect.up * k * Mathf.Sign(ship.img.transform.localScale.y);

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
        pointer.localPosition = rect.localPosition;
    }

    public void CldrOn()
    {
        cldr.enabled = true;
    }

    private void SwitchEmitting()
    {
        if (trail) trail.emitting = !trail.emitting;
    }

    private void OnDestroy()
    {
        Destroy(pointer.gameObject);
    }
}
