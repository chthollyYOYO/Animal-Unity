using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public class ResourceManager
    {
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源的地址</param>
        public static async UniTask<UnityEngine.Object> LoadAssetAsync(string path)
        {
            var obj = await Resources.LoadAsync(path);
            return obj;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源的地址</param>
        public static async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var obj = await Resources.LoadAsync(path);
            return obj as T;
        }

        public static T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            var obj = Resources.Load<T>(path);
            if (obj == null)
                Debug.LogError($"资源不存在:{path}");
            return obj;
        }
    }
}
