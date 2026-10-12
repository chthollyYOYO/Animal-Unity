using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public partial class InputSystem : Singleton<InputSystem>
    {
        public BoolPlus enabled = new(true);
        public List<IInputController> controllers = new();

        private int opFrameCount;
        private List<IInputController> addList = new();
        private List<IInputController> removeList = new();

        public void Initialize()
        {
            GameMonoSingleton.Instance.onUpdate += OnUpdate;
            GameMonoSingleton.Instance.onGUI += OnGUI;
        }

        public void RegisterController(IInputController controller)
        {
            opFrameCount = Time.frameCount;
            addList.Add(controller);
            removeList.Remove(controller);
        }

        public void UnRegisterController(IInputController controller)
        {
            opFrameCount = Time.frameCount;
            removeList.Add(controller);
            addList.Remove(controller);
        }

        private void OnUpdate(float deltaTime)
        {
            if (!enabled.GetValue())
                return;

            CheckDirty();
            if (controllers.Count > 0)
            {
                controllers[0].OnInputUpdate(deltaTime);
            }
        }

        private void OnGUI()
        {
            if (!enabled.GetValue())
                return;

            CheckDirty();
            if (controllers.Count > 0)
            {
                controllers[0].OnGUI();
            }
        }

        private void CheckDirty()
        {
            if (Time.frameCount > opFrameCount)
            {
                if (addList.Count > 0)
                {
                    foreach (var controller in addList)
                    {
                        if (controllers.Contains(controller))
                            controllers.Remove(controller);

                        controllers.AddOrInsert(controller, ctrl => controller.sortingOrder >= ctrl.sortingOrder);
                        controller.OnEnable();
                    }
                    addList.Clear();
                }

                if (removeList.Count > 0)
                {
                    foreach (var controller in removeList)
                    {
                        if (!controllers.Contains(controller))
                            continue;

                        controllers.Remove(controller);
                        controller.OnDisable();
                    }
                    removeList.Clear();
                }
            }
        }
    }
}
