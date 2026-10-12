using FairyGUI;
using System.Linq;

namespace System.Collections.Generic
{
    public static partial class Extension
    {
        public static void HandleList<T>(this List<T> list, Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }

        public static List<T> DeepCopy<T>(this List<T> source, Func<T, T> copyFunc)
        {
            var result = new List<T>();
            foreach (var item in source)
                result.Add(copyFunc(item));
            return result;
        }

        public static T RandomOneElement<T>(this List<T> list, bool remove = false)
        {
            if (list.Count == 0)
                return default;

            var result = list[UnityEngine.Random.Range(0, list.Count)];
            if (remove)
                list.Remove(result);
            return result;
        }

        public static T RandomOneElement<T>(this List<T> list, List<float> weights)
        {
            float sum = weights.Sum();
            float rand = UnityEngine.Random.value * sum;
            float current = 0;
            for (int i = 0; i < list.Count; i++)
            {
                current += weights[i];
                if (current >= rand)
                    return list[i];
            }
            return default;
        }

        public static int RandomOnRange(this List<int> list, bool includeMax = false)
        {
            if (list.Count != 2)
                throw new Exception("RandomOnRange list should have 2 elements");
            return UnityEngine.Random.Range(list[0], list[1] + (includeMax? 1 : 0));
        }

        public static float RandomOnRange(this List<float> list, bool includeMax = false)
        {
            if (list.Count != 2)
                throw new Exception("RandomOnRange list should have 2 elements");
            return UnityEngine.Random.Range(list[0], list[1] + (includeMax ? 1 : 0));
        }

        public static Dictionary<T, int> ToCountDict<T>(this List<T> list)
        {
            Dictionary<T, int> result = new();
            foreach (var item in list)
            {
                if (result.ContainsKey(item))
                    result[item]++;
                else
                    result.Add(item, 1);
            }
            return result;
        }

        public static void HandleArray<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }

        public static void HandleArray<T>(this Array array, Action<T> action)
        {
            foreach (var item in array)
                action((T)item);
        }

        public static void AddRange<T>(this Dictionary<T, int> dict, Dictionary<T, int> other)
        {
            foreach (var item in other)
            {
                if (dict.ContainsKey(item.Key))
                    dict[item.Key] += item.Value;
                else
                    dict.Add(item.Key, item.Value);
            }
        }

        public static void AddOrInsert<T>(this List<T> list, T value, Predicate<T> predicate)
        {
            if (list.Count == 0)
            {
                list.Add(value);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (predicate(list[i]))
                    {
                        list.Insert(i, value);
                        return;
                    }
                }
                list.Add(value);
            }
        }
    }
}
