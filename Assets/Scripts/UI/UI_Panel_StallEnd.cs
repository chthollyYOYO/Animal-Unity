using Framework;

namespace UI
{
    public partial class UI_Panel_StallEnd : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();

        public void OnCreate()
        {
            onClick.Add(OnClick);
        }

        public void OnShow()
        {
            AudioManager.Instance.PlayBgm("Music/xuzhang/BGM-jiesuan", false);
            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClick()
        {
            UIManager.Instance.HidePanel<UI_Panel_StallEnd>();
            UIManager.Instance.ShowPanelSync<UI_Panel_Thank>();
        }
    }
}
