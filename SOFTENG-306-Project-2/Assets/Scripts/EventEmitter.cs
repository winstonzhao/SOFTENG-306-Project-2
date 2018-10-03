using UnityEngine;

public abstract class EventEmitter : MonoBehaviour
{
    public delegate void EventHandlerDelegate();
}