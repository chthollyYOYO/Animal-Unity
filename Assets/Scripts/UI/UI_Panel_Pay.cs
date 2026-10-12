using System;
using FairyGUI;
using Framework;
using GameLogic;
using UnityEngine;

namespace UI
{
    public struct PayPanelParam
    {
        public ShelfItem shelfitem;
        public Action<int> OnConfirm;
    }
    public partial class UI_Panel_Pay : ICreate, IShow<PayPanelParam>, IHide
    {
        private InputController_UI controller = new();

        private ShelfItem shelfItem;
        private Action<int> onConfirm;

        private int itemCount;
        private bool numChanging;
        private bool waitJRelease;
        private float timeLock;


        private bool pressing;

        private LongPressGesture add1;
        private LongPressGesture add10;
        private LongPressGesture minus1;
        private LongPressGesture minus10;

        private string txt_NumSave;
        public void OnCreate()
        {
            btn_cancel.onClick.Add(Hide);
            btn_confirm.onClick.Add(Buy);
            InitLongPress();
            InitOncePress();

            InitItemNum();
        }
        public void OnShow(PayPanelParam param)
        {
            shelfItem = param.shelfitem;
            onConfirm = param.OnConfirm;
            Update(shelfItem);
            InputSystem.Instance.RegisterController(controller);
            GameMonoSingleton.Instance.onLogicUpdate += OnLogicUpdate;
            timeLock = 0;
            waitJRelease = true;
        }

        public void OnLogicUpdate(float deltaTime)
        {
            if (waitJRelease)
            {
                if (!Input.GetKey(KeyCode.J))
                    waitJRelease = false;
                return;
            }
            InputCheck(deltaTime);
            if (intxt_itemNum.text == "\n")
                intxt_itemNum.text = txt_NumSave;
        }
        public void Update(ShelfItem shelfitem)
        {
            intxt_itemNum.maxLength = 3;
            intxt_itemNum.text = "1";
            txt_NumSave = "1";
            lod_item.url = shelfitem.item?.GetIcon();
            itemCount = 1;
        }
        public void Hide()
        {
            UIManager.Instance.HidePanel<UI_Panel_Pay>();
        }
        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            GameMonoSingleton.Instance.onLogicUpdate -= OnLogicUpdate;
        }     
        public void Buy()
        {
            if (int.TryParse(intxt_itemNum.text, out int count))
            {
                itemCount = count;
                onConfirm?.Invoke(itemCount);
                Hide();
            }
        }

        private void InitItemNum()
        {
            intxt_itemNum.maxLength = 3;
            intxt_itemNum.text = "1";
            txt_NumSave = "1";
            intxt_itemNum.onClick.Add(() =>
            {
                numChanging = true;
                intxt_itemNum.RequestFocus();
                intxt_itemNum.SetSelection(0, intxt_itemNum.text.Length);
            });
            intxt_itemNum.onChanged.Add(OnItemNumChanged);
        }
        private void InitLongPress()
        {           
            add1 = new(btn_add);
            add1.interval = 0.1f;
            add1.onBegin.Add(Add1);
            add1.onAction.Add(Add1);

            add10 = new(btn_add10);
            add10.interval = 0.1f;
            add10.onBegin.Add(Add10);
            add10.onAction.Add(Add10);

            minus1 = new(btn_minus);
            minus1.interval = 0.1f;
            minus1.onBegin.Add(Minus1);
            minus1.onAction.Add(Minus1);

            minus10 = new(btn_minus10);
            minus10.interval = 0.1f;
            minus10.onBegin.Add(Minus10);
            minus10.onAction.Add(Minus10);
        }
        private void InitOncePress()
        {
            btn_add.onClick.Add(() =>
            {
                timeLock = 0;
                Add1();
            });
            btn_minus.onClick.Add(() =>
            {
                timeLock = 0;
                Minus1();
            });
            btn_add10.onClick.Add(() =>
            {
                timeLock = 0;
                Add10();
            });
            btn_minus10.onClick.Add(() =>
            {
                timeLock = 0;
                Minus10();
            });
        }
        private void OnItemNumChanged()
        {
            if (!numChanging)
                return;

            if (int.TryParse(intxt_itemNum.text, out int value))
            {
                int itemCanBuy = Record.current.money / Mathf.CeilToInt(shelfItem.itemPrice);
                value = Mathf.Clamp(value, 1, shelfItem.item.count);
                value = Mathf.Clamp(value, 1, itemCanBuy);
                intxt_itemNum.text = value.ToString();
                txt_NumSave = value.ToString();
                numChanging = true;
            }

            if (string.IsNullOrEmpty(intxt_itemNum.text))
            {
                intxt_itemNum.text = "1";
                numChanging = true;
                return;
            }
        }

        private void InputCheck(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
                Buy();

            if (Input.GetKeyDown(KeyCode.K))
                Hide();

            if (Input.GetKey(KeyCode.A))
            {
                timeLock -= deltaTime;
                Minus10();
                pressing = true;
            }     

            if (Input.GetKey(KeyCode.D))
            {
                timeLock -= deltaTime;
                Add10();
                pressing = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                timeLock -= deltaTime;
                Minus1();
                pressing = true;
            }

            if (Input.GetKey(KeyCode.W))
            {
                timeLock -= deltaTime;
                Add1();
                pressing = true;
            }

            if (!pressing)
                timeLock = 0;

            pressing = false;
        }

        private void Add10()
        {
            if (int.TryParse(intxt_itemNum.text, out int Num) && timeLock <= 0)
            {
                numChanging = true;
                Num += 10;
                intxt_itemNum.text = Num.ToString();
                OnItemNumChanged();
                timeLock = 0.5f;
            }
        }

        private void Minus10()
        {
            if (int.TryParse(intxt_itemNum.text, out int Num) && timeLock <= 0)
            {
                numChanging = true;
                Num -= 10;
                intxt_itemNum.text = Num.ToString();
                OnItemNumChanged();
                timeLock = 0.5f;
            }
        }

        private void Add1()
        {
            if (int.TryParse(intxt_itemNum.text, out int Num) && timeLock <= 0)
            {
                numChanging = true;
                Num += 1;
                intxt_itemNum.text = Num.ToString();
                OnItemNumChanged();
                timeLock = 0.5f;
            }
        }

        private void Minus1()
        {
            if (int.TryParse(intxt_itemNum.text, out int Num) && timeLock <= 0)
            {
                numChanging = true;
                Num -= 1;
                intxt_itemNum.text = Num.ToString();
                OnItemNumChanged();
                timeLock = 0.5f;
            }
        }
    }
}
