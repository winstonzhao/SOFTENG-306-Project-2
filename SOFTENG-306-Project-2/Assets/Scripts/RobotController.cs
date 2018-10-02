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
    private int X = 1;
    private int Y = 1;
    private int Z = 1;

    // Default speed for transforming the robot
    private float speed = 5;

    // Boolean and object to see if the robot is currently holding an object
    private bool hasElement = false;
    private GameObject carrying;

    // Fields to animating the robot transformation
    private Vector3 from;
    private Vector3 to;
    private float timePassed;
    private float maxTimePassed;

    // Checking if the last action has been sucessfulyl performed
    public bool IsFinished {
        get  {
            return timePassed >= maxTimePassed;
        }
    }

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
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft
    }

    // Initialization
    void Start()
    {
        isoTransform = this.GetOrAddComponent<IsoTransform>();
        generator = gameObject.GetComponentInParent(typeof(SoftwareLevelGenerator)) as SoftwareLevelGenerator;
    }

    // Update is called once per frame
    void Update()
    {
        if (from != Vector3.zero && to != Vector3.zero) 
        {
            timePassed += Time.deltaTime;
            isoTransform.Position = Vector3.Lerp(from, to, timePassed / maxTimePassed);
        }
    }

    // Moving 1 step in specified direction, return true if the command is valid
    public bool Move(Direction direction) {
        // Initialise position and destination vector
        var currentPos = this.GetComponent<IsoTransform>().Position;
        var destPos = Vector3.zero;
        int dx = 0;
        int dz = 0;

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
            Shift(currentPos, destPos);
            return true;
        }
    }



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
    private void Shift(Vector3 from, Vector3 to)
    {
        timePassed = 0f;
        maxTimePassed = Vector3.Distance(from, to) / speed;
        this.from = from;
        this.to = to;
    }

    // Used for debugging
    //public void debug()
    //{
    //    generator.PrintMap();
    //}
}