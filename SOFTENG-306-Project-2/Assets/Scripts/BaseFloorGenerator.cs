using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class BaseFloorGenerator : MonoBehaviour
{
    public String Prefix = "Leech Floor Tile";

    public float FromX = -16f;

    public float ToX = 16f;

    public float FromY = -16f;

    public float ToY = 16f;

    private void Awake()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/leech_floor_tile.prefab", typeof(GameObject));
        var i = 1;

        for (var x = FromX; x <= ToX; x++)
        {
            for (var y = FromY; y <= ToY; y++)
            {
                var tile = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                tile.name = Prefix + " (" + i + ")";
                tile.GetComponent<IsoTransform>().Position = new Vector3(x, 0, y);
                tile.GetComponent<IsoTransform>().ShowBounds = false;
                i++;
            }
        }
    }
}
