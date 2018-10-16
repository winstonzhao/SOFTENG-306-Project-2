using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
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
