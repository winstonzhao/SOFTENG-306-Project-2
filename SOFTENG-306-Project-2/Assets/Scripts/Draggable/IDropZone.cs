public interface IDropZone
{
    // A draggable object enters this drop zone
    void OnDragEnter(Draggable item);

    // A draggable object leaves this drop zone
    void OnDragExit(Draggable item);

    // Drag finishes inside this drop zone
    void OnDrop(Draggable item);

    // Drag has finished by an object owned by thisd drop zone
    void OnDragFinish(Draggable item);

    // An item in this drop zone is being dragged (called every frame)
    void OnItemDrag(Draggable item);

    // An item in this drop zone begins to be dragged
    void OnDragStart(Draggable item);

    // An item has been allocated to another drop zone
    void OnItemRemove(Draggable item);
}