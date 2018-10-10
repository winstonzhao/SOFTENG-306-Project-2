using System;
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

    public enum Command
    {
        MOVE,
        PICKUP,
        DROP,
        SWAP
    }

    public enum Compare
    {
        LESS_THAN,
        GREATER_THAN,
        EQUAL_TO
    }

    // Sprites used to represent different state of the object
    private static string NO_ELEMENT = "software_minigame/Sprites/robot1";
    private static string HAS_ELEMENT = "software_minigame/Sprites/robot2";

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

    // Move to the specified coordinate in the scene.
    public bool MoveTo(Vector3 destination)
    {
        // Calculate the required distance to translate for x and z
        var currentPos = isoTransform.Position;
        int x = (int)(destination.x - currentPos.x);
        int z = (int)(destination.z - currentPos.z);

        // This is for storing ths sequence of paths to traverse through
        Vector3[] path = new Vector3[Mathf.Abs(x) + Mathf.Abs(z) + 1];
        path[0] = currentPos;
        int count = 1;

        // temp variables are used as a ghost for collision detection
        int tempX = X;
        int tempZ = Z;
        bool moveX = true;
        bool moveZ = true;

        // Traverse through x first by default
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

        if (!moveZ)
        {
            path = new Vector3[Mathf.Abs(x) + Mathf.Abs(z) + 1];
            path[0] = currentPos;
            count = 1;
            // temp variables are used as a ghost for collision detection
            tempX = X;
            tempZ = Z;
        }

        // Traverse through x with pivot
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

        if (!moveZ && moveX)
        {
            moveZ = true;
            Debug.Log("No z then x path, trying x then z");
            // Traverse through x first by default
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
        
        // If no collision then run it as a coroutine
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
            // Initialise required var
            var sprite = Resources.Load<Sprite>(HAS_ELEMENT);

            // Check the direction of operation and set relevant flag
            CheckDirection(direction);

            // Check if the target position has an object to pickup
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.ELEMENT)
            {
                carrying = generator.GetObject(X + dx, Z + dz);
                generator.SetMapLayout(X + dx, Z + dz, Command.PICKUP, null);
                hasElement = true;
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

    public bool SwapItem(Directions direction)
    {
        if (hasElement || carrying != null)
        {
            // Initialise required var
            var sprite = Resources.Load<Sprite>(HAS_ELEMENT);

            // Check the direction of operation and set relevant flag
            CheckDirection(direction);
            
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.ELEMENT)
            {
                carrying = generator.SetMapLayout(X + dx, Z + dz, Command.SWAP, carrying);
                this.GetComponent<SpriteRenderer>().sprite = sprite;
                return true;
            }
        }

        return false;
    }

    public bool CompareItem(Directions direction, Compare option, bool item, int variable, int input)
    {

        int compareWith = input;
        int value = variable;
        if (item)
        {
            if (hasElement || carrying != null)
            {
                // Check the direction of operation and set relevant flag
                CheckDirection(direction);

                if (generator.GetMapLayout(X + dx, Z + dz) != SoftwareLevelGenerator.Layout.ELEMENT)
                {
                    return false;
                }
                
                compareWith = generator.GetObject(X + dx, Z + dz).GetComponent<ArrayElement>().value;
                value = carrying.GetComponent<ArrayElement>().value;
            }
            else
            {
                return false;
            }
        }
        
        switch (option)
        {
            case Compare.EQUAL_TO:
                if (value == compareWith)
                {
                    print("yes");
                    return true;
                }
                break;
            case Compare.LESS_THAN:
                if (value < compareWith)
                {
                    print("yes");
                    return true;
                }
                break;
            case Compare.GREATER_THAN:
                if (value > compareWith)
                {
                    print("yes");
                    return true;
                }
                break;
        }
        
        return false;
    }

    // Used for checking direction for commands
    private void CheckDirection(Directions direction)
    {
        ResetDirection();
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

    // Used to reset direction check at end of commands
    private void ResetDirection()
    {
        dx = 0;
        dz = 0;
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