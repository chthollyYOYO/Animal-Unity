using System;
using Framework;

namespace UI
{
    public struct BeatmapRestartParm
    {
        public Action OnConfirm;
    }
    public partial class UI_Anim_CountDown: IShow<BeatmapRestartParm>,IHide
    {
        private Action confirm;
        private InputController_UI controller = new();

        public void OnShow(BeatmapRestartParm param)
        {
            InputSystem.Instance.RegisterController(controller);
            confirm = param.OnConfirm;
            CountDown.Play(() => Hide());
        }
        public void Hide()
        {
            UIManager.Instance.HidePanel<UI_Anim_CountDown>();
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            confirm?.Invoke();
        }
    }
}