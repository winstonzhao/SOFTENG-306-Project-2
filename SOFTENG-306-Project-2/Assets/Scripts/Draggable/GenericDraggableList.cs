using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic class to encapsulate a list of Draggable Items
/// </summary>
public abstract class GenericDraggableList : MonoBehaviour
{
    public bool copyOnDrag = true;
    public bool rearrangeable = true;

    // Default spacing between objects in the list
    public float layoutSpacing = 2;

    public List<Draggable> listItems = new List<Draggable>();

    protected List<System.Type> allowedItems = new List<System.Type>();
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


    /// <summary>
    /// All draggable items in this list
    /// </summary>
    public IEnumerable<Draggable> ListItems { get { return listItems.AsReadOnly(); } }

    /// <summary>
    /// To be used when then given item should be highlighted in some way
    /// (how to highlight is to left up to the implementing classes)
    /// </summary>
    public virtual void Highlight(Draggable item)
    {

    }

    // Finds the index the given item should be in the list based on it's y position
    protected int FindIndex(Draggable item)
    {
        var maxIndex = -1;
        for (int i = 0; i < listItems.Count; i++)
        {
            if (listItems[i].transform.position.y < item.transform.position.y)
            {
                maxIndex = i;
            }
        }

        return maxIndex + 1;
    }


}