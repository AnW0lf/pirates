using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CurrentItem : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public ShipInfo item;
    public int id;


    public void OnDrop(PointerEventData eventData)
    {
        if (DragHandler.itemBeingDragged)
        {
            CurrentItem other = DragHandler.itemBeingDragged.GetComponentInParent<CurrentItem>();
            if (item && other && other.item && item == other.item)
            {
                inventory.Merge(this, other);
            }
            else inventory.Switch(this, other);
        }
    }

}
