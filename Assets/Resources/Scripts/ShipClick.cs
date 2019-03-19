using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClick : MonoBehaviour
{
    public Ship ship;
    private bool visible = true;

    public bool IsVisible()
    {
        return visible;
    }

    private void OnMouseUpAsButton()
    {
        if (!ship.InRaid())
            ship.BeginRaid();
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    // Собираем бонус
    public void OnTriggerEnter2D(Collider2D other)
    {
         if (other.gameObject.CompareTag("Bonus"))
        {
            other.gameObject.GetComponentInParent<BonusPoint>().active = false;
            Destroy(other.gameObject);

            if (other.gameObject.GetComponent<BonusBehavior>().bonusMoney)
                ship.rewardModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
            if (other.gameObject.GetComponent<BonusBehavior>().bonusSpeed)
                ship.raidTimeModifier += other.gameObject.GetComponent<BonusBehavior>().modifier;
        }
    }
}
