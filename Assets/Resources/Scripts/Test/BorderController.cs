using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class BorderController : MonoBehaviour
{
    public int islandNumber;

    private RectTransform parent;
    private new BoxCollider2D collider;
    private void Start()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();

        Invoke("SetSize", 1f);
    }

    private void SetSize()
    {
        //print(parent.name + " ( " + parent.sizeDelta.x + " : " + parent.sizeDelta.y + " )");
        collider.size = new Vector2(parent.sizeDelta.x, parent.sizeDelta.y);
    }
}
