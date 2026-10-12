using FairyGUI;
using Framework;
using System;
using UnityEngine;

namespace UI
{
    public static partial class Extension
    {
        public static void ChildrenRecursionAction(this GComponent com, Action<GComponent> action)
        {
            action(com);
            var children = com.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] is GComponent c)
                    c.ChildrenRecursionAction(c => action(c));
            }
        }

        public static void HandleChildren(this GComponent com, Action<int, GObject> action)
        {
            for (int i = 0; i < com.numChildren; i++)
                action(i, com.GetChildAt(i));
        }

        public static Vector2 GetPositionForRoot(this GObject obj)
        {
            Vector2 result;
            if (obj.parent == null || obj.parent == GRoot.inst)
            {
                result = obj.xy;
            }
            else
            {
                result = obj.xy + GetPositionForRoot(obj.parent);
                if ((obj is GComponent com) && com.container != null && com.container.xy != obj.xy)
                    result -= new Vector2(com.container.position.x, -com.container.position.y);
            }

            return result;
        }

        public static Vector2 GetScaleForRoot(this GObject obj)
        {
            Vector2 result;
            if (obj.parent == null || obj.parent == GRoot.inst)
            {
                result = obj.scale;
            }
            else
            {
                result = obj.scale * GetScaleForRoot(obj.parent);
                //if ((obj is GComponent com) && com.container != null && com.container.scale != obj.scale)
                //    result -= new Vector2(com.container.scale.x, -com.container.scale.y);
            }
            return result;
        }

        public static void SetSelectedBool(this Controller ctrl, bool value)
        {
            ctrl.selectedIndex = value ? 1 : 0;
        }

        public static void ResizeToFit(this GComponent com)
        {
            float x = 0;
            float y = 0;
            com.HandleChildren((index, child) =>
            {
                if (child.x + child.width > x)
                    x = child.x + child.width;
                if (child.y + child.height > y)
                    y = child.y + child.height;
            });
            com.SetSize(x, y);
        }

        public static void CustomFireClick(this GObject obj)
        {
            if (obj is GButton button)
                button.FireClick(true, false);
            obj.onClick.Call();
            obj.onTouchBegin.Call();
            obj.onTouchEnd.Call();
        }

        public static void SetSelected(this GComponent com, bool selected)
        {
            var ctrl = com.GetController("button");
            if (ctrl != null)
                ctrl.selectedPage = selected ? GButton.DOWN : GButton.UP;
        }

        public static void CloseSelf(this GComponent com)
        {
            UIManager.Instance.HidePanel(com);
        }
    }
}
