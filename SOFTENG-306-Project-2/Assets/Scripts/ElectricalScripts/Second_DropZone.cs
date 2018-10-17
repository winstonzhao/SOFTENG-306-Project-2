using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Second_DropZone : MonoBehaviour, IDropZone
{

    private Draggable currentItem;
    public Draggable[] expectedGates;
    public bool expected { get; private set; }
    public string circuitTag;


    public void Start()
    {
        expected = false;
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

    }

    public bool GetExpected()
    {
        return expected;
    }

    public void OnDragEnter(Draggable item)
    {
        
    }

    public void OnDragExit(Draggable item)
    {
       
    }

    public void OnDragFinish(Draggable item)
    {
        if (!item.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
        {
            item.GetComponent<DraggableItemReturn>().SetDropZone(null);
        }
    }

    public void OnDragStart(Draggable item)
    {

    }

    public void OnDrop(Draggable item)
    {
        currentItem = item;
        currentItem.HomePos = transform.position;

        foreach (Draggable gate in expectedGates)
        {
            if (currentItem == gate)
            {
                expected = true;
            }
        }

        if (expected)
        {
            GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag(circuitTag);

            foreach (GameObject circ in circuitPieces)
            {
                circ.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

    }

    public void OnItemDrag(Draggable item)
    {

    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
    }

    public bool CanDrop(Draggable item)
    {
        return true;
    }

}
