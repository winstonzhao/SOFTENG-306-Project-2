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
        //GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
       // GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
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
        Debug.LogWarning("in here");
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
            Debug.LogWarning("Got into the expected!!");
            //GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
            //GameObject.FindWithTag("Light").GetComponent<SpriteRenderer>().sprite = newSprite;

            GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag("Circuit_2");

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
