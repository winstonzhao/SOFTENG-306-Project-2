using UnityEngine;
using System.Collections;

/// <summary>
/// Map used for a TilebasedIsoObjectController
/// </summary>
public class GridMap : MonoBehaviour {

    public delegate Tile MapPosToPrefab(int x, int y, int z);

    //flattened 3dimensional array. Unity does not serialize multidimensional arrays;
    public Tile[] tiles;

    public Vector3 tileSize;
    public Vector3 mapSize;

    //define operators. comfort functions
    public Tile this[int i, int j, int k] {
        get {
            if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                return null;
            else
                return tiles[(int)(i * mapSize.y + j + k * mapSize.x * mapSize.y)];
        }

         set {
             if (i >= mapSize.x || j >= mapSize.y || k >= mapSize.z || i < 0 || j < 0 || k < 0)
                 return;
             else
                 tiles[(int)(i * mapSize.y + j + k * mapSize.x * mapSize.y)] = value;
        }
    }

    public Tile getTile(Vector3 vec) {
        return this[(int)vec.x, (int)vec.y, (int)vec.z];
        
    }

    public void initTestMap(Vector3 mapSize, MapPosToPrefab function, Vector3 tileSize) {
        this.tiles = new Tile[(int)mapSize.x* (int)mapSize.y* ((int)mapSize.z + 1)];
        this.mapSize = mapSize;
        this.tileSize = tileSize;
        for (int i = 0; i < (int)mapSize.x; i++) {
            for (int j = 0; j < (int)mapSize.y; j++) {
                for (int k = 0; k < (int)mapSize.z; k++) {
					Tile functionTile = function(i,j,k);
					if(functionTile != null) {
	                    GameObject go =(GameObject) GameObject.Instantiate(functionTile.gameObject, Vector3.zero, Quaternion.identity);
	                    Tile t = go.GetComponent<Tile>();
	                    t.Position = Vector3.Scale(new Vector3(i,j,k), tileSize);
	                    go.name = "tile_" + i + j + k;
	                    this[i, j, k] = t;
					}
                }
            }
        }
    }

    public TurnbasedIsoObjectController instantiatePlayer(Vector3 pos, TurnbasedIsoObjectController playerPrototype) {
        GameObject go = (GameObject)GameObject.Instantiate(playerPrototype.gameObject);
        var player = go.GetComponent<TurnbasedIsoObjectController>();
        player.isoObj.Position = Vector3.Scale(pos, tileSize);
        var z = tileSize.z /2;
        player.isoObj.Position += new Vector3(0,0,z);

        return player;

    }

    
	
}
