using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class InteractableObjectManager : Singleton<InteractableObjectManager>
    {
        // 静态字典：以网格坐标为键存储静态交互物体
        //private Dictionary<Vector2Int, InteractableObject> staticInteractables = new();

        // 动态列表：存储动态交互物体
        private List<InteractableObject> dynamicInteractables = new();

        public void RegisterObject(InteractableObject obj)
        {
            //if (obj.gameObject.isStatic)
            //    staticInteractables.Add(obj.GetGridPosition(), obj);
            //else
                dynamicInteractables.Add(obj);
        }

        public void UnregisterObject(InteractableObject obj)
        {
            //if (obj.gameObject.isStatic)
            //    staticInteractables.Remove(obj.GetGridPosition());
            //else
                dynamicInteractables.Remove(obj);
        }

        /// <summary>
        /// 在指定网格位置查找可交互物体
        /// </summary>
        //public InteractableObject FindInteractableAtPosition(Vector2Int gridPosition)
        //{
        //    // 1. 先遍历动态列表
        //    foreach (var obj in dynamicInteractables)
        //    {
        //        if (obj.GetGridPosition() == gridPosition && obj.IsInteractable())
        //        {
        //            return obj;
        //        }
        //    }

        //    // 2. 再查静态字典
        //    if (staticInteractables.TryGetValue(gridPosition, out InteractableObject staticObj) && staticObj.IsInteractable())
        //    {
        //        return staticObj;
        //    }

        //    return null;
        //}

        public InteractableObject FindInteractable(Vector2 position, Vector2 direction)
        {
            InteractableObject result = null;
            float min = float.MaxValue;
            for (int i = 0; i < dynamicInteractables.Count; i++)
            {
                var obj = dynamicInteractables[i];
                float sqlMagnitude = Vector2.SqrMagnitude((Vector2)obj.point.position - position);
                if (sqlMagnitude < 1.5f)
                {
                    if (sqlMagnitude < min)
                    {
                        min = sqlMagnitude;
                        result = obj;
                    }
                }
            }

            if (result == null || !result.IsInteractable())
                return null;

            return result;
        }
    }
}
