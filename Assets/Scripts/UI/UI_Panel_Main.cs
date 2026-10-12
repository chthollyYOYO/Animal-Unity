using FairyGUI;
using Framework;
using GameLogic;
using System.Collections.Generic;

namespace UI
{
    public partial class UI_Panel_Main : ICreate, IShow, IHide
    {
        public static UI_Panel_Main current;

        public void OnCreate()
        {
            list_bag.itemRenderer = BagItemRenderer;
            //ctrl_interactStyle.selectedIndex = 7;
        }

        public void OnShow()
        {
            Update();
            BagSystem.Instance.onBagChanged += UpdateBag;
            Record.current.onMoneyChanged += UpdateMoney;
            InteractableTipManager.Instance.onTipChanged += UpdateInteractButton;
            PlayerController.Instance.onSceneChanged += UpdateBag;
            current = this;
        }

        public void OnHide()
        {
            BagSystem.Instance.onBagChanged -= UpdateBag;
            Record.current.onMoneyChanged -= UpdateMoney;
            InteractableTipManager.Instance.onTipChanged -= UpdateInteractButton;
            PlayerController.Instance.onSceneChanged -= UpdateBag;
            current = null;
        }

        private void Update()
        {
            txt_date.SetVar("date", Record.current.date.ToString()).FlushVars();
            UpdateTime();
            txt_money.SetVar("money", Record.current.money.ToString()).FlushVars();
            UpdateBag();
            UpdateInteractButton();
        }

        private void UpdateMoney()
        {
            //考虑到上错菜只有60%金额，money为float的情况，转换成int并向上取整
            txt_money.SetVar("money", Record.current.money.ToString()).FlushVars();
        }

        private void UpdateTime()
        {
            int period = Record.current.timePeriod;
            float rotation = period switch
            {
                1 => -180,
                2 => -90,
                3 or _ => 0,
            };
            float pro = Record.current.secondsThisPeriod / ConfigManager.GetGameCfg(0).SecondsPerTime[period - 1];
            rotation += pro * 90;
            img_time.rotation = rotation;
        }

        private void UpdateBag()
        {
            list_bag.numItems = BagSystem.Instance.items.Length;
            ctrl_showBag.SetSelectedBool(PlayerController.Instance.currentScene == SceneName.经营场景);
        }

        private void BagItemRenderer(int index, GObject obj)
        {
            var item = BagSystem.Instance.GetItem(index);
            obj.asButton.icon = item?.GetIcon();

            obj.data = ("bag", index);
        }

        public List<GObject> GetBagButtons()
        {
            List<GObject> objs = new();
            for (int i = 0; i < BagSystem.Instance.items.Length; i++)
            {
                if (BagSystem.Instance.HasItem(i))
                    objs.Add(list_bag.GetChildAt(i).asCom);
            }
            return objs;
        }

        public bool TrySelectedItem(InputController_UI controller, int index)
        {
            var com = list_bag.GetNearestItem(BagSystem.Instance.items, index);
            if (com != null)
                controller.SetSelection(com);
            return com != null;
        }

        //public GComponent GetScene()
        //{
        //    return com_scene;
        //}

        //public void ChangeInteractButtonStyle(int style)
        //{
        //    ctrl_interactStyle.selectedIndex = style;
        //    if (style == 2 || style == 4)
        //        com_opLongPress.Update(0);
        //}

        public void OnLongPressInteract(float value)
        {
            com_opLongPress.Update(value);
        }

        private void UpdateInteractButton()
        {
            if (InteractableTipManager.Instance.isShowTip)
                ctrl_interactStyle.selectedIndex = InteractableTipManager.Instance.tipStyle;
            else
                ctrl_interactStyle.selectedIndex = 0;

            com_opLongPress.Update(0);
        }
    }
}
