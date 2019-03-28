using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public int bonusDelay;
    public GameObject[] bonuses;

    private int randomPoint, curDelay;
    private GameObject _bonus;

    void OnEnable()
    {
        curDelay = bonusDelay;
        StartCoroutine(BonusSpawner());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator BonusSpawner()
    {
        while (true)
        {
            //Debug.Log(curDelay);
            WaitForSeconds wait = new WaitForSeconds(curDelay);
            yield return wait;

            List<Transform> children = new List<Transform>();

            foreach(BonusPoint child in GetComponentsInChildren<BonusPoint>())
            {
                if(!child.GetComponent<BonusPoint>().active)
                {
                    children.Add(child.transform);
                }
            }

            while (children.Count > 0)
            {
                yield return wait;
                Transform child = children[Random.Range(0, children.Count)];
                SetBonus(child, Random.Range(0, bonuses.Length));
                children.Remove(child);
            }
        }
    }

    private void SetBonus(Transform child, int bonus)
    {
        _bonus = Instantiate(bonuses[bonus], child);
        _bonus.transform.localPosition = new Vector3(Random.Range(-_bonus.GetComponent<RectTransform>().sizeDelta.x / 2f,
            _bonus.GetComponent<RectTransform>().sizeDelta.x / 2f), Random.Range(-_bonus.GetComponent<RectTransform>().sizeDelta.y / 2f,
            _bonus.GetComponent<RectTransform>().sizeDelta.y / 2f), _bonus.transform.localPosition.z);
        child.GetComponent<BonusPoint>().active = true;
    }

    public void Bonus(int bonus, int count)
    {
        if (bonuses.Length <= bonus) return;
        List<Transform> children = new List<Transform>();

        foreach (BonusPoint child in GetComponentsInChildren<BonusPoint>())
        {
            if (!child.GetComponent<BonusPoint>().active)
            {
                children.Add(child.transform);
            }
        }

        for (int i = 0; children.Count > 0 && i < count; i++)
        {
            Transform child = children[Random.Range(0, children.Count)];
            SetBonus(child, bonus);
            children.Remove(child);
        }
    }
}
