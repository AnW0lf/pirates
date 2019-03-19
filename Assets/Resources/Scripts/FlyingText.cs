using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingText : MonoBehaviour
{
    public bool money, speed, wheel;
    public GameObject moneyText, speedText, wheelText;

    // Update is called once per frame
    void Update()
    {
        if (money)
            moneyText.SetActive(true);
        if (speed)
            speedText.SetActive(true);
        if (wheel)
            wheelText.SetActive(true);

        if (!GetComponent<Animation>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
