using Framework;
using UnityEngine;

namespace UI
{
    public partial class UI_Panel_Thank : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();

        public void OnCreate()
        {
            btn_exit.onClick.Add(OnClickExit);
        }

        public void OnShow()
        {
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClickExit()
        {
            if (Application.isPlaying)
                Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}
