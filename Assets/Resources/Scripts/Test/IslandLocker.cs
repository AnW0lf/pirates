using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandLocker : MonoBehaviour
{
    public int unlockLevel;
    public GameObject piers, tools, fade, icon;

    private Global global;

    private void Awake()
    {
        global = Global.Instance;
    }

    private void Start()
    {
        icon.SetActive(true);
        if (global.Level >= unlockLevel)
        {
            fade.SetActive(false);
            piers.SetActive(true);
            tools.SetActive(true);
        }
        else
        {
            fade.SetActive(true);
            piers.SetActive(false);
            tools.SetActive(false);
        }
    }
}
