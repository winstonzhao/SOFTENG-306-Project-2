using System;
using System.Collections.Generic;
using System.IO;
using Quests;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Helper for JSON file persistence
    /// </summary>
    public class JsonFiles : Singleton<JsonFiles>
    {
        private string PersistentDataPath;

        private void Awake()
        {
            PersistentDataPath = Application.persistentDataPath;
        }

        /// <summary>
        /// Write the <see cref="data"/> list to the given <see cref="path"/>
        /// </summary>
        /// <param name="path">the path to save into</param>
        /// <param name="data">the data to save</param>
        /// <typeparam name="T">the type of data in the list</typeparam>
        public void WriteList<T>(string path, List<T> data)
        {
            var serializable = new SerializableList<T>(data);
            Write(path, serializable);
        }

        /// <summary>
        /// <see cref="Read{T}"/>
        /// </summary>
        /// <param name="path">the path to save into</param>
        /// <typeparam name="T">type of value of the list</typeparam>
        /// <returns>the list read or null</returns>
        public List<T> ReadList<T>(string path)
        {
            var serialized = Read(path, input => input);
            var wrapper = JsonUtility.FromJson<SerializableList<T>>(serialized);
            return wrapper != null ? wrapper.Value : null;
        }

        /// <summary>
        /// Write the <see cref="data"/> dictionary to the given <see cref="path"/>
        /// </summary>
        /// <param name="path">the path to save into</param>
        /// <param name="data">the data to save</param>
        /// <typeparam name="K">the type of keys in the dictionary</typeparam>
        /// <typeparam name="V">the type of values in the dictionary</typeparam>
        public void WriteDictionary<K, V>(string path, Dictionary<K, V> data)
        {
            var serializable = new SerializableDictionary<K, V>(data);
            Write(path, serializable);
        }

        /// <summary>
        /// <see cref="Read{T}"/>
        /// </summary>
        /// <param name="path">the path to save into</param>
        /// <typeparam name="K">type of key of the dictionary</typeparam>
        /// <typeparam name="V">type of value of the dictionary</typeparam>
        /// <returns>the dictionary read or null</returns>
        public Dictionary<K, V> ReadDictionary<K, V>(string path)
        {
            var serialized = Read(path, input => input);
            var wrapper = JsonUtility.FromJson<SerializableDictionary<K, V>>(serialized);
            return wrapper != null ? wrapper.Value : null;
        }

        /// <summary>
        /// Serializes & writes the given object to the given <see cref="path"/>
        /// </summary>
        /// <param name="path">the path to save into</param>
        /// <param name="data">the data to save</param>
        /// <param name="serializer">optional custom serializer from type <see cref="T"/> to string</param>
        /// <typeparam name="T">the type of data to serialize</typeparam>
        public void Write<T>(string path, T data, Converter<T, string> serializer = null)
        {
            var file = Path.Combine(PersistentDataPath, path);

            var serialized = serializer != null ? serializer(data) : JsonUtility.ToJson(data);

            File.WriteAllText(file, serialized);
        }

        /// <summary>
        /// Read the file at the given <see cref="path"/> and deserialize it into the given type <see cref="T"/>
        /// </summary>
        /// <param name="path">the path of the file to read</param>
        /// <param name="deserializer">optional custom deserializer from string to type <see cref="T"/></param>
        /// <typeparam name="T">the type to deserialize into</typeparam>
        /// <returns>the object that was read from the path or null if nothing was found</returns>
        public T Read<T>(string path, Converter<string, T> deserializer = null)
        {
            var file = Path.Combine(PersistentDataPath, path);

            if (!File.Exists(file))
            {
                return default(T);
            }

            var serialized = File.ReadAllText(file);

            return deserializer != null ? deserializer(serialized) : JsonUtility.FromJson<T>(serialized);
        }
    }
}
