using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeMenuSlot : MonoBehaviour
{
    public GameObject lockIcon, shipIcon;

    public bool Lock { get { return lockIcon.activeInHierarchy; } set { lockIcon.SetActive(value); } }
    public bool Fill { get { return shipIcon.activeInHierarchy; } set { shipIcon.SetActive(Lock ? false : value); } }
}
