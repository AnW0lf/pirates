using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiersUpgrade : MonoBehaviour
{
    public List<PierManager> piers;

    public void OpenShipUpgradeMenu()
    {
        EventManager.SendEvent("OpenShipUpgradeMenu", piers);
        Debug.Log("Open");
    }
}
