using UnityEngine;
using System;
using System.Security.Cryptography;
using Random = System.Random;

public class ArrayElement : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	private static string ITEM = "software_minigame/Sprites/item";

	private int value;
	public int Value
	{
		get { return value; }
		set
		{
			this.value = value;
			if (spriteRenderer != null) spriteRenderer.sprite = Resources.Load<Sprite>(ITEM + value);
		}
	}

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	public void Generate () {
		Value = NextInt(1, 10);
    }

	private static int NextInt(int min, int max)
	{
		RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
		byte[] buffer = new byte[4];

		rng.GetBytes(buffer);
		int result = BitConverter.ToInt32(buffer, 0);

		return new Random(result).Next(min, max);
	}
}
