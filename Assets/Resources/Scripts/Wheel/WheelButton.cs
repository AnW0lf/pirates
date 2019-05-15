using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelButton : MonoBehaviour
{
    public bool wheelOpened;
    public GameObject quests;

    public void WheelSwitch()
    {
        if (!wheelOpened)
        {
            GetComponent<Animation>().Play("WheelOpen");
            quests.SetActive(false);
            wheelOpened = true;
        }
        else
        {
            GetComponent<Animation>().Play("WheelClose");
            quests.SetActive(true);
            wheelOpened = false;
        }
    }

    public void WheelSwitchOn()
    {
        GetComponent<Animation>().Play("WheelOpen");
        quests.SetActive(false);
        wheelOpened = true;
    }


}
