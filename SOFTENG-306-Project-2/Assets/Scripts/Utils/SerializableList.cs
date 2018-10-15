using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
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
