using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public static partial class ConfigManager
    {
        /// <summary>
        /// 此名称开头的静态方法将在管理器初始化时自动调用
        /// </summary>
        public const string INIT_METHOD_NAME_START = "Init_";

        public static bool isInitialized = false;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static async UniTask Initialize()
        {
            if (isInitialized)
                return;
            
            isInitialized = true;
            var type = typeof(ConfigManager);
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                if (method.Name.StartsWith(INIT_METHOD_NAME_START) && method.IsStatic)
                {
                    var result = method.Invoke(null, null);
                    if (result is UniTask task)
                        await task;
                }
            }
        }

        public static async UniTask<TextAsset> LoadConfig(string path)
        {
            TextAsset textAsset = null;
            if (Application.isPlaying)
            {
                textAsset = await ResourceManager.LoadAssetAsync<TextAsset>(path);
            }
            else
            {
#if UNITY_EDITOR
                textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources/{path}.json");
#endif
            }
            return textAsset;
        }
    }
}
