// Copyright 2016 Marvin Neurath - <marvin.neurath@code-beans.com>

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding {
	/// <summary>
	/// Base node interface with undirected arcs 
	/// </summary>
	public interface INode {
		/// <summary>
		/// The geographical position of the node.
		/// </summary>
		/// <exception cref="ArgumentNullException">Cannot set the Position to null.</exception>
		Vector3 Position { get; }

		/// <summary>
		/// Collection of  outgoing arcs of this node
		/// </summary>
		HashSet<INode> NextNodes { get; }

		/// <summary>
		/// Returns if this node is passable 
		/// </summary>
		bool Passable { get; set; }
	}


}