using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f, maxYPosIslands = 63f, maxYPosMap = 40f;
    [SerializeField]
    [Range(0f, 1f)]
    private float inertia = 0.1f;
    [SerializeField]
    private Transform cameraTransform, mask;
    [SerializeField]
    private Vector2 islandsPos, mapPos;

    private float velocity = 0f;
    private bool dragged = false, swapped = false;
    private Place place = Place.ISLANDS;
    private Vector3 islandsPosition, mapPosition;

    private void Awake()
    {
        islandsPosition = new Vector3(islandsPos.x, islandsPos.y, cameraTransform.position.z);
        mapPosition = new Vector3(mapPos.x, mapPos.y, cameraTransform.position.z);
    }

    private void OnMouseDown()
    {
        dragged = true;
    }

    private void OnMouseDrag()
    {
        velocity = -Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 pos = cameraTransform.position;
        pos.y += velocity;

        if (place == Place.ISLANDS)
            pos.y = Mathf.Clamp(pos.y, 0, maxYPosIslands);
        else
            pos.y = Mathf.Clamp(pos.y, 0, maxYPosMap);

        cameraTransform.position = pos;

    }

    private void OnMouseUp()
    {
        dragged = false;
    }

    private void Update()
    {
        if (dragged) return;

        Vector3 pos = cameraTransform.position;
        pos.y += velocity;

        if (place == Place.ISLANDS)
            pos.y = Mathf.Clamp(pos.y, 0, maxYPosIslands);
        else
            pos.y = Mathf.Clamp(pos.y, 0, maxYPosMap);

        cameraTransform.position = pos;

        velocity *= inertia;
    }

    public void IslandsMapSwap()
    {
        if (swapped) return;

        swapped = true;

        if (place == Place.ISLANDS)
        {
            place = Place.MAP;
            islandsPosition = cameraTransform.position;
            mask.LeanScale(Vector3.zero, 0.5f);
            LeanTween.delayedCall(0.6f, () => cameraTransform.position = mapPosition);
            LeanTween.delayedCall(0.6f, () => mask.LeanScale(Vector3.one * 15f, 0.5f));
            LeanTween.delayedCall(1.2f, () => swapped = false);
        }
        else
        {
            place = Place.ISLANDS;
            mapPosition = cameraTransform.position;
            mask.LeanScale(Vector3.zero, 0.5f);
            LeanTween.delayedCall(0.6f, () => cameraTransform.position = islandsPosition);
            LeanTween.delayedCall(0.6f, () => mask.LeanScale(Vector3.one * 15f, 0.5f));
            LeanTween.delayedCall(1.2f, () => swapped = false);
        }
    }

    enum Place { ISLANDS, MAP }
}
