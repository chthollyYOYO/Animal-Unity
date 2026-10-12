using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace Framework
{
    public static class UIFinder
    {
        public static async UniTask<GComponent> FindPanel(string panelName)
        {
            var key = $"UI_{panelName}";
            GComponent result = null;
            while (result == null)
            {
                result = UIManager.Instance.FindPanel(key);
                await UniTask.NextFrame();
            }
            return result;
        }

        public static async UniTask<UIPanel> FindPanelInWorld(string gameObjectName)
        {
            UIPanel result = null;
            while (result == null)
            {
                var gameObject = GameObject.Find(gameObjectName);
                if (gameObject != null)
                {
                    result = gameObject.GetComponentInChildren<UIPanel>();
                    await UniTask.NextFrame();
                }
                else
                {
                    Debug.LogError($"Can not find GameObject {gameObjectName}");
                }
            }
            return result;
        }

        public static GObject FindChildRecursion(this GComponent com, string childName)
        {
            var children = com.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                if (child.name == childName)
                    return child;

                if (child is GComponent childCom)
                {
                    var result = childCom.FindChildRecursion(childName);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public static Vector2 WorldToUIPoint(Vector3 position)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(position);
            var uiPoint = new Vector2(screenPoint.x, Screen.height - screenPoint.y) / UIContentScaler.scaleFactor;
            return uiPoint;
        }

        public static Vector2 FindPosInGRoot(GObject obj)
        {
            GObject o = obj;
            Vector2 pos = obj.xy;
            while (o.parent != GRoot.inst && o.parent != null)
            {
                pos = new Vector2(pos.x * o.parent.scaleX, pos.y * o.parent.scaleY);
                pos += o.parent.xy;
                o = o.parent;
            }
            return pos;
        }

        public static Vector2 FindSizeInGRoot(GObject obj)
        {
            GObject o = obj;
            Vector2 size = new Vector2(obj.actualWidth, obj.actualHeight);
            while (o.parent!= GRoot.inst && o.parent!= null)
            {
                size = new Vector2(size.x * o.parent.scaleX, size.y * o.parent.scaleY);
                o = o.parent;
            }
            return size;
        }
    }
}