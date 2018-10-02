using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;

// [RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(BoxCollider2D))]

public class DraggableScrollList : MonoBehaviour, IDropZone
{

    public List<Draggable> listItems = new List<Draggable>();

    public IEnumerable<Draggable> ListItems { get { return listItems.AsReadOnly(); } }

    public bool copyOnDrag = true;
    public bool rearrangeable = true;

    public int maxHeight = 100;

    private Vector3 prevPos;

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

    // Use this for initialization
    void Start()
    {
        boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
        var rigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.isTrigger = true;

        layout();

        foreach (var draggable in listItems)
        {
            draggable.transform.position = draggable.HomePos;
            draggable.SetDropZone(this);
        }
        prevPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var rectTransform = GetComponent<RectTransform>();
        // if (rectTransform.localPosition != prevPos)
        // {
        //     layout();
        // }

        prevPos = rectTransform.localPosition;
    }

    void layout()
    {
        float i = -layoutSpacing;
        float maxWidth = 0;

        var itemHeight = 0.0f;

        foreach (var draggable in listItems)
        {
            i += layoutSpacing;

            if (draggable == null) continue;

            // Offset by .1f in the z so the child objects will handle mouse clicks before the list
            draggable.HomePos = new Vector3(transform.position.x, transform.position.y + i, transform.position.z - .1f);

            maxWidth = Mathf.Max(draggable.Size.x, maxWidth);
            itemHeight = draggable.Size.y;
        }

        var width = Mathf.Max(MinSize.x, maxWidth);
        var height = Mathf.Max(MinSize.y, i + itemHeight);
        var colliderSize = new Vector2(width, height) * 1/GetComponent<RectTransform>().lossyScale.x;

        var parentHeight = Mathf.Min(height * 1/GetComponent<RectTransform>().lossyScale.x, maxHeight);

        boxCollider.size = colliderSize;
        boxCollider.offset = new Vector2(0, (height - itemHeight) / 2) * 1/GetComponent<RectTransform>().lossyScale.x;
        GetComponent<RectTransform>().sizeDelta = colliderSize;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -parentHeight / 2 + (itemHeight / 2 * 1/GetComponent<RectTransform>().lossyScale.x));

        var parentTransform = transform.parent.parent.GetComponent<RectTransform>();
        parentTransform.sizeDelta = new Vector2(colliderSize.x, parentHeight);
        parentTransform.anchoredPosition = new Vector3(0, parentHeight/2, 0);
    }

    public void UpdateObject(Draggable item)
    {
        if (!rearrangeable) return;

        item.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform);

        int i = listItems.IndexOf(item);
        int indexDiff = 0;

        if (i > 0 && item.transform.position.y < listItems[i - 1].transform.position.y)
        {
            indexDiff = -1;
        }
        else if (i < listItems.Count - 1 && item.transform.position.y > listItems[i + 1].transform.position.y)
        {
            indexDiff = 1;
        }

        if (indexDiff != 0)
        {
            listItems[i] = listItems[i + indexDiff];
            listItems[i + indexDiff] = item;

            var oldHome = item.HomePos;
            var newHome = listItems[i].HomePos;
            item.HomePos = newHome;
            listItems[i].HomePos = oldHome;
        }

    }

    public bool AddToList(Draggable item, int index)
    {
        if (!rearrangeable)
        {
            Destroy(item.gameObject);
            return false;
        }

        listItems.Insert(index, item);
        item.GetComponent<RectTransform>().SetParent(transform.parent);
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
            var itemClone = Instantiate(item);
            listItems.Insert(listItems.IndexOf(item), itemClone);
            listItems.Remove(item);
            layout();
            itemClone.GetComponent<DraggableItem>().SetDropZone(this);
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
            item.GetComponent<RectTransform>().SetParent(transform.parent);
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
