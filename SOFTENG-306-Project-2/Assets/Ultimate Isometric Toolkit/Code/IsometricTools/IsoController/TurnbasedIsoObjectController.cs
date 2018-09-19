using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Stores actions (forward,turnRight/Left, jump) in a queue and performs these over time. New Actions may be added during runtime.
/// May use for turnbased games, such as chess,risk, monopoly, LUDO, etc.
/// </summary>
public class TurnbasedIsoObjectController : AbstractIsoObjectController {

    IsoDirection currentForwardDirection = IsoDirection.North;
    public float forwardSpeed = .5f;
    public float turnSpeed = .1f;

    private bool inAction = false;
    Queue<Action> actionQueue = new Queue<Action>();

    [SerializeField]
    GridMap map;

    public Vector3 positionInMap;


    void Update() {
        if (Input.GetKeyDown(KeyCode.W))
            actionQueue.Enqueue(forward);
        if (Input.GetKeyDown(KeyCode.A))
            actionQueue.Enqueue(turnCounterclockWise);
        else if (Input.GetKeyDown(KeyCode.D))
            actionQueue.Enqueue(turnClockwise);
        if (Input.GetKeyDown(KeyCode.Space)) {
            actionQueue.Enqueue(jump);
        }

        if (!inAction && actionQueue.Count > 0) {
            actionQueue.Dequeue().Invoke();
        }
    }


    void forward() {
        if (canMoveForward()) {
            inAction = true;
            var newPos = this.isoObj.Position + Isometric.vectorToIsoDirection(currentForwardDirection);
            positionInMap += Isometric.vectorToIsoDirection(currentForwardDirection);
            moveTo(newPos, (x) => EasingFunctions.QuadEaseOut(x, 0, 1, forwardSpeed), () => {
                inAction = false;
            }, 0, forwardSpeed);
        }
    }

    void turnClockwise() {
        inAction = true;
        waitForSeconds(turnSpeed, () => {
            currentForwardDirection = (IsoDirection)(((int)currentForwardDirection + 1) % 4);
            inAction = false;
        });
    }

    void turnCounterclockWise() {
        var newDirectionValue = ((int)currentForwardDirection - 1);
        if (newDirectionValue < 0)
            newDirectionValue += 4;
        newDirectionValue = newDirectionValue % 4;

        inAction = true;
        waitForSeconds(turnSpeed, () => {
            currentForwardDirection = (IsoDirection)newDirectionValue;
            inAction = false;
        });

    }

    void jump() {
        if (canJumpUp())
            jumpUp();
        else if (canJumpDown())
            jumpDown();
    }

    private void jumpUp() {
        var finalNewPos = posInMapToWorldPos(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Up));
        var tempNewPos = posInMapToWorldPos(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) * 2 / 3f + Isometric.vectorToIsoDirection(IsoDirection.Up) * 4 / 3f);
        inAction = true;
        moveTo(tempNewPos, (x) => EasingFunctions.Linear(x, 0, 1, forwardSpeed * 2 / 3f), () => {
            moveTo(finalNewPos, (x) => EasingFunctions.Linear(x, 0, 1, forwardSpeed * 1 / 3f), () => {
                positionInMap = positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Up);
                inAction = false; 
            }, 0, forwardSpeed * 1 / 3f);
        }, 0, forwardSpeed * 2 / 3f);
    }

    private void jumpDown() {
        var finalNewPos = posInMapToWorldPos(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Down));
        var tempNewPos = posInMapToWorldPos(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) * 1 / 2f + Isometric.vectorToIsoDirection(IsoDirection.Up));
        inAction = true;
        moveTo(tempNewPos, (x) => EasingFunctions.Linear(x, 0, 1, forwardSpeed * 1 / 2f), () => {
            moveTo(finalNewPos, (x) => EasingFunctions.Linear(x, 0, 1, forwardSpeed * 1 / 2f), () => {
                inAction = false;
                positionInMap = positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Down);
            }, 0, forwardSpeed * 1 / 2f);
        }, 0, forwardSpeed * 1 / 2f);
    }

    public void init(GridMap map, Vector3 startPosInMap) {
        this.map = map;
        this.positionInMap = startPosInMap;
    }

    //helper functions
    bool canMoveForward() {
        try {
            return frontTile() == null && frontFloorTile() != null;
        } catch (IndexOutOfRangeException) {
            return false;
        }
    }

    bool canJumpUp() {
        try {
            return topTile() == null && topFrontTile() == null && frontTile() != null;
        } catch (IndexOutOfRangeException) {
            return false;
        }
    }

    bool canJumpDown() {
        try {
            return frontTile() == null && frontFloorTile() == null && lowerFrontFloorTile() != null;
        } catch (IndexOutOfRangeException) {
            return false;
        }
    }

    Tile frontTile() {
        return map.getTile(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection));
    }
    Tile topFrontTile() {
        return map.getTile(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Up));
    }

    Tile topTile() {
        return map.getTile(positionInMap + Isometric.vectorToIsoDirection(IsoDirection.Up));
    }

    Tile frontFloorTile() {
        return map.getTile(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Down));
    }
    Tile lowerFrontFloorTile() {
        return map.getTile(positionInMap + Isometric.vectorToIsoDirection(currentForwardDirection) + Isometric.vectorToIsoDirection(IsoDirection.Down) * 2);
    }

   
    private Vector3 posInMapToWorldPos(Vector3 posInMap) {
        posInMap = Vector3.Scale(posInMap, map.tileSize);
        var z = map.tileSize.z / 2;
        posInMap += new Vector3(0, 0, z);

        return posInMap;
    }

    public void setSpeed(float value) {
        this.forwardSpeed = value;
    }
   
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Isometric.isoProjection(Isometric.vectorToIsoDirection(currentForwardDirection)));
    }

}
