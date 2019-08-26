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
        startPosY = rect.position.y;
    }

    public void Swap()
    {
        if (swaping) return;
        StartCoroutine(Swaping(startPosY + (opened ? 0f : rect.sizeDelta.y), duration));
        opened = !opened;
    }

    private IEnumerator Swaping(float newPosY, float duration)
    {
        swaping = true;

        float t = 0f, oldPosY = rect.position.y;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            rect.position = new Vector3(transform.position.x, Mathf.Lerp(oldPosY, newPosY, t), transform.position.z);
            yield return null;
        }

        swaping = false;
    }
}
