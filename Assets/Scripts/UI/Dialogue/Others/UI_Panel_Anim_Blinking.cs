using Framework;
using System;

namespace UI
{
    public partial class UI_Panel_Anim_Blinking : ICreate, IShow<Action>, IHide, ISortable
    {
        int ISortable.SortingOrder => 30;

        private Action onNext;
        private InputController_UI controller = new();

        public void OnCreate()
        {
            btn_next.onClick.Add(OnClickNext);
            controller.onClickSpace += OnClickNext;
        }

        public void OnShow(Action onNext)
        {
            this.onNext = onNext;
            anim.Play();
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClickNext()
        {
            if (anim.playing)
                return;

            onNext?.Invoke();
        }
    }
}
