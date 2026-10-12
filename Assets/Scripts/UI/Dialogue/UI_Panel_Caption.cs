using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_Caption : ICreate, IShow<DialogueCaptionContext>, IHide, ISortable
    {
        int ISortable.SortingOrder => 30;

        private DialogueCaptionContext context;
        private InputController_UI controller = new();

        public void OnCreate()
        {
            btn_next.onClick.Add(OnClickNext);
            controller.onClickSpace += OnClickNext;
        }

        public void OnShow(DialogueCaptionContext context)
        {
            this.context = context;
            txt_content.text = context.content;
            anim_show.Play();
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClickNext()
        {
            if (anim_show.playing)
                return;

            if (anim_hide.playing)
                return;

            anim_hide.Play(() => context.onFinished?.Invoke());
        }
    }
}
