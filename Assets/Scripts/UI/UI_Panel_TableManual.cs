using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Component_TableManual
    {
        public int tableNum => StallSystem.Instance.tables.Count;

        private InputController_UI tableManual_controller = new();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            list_item.itemRenderer = ItemRendererOption;
            tableManual_controller.onClickItem = OnClickItem;
            tableManual_controller.onClickClose = Hide;
        }

        public void Show()
        {
            visible = true;

            InputSystem.Instance.RegisterController(tableManual_controller);
            StallSystem.Instance.onTableChanged += Update;
            StallSystem.Instance.onStallingChanged += Update;
            StallSystem.Instance.onLiningChanged += Update;

            InteractableTipManager.Instance.isShowTip = false;

            Update();
            tableManual_controller.RegisterObjects(list_item.GetChildren());
            tableManual_controller.SetSelection(list_item.GetChildAt(0));
        }

        public void Hide()
        {
            visible = false;

            InputSystem.Instance.UnRegisterController(tableManual_controller);
            StallSystem.Instance.onTableChanged -= Update;
            StallSystem.Instance.onStallingChanged -= Update;
            StallSystem.Instance.onLiningChanged -= Update;

            InteractableTipManager.Instance.isShowTip = true;
        }

        public void OnClickItem(GObject gObject)
        {
            int tableId = list_item.GetChildIndex(gObject) + 1;
            //只有当顾客在收银台前，且玩家想要选择的座位为空才可以安排座位
            if (StallSystem.Instance.IsSitDownable(tableId))
            {
                //var obj = gObject as UI_Button_TableItems;
                //var controller = obj.GetController("state_customer");
                //将对应UI text变灰，表示座位已被安排，现不可选
                //controller.selectedPage = "full";
                //obj.state_customer.selectedPage = "full";
                AudioManager.Instance.PlaySound("Music/jiaohu/xuanzuo");
                StallSystem.Instance.AssignDinerTable(tableId);
            }
        }

        private void Update()
        {
            if (!StallSystem.Instance.isStalling)
            {
                Hide();
                return;
            }

            if (!StallSystem.Instance.IsPeekReady())
            {
                Hide();
                return;
            }

            list_item.numItems = tableNum;
        }

        public void ItemRendererOption(int index, GObject obj)
        {
            var opt = obj as UI_Button_TableItems;
            int tableId = index + 1;
            bool isFull = StallSystem.Instance.IsSitDownable(tableId);
            opt.Update(tableId, !isFull);
        }
    }
}