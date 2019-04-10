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
    private Vector3 startPos;
    private Ship ship;

    private void Awake()
    {
        ship = transform.parent.GetComponentInParent<Ship>();
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
        pointer.position = transform.position;
        pointer.eulerAngles = transform.eulerAngles;
        for (float i = 0f; i < time; i += Time.deltaTime)
        {
            clock.fillAmount = i / time;
            yield return null;
        }
        pointer.gameObject.SetActive(false);
        isTimerActive = false;
    }
}
