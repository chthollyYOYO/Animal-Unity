using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_Table : ICreate, IShow<InteractableObject_Table>, IHide
    {
        private InputController_UI controller = new();
        private InteractableObject_Table table;
        private Item selectionItem;

        private int needItemId => table.needItemId;
        private Item item => table.item;

        public void OnCreate()
        {
            controller.onClickClose += OnClickClose;
            controller.onClickItem += OnClickItem;
            controller.onSelectionChanged += OnSelectionChanged;

            btn_close.onClick.Add(OnClickClose);

            btn_boxItem.data = ("table", 0);
        }

        public void OnShow(InteractableObject_Table table)
        {
            this.table = table;
            table.onItemChanged += Update;
            Update();

            InputSystem.Instance.RegisterController(controller);
            UpdateController();
            UI_Panel_Main.current.TrySelectedItem(controller, 0);

            InteractableTipManager.Instance.isShowTip = false;
        }

        public void OnHide()
        {
            table.onItemChanged -= Update;
            table = null;

            InputSystem.Instance.UnRegisterController(controller);
            controller.SetSelection(null);

            InteractableTipManager.Instance.isShowTip = true;
        }

        private void Update()
        {
            lod_icon.SetItemIcon(needItemId);
            btn_boxItem.Update(item, false);
            com_itemInfo.Update(selectionItem);
        }

        private void UpdateController()
        {
            controller.UnregisterAllObjects();
            controller.RegisterObjects(UI_Panel_Main.current.GetBagButtons());
            if (item != null)
                controller.RegisterObject(btn_boxItem);
        }

        private void OnClickItem(GObject obj)
        {
            if (obj.data is (string key, int index))
            {
                if (key == "bag")
                {
                    var item = BagSystem.Instance.GetItem(index);

                    if (table.TryAddItem(item))
                    {
                        BagSystem.Instance.RemoveItem(index);

                        UpdateController();
                        if (!UI_Panel_Main.current.TrySelectedItem(controller, index))
                            TrySelectedItem();
                    }
                }
                else if (key == "table")
                {
                    var item = table.item;
                    if (BagSystem.Instance.TryAddItem(item))
                    {
                        table.RemoveItem();

                        UpdateController();
                        UI_Panel_Main.current.TrySelectedItem(controller, 3);
                    }
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
                    selectionItem = BagSystem.Instance.items[index];
                }
                else if (key == "table")
                {
                    selectionItem = item;
                }
            }
            com_itemInfo.Update(selectionItem);
        }

        public bool TrySelectedItem()
        {
            bool result = item != null;
            if (result)
                controller.SetSelection(btn_boxItem);
            return result;
        }

        private void OnClickClose()
        {
            table.TryServe();
            this.CloseSelf();
        }
    }
}