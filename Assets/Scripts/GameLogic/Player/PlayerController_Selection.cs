using System;
using UnityEngine;

namespace GameLogic
{
    public partial class PlayerController
    {
        public event Action onSelectedChanged;
        public InteractableObject currentSelectedObject
        {
            get => _currentSelectedObject;
            set
            {
                if (_currentSelectedObject != value)
                {
                    _currentSelectedObject = value;
                    onSelectedChanged?.Invoke();
                }
            }
        }
        private InteractableObject _currentSelectedObject;

        public Transform interactPoint;

        private void InitInteract()
        {
            interactPoint = transform.Find("InteractPoint");
            if (interactPoint == null)
                interactPoint = transform;
        }

        private void HandleInteract(float deltaTime)
        {
            // 计算需要高亮的网格坐标（玩家坐标 + 玩家方向）
            //Vector2Int targetGridPosition = position + direction;

            // 查找需要高亮的物体
            InteractableObject targetObject = InteractableObjectManager.Instance.FindInteractable(interactPoint.position, direction);

            if (currentSelectedObject != targetObject)
            {
                if (currentSelectedObject != null)
                {
                    currentSelectedObject.OnCancelSelected();
                }

                if (targetObject != null)
                {
                    targetObject.OnSelected();
                }

                currentSelectedObject = targetObject;
            }

            // 检测交互按键
            if (targetObject != null)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    targetObject.OnInteract();
                }

                if (Input.GetKey(KeyCode.J))
                {
                    targetObject.OnLongPress(deltaTime);
                }

                if (Input.GetKeyUp(KeyCode.J) || (Input.anyKeyDown && Input.GetKeyDown(KeyCode.J)))
                {
                    targetObject.OnLongPressEnd();
                }
            }
        }
    }
}