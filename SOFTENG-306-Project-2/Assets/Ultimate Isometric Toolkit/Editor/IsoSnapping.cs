using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public class IsoSnapping : EditorWindow {
    //Snap isoObject to multiple of this vector 
    public static Vector3 snappingVector = Vector3.one;
    
    //enable - disable
    public static bool doSnap;

     [MenuItem( "IsoTools/IsometricSnapping %_l" )]

      static void Init()
     {
         var window = (IsoSnapping)EditorWindow.GetWindow(typeof(IsoSnapping));
         window.maxSize = new Vector2( 400, 200 );
     }
 
     public void OnGUI()
     {
         doSnap = EditorGUILayout.Toggle(new GUIContent("Auto Snap", ((doSnap)?"Disable":"Enable") + " automatic snapping for IsoObjects"), doSnap);
         snappingVector = EditorGUILayout.Vector3Field(new GUIContent("Snap Value", "Selection will snap to a closest multilpe in each direction"), snappingVector);
         if (snappingVector.x == 0 || snappingVector.y == 0 || snappingVector.z == 0) {
             doSnap = false;
             EditorGUILayout.HelpBox("Snapping to a multiple of zero not allowed", MessageType.Warning);
         }

         if (GUILayout.Button(new GUIContent("Snap selection", "Snap the current selection in Scene view to the  \n closest multiple of the snapping Vector"))) {
             foreach(GameObject obj in Selection.gameObjects.Where(c => c.GetComponent<IsoObject>() != null).ToList()) {
                 var isoObj = obj.GetComponent<IsoObject>();
                 isoObj.Position = Round(isoObj.Position);
             }
         }

         GUILayout.Space(10);
     }
      
     //Ceils to next multiple of (a must at (0,0,0) (0,y,z) etc.)
     public static Vector3 Ceil( Vector3 input)
     {
         var x =  snappingVector.x * Mathf.Ceil((input.x / snappingVector.x));
         var y = snappingVector.y * Mathf.Ceil((input.y / snappingVector.y));
         var z = snappingVector.z * Mathf.Ceil((input.z / snappingVector.z));

         return new Vector3(x, y, z);
     }

     //Rounds to the next multiple
     public static Vector3 Round(Vector3 input) {
         var x = snappingVector.x * Mathf.Round((input.x / snappingVector.x));
         var y = snappingVector.y * Mathf.Round((input.y / snappingVector.y));
         var z = snappingVector.z * Mathf.Round((input.z / snappingVector.z));

         return new Vector3(x, y, z);
     }

}
