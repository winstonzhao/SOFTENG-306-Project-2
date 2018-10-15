using UnityEngine;
using UnityEditor;
using UltimateIsometricToolkit.physics;
using System.Collections.Generic;
using System;

//[RequireComponent(typeof(IsoCollider))]
//[RequireComponent(typeof(Rigidbody2D))]
public class IsoDropZone : MonoBehaviour, IDropZone
{

    private GameObject child;

    public string prefebName;
    public List<string> droppableNames;
    public int ItemPrice = 0;
    public bool Returner = false;

    private Draggable currentItem;

    private bool isEnabled = true;

    public void Start()
    {

        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        createPrefeb();
    }

    /** 
    * Create a new tile prefab to be placed on top of the factory for road building 
    */
    void createPrefeb()
    {
        if (prefebName != "")
        {
            // instantiate
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefebName);
            child = Instantiate<GameObject>(prefab);
            child.GetComponent<DraggableIsoItem>().SetDropZone(this);
            child.GetComponent<DraggableIsoItem>().homePos = transform.position;
            Debug.Log(child.GetComponent<DraggableIsoItem>().homePos);
            if (child)
            {
                Debug.Log("new " + prefebName + " created");
            }


        }
    }

    void OnMouseEnter()
    {
        //Debug.Log("drop zone mouse entered");
    }

    void FixedUpdate()
    {
        //if (prefebName != null && !CivilLevelController.instance.IsBudgetAvailable(ItemPrice))
        if (prefebName != null && !GameObject.Find("CivilLevelController").GetComponent<CivilLevelController>().IsBudgetAvailable(ItemPrice))
        {

            //Debug.Log("Budget not available for " + prefebName);
        }
    }

    public void OnDragEnter(Draggable item)
    {
        if(isEnabled)
        {
            //SetDropZoneActive(false);
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        }

    }

    public void OnDragExit(Draggable item)
    {
        if(isEnabled) {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
            //SetDropZoneActive(true);
            Debug.Log("Dragged away");
        }


    }

    public void OnDragFinish(Draggable item)
    {
        //if (!item.GetComponent<IsoCollider>().GetComponent<Collider>().IsTouching(GetComponent<IsoCollider>()))
        //{
        //    // disable if we need the snapback effect
        //    //item.GetComponent<DraggableItem>().SetDropZone(null);
        //}
        SetDropZoneActive(false);

        GetComponent<SpriteRenderer>().sortingLayerName = "BackGround";
    }

    public void OnDragStart(Draggable item)
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
    }

    public void OnDrop(Draggable item)
    {
        currentItem = item;
        currentItem.HomePos = transform.position;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        GetComponent<SpriteRenderer>().sortingLayerName = "BackGround";

        // update budget if this drop zone is a factory
        if (prefebName != "" || Returner)
        {
            //CivilLevelController.instance.UpdateBudget(ItemPrice);
            GameObject.Find("CivilLevelController").GetComponent<CivilLevelController>().UpdateBudget(((DraggableIsoItem)item).Price);
            // delete the returned block to prevent duplicated building blocks been instantiated
            item.gameObject.SetActive(false);
        }
        else // otherwise disable the ground tile
        {
            gameObject.SetActive(false);
        }
    }

    public void OnItemDrag(Draggable item)
    {

    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
        createPrefeb();
        gameObject.SetActive(true);
        SetDropZoneActive(true);

        // update budget
        //CivilLevelController.instance.UpdateBudget(-ItemPrice);
        GameObject.Find("CivilLevelController").GetComponent<CivilLevelController>().UpdateBudget(-ItemPrice);
    }

    public bool CanDrop(Draggable item)
    {
        return true;
    }

    /*
     * Enable or disable the drop zone by changing whether or not it can be hit by a ray cast
     */
    private void SetDropZoneActive(bool enable)
    {
        gameObject.layer = enable ? 1 : 2;
    }

    /** 
     * Display the tile factories as enable or disable depending on budget availability 
     */
    public void setEnable(bool enable)
    {
        if (enable == isEnabled) return;
        isEnabled = enable;

        if (!enable)
        {
            child.SetActive(false);
            GetComponent<SpriteRenderer>().color = new Color(0.49f, 0.49f, 0.49f);
        }
        else
        {
            child.SetActive(true);
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        Debug.Log(enable ? prefebName + " enabled" : prefebName + " disabled");
    }
}
