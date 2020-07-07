using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    private void Start()
    {
        StartCoroutine(SpriteOn());
    }

    IEnumerator SpriteOn()
    {
        yield return new WaitForSeconds(4.3f);
        GetComponent<Image>().enabled = true;
    }
}
