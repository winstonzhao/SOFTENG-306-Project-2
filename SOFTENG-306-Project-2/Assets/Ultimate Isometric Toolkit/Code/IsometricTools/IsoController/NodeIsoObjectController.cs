using UnityEngine;
using System.Collections;
/// <summary>
/// Node based Controller. May use in a Tower Defense game for creeps.
/// </summary>
public class NodeIsoObjectController : AbstractIsoObjectController {
    //units/second
    public float speed = 2f;
    public bool loop = true;
    public Node[] nodes;

    public int nextNode = 0;

    void Start() {
        this.isoObj = GetComponent<IsoObject>();
        moveToNextNode();
    }
	public void moveToNextNode() {

        if (nodes.Length > 0 && nodes[nextNode] != null) {
            var newPos = nodes[nextNode].Position;
            float distance = (isoObj.Position - newPos).magnitude;

            moveTo(newPos, (x) => EasingFunctions.Linear(x, 0, 1, distance / speed), () => {
                if (nextNode < nodes.Length - 1) {
                    nextNode++;
                    moveToNextNode();
                } else {
                    if (loop) {
                        nextNode = 0;
                        moveToNextNode();
                    }
                }

            }, 0, distance / speed);
        }
    }


    private void calcNextNode() {
        if (nextNode < nodes.Length - 1) {
            nextNode++;
        } else {
            if (loop) {
                nextNode = 0;
            }
        }

    }
}
