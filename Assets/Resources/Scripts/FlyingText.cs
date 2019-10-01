using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingText : MonoBehaviour
{
    public bool money, speed, wheel, reward, exp;
    public Text moneyText, speedText, wheelText, rewardText, expText;
    public Sprite[] sprites;
    public Image expImage;

    private Island island;

    void Start()
    {
        if (money)
            moneyText.gameObject.SetActive(true);
        if (speed)
            speedText.gameObject.SetActive(true);
        if (wheel)
            wheelText.gameObject.SetActive(true);
        if (reward)
            rewardText.gameObject.SetActive(true);
        if (exp)
        {
            if (sprites.Length >= 3)
            {
                island = Island.Instance;
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
            expText.gameObject.SetActive(true);
        }
    }

    public void SetReward(int value)
    {
        rewardText.text = "+" + CheckRange(value);
    }

    public void SetExp(int value)
    {
        expText.text = "+" + CheckRange(value);
    }

    private string CheckRange(int value)
    {
        if (value < 10000)
        {
            return value.ToString();
        }
        else
        {
            float v = value, degree;
            for (degree = 0; v > 1000f; degree++, v /= 1000f) ;

            string str = v.ToString();
            str = str.Length >= 5 ? str.Substring(0, 5) : str;
            str = str.Replace(',', '.');
            for (; str.Length < 5; str = str.Contains(".") ? str += "0" : str += ".") ;

            switch (degree)
            {
                case 0: return str;
                case 1: return str + "K";
                case 2: return str + "M";
                case 3: return str + "B";
                case 4: return str + "T";
                case 5: return str + "q";
                case 6: return str + "Q";
                case 7: return str + "s";
                case 8: return str + "S";
                default: return str + "?";
            }
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
