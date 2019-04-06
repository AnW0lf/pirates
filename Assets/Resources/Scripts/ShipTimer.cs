using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTimer : MonoBehaviour
{
    public Transform pointer, arrow;
    public Image clock;
    public Color color;

    private bool isTimerActive;
    private Vector3 startPos, startScale;
    private Ship ship;

    private Vector3 scale = new Vector3(0.006f, 0.006f, 1f);

    private void Awake()
    {
        startPos = pointer.localPosition;
        startScale = pointer.localScale;
        ship = transform.parent.GetComponentInParent<Ship>();
        pointer.localScale = scale;
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
        arrow.GetComponent<Image>().color = color;
        pointer.gameObject.SetActive(true);
        pointer.SetParent(transform.parent, true);
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        for (float i = 0f; i < time; i += Time.fixedDeltaTime)
        {
            clock.fillAmount = i / time;
            yield return wait;
        }
        pointer.SetParent(transform);
        pointer.localPosition = startPos;
        pointer.localScale = startScale;
        pointer.gameObject.SetActive(false);
        isTimerActive = false;
        pointer.localScale = Vector3.right * pointer.localScale.x + Vector3.up * pointer.localScale.y * Mathf.Sign(transform.localScale.x) + Vector3.forward * pointer.localScale.z;
    }
}
