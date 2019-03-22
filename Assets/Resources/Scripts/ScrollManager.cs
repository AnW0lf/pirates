using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public int level;
    private RectTransform rect;
    private Island island;
    private float unit = 1920f;
    private int childCount;

    private void Awake()
    {
        island = Island.Instance();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        childCount = transform.childCount;
        float sizeY = childCount * unit;
        Vector2 pos = new Vector3(rect.localPosition.x, sizeY - (unit * (1 + island.Level / level)), rect.localPosition.z);
        rect.sizeDelta = new Vector2(.0f, sizeY);
        rect.localPosition = pos;
    }
}
