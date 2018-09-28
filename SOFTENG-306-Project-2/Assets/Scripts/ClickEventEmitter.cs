public class ClickEventEmitter : EventEmitter
{
    private EventHandler eventHandler;

    public override void SetEventHandler(EventHandler handler)
    {
        eventHandler = handler;
    }

    public void OnMouseDown()
    {
        eventHandler();
    }
}