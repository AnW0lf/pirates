using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelButton : MonoBehaviour
{
    public bool wheelOpened;

    public void WheelSwitch()
    {
        if (!wheelOpened)
        {
            gameObject.GetComponent<Animation>().Play("WheelOpen");
            wheelOpened = true;
        }
        else
        {
            gameObject.GetComponent<Animation>().Play("WheelClose");
            wheelOpened = false;
        }
    }
}
