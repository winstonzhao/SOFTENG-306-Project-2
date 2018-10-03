using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Generic 3d Grid Map class. The original GridMap stored Tile Objects which have no additional functionality, but served as a exemplary implementation of a simple datastructure. This caused much confusion.
/// This is a generic version of the grid map to store any type that inherites from IsoObject. Since it is generic it can't be added to any GameObject. But we are talking about datastorage/datastructure.
/// Therefore this isn't a component, but can be wrapped in one.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class GenericGridMap<T> where T : IsoTransform
{
    /// <summary>
    /// Allows to define a function that when called assignes an Instance of T or null to a given position (x,y,z).
    /// Note: You may use this to generate procedual worlds like minecraft.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public delegate T MapPosToT(int x, int y, int z);

    /// <summary>
    /// Allows to define an action to be performed when a tile gets wiped out the datastructure.
    /// Note: You may use this delegate for pooling objects rather than simply destroying object.
    /// </summary>
    /// <param name="t">The instance of T to process</param>
    /// <returns> An action to be invoked</returns>
    public delegate Action onClear(T t);

    /// <summary>
    /// Flattened 3dimensional array. Unity does not serialize multidimensional arrays;
    /// </summary>
    public T[] tiles;

    /// <summary>
    /// Defines the size of a 'cell' in our map (width,height,depth) in unity units
    /// </summary>
    public Vector3 tileSize;

    /// <summary>
    /// The Size of the Grid, where the height is mapSize.y
    /// </summary>
    public Vector3 mapSize;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tileSize"> the size of a tile relative to a cubic unit in unity</param>
    /// <param name="mapSize"></param>
    public GenericGridMap(Vector3 tileSize, Vector3 mapSize)
    {
        this.tileSize = tileSize;
        this.mapSize = mapSize;
        tiles = new T[(int) mapSize.x * (int) mapSize.y * ((int) mapSize.z + 1)];
    }

    /// <summary>
    /// Comfort access via 3 int values
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public T this[int i, int j, int k]
    {
        get
        {
            if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                return null;
            else
                return tiles[(int) (i * mapSize.y + j + k * mapSize.x * mapSize.y)];
        }

        set
        {
            if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                return;
            else
                tiles[(int) (i * mapSize.y + j + k * mapSize.x * mapSize.y)] = value;
        }
    }

    /// <summary>
    /// Comfort access via Vector3
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public T this[Vector3 pos]
    {
        get
        {
            var i = (int) pos.x;
            var j = (int) pos.y;
            var k = (int) pos.z;
            if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                return null;
            else
                return tiles[(int) (i * mapSize.y + j + k * mapSize.x * mapSize.y)];
        }

        set
        {
            var i = (int) pos.x;
            var j = (int) pos.y;
            var k = (int) pos.z;
            if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                return;
            tiles[(int) (i * mapSize.y + j + k * mapSize.x * mapSize.y)] = value;
        }
    }

    /// <summary>
    /// Applies a function to generate worlds.
    /// Note: You may use this function to procedually generate worlds (such as minecraft, chess fields, etc.)
    /// You should call the clear method before.
    /// </summary>
    /// <param name="function">Function MUST provide an new instance, NOT a prototype/prefab of T. It may return null.
    /// This allows to avoid instantiation costs upon calling this function.</param>
    public void applyFunctionToMap(MapPosToT function)
    {
        clear();
        for (int i = 0; i < (int) mapSize.x; i++)
        {
            for (int j = 0; j < (int) mapSize.y; j++)
            {
                for (int k = 0; k < (int) mapSize.z; k++)
                {
                    T functionTile = function(i, j, k);
                    if (functionTile == null) continue;
                    functionTile.Position = Vector3.Scale(new Vector3(i, j, k), tileSize);
                    functionTile.name = "tile_" + i + j + k;
                    this[i, j, k] = functionTile;
                }
            }
        }
    }

    /// <summary>
    /// clears the whole map and destroys all objects stored in it.
    /// </summary>
    public void clear()
    {
        for (int i = 0; i < (int) mapSize.x; i++)
        {
            for (int j = 0; j < (int) mapSize.y; j++)
            {
                for (int k = 0; k < (int) mapSize.z; k++)
                {
                    var obj = this[i, j, k];
                    if (obj != null)
                    {
                        Object.DestroyImmediate(obj.gameObject);
                        this[i, j, k] = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// clears the whole map and performs a callback on all objects stored in it.
    /// Note: You may use this function over clear() to pool your objects rather than destroying them.
    /// </summary>
    /// <param name="onClearCallback">Callback that gets an instance of T and handles further processing (hiding,destruction, pooling,etc.) of that instance.</param>
    public void clear(onClear onClearCallback)
    {
        for (int i = 0; i < (int) mapSize.x; i++)
        {
            for (int j = 0; j < (int) mapSize.y; j++)
            {
                for (int k = 0; k < (int) mapSize.z; k++)
                {
                    onClearCallback(this[i, j, k]).Invoke();
                    this[i, j, k] = null;
                }
            }
        }
    }
}
