using System;
using Framework;

namespace UI
{
    public partial class UI_Panel_Anim_Moving : IShow<Action>, ISortable
    {
        int ISortable.SortingOrder => 30;

        public void OnShow(Action onNext)
        {
            AudioManager.Instance.PlaySound("Music/jiaohu/jiazai");
            anim.Play(() => onNext?.Invoke());
        }
    }
}
