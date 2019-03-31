using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public GameObject money, delay, spin;
    public bool active = false;

    public enum BonusType { MATERIAL, DELAY, SPIN };

    public void AddBonus(BonusType type)
    {
        if (active) return;
        active = true;
        switch (type)
        {
            case BonusType.MATERIAL:
                money.SetActive(true);
                money.transform.localPosition = new Vector3(Random.Range(-money.GetComponent<RectTransform>().sizeDelta.x / 2f,
                    money.GetComponent<RectTransform>().sizeDelta.x / 2f), Random.Range(-money.GetComponent<RectTransform>().sizeDelta.y / 2f,
                    money.GetComponent<RectTransform>().sizeDelta.y / 2f), money.transform.localPosition.z);
                break;
            case BonusType.DELAY:
                delay.SetActive(true);
                delay.transform.localPosition = new Vector3(Random.Range(-delay.GetComponent<RectTransform>().sizeDelta.x / 2f,
                    delay.GetComponent<RectTransform>().sizeDelta.x / 2f), Random.Range(-delay.GetComponent<RectTransform>().sizeDelta.y / 2f,
                    delay.GetComponent<RectTransform>().sizeDelta.y / 2f), delay.transform.localPosition.z);
                break;
            case BonusType.SPIN:
                spin.SetActive(true);
                spin.transform.localPosition = new Vector3(Random.Range(-spin.GetComponent<RectTransform>().sizeDelta.x / 2f,
                    spin.GetComponent<RectTransform>().sizeDelta.x / 2f), Random.Range(-spin.GetComponent<RectTransform>().sizeDelta.y / 2f,
                    spin.GetComponent<RectTransform>().sizeDelta.y / 2f), spin.transform.localPosition.z);
                break;
        }
    }
}
