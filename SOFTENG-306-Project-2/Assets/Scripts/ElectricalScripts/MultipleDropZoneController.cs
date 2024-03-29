﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controller for multiple dropzones in an electrical minigame level
 */
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MultipleDropZoneController : MonoBehaviour, IDropZone
{

    private Draggable currentItem;
    public Draggable[] expectedGates;
    public bool expected { get; private set; }
    public string circuitTag;

    /*
     * Get the rigidbodies in the level on start
     */
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

    /*
     * Return the expected input
     */
    public bool GetExpected()
    {
        return expected;
    }

    public void OnDragEnter(Draggable item)
    {
        
    }

    /*
     * When the item is dragged out of the dropzone reset the changed components
     */ 
    public void OnDragExit(Draggable item)
    {
        // if there is not draggable item
        if (currentItem == null)
        {
            // set expected to false as the expected component is not in the dropzone
            expected = false;

            //set the circuit pieces back to grey
            GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag(circuitTag);
            foreach (GameObject circ in circuitPieces)
            {
                circ.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }

    }

    /*
     * When player finishes dragging an object, check if it is touching any
     * collider objects
     */
    public void OnDragFinish(Draggable item)
    {
        if (!item.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
        {
            item.GetComponent<DraggableItemReturn>().SetDropZone(null);
        }
    }

    /*
     * When the player starts to drag the item set the current item in the drop zone to null
     */ 
    public void OnDragStart(Draggable item)
    {
        currentItem = null;
    }

    /*
     * When player lets a logic gate go, check if the logic gate 
     * is within a dropzone area
     */
    public void OnDrop(Draggable item)
    {
        currentItem = item;
        currentItem.HomePos = transform.position;

        // check for each logic gate
        foreach (Draggable gate in expectedGates)
        {
            // check if a logic gate is expected in dropzone
            if (currentItem == gate)
            {
                expected = true;
            }
        }

        // if logic gate is expected, turn the corresponding circuit pieces yellow
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

    /*
     * Set item in the dropzone as null if a logic gate is removed
     */
    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
        expected = false;
    }

    public bool CanDrop(Draggable item)
    {
        return true;
    }

}
