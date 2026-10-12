using Framework;

namespace UI
{
    public partial class UI_Component_Intent
    {
        public void Update(int style, int itemId = 0)
        {
            ctrl_style.selectedIndex = style;
            lod_icon.SetItemIcon(itemId);
        }
    }
}
