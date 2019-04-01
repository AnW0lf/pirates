using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenuLevel : MonoBehaviour
{
    public GameObject level;

    private void Start()
    {
        level.SetActive(true);
    }
}
