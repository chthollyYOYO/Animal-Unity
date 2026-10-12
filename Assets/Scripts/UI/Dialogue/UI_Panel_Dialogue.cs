using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_Dialogue : ICreate, IShow<DialogueContext>, IHide, ISortable
    {
        int ISortable.SortingOrder => 15;

        private DialogueContext context;
        private TextPlayer player = new();
        private InputController_UI controller = new();

        public void OnCreate()
        {
            btn_next.onClick.Add(OnClickNext);
            controller.onClickSpace += OnClickNext;
            player.OnTextChanged += UpdateContent;
            player.OnPlayEnd += OnPlayEnd;
        }

        public void OnShow(DialogueContext context)
        {
            this.context = context;
            txt_name.text = context.name;
            UpdateContent(string.Empty);

            if (context.roleImage != null)
                lod_role.texture = new(context.roleImage);
            else
                lod_role.texture = null;

            ctrl_showName.SetSelectedBool(!string.IsNullOrEmpty(context.name));

            player.Play(context.content);
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void UpdateContent(string content)
        {
            txt_content.text = content;
        }

        private void OnPlayEnd()
        {
            context.onTextPlayEnd?.Invoke();
        }

        private void OnClickNext()
        {
            if (player.IsPlaying)
            {
                player.Stop();
                UpdateContent(context.content);
            }
            else
            {
                context.onNext?.Invoke();
            }
        }
    }
}