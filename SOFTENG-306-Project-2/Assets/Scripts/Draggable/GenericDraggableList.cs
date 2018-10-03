using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic class to encapsulate a list of Draggable Items
/// </summary>
public abstract class GenericDraggableList : MonoBehaviour
{
    /// <summary>
    /// All draggable items in this list
    /// </summary>
    public abstract IEnumerable<Draggable> ListItems { get; }
}