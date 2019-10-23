using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrashBucketController : MonoBehaviour
{
    public Color solid = Color.white, translucent = new Color(1f, 1f, 1f, 0.52f);
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (DragHandler.itemBeingDragged)
        {
            if (img.color != solid) img.color = solid;
        }
        else
        {
            if (img.color != translucent) img.color = translucent;
        }
    }
}
