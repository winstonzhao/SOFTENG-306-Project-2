using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("No longer updated")]
public class SquareResizer : MonoBehaviour
{

    public float Width;
    private BoxCollider2D collider;
    private MeshRenderer meshRenderer;

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
        if (collider == null) collider = GetComponent<BoxCollider2D>();
        
        if (meshRenderer == null)
        {
            var text = transform.GetChild(1);
            meshRenderer = text.GetComponent<MeshRenderer>();
        }

        var square = transform.GetChild(0);

        Width = meshRenderer.bounds.size.x;
        square.transform.localScale = new Vector3(Width, 1, 1);

        collider.size = new Vector3(Width, 1, 1);
        collider.isTrigger = true;
    }
}
