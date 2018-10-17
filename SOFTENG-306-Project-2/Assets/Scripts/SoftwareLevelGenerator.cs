using System.Collections.Generic;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System.Collections;
using System.Linq;
using Instructions;
using UnityEngine.SceneManagement;
using Utils;

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
    private bool finishedInputs;

    // Prefabs and Sprites used for different states of the scene
    private static string ELEMENT_PREFAB = "software_minigame/Prefabs/item";
    private static string ITEM = "software_minigame/Sprites/item";
    private static string MORE_INPUTS = "software_minigame/Sprites/key1";
    private static string FINISH_INPUTS = "software_minigame/Sprites/key2";
    private static string INCORRECT_OUTPUT = "software_minigame/Sprites/lock1";
    private static string CORRECT_OUTPUT = "software_minigame/Sprites/lock2";

    // Used for storing all generated object for checking answer.
    private List<GameObject> generatedObjects = new List<GameObject>();
    
    // Used for storing position of array for MoveToIndex
    private Dictionary<string, Vector3> arrayMap;
    
    // Used to run all the instructions
    private InstructionExecutor instructionExecutor;
    public SoftwareEndScreen endCanvas;

    // Enum used to map out the layout of the scene
    public enum Layout
    {
        EMPTY = 0,
        ELEMENT = 1,
        PADDING = -1,
        INDEX = 2
    }

    // Initialisation
    void Start()
    {
        GeneratedLevel(currentLevel);
    }

    // Resetting the level
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
        finishedInputs = false;
        // Destroy any outdated objects
        foreach (var go in generatedObjects)
        {
            Destroy(go);
        }
        generatedObjects.Clear();
        currentLevel = level;
        
        // Regenerate floor layout
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
            {Layout.PADDING, Layout.PADDING, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.EMPTY, Layout.PADDING},
            {Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING, Layout.PADDING},
        };
        
        // Default input location
        inputX = 1;
        inputZ = 7;
        input = new Vector3(inputX - 1, 1, inputZ - 2);
        
        // Empty object map
        objectMap = new GameObject[11, 9];
        
        // Used for creating custom input elements
        GameObject prefab = Resources.Load<GameObject>(ELEMENT_PREFAB);
        GameObject obj;
        SpriteRenderer renderer;
        int value;
        
        // Dictionary used to map index and location of arrays
        arrayMap = new Dictionary<string, Vector3>();

        // Switch statement for setting up different levels
        switch (currentLevel)
        {
            case 1:
                // Pick and Drop without movement into array of size 1 - PickUp and Drop
                inputX = 6;
                inputZ = 5;
                numElements = 1;
                break;
            case 2:
                // Insert single element from input to array of size 1 - MoveTo (by coordinate)
                numElements = 1;
                break;
            case 3:
                // Select the larger element to put in an array of size 1 - Compare
                numElements = 1;
                obj = Instantiate<GameObject>(prefab);
                obj.GetComponent<IsoTransform>().Position = new Vector3(5, 0.8f, 4);
                obj.AddComponent<ArrayElement>().Generate();
                value = obj.GetComponent<ArrayElement>().Value;
                renderer = obj.GetComponent<SpriteRenderer>();
                renderer.sprite = Resources.Load<Sprite>(ITEM + value);
                obj.transform.parent = this.transform;
                generatedObjects.Add(obj);
                layoutMap[6, 5] = Layout.ELEMENT;
                objectMap[6, 5] = obj;
                break;
            case 4:
                // Out of order insertion into empty index - MoveTo (by index)
                numElements = 1;
                var indexNum = RandomGenerator.GetNext(4) + 4;
                for (int x = 4; x < 8; x++)
                {
                    if (x != indexNum)
                    {
                        obj = Instantiate<GameObject>(prefab);
                        obj.GetComponent<IsoTransform>().Position = new Vector3(x - 1, 0.8f, 5);
                        obj.AddComponent<ArrayElement>().Generate();
                        value = obj.GetComponent<ArrayElement>().Value;
                        renderer = obj.GetComponent<SpriteRenderer>();
                        renderer.sprite = Resources.Load<Sprite>(ITEM + value);
                        obj.transform.parent = this.transform;
                        generatedObjects.Add(obj);
                        layoutMap[x, 6] = Layout.ELEMENT;
                        objectMap[x, 6] = obj;
                    }
                    arrayMap.Add("a" + (x - 4), new Vector3(x - 1, 1, 4));
                }
                obj = Instantiate<GameObject>(prefab);
                obj.GetComponent<IsoTransform>().Position = new Vector3(1, 0.8f, 0);
                obj.AddComponent<IndexElement>().Generate();
                obj.GetComponent<IndexElement>().Value = indexNum - 4;
                obj.transform.parent = this.transform;
                generatedObjects.Add(obj);
                layoutMap[2, 1] = Layout.INDEX;
                objectMap[2, 1] = obj;
                break;
            case 5:
                // Insert into 4 element array but no order required - Jump (Loop), Mixture of previous levels
                numElements = 4;
                for (int x = 4; x < 8; x++)
                {
                    arrayMap.Add("a" + (x - 4), new Vector3(x - 1, 1, 4));
                }
                obj = Instantiate<GameObject>(prefab);
                obj.GetComponent<IsoTransform>().Position = new Vector3(1, 0.8f, 5);
                obj.AddComponent<IndexElement>().Generate();
                obj.GetComponent<IndexElement>().Value = 0;
                obj.transform.parent = this.transform;
                generatedObjects.Add(obj);
                layoutMap[2, 6] = Layout.INDEX;
                objectMap[2, 6] = obj;
                break;
            case 6:
                // Sort 2 element from input - Swap, Mixture of previous levels
                numElements = 2;
                for (int x = 5; x < 7; x++)
                {
                    arrayMap.Add("a" + (x - 5), new Vector3(x - 1, 1, 4));
                }
                obj = Instantiate<GameObject>(prefab);
                obj.GetComponent<IsoTransform>().Position = new Vector3(1, 0.8f, 0);
                obj.AddComponent<IndexElement>().Generate();
                obj.GetComponent<IndexElement>().Value = 0;
                obj.transform.parent = this.transform;
                generatedObjects.Add(obj);
                layoutMap[2, 1] = Layout.INDEX;
                objectMap[2, 1] = obj;
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
            obj.AddComponent<ArrayElement>().Generate();
            int value = obj.GetComponent<ArrayElement>().Value;
            print("hi" + value);
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(ITEM + value);
            obj.transform.parent = this.transform;
            generatedObjects.Add(obj);
            layoutMap[inputX, inputZ] = Layout.ELEMENT;
            objectMap[inputX, inputZ] = obj;
            numElements--;
        } else {
            // No elements left
            finishedInputs = true;
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
        // There are still elements left
        if (!finishedInputs)
        {
            return false;
        }
        
        GameObject obj = this.transform.Find("Output").Find("Output").gameObject;
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(CORRECT_OUTPUT);

        // Checking correctness of output for current level
        switch (currentLevel)
        {
            case 1:
                if (objectMap[5, 6] != null && layoutMap[5, 6] == Layout.ELEMENT)
                {
                    renderer.sprite = sprite;
                    return true;
                }
                return false;
            case 2:
                if (objectMap[5, 6] != null && layoutMap[5, 6] == Layout.ELEMENT)
                {
                    renderer.sprite = sprite;
                    return true;
                }
                return false;
            case 3:
                if (objectMap[5, 6] != null)
                {
                    int value = objectMap[5, 6].GetComponent<ArrayElement>().Value;
                    foreach (GameObject o in generatedObjects)
                    {
                        if (o.GetComponent<ArrayElement>().Value > value)
                        {
                            return false;
                        }
                    }
                    
                    renderer.sprite = sprite;
                    return true;
                }
                return false;
            case 4:
                for (int x = 4; x < 8; x++)
                {
                    if (objectMap[x, 6] == null || layoutMap[x, 6] != Layout.ELEMENT)
                    {
                        return false;
                    }
                }
                renderer.sprite = sprite;
                return true;
            case 5:
                for (int x = 4; x < 8; x++)
                {
                    if (objectMap[x, 6] == null || layoutMap[x, 6] != Layout.ELEMENT)
                    {
                        return false;
                    }
                }
                renderer.sprite = sprite;
                return true;
            case 6:
                for (int x = 5; x < 6; x++)
                {
                    GameObject a = objectMap[x, 6];
                    GameObject b = objectMap[x + 1, 6];
                    
                    if (a == null || layoutMap[x, 6] != Layout.ELEMENT)
                    {
                        return false;
                    }
                    if (b == null || layoutMap[x + 1, 6] != Layout.ELEMENT)
                    {
                        return false;
                    }
                    if (a.GetComponent<ArrayElement>().Value > b.GetComponent<ArrayElement>().Value)
                    {
                        return false;
                    }   
                }
                renderer.sprite = sprite;
                return true;
        }
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

            var finished = CheckAnswer();

            if (finished)
            {
                Debug.Log("Finished");
                instructionExecutor = FindObjectOfType<InstructionExecutor>();
                instructionExecutor.Stop();
                endCanvas = Resources.FindObjectsOfTypeAll<SoftwareEndScreen>()[0];
                StartCoroutine(EndScreen());
            }
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

    // Used to find the location for specified index and array combination
    public Vector3 IndexLocation(string index)
    {
        Vector3 pos;
        return arrayMap.TryGetValue(index, out pos) ? pos : Vector3.zero;
    }

    // Show end screen
    private IEnumerator EndScreen()
    {
        if (currentLevel >= 3)
        {
            Toolbox.Instance.QuestManager.MarkCurrentFinished("software-workshop");
        }

        if (currentLevel == 7)
        {
            Toolbox.Instance.AchievementsManager.MarkCompleted("master-workshop");
            Toolbox.Instance.AchievementsManager.MarkCompleted("software-master-workshop");
        }

        yield return new WaitForSeconds(0.5f);
        endCanvas.Open();
    }

}
