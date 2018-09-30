using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEditor;

public class SoftwareLevelGenerator : MonoBehaviour
{

    public int currentLevel;
    public int numElements;

    private int inputX;
    private int inputZ;
    private int outputX;
    private int outputZ;

    private Layout[,] layoutMap;
    private GameObject[,] objectMap;
    private static string ELEMENT_PREFAB = "Assets/Prefabs/software_minigame/test_item.prefab";
    private static string FINISH_INPUTS = "Assets/Sprites/software_minigame/abstractTile_33.png";


    public enum Layout
    {
        EMPTY = 0,
        ELEMENT = 1,
        PADDING = -1
    }

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
                inputX = 1;
                inputZ = 7;
                outputX = 1;
                outputZ = 1;
                objectMap = new GameObject[11, 9];
                NextInputElement();
                break;
        }
    }

    public void NextInputElement() {
        if (numElements != 0) {
            Object prefab = AssetDatabase.LoadAssetAtPath(ELEMENT_PREFAB, typeof(GameObject));
            GameObject obj = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.GetComponent<IsoTransform>().Position = new Vector3(inputX - 1, 0.8f, inputZ - 1);
            obj.AddComponent<ArrayElement>();
            layoutMap[inputX, inputZ] = Layout.ELEMENT;
            objectMap[inputX, inputZ] = obj;
            obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            numElements--;
        } else {
            GameObject obj = this.transform.Find("Input").Find("Input").gameObject;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            Sprite sprite = (UnityEngine.Sprite)AssetDatabase.LoadAssetAtPath(FINISH_INPUTS, typeof(Sprite));
            renderer.sprite = sprite;
        }
    }

    public bool CheckAnswer()
    {
        if (numElements != 0)
        {
            return false;
        }

        switch (currentLevel)
        {
            case 1:
                return true;
        }

        return false;
    }

    public void SetMapLayout(int x, int z, Layout layout, GameObject obj)
    {
        if (layout == Layout.EMPTY)
        {
            Debug.Log("Pick x: " + x + " z: " + z);
            layoutMap[x, z] = layout;
            objectMap[x, z].SetActive(false);
            objectMap[x, z] = null;
        }
        else if (layout == Layout.ELEMENT)
        {
            Debug.Log("Drop x: " + x + " z: " + z);
            var oldPos = obj.GetComponent<IsoTransform>().Position;
            objectMap[(int)oldPos.x, (int)oldPos.z] = null;
            layoutMap[x, z] = layout;
            obj.GetComponent<IsoTransform>().Position = new Vector3(x, 0, z);
            objectMap[x, z] = obj;
            objectMap[x, z].SetActive(true);
            if ((x != inputX) || (z != inputZ)) {
                NextInputElement();
            }
        }
    }

    public void PrintMap()
    {
        for (int i = 0; i < 11; i++)
        {
            string map = "";
            for (int j = 0; j < 9; j++)
            {
                map = map + layoutMap[i, j] + ": ";
            }
            Debug.Log(map);
        }
    }

    public Layout GetMapLayout(int x, int z)
    {
        return layoutMap[x, z];
    }

    public GameObject GetObject(int x, int z)
    {
        return objectMap[x, z];
    }
}
