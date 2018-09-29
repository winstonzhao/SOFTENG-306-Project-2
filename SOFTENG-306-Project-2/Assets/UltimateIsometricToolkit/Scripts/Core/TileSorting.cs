using System;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core {
	/// <summary>
	/// Sorts IsoTransforms into discrete 1x1 unit tiles on the XZ plane. Then sorts by height in each tile
	/// </summary>
	[Serializable,AddComponentMenu("UIT/Sorting/Tiled Sorting")]
	public class TileSorting : SortingStrategy {
		private readonly Dictionary<int,List<IsoTransform>> _tileDictionary = new Dictionary<int, List<IsoTransform>>();  
		private readonly Dictionary<IsoTransform,int> _inverseTileDictionary = new Dictionary<IsoTransform, int>();

		readonly List<int> _sortedIndices = new List<int>(); 
		private readonly YComparer _yComparer = new YComparer();
		
		public override void Resolve(IsoTransform isoTransform) {
			Remove(isoTransform);
			var xyPos = Isometric.IsoToUnitySpace(isoTransform.Position);
			
			isoTransform.transform.position = new Vector3(xyPos.x,xyPos.y,isoTransform.transform.position.z);
			
			var index = GetTile(isoTransform);
			int insertIndex;
			if (!_tileDictionary.ContainsKey(index)) {
				_tileDictionary[index] = new List<IsoTransform>();
				insertIndex = ~_sortedIndices.BinarySearch(index);
				_sortedIndices.Insert(insertIndex,index);
			}
			insertIndex = _tileDictionary[index].BinarySearch(isoTransform,_yComparer);
			if (insertIndex < 0)
				insertIndex = ~insertIndex;
			_tileDictionary[index].Insert(insertIndex,isoTransform);
			_inverseTileDictionary.Add(isoTransform,index);
		}

		public override void Sort() {
			float depth = 0;
			for (int i = 0; i < _sortedIndices.Count; i++) {
				var index = _sortedIndices[i];
				for (int j = 0; j < _tileDictionary[index].Count; j++) {
					var isoTransform = _tileDictionary[index][j];
					isoTransform.Depth = depth;
					depth += .1f;
				}
			}
		}

		public override IEnumerable<IsoTransform> Entities {
			get { return _inverseTileDictionary.Keys; }
		}

		public override void Remove(IsoTransform isoTransform) {
			int tileindex;
			if (!_inverseTileDictionary.TryGetValue(isoTransform, out tileindex))
				return;
			_tileDictionary[tileindex].Remove(isoTransform);
			_inverseTileDictionary.Remove(isoTransform);
		}

		private int GetTile(IsoTransform isoTransform) {
			return Mathf.FloorToInt(isoTransform.Min.x + .5f) + Mathf.FloorToInt(isoTransform.Min.z + .5f);
		}

		private class YComparer : IComparer<IsoTransform> {
			public int Compare(IsoTransform x, IsoTransform y) {
				return x.Min.y <= y.Min.y ? 1 : -1;
			}
		}

	}
}
