using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Common
{
    public static class DataService
    {
        private static JsonSerializer m_JsonSerializer;
        private static JsonSerializer JsonSerializer => m_JsonSerializer ??= new JsonSerializer();

        public static void Save<T>(T data, string fileName = null) where T : new()
        {
            var filePath = BuildPath<T>(fileName);
            using var streamWriter = new StreamWriter(filePath);
            streamWriter.AutoFlush = true;
            JsonSerializer.Serialize(streamWriter, data);
        }

        public static T Load<T>(string fileName = null) where T : new()
        {
            var filePath = BuildPath<T>(fileName);
            T data;
            if (File.Exists(filePath))
            {
                try
                {
                    var fileStream = File.OpenRead(filePath);
                    using var streamReader = new StreamReader(fileStream);
                    using var jsonTextReader = new JsonTextReader(streamReader);
                    data = JsonSerializer.Deserialize<T>(jsonTextReader);

                    if (!EqualityComparer<T>.Default.Equals(data, default))
                    {
                        return data;
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Exception on load file ({filePath})\n{exception.Message}");
                    Remove<T>();
                }
            }

            data = new T();
            return data;
        }

        public static void Remove<T>(string fileName = null, T file = default) where T : new()
        {
            var filePath = BuildPath<T>(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string BuildPath<T>(string name = null)
        {
            return Path.Combine(Application.persistentDataPath, string.IsNullOrWhiteSpace(name) ? $"{typeof(T).Name}" : name);
        }
    }
}