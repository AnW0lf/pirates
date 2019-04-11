using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardState : MonoBehaviour
{
    public int dayNumber, reward;
    public Text dayText, rewardText;
    public GameObject flag;
    public Color enable, focused, disable;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        dayText.text = "Day " + dayNumber.ToString();
        rewardText.text = "+" + reward.ToString();
    }

    public void SetState(int state)
    {
        switch (state)
        {
            case 0:
                image.color = disable;
                flag.SetActive(true);
                break;
            case 1:
                image.color = focused;
                flag.SetActive(false);
                break;
            case 2:
                image.color = enable;
                flag.SetActive(false);
                break;
            default:
                break;
        }
    }
}
