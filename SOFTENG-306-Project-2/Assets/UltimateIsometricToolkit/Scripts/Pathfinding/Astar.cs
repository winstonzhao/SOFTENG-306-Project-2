using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding {
	public class Astar {
		
		#region Heuristics
		/// <summary>
		/// A Function that associates a value to the transition to the target node.
		/// The next node to evaluate is always the one with minimum heuristic value from all options available.
		/// For astar to terminate constrain is that the target node of a path must always have minimum heuristic value
		/// </summary>
		public delegate float Heuristic(INode nodeToEvaluate, INode targetNode);
		
		/// <summary>
		/// Euclidian distance. More resource intensive due to sqrt operation. 
		/// </summary>
		public static Heuristic EuclidianHeuristic { get { return (evaluate, target) => Vector3.Distance(evaluate.Position, target.Position); } }

		/// <summary>
		/// Maximum distance. Maximum of absolute vector components. 
		/// </summary>
		public static Heuristic MaxAlongAxisHeuristic {
			get {
				return (evaluate, target) => {
					var dist = evaluate.Position - target.Position;
					dist = new Vector3(Mathf.Abs(dist.x), Mathf.Abs(dist.y), Mathf.Abs(dist.z));
					return Mathf.Max(dist.x, dist.y, dist.z);
				};
			}
		}

		private readonly Heuristic _heuristic;
		private readonly float _heuristicWeight;
		
		/// <summary>
		/// Manhatten distance. Sum of all absolute vector components
		/// </summary>
		public static Heuristic ManhattanHeuristic {
			get {
				return (evaluate, target) => {
					var dist = evaluate.Position - target.Position;
					dist = new Vector3(Mathf.Abs(dist.x), Mathf.Abs(dist.y), Mathf.Abs(dist.z));
					return dist.x + dist.y + dist.z;
				};
			}
		}
		/// <summary>
		/// Vertical distance. Returns the Vertical distance
		/// </summary>
		public static Heuristic AvoidVerticalSteepsHeuristic {
			get { return (evaluate, target) => Mathf.Abs(evaluate.Position.y - target.Position.y); }
		}
#endregion

		private readonly EventProcessor _eventProcessor;

		public Astar(Heuristic heuristic) {
			_heuristic = heuristic;
			_eventProcessor = EventProcessor.Instance;
		}

		private void Init(INode startNode, INode endNode, float maxVerticalDist) {
			if (startNode == null || endNode == null)
				throw new ArgumentNullException();
			target = endNode;
			_maxVerticalDist = maxVerticalDist;
			_finalState = null;
			closed.Clear();
			open.Clear();
			var initialState = new NodeState(startNode);
			foreach (var nextNode in startNode.NextNodes) {
				NodeToState.Add(nextNode, new NodeState(nextNode, initialState));
				open.Add(nextNode);
			}
			closed.Add(startNode);
		}
		
		public void SearchPath(INode startNode, INode endNode, float maxVerticalDistance, Action<List<Vector3>> successCallback, Action failureCallback = null) {
			Init(startNode,endNode, maxVerticalDistance);
			new Thread(() => {
				while (Next()) { }
				if (_finalState == null) {
					if (failureCallback != null)
						_eventProcessor.QueueEvent(failureCallback);
				} else {
					var invertedpath = new List<Vector3>();
					var path = _finalState;
					while (path != null) {
						invertedpath.Add(path.Node.Position);
						path = path.Previous;
					}
					invertedpath.Reverse();
					open.Clear();
					closed.Clear();

					_eventProcessor.QueueEvent(() =>successCallback(invertedpath));
				}
			}).Start();
		}
		private INode target;
		private HashSet<INode> closed = new HashSet<INode>(); 
		//private List<INode> open = new List<INode>(); 
		private Dictionary<INode, NodeState> NodeToState = new Dictionary<INode, NodeState>(); 
		private List<INode> open = new List<INode>(); 
		/// <summary>
		/// Evaluate the heuristic estimated cost from node n to the target
		/// </summary>
		/// <param name="node">given node</param>
		/// <param name="cost">cost to reach given node</param>
		/// <returns></returns>
		private float Eval(INode node, float cost) {
			//return _heuristicWeight * cost + (1 - cost) * _heuristic(node, target); 
			return cost + _heuristic(node, target);
		}

		private NodeState _finalState;
		private bool _searchStarted;
		private float _maxVerticalDist;

		private bool Next() {
			if (open.Count == 0)
				return false;
			
			var best = open.First();
			//use heuristic to figure out our best option
			foreach (var node in open) {
				if (Eval(node, NodeToState[node].Cost) < Eval(best, NodeToState[best].Cost))
					best = node;
			}
			//we have reached our target node, terminate
			if (best == target) {
				_finalState = NodeToState[best];
				return false;
			}
			Search(best); //evaluate all new options from our best node and update costs
			return open.Count > 0; //we have options left (if 0 no path could be found)
		}

		/// <summary>
		/// Adds all next nodes from given node to open list, if not already visted or open already and updates costs if necessary
		/// </summary>
		/// <param name="node"></param>
		private void Search(INode node) {
			closed.Add(node);
			var state = NodeToState[node];
			open.Remove(node);
			foreach (var nextNode in node.NextNodes) {
				if(!nextNode.Passable || closed.Contains(nextNode) || Mathf.Abs(node.Position.y - nextNode.Position.y) > _maxVerticalDist)
					continue;
				//add if not exists
				if(!NodeToState.ContainsKey(nextNode))
					NodeToState.Add(nextNode,new NodeState(nextNode,state));
				if(!open.Contains(nextNode))
					open.Add(nextNode);
				//update costs if neccessary
				if (state.Cost + Vector3.Distance(nextNode.Position, node.Position) < NodeToState[nextNode].Cost)
					NodeToState[nextNode].Previous = state;
			}
			
		}

		/// <summary>
		/// State of a node 
		/// </summary>
		private class NodeState {
			/// <summary>
			/// Best predecessor at a given time during astar search
			/// </summary>
			public NodeState Previous {
				get { return _previous; }
				set {
					if (value == null) {
						Cost = 0;
						return;
					}
					if (_previous == value)
						return;
					_previous = value;
					Cost = _previous.Cost + Vector3.Distance(_previous.Node.Position, Node.Position);
					
				}
			}
			/// <summary>
			/// Associated Node
			/// </summary>
			public INode Node;
			/// <summary>
			/// Total cost (running sum) to get to this Node from a given start node
			/// </summary>
			public float Cost;
			private NodeState _previous;

			public NodeState(INode node, NodeState previous = null) {
				Node = node;
				Previous = previous;
				
			}
		} 
 
	
	}
}
