using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class DraggableList : GenericDraggableList, IDropZone
{

    public List<Draggable> listItems = new List<Draggable>();

    public override IEnumerable<Draggable> ListItems { get { return listItems.AsReadOnly(); } }

    [SerializeField]
    private bool copyOnDrag = true;

    [SerializeField]
    private bool rearrangeable = true;

    private float itemHeight = 0f;

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

    public bool Rearrangeable
    {
        get
        {
            return rearrangeable;
        }
        set
        {
            rearrangeable = value;
        }
    }

    public bool CopyOnDrag
    {
        get
        {
            return copyOnDrag;
        }
        set
        {
            copyOnDrag = value;
        }
    }

    public float layoutSpacing = 2;

    public Vector2 MinSize = new Vector2(1, 1);

    private BoxCollider2D boxCollider;

    // Use this for initialization
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.isTrigger = true;

        layout();

        foreach (var draggable in listItems)
        {
            draggable.transform.position = draggable.HomePos;
            draggable.SetDropZone(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

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

            if (itemHeight == 0)
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
        var height = Mathf.Max(MinSize.y, i - itemHeight);
        var colliderSize = new Vector2(width, height);

        boxCollider.size = colliderSize;
        boxCollider.offset = new Vector2(0, colliderSize.y / 2);
    }

    public void UpdateObject(Draggable item)
    {
        if (!Rearrangeable) return;

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
        if (CopyOnDrag)
        {
            var itemClone = Instantiate(item);
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
