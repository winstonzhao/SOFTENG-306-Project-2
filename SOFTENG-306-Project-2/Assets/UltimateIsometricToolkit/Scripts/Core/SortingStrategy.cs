using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core {

	/// <summary>
	/// Sorting strategy defines how objects are sorted
	/// </summary>
	[Serializable]
	public abstract class SortingStrategy : MonoBehaviour {
		public abstract void Resolve(IsoTransform isoTransform);

		public abstract void Sort();

		public abstract IEnumerable<IsoTransform> Entities { get; }

		public abstract void Remove(IsoTransform isoTransform);

	}
}
