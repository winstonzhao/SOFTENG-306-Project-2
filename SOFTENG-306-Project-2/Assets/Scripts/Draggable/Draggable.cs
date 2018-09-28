using UnityEngine;

public abstract class Draggable : MonoBehaviour
{
    public virtual Vector3 HomePos { get; set; }

    public abstract void SetDropZone(IDropZone dropZone);
}