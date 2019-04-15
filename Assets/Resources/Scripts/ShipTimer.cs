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
    private Camera cam;

    private void Awake()
    {
        ship = transform.parent.GetComponentInParent<Ship>();
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && !isTimerActive)
        {
            transform.GetComponentInParent<CapsuleCollider2D>().enabled = false;
            if (collision.gameObject.name.Equals("RightBorder") || collision.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime() + 1.5f, true));
            else
                StartCoroutine(Timer(ship.GetRaidTime(), false));
        }
    }

    private IEnumerator Timer(float time, bool isSide)
    {
        isTimerActive = true;
        arrow.GetComponent<Image>().color = color;
        pointer.gameObject.SetActive(true);
        float height = 2f * cam.orthographicSize, width = height * cam.aspect, xPos = transform.position.x, yPos = transform.position.y;
        Vector3 pointerPos = new Vector3(isSide ? (xPos > 0f ? width / 2f : -width / 2f): xPos,
            isSide ? yPos : (yPos > 0f ? height / 2f - 0.5f : -height / 2f + 1.7f), transform.position.z);
        Debug.Log(pointerPos);
        pointer.position = pointerPos;
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
