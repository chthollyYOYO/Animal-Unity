using Framework;
using System;

namespace UI
{
    public partial class UI_Panel_VegetableShop : IShow<Action>
    {
        public void OnShow(Action onPlayCompleted)
        {
            AudioManager.Instance.PlaySound("Music/jiaohu/jindian");
            entryAnimation.Play(() => 
            {
                onPlayCompleted?.Invoke();
            });
        }
        
        public void Hide()
        {
            UIManager.Instance.HidePanel<UI_Panel_VegetableShop>();
        }
    }
}