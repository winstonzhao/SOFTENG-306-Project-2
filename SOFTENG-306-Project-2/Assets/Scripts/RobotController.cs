using System.Collections;
using Instructions;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;
using UnityEngine;

[RequireComponent(typeof(IsoRigidbody))]
public class RobotController : MonoBehaviour
{
    // Components required for the robot controller
    private IsoTransform isoTransform;
    private SoftwareLevelGenerator generator;

    // Used to map out the position of the robot in the scene
    public int X;
    public int Y;
    public int Z;

    // temp variables used for directions
    private int dx;
    private int dz;

    private Vector3 startPos;

    // Default speed for transforming the robot
    private float speed = 5;

    // Boolean and object to see if the robot is currently holding an object
    private bool hasElement = false;
    private GameObject carrying;

    // Fields to animating the robot transformation
    private float timePassed;
    private float maxTimePassed;

    // Which command the robot is executing which changes the layout of the map
    public enum Command
    {
        PICKUP,
        DROP,
        SWAP
    }
    
    // Possible compare instructions for the Robot
    public enum Compare
    {
        LESS_THAN = 0,
        GREATER_THAN = 2,
        EQUAL_TO = 1
    }

    // Sprites used to represent different state of the object
    private static string NO_ELEMENT = "software_minigame/Sprites/robot";
    private static string HAS_ELEMENT = "software_minigame/Sprites/holding";

    // Initialization
    void Start()
    {
        isoTransform = this.GetOrAddComponent<IsoTransform>();
        generator = gameObject.GetComponentInParent(typeof(SoftwareLevelGenerator)) as SoftwareLevelGenerator;

        startPos = isoTransform.Position;
        // Initialise offset coordinates
        X = (int)(isoTransform.Position.x + 1);
        Y = 1;
        Z = (int)(isoTransform.Position.z + 1);
    }

    // Used to reset the position of the robot to start
    public void ResetPos()
    {
        hasElement = false;
        carrying = null;

        var sprite = Resources.Load<Sprite>(NO_ELEMENT);
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        isoTransform.Position = startPos;
        X = (int)(isoTransform.Position.x + 1);
        Y = 1;
        Z = (int)(isoTransform.Position.z + 1);

        generator.Restart();
    }

    // Move to the specified coordinate or index of the an array in the scene
    // If it is coordinate, it will use the destination vector3
    // If destination is Vector3.zero, it will use the array concatenate with index as identifier for accessing the array
    // e.g "a" + 0 will access index 0 of array a
    public bool MoveTo(Vector3 destination, string index)
    {
        // Check if is index or coordinate access
        var dest = destination;
        if (destination == Vector3.zero)
        {
            dest = generator.IndexLocation(index);
            if (dest == Vector3.zero)
            {
                return false;
            }
        }
        
        // Calculate the required distance to translate for x and z
        var currentPos = isoTransform.Position;
        int x = (int)(dest.x - currentPos.x);
        int z = (int)(dest.z - currentPos.z);

        // This is for storing ths sequence of paths to traverse through
        Vector3[] path = new Vector3[Mathf.Abs(x) + Mathf.Abs(z) + 1];
        path[0] = currentPos;
        int count = 1;

        // temp variables are used as a ghost for collision detection
        int tempX = X;
        int tempZ = Z;
        bool moveX = true;
        bool moveZ = true;

        // Traverse through z first by default
        Debug.Log("Trying z then x");
        for (int i = 0; i < Mathf.Abs(z); i++)
        {
            tempZ = z < 0 ? --tempZ : ++tempZ;

            if (generator.GetMapLayout(tempX, tempZ) == SoftwareLevelGenerator.Layout.EMPTY)
            {
                path[count] = new Vector3(tempX - 1, Y, tempZ - 1);
                count++;
            }
            else
            {
                moveZ = false;
            }
        }

        // Check if collision exist if z direction, if it does the reset the path and try path x then z
        if (!moveZ)
        {
            path = new Vector3[Mathf.Abs(x) + Mathf.Abs(z) + 1];
            path[0] = currentPos;
            count = 1;
            // temp variables are used as a ghost for collision detection
            tempX = X;
            tempZ = Z;
        }

        // Traverse through x
        for (int i = 0; i < Mathf.Abs(x); i++)
        {
            tempX = x < 0 ? --tempX : ++tempX;

            // Check for collision and add to path
            if (generator.GetMapLayout(tempX, tempZ) == SoftwareLevelGenerator.Layout.EMPTY)
            {
                path[count] = new Vector3(tempX - 1, Y, tempZ - 1);
                count++;
            }
            else
            {
                moveX = false;
            }
        }

        // This is when there was a collision in the path z then x, and no collision was detected in x path,
        // therefore, is not checking path x then z
        if (!moveZ && moveX)
        {
            moveZ = true;
            Debug.Log("No z then x path, trying x then z");
            for (int i = 0; i < Mathf.Abs(z); i++)
            {
                tempZ = z < 0 ? --tempZ : ++tempZ;

                if (generator.GetMapLayout(tempX, tempZ) == SoftwareLevelGenerator.Layout.EMPTY)
                {
                    path[count] = new Vector3(tempX - 1, Y, tempZ - 1);
                    count++;
                }
                else
                {
                    moveZ = false;
                }
            }
        }
        
        // If no collision then run it as a co-routine
        if (moveX && moveZ)
        {
            StartCoroutine(Shift(path));
            return true;
        }
        
        Debug.Log("COLLIDE");
        return false;  
    }

    // Moving 1 step in specified direction, return true if the command is valid
    public bool Move(Directions direction)
    {
        // Initialise position and destination vector
        var currentPos = this.GetComponent<IsoTransform>().Position;
        var destPos = Vector3.zero;
        Vector3[] path = new Vector3[2];
        path[0] = currentPos;

        // Check the direction of operation and set relevant flag
        CheckDirection(direction);

        // Check if the tile is empty to allow movement and avoid collisions
        if (generator.GetMapLayout(X + dx, Z + dz) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            return false;
        }

        // The operation is valid
        X += dx;
        Z += dz;

        destPos = new Vector3(currentPos.x + dx, Y, currentPos.z + dz);
        path[1] = destPos;
        StartCoroutine(Shift(path));

        return true;
    }

    // Pickup the item in the specified direction
    public bool PickUpItem(Directions direction)
    {
        if (!hasElement || carrying == null)
        {
            

            // Check the direction of operation and set relevant flag
            CheckDirection(direction);

            // Check if the target position has an object to pickup
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.ELEMENT)
            {
                carrying = generator.GetObject(X + dx, Z + dz);
                generator.SetMapLayout(X + dx, Z + dz, Command.PICKUP, null);
                hasElement = true;
                // Initialise required var
                var sprite = Resources.Load<Sprite>(HAS_ELEMENT + carrying.GetComponent<ArrayElement>().value);
                this.GetComponent<SpriteRenderer>().sprite = sprite;
                return true;
            }
        }

        return false;
    }

    // Drop the item to specified direction
    public bool DropItem(Directions direction)
    {
        if (hasElement || carrying != null)
        {
            // Initialise required var
            var sprite = Resources.Load<Sprite>(NO_ELEMENT);

            // Check the direction of operation and set relevant flag
            CheckDirection(direction);

            // Check if the target position is empty for dropping the object
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.EMPTY)
            {
                generator.SetMapLayout(X + dx, Z + dz, Command.DROP, carrying);
                carrying = null;
                hasElement = false;
                this.GetComponent<SpriteRenderer>().sprite = sprite;
                return true;
            }
        }
        
        return false;
    }

    // Used to swap the item which the robot is holding with the item in the specified direction
    public bool SwapItem(Directions direction)
    {
        if (hasElement || carrying != null)
        {
            // Check the direction of operation and set relevant flag
            CheckDirection(direction);
            
            // Check if an element exists in the specified direction
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.ELEMENT)
            {
                carrying = generator.SetMapLayout(X + dx, Z + dz, Command.SWAP, carrying);
                var sprite = Resources.Load<Sprite>(HAS_ELEMENT + carrying.GetComponent<ArrayElement>().value);
                this.GetComponent<SpriteRenderer>().sprite = sprite;
                return true;
            }
        }

        return false;
    }

    // This is used for both the compare operation as well as the jump if operation
    // If it is compare operation, the item param will be set to true, and the compare is done between the item which the 
    // robot is currently holding and the item in the direction specified.
    // Otherwise, for jump compare, the item param is set to false and the comparison is done between the variable and
    // the input param
    public bool CompareItem(Directions direction, Compare option, bool item, int variable, int input)
    {
        // Set to compare for jump by default
        int compareWith = input;
        int value = variable;
        
        // If item flag is set then it is used by the compare instruction and override the comparing values with
        // appropriate values.
        if (item)
        {
            if (hasElement || carrying != null)
            {
                // Check the direction of operation and set relevant flag
                CheckDirection(direction);

                // Check if the item exist
                if (generator.GetMapLayout(X + dx, Z + dz) != SoftwareLevelGenerator.Layout.ELEMENT)
                {
                    return false;
                }
                
                // Override flags for to the compare instruction
                compareWith = generator.GetObject(X + dx, Z + dz).GetComponent<ArrayElement>().value;
                value = carrying.GetComponent<ArrayElement>().value;
            }
            else
            {
                return false;
            }
        }
        
        // Check for specified type of comparison
        switch (option)
        {
            case Compare.EQUAL_TO:
                if (value == compareWith)
                {
                    return true;
                }
                break;
            case Compare.LESS_THAN:
                if (value < compareWith)
                {
                    return true;
                }
                break;
            case Compare.GREATER_THAN:
                if (value > compareWith)
                {
                    return true;
                }
                break;
        }
        
        return false;
    }

    // Used for checking direction for commands
    private void CheckDirection(Directions direction)
    {
        // Reset old direction
        dx = 0;
        dz = 0;
        // Checking the direction which the command specifies and set relevant flags
        switch (direction)
        {
            case Directions.Up:
                dx = 1;
                break;
            case Directions.Right:
                dz = -1;
                break;
            case Directions.Down:
                dx = -1;
                break;
            case Directions.Left:
                dz = 1;
                break;
        }
    }

    // Animation for moving the robot
    private IEnumerator Shift(Vector3[] path)
    {
        // Loop through the path
        for (int i = 0; i < path.Length - 1; i++)
        {
            timePassed = 0f;
            maxTimePassed = Vector3.Distance(path[i], path[i + 1]) / speed;

            // Move if you are not fully translated to next node in the path
            while (isoTransform.Position != path[i + 1])
            {
                timePassed += Time.deltaTime;
                isoTransform.Position = Vector3.Lerp(path[i], path[i + 1], timePassed / maxTimePassed);

                // If time has passed then round coordinates to nearest node.
                if (timePassed >= maxTimePassed)
                {
                    var pos = isoTransform.Position;
                    pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                    break;
                }
                yield return null;
            }
        }

        // Update the offset coordinates
        X = (int)isoTransform.Position.x + 1;
        Z = (int)isoTransform.Position.z + 1;
    }
}