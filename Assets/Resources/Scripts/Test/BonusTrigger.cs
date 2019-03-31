using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTrigger : MonoBehaviour
{
    public Bonus.BonusType type;

    private Global global;

    private void Awake()
    {
        global = Global.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShipRaid sr = collision.transform.GetComponent<ShipRaid>();
            switch (type)
            {
                case Bonus.BonusType.MATERIAL:
                    global.AddMaterial(sr.reward);
                    EventManager.SendEvent("AddMaterial");
                    break;
                case Bonus.BonusType.DELAY:
                    sr.delay = sr.delay / 2f;
                    break;
                case Bonus.BonusType.SPIN:
                    EventManager.SendEvent("AddSpin");
                    break;
            }
            GetComponentInParent<Bonus>().active = false;
            gameObject.SetActive(false);
        }
    }
}
