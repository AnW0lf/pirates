using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewShipWindow : MonoBehaviour
{
    public GameObject fade, window;

    public Text shipName;
    public Image shipIcon;
    public Button lookBtn;
    public Transform[] piersArray;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        fade.SetActive(false);
        window.SetActive(false);
    }

    public void Open()
    {
        foreach (Transform piers in piersArray)
        {
            foreach (PierManager pier in piers.GetComponentsInChildren<PierManager>())
            {
                if (pier.minLvl == island.Level)
                {
                    fade.SetActive(true);
                    window.SetActive(true);
                    shipName.text = pier.shipName;
                    shipIcon.sprite = pier.shipIcon;
                    break;
                }
            }
        }
    }
}
