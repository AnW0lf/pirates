using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MenuSwap : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.5f;

    private float startPosY;
    private RectTransform rect;
    private bool swaping = false, opened = false;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startPosY = rect.anchoredPosition.y;
    }

    public void Swap()
    {
        if (swaping) return;
        StartCoroutine(Swaping((opened ? startPosY : rect.sizeDelta.y), duration));
        opened = !opened;
    }

    private IEnumerator Swaping(float newPosY, float duration)
    {
        swaping = true;

        float t = 0f, oldPosY = rect.anchoredPosition.y;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(oldPosY, newPosY, t));
            yield return null;
        }

        swaping = false;
    }
}
