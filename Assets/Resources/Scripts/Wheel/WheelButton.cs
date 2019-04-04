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
            GetComponent<Animation>().Play("WheelOpen");
            wheelOpened = true;
        }
        else
        {
            GetComponent<Animation>().Play("WheelClose");
            wheelOpened = false;
        }
    }


}
