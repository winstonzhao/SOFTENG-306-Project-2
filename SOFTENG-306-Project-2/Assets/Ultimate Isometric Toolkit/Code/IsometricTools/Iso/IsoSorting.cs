using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Sorts all instances of IsoObjects. Lazy intialization done in IsoObject class.
/// </summary>
/// 
[ExecuteInEditMode]
public class IsoSorting : Singleton<IsoSorting> {
	
	int maxDepth = 0;

	// Update is called once per frame
	void LateUpdate () {
		//resort();
		topoSort();
	}
	
	

//	public void resort() {
//		List<IsoObject> objs = FindObjectsOfType<IsoObject>().ToList();
//		objs.Sort();
//		for (int i = 0; i < objs.Count; i++) {
//			objs[i].Depth =  i;
//		}
//	}
	
	public void calcDependencies(List<IsoObject> isoObjs) {

		foreach(IsoObject a in isoObjs) {
            if (a.Ground == null)  {
                a.Ground = a;
            }
			a.Depth = -(int)(a.isoProjection(a.Ground.Position).y * 100) + a.TargetOffset;
			// a.BehindObjects = new List<IsoObject>();
			// foreach(IsoObject b in isoObjs) {
			// 	if(a == b)
			// 		continue;
			// 	if(a.CompareTo(b) >= 1) {
			// 		a.BehindObjects.Add(b);
			// 	}
			// }
			// a.Visited = false;
		}
	}

	//topoligical sort - most recent code
	public void topoSort() {
		List<IsoObject> objs = FindObjectsOfType<IsoObject>().ToList();

		calcDependencies(objs);
		maxDepth = 0;
		// foreach(IsoObject obj in objs) {
		// 	visitNode(obj);
		// }
	}
	
	private void visitNode(IsoObject node) {
		if(!node.Visited)  {
			node.Visited = true;
			foreach(IsoObject nextNode in node.BehindObjects.ToList()) {
				visitNode(nextNode);
				//node.behindObjects.Remove(nextNode);
			}
			node.Depth = maxDepth++;
		}
		
	}
}
