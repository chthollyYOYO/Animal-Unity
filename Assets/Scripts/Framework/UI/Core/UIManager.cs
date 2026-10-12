using Cysharp.Threading.Tasks;
using FairyGUI;
using System;
using System.Collections.Generic;
using UI;

namespace Framework
{
    public class UIManager : Singleton<UIManager>
    {
        public int cacheCount = 20;
        public event Action<GComponent> OnPanelShow;
        public event Action<GComponent> OnPanelHide;

        private readonly Dictionary<string, GComponent> coms = new();
        private readonly List<GComponent> caches = new();

        /// <summary>
        /// 显示面板
        /// </summary>
        public void ShowPanelForget<T>() where T : GComponent, IShow
        {
            ShowPanel<T>().Forget();
        }

        public void ShowPanelForget<T, P>(P param) where T : GComponent, IShow<P>
        {
            ShowPanel<T, P>(param).Forget();
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public async UniTask ShowPanel<T>() where T : GComponent, IShow
        {
            var key = Helper.GetUIKey<T>();
            await ShowPanel(key);
        }

        public async UniTask ShowPanel<T, P>(P param) where T : GComponent, IShow<P>
        {
            var key = Helper.GetUIKey<T>();
            await ShowPanel(key, param);
        }

        public async UniTask ShowPanel(string key)
        {
            if (!coms.TryGetValue(key, out var com))
            {
                coms.Add(key, null);
                coms[key] = com = await GetOrCreate(key);
                OnShowPanel(com);
            }
            else if (com == null)
            {
                await UniTask.WaitUntil(() => coms[key] != null);
            }
        }

        public async UniTask ShowPanel<P>(string key, P param)
        {
            if (!coms.TryGetValue(key, out var com))
            {
                coms.Add(key, null);
                coms[key] = com = await GetOrCreate(key);
                OnShowPanel(com, param);
            }
            else if (com == null)
            {
                await UniTask.WaitUntil(() => coms[key] != null);
            }
        }

        /// <summary>
        /// 同步显示面板，需确保资源已加载
        /// </summary>
        public void ShowPanelSync<T>() where T : GComponent//, IShow
        {
            var key = Helper.GetUIKey<T>();
            ShowPanelSync(key);
        }
        
        public void ShowPanelSync<T, P>(P param) where T : GComponent, IShow<P>
        {
            var key = Helper.GetUIKey<T>();
            ShowPanelSync(key, param);
        }

        private void ShowPanelSync(string key)
        {
            if (!coms.TryGetValue(key, out var com))
            {
                coms[key] = com = GetOrCreateSync(key);
                OnShowPanel(com);
            }
        }

        private void ShowPanelSync<P>(string key, P param)
        {
            if (!coms.TryGetValue(key, out var com))
            {
                coms[key] = com = GetOrCreateSync(key);
                OnShowPanel(com, param);
            }
        }

        private void OnShowPanel(GComponent com)
        {
            GRoot.inst.AddChild(com);
            com.visible = true;

            com.ChildrenRecursionAction(child =>
            {
                if (child is IShow iShow)
                    iShow.OnShow();
            });

            OnPanelShow?.Invoke(com);
        }

        private void OnShowPanel<P>(GComponent com, P param)
        {
            GRoot.inst.AddChild(com);
            com.visible = true;

            com.ChildrenRecursionAction(child =>
            {
                if (child is IShow<P> iShowObj)
                    iShowObj.OnShow(param);
                else if (child is IShow iShow)
                    iShow.OnShow();
            });

            OnPanelShow?.Invoke(com);
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void HidePanel<T>() where T : GComponent
        {
            var key = Helper.GetUIKey<T>();
            HidePanel(key);
        }

        public void HidePanel(GComponent com)
        {
            var key = Helper.GetUIKey(com);
            HidePanel(key);
        }

        public void HidePanel(string key)
        {
            if (coms.TryGetValue(key, out var com) && com != null)
            {
                com.visible = false;
                coms.Remove(Helper.GetUIKey(com));
                GRoot.inst.RemoveChild(com);

                com.ChildrenRecursionAction(child =>
                {
                    if (child is IHide iHide)
                        iHide.OnHide();
                });

                Release(com);
                OnPanelHide?.Invoke(com);
            }
        }

        public void HidePanels(Predicate<GComponent> predicate)
        {
            List<GComponent> hides = new();
            foreach (var com in coms.Values)
            {
                if (predicate(com))
                    hides.Add(com);
            }

            foreach (var com in hides)
                HidePanel(Helper.GetUIKey(com));
        }

        /// <summary>
        /// 查找面板
        /// </summary>
        public GComponent FindPanel(string key)
        {
            if (coms.TryGetValue(key, out var com) && com != null)
                return com;
            return null;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCaches()
        {
            for (int i = 0; i < caches.Count; i++)
                Destroy(caches[i]);
            caches.Clear();
        }

        /// <summary>
        /// 获取|创建面板
        /// </summary>
        private async UniTask<GComponent> GetOrCreate(string key)
        {
            var com = Get(key);
            com ??= await Create(key);
            return com;
        }

        /// <summary>
        /// 获取|同步创建面板，需确保资源已加载
        /// </summary>
        private GComponent GetOrCreateSync(string key)
        {
            var com = Get(key);
            com ??= CreateSync(key);
            return com;
        }

        /// <summary>
        /// 获取面板
        /// </summary>
        private GComponent Get(string key)
        {
            var com = caches.Find(it => Helper.GetUIKey(it) == key);
            if (com != null)
            {
                caches.Remove(com);
                return com;
            }
            return null;
        }

        /// <summary>
        /// 创建面板
        /// </summary>
        private async UniTask<GComponent> Create(string key)
        {
            if (UIRegisterInfoManager.Instance.TryGetInfo(key, out var info))
            {
                await UIResourceManager.Instance.LoadByKey(key);
                GComponent com = null;
                UIPackage.CreateObjectAsync(info.pkgName, info.resName, obj =>
                {
                    OnCreatePanel(key, obj);
                    com = obj.asCom;
                });
                await UniTask.WaitUntil(() => com != null);
                return com;
            }
            return null;
        }

        /// <summary>
        /// 同步创建面板，需确保资源已加载
        /// </summary>
        private GComponent CreateSync(string key)
        {
            if (UIRegisterInfoManager.Instance.TryGetInfo(key, out var info))
            {
                var obj = UIPackage.CreateObject(info.pkgName, info.resName);
                OnCreatePanel(key, obj);
                return obj.asCom;
            }
            return null;
        }

        private void OnCreatePanel(string key, GObject obj)
        {
            var com = obj.asCom;
            com.gameObjectName = key;

            bool fairyBatching = true;
            com.ChildrenRecursionAction(child =>
            {
                if (child is INotFairyBatching)
                    fairyBatching = false;
            });
            com.fairyBatching = fairyBatching;

            com.ChildrenRecursionAction(child =>
            {
                if (child is ICreate iCreate)
                    iCreate.OnCreate();
            });
        }

        /// <summary>
        /// 释放面板
        /// </summary>
        private void Release(GComponent com)
        {
            if (com is INotCache)
            {
                Destroy(com);
            }
            else
            {
                if (caches.Count >= cacheCount)
                {
                    Destroy(caches[0]);
                    caches.RemoveAt(0);
                }
                caches.Add(com);
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        private void Destroy(GComponent com)
        {
            com.ChildrenRecursionAction(child =>
            {
                if (child is IDestroy iDestroy)
                    iDestroy.OnDestroy();
            });
            
            com.Dispose();
            UIResourceManager.Instance.TryUnloadByPkgName(Helper.GetUIKey(com));
        }
    }
}