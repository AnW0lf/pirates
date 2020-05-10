using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinRushTextSwitcher : MonoBehaviour
{
    public int duration;
    public string[] texts;

    void OnEnable()
    {
        StartCoroutine(TextSwitch(duration));
    }
    private void OnDisable()
    {
        StopCoroutine(TextSwitch(duration));
    }

    private IEnumerator TextSwitch(int duration)
    {
        while (gameObject.activeSelf)
        {
            gameObject.GetComponent<Text>().text = texts[(int)Random.Range(0, texts.Length)];
            yield return new WaitForSeconds(3f);
        }
    }
}
