using System;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class BaseFloorGenerator : MonoBehaviour
{
    public String prefix = "Floor Tile";

    public Object prefab;

    public int sizeX = 16;

    public int sizeY = 16;

    public bool barriers;

    private List<GameObject> tiles;

    private void Awake()
    {
        tiles = new List<GameObject>();
        this.GetOrAddComponent<IsoTransform>();
        GenerateFloor();
        barriers = true;
    }

    public void RePaint() 
    {
        foreach (GameObject obj in tiles) {
            DestroyImmediate(obj);
        }

        tiles = new List<GameObject>();
        GenerateFloor();

    }

    private void GenerateFloor() 
    {
        if (!prefab) { 
            Debug.Log("Select a prefab");
            return;
        }
        var i = 0;

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                GameObject tile = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                tile.transform.parent = this.transform;
                tiles.Add(tile.gameObject);
                tile.name = prefix + " (" + i + ")";
                tile.GetComponent<IsoTransform>().Position = new Vector3(x, 0, y);
                tile.GetComponent<IsoTransform>().ShowBounds = true;
                i++;
            }
        }

        if (barriers) {
            var j = 0;
            Object collider = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/invis_collider.prefab", typeof(GameObject));

            for (var x = -1; x <= sizeX; x++)
            {
                GameObject bottomRight = PrefabUtility.InstantiatePrefab(collider) as GameObject;
                bottomRight.transform.parent = this.transform;
                tiles.Add(bottomRight.gameObject);
                bottomRight.name = "(I)" + " (" + j + ")";
                bottomRight.GetComponent<IsoTransform>().Position = new Vector3(x, 0.6f, -1);
                bottomRight.GetComponent<IsoTransform>().ShowBounds = true;
                j++;


                GameObject topLeft = PrefabUtility.InstantiatePrefab(collider) as GameObject;
                topLeft.transform.parent = this.transform;
                tiles.Add(topLeft.gameObject);
                topLeft.name = "(I)" + " (" + j + ")";
                topLeft.GetComponent<IsoTransform>().Position = new Vector3(x, 0.6f, sizeX);
                topLeft.GetComponent<IsoTransform>().ShowBounds = true;
                j++;
            }

            for (var y = -1; y <= sizeY; y++)
            {
                GameObject bottomLeft = PrefabUtility.InstantiatePrefab(collider) as GameObject;
                bottomLeft.transform.parent = this.transform;
                tiles.Add(bottomLeft.gameObject);
                bottomLeft.name = "(I)" + " (" + j + ")";
                bottomLeft.GetComponent<IsoTransform>().Position = new Vector3(-1, 0.6f, y);
                bottomLeft.GetComponent<IsoTransform>().ShowBounds = true;
                j++;

                GameObject topRight = PrefabUtility.InstantiatePrefab(collider) as GameObject;
                topRight.transform.parent = this.transform;
                tiles.Add(topRight.gameObject);
                topRight.name = "(I)" + " (" + j + ")";
                topRight.GetComponent<IsoTransform>().Position = new Vector3(sizeY, 0.6f, y);
                topRight.GetComponent<IsoTransform>().ShowBounds = true;
                j++;
            }

        }
    }
}
