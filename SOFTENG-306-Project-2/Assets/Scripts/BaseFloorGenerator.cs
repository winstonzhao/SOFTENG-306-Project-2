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

    private List<GameObject> tiles;

    private void Awake()
    {
        tiles = new List<GameObject>();
        this.GetOrAddComponent<IsoTransform>();
        GenerateFloor();
    }

    public void RePaint() 
    {
        Debug.Log("DESTORYING" + gameObject.transform.childCount);

        foreach (GameObject obj in tiles) {
            DestroyImmediate(obj);
        }

        tiles = new List<GameObject>();
        GenerateFloor();

        //SpriteRenderer r = go.AddComponent<SpriteRenderer>();
        //check = go.GetComponent<SpriteRenderer>().sprite = check;

    }

    private void GenerateFloor() 
    {
        if (!prefab) { 
            Debug.Log("Select a prefab");
            return;
        }
        var i = 0;

        for (var x = 0; x < this.GetComponent<BaseFloorGenerator>().sizeX; x++)
        {
            for (var y = 0; y < this.GetComponent<BaseFloorGenerator>().sizeY; y++)
            {
                GameObject tile = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                tile.transform.parent = this.transform;
                tiles.Add(tile.gameObject);
                tile.name = prefix + " (" + i + ")";
                tile.GetComponent<IsoTransform>().Position = new Vector3(x, 0, y);
                tile.GetComponent<IsoTransform>().ShowBounds = true;
                i++;
                Debug.Log(i + ":" + x + ":" + y);
            }
        }
    }
}
