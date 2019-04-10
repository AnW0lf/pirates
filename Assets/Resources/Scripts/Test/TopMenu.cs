using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    void Start()
    {
        if(Screen.safeArea.yMax != Screen.safeArea.height)
        {
            GetComponent<RectTransform>().sizeDelta -= Vector2.up * Mathf.Abs(Screen.safeArea.height - Screen.safeArea.yMax);
        }
    }
}
