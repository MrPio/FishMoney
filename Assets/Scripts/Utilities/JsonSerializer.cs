﻿using System.IO;
using UnityEngine;

namespace Managers
{
    public class JsonSerializer
    {
        private static JsonSerializer _instance;
        public static JsonSerializer Instance => _instance ??= new JsonSerializer();

        private JsonSerializer()
        {
        }

        public void Serialize(object obj, string dir, string filename)
        {
            var path = Path.Combine(Application.persistentDataPath, dir + "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(path + filename + ".json", JsonUtility.ToJson(obj));
        }

        public T Deserialize<T>(string filePath, T ifNotExist)
        {
            var path = Path.Combine(Application.persistentDataPath, filePath + ".json");
            return !File.Exists(path) ? ifNotExist : JsonUtility.FromJson<T>(File.ReadAllText(path));
        }
    }
}