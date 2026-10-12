using FairyGUI;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    public class UIRegisterInfoManager : Singleton<UIRegisterInfoManager>
    {
        private readonly Dictionary<string, UIRegisterInfo> infos = new();

        /// <summary>
        /// 注册全部(反射)
        /// </summary>
        public void RegisterAll()
        {
            var baseType = typeof(GComponent);
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace == UISystem.UI_GCOMPONENT_NAMESPACE && type.IsSubclassOf(baseType))
                {
                    var registerMethod = type.GetMethod(UISystem.UI_GCOMPONENT_REGISTER_FUNC);
                    registerMethod?.Invoke(null, null);
                }
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void Register<T>(string pkgName, string resName) where T : GComponent
        {
            var key = Helper.GetUIKey<T>();
            if (!infos.ContainsKey(key))
            {
                infos[key] = new()
                {
                    pkgName = pkgName,
                    resName = resName
                };
            }
        }

        /// <summary>
        /// 尝试获取面板注册信息
        /// </summary>
        public bool TryGetInfo(string key, out UIRegisterInfo info)
        {
            return infos.TryGetValue(key, out info);
        }
    }
}