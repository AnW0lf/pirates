using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelContentController : MonoBehaviour
{
    public float speed, onY, offY;

    public bool IsOpen { get; private set; }

    private bool isScrolling = false;
    private float minDis = 10f;

    public void Scroll()
    {
        if (!isScrolling)
            StartCoroutine(Scrolling());
    }

    public IEnumerator Scrolling()
    {
        isScrolling = true;
        while(transform.position.y - (IsOpen ? onY : offY) > minDis)
        {
            transform.position += Vector3.up * (IsOpen ? 1f : -1f) * speed * Time.deltaTime;
            Debug.Log("Scrolling");
            yield return null;
        }
        IsOpen = !IsOpen;
        isScrolling = false;
    }
}
