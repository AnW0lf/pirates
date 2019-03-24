using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteRotation : MonoBehaviour
{
    public bool direction;
    public float speed, rotationTime;
    public bool IsRolling { get; private set; }

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        IsRolling = false;
    }

    public void Roll(float angle)
    {
        if (IsRolling) return;
        StartCoroutine(Rolling(angle));
    }

    private IEnumerator Rolling(float angle)
    {
        IsRolling = true;
        float a = .0f;
        Vector3 direction = Vector3.back * (this.direction ? -1f : 1f);
        while (a < rotationTime)
        {
            rect.Rotate(direction, speed * Time.deltaTime);
            a += Time.deltaTime;
            yield return null;
        }
        a = angle - 180f < 0f ? angle - 180f + 360f : angle - 180f;
        while (Mathf.Abs(rect.localEulerAngles.z - a) >= 4f)
        {
            rect.Rotate(direction, speed * Time.deltaTime);
            yield return null;
        }
        a = speed * speed / 360f;
        float stopSpeed = speed;
        while (Mathf.Abs(rect.localEulerAngles.z - angle) >= 1f && stopSpeed > 0f)
        {
            rect.Rotate(direction, stopSpeed * Time.deltaTime);
            stopSpeed -= a * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        IsRolling = false;
    }

}
