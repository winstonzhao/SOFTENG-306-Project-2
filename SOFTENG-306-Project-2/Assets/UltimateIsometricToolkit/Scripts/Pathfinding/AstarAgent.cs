using System;
using System.Collections;
using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding {
	/// <summary>
	/// Astar monobehaviour
	/// </summary>
	[RequireComponent(typeof(IsoTransform)), AddComponentMenu("UIT/Pathfinding/A* Agent")]
	public class AstarAgent : MonoBehaviour {
		public float JumpHeight = 1; //vertical distance threshold to next node
		public float Speed = 2; //units per second
		public GridGraph Graph;
		public Heuristic heuristic;
        public string Type;
		private Animator animator;

		void Awake()
		{
			animator = GetComponent<Animator>();
		}

		/// <summary>
        /// Finds a path to given destination under a heuristic if such path exists
        /// </summary>
        /// <param name="destination"></param>
        public void MoveTo(Vector3 destination) {
			var astar = new Astar(GetFromEnum(heuristic));
			
			var startNode = Graph.ClosestNode(GetComponent<IsoTransform>().Position);
			var endNode = Graph.ClosestNode(destination);
			if (startNode == null) {
				Debug.LogError("Invalid position, no node found close enough to " + GetComponent<IsoTransform>().Position);
				return;
			}
			if (endNode == null) {
				Debug.LogError("Invalid position, no node found close enough to " + destination);
				return;
			}
			astar.SearchPath(startNode, endNode, JumpHeight, path =>
			{
				StopAllCoroutines();
				StartCoroutine(MoveAlongPathInternal(path));
			}, () =>
			{
				Debug.Log("No path found");
			});
		}

		private IEnumerator StepTo(Vector3 from, Vector3 to, float speed) {
			var timePassed = 0f;
			var isoTransform = GetComponent<IsoTransform>();
			var maxTimePassed = Vector3.Distance(from, to) / speed;
			var transition = to - from;
			
			animator.SetBool("NE", transition.x == 1);
			animator.SetBool("NW", transition.z == 1);
			animator.SetBool("SE", transition.z == -1);
			animator.SetBool("SW", transition.x == -1);
			
			while (timePassed + Time.deltaTime < maxTimePassed) {
				timePassed += Time.deltaTime;
				isoTransform.Position = Vector3.Lerp(from, to, timePassed/maxTimePassed);
				yield return null;
			}
		}

		private IEnumerator MoveAlongPathInternal(IEnumerable<Vector3> path) {
			foreach (var pos in path) {
				yield return StepTo(GetComponent<IsoTransform>().Position, pos + new Vector3(0, GetComponent<IsoTransform>().Size.y / 2, 0), Speed);
			}
		}

		private Astar.Heuristic GetFromEnum(Heuristic heuristic) {
			switch (heuristic) {
				case Heuristic.EuclidianDistance:
					return Astar.EuclidianHeuristic;
				case Heuristic.MaxAlongAxis:
					return Astar.MaxAlongAxisHeuristic;
				case Heuristic.ManhattenDistance:
					return Astar.ManhattanHeuristic;
				case Heuristic.AvoidVerticalSteeps:
					return Astar.AvoidVerticalSteepsHeuristic;
				default:
					throw new ArgumentOutOfRangeException("heuristic", heuristic, null);
			}
		}

		public enum Heuristic {
			EuclidianDistance,
			MaxAlongAxis,
			ManhattenDistance,
			AvoidVerticalSteeps
		}
	}
}
