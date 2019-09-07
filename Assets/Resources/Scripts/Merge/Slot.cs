using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Slot : MonoBehaviour, IDropHandler
{
    public ShipInfo itemInfo;
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            else return null;
        }
    }

    public InventorySlot inventorySlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (!item && inventorySlot.State != SlotState.LOCKED)
        {
            DragHandler.itemBeingDragged.transform.parent.GetComponent<Slot>().itemInfo = null;
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            itemInfo = DragHandler.itemBeingDragged.GetComponent<DragHandler>().itemInfo;
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
