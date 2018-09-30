using UnityEngine;

public class ArrayElement : MonoBehaviour {
    public int value;

	// Use this for initialization
	void Start () {
        System.Random random = new System.Random();
        value = random.Next(1, 10);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
