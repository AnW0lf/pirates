using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public int bonusDelay;
    public GameObject[] bonuses;

    private int activeBonuses, maxBonuses, randomPoint, randomBonus;
    private GameObject _bonus;

    void Start()
    {
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

            yield return new WaitForSeconds(bonusDelay);

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
                transform.GetChild(randomPoint).gameObject.GetComponent<BonusPoint>().active = true;
            }
        }
    }
}
