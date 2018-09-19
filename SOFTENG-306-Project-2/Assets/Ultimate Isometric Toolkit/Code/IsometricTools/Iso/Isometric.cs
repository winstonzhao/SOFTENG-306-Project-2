using UnityEngine;
using System.Collections;

/// <summary>
/// Helper class for isometric projections in common directions
/// </summary>
public class Isometric {

    public static Vector3 vectorToIsoDirection(IsoDirection dir) {
        switch (dir) {
            case IsoDirection.North:
                return Vector3.right;
            case IsoDirection.East:
                return Vector3.down;
            case IsoDirection.South:
                return Vector3.left;
            case IsoDirection.West:
                return Vector3.up;
            case IsoDirection.Up:
                return Vector3.forward;
            case IsoDirection.Down:
                return Vector3.back;
            default:
                return Vector2.zero;
        }

    }

    public static Vector2 isoProjection(Vector3 pt) {
        Vector2 vec = new Vector2(0, 0);
        vec.x = (pt.x - pt.y);
        vec.y = (pt.x + pt.y) / 2;
        vec.y += pt.z;
        return vec;
    }

}
