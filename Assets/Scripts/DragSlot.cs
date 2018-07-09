using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IDropHandler
{
    public event EventHandler OnItemDrop;
    public event EventHandler OnItemRemoved;

    [SerializeField]
    private ItemTypes AllowedItems = ItemTypes.Any;

    private GameObject previousItem;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            if (AllowedItems != ItemTypes.Any)
            {
                var itemDetails = DragHandler.itemBeingDragged.GetComponent<ItemDetails>();
                if (itemDetails.ItemType != AllowedItems)
                {
                    return;
                }
            }

            DragHandler.itemBeingDragged.transform.SetParent(transform);
            previousItem = DragHandler.itemBeingDragged;

            if (OnItemDrop != null)
            {
                OnItemDrop(gameObject, null);
            }
        
            item.GetComponent<DragHandler>().OnParentChange += DragSlot_OnParentChange;
        }
    }

    private void DragSlot_OnParentChange(object sender, EventArgs e)
    {
        if (item == null)
        {
            previousItem.GetComponent<DragHandler>().OnParentChange -= DragSlot_OnParentChange;
            previousItem = null;

            if (OnItemRemoved != null)
            {
                OnItemRemoved(gameObject, new EventArgs());
            }
        }
    }
}
