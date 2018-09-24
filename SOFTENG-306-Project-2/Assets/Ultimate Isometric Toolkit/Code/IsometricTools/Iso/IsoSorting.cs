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

	// Update is called once per frame
	void LateUpdate () {
		topoSort();
	}

	public void calcDependencies(List<IsoObject> isoObjs) {

		foreach(IsoObject a in isoObjs) {
            if (a.Ground == null)  {
                a.Ground = a;
            }
			a.Depth = -(int)(a.isoProjection(a.Ground.Position).y * 100) + a.TargetOffset + (int)(a.Ground.Position.z * 100);
		}
	}

	//topoligical sort - most recent code
	public void topoSort() {
		List<IsoObject> objs = FindObjectsOfType<IsoObject>().ToList();

		calcDependencies(objs);
    }
}
