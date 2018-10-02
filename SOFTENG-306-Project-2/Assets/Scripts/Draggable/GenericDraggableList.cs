using System.Collections.Generic;
using UnityEngine;

public abstract class GenericDraggableList : MonoBehaviour
{
    public abstract IEnumerable<Draggable> ListItems { get; }
}