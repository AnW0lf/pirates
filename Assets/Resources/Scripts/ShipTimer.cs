using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTimer : MonoBehaviour
{
    public Transform pointer, arrow, timer;
    public Image clock, icon;

    private bool isTimerActive;
    private Vector3 startPos, startAngles;

    private void Awake()
    {
        startPos = pointer.localPosition;
        startAngles = arrow.localEulerAngles;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && !isTimerActive)
        {
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        isTimerActive = true;
        pointer.gameObject.SetActive(true);
        icon.sprite = GetComponent<Image>().sprite;
        pointer.SetParent(transform.parent, true);
        timer.eulerAngles = Vector3.zero;
        float time = transform.parent.GetComponentInParent<Ship>().GetRaidTime();
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        for (float i = 0f; i < time; i += Time.fixedDeltaTime)
        {
            clock.fillAmount = 1f - i / time;
            yield return wait;
        }
        pointer.SetParent(transform);
        pointer.localPosition = startPos;
        timer.localEulerAngles = startAngles;
        pointer.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        isTimerActive = false;
    }
}
