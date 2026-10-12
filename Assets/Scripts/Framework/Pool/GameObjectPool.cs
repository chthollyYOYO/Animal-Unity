using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, Stack<GameObject>> cache = new();

        public GameObject Get(string key, GameObject prefab)
        {
            GameObject go;
            if (cache.TryGetValue(key, out var stack) && stack.Count > 0)
            {
                go = stack.Pop();
            }
            else
            {
                go = Instantiate(prefab);
            }

            go.SetActive(true);
            return go;
        }

        public void Release(string key, GameObject go)
        {
            if (!cache.TryGetValue(key, out var stack))
            {
                stack = cache[key] = new();
            }
            stack.Push(go);

            go.transform.parent = gameObject.transform;
            go.SetActive(false);
        }
    }
}

