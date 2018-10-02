using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;

namespace UltimateIsometricToolkit.examples
{
    /// <summary>
    /// Generates procedual levels
    /// Level generator with ruffness, amplitude and seed value
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        public Vector3 WorldSize = new Vector3(10, 8, 10);

        public int Seed = 1;

        public float Roughness = 1f;

        public float Amplitude = 1f;

        [SerializeField, HideInInspector]
        private GameObject _root;

        /// <summary>
        /// Datastructure to store the IsoObjects in
        /// </summary>
        [SerializeField]
        public GenericGridMap<IsoTransform> Map;

        //prefab to spawn
        public IsoTransform Prefab;

        public void instantiate()
        {
            if (Map != null)
                Clear();
            if (_root == null)
                _root = new GameObject("Level");
            Map = new GenericGridMap<IsoTransform>(Prefab.Size, WorldSize);
            Map.applyFunctionToMap((x, y, z) => MapToTile(x, y, z));
            for (int i = 0; i < Map.tiles.Length; i++)
            {
                if (Map.tiles[i] != null)
                    Map.tiles[i].transform.parent = _root.transform;
            }
        }

        /// <summary>
        /// Wraps GenericGridMap<T>.clear() for the custom editor
        /// </summary>
        public void Clear()
        {
            Map.clear();
            Map = null;
            DestroyImmediate(_root);
        }

        /// <summary>
        /// Returns an instance of prefab or null at a given position (x,y,z)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public IsoTransform MapToTile(int x, int y, int z)
        {
            var vec = new Vector2(x, z) * Roughness + new Vector2(Seed, Seed);
            var height = Mathf.PerlinNoise(vec.x / WorldSize.x, vec.y / WorldSize.y);

            return y <= height * Amplitude ? Instantiate(Prefab) : null;
        }
    }
}
