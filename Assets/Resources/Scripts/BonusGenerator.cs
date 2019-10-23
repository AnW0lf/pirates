using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public Vector2Int levelRange;
    public int bonusDelay;
    public GameObject[] bonuses;
    public int[] chances;

    private int randomPoint, curDelay;
    private GameObject _bonus;
    private Island island;

    void OnEnable()
    {
        curDelay = bonusDelay;
        StartCoroutine(BonusSpawner());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        island = Island.Instance;
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
                SetBonus(child, GetRandomBonusNumber());
                children.Remove(child);
            }
        }
    }

    private int GetRandomBonusNumber()
    {
        if (chances.Length != bonuses.Length) return 0;
        int i = 0;
        int maxChance = 0;
        foreach (int item in chances)
            maxChance += item;
        int chance = Random.Range(0, maxChance);
        for (i = 0; i < chances.Length; i++)
        {
            if (chance - chances[i] < 0)
                break;
            chance -= chances[i];
        }
        if ((island.Level < levelRange.x || island.Level > levelRange.y) && i == chances.Length - 1 && i != 0)
            i--;
        return i;
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

    public void RandomBonus(int count)
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
            SetBonus(child, GetRandomBonusNumber());
            children.Remove(child);
        }
    }
}
