using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Wrapper to save dictionaries - <see cref="JsonUtility"/> does not support dictionaries.
    ///
    /// Converts the dictionary into a list of key/values.
    /// </summary>
    /// <typeparam name="K">type of keys in the dictionary</typeparam>
    /// <typeparam name="V">type of values in the dictionary</typeparam>
    [Serializable]
    public class SerializableDictionary<K, V>
    {
        [SerializeField]
        private List<K> keys = new List<K>();

        [SerializeField]
        private List<V> values = new List<V>();

        private Dictionary<K, V> value;
        public Dictionary<K, V> Value
        {
            get
            {
                if (value == null)
                {
                    value = new Dictionary<K, V>();
                    // Populate the dictionary
                    for (var i = 0; i < keys.Count; i++)
                    {
                        var key = keys[i];
                        value[key] = values[i];
                    }
                }

                return value;
            }
        }

        public SerializableDictionary(Dictionary<K, V> value)
        {
            foreach (var key in value.Keys)
            {
                keys.Add(key);
                values.Add(value[key]);
            }
        }
    }
}
