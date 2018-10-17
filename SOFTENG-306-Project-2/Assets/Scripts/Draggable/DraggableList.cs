using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DraggableList : GenericDraggableList, IDropZone
{
    private float itemHeight = 0f;

    public Vector2 MinSize = new Vector2(1, 1);

    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.isTrigger = true;

        layout();

        foreach (var draggable in listItems)
        {
            draggable.SetDropZone(this);
        }
        layout();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Moves all the items in the list to their appropriate locations
    /// </summary>
    void layout()
    {
        float i = itemHeight/2;
        float maxWidth = 0;

        foreach (var draggable in listItems)
        {
            if (draggable == null)
            {
                i += itemHeight + layoutSpacing;
                continue;
            }

            // Calculate the item height from the first item
            if (Math.Abs(itemHeight) < 0.01f)
            {
                itemHeight = draggable.Size.y;
                i = itemHeight / 2;
            }

            // Offset by .1f in the z so the child objects will handle mouse clicks before the list
            draggable.HomePos = new Vector3(transform.localPosition.x, transform.localPosition.y + i, transform.position.z - .1f);

            maxWidth = Mathf.Max(draggable.Size.x, maxWidth);
            i += draggable.Size.y + layoutSpacing;
            itemHeight = draggable.Size.y;
        }

        var width = Mathf.Max(MinSize.x, maxWidth);
        var height = Mathf.Max(MinSize.y, i - itemHeight / 2 - layoutSpacing);
        var colliderSize = new Vector2(width, height);

        // Move collider to fit entire list
        boxCollider.size = colliderSize;
        boxCollider.offset = new Vector2(0, colliderSize.y / 2);
    }

    public void UpdateObject(Draggable item)
    {
        if (!rearrangeable) return;

        // Move to correct position
        int targetIndex = FindIndex(item);
        targetIndex = Math.Min(targetIndex, listItems.Count - 1);

        listItems.Remove(item);
        listItems.Insert(targetIndex, item);
        layout();

    }

    public bool AddToList(Draggable item, int index)
    {
        if (!rearrangeable)
        {
            // Cannot have items added
            Destroy(item.gameObject);
            return false;
        }

        listItems.Insert(index, item);
        layout();
        return true;
    }

    public void RemoveFromList(Draggable item)
    {
        listItems.Remove(item);
        layout();
    }

    public void AddDummyToList(int index)
    {
        if (!rearrangeable) return;
        listItems.Insert(index, null);
        layout();
    }

    public void RemoveDummies()
    {
        listItems = listItems.Where(i => i != null).ToList();
        layout();
    }

    public int IndexOf(Draggable item)
    {
        return listItems.IndexOf(item);
    }

    public void OnDragEnter(Draggable item)
    {
        if (listItems.Contains(item)) return;

        AddDummyToList(0);
    }

    public void OnDragExit(Draggable item)
    {
        RemoveDummies();
    }

    public void OnDrop(Draggable item)
    {
        RemoveDummies();
        AddToList(item, 0);
    }

    public void OnItemDrag(Draggable item)
    {
        UpdateObject(item);
    }

    public void OnDragStart(Draggable item)
    {
        if (copyOnDrag)
        {
            // Create a clone to stay in the list and allow the old item to be dragged away
            var itemClone = Instantiate(item, item.transform.parent);
            item.transform.SetAsLastSibling();
            itemClone.transform.SetAsLastSibling();

            listItems.Insert(listItems.IndexOf(item), itemClone);
            listItems.Remove(item);

            layout();
            itemClone.GetComponent<Draggable>().SetDropZone(this);
        }
    }

    public void OnItemRemove(Draggable item)
    {
        listItems.Remove(item);
        layout();
    }

    public void OnDragFinish(Draggable item)
    {
        if (copyOnDrag)
        {
            Destroy(item.gameObject);
        }
        else
        {
            layout();
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
