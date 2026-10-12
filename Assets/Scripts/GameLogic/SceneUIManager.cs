using FairyGUI;
using Framework;

namespace GameLogic
{
    public class SceneUIManager : Singleton<SceneUIManager>
    {
        private GComponent com;

        public void Initialize()
        {
            com = new();
            GRoot.inst.AddChildAt(com, 0);
        }

        public T CreateUI<T>(string pkgName, string resName) where T : GObject
        {
            var res = UIPackage.CreateObject(pkgName, resName);
            com.AddChild(res);
            return res as T;
        }

        public void DestroyUI(GObject obj)
        {
            com.RemoveChild(obj);
            obj.Dispose();
        }
    }
}
