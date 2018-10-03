using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEditor;

public class SoftwareLevelGenerator : MonoBehaviour
{
    // Fields used to represent the current level and game
    public int currentLevel;
    public int numElements;
    private Layout[,] layoutMap;
    private GameObject[,] objectMap;

    // Used to locate the input and output gameobject in the scene
    public volatile int inputX;
    public volatile int inputZ;
    public Vector3 input;

    // Prefabs and Sprites used for different states of the scene
    private static string ELEMENT_PREFAB = "software_minigame/Prefabs/test_item";
    private static string FINISH_INPUTS = "software_minigame/Sprites/key2";
    private static string CORRECT_OUTPUT = "software_minigame/Sprites/lock2";

    // Enum used to map out the layout of the scene
    public enum Layout
    {
        EMPTY = 0,
        ELEMENT = 1,
        PADDING = -1
    }

    // Initialisation
    void Start()
    {
        GeneratedLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Used to initialised diferent levels
    public void GeneratedLevel(int level)
    {
        currentLevel = level;
        // Switch statement for setting up different levels
        switch (currentLevel)
        {
            case 1:
                numElements = 1;
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
                input = new Vector3(inputX - 1, 1, inputZ - 2);
                objectMap = new GameObject[11, 9];
                NextInputElement();
                break;
        }
    }

    // Used for displaying the next element if the current element has been moved else where
    public void NextInputElement() 
    {
        if (numElements != 0) {
            // Next input element will pop out at input
            GameObject prefab = Resources.Load<GameObject>(ELEMENT_PREFAB);
            GameObject obj = Instantiate<GameObject>(prefab);
            obj.GetComponent<IsoTransform>().Position = new Vector3(inputX - 1, 0.8f, inputZ - 1);
            obj.AddComponent<ArrayElement>();
            obj.transform.parent = this.transform;
            layoutMap[inputX, inputZ] = Layout.ELEMENT;
            objectMap[inputX, inputZ] = obj;
            numElements--;
        } else {
            // No elements left
            layoutMap[inputX, inputZ] = Layout.EMPTY;
            GameObject obj = this.transform.Find("Input").Find("Input").gameObject;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>(FINISH_INPUTS);
            renderer.sprite = sprite;
        }
    }

    // Used to check if the answer is correct
    public bool CheckAnswer()
    {
        // There are stil elements left
        if (numElements != 0)
        {
            return false;
        }

        switch (currentLevel)
        {
            case 1:
                int max = 0;
                for (int x = 5; x < 6; x++)
                {
                    if (objectMap[x, 6] != null)
                    {
                        int value = objectMap[x, 6].GetComponent<ArrayElement>().value;
                        if (!(layoutMap[x, 6] == Layout.ELEMENT))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                GameObject obj = this.transform.Find("Output").Find("Output").gameObject;
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                Sprite sprite = Resources.Load<Sprite>(CORRECT_OUTPUT);
                renderer.sprite = sprite;
                return true;
        }

        return false;
    }

    // Used for updating the layout of the scene when a command is valid
    public void SetMapLayout(int x, int z, RobotController.Command command, GameObject obj)
    {
        if (command == RobotController.Command.PICKUP)
        {
            Debug.Log("Pick x: " + x + " z: " + z);
            layoutMap[x, z] = Layout.EMPTY;
            objectMap[x, z].SetActive(false);
            objectMap[x, z] = null;
        }
        else if (command == RobotController.Command.DROP)
        {
            Debug.Log("Drop x: " + x + " z: " + z);
            var oldPos = obj.GetComponent<IsoTransform>().Position;
            objectMap[(int)oldPos.x, (int)oldPos.z] = null;
            layoutMap[x, z] = Layout.ELEMENT;
            obj.GetComponent<IsoTransform>().Position = new Vector3(x - 1, 0.8f, z - 1);
            objectMap[x, z] = obj;
            objectMap[x, z].SetActive(true);
            if (((x != inputX) || (z != inputZ)) && (objectMap[inputX, inputZ] == null)) {
                NextInputElement();
            }
            CheckAnswer();
        }
    }

    // Getter for checking if a specific index is empty or not
    public Layout GetMapLayout(int x, int z)
    {
        return layoutMap[x, z];
    }

    // Getter for getting the object at the specific index
    public GameObject GetObject(int x, int z)
    {
        return objectMap[x, z];
    }

    // Used for debugging and printing the layout of the scene
    //public void PrintMap()
    //{
    //    for (int i = 0; i < 11; i++)
    //    {
    //        string map = "";
    //        for (int j = 0; j < 9; j++)
    //        {
    //            map = map + layoutMap[i, j] + ": ";
    //        }
    //        Debug.Log(map);
    //    }
    //}
}
