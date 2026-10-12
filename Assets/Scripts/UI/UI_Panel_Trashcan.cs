using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_Trashcan : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();
        private Item selectionItem;

        public void OnCreate()
        {
            controller.onClickClose += OnClickClose;
            controller.onClickItem += OnClickItem;
            controller.onSelectionChanged += OnSelectionChanged;

            btn_close.onClick.Add(OnClickClose);
        }

        public void OnShow()
        {
            Update();

            InputSystem.Instance.RegisterController(controller);
            UpdateController();
            UI_Panel_Main.current.TrySelectedItem(controller, 0);

            InteractableTipManager.Instance.isShowTip = false;
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            controller.SetSelection(null);

            InteractableTipManager.Instance.isShowTip = true;
        }

        private void Update()
        {
            com_itemInfo.Update(selectionItem);
        }

        private void UpdateController()
        {
            controller.UnregisterAllObjects();
            controller.RegisterObjects(UI_Panel_Main.current.GetBagButtons());
        }

        private void OnClickItem(GObject obj)
        {
            if (obj.data is (string key, int index))
            {
                if (key == "bag")
                {
                    BagSystem.Instance.RemoveItem(index);
                    AudioManager.Instance.PlaySound("Music/jiaohu/lajitong");
                    UpdateController();
                    UI_Panel_Main.current.TrySelectedItem(controller, index);
                }
            }
        }

        private void OnSelectionChanged(GObject old, GObject obj)
        {
            if (obj == null)
            {
                selectionItem = null;
            }
            else if (obj.data is (string key, int index))
            {
                if (key == "bag")
                {
                    selectionItem = BagSystem.Instance.GetItem(index);
                }
            }
            com_itemInfo.Update(selectionItem);
        }

        private void OnClickClose()
        {
            this.CloseSelf();
        }
    }
}
