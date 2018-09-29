using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SingleDropZoneReturn : MonoBehaviour, IDropZone
{
    public Sprite newSprite;
    private Draggable currentItem;
    public Canvas endOfLevelCanvas;

    public void Start()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) 
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void OnDragEnter(Draggable item)
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
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
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        GameObject.FindWithTag("Light").GetComponent<SpriteRenderer>().sprite = newSprite;

        GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag("Circuit");

        foreach (GameObject circ in circuitPieces) {
            circ.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        

        StartCoroutine(endOfLevel((myReturnValue) => {
            if (myReturnValue) { endOfLevelCanvas.enabled = true; }
        }));

    }

    private IEnumerator endOfLevel(System.Action<bool> callback)
    {
        yield return new WaitForSeconds(2);
        callback(true);
       
 
    }

    public void OnItemDrag(Draggable item)
    {
        
    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
    }
}