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

        public override List<IsoTransform> GetInvalidTiles(IsoTransform tile,
            Dictionary<DraggableIsoItem.Direction, IsoTransform> adjacentTiles)
        {
            List<IsoTransform> tilesToRemove = new List<IsoTransform>();
            var draggableTile = tile.GetComponentInParent<DraggableIsoItem>();
            var thisTileName = draggableTile.name;
            var thisDir = draggableTile.direction;
            IsoTransform adjTile;
            DraggableIsoItem draggableAdjTile;
            string adjTileName;
            DraggableIsoItem.Direction adjTileDirection;
            // only check road and bridge tiles, the path will already break if the road or bridge adjacent 
            // to the other blocks is invalid
            if (thisTileName == "road" || thisTileName == "bridge")
            {
                if (thisDir == NE || thisDir == SW) // for road or bridge tiles heading in NE or SW direction
                {
                    if (adjacentTiles.TryGetValue(NE, out adjTile)) // get the adjacent tile at NE
                    {
                        draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                        adjTileName = draggableAdjTile.name;
                        adjTileDirection = draggableAdjTile.direction;
                        switch (adjTileName)
                        {
                            case "road":
                            case "bridge":
                                if (adjTileDirection != NE && adjTileDirection != SW) tilesToRemove.Add(tile);
                                break;
                            case "corner":
                                if (adjTileDirection != NE && adjTileDirection != SE) tilesToRemove.Add(tile);
                                break;
                            case "three_way_intersection":
                                if (adjTileDirection == NW) tilesToRemove.Add(tile);
                                break;
                        }
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
                                if (adjTileDirection != NE && adjTileDirection != SW) tilesToRemove.Add(tile);
                                break;
                            case "corner":
                                if (adjTileDirection != SW && adjTileDirection != NW) tilesToRemove.Add(tile);
                                break;
                            case "three_way_intersection":
                                if (adjTileDirection == SE) tilesToRemove.Add(tile);
                                break;
                        }
                    }
                }
                else if (thisDir == SE || thisDir == NW) // for road or bridge tiles heading in SE or NW direction
                {
                    if (adjacentTiles.TryGetValue(SE, out adjTile)) // get the adjacent tile at SE
                    {
                        draggableAdjTile = adjTile.GetComponentInParent<DraggableIsoItem>();
                        adjTileName = draggableAdjTile.name;
                        adjTileDirection = draggableAdjTile.direction;
                        switch (adjTileName)
                        {
                            case "road":
                            case "bridge":
                                if (adjTileDirection != SE && adjTileDirection != NW) tilesToRemove.Add(tile);
                                break;
                            case "corner":
                                if (adjTileDirection != SE && adjTileDirection != SW) tilesToRemove.Add(tile);
                                break;
                            case "three_way_intersection":
                                if (adjTileDirection == NE) tilesToRemove.Add(tile);
                                break;
                        }
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
                                if (adjTileDirection != SE && adjTileDirection != NW) tilesToRemove.Add(tile);
                                break;
                            case "corner":
                                if (adjTileDirection != NE && adjTileDirection != NW) tilesToRemove.Add(tile);
                                break;
                            case "three_way_intersection":
                                if (adjTileDirection == SW) tilesToRemove.Add(tile);
                                break;
                        }
                    }
                }
            }

            return tilesToRemove;
        }
    }
}