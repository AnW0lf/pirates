using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public int bonusDelay;
    public GameObject[] bonuses;

    private int activeBonuses, maxBonuses, randomPoint, randomBonus, curDelay;
    private GameObject _bonus;

    void OnEnable()
    {
        curDelay = bonusDelay;
        activeBonuses = 0; maxBonuses = 0;
        StartCoroutine(BonusSpawner());
    }

    void Update()
    {
        
    }

    IEnumerator BonusSpawner()
    {
        while (true)
        {
            Debug.Log(curDelay);
            yield return new WaitForSeconds(curDelay);

            maxBonuses = 0; activeBonuses = 0;

            foreach (BonusPoint point in GetComponentsInChildren<BonusPoint>())
            {
                maxBonuses += 1;

                if (point.active)
                    activeBonuses += 1;
            }

            if (activeBonuses < maxBonuses)
            {
                do
                {
                    randomPoint = (int)Random.Range(0f, GetComponentsInChildren<BonusPoint>().Length);
                }
                while (transform.GetChild(randomPoint).gameObject.GetComponent<BonusPoint>().active);

                randomBonus = (int)Random.Range(0f, bonuses.Length);
                _bonus = Instantiate(bonuses[randomBonus], transform.GetChild(randomPoint));
                _bonus.transform.localPosition = new Vector3(Random.Range(-_bonus.GetComponent<RectTransform>().sizeDelta.x / 2f, _bonus.GetComponent<RectTransform>().sizeDelta.x / 2f), Random.Range(-_bonus.GetComponent<RectTransform>().sizeDelta.y / 2f, _bonus.GetComponent<RectTransform>().sizeDelta.y / 2f), _bonus.transform.localPosition.z);
                transform.GetChild(randomPoint).gameObject.GetComponent<BonusPoint>().active = true;
            }
        }
    }
}
