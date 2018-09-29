using UnityEngine;
using System.Linq;
using UltimateIsometricToolkit.physics;
using UnityEditor;

public class IsoCleanUp : MonoBehaviour {

	/// <summary>
	/// Unhides artefacts from previous version bug that should be destroyed in editor mode
	/// Previous version had a bug that would leak 'Ghost'objects in a scene. This will unhide all ghosts to be deleted
	/// Delete all ghosts that show up in your hierarchy in editor mode (not during play mode!).
	/// </summary>
	[MenuItem("Tools/UIT/Unhide ghosts artefacts from previous version")]
	static void DoSomething() {
		foreach (var ghost in FindObjectsOfType<Ghost>().ToList()) {
			ghost.gameObject.hideFlags = HideFlags.None;
			ghost.gameObject.name = "Ghost artefact, please delete manually and save scene!";
			
		}
	}
}
