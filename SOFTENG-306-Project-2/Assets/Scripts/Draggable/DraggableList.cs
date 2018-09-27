using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;

public class DraggableList : MonoBehaviour, IDropZone
{

    public List<Draggable> listItems = new List<Draggable>();

    public SquareResizer mainObject;

    public List<string> strings;

    [SerializeField]
    private bool copyOnDrag = true;

    [SerializeField]
    private bool rearrangeable = true;

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
    }

    public int layoutSpacing = 2;

    private BoxCollider2D boxCollider;

    // Use this for initialization
    void Start()
    {
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        var rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.isTrigger = true;

        foreach (var text in strings)
        {
            var newSprite = Instantiate(mainObject);
            var draggable = newSprite.gameObject.AddComponent<DraggableItem>();
            listItems.Add(newSprite.GetComponent<Draggable>());

            newSprite.GetComponentInChildren<TextMesh>().text = text;
            newSprite.UpdateSize();

            draggable.SetDropZone(this);
        }
        layout();

        foreach(var draggable in listItems) draggable.transform.position = draggable.HomePos;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void layout()
    {
        int i = -layoutSpacing;
        float maxWidth = 0;

        foreach (var draggable in listItems)
        {
            i += layoutSpacing;

            if (draggable == null) continue;

            // Offset by .1f in the z so the child objects will handle mouse clicks before the list
            draggable.HomePos = new Vector3(transform.position.x, transform.position.y + i, transform.position.z - .1f);

            maxWidth = Mathf.Max(draggable.GetComponent<SquareResizer>().Width, maxWidth);
        }

        var colliderSize = new Vector2(maxWidth, i + 1);

        boxCollider.size = colliderSize;
        boxCollider.offset = new Vector2(0, (i) / 2);
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
            layout();
        }
    }
}
