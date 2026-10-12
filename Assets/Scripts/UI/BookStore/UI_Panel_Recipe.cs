using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_Recipe : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();

        public void OnCreate()
        {
            list_recipe.itemRenderer = ItemRendererOption;
            controller.onClickItem = OnClikItem;
            controller.onClickClose = Hide;
            btn_close.onClick.Add(Hide);
        }

        public void OnShow()
        {
            list_recipe.numItems = BookStoreSystem.Instance.shelfItems.Count;
            BookStoreSystem.Instance.itemNumChange += UpdateStock;
            InputSystem.Instance.RegisterController(controller);
            controller.RegisterObjects(list_recipe.GetChildren());
            controller.SetSelection(list_recipe.GetChildAt(0));
            Record.current.onMoneyChanged += UpdateStock;
        }

        public void Hide()
        {
            UIManager.Instance.HidePanel<UI_Panel_Recipe>();
        }

        public void OnHide()
        {
            BookStoreSystem.Instance.itemNumChange -= UpdateStock;
            Record.current.onMoneyChanged -= UpdateStock;
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void UpdateStock()
        {
            list_recipe.numItems = BookStoreSystem.Instance.shelfItems.Count;
        }

        private void ItemRendererOption(int index, GObject obj)
        {
            var ui = obj as UI_Button_Recipe;
            ui.Update(BookStoreSystem.Instance.shelfItems[index]);
        }

        private void OnClikItem(GObject obj)
        {
            int itemIndex = list_recipe.GetChildIndex(obj);
            if (BookStoreSystem.Instance.shelfItems[itemIndex].item.count != 0 &&
                Record.current.money >= BookStoreSystem.Instance.shelfItems[itemIndex].itemPrice)
            {
                BookStoreSystem.Instance.BuyByIndex(itemIndex);
            }
        }
    }
}