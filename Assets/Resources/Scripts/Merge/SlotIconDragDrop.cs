using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotIconDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 pos, mousePos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        pos = transform.position;
        transform.localPosition += Vector3.back;
        mousePos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = pos + (Input.mousePosition - mousePos) / 243.6f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        print(eventData.pointerEnter.GetComponent<SlotIconDragDrop>() != null);
        //print(eventData.pointerEnter.GetComponent<MergeMenuSlot>().SlotNumber);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }
}
