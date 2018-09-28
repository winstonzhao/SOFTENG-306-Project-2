using UnityEngine;

public abstract class EventEmitter : MonoBehaviour
{
    public delegate void EventHandler();
    public abstract void SetEventHandler(EventHandler handler);
}