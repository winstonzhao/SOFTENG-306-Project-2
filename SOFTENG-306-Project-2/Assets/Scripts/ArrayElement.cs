using UnityEngine;

public class ArrayElement : MonoBehaviour {
    public int value = 0;

	// Use this for initialization
	void Start () {
        int level = this.GetComponentInParent<SoftwareLevelGenerator>().currentLevel;

        switch (level)
        {
            default:
                System.Random random = new System.Random();
                value = random.Next(1, 10);
                break;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
