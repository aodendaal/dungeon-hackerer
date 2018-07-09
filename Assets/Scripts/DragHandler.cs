using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event EventHandler OnItemDrop;
    public event EventHandler OnParentChange;

    public static GameObject itemBeingDragged;
    private Vector3 startPosition;
    private Transform startParent;
    private Transform panelParent;

    public void Start()
    {
        panelParent = GameObject.Find("InventoryPanel").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
       
        startParent = transform.parent;

        transform.SetParent(panelParent);
        transform.SetAsLastSibling();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == panelParent)
        {
            transform.SetParent(startParent);
        }

        transform.position = transform.parent.position;

        if (transform.parent != startParent)
        {
            if (OnParentChange != null)
            {
                OnParentChange(gameObject, new EventArgs());
            }
        }

        if (OnItemDrop != null)
        {
            OnItemDrop(gameObject , new EventArgs());
        }
    }
}
