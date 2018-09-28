using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SingleDropZone : MonoBehaviour, IDropZone
{
    private Draggable currentItem;

    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) 
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void OnDragEnter(Draggable item)
    {
        spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }

    public void OnDragFinish(Draggable item)
    {
        if (!item.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
        {
            item.GetComponent<DraggableItem>().SetDropZone(null);
        }
    }

    public void OnDragStart(Draggable item)
    {
        
    }

    public void OnDrop(Draggable item)
    {
        currentItem = item;
        currentItem.HomePos = transform.position;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }

    public void OnItemDrag(Draggable item)
    {
        
    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
    }
}