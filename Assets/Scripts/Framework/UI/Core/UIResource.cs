using Cysharp.Threading.Tasks;
using FairyGUI;
using System.Collections.Generic;

namespace Framework
{
    public class UIResource
    {
        public string PkgName { get; private set; }

        private readonly List<string> refs = new();
        //private readonly List<ResourceHandle> handles = new();

        public UIResource(string pkgName)
        {
            PkgName = $"UI/{pkgName}";
        }

        /// <summary>
        /// 添加引用
        /// </summary>
        public void AddRef(string key)
        {
            if (!refs.Contains(key))
                refs.Add(key);
        }

        /// <summary>
        /// 移除引用
        /// </summary>
        public void RemoveRef(string key)
        {
            if (refs.Contains(key))
                refs.Remove(key);
        }

        /// <summary>
        /// 加载包
        /// </summary>
        public UniTask Load()
        {
            UIPackage.AddPackage(PkgName);
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 尝试卸载包
        /// </summary>
        public void TryUnload()
        {
            if (refs.Count == 0)
            {
                UIPackage.RemovePackage(PkgName);
            }
        }
    }
}
