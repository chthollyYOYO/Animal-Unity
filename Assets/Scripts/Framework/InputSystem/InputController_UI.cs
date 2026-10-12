using FairyGUI;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class InputController_UI : IInputController
    {
        public int sortingOrder { get; set; }

        public UnityAction<GObject> onClickItem;
        public UnityAction onClickClose;
        public UnityAction<GObject> onLongPressDeleteStart;
        public UnityAction<GObject> onLongPressDelete;
        public UnityAction onLongPressDeleteEnd;
        public UnityAction<GObject, GObject> onSelectionChanged;
        public UnityAction onClickSpace;

        //同行限制：当存在同行按钮时，左右导航仅在同行按钮进行
        public bool enableSameRowLimit = false;

        //同列限制：当存在同列按钮时，上下导航仅在同列按钮进行；
        public bool enableSameColumnLimit = false;

        public GObject selection { get; private set; }
        private List<GObject> objs = new();
        private bool isLongPressDelete;

        public virtual void OnEnable()
        {
            Stage.inst.onTouchMove.Add(OnTouchMove);
        }

        public virtual void OnDisable()
        {
            Stage.inst.onTouchMove.Remove(OnTouchMove);
        }

        public virtual void OnGUI()
        {
            if (Event.current == null)
                return;

            if (InputTextField.EatKeyEvent(Event.current))
                return;

            if (Event.current.keyCode == KeyCode.None)
                return;

            if (Event.current.rawType == EventType.KeyDown)
            {
                OnKeyDown(Event.current.keyCode);
            }

            if (Event.current.rawType == EventType.KeyUp)
            {
                OnKeyUp(Event.current.keyCode);
            }
        }

        public void RegisterObject(GObject obj)
        {
            if (objs.Contains(obj))
                return;

            objs.Add(obj);
            obj.onClick.Add(OnClickItem);
        }

        public void RegisterObjects(IEnumerable<GObject> objs)
        {
            foreach (var obj in objs)
                RegisterObject(obj);
        }

        public void UnregisterObject(GObject obj)
        {
            if (!objs.Contains(obj))
                return;

            objs.Remove(obj);
            obj.onClick.Remove(OnClickItem);

            if (selection == obj)
                SetSelection(null);
        }

        public void UnregisterAllObjects()
        {
            objs.Clear();
            SetSelection(null);
        }

        public void SetSelection(GObject obj)
        {
            if (selection != obj)
            {
                var old = selection;
                selection = obj;
                old?.asCom.SetSelected(false);
                obj?.asCom.SetSelected(true);
                onSelectionChanged?.Invoke(old, obj);
                AudioManager.Instance.PlaySound("Music/jiaohu/huadong");
            }
        }

        private void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.K || keyCode == KeyCode.Escape)
            {
                onClickClose?.Invoke();
                AudioManager.Instance.PlaySound("Music/jiaohu/dianji");
                return;
            }

            if (keyCode == KeyCode.J && selection == null)
            {
                onClickSpace?.Invoke();
                return;
            }

            if (objs.Count == 0)
                return;

            if (selection == null)
                return;

            Vector2 position = selection.GetPositionForRoot();
            GObject target = null;
            Vector2 offset = Vector2.one * float.MaxValue;
            bool limit = false;
            switch (keyCode)
            {
                case KeyCode.LeftArrow:
                case KeyCode.A:
                    foreach (var obj in objs)
                    {
                        if (obj == selection)
                            continue;

                        Vector2 p = obj.GetPositionForRoot();
                        Vector2 o = p - position;

                        if (enableSameRowLimit && o.y == 0)
                            limit = true;

                        bool IsBetter()
                        {
                            //只考虑严格在左边的
                            if (o.x >= 0) return false;

                            //优选同行
                            if (o.y == 0 && offset.y != 0) return true;
                            if (o.y != 0 && offset.y == 0) return false;

                            //比较水平距离
                            float absX_o = Mathf.Abs(o.x);
                            float absX_offset = Mathf.Abs(offset.x);
                            if (absX_o < absX_offset) return true;
                            if (absX_o > absX_offset) return false;

                            //比较垂直距离
                            return Mathf.Abs(o.y) < Mathf.Abs(offset.y);
                        }

                        if (IsBetter())
                        {
                            target = obj;
                            offset = o;
                        }
                    }

                    if (limit && offset.y != 0)
                        target = null;

                    break;

                case KeyCode.RightArrow:
                case KeyCode.D:
                    foreach (var obj in objs)
                    {
                        if (obj == selection)
                            continue;

                        Vector2 p = obj.GetPositionForRoot();
                        Vector2 o = p - position;

                        if (enableSameRowLimit && o.y == 0)
                            limit = true;

                        bool IsBetter()
                        {
                            if (o.x <= 0) return false;

                            if (o.y == 0 && offset.y != 0) return true;
                            if (o.y != 0 && offset.y == 0) return false;

                            float absX_o = Mathf.Abs(o.x);
                            float absX_offset = Mathf.Abs(offset.x);
                            if (absX_o < absX_offset) return true;
                            if (absX_o > absX_offset) return false;

                            return Mathf.Abs(o.y) < Mathf.Abs(offset.y);
                        }

                        if (IsBetter())
                        {
                            target = obj;
                            offset = o;
                        }
                    }

                    if (limit && offset.y != 0)
                        target = null;

                    break;

                case KeyCode.UpArrow:
                case KeyCode.W:
                    foreach (var obj in objs)
                    {
                        if (obj == selection)
                            continue;

                        Vector2 p = obj.GetPositionForRoot();
                        Vector2 o = p - position;

                        if (enableSameColumnLimit && o.x == 0)
                            limit = true;

                        bool IsBetter()
                        {
                            if (o.y >= 0) return false;

                            if (o.x == 0 && offset.x != 0) return true;
                            if (o.x != 0 && offset.x == 0) return false;

                            float absY_o = Mathf.Abs(o.y);
                            float absY_offset = Mathf.Abs(offset.y);
                            if (absY_o < absY_offset) return true;
                            if (absY_o > absY_offset) return false;

                            return Mathf.Abs(o.x) < Mathf.Abs(offset.x);
                        }

                        if (IsBetter())
                        {
                            target = obj;
                            offset = o;
                        }
                    }

                    if (limit && offset.x != 0)
                        target = null;

                    break;

                case KeyCode.DownArrow:
                case KeyCode.S:
                    foreach (var obj in objs)
                    {
                        if (obj == selection)
                            continue;

                        Vector2 p = obj.GetPositionForRoot();
                        Vector2 o = p - position;

                        if (enableSameColumnLimit && o.x == 0)
                            limit = true;

                        bool IsBetter()
                        {
                            if (o.y <= 0) return false;

                            if (o.x == 0 && offset.x != 0) return true;
                            if (o.x != 0 && offset.x == 0) return false;

                            float absY_o = Mathf.Abs(o.y);
                            float absY_offset = Mathf.Abs(offset.y);
                            if (absY_o < absY_offset) return true;
                            if (absY_o > absY_offset) return false;

                            return Mathf.Abs(o.x) < Mathf.Abs(offset.x);
                        }

                        if (IsBetter())
                        {
                            target = obj;
                            offset = o;
                        }
                    }

                    if (limit && offset.x != 0)
                        target = null;

                    break;

                case KeyCode.J:
                    onClickItem?.Invoke(selection);
                    break;

                case KeyCode.L:
                    if (isLongPressDelete)
                    {
                        onLongPressDelete?.Invoke(selection);
                    }
                    else
                    {
                        isLongPressDelete = true;
                        onLongPressDeleteStart?.Invoke(selection);
                    }
                    break;
            }

            if (target != null)
            {
                SetSelection(target);
            }

            if (keyCode != KeyCode.L)
            {
                OnLongPressDeleteEnd();
            }
        }

        private void OnKeyUp(KeyCode keyCode)
        {
            OnLongPressDeleteEnd();
        }

        private void OnClickItem(EventContext context)
        {
            onClickItem?.Invoke(context.sender as GObject);
            AudioManager.Instance.PlaySound("Music/jiaohu/dianji");
            OnLongPressDeleteEnd();
        }

        private void OnTouchMove(EventContext context)
        {
            var target = Stage.inst.GetTouchTarget(0);
            var obj = target?.gOwner;
            while (obj != null)
            {
                if (objs.Contains(obj))
                {
                    SetSelection(obj);
                    OnLongPressDeleteEnd();
                    break;
                }
                obj = obj.parent;
            }
        }

        public void OnLongPressDeleteEnd()
        {
            if (isLongPressDelete)
            {
                isLongPressDelete = false;
                onLongPressDeleteEnd?.Invoke();
            }
        }
    }
}