using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class for handling cursor changing of sprites, attach this to your main camera in a scene
 */
public class CursorHandler : MonoBehaviour {

	public Texture2D mouseDefaultTexture; // Default cursor sprite
	public Texture2D mouseOverTexture; // Pointer sprite i.e. Hand
	public Texture2D mouseDragTexture; // Drag sprite i.e. Grabbing hand
	public CursorMode cursorMode = CursorMode.Auto;
	
	// For ensuring accuracy with pointing by the sprites, these are offsets
	public Vector2 hotSpotDefault = Vector2.zero;
	public Vector2 hotSpotOver = Vector2.zero;
	public Vector2 hotSpotDrag = Vector2.zero;

	/**
	 * Set the default cursor image
	 */
	private void Awake()
	{
		Cursor.SetCursor(mouseDefaultTexture, hotSpotDefault, cursorMode);
	}

	/**
	 * Cursor enters the area, i.e. Mouse enter
	 */
	public void CursorEnter()
	{
		Cursor.SetCursor(mouseOverTexture, hotSpotOver, cursorMode);
	}
	
	/**
	 * Reset cursor to default
	 */
	public void CursorClear()
	{
		Cursor.SetCursor(mouseDefaultTexture, hotSpotDefault, cursorMode);
	}

	/**
	 * Cursor is dragging an Object
	 */
	public void CursorDrag()
	{
		Cursor.SetCursor(mouseDragTexture, hotSpotDrag, cursorMode);
	}
}
