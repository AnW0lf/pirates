using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeMenuSlot : MonoBehaviour
{
    public GameObject lockIcon, shipIcon;
    public Text levelText;
    public int SlotNumber { get; set; }

    public bool Lock { get { return lockIcon.activeInHierarchy; } set { lockIcon.SetActive(value); } }
    public bool Fill { get { return shipIcon.activeInHierarchy; } set { shipIcon.SetActive(Lock ? false : value); } }

    public void SetIcon(Sprite icon, int level)
    {
        shipIcon.GetComponent<Image>().sprite = icon;
        levelText.text = level.ToString();
    }
}
