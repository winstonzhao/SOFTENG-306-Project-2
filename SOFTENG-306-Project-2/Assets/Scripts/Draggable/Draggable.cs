using UnityEngine;

/// <summary>
/// A Draggable item that can be put into an IDropZone
/// </summary>
public abstract class Draggable : MonoBehaviour
{
    /// <summary>
    /// The position where the draggable object should return to
    /// </summary>
    public virtual Vector3 HomePos { get; set; }

    /// <summary>
    /// The virtual Width and Height of the draggable object
    /// </summary>
    public abstract Vector2 Size { get; set; }

    /// <summary>
    /// Sets the current drop zone of the Draggable object to the given drop zone
    /// </summary>
    public abstract void SetDropZone(IDropZone newDropZone);
}
