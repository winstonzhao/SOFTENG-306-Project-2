using UnityEngine;
using UnityEditor;
using UltimateIsometricToolkit.physics;

/// <summary>
/// An implementation of IDropZone that can only contain a single item
/// </summary>
[RequireComponent(typeof(IsoCollider))]
public class SingleDropZone : MonoBehaviour, IDropZone
{

    private GameObject child;

    public string prefebName;

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

        createPrefeb();
    }

    void createPrefeb()
    {
        if (prefebName != "")
        {
            // instantiate
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefebName + ".prefab", typeof(GameObject));
            child = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            child.GetComponent<DraggableItem>().SetDropZone(this);
            child.GetComponent<DraggableItem>().HomePos = transform.position;
            Debug.Log(child.GetComponent<DraggableItem>().HomePos);
            if (child)
            {
                Debug.Log("new prefeb created");
            }

        }
    }

    void OnMouseEnter()
    {
        //Debug.Log("drop zone mouse entered");
    }

    public void OnDragEnter(Draggable item)
    {
        SetDropZoneActive(false);
        spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        SetDropZoneActive(true);
        Debug.Log("Dragged away");
    }

    public void OnDragFinish(Draggable item)
    {
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
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        spriteRenderer.sortingLayerName = "BackGround";
    }

    public void OnItemDrag(Draggable item)
    {

    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
        createPrefeb();
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
