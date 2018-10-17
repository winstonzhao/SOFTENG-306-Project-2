using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding
{
    public abstract class RotationUtil : MonoBehaviour
    {
        public abstract Dictionary<DraggableIsoItem.Direction, IsoTransform> GetAdjacentTiles(IsoTransform tile,
            Dictionary<Vector3, IsoTransform> locs);

        public abstract List<IsoTransform> GetInvalidTiles(IsoTransform tile,
            Dictionary<DraggableIsoItem.Direction, IsoTransform> adjacentTiles);
    }
}