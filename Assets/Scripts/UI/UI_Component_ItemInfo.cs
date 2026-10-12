using GameLogic;

namespace UI
{
    public partial class UI_Component_ItemInfo
    {
        public void Update(Item item)
        {
            visible = item != null;
            if (visible)
            {
                lod_icon.url = item.GetIcon();
                txt_name.text = item.GetName();
                txt_desc.text = item.GetDesc();
            }
        }
    }
}
