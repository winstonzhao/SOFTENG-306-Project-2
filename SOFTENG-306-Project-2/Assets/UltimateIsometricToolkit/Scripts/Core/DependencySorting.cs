using System;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core {
	/// <summary>
	/// Topological Sorting on a dependency graph where nodes are IsoTransforms and arcs define an isBehind relationship
	/// </summary>
	[ExecuteInEditMode,Serializable, AddComponentMenu("UIT/Sorting/Dependency Sorting")]
	public class DependencySorting : SortingStrategy {
		private readonly Dictionary<IsoTransform, SortingState> _isoTransformToSortingState =
			new Dictionary<IsoTransform, SortingState>();

		private readonly List<SortingState> _nodes = new List<SortingState>();

		public override IEnumerable<IsoTransform> Entities {
			get { return _isoTransformToSortingState.Keys; }
		}

		public override void Remove(IsoTransform isoTransform) {
			if (!_isoTransformToSortingState.ContainsKey(isoTransform))
				return;
			_nodes.Remove(_isoTransformToSortingState[isoTransform]);
			_isoTransformToSortingState.Remove(isoTransform);
		}

		public override void Resolve(IsoTransform isoTransform) {
			if (isoTransform == null)
				return;

			//apply xy screen position
			var posXY = Isometric.IsoToUnitySpace(isoTransform.Position);
			isoTransform.transform.position = new Vector3(posXY.x, posXY.y, isoTransform.transform.position.z);

			//add if not exists
			SortingState sortingState;
			if (!_isoTransformToSortingState.TryGetValue(isoTransform, out sortingState)) {
				sortingState = new SortingState(isoTransform);
				_nodes.Add(sortingState);
				_isoTransformToSortingState.Add(isoTransform, sortingState);
			}
			//remove from parent dependencies
			for (int i = 0; i < sortingState.ParentDependencies.Count; i++) {
				sortingState.ParentDependencies[i].ChildDependencies.Remove(sortingState);
			}
			sortingState.ParentDependencies.Clear();

			//remove from child dependencies
			for (int i = 0; i < sortingState.ChildDependencies.Count; i++) {
				var childDependency = sortingState.ChildDependencies[i];
				childDependency.ParentDependencies.Remove(sortingState);
			}
			sortingState.ChildDependencies.Clear();
			sortingState.Hexagon = new Hexagon(isoTransform);
			CalculateDependencies(sortingState);
		}

		private float SetDepthRecursive(SortingState state, float depth) {
			
			//termination
			if (state.Visited)
				return depth;
			state.Visited = true;

			//recursive call
			var result = depth;
			for (int i = 0; i < state.ParentDependencies.Count; i++) {
				var dependency = state.ParentDependencies[i];
				result = SetDepthRecursive(dependency, result);
			}
			//set result
			state.Depth = result;
			return state.Depth + .1f;
		}

		private void PlaceAll() {
			for (int i = 0; i < _nodes.Count; i++) {
				var state = _nodes[i];
				state.Visited = false;
			}

			var depth = 0f;
			//SetDepthIterative();
			for (int i = 0; i < _nodes.Count; i++) {
				var sortingState = _nodes[i];
				depth = SetDepthRecursive(sortingState, depth);
			}
		}


		/// <summary>
		/// Returns if a should be drawn behind b
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool ADependsOnB(IsoTransform a, IsoTransform b) {
			//avoid multiple calculations
			var aMin = a.Min;
			var aMax = a.Max;
			var bMin = b.Min;
			var bMax = b.Max;
			var aMaxBehindbMin = aMax.x > bMin.x && bMax.y > aMin.y && aMax.z > bMin.z;
			var bMaxBehindaMin = bMax.x > aMin.x && aMax.y > bMin.y && bMax.z > aMin.z;

			if (!aMaxBehindbMin || !bMaxBehindaMin)
				return aMaxBehindbMin;
			
			//bounds intersect
			var bMinToAMax = new Vector3(aMax.x - bMin.x, bMax.y - aMin.y, aMax.z - bMin.z);
			var aMinToBMax = new Vector3(bMax.x - aMin.x, aMax.y - bMin.y, bMax.z - aMin.z);

			var dist = bMinToAMax - aMinToBMax;

			var intersection = a.Size + b.Size - new Vector3(Mathf.Abs(dist.x), Mathf.Abs(dist.y), Mathf.Abs(dist.z));
			//find min intersection axis
			if (intersection.x <= intersection.z && intersection.x <= intersection.y)
				return bMinToAMax.x > aMinToBMax.x;
			if (intersection.z <= intersection.x && intersection.z <= intersection.y)
				return bMinToAMax.z > aMinToBMax.z;
			return bMinToAMax.y > aMinToBMax.y;
		}

		
		private void RebuildDependencyGraph() {
			for (int i = 0; i < _nodes.Count; i++) {
				var sortingState = _nodes[i];
				sortingState.ParentDependencies.Clear();
				sortingState.ChildDependencies.Clear();
			}
			for (int i = 0; i < _nodes.Count; i++) {
				var sortingState = _nodes[i];
				Resolve(sortingState.Isotransform);
			}
		}

		public override void Sort() {
			PlaceAll();
		}

		private void CalculateDependencies(SortingState sortingState) {
			for (int i = 0; i < _nodes.Count; i++) {
				var other = _nodes[i];
				if (!sortingState.Hexagon.Overlaps(other.Hexagon))
					continue;
				if (ADependsOnB(sortingState.Isotransform, other.Isotransform)) {
					other.ChildDependencies.Add(sortingState);
					sortingState.ParentDependencies.Add(other);
				} else {
					sortingState.ChildDependencies.Add(other);
					other.ParentDependencies.Add(sortingState);
				}
			}
		}

		private class SortingState {
			public readonly List<SortingState> ParentDependencies = new List<SortingState>();
			public readonly List<SortingState> ChildDependencies = new List<SortingState>();
			public readonly IsoTransform Isotransform;
			public bool Visited;
			public Hexagon Hexagon;
			public float Depth {
				get {return Isotransform.Depth;}
				set { Isotransform.Depth = value; }
			}

		public SortingState(IsoTransform isotransform) {
				Isotransform = isotransform;
				Hexagon = new Hexagon(Isotransform);
			}
		}

		private class Hexagon {
			private Vector2 _min;
			private Vector2 _max;
			private Vector2 _h;

			public Hexagon(IsoTransform isoTransform) {
				var isoMin = isoTransform.Position - isoTransform.Size/2;
				var isoMax = isoTransform.Position + isoTransform.Size/2;
				_min = new Vector2(isoMin.x + isoMin.y, isoMin.z + isoMin.y);
				_max = new Vector2(isoMax.x + isoMax.y, isoMax.z + isoMax.y);
				var left = Isometric.IsoToUnitySpace(isoTransform.Position + new Vector3(-isoTransform.Size.x / 2, -isoTransform.Size.y / 2, isoTransform.Size.z / 2)).x;
				var right = Isometric.IsoToUnitySpace(isoTransform.Position + new Vector3(isoTransform.Size.x / 2, -isoTransform.Size.y / 2, -isoTransform.Size.z / 2)).x;
				_h = new Vector2(left, right);
			}

			public bool Overlaps(Hexagon b) {
				if (this == b)
					return false;
				if(_min.x >= b._max.x || b._min.x >= _max.x)
					return false;
				if (_min.y >= b._max.y || b._min.y >= _max.y)
					return false;
				if (_h.x >= b._h.y || b._h.x >= _h.y)
					return false;
				return true;
			}

		}
	}

}
