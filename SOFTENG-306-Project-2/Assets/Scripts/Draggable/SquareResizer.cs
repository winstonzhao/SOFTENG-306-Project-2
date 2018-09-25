using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareResizer : MonoBehaviour
{

    [SerializeField]
    private float width;
    public float Width { get { return width; } }

    // Use this for initialization
    void Start()
    {
        UpdateSize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSize()
    {
        var text = transform.GetChild(1);
        var square = transform.GetChild(0);

        width = text.GetComponent<MeshRenderer>().bounds.size.x;
        square.transform.localScale = new Vector3(width, 1, 1);

        GetComponent<BoxCollider2D>().size = new Vector3(width, 1, 1);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
