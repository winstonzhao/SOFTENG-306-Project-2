using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Wrapper to save lists - <see cref="JsonUtility"/> does not support raw lists.
    /// </summary>
    /// <typeparam name="T">type of values in the list</typeparam>
    [Serializable]
    public class SerializableList<T>
    {
        [SerializeField]
        private List<T> value;
        public List<T> Value
        {
            get { return value; }
        }

        public SerializableList(List<T> list)
        {
            value = list;
        }
    }
}
