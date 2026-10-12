using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Panel_BuyVegetable : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();

        public void OnCreate()
        {
            list_shelfItems.itemRenderer = ItemRendererOption;
            controller.onClickItem = OnClickItem;
            btn_close.onClick.Add(Hide);
            controller.onClickClose = Hide;
        }
        public void OnShow()
        {
            InputSystem.Instance.RegisterController(controller);          
            VegetableShopSystem.Instance.itemNumChange += UpdateStock;
            Record.current.onMoneyChanged += UpdateMoney;
            UpdateStock();
            UpdateMoney();
            controller.RegisterObjects(list_shelfItems.GetChildren());
            controller.SetSelection(list_shelfItems.GetChildAt(0));
        }
        
        public void Hide()
        {
            UIManager.Instance.HidePanel<UI_Panel_BuyVegetable>();
        }
        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            VegetableShopSystem.Instance.itemNumChange -= UpdateStock;
            Record.current.onMoneyChanged -= UpdateMoney;
        }

        public void UpdateStock()
        {
            list_shelfItems.numItems = VegetableShopSystem.Instance.shelfItems.Count;
        }

        public void ItemRendererOption(int index, GObject obj)
        {
            var ui = obj as UI_Button_Vegetables;
            ui.Update(VegetableShopSystem.Instance.shelfItems[index]);
        }

        public void OnClickItem(GObject obj)
        {
            int itemIndex = list_shelfItems.GetChildIndex(obj);
            if (VegetableShopSystem.Instance.shelfItems[itemIndex].item.count != 0 &&
                Record.current.money >= VegetableShopSystem.Instance.shelfItems[itemIndex].itemPrice)
            {
                PayPanelParam param = new()
                {
                    shelfitem = VegetableShopSystem.Instance.shelfItems[itemIndex],
                    OnConfirm = (Count) =>
                    {
                        VegetableShopSystem.Instance.BuyByIndex(itemIndex, Count);
                    }
                };
                UIManager.Instance.ShowPanelForget<UI_Panel_Pay, PayPanelParam>(param);
            }
        }
        public void UpdateMoney()
        {
            txt_money.SetVar("Money",Record.current.money.ToString()).FlushVars();
            list_shelfItems.numItems = VegetableShopSystem.Instance.shelfItems.Count;
        }
    }
}