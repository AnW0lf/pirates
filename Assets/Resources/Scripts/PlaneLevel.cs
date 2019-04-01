using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLevel : MonoBehaviour
{
    public GameObject level;

    private void Start()
    {
        level.SetActive(true);
    }
}
