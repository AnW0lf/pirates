using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipClick : MonoBehaviour
{
    public Ship ship;
    public LifebuoyManager lifebuoys;
    public GameObject flyingText;
    public Transform pointer, arrow;
    public Image clock;
    public Color color;
    public TrailRenderer trail;
    public IslandController islandController = null;

    private GameObject _flyingText;
    private Island island;
    private Camera cam;
    private Vector3 startPos;
    private bool isTimerActive;
    private string borderName;

    private void Awake()
    {
        island = Island.Instance();
        cam = Camera.main;
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

            if (other.gameObject.GetComponent<BonusBehavior>().bonusMaterial)
            {
                ship.rewardModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                ft.exp = true;
                ft.expText.GetComponent<Text>().text = "+" + ship.reward.ToString();

                EventManager.SendEvent("BonusCollected", ship.ShipName, "Material");
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusMoney)
            {
                if (islandController != null)
                {
                    BigDigit reward = islandController.GetReward() * 5 * island.Level;
                    ft.money = true;
                    ft.moneyText.GetComponent<Text>().text = "+" + reward.ToString();
                    islandController.GenerateBonusMoney(reward);
                    EventManager.SendEvent("BonusCollected", ship.ShipName, "Money");
                }
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusSpeed)
            {
                ship.raidTimeModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                ft.speed = true;
                ft.speedText.GetComponent<Text>().text = "-" + (int)(ship.raidTime / Mathf.Pow(2f, ship.raidTimeModifier)) + "s";

                EventManager.SendEvent("BonusCollected", ship.ShipName, "Speed");
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusWheel)
            {
                if (island.Level >= 2)
                {
                    lifebuoys.AddLifebuoy();
                    _flyingText.GetComponent<FlyingText>().wheel = true;

                    EventManager.SendEvent("BonusCollected", ship.ShipName, "WheelSpin");
                }
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Border") && !isTimerActive)
        {
            borderName = other.gameObject.name;
            Invoke("SwitchEmmiting", 0.15f);
            if (other.gameObject.name.Equals("RightBorder") || other.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime() + 1.5f, true));
            else
                StartCoroutine(Timer(ship.GetRaidTime(), false));
        }
        else if (other.CompareTag("Border") && isTimerActive && borderName == other.gameObject.name)
        {
            borderName = "";
            Invoke("SwitchEmmiting", 0.4f);
            isTimerActive = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    private IEnumerator Timer(float time, bool isSide)
    {
        isTimerActive = true;
        arrow.GetComponent<Image>().color = color;
        pointer.gameObject.SetActive(true);
        float height = 2f * cam.orthographicSize, width = height * cam.aspect, xPos = transform.position.x, yPos = transform.position.y;
        Vector3 pointerPos = new Vector3(isSide ? (xPos > 0f ? width / 2f : -width / 2f) : xPos,
            isSide ? yPos : (yPos > 0f ? height / 2f - 0.5f : -height / 2f + 1.7f), transform.position.z);
        pointer.position = pointerPos;
        pointer.eulerAngles = transform.eulerAngles;
        for (float i = 0f; i < time; i += Time.deltaTime)
        {
            clock.fillAmount = i / time;
            yield return null;
        }
        pointer.gameObject.SetActive(false);
    }

    private void SwitchEmmiting()
    {
        trail.emitting = !trail.emitting;
    }
}
