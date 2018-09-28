using UnityEngine;

[RequireComponent(typeof(Collider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class SingleDropZone : MonoBehaviour, IDropZone
{
    private Draggable currentItem;

    public void Start()
    {
        
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        
    }
    void OnMouseEnter()
    {
        Debug.Log("drop zone mouse entered");
    }

    public void OnDragEnter(Draggable item)
    {
        SetDropZoneActive(false);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        SetDropZoneActive(true);
    }

    public void OnDragFinish(Draggable item)
    {
        if (!item.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
        {
            // disable if we need the snapback effect
            //item.GetComponent<DraggableItem>().SetDropZone(null);
        }
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
    /*
     * Enable or disable the drop zone by changing whether or not it can be hit by a ray cast
     */
    private void SetDropZoneActive(bool enable)
    {
        gameObject.layer = enable ? 1 : 2;
    }
}
