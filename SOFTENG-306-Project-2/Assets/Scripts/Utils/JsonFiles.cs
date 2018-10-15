using System;
using System.Collections.Generic;
using System.IO;
using Quests;
using UnityEngine;

namespace Utils
{
    public class JsonFiles : Singleton<JsonFiles>
    {
        private string PersistentDataPath;

        private void Awake()
        {
            PersistentDataPath = Application.persistentDataPath;
        }

        public void WriteList<T>(string path, List<T> data)
        {
            var serializable = new SerializableList<T>(data);
            Write(path, serializable);
        }

        public List<T> ReadList<T>(string path)
        {
            var serialized = Read(path, input => input);
            var wrapper = JsonUtility.FromJson<SerializableList<T>>(serialized);
            return wrapper != null ? wrapper.Value : null;
        }

        public void Write<T>(string path, T data, Converter<T, string> serializer = null)
        {
            var file = Path.Combine(PersistentDataPath, path);

            var serialized = serializer != null ? serializer(data) : JsonUtility.ToJson(data);

            File.WriteAllText(file, serialized);
        }

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
