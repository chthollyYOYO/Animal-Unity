using FairyGUI;
using Framework;
using GameLogic;
using System.Collections.Generic;

namespace UI
{
    public partial class UI_Panel_Box : ICreate, IShow<IInteractableObject_Box>, IHide
    {
        private InputController_UI controller = new();
        private IInteractableObject_Box box;
        private Item selectionItem;

        public void OnCreate()
        {
            controller.onClickClose += OnClickClose;
            controller.onClickItem += OnClickItem;
            controller.onSelectionChanged += OnSelectionChanged;

            list_boxItem.itemRenderer = InItemRenderer;
            btn_close.onClick.Add(OnClickClose);
        }

        public void OnShow(IInteractableObject_Box box)
        {
            this.box = box;
            box.onItemChanged += Update;
            Update();

            InputSystem.Instance.RegisterController(controller);
            UpdateController();
            if (!UI_Panel_Main.current.TrySelectedItem(controller, 0))
                TrySelectedItem(0);

            InteractableTipManager.Instance.isShowTip = false;
        }

        public void OnHide()
        {
            box.onItemChanged -= Update;
            box = null;

            InputSystem.Instance.UnRegisterController(controller);
            controller.SetSelection(null);

            InteractableTipManager.Instance.isShowTip = true;
        }

        private void Update()
        {
            ctrl_style.selectedIndex = box.flagType switch
            {
                SceneFlagType.冰箱 => 0,
                SceneFlagType.饮料柜 => 1,
                SceneFlagType.保温箱 => 2,
                _ => 0
            };
            list_boxItem.numItems = box.GetLength();
            com_itemInfo.Update(selectionItem);
        }

        private void UpdateController()
        {
            controller.UnregisterAllObjects();
            controller.RegisterObjects(UI_Panel_Main.current.GetBagButtons());
            controller.RegisterObjects(GetButtons());
        }

        private void OnClickItem(GObject obj)
        {
            if (obj.data is (string key, int index))
            {
                if (key == "bag")
                {
                    var item = BagSystem.Instance.GetItem(index);
                    if (box.TryAddItem(item))
                    {
                        BagSystem.Instance.RemoveItem(index);

                        UpdateController();
                        if (!UI_Panel_Main.current.TrySelectedItem(controller, index))
                            TrySelectedItem(3);
                    }
                }
                else if (key == "box")
                {
                    var item = box.GetItemOne(index);
                    if (BagSystem.Instance.TryAddItem(item))
                    {
                        box.RemoveItem(index);

                        UpdateController();
                        if (!TrySelectedItem(index))
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
                    selectionItem = BagSystem.Instance.GetItem(index);
                }
                else if (key == "box")
                {
                    selectionItem = box.GetItem(index);
                }
            }
            com_itemInfo.Update(selectionItem);
        }

        public List<GObject> GetButtons()
        {
            List<GObject> objs = new();
            for (int i = 0; i < box.GetLength(); i++)
            {
                if (box.GetItem(i) != null)
                    objs.Add(list_boxItem.GetChildAt(i).asCom);
            }
            return objs;
        }

        public bool TrySelectedItem(int index)
        {
            var com = list_boxItem.GetNearestItem(box.items, index);
            if (com != null)
                controller.SetSelection(com);
            return com != null;
        }

        private void InItemRenderer(int index, GObject obj)
        {
            var ui = obj as UI_Button_BoxItem;
            var item = box.GetItem(index);
            ui.Update(item, box.isShowCount);

            obj.data = ("box", index);
        }

        private void OnClickClose()
        {
            this.CloseSelf();
        }
    }
}
