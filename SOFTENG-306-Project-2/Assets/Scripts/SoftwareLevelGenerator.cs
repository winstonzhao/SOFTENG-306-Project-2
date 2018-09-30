using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEditor;

public class SoftwareLevelGenerator : MonoBehaviour
{

    public int currentLevel;
    public int numElements;

    private Layout[,] layoutMap;
    private GameObject[,] objectMap;
    private static string ELEMENT_PREFAB = "Assets/Prefabs/software_minigame/test_item.prefab";


    // Use this for initialization
    void Start()
    {
        GeneratedLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void GeneratedLevel(int level)
    {
        currentLevel = level;
        numElements = 6;
        switch (currentLevel)
        {
            case 1:
                layoutMap = new Layout[11, 9]
                {   {Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
                    {Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING},
                };
                objectMap = new GameObject[11, 9];

                Object prefab = AssetDatabase.LoadAssetAtPath(ELEMENT_PREFAB, typeof(GameObject));
                GameObject obj = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.GetComponent<IsoTransform>().Position = new Vector3(0, 0.8f, 6);
                obj.AddComponent<ArrayElement>();
                layoutMap[1, 7] = Layout.ELEMENT;
                objectMap[1, 7] = obj;
                break;
        }
    }

    public void SetMapLayout(int x, int z, Layout layout, GameObject obj) {
        if (layout == Layout.EMPTY) {
            layoutMap[x, z] = layout;
            DestroyImmediate(objectMap[x, z]);
        } else if (layout == Layout.ELEMENT) {
            layoutMap[x, z] = layout;
            objectMap[x, z] = obj;
        }
    }

    public Layout GetMapLayout(int x, int z) {
        return layoutMap[x, z];
    }

    public enum Layout
    {
        EMPTY = 0,
        ELEMENT = 1,
        PADDING = -1
    }
}
