using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAutosize : MonoBehaviour
{
    public RectTransform canvas;

    private void Awake()
    {
        Invoke("Autosize", 0.1f);
    }

    private void Autosize()
    {
        //print("Canvas width : " + canvas.sizeDelta.x);
        HorizontalLayoutGroup group = GetComponent<HorizontalLayoutGroup>();
        float panelWidth = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        float screenWidth = canvas.sizeDelta.x;
        float sideWidth = (screenWidth - panelWidth) / 2f;
        group.spacing = screenWidth - panelWidth;
        group.padding.left = (int)sideWidth;
        group.padding.right = (int)sideWidth;
    }
}
