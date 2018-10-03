using UnityEngine;

public abstract class EventEmitter<T> : MonoBehaviour
{
    public delegate void EventHandler(T args);
    public abstract void SetEventHandler(EventHandler handler);
}