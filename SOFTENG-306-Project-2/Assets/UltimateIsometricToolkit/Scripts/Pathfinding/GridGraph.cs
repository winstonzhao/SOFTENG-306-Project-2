using System;
using System.Collections.Generic;
using System.Linq;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding {

	/// <summary>
	/// GridGraph
	/// </summary>
	[Serializable,AddComponentMenu("UIT/Pathfinding/GridGraph")]
	public class GridGraph : MonoBehaviour {
		private static readonly Vector2 North = new Vector2(1, 0);
		private static readonly Vector2 South = North * -1;
		private static readonly Vector2 East = new Vector2(0, -1);
		private static readonly Vector2 West = East * -1;
		private static readonly Vector2 NorthEast = North + East;
		private static readonly Vector2 NorthWest = North + West;
		private static readonly Vector2 SouthEast = South + East;
		private static readonly Vector2 SouthWest = South + West;

		private static readonly Vector2[] AdjacentPositions = {
			North,East,South,West,
			NorthEast,SouthEast,SouthWest,NorthWest
		};

		public float MaxScanHeight = 20;
		public bool ShowGraph = false;
		private Dictionary<Vector2, List<Gap>> _gridGraph = new Dictionary<Vector2, List<Gap>>(); 
		public List<IsoTransform> Ignorables = new List<IsoTransform>(); 
		#region Unity Callbacks 
		void Start() {
			UpdateGraph();
		}


		void OnDrawGizmos() {
			if (!ShowGraph)
				return;
			Gizmos.color = Color.red;
			foreach (var gridPos in _gridGraph.Keys) {
				foreach (var gap in _gridGraph[gridPos]) {
					Gizmos.color = gap.Passable ? Color.red : Color.black;
					var center = new Vector3(gridPos.x, (gap.MaxY + gap.MinY)/2, gridPos.y);
					var size = new Vector3(1, gap.MaxY - gap.MinY, 1);
					GizmosExtension.DrawIsoWireCube(center,size);
					foreach (var nextNode in gap.NextNodes) {
						GizmosExtension.DrawIsoArrow(gap.Position,nextNode.Position);
					}
				}
			}
		}
		#endregion
		
		public Gap ClosestNode(Vector3 position) {
			return _gridGraph.SelectMany(kvp=> kvp.Value).OrderBy(n => (position - n.Position).sqrMagnitude).FirstOrDefault();
			//TODO fix for performance reasons
		}

		/// <summary>
		/// Returns all Nodes that are within the (min,max) bounds
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public List<Gap> NodesInBounds(Vector3 min, Vector3 max) {
			if (_gridGraph == null)
				return null;
			min = new Vector3(Mathf.Floor(min.x), min.y, Mathf.Floor(min.z));
			max = new Vector3(Mathf.Floor(max.x), max.y, Mathf.Floor(max.z));
			var gridPositions = new List<Vector2>();
			List<Gap> gaps = new List<Gap>();
			var size = max - min;
			for (var i = 0; i < Mathf.Abs(size.x); i++)
				for (var j = 0; j < Mathf.Abs(size.z); j++)
					gridPositions.Add(new Vector2(min.x + i * Mathf.Sign(size.x), min.z + j * Mathf.Sign(size.z)));
			foreach (var gridPosition in gridPositions) {
					if (_gridGraph.ContainsKey(gridPosition))
						foreach (var gap in _gridGraph[gridPosition]) {
							if(Mathf.Min(gap.MaxY,max.y) - Mathf.Max(gap.MinY,min.y) > 0)
								gaps.Add(gap);
						}
			}
			return gaps;
		} 

		private static Vector2 NodePosToGridPos(Vector3 nodePos) {
			return new Vector2(Mathf.Floor(nodePos.x),Mathf.Floor(nodePos.z));
		}

		public void UpdateGraph() {
			_gridGraph = UpdateGraphInternal(FindObjectsOfType<IsoTransform>().Where(isoT => !Ignorables.Contains(isoT)));
		}
		private Dictionary<Vector2, List<Gap>> UpdateGraphInternal(IEnumerable<IsoTransform> worldObjects, float minAgentHeight = 0.5f) {
			var raster = Rasterize(worldObjects);
			var grid = new Dictionary<Vector2, List<Gap>>();
			
			//calculate nodes
			foreach (var gridPos in raster.Keys) {
				grid.Add(gridPos,CalculateGaps(gridPos,raster[gridPos]));
			}

			foreach (var gridPos in grid.Keys) {
				var adjacentCells = AdjacentPositions.Select(adjacentPosition => adjacentPosition + gridPos).ToArray();

				foreach (var gap in grid[gridPos]) {
					var directAdjacentArcFound = new bool[4];
					//direct adjacent cells
					for (var i = 0; i < 4; i++) {
						List<Gap> otherGaps;
						if (!grid.TryGetValue(adjacentCells[i], out otherGaps))
							continue;
						foreach (var otherGap in otherGaps) {
							if (!otherGap.Visited && gap.Intersect(otherGap) >= minAgentHeight) {
								gap.NextNodes.Add(otherGap);
								otherGap.NextNodes.Add(gap);
								directAdjacentArcFound[i] = true;
							}
						}
					}
					//diagonal adjacent cells
					for (var i = 4; i < 8; i++) {
						//skip if 2 direct adjacent nodes not found
						if (!directAdjacentArcFound[i % 4] || !directAdjacentArcFound[(i - 3) % 4])
							continue;
						List<Gap> otherNodes;
						if (!grid.TryGetValue(adjacentCells[i], out otherNodes))
							continue;
						foreach (var otherNode in otherNodes) {
							if (!otherNode.Visited && gap.Intersect(otherNode) >= minAgentHeight) {
								gap.NextNodes.Add(otherNode);
								otherNode.NextNodes.Add(gap);
							}
						}
					}
					gap.Visited = true;
				}
			}
			return grid;

		}

		/// <summary>
		/// Sorts objects into a xz grid for further processing
		/// </summary>
		/// <param name="worldObjects"></param>
		/// <returns></returns>
		private static Dictionary<Vector2, List<IsoTransform>> Rasterize(IEnumerable<IsoTransform> worldObjects) {
			var grid = new Dictionary<Vector2, List<IsoTransform>>();
			
			foreach (var worldObj in worldObjects) {
				var gridPos = NodePosToGridPos(worldObj.Position);
				if (!grid.ContainsKey(gridPos))
					grid.Add(gridPos, new List<IsoTransform>());
				grid[gridPos].Add(worldObj);
			}
			return grid;
		}

		private List<Gap> CalculateGaps(Vector2 gridPos, List<IsoTransform> worldObjects, float minGapThreshold = 0.1f) {
			var gapList = new List<Gap>();
			worldObjects.Sort((a,b) => a==b ? 0 : a.Min.y > b.Min.y ? 1:-1);
			for (int i = 0; i < worldObjects.Count; i++) {
				var nextHeight = worldObjects[i] == worldObjects.Last() ? MaxScanHeight : worldObjects[i + 1].Min.y;
				if (worldObjects[i].Max.y >= nextHeight)
					continue;
				var gapStart = worldObjects[i].Max.y;
				var gapEnd = nextHeight;
				if(gapEnd-gapStart >= minGapThreshold)
					gapList.Add(new Gap(gapStart,gapEnd,gridPos));
			}

			return gapList;
		}

		public class Gap : INode {

			public bool Visited { get; set; }

			public Vector3 Position {
				get {
					return new Vector3(_gridPos.x, MinY,_gridPos.y);
				}
			}

			public HashSet<INode> NextNodes { get; private set; }

			public bool Passable { get; set; }
		
			public readonly float MinY;
			public readonly float MaxY;
			private readonly Vector2 _gridPos;

			public Gap( float minY, float maxY, Vector2 gridPos) {
				MinY = minY;
				MaxY = maxY;
				_gridPos = gridPos;
				NextNodes = new HashSet<INode>();
				Passable = true;
				Visited = false;
			}

			public float Intersect(Gap other) {
				return Mathf.Min(MaxY, other.MaxY) - Mathf.Max(MinY, other.MinY);
			}
		}
	}
}
