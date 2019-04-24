using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTimer : MonoBehaviour
{
    public Transform pointer, arrow;
    public Image clock;
    public Color color;
    public TrailRenderer trail;

    private bool isTimerActive;
    private string borderName;
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
            borderName = collision.gameObject.name;
            Invoke("SwitchEmmiting", 0.15f);
            if (collision.gameObject.name.Equals("RightBorder") || collision.gameObject.name.Equals("LeftBorder"))
                StartCoroutine(Timer(ship.GetRaidTime() + 1.5f, true));
            else
                StartCoroutine(Timer(ship.GetRaidTime(), false));
        }
        else if (collision.CompareTag("Border") && isTimerActive && borderName == collision.gameObject.name)
        {
            borderName = "";
            Invoke("SwitchEmmiting", 0.4f);
            isTimerActive = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
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
        pointer.position = pointerPos;
        pointer.eulerAngles = transform.eulerAngles;
        for (float i = 0f; i < time; i += Time.deltaTime)
        {
            clock.fillAmount = i / time;
            yield return null;
        }
        pointer.gameObject.SetActive(false);
    }

    private void SwitchEmmiting()
    {
        trail.emitting = !trail.emitting;
    }
}
