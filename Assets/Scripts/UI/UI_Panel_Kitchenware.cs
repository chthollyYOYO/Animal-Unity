using FairyGUI;
using Framework;
using GameLogic;
using System.Collections.Generic;

namespace UI
{
    public partial class UI_Panel_Kitchenware : ICreate, IShow<InteractableObject_Kitchenware>, IHide
    {
        private InputController_UI controller = new();
        private InteractableObject_Kitchenware kitchenware;
        private Item selectionItem;

        private Item[] inItems => kitchenware.inItems;
        private Item outItem => kitchenware.outItem;

        public void OnCreate()
        {
            controller.onClickClose += OnClickClose;
            controller.onClickItem += OnClickItem;
            controller.onSelectionChanged += OnSelectionChanged;

            list_item.itemRenderer = InItemRenderer;
            btn_close.onClick.Add(OnClickClose);

            btn_item.data = ("kitchenware_out", 0);
        }

        public void OnShow(InteractableObject_Kitchenware kitchenware)
        {
            this.kitchenware = kitchenware;
            kitchenware.onItemChanged += Update;
            Update();

            InputSystem.Instance.RegisterController(controller);
            UpdateController();
            UI_Panel_Main.current.TrySelectedItem(controller, 0);

            InteractableTipManager.Instance.isShowTip = false;
        }

        public void OnHide()
        {
            kitchenware.onItemChanged -= Update;
            //kitchenware = null;

            InputSystem.Instance.UnRegisterController(controller);
            controller.SetSelection(null);

            InteractableTipManager.Instance.isShowTip = true;
        }

        private void Update()
        {
            ctrl_style.selectedIndex = kitchenware.kitchenwareId;
            list_item.numItems = 4;
            btn_item.Update(outItem);
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
                    if (kitchenware.TryAddItem(item))
                    {
                        BagSystem.Instance.RemoveItem(index);

                        UpdateController();
                        if (!UI_Panel_Main.current.TrySelectedItem(controller, index))
                            TrySelectedItem(3);
                    }
                }
                else if (key == "kitchenware_in")
                {
                    var item = kitchenware.GetInItem(index);
                    if (BagSystem.Instance.TryAddItem(item))
                    {
                        kitchenware.RemoveItem(index);

                        UpdateController();
                        if (!TrySelectedItem(index))
                            UI_Panel_Main.current.TrySelectedItem(controller, 3);
                    }
                }
                else if (key == "kitchenware_out")
                {
                    OnClickClose();
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
                else if (key == "kitchenware_in")
                {
                    selectionItem = inItems[index];
                }
                else if (key == "kitchenware_out")
                {
                    selectionItem = outItem;
                }
            }
            com_itemInfo.Update(selectionItem);
        }

        public List<GObject> GetButtons()
        {
            List<GObject> objs = new();
            for (int i = 0; i < inItems.Length; i++)
            {
                if (inItems[i] != null)
                    objs.Add(list_item.GetChildAt(i).asCom);
            }

            if (outItem != null)
                objs.Add(btn_item);

            return objs;
        }

        public bool TrySelectedItem(int index)
        {
            var com = list_item.GetNearestItem(inItems, index);
            if (com != null)
                controller.SetSelection(com);
            return com != null;
        }

        private void InItemRenderer(int index, GObject obj)
        {
            var ui = obj as UI_Button_KitchenwareItem;
            ui.Update(inItems[index]);

            obj.data = ("kitchenware_in", index);
        }

        private void OnClickClose()
        {
            if (outItem == null)
            {
                var items = kitchenware.RemoveAllItems();
                BagSystem.Instance.TryAddItem(items);
            }
            else
            {
                var qte = outItem.GetCfg().Qte;
                PlayerController.Instance.StartQte(qte, (success) =>
                {
                    kitchenware.StartCooking(/*UI_Panel_Main.current.GetScene()*/);
                });
            }

            this.CloseSelf();
        }
    }
}
