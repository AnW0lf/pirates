using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOpenClose : MonoBehaviour
{
    private Animation anim;
    private bool isOpen = false;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void Open()
    {
        if (isOpen)
        {
            anim.Play("WheelClose");
            isOpen = false;
        }
        else
        {
            anim.Play("WheelOpen");
            isOpen = true;
        }
    }
}
