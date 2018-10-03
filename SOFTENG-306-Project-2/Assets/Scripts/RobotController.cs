using System.Collections;
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
        DROP
    }

    // Sprites used to represent different state of the object
    private static string NO_ELEMENT = "software_minigame/Sprites/robot1";
    private static string HAS_ELEMENT = "software_minigame/Sprites/robot2";

    // Enum to represent the direction for executing the particular action
    public enum Direction
    {
        TopRight = 0,
        BottomRight = 3,
        BottomLeft = 1,
        TopLeft = 2,
    }

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

    // Update is called once per frame
    void Update()
    {
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

        // Traverse through x first by default
        for (int i = 0; i < Mathf.Abs(x); i++)
        {
            tempX = x < 0 ? --tempX : ++tempX;

            // Check for collision and add to path
            if ((generator.GetMapLayout(tempX, tempZ) == SoftwareLevelGenerator.Layout.EMPTY))
            {
                path[count] = new Vector3(tempX - 1, Y, tempZ - 1);
                count++;
            }
            else
            {
                return false;
            }
        }

        // Traverse through z with pivot
        for (int i = 0; i < Mathf.Abs(z); i++)
        {
            tempZ = z < 0 ? --tempZ : ++tempZ;

            if ((generator.GetMapLayout(tempX, tempZ) == SoftwareLevelGenerator.Layout.EMPTY))
            {
                path[count] = new Vector3(tempX - 1, Y, tempZ - 1);
                count++;
            }
            else
            {
                return false;
            }
        }

        // If no collision then run it as a coroutine
        StartCoroutine(Shift(path));
        return true;
    }

    // Moving 1 step in specified direction, return true if the command is valid
    public bool Move(Direction direction)
    {
        // Initialise position and destination vector
        var currentPos = this.GetComponent<IsoTransform>().Position;
        var destPos = Vector3.zero;
        int dx = 0;
        int dz = 0;
        Vector3[] path = new Vector3[2];
        path[0] = currentPos;

        // Check the direction of operation and set relevant flag
        switch (direction)
        {
            case Direction.BottomLeft:
                dx = -1;
                break;
            case Direction.TopLeft:
                dz = 1;
                break;
            case Direction.TopRight:
                dx = 1;
                break;
            case Direction.BottomRight:
                dz = -1;
                break;
        }

        // Check if the tile is empty to allow movement and avoid collisions
        if (generator.GetMapLayout(X + dx, Z + dz) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            return false;
        }
        else
        {
            // The operation is valid
            X += dx;
            Z += dz;

            destPos = new Vector3(currentPos.x + dx, Y, currentPos.z + dz);
            path[1] = destPos;
            StartCoroutine(Shift(path));
            return true;
        }
    }

    // Pickup the item in the specified direction
    public bool PickUpItem(Direction direction)
    {
        if (!hasElement || (carrying == null))
        {
            // Initialise required flags
            var sprite = Resources.Load<Sprite>(HAS_ELEMENT);
            int dx = 0;
            int dz = 0;

            // Check the direction of operation and set relevant flag
            switch (direction)
            {
                case Direction.TopRight:
                    dx = 1;
                    break;
                case Direction.BottomRight:
                    dz = -1;
                    break;
                case Direction.BottomLeft:
                    dx = -1;
                    break;
                case Direction.TopLeft:
                    dz = 1;
                    break;
            }

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
    public bool DropItem(Direction direction)
    {
        if (hasElement || (carrying != null))
        {
            // Initialise required flags
            int dx = 0;
            int dz = 0;
            var sprite = Resources.Load<Sprite>(NO_ELEMENT);

            // Check the direction of operation and set relevant flag
            switch (direction)
            {
                case Direction.TopRight:
                    dx = 1;
                    break;
                case Direction.BottomRight:
                    dz = -1;
                    break;
                case Direction.BottomLeft:
                    dx = -1;
                    break;
                case Direction.TopLeft:
                    dz = 1;
                    break;
            }

            // Check if the target position is empty for dropping the object
            if (generator.GetMapLayout(X + dx, Z + dz) == SoftwareLevelGenerator.Layout.EMPTY)
            {
                generator.SetMapLayout(X + dx, Z + dz, Command.DROP, carrying);
                carrying = null;
                hasElement = false;
                SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                return true;
            }
        }

        return false;
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

                // If time has passed then round cooridinates to nearest node.
                if (timePassed >= maxTimePassed)
                {
                    var pos = isoTransform.Position;
                    pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                    break;
                }
                yield return null;
            }
        }

        // Update the offset coodinates
        X = (int)isoTransform.Position.x + 1;
        Z = (int)isoTransform.Position.z + 1;
    }

    // Used for debugging
    //public void debug()
    //{
    //    generator.PrintMap();
    //}
}