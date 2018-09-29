public class ClickEventEmitter : EventEmitter
{
    public EventHandlerDelegate EventHandler;

    public void OnMouseDown()
    {
        EventHandler();
    }
}