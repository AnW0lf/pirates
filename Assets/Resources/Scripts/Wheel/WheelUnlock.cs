using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelUnlock : MonoBehaviour
{
    public int minLevel;

    private Island island;
    private Button btn;
    public GameObject unlockMenu, viewport; 

    private void Awake()
    {
        island = Island.Instance();
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        if (island.Level < minLevel)
        {
            viewport.SetActive(false);
        }
        else
        {
            UnlockWheel();
        }
    }

    public void Open()
    {
        if (island == null) island = Island.Instance();
        if (viewport == null) viewport = transform.GetChild(0).gameObject;
        if (island.Level >= minLevel && !viewport.activeInHierarchy)
        {
            unlockMenu.SetActive(true);
        }
    }

    public void UnlockWheel()
    {
        viewport.SetActive(true);
        //this.enabled = false;
    }
}
