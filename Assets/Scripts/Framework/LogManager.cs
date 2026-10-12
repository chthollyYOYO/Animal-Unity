using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class LogManager
    {
        private static Dictionary<string, float> timeDict = new();

        public static void Info(string message)
        {
            Debug.Log(message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public static void Error(string message)
        {
            Debug.LogError(message);
        }

        public static void SaveTime(string message)
        {
            timeDict[message] = Time.realtimeSinceStartup;
        }

        public static void ReadTime(string message)
        {
            if (timeDict.TryGetValue(message, out float time))
                Debug.Log($"{message}: {Time.realtimeSinceStartup - time}s");
        }
    }
}
