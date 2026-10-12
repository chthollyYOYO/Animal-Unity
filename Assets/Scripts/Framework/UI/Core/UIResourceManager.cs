using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Framework
{
    public class UIResourceManager : Singleton<UIResourceManager>
    {
        private readonly Dictionary<string, UIResource> resources = new();

        /// <summary>
        /// 加载面板所在包，内部记录引用，面板创建时的加载建议用此方法
        /// </summary>
        public async UniTask LoadByKey(string key)
        {
            if (UIRegisterInfoManager.Instance.TryGetInfo(key, out var info))
            {
                var resource = GetOrCreateUIResource(info.pkgName);
                resource.AddRef(key);
                await resource.Load();
            }
        }

        /// <summary>
        /// 加载包，通用包加载建议使用此方法
        /// </summary>
        public async UniTask LoadByPkgName(string pkgName)
        {
            if (!resources.ContainsKey(pkgName))
            {
                var resource = GetOrCreateUIResource(pkgName);
                await resource.Load();
            }
        }

        /// <summary>
        /// 尝试卸载面板所在包，内部检测引用，面板销毁时卸载建议用此方法
        /// </summary>
        public void TryUnloadByKey(string key)
        {
            if (UIRegisterInfoManager.Instance.TryGetInfo(key, out var info))
            {
                var resource = GetOrCreateUIResource(info.pkgName);
                resource.RemoveRef(key);
                resource.TryUnload();
            }
        }

        /// <summary>
        /// 尝试卸载包，通用包卸载建议使用此方法
        /// </summary>
        public void TryUnloadByPkgName(string pkgName)
        {
            if (resources.TryGetValue(pkgName, out var resource))
            {
                resource.TryUnload();
                resources.Remove(pkgName);
            }
        }

        private UIResource GetOrCreateUIResource(string pkgName)
        {
            if (!resources.TryGetValue(pkgName, out var resource))
                resources.Add(pkgName, resource = new(pkgName));
            return resource;
        }
    }
}