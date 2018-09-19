using UnityEngine;
using System.Collections;
/// <summary>
/// Simple continuous movement with WSAD/Arrow Keys movement. No collision detection, gravity, etc.
/// </summary>
public class SimpleIsoObjectController : AbstractIsoObjectController {

    public float speed = 10;
    

    new void Update() {
        translate(Isometric.vectorToIsoDirection(IsoDirection.North) * Input.GetAxis("Vertical") * Time.deltaTime * speed);
        translate(Isometric.vectorToIsoDirection(IsoDirection.East) * Input.GetAxis("Horizontal") * Time.deltaTime * speed);
     
    }


    /// <summary>
    /// Translate this Controller in Direction direction.
    /// </summary>
    /// <param name="direction"></param> Real direction. Not a isometric projection.May use Isometric.vectorToIsoDirection to get a proper direction.
    public void translate(Vector3 direction) {
        isoObj.Position += direction;
    }



	
}
