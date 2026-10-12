using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_DialogueOption : ICreate, IShow<DialogueOptionContext>, IHide, ISortable
    {
        int ISortable.SortingOrder => 50;

        private DialogueOptionContext context;
        private InputController_UI controller = new();

        public void OnCreate()
        {
            list_option.itemRenderer = ItemRenderer;
            controller.onClickItem += OnClickItem;
        }

        public void OnShow(DialogueOptionContext context)
        {
            this.context = context;
            Update();

            InputSystem.Instance.RegisterController(controller);
            controller.RegisterObjects(list_option.GetChildren());
            controller.SetSelection(list_option.GetChildAt(0));
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private async void Update()
        {
            list_option.numItems = context.contents.Count;
            list_option.ResizeToFit();
        }

        private void ItemRenderer(int index, GObject obj)
        {
            var content = this.context.contents[index];
            if (obj is UI_Button_Option option)
            {
                option.Update(content);
            }
        }

        private void OnClickItem(GObject obj)
        {
            int index = list_option.GetChildIndex(obj);
            this.context.onChoose?.Invoke(index);
        }
    }
}