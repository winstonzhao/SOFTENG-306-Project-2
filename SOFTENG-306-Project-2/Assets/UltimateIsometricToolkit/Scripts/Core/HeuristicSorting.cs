using System;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core {
	/// <summary>
	/// Calculates a continuous depth value for each IsoTransform based on its position. Then sorts by depth
	/// </summary>
	[Serializable, AddComponentMenu("UIT/Sorting/Heuristic Sorting")]
	public class HeuristicSorting : SortingStrategy {
		private HashSet<IsoTransform> _entities = new HashSet<IsoTransform>(); 
		
		public override void Resolve(IsoTransform isoTransform) {
			isoTransform.transform.position = Isometric.IsoToUnitySpace(isoTransform.Position);
			_entities.Add(isoTransform);
		}

		public override IEnumerable<IsoTransform> Entities {
			get { return _entities; }
		}

		public override void Remove(IsoTransform isoTransform) {
			_entities.Remove(isoTransform);
		}


		public override void Sort() {
			
		}
	}



}
