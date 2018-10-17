using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEngine;

namespace Civil_Mini_Game
{
    public class CivilRotationUtil : RotationUtil
    {
        static DraggableIsoItem.Direction NE = DraggableIsoItem.Direction.NE;
        static DraggableIsoItem.Direction SE = DraggableIsoItem.Direction.SE;
        static DraggableIsoItem.Direction SW = DraggableIsoItem.Direction.SW;
        static DraggableIsoItem.Direction NW = DraggableIsoItem.Direction.NW;

        /**
         * Get all four tiles adjacent to the given tile in four directions.
         */
        public override Dictionary<DraggableIsoItem.Direction, IsoTransform> GetAdjacentTiles(IsoTransform tile,
            Dictionary<Vector3, IsoTransform> locs)
        {
            // Get the tiles adjacent to this tile on the x axis and store their corresponding direction
            var adjacentTiles = new Dictionary<DraggableIsoItem.Direction, IsoTransform>();
            for (int x = -1; x <= 1; x += 2)
            {
                IsoTransform adjacent;
                if (locs.TryGetValue(new Vector3(tile.Position.x + x, tile.Position.y, tile.Position.z), out adjacent))
                {
                    DraggableIsoItem.Direction direction;
                    if (x == -1)
                    {
                        //direction = DraggableIsoItem.Direction.SW;
                        direction = SW;
                    }
                    else
                    {
                        //direction = DraggableIsoItem.Direction.NE;
                        direction = NE;
                    }

                    adjacentTiles.Add(direction, adjacent);
                }
            }

            // Get the tiles adjacent to this tile on the z axis and store their corresponding direction
            for (int z = -1; z <= 1; z += 2)
            {
                IsoTransform adjacent;
                if (locs.TryGetValue(new Vector3(tile.Position.x, tile.Position.y, tile.Position.z + z), out adjacent))
                {
                    DraggableIsoItem.Direction direction;
                    if (z == -1)
                    {
                        direction = SE;
                    }
                    else
                    {
                        direction = NW;
                    }

                    adjacentTiles.Add(direction, adjacent);
                }
            }

            return adjacentTiles;
        }

        /**
         * Get the invalid tiles (those ones that rotate in the wrong directions) with the given tile and the
         * given adjacent tiles.
         */
        public override List<IsoTransform> GetInvalidTiles(IsoTransform tile,
            Dictionary<DraggableIsoItem.Direction, IsoTransform> adjacentTiles)
        {
            List<IsoTransform> tilesToRemove = new List<IsoTransform>();

            // get information of the current tile we are looking at
            var draggableTile = tile.GetComponentInParent<DraggableIsoItem>();
            var thisTileName = draggableTile.name; // name of the tile (road, bridge, etc.)
            var thisDir = draggableTile.direction; // direction of the tile

            // check validness of the adjacent tiles one by one
            IsoTransform adjTile;
            DraggableIsoItem draggableAdjTile;
            string adjTileName;
            DraggableIsoItem.Direction adjTileDirection;
            // only check road and bridge tiles, the path will already break if the road or bridge adjacent 
            // to the other blocks is invalid
            if (thisTileName != "road" && thisTileName != "bridge") return tilesToRemove;
            if (thisDir == NE || thisDir == SW) // for road or bridge tiles heading in NE or SW direction
            {
                var noAdjAtNE = false;
                var noAdjAtSW = false;
                if (adjacentTiles.TryGetValue(NE, out adjTile)) // get the adjacent tile at NE
                {
                    draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                    adjTileName = draggableAdjTile.name;
                    adjTileDirection = draggableAdjTile.direction;
                    switch (adjTileName)
                    {
                        case "road":
                        case "bridge":
                            // the road or bridge at the NE should be directing NE or SW
                            if (adjTileDirection != NE && adjTileDirection != SW) tilesToRemove.Add(tile);
                            break;
                        case "corner":
                            // the corner at NE should be direction NE or SE
                            if (adjTileDirection != NE && adjTileDirection != SE) tilesToRemove.Add(tile);
                            break;
                        case "three_way_intersection":
                            // the three way intersection at NE should not face NW
                            if (adjTileDirection == NW) tilesToRemove.Add(tile);
                            break;
                    }
                }
                else
                {
                    noAdjAtNE = true;
                }

                if (adjacentTiles.TryGetValue(SW, out adjTile)) // get the adjacent tile at SW
                {
                    draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                    adjTileName = draggableAdjTile.name;
                    adjTileDirection = draggableAdjTile.direction;
                    switch (adjTileName)
                    {
                        case "road":
                        case "bridge":
                            // the road or bridge at the SW should be directing NE or SW
                            if (adjTileDirection != NE && adjTileDirection != SW) tilesToRemove.Add(tile);
                            break;
                        case "corner":
                            // the corner at SW should be direction SW or NE
                            if (adjTileDirection != SW && adjTileDirection != NW) tilesToRemove.Add(tile);
                            break;
                        case "three_way_intersection":
                            // the three way intersection at SW should not face SE
                            if (adjTileDirection == SE) tilesToRemove.Add(tile);
                            break;
                    }
                }
                else
                {
                    noAdjAtSW = true;
                }

                if (noAdjAtNE && noAdjAtSW)
                {
                    // if the road or bridge does not have adjacent tile on both ends, it is certainly not traversable
                    tilesToRemove.Add(tile);
                }
            }
            else if (thisDir == SE || thisDir == NW) // for road or bridge tiles heading in SE or NW direction
            {
                var noAdjAtSE = false;
                var noAdjAtNW = false;
                if (adjacentTiles.TryGetValue(SE, out adjTile)) // get the adjacent tile at SE
                {
                    draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                    adjTileName = draggableAdjTile.name;
                    adjTileDirection = draggableAdjTile.direction;
                    switch (adjTileName)
                    {
                        case "road":
                        case "bridge":
                            // the road or bridge at the SE should be directing SE or NW
                            if (adjTileDirection != SE && adjTileDirection != NW) tilesToRemove.Add(tile);
                            break;
                        case "corner":
                            // the corner at SE should be direction SE or SW
                            if (adjTileDirection != SE && adjTileDirection != SW) tilesToRemove.Add(tile);
                            break;
                        case "three_way_intersection":
                            // the three way intersection at SE should not face NE
                            if (adjTileDirection == NE) tilesToRemove.Add(tile);
                            break;
                    }
                }
                else
                {
                    noAdjAtSE = true;
                }

                if (adjacentTiles.TryGetValue(NW, out adjTile)) // get the adjacent tile at NW
                {
                    draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                    adjTileName = draggableAdjTile.name;
                    adjTileDirection = draggableAdjTile.direction;
                    switch (adjTileName)
                    {
                        case "road":
                        case "bridge":
                            // the road or bridge at the NW should be directing SE or NW
                            if (adjTileDirection != SE && adjTileDirection != NW) tilesToRemove.Add(tile);
                            break;
                        case "corner":
                            // the corner at NW should be direction NE or NW
                            if (adjTileDirection != NE && adjTileDirection != NW) tilesToRemove.Add(tile);
                            break;
                        case "three_way_intersection":
                            // the three way intersection at NW should not face SW
                            if (adjTileDirection == SW) tilesToRemove.Add(tile);
                            break;
                    }
                }
                else
                {
                    noAdjAtNW = true;
                }

                if (noAdjAtSE && noAdjAtNW)
                {
                    // if the road or bridge does not have adjacent tile on both ends, it is certainly not traversable
                    tilesToRemove.Add(tile);
                }
            }

            return tilesToRemove;
        }
    }
}