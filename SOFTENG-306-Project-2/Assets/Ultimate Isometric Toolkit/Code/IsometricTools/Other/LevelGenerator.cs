using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public Vector3 size = new Vector3(10,10,15);
	public Vector3 tileSize = new Vector3(1,1,.68f);

	public int seed = 1;
	public float ruffness = 1f;
	public float amplitude = 1f;

	public GridMap map	;

	public Tile grassTile;



	public void instantiate() {
		if(map != null) {
			reset(map);
			map.initTestMap(size, (x,y,z) => mapToTile(x,y,z), tileSize);
		}
	}

	public Tile mapToTile(int x, int y, int z) {
		Vector2 vec = new Vector2(x,y) * ruffness + new Vector2(seed, seed);
		float height = Mathf.PerlinNoise(vec.x/size.x, vec.y/size.y);

		if( z <= height * amplitude) {
			return grassTile;
		} else {
			return null;
		}
	}

	public void reset( GridMap map) {
		for (int i = 0; i < map.mapSize.x; i++) {
			for (int j = 0; j < map.mapSize.y; j++) {
				for (int k = 0; k < map.mapSize.z; k++) {
					if(map[i,j,k] != null)
						GameObject.DestroyImmediate(map[i,j,k].gameObject);
				}
			}
		}
	}

	public void setSize(float value) {
		size = new Vector3((int)value,(int)value,(int)value);
	}

	public void setRuffness(float value) {
		ruffness = value;
	}

	public void setAmplitude(float value) {
		amplitude = value;
	}
}
