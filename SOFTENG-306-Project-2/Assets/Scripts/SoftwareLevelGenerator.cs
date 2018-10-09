using System.Collections.Generic;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System.Collections;

public class SoftwareLevelGenerator : MonoBehaviour
{
    // Fields used to represent the current level and game
    public int currentLevel;
    public int numElements;
    public GameObject endScreen;
    private Layout[,] layoutMap;
    private GameObject[,] objectMap;

    // Used to locate the input and output gameobject in the scene
    public volatile int inputX;
    public volatile int inputZ;
    public Vector3 input;

    // Prefabs and Sprites used for different states of the scene
    private static string ELEMENT_PREFAB = "software_minigame/Prefabs/test_item";
    private static string MORE_INPUTS = "software_minigame/Sprites/key1";
    private static string FINISH_INPUTS = "software_minigame/Sprites/key2";
    private static string INCORRECT_OUTPUT = "software_minigame/Sprites/lock1";
    private static string CORRECT_OUTPUT = "software_minigame/Sprites/lock2";

    private List<GameObject> generatedObjects = new List<GameObject>();

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
        GeneratedLevel(currentLevel);
    }

    public void Restart() {
        GeneratedLevel(currentLevel);
        GameObject input = this.transform.Find("Input").Find("Input").gameObject;
        Sprite i = Resources.Load<Sprite>(MORE_INPUTS);
        input.GetComponent<SpriteRenderer>().sprite = i;
        GameObject output = this.transform.Find("Output").Find("Output").gameObject;
        Sprite o = Resources.Load<Sprite>(INCORRECT_OUTPUT);
        output.GetComponent<SpriteRenderer>().sprite = o;
    }

    // Used to initialised different levels
    public void GeneratedLevel(int level)
    {
        foreach (var go in generatedObjects)
        {
            Destroy(go);
        }
        currentLevel = level;
        
        layoutMap = new[,]
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

        
        // Switch statement for setting up different levels
        switch (currentLevel)
        {
            case 1:
                numElements = 1;
                break;
            case 2:
                numElements = 4;
                break;
        }
        NextInputElement();
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
            generatedObjects.Add(obj);
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
        
        GameObject obj = this.transform.Find("Output").Find("Output").gameObject;
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(CORRECT_OUTPUT);

        switch (currentLevel)
        {
            case 1:
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
                renderer.sprite = sprite;
                return true;
            case 2:
                for (int x = 4; x < 8; x++)
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
                renderer.sprite = sprite;
                return true;
        }
        
//        StartCoroutine(EndScreen());
        return false;
    }

    // Used for updating the layout of the scene when a command is valid
    public GameObject SetMapLayout(int x, int z, RobotController.Command command, GameObject obj)
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
            objectMap[(int) oldPos.x, (int) oldPos.z] = null;
            layoutMap[x, z] = Layout.ELEMENT;
            obj.GetComponent<IsoTransform>().Position = new Vector3(x - 1, 0.8f, z - 1);
            objectMap[x, z] = obj;
            objectMap[x, z].SetActive(true);
            if (((x != inputX) || (z != inputZ)) && objectMap[inputX, inputZ] == null)
            {
                NextInputElement();
            }

            CheckAnswer();
        } 
        else if (command == RobotController.Command.SWAP)
        {
            Debug.Log("Swap x: " + x + " z: " + z);
            GameObject temp = objectMap[x, z];
            temp.SetActive(false);
            objectMap[x, z] = obj;
            obj.GetComponent<IsoTransform>().Position = new Vector3(x - 1, 0.8f, z - 1);
            obj.SetActive(true);
            return temp;
        }

        return null;
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

    private IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(1.0f);
        endScreen.GetComponent<SoftwareEndScreen>().Open();
    }
}
