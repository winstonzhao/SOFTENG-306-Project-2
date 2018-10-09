﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using UnityEngine.UI;

// [RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(BoxCollider2D))]
public class DraggableScrollList : GenericDraggableList, IDropZone
{

    public List<Draggable> listItems = new List<Draggable>();
    public Canvas parentCanvas;
    public Scrollbar Scrollbar;

    public override IEnumerable<Draggable> ListItems { get { return listItems.AsReadOnly(); } }

    public bool copyOnDrag = true;
    public bool rearrangeable = true;

    public float maxHeight = 100;

    private Vector3 prevPos;

    private float itemHeight = 0.0f;

    private List<System.Type> allowedItems = new List<System.Type>();
    public List<System.Type> AllowedItems
    {
        get
        {
            return allowedItems;
        }
        set
        {
            allowedItems = value;
        }
    }

    public float layoutSpacing = 2;

    public Vector2 MinSize = new Vector2(1, 1);

    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;

    // Use this for initialization
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.isTrigger = true;

        Layout();

        foreach (var draggable in listItems)
        {
            draggable.transform.position = draggable.HomePos;
            draggable.SetDropZone(this);
        }
        prevPos = rectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Layout()
    {
        if (boxCollider == null) return;

        var scale = rectTransform.lossyScale.x;

        float i = itemHeight/2;
        float maxWidth = 0;

        foreach (var draggable in listItems)
        {
            if (draggable == null)
            {
                i += itemHeight + layoutSpacing;
                continue;
            }

            if (Math.Abs(itemHeight) < 0.01f)
            {
                itemHeight = draggable.Size.y;
                i = itemHeight / 2;
            }

            // Offset by .1f in the z so the child objects will handle mouse clicks before the list
            draggable.HomePos = new Vector3(transform.position.x, transform.position.y + i, transform.position.z - .1f);

            maxWidth = Mathf.Max(draggable.Size.x, maxWidth);
            i += draggable.Size.y + layoutSpacing;
            itemHeight = draggable.Size.y;
        }

        var width = Mathf.Max(MinSize.x, maxWidth);
        var height = Mathf.Max(MinSize.y, i - itemHeight / 2 - layoutSpacing);
        height = Mathf.Max(maxHeight * scale, height);
        var colliderSize = new Vector2(width, height) * 1 / scale;

        boxCollider.size = colliderSize;
        boxCollider.offset = new Vector2(0, colliderSize.y / 2);

        rectTransform.sizeDelta = colliderSize;
        rectTransform.anchoredPosition = new Vector2(0, 0);

        if (width > MinSize.x) transform.parent.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(colliderSize.x, transform.parent.parent.parent.GetComponent<RectTransform>().sizeDelta.y);
    }

    // Checks if this scroll list should still be the parent of the item
    private void UpdateParent(Draggable item)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!boxCollider.bounds.IntersectRay(ray))
        {
            item.GetComponent<RectTransform>().SetParent(parentCanvas.transform);
            item.transform.SetAsLastSibling();
        }
        else
        {
            item.GetComponent<RectTransform>().SetParent(transform.parent);
            item.transform.SetAsLastSibling();
        }
    }

    public override void Highlight(Draggable item)
    {
        var index = listItems.FindIndex(i => i == item);
        Scrollbar.value = (float)index / listItems.Count;
    }

    public void UpdateObject(Draggable item)
    {
        if (!rearrangeable) return;

        UpdateParent(item);

        var scrollbarHeight = maxHeight * transform.lossyScale.y;
        if (item.transform.position.y > Scrollbar.transform.position.y + scrollbarHeight/2)
        {
            Scrollbar.value += 0.1f;
        }
        else if (item.transform.position.y < Scrollbar.transform.position.y - scrollbarHeight/2)
        {
            Scrollbar.value -= 0.1f;
        }

        int targetIndex = FindIndex(item);
        targetIndex = Math.Min(targetIndex, listItems.Count - 1);

        listItems.Remove(item);
        listItems.Insert(targetIndex, item);
        Layout();

    }

    public bool AddToList(Draggable item, int index)
    {
        if (!rearrangeable)
        {
            Destroy(item.gameObject);
            return false;
        }
        item.GetComponent<RectTransform>().SetParent(transform.parent);
        listItems.Insert(index, item);
        Layout();
        return true;
    }

    public void RemoveFromList(Draggable item)
    {
        listItems.Remove(item);
        Layout();
    }

    public void AddDummyToList(int index)
    {
        if (!rearrangeable) return;
        listItems.Insert(index, null);
        Layout();
    }

    public void RemoveDummies()
    {
        listItems = listItems.Where(i => i != null).ToList();
        Layout();
    }

    private int FindIndex(Draggable item)
    {
        var maxIndex = -1;
        for (int i = 0; i < listItems.Count; i++)
        {
            if (listItems[i].transform.position.y < item.transform.position.y)
            {
                maxIndex = i;
            }
        }

        return maxIndex + 1;
    }

    public int IndexOf(Draggable item)
    {
        return listItems.IndexOf(item);
    }

    public void OnDragEnter(Draggable item)
    {
        if (listItems.Contains(item)) return;


        AddDummyToList(FindIndex(item));
    }

    public void OnDragExit(Draggable item)
    {
        RemoveDummies();
    }

    public void OnDrop(Draggable item)
    {
        RemoveDummies();
        AddToList(item, FindIndex(item));
    }

    public void OnItemDrag(Draggable item)
    {
        UpdateObject(item);
    }

    public void OnDragStart(Draggable item)
    {
        if (copyOnDrag)
        {
            var itemClone = Instantiate(item);
            listItems.Insert(listItems.IndexOf(item), itemClone);
            listItems.Remove(item);
            Layout();
            itemClone.SetDropZone(this);
        }
    }

    public void OnItemRemove(Draggable item)
    {
        listItems.Remove(item);
        Layout();
    }

    public void OnDragFinish(Draggable item)
    {
        if (copyOnDrag)
        {
            Destroy(item.gameObject);
        }
        else
        {
            if (item.transform.parent != transform.parent)
            {
                OnItemRemove(item);
                Destroy(item.gameObject);
            }

            Layout();
        }
    }

    public bool CanDrop(Draggable item)
    {
        if (allowedItems.Count == 0) return true;

        foreach (var allowedItem in allowedItems)
        {
            if (item.GetComponent(allowedItem)) return true;
        }
        return false;
    }
}
