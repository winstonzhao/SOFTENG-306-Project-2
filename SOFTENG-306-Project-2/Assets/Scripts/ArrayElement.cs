using UnityEngine;
using System;
using System.Security.Cryptography;
using Random = System.Random;

public class ArrayElement : MonoBehaviour {
    public int value;

	// Use this for initialization
	public void Generate () {
		value = NextInt(1, 10);
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
