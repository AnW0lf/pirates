using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMoneyPulse : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.Subscribe("AddMoneyPulse", Pulse);
    }

    private void Pulse(object[] arg0)
    {
        anim.SetTrigger("AddMoney");
    }
}
