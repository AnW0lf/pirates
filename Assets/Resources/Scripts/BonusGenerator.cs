using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public bool drawGizmos = false;
    public int bonusSpawnDelay;
    public float xRange, yRange;
    public GameObject[] bonuses;
    public int[] chances;
    public Transform[] points;

    void OnEnable()
    {
        StartCoroutine(BonusSpawner());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private List<Transform> EmptyPoints
    {
        get
        {
            List<Transform> emptyPoints = new List<Transform>();

            foreach (Transform point in points)
            {
                if (point.childCount == 0)
                    emptyPoints.Add(point);
            }

            return emptyPoints;
        }
    }

    IEnumerator BonusSpawner()
    {
        while (true)
        {
            WaitForSeconds wait = new WaitForSeconds(bonusSpawnDelay);
            yield return wait;

            List<Transform> emptyPoints = EmptyPoints;

            while (emptyPoints.Count > 0)
            {
                yield return wait;
                Transform child = emptyPoints[Random.Range(0, emptyPoints.Count)];
                SetBonus(child, GetRandomBonusNumber());
                emptyPoints.Remove(child);
            }
        }
    }

    private int GetRandomBonusNumber()
    {
        if (chances.Length != bonuses.Length) return 0;
        int maxChance = chances.Sum();
        int chance = Random.Range(0, maxChance);
        int i;
        for (i = 0; i < chances.Length; i++)
        {
            if (chance - chances[i] <= 0)
                break;
            chance -= chances[i];
        }
        if (Island.Instance().Level < 2 && i == chances.Length - 1 && i != 0)
            i--;
        return i;
    }

    private void SetBonus(Transform container, int bonus)
    {
        GameObject _bonus = Instantiate(bonuses[bonus], container);
        _bonus.transform.localPosition = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), _bonus.transform.localPosition.z);
        _bonus.gameObject.layer = gameObject.layer;
    }

    public void Bonus(int bonus, int count)
    {
        if (bonuses.Length <= bonus) return;
        List<Transform> emptyPoints = EmptyPoints;

        for (int i = 0; emptyPoints.Count > 0 && i < count; i++)
        {
            Transform child = emptyPoints[Random.Range(0, emptyPoints.Count)];
            SetBonus(child, bonus);
            emptyPoints.Remove(child);
        }
    }

    public void RandomBonus(int count)
    {
        List<Transform> emptyPoints = EmptyPoints;

        for (int i = 0; emptyPoints.Count > 0 && i < count; i++)
        {
            Transform child = emptyPoints[Random.Range(0, emptyPoints.Count)];
            SetBonus(child, GetRandomBonusNumber());
            emptyPoints.Remove(child);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Vector3 cubeSize = new Vector3(Mathf.Abs(xRange * 2f), Mathf.Abs(yRange * 2f), 0.1f);
        foreach (Transform point in points) Gizmos.DrawCube(point.position, cubeSize);
    }
}
