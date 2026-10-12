using System;
using FairyGUI;
using Framework;
using GameLogic;

namespace UI
{
    public struct GiftBagParam
    {
        public Action<int> OnConfirm;
    }
    public partial class UI_Panel_GiftBag: ICreate, IShow<GiftBagParam>, IHide
    {
        InputController_UI controller = new();       
        public Action<int> OnConfirm;
        private bool sentGift;
        public void OnCreate()
        {
            list_BagItems.itemRenderer = ItemRendererOption;
            controller.onClickItem = OnClikItem;
            controller.onClickClose = Hide;
            btn_close.onClick.Add(Hide);
        }
        public void OnShow(GiftBagParam param)
        {
            sentGift = false;
            UpdateBag();
            OnConfirm = param.OnConfirm;
            InputSystem.Instance.RegisterController(controller);
            controller.RegisterObjects(list_BagItems.GetChildren());
            if (GiftBagManager.Instance.gifts.Count > 0)
                controller.SetSelection(list_BagItems.GetChildAt(0));
            GiftBagManager.Instance.giftNumChanged += UpdateBag;
        }

        public void Hide()
        {
            if (!sentGift)
                OnConfirm?.Invoke(0);
            UIManager.Instance.HidePanel<UI_Panel_GiftBag>();
        }
        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            GiftBagManager.Instance.giftNumChanged -= UpdateBag;
        }

        private void ItemRendererOption(int index, GObject obj)
        {
            var ui = obj as UI_Button_GiftBagItems;
            ui.Update(GiftBagManager.Instance.gifts[index]);
        }

        private void UpdateBag()
        {
            list_BagItems.numItems = GiftBagManager.Instance.gifts.Count;
        }

        private void OnClikItem(GObject obj)
        {
            int index = list_BagItems.GetChildIndex(obj);
            if (GiftBagManager.Instance.gifts != null)
            {
                int giftId = GiftBagManager.Instance.gifts[index].id;
                GiftBagManager.Instance.SendGiftById(giftId);
                OnConfirm?.Invoke(giftId);
                sentGift = true;
                Hide();
            }
        }
    }
}