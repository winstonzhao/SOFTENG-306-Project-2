using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;

[RequireComponent(typeof (IsoTransform))]
public class TouchController : MonoBehaviour {


	// Moves object according to finger movement on the screen
	float speed = 0.1f;

	void Update() {
		if (Input.touchCount != 1 || Input.GetTouch(0).phase != TouchPhase.Moved) return;
		// Get movement of the finger since last frame
		var touchDeltaPosition = Input.GetTouch(0).deltaPosition;

		// Move object across XY plane
		GetComponent<IsoTransform>().Translate(new Vector3(-touchDeltaPosition.x*speed,
			-touchDeltaPosition.y*speed, 0));
	}
}
