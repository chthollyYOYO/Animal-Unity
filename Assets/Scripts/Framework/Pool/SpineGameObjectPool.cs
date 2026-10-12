using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SpineGameObjectPool : MonoSingleton<SpineGameObjectPool>
    {
        private Dictionary<string, Stack<GameObject>> cache = new();

        public GameObject Get(string key, SkeletonDataAsset dataAsset)
        {
            GameObject go;
            if (cache.TryGetValue(key, out var stack) && stack.Count > 0)
            {
                go = stack.Pop();
            }
            else
            {
                go = Helper.Instantiate(dataAsset);
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
