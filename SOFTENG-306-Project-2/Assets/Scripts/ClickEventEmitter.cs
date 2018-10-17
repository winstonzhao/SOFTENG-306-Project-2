using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventEmitter : EventEmitter, IPointerDownHandler
{
    public EventHandlerDelegate EventHandler;

    private bool enabled = true;

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Enabled)
        {
            EventHandler();
        }
    }
}