using UnityEngine;
using System;
using System.Security.Cryptography;
using Random = System.Random;

public class IndexElement : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	private static string ITEM = "software_minigame/Sprites/counter";

	private int value = -1;
	public int Value
	{
		get { return value; }
		set
		{
			this.value = value;
			if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = Resources.Load<Sprite>(ITEM + value);
		}
	}

	// Use this for initialization
	public void Generate ()
	{
		Value = 0;
    }

}
