using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class BaseFloorGenerator : MonoBehaviour
{
    public String Prefix = "Floor Tile";

    public GameObject Prefab;

    public int SizeX = 16;

    public int SizeZ = 16;

    public float Y = 0f;

    public bool Barriers = true;

    private void Awake()
    {
        this.GetOrAddComponent<IsoTransform>();
    }

    public void RePaint()
    {
        while (transform.childCount != 0) {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        GenerateFloor();
    }

    private void GenerateFloor()
    {
        if (!Prefab)
        {
            Debug.Log("Select a prefab");
            return;
        }

        var isoTransform = GetComponent<IsoTransform>();
        var parentX = isoTransform.Position.x;
        var parentY = isoTransform.Position.y;
        var parentZ = isoTransform.Position.z;

        var collider = Resources.Load<GameObject>("Prefabs/invis_collider.prefab");

        var height = 1.0f;

        for (var dx = 0; dx <= SizeX; dx++)
        {
            for (var dz = 0; dz <= SizeZ; dz++)
            {
                var x = parentX + dx;
                var z = parentZ + dz;
                var inserted = Insert(Prefab, Prefix + " (" + x + ", " + z + ")", x, Y + parentY, z);
                height = inserted.GetComponent<IsoTransform>().Size.y;
            }
        }

        if (Barriers)
        {
            var fromX = parentX;
            var toX = parentX + SizeX;
            var fromZ = parentZ;
            var toZ = parentZ + SizeZ;

            Insert(collider, "Collider Top Left", fromX + SizeX / 2.0f, height + parentY, toZ + 1)
                .GetComponent<IsoTransform>().Size = new Vector3(SizeX + 1.0f, 1.0f, 1.0f);

            Insert(collider, "Collider Bottom Right", fromX + SizeX / 2.0f, height + parentY, fromZ - 1)
                .GetComponent<IsoTransform>().Size = new Vector3(SizeX + 1.0f, 1.0f, 1.0f);

            Insert(collider, "Collider Bottom Left", fromX - 1.0f, height + parentY, fromZ + SizeZ / 2.0f)
                .GetComponent<IsoTransform>().Size = new Vector3(1.0f, 1.0f, SizeZ + 1.0f);

            Insert(collider, "Collider Top Right", toX + 1.0f, height + parentY, fromZ + SizeZ / 2.0f)
                .GetComponent<IsoTransform>().Size = new Vector3(1.0f, 1.0f, SizeZ + 1.0f);
        }
    }

    private GameObject Insert(GameObject prefab, String name, float x, float y, float z)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.parent = transform;
        gameObject.name = name;

        gameObject.AddComponent<IsoBoxCollider>();
        var isoTransform = gameObject.GetComponent<IsoTransform>();
        isoTransform.Position = new Vector3(x, y, z);
        isoTransform.ShowBounds = name.StartsWith("Collider");

        return gameObject;
    }
}
