using UnityEngine;
using System.IO;

namespace _Scripts.Extensions.ScriptableObjectExtensions
{

    public static class ScriptableObjectSerializer<T> where T : ScriptableObject
    {
        // Serialize a ScriptableObject to JSON
        public static string Serialize(T scriptableObject)
        {
            return JsonUtility.ToJson(scriptableObject, true);
        }

        // Deserialize JSON to a ScriptableObject
        public static T Deserialize(string json)
        {
            T scriptableObject = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(json, scriptableObject);
            return scriptableObject;
        }

        // Save a ScriptableObject to a file
        public static void SaveToFile(T scriptableObject, string filePath)
        {
            string json = Serialize(scriptableObject);
            File.WriteAllText(filePath, json);
        }

        // Load a ScriptableObject from a file
        public static T LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return Deserialize(json);
            }
            return null;
        }
    }

}
