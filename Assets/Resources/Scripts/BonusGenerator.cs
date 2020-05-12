using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public int bonusDelay;
    public Bonus[] bonuses;

    private int randomPoint, curDelay;
    private GameObject _bonus;
    private Island island;

    void OnEnable()
    {
        island = Island.Instance();
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

            foreach (BonusPoint child in GetComponentsInChildren<BonusPoint>())
            {
                if (!child.GetComponent<BonusPoint>().active)
                {
                    children.Add(child.transform);
                }
            }

            while (children.Count > 0)
            {
                yield return wait;
                Transform child = children[Random.Range(0, children.Count)];
                SetBonus(child, RandomBonus.prefab);
                children.Remove(child);
            }
        }
    }

    private Bonus RandomBonus
    {
        get
        {
            if (bonuses.Length == 0) return null;

            List<Bonus> unlocked = new List<Bonus>();
            foreach (Bonus bonus in bonuses)
                if (island.Level >= bonus.unlockLevel) unlocked.Add(bonus);

            if (unlocked.Count == 0) return null;

            int max = unlocked.Sum(b => b.chance);
            int rnd = Random.Range(0, max);

            for (int i = 0; i < unlocked.Count; i++)
            {
                if (rnd < unlocked[i].chance) return unlocked[i];
                else rnd -= unlocked[i].chance;
            }
            return unlocked.Last();
        }
    }

    private void SetBonus(Transform child, GameObject prefab)
    {
        _bonus = Instantiate(prefab, child);
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
            SetBonus(child, bonuses[bonus].prefab);
            children.Remove(child);
        }
    }

    public void InstantiateRandomBonus(int count)
    {
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
            SetBonus(child, RandomBonus.prefab);
            children.Remove(child);
        }
    }
}

[System.Serializable]
public class Bonus
{
    public string name;
    public GameObject prefab;
    public int chance;
    public int unlockLevel;

    public Bonus(string name, GameObject prefab, int chance, int unlockLevel)
    {
        this.name = name;
        this.prefab = prefab;
        this.chance = chance;
        this.unlockLevel = unlockLevel;
    }
}
