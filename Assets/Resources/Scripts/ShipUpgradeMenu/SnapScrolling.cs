using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range(0f, 20f)]
    [Header("Controllers")]
    public float snapSpeed;
    [Header("Other Objects")]
    public ScrollRect scrollRect;

    public GameObject[] pans;
    private float[] pansPos;

    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanID;
    private bool isScrolling, isSet;

    private void Start()
    {
        contentRect = GetComponent<RectTransform>();
        pansPos = new float[pans.Length];
        for (int i = 0; i < pans.Length; i++)
        {
            pansPos[i] = -pans[i].transform.localPosition.x;
        }
    }

    private void FixedUpdate()
    {
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (contentRect.anchoredPosition.x >= pansPos[0] && !isScrolling
            || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1] && !isScrolling
            || scrollVelocity < 400 && !isScrolling)
        {
            scrollRect.inertia = false;
            if (!isSet)
                SetPan();
        }
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID], snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }

    public void SetPan()
    {
        float nearestPos = float.MaxValue;
        for (int i = 0; i < pans.Length; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i]);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
        }
    }

    public void SetPan(int id)
    {
        isSet = true;
        selectedPanID = id;
    }

    public void Unset()
    {
        isSet = false;
    }
}