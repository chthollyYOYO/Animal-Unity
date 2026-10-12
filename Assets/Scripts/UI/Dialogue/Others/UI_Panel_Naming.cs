using Framework;
using System;

namespace UI
{
    public partial class UI_Panel_Naming : ICreate, IShow<Action>, ISortable
    {
        int ISortable.SortingOrder => 30;

        private Action onNext;

        public void OnCreate()
        {
            btn_confirm.onClick.Add(OnClickConfirm);
        }

        public void OnShow(Action onNext)
        {
            this.onNext = onNext;
        }

        private void OnClickConfirm()
        {
            if (string.IsNullOrEmpty(input_name.text))
                return;

            Record.current.name = input_name.text;
            onNext?.Invoke();
        }
    }
}
