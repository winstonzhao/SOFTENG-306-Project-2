using UnityEngine.EventSystems;

public class ClickEventEmitter : EventEmitter, IPointerClickHandler
{
    public EventHandlerDelegate EventHandler;

    private bool enabled = true;

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Enabled)
        {
            EventHandler();
        }
    }
}