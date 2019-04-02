using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceActivator : MonoBehaviour
{
    public GameObject[] children;

    private void Start()
    {
        foreach (GameObject child in children)
            child.SetActive(true);
    }
}
