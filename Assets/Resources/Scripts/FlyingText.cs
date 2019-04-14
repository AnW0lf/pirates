using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingText : MonoBehaviour
{
    public bool money, speed, wheel, reward, exp;
    public GameObject moneyText, speedText, wheelText, rewardText, expText;
    public Sprite[] sprites;
    public Image expImage;

    private Island island;

    void Start()
    {
        if (money)
            moneyText.SetActive(true);
        if (speed)
            speedText.SetActive(true);
        if (wheel)
            wheelText.SetActive(true);
        if (reward)
            rewardText.SetActive(true);
        if (exp)
        {
            if (sprites.Length >= 3)
            {
                island = Island.Instance();
                if (island.Level < 25)
                {
                    expImage.sprite = sprites[0];
                }
                else if (island.Level < 50)
                {
                    expImage.sprite = sprites[1];
                }
                else
                {
                    expImage.sprite = sprites[2];
                }
            }
            expText.SetActive(true);
        }
    }

    private void Update()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
