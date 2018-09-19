using UnityEngine;
using System.Collections;

/// <summary>
/// Helper class. Binds all instances as a global interface.
/// </summary>
public class GameSetup : Singleton<GameSetup> {

    public Tile[] tilePrototypes;

    public TurnbasedIsoObjectController playerPrototype;

    GridMap map;
    TurnbasedIsoObjectController player;

    void Awake() {
        GameObject go = new GameObject();
        go.name = "GridMap";
        var map = go.AddComponent<GridMap>();
        map.initTestMap(new Vector3(5,5,3), (x,y,z) => tilePrototypes[z], new Vector3(1,1,.75f));
        instantiatePlayer(new Vector3(0, 0, 3), map.tileSize);
        player.init(map, new Vector3(0, 0, 3));
    }

    void instantiatePlayer(Vector3 mapPos, Vector3 tileSize) {
        GameObject go = (GameObject)GameObject.Instantiate(playerPrototype.gameObject);
        player = go.GetComponent<TurnbasedIsoObjectController>();
        player.isoObj.Position = Vector3.Scale(mapPos, tileSize);
        var z = tileSize.z / 2;
        player.isoObj.Position += new Vector3(0, 0, z);
    }

    
 
	
}
