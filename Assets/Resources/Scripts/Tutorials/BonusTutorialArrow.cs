using UnityEngine;
using System.Collections;

public class BonusTutorialArrow : MonoBehaviour
{
    private bool ShipTutorial
    {
        get
        {
            string key = "ShipTutorial";
            if (!PlayerPrefs.HasKey(key)) PlayerPrefs.SetInt(key, 1);
            return PlayerPrefs.GetInt(key) > 0;
        }
    }

    void Update()
    {
        if (!ShipTutorial) Destroy(gameObject);
    }
}
