using UI;
using System;
using Framework;

namespace UI
{
    public partial class UI_Panel_Bookstore : ICreate, IShow<Action>, IHide
    {
        InputController_UI controller = new();

        public void OnCreate()
        {
            controller.onClickClose += Hide;
        }

        public void OnShow(Action onAnimCompleted)
        {
            AudioManager.Instance.PlaySound("Music/jiaohu/jindian");
            entryBookStore.Play(() =>
            {
                onAnimCompleted?.Invoke();
            });

            InputSystem.Instance.RegisterController(controller);
            UpdateMoney();
            Record.current.onMoneyChanged += UpdateMoney;
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
            Record.current.onMoneyChanged -= UpdateMoney;
        }
        
        private void Hide()
        {
            UIManager.Instance.HidePanel<UI_Panel_Bookstore>();
        }

        public void UpdateMoney()
        {
            txt_money.SetVar("Money", Record.current.money.ToString()).FlushVars();      
        }
    }
}