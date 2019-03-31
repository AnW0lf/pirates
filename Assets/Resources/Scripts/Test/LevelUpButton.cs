using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpButton : MonoBehaviour
{
    private Global global;

    private void Awake()
    {
        global = Global.Instance;
    }

    public void Click()
    {
        global.LevelUp();
        EventManager.SendEvent("LevelUp");
    }
}
