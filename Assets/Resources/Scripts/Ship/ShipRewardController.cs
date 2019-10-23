using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipRewardController : MonoBehaviour
{
    [SerializeField]
    private Image icon = null;
    [SerializeField]
    private Sprite[] materials = null;

    private Transform icon_transform;
    private GameObject icon_object;
    private Island island;
    private ShipRewardManager manager;

    private void Awake()
    {
        island = Island.Instance;
        icon_transform = icon.transform;
        icon_object = icon.gameObject;
        manager = transform.GetComponentInParent<ShipRewardManager>();
        if (!manager) Debug.LogWarning("ShipRewardManager is not found!..");
    }

    public void EnableIcon()
    {
        if (!icon_object.activeInHierarchy) icon_object.SetActive(true);
        if (materials.Length >= 3)
        {
            if (island.Level < 25)
            {
                icon.sprite = materials[0];
            }
            else if (island.Level < 50)
            {
                icon.sprite = materials[1];
            }
            else
            {
                icon.sprite = materials[2];
            }
        }
    }

    public void DisableIcon(BigDigit reward)
    {
        island.SetParameter("QuitTime", DateTime.Now.ToString());
        if (icon_object.activeInHierarchy) icon_object.SetActive(false);
        if (manager) manager.AddExp(reward);
    }
}
