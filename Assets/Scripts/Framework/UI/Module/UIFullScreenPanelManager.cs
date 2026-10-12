using FairyGUI;
using UI;

namespace Framework
{
    public class UIFullScreenPanelManager : Singleton<UIFullScreenPanelManager>
    {
        public void Initialize()
        {
            UIManager.Instance.OnPanelShow += OnPanelShow;
        }

        private void OnPanelShow(GComponent com)
        {
            if (GetIsFullScreen(com))
            {
                com.size = com.parent.size;
                com.AddRelation(com.parent, RelationType.Size);
            }
        }

        private bool GetIsFullScreen(GComponent com)
        {
            if (com is IFullScreen)
                return true;

            if (com is INotFullScreen)
                return false;

            if (com.width == UISystem.UI_DESIGN_WIDTH && com.height == UISystem.UI_DESIGN_HEIGHT)
                return true;

            return false;
        }
    }
}
