using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipClick : MonoBehaviour
{
    public Ship ship;
    public LifebuoyManager lifebuoys;
    public GameObject flyingText;
    private GameObject _flyingText;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    // Собираем бонус
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bonus"))
        {
            other.gameObject.GetComponentInParent<BonusPoint>().active = false;

            _flyingText = Instantiate(flyingText, other.transform.parent);
            _flyingText.transform.localPosition = new Vector3(0f, 0f, 0f);

            if (other.gameObject.GetComponent<BonusBehavior>().bonusMoney)
            {
                ship.rewardModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                _flyingText.GetComponent<FlyingText>().money = true;
                _flyingText.GetComponent<FlyingText>().moneyText.GetComponent<Text>().text = "+" + (int)(ship.reward);

            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusSpeed)
            {
                ship.raidTimeModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
                _flyingText.GetComponent<FlyingText>().speed = true;
                _flyingText.GetComponent<FlyingText>().speedText.GetComponent<Text>().text = "-" + (int)(ship.raidTime / Mathf.Pow(2f, ship.raidTimeModifier)) + "s";
            }
            if (other.gameObject.GetComponent<BonusBehavior>().bonusWheel)
            {
                if (island.Level >= 2)
                {
                    lifebuoys.AddLifebuoy();
                    _flyingText.GetComponent<FlyingText>().wheel = true;
                }
            }

            Destroy(other.gameObject);
        }
    }
}
