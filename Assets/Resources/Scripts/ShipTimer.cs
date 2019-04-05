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
    private Ship ship;

    private void Awake()
    {
        startPos = pointer.localPosition;
        startAngles = arrow.localEulerAngles;
        ship = transform.parent.GetComponentInParent<Ship>();
    }

    private void Start()
    {
        pointer.localScale = new Vector3(pointer.localScale.x, pointer.localScale.y * Mathf.Sign(transform.localScale.x), pointer.localScale.z);
        icon.transform.localEulerAngles = new Vector3(0f, 0f, icon.transform.localEulerAngles.z * Mathf.Sign(transform.localScale.x));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && !isTimerActive)
        {
            transform.GetComponentInParent<CapsuleCollider2D>().enabled = false;
            if (collision.gameObject.name.Equals("RightBorder") || collision.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime() + 1.5f));
            else
                StartCoroutine(Timer(ship.GetRaidTime()));
        }
    }

    private IEnumerator Timer(float time)
    {
        isTimerActive = true;
        pointer.gameObject.SetActive(true);
        icon.sprite = GetComponent<Image>().sprite;
        pointer.SetParent(transform.parent, true);
        timer.eulerAngles = Vector3.zero;
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
        pointer.localScale = new Vector3(pointer.localScale.x, pointer.localScale.y * Mathf.Sign(transform.localScale.x), pointer.localScale.z);
        icon.transform.localEulerAngles = new Vector3(0f, 0f, icon.transform.localEulerAngles.z * Mathf.Sign(transform.localScale.x));
        isTimerActive = false;
    }
}
