using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_DialogueImage : ICreate, IShow<DialogueImageContext>, IHide, ISortable
    {
        int ISortable.SortingOrder => 20;

        private DialogueImageContext context;
        private float? timer;
        private InputController_UI controller = new();

        public void OnCreate()
        {
            btn_next.onClick.Add(OnClickNext);
            controller.onClickSpace += OnClickNext;
        }

        public void OnShow(DialogueImageContext context)
        {
            this.context = context;
            lod.texture = new(context.texture);

            timer = 2;
            GameMonoSingleton.Instance.onUpdate += OnUpdate;
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            GameMonoSingleton.Instance.onUpdate -= OnUpdate;
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClickNext()
        {
            if (timer == null)
                context.onNext?.Invoke();
        }

        private void OnUpdate(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                timer = null;
                GameMonoSingleton.Instance.onUpdate -= OnUpdate;
            }
        }
    }
}