//using FairyGUI;
//using GameLogic;
//using System;
//using System.Collections.Generic;
//using UI;
//using UnityEngine;

//namespace Framework
//{
//    public enum DragInableState
//    {
//        None = 0,
//        False = 1,
//        True = 2,
//    }

//    public interface IDragable
//    {
//        GLoader GetLoader();

//        void SetDragInableState(DragInableState state);
//    }

//    public class DragService : Singleton<DragService>
//    {
//        private Dictionary<GObject, Storage> begins = new();
//        private Dictionary<GObject, Storage> ends = new();
//        private Dictionary<GObject, Action> onTouchBegins = new();
//        private Dictionary<GObject, Action<Item, int>> customReturns = new();
//        private bool isDraging;
//        private GLoader dragLoader;
//        private GObject fromObj;
//        private Storage fromStorage;
//        private int fromIndex;
//        private Item dragItem;
//        private Vector2 dragOffset;

//        public void Initialize()
//        {
//            Stage.inst.onTouchMove.Add(OnTouchMoveStage);
//            Stage.inst.onTouchEnd.Add(OnTouchEndStage);
//        }

//        public void BindDragBegin(GObject obj, Storage storage)
//        {
//            if (!begins.ContainsKey(obj))
//                obj.onTouchBegin.Add(OnTouchBegin);
//            begins[obj] = storage;
//        }

//        public void BindDragEnd(GObject obj, Storage storage)
//        {
//            if (!ends.ContainsKey(obj))
//                obj.onTouchEnd.Add(OnTouchEnd);
//            ends[obj] = storage;
//        }

//        public void UnbindDrag(GObject obj)
//        {
//            obj.onTouchBegin.Remove(OnTouchBegin);
//            obj.onTouchEnd.Remove(OnTouchEnd);
//            if (begins.ContainsKey(obj))
//                begins.Remove(obj);
//            if (ends.ContainsKey(obj))
//                ends.Remove(obj);
//            if (onTouchBegins.ContainsKey(obj))
//                onTouchBegins.Remove(obj);
//            if (customReturns.ContainsKey(obj))
//                customReturns.Remove(obj);
//        }

//        public void AddTouchBeginCallback(GObject obj, Action callback)
//        {
//            onTouchBegins[obj] = callback;
//        }
        
//        public void AddCustomReturn(GObject obj, Action<Item, int> callback)
//        {
//            customReturns[obj] = callback;
//        }

//        private void OnTouchBegin(EventContext context)
//        {
//            fromObj = context.sender as GObject;
//            if (begins.TryGetValue(fromObj, out fromStorage))
//            {
//                if (fromObj.parent is GList gList)
//                    fromIndex = gList.GetChildIndex(fromObj);
//                else
//                    fromIndex = 0;
//                dragItem = fromStorage.GetItem(fromIndex);

//                if (dragItem == null)
//                    return;

//                isDraging = true;
//                if (fromObj is IDragable dragable)
//                    CreateDragLoader(dragable.GetLoader(), context.inputEvent.position / UIContentScaler.scaleFactor, dragItem);
//                else if (fromObj is GLoader gLoader)
//                    CreateDragLoader(gLoader, context.inputEvent.position / UIContentScaler.scaleFactor, dragItem);
//                fromStorage.Remove(fromIndex);

//                if (onTouchBegins.TryGetValue(fromObj, out var callback))
//                    callback();

//                context.StopPropagation();
//            }
//        }

//        private void OnTouchEnd(EventContext context)
//        {
//            if (!isDraging)
//                return;

//            isDraging = false;
//            dragLoader.visible = false;
//            ClearDragInableState();

//            var obj = context.sender as GObject;
//            if (ends.TryGetValue(obj, out var endStorage))
//            {
//                if (obj is GList gList)
//                {
//                    var toIndex = GetDragToIndex(gList, endStorage);
//                    if (endStorage.Addable(dragItem, toIndex))
//                        endStorage.Add(dragItem, toIndex);
//                    else if (customReturns.TryGetValue(fromObj, out var callback))
//                        callback(dragItem, fromIndex);
//                    else
//                        fromStorage.Add(dragItem, fromIndex);
//                }
//                else
//                {
//                    if (endStorage.Addable(dragItem))
//                        endStorage.Add(dragItem);
//                    else if (customReturns.TryGetValue(fromObj, out var callback))
//                        callback(dragItem, fromIndex);
//                    else
//                        fromStorage.Add(dragItem, fromIndex);
//                }
//            }
//        }

//        private void OnTouchEndStage()
//        {
//            if (!isDraging)
//                return;

//            isDraging = false;
//            dragLoader.visible = false;
//            ClearDragInableState();

//            if (customReturns.TryGetValue(fromObj, out var callback))
//                callback(dragItem, fromIndex);
//            else
//                fromStorage.Add(dragItem, fromIndex);
//        }

//        private void OnTouchMoveStage(EventContext context)
//        {
//            if (!isDraging)
//                return;

//            dragLoader.xy = context.inputEvent.position / UIContentScaler.scaleFactor - dragOffset;
//            ClearDragInableState();

//            var obj = FindTouchEndObject(dragLoader.xy);
//            if (obj != null && ends.TryGetValue(obj, out var storage))
//            {
//                if (obj is GList gList)
//                {
//                    SetDragInableState(gList, storage, dragItem);
//                }
//            }
//        }

//        private void CreateDragLoader(GLoader sourceLoader, Vector2 position, Item item)
//        {
//            dragLoader ??= new();
//            dragLoader.url = sourceLoader.url;
//            dragLoader.pivot = sourceLoader.pivot;
//            //dragLoader.pivotAsAnchor = true;
//            //dragLoader.rotation = sourceLoader.rotation;
//            dragLoader.rotation = item.isRotation ? -90 : 0;
//            //dragLoader.width = sourceLoader.width;
//            dragLoader.width = UI_item.initSize.x * item.InitWidth + UI_item.initSpace.x * (item.InitWidth - 1);
//            //dragLoader.height = sourceLoader.height;
//            dragLoader.height = UI_item.initSize.y * item.InitHeight + UI_item.initSpace.y * (item.InitHeight - 1);
//            dragLoader.xy = sourceLoader.GetPositionForRoot();
//            dragLoader.visible = true;
//            dragLoader.touchable = false;
//            dragLoader.fill = FillType.Scale;
//            dragLoader.align = AlignType.Center;
//            dragLoader.verticalAlign = VertAlignType.Middle;
//            GRoot.inst.AddChild(dragLoader);

//            dragOffset = (position - dragLoader.xy) / sourceLoader.parent.scale;
//        }

//        private Vector2Int GetDragToIndex(GList gList, Storage toStorage)
//        {
//            Vector2 v = dragLoader.xy;
//            //if (dragItem.isRotation)
//            //    v.x -= dragLoader.height;
//            float min = float.MaxValue;
//            int index = -1;
//            for (int i = 0; i < gList.numChildren; i++)
//            {
//                var p = gList.GetChildAt(i).GetPositionForRoot();
//                var d = (p.x - v.x) * (p.x - v.x) + (p.y - v.y) * (p.y - v.y);
//                if (d < min)
//                {
//                    min = d;
//                    index = i;
//                }
//            }
//            return new(index % toStorage.Width, index / toStorage.Width);
//        }

//        private GObject FindTouchEndObject(Vector2 position)
//        {
//            foreach (var obj in ends.Keys)
//            {
//                var p = obj.GetPositionForRoot();
//                var s = obj.GetScaleForRoot();
//                if (obj.onStage
//                    && position.x >= p.x && position.x <= p.x + s.x * obj.width
//                    && position.y >= p.y && position.y <= p.y + s.y * obj.height)
//                {
//                    return obj;
//                }
//            }
//            return null;
//        }

//        private void ClearDragInableState()
//        {
//            foreach (var obj in ends.Keys)
//            {
//                if (obj is GList gList)
//                {
//                    for (int i = 0; i < gList.numChildren; i++)
//                    {
//                        if (gList.GetChildAt(i) is IDragable dragable)
//                        {
//                            dragable.SetDragInableState(DragInableState.None);
//                        }
//                    }
//                }
//            }
//        }

//        private void SetDragInableState(GList gList, Storage storage, Item item)
//        {
//            var toIndex = GetDragToIndex(gList, storage);
//            bool addable = storage.Addable(item, toIndex);
//            for (int i = 0; i < gList.numChildren; i++)
//            {
//                if (gList.GetChildAt(i) is IDragable dragable)
//                {
//                    var v = storage.IntIndexToVector2Int(i);
//                    if (v.x >= toIndex.x && v.x <= toIndex.x + item.Width - 1
//                        && v.y >= toIndex.y && v.y <= toIndex.y + item.Height - 1)
//                    {
//                        var grid = storage.GetGrid(v);
//                        dragable.SetDragInableState(addable ? DragInableState.True : DragInableState.False);
//                    }
//                    else
//                    {
//                        dragable.SetDragInableState(DragInableState.None);
//                    }
//                }
//            }
//        }
//    }
//}