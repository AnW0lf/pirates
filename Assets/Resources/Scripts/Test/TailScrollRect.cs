using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TailScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public bool Scrolled { get; private set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Scrolled = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Scrolled = false;
    }
}
