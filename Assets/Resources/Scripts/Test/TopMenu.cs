using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    private RectTransform rect;

    void Start()
    {
        if (Screen.safeArea.yMax != Screen.safeArea.height)
        {
            rect = GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 0.9125f);
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
        }
        this.enabled = false;
    }
}
