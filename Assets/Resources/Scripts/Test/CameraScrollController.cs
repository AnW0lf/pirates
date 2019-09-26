using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScrollController : MonoBehaviour
{
    [SerializeField]
    private Transform content = null;
    [SerializeField]
    private CanvasScaler canvas = null;

    private Camera mainCamera;
    private Transform cameraTransform;
    private float k;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        if (content == null) Debug.LogWarning("Content is not defined");
        if (canvas == null) Debug.LogWarning("Canvas is not defined");
    }

    private void Start()
    {
        k = (float)canvas.referenceResolution.y / (mainCamera.orthographicSize * 2f);
    }

    private void Update()
    {
        float newY = -(content.localPosition.y / k);
        cameraTransform.position = new Vector3(cameraTransform.position.x, newY, cameraTransform.position.z);
    }

    private void FixedUpdate()
    {
        float newY = -(content.localPosition.y / k);
        cameraTransform.position = new Vector3(cameraTransform.position.x, newY, cameraTransform.position.z);
    }

    private void LateUpdate()
    {
        float newY = -(content.localPosition.y / k);
        cameraTransform.position = new Vector3(cameraTransform.position.x, newY, cameraTransform.position.z);
    }
}
