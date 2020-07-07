using UnityEngine;
using System.Collections;

public class BonusTutorial : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _expPrefab;
    [SerializeField] private GameObject _speedPrefab;
    [SerializeField] private GameObject _moneyPrefab;
    [Header("Bonus Points")]
    [SerializeField] private BonusPoint _expPoint;
    [SerializeField] private BonusPoint _speedPoint;
    [SerializeField] private BonusPoint _moneyPoint;

    private void Start()
    {
        if (TutorialComplete) Destroy(gameObject);
    }

    private bool TutorialComplete
    {
        get
        {
            string key = "BonusTutorial";
            if (!PlayerPrefs.HasKey(key)) PlayerPrefs.SetInt(key, 0);
            return PlayerPrefs.GetInt(key) > 0;
        }
        set
        {
            string key = "BonusTutorial";
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }

    private void Spawn(GameObject prefab, BonusPoint point)
    {
        GameObject instance = Instantiate(prefab, point.transform);
        point.active = true;
    }

    public void Begin()
    {
        Spawn(_expPrefab, _expPoint);
        Spawn(_speedPrefab, _speedPoint);
        Spawn(_moneyPrefab, _moneyPoint);

        TutorialComplete = true;
        Destroy(gameObject);
    }
}
